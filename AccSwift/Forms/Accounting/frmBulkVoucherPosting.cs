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
using Inventory.DataSet;
using Inventory.CrystalReports;
using DateManager;
using ErrorManager;
using System.IO;
using System.Data.SqlClient;
using Common;

namespace Inventory.Forms.Accounting
{
    public partial class frmbulkvoucherposting : Form, IfrmDateConverter
    {
        SourceGrid.Cells.Button btnRowDelete = new SourceGrid.Cells.Button("");
        SourceGrid.Cells.Controllers.CustomEvents evtDelete = new SourceGrid.Cells.Controllers.CustomEvents();
        SourceGrid.Cells.Controllers.CustomEvents evtcboDrCrSelectedIndexChanged = new SourceGrid.Cells.Controllers.CustomEvents();
        SourceGrid.Cells.Controllers.CustomEvents evtAmountFocusLost = new SourceGrid.Cells.Controllers.CustomEvents();
        ListItem liCatID = new ListItem();
        VoucherConfiguration m_VouConfig = new VoucherConfiguration();
        DataTable dtaccclass = new DataTable();
        private int OnlyReqdDetailRows = 0;
        private int sno=1;
        //private int serieid ;
        //private string NumberingType;
        public frmbulkvoucherposting()
        {
            InitializeComponent();
        }

        private void btnDate_Click(object sender, EventArgs e)
        {
            frmDateConverter frm = new frmDateConverter(this, Date.ToDotNet(txtDate.Text));
            frm.ShowDialog();
        }
        public void DateConvert(DateTime DotNetDate)
        {
            txtDate.Text = Date.ToSystem(DotNetDate);
        }

        private void frmPayroll_Load(object sender, EventArgs e)
        {
            ListCategory(cboCategoryName);
            txtDate.Mask = Date.FormatToMask();
            txtDate.Text = Date.ToSystem(Date.GetServerDate());
            //Event trigerred when delete button is clicked
            evtDelete.Click += new EventHandler(Delete_Row_Click);      
            grdpayroll.Redim(2, 11);
            //btnRowDelete.Image = global::AccSwift.Properties.Resources.gnome_window_close;
            //Prepare the header part for grid
            AddGridHeader();
            AddRowParyroll(1);
        }
        private void AddGridHeader()
        {
            grdpayroll[0, 0] = new MyHeader("Delete");
            grdpayroll[0, 1] = new MyHeader("Particulars/Accounting Head");
            grdpayroll[0, 2] = new MyHeader("Dr/Cr");
            grdpayroll[0, 3] = new MyHeader("Amount");
            grdpayroll[0, 4] = new MyHeader("Remarks");
            grdpayroll[0, 5] = new MyHeader("LedgerID");
            grdpayroll[0, 6] = new MyHeader("VoucherType");
            grdpayroll[0, 7] = new MyHeader("VoucherNumber");
            grdpayroll[0, 8] = new MyHeader("sid");
            grdpayroll[0, 9] = new MyHeader("pid");
            grdpayroll[0, 10] = new MyHeader("rowid");


            grdpayroll[0, 0].Column.Width = 50;
            grdpayroll[0, 1].Column.Width = 200;
            grdpayroll[0, 2].Column.Width = 50;
            grdpayroll[0, 3].Column.Width = 100;
            grdpayroll[0, 4].Column.Width = 200;
            grdpayroll[0, 5].Column.Width = 50;
            grdpayroll[0, 6].Column.Width = 50;
            grdpayroll[0, 7].Column.Width = 50;
            grdpayroll[0, 8].Column.Width = 50;
            grdpayroll[0, 9].Column.Width = 50;
            grdpayroll[0, 5].Column.Visible = false;
            grdpayroll[0, 6].Column.Visible = false;
            grdpayroll[0, 7].Column.Visible = false;
            grdpayroll[0, 8].Column.Visible = false;
            grdpayroll[0, 9].Column.Visible = false;
            grdpayroll[0, 10].Column.Visible = false;
        }
        private class MyHeader : SourceGrid.Cells.ColumnHeader
        {
            public MyHeader(object value)
                : base(value)
            {
                //1 Header Row
                SourceGrid.Cells.Views.ColumnHeader view = new SourceGrid.Cells.Views.ColumnHeader();
                view.Font = new Font("Arial", 9, FontStyle.Bold);
                view.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleCenter;
                View = view;

                AutomaticSortEnabled = false;
            }
        }
        private void AddRowParyroll(int RowCount)
        {
            //Add a new row
            grdpayroll.Redim(Convert.ToInt32(RowCount + 1), grdpayroll.ColumnsCount);
            SourceGrid.Cells.Button btnDelete = new SourceGrid.Cells.Button("");
            btnDelete.Image = global::Inventory.Properties.Resources.gnome_window_close;
            //btnDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            int i = RowCount;

            grdpayroll[i, 0] = btnDelete;
            grdpayroll[i, 0].AddController(evtDelete);

            SourceGrid.Cells.Editors.TextBox txtparticulars=new SourceGrid.Cells.Editors.TextBox(typeof(string));
            txtparticulars.EditableMode = SourceGrid.EditableMode.Focus | SourceGrid.EditableMode.AnyKey;
            grdpayroll[i, 1] = new SourceGrid.Cells.Cell("",txtparticulars);
            grdpayroll[i, 1].Value = "";

            SourceGrid.Cells.Editors.ComboBox cboDrCr = new SourceGrid.Cells.Editors.ComboBox(typeof(string));
            cboDrCr.StandardValues = new string[] { "Debit", "Credit" };
            cboDrCr.Control.DropDownStyle = ComboBoxStyle.DropDownList;
            cboDrCr.EditableMode = SourceGrid.EditableMode.Focus;
            string strDrCr = "Debit";
            if (grdpayroll[i - 1, 3].Value.ToString() == "Debit")
                strDrCr = "Credit";
            grdpayroll[i, 2] = new SourceGrid.Cells.Cell(strDrCr, cboDrCr);
            grdpayroll[i, 2].AddController(evtcboDrCrSelectedIndexChanged);

            SourceGrid.Cells.Editors.TextBox txtAmount = new SourceGrid.Cells.Editors.TextBox(typeof(string));
            txtAmount.EditableMode = SourceGrid.EditableMode.Focus;
            grdpayroll[i, 3] = new SourceGrid.Cells.Cell("", txtAmount);
            grdpayroll[i, 3].AddController(evtAmountFocusLost);

            SourceGrid.Cells.Editors.TextBox txtRemarks = new SourceGrid.Cells.Editors.TextBox(typeof(string));
            txtRemarks.EditableMode = SourceGrid.EditableMode.Focus;
            grdpayroll[i, 4] = new SourceGrid.Cells.Cell("", txtRemarks);

            SourceGrid.Cells.Editors.TextBox txtLedgerID = new SourceGrid.Cells.Editors.TextBox(typeof(string));
            txtLedgerID.EditableMode = SourceGrid.EditableMode.None;
            grdpayroll[i, 5] = new SourceGrid.Cells.Cell("", txtLedgerID);
            grdpayroll[i, 5].Value = "";

            SourceGrid.Cells.Editors.TextBox txtVoucherType = new SourceGrid.Cells.Editors.TextBox(typeof(string));
            txtVoucherType.EditableMode = SourceGrid.EditableMode.None;
            grdpayroll[i, 6] = new SourceGrid.Cells.Cell("", txtVoucherType);
            grdpayroll[i, 6].Value = "";

            SourceGrid.Cells.Editors.TextBox txtvoucherno = new SourceGrid.Cells.Editors.TextBox(typeof(string));
            txtvoucherno.EditableMode = SourceGrid.EditableMode.None;
            grdpayroll[i, 7] = new SourceGrid.Cells.Cell("", txtvoucherno);
            grdpayroll[i, 7].Value = "";

            SourceGrid.Cells.Editors.TextBox txtsid = new SourceGrid.Cells.Editors.TextBox(typeof(string));
            txtsid.EditableMode = SourceGrid.EditableMode.None;
            grdpayroll[i, 8] = new SourceGrid.Cells.Cell("", txtsid);
            grdpayroll[i, 8].Value = "";

            SourceGrid.Cells.Editors.TextBox txtpid = new SourceGrid.Cells.Editors.TextBox(typeof(string));
            txtpid.EditableMode = SourceGrid.EditableMode.None;
            grdpayroll[i, 9] = new SourceGrid.Cells.Cell("", txtpid);
            grdpayroll[i, 9].Value = "";

            SourceGrid.Cells.Editors.TextBox txtrowid = new SourceGrid.Cells.Editors.TextBox(typeof(string));
            txtrowid.EditableMode = SourceGrid.EditableMode.None;
            grdpayroll[i, 10] = new SourceGrid.Cells.Cell("", txtrowid);
            grdpayroll[i, 10].Value = "";

        }
        private void Delete_Row_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete the row?", "Question", MessageBoxButtons.YesNo) == DialogResult.No)
                return;
            SourceGrid.CellContext ctx = (SourceGrid.CellContext)sender;
            //Do not delete if its the last Row because it contains (NEW)
            if (ctx.Position.Row <= grdpayroll.RowsCount - 2)
                grdpayroll.Rows.Remove(ctx.Position.Row);
        }
        private void ListCategory(ComboBox ComboBoxControl)
        {
            BusinessLogic.Accounting.BulkPosting bp = new BusinessLogic.Accounting.BulkPosting();
            ComboBoxControl.Items.Clear();
            DataTable dt=bp.GetBulkMasterinfo();
            ComboBoxControl.Items.Add(new ListItem((0), "None"));
            for (int i = 0; i < dt.Rows.Count;i++ )
            {
                DataRow dr = dt.Rows[i];
                ComboBoxControl.Items.Add(new ListItem((int)dr["id"],dr["Name"].ToString()));     
            }
            ComboBoxControl.SelectedIndex = 0;
            ComboBoxControl.DisplayMember = "value";
            ComboBoxControl.ValueMember = "id";
        }

        private void cboCategoryName_SelectedIndexChanged(object sender, EventArgs e)
        {
            //ListItem liLedgerID = new ListItem();
            //liLedgerID = (ListItem)comboBankAccount.SelectedItem;
            //int catid = (ListItem)cboCategoryName.SelectedItem;
            //int catid =Convert.ToInt32( cboCategoryName.ValueMember.ToString());
           // cleargrid();
            liCatID = (ListItem)cboCategoryName.SelectedItem;
            int catid = liCatID.ID;
            if (catid != 0)
            {
                FillGrid(catid);
            }
            //else
            //{
            //    cleargrid();
            //}
          
        }
        private void FillGrid(int catid)
        {
            DataTable dt;
            BusinessLogic.Accounting.BulkPosting SBP = new BusinessLogic.Accounting.BulkPosting();
            dt=SBP.GetAllDataFromBulk(catid);
            foreach(DataRow dr in dt.Rows)
            {
                WriteTransaction(dr["Particulars"].ToString(), dr["DrCr"].ToString(), Convert.ToDouble(dr["Amount"].ToString()), Convert.ToInt32(dr["LedgerID"].ToString()), dr["VoucherType"].ToString(), dr["VoucherNumber"].ToString(), dr["remarks"].ToString(), Convert.ToInt32(dr["SeriesID"].ToString()), Convert.ToInt32(dr["ProjectID"].ToString()), Convert.ToInt32(dr["RowID"].ToString()));
            }
        }
        private void WriteTransaction(string particulars,string drcr,double amount,int ledgerid,string vtype,string vnumber,string remarks,int sid,int pid,int rowid)
        {
            grdpayroll.Rows.Insert(sno);
            SourceGrid.Cells.Button btnDelete = new SourceGrid.Cells.Button("");
            btnDelete.Image = global::Inventory.Properties.Resources.gnome_window_close;
            grdpayroll[sno, 0] = btnDelete;
            grdpayroll[sno, 0].AddController(evtDelete);

            SourceGrid.Cells.Editors.TextBox txtparticulars = new SourceGrid.Cells.Editors.TextBox(typeof(string));
            txtparticulars.EditableMode = SourceGrid.EditableMode.Focus | SourceGrid.EditableMode.AnyKey;
            grdpayroll[sno, 1] = new SourceGrid.Cells.Cell("", txtparticulars);
            grdpayroll[sno, 1].Value = particulars;
           // grdpayroll[sno, 1] = new SourceGrid.Cells.Cell(particulars);
            SourceGrid.Cells.Editors.ComboBox cboDrCr = new SourceGrid.Cells.Editors.ComboBox(typeof(string));
            cboDrCr.StandardValues = new string[] { "Debit", "Credit" };
            cboDrCr.Control.DropDownStyle = ComboBoxStyle.DropDownList;
            cboDrCr.EditableMode = SourceGrid.EditableMode.Focus;
            string strDrCr = "Debit";
            if (grdpayroll[sno - 1, 3].Value.ToString() == "Debit")
                strDrCr = "Credit";
            grdpayroll[sno, 2] = new SourceGrid.Cells.Cell(strDrCr, cboDrCr);
            grdpayroll[sno, 2].Value = drcr;
            grdpayroll[sno, 2].AddController(evtcboDrCrSelectedIndexChanged);
            //grdpayroll[sno, 2] = new SourceGrid.Cells.Cell(drcr);
            SourceGrid.Cells.Editors.TextBox txtAmount = new SourceGrid.Cells.Editors.TextBox(typeof(string));
            txtAmount.EditableMode = SourceGrid.EditableMode.Focus;
            grdpayroll[sno, 3] = new SourceGrid.Cells.Cell("", txtAmount);
            grdpayroll[sno, 3].AddController(evtAmountFocusLost);
            grdpayroll[sno, 3].Value = amount;
            //grdpayroll[sno, 3] = new SourceGrid.Cells.Cell(amount);
         
            SourceGrid.Cells.Editors.TextBox txtRemarks = new SourceGrid.Cells.Editors.TextBox(typeof(string));
            txtRemarks.EditableMode = SourceGrid.EditableMode.Focus;
            grdpayroll[sno, 4] = new SourceGrid.Cells.Cell("", txtRemarks);
            try
            {
                grdpayroll[sno, 4].Value = remarks;
                //grdpayroll[sno, 4] = new SourceGrid.Cells.Cell(remarks);
            }
            catch
            {
                grdpayroll[sno, 4].Value = "";
                //grdpayroll[sno, 4] = new SourceGrid.Cells.Cell(" ");
            }
            SourceGrid.Cells.Editors.TextBox txtLedgerID = new SourceGrid.Cells.Editors.TextBox(typeof(string));
            txtLedgerID.EditableMode = SourceGrid.EditableMode.None;
            grdpayroll[sno, 5] = new SourceGrid.Cells.Cell("", txtLedgerID);
            grdpayroll[sno, 5].Value = ledgerid;
            //grdpayroll[sno, 5] = new SourceGrid.Cells.Cell(ledgerid);

            SourceGrid.Cells.Editors.TextBox txtVoucherType = new SourceGrid.Cells.Editors.TextBox(typeof(string));
            txtVoucherType.EditableMode = SourceGrid.EditableMode.None;
            grdpayroll[sno, 6] = new SourceGrid.Cells.Cell("", txtVoucherType);
            grdpayroll[sno, 6].Value = vtype;
           // grdpayroll[sno, 6] = new SourceGrid.Cells.Cell(vtype);
            SourceGrid.Cells.Editors.TextBox txtvoucherno = new SourceGrid.Cells.Editors.TextBox(typeof(string));
            txtvoucherno.EditableMode = SourceGrid.EditableMode.None;
            grdpayroll[sno, 7] = new SourceGrid.Cells.Cell("", txtvoucherno);
            grdpayroll[sno, 7].Value = vnumber;
            //grdpayroll[sno, 7] = new SourceGrid.Cells.Cell(vnumber);
            SourceGrid.Cells.Editors.TextBox txtsid = new SourceGrid.Cells.Editors.TextBox(typeof(string));
            txtsid.EditableMode = SourceGrid.EditableMode.None;
            grdpayroll[sno, 8] = new SourceGrid.Cells.Cell("", txtsid);
            grdpayroll[sno, 8].Value = sid;
            // grdpayroll[sno, 8] = new SourceGrid.Cells.Cell(vtype);
            SourceGrid.Cells.Editors.TextBox txtpid = new SourceGrid.Cells.Editors.TextBox(typeof(string));
            txtpid.EditableMode = SourceGrid.EditableMode.None;
            grdpayroll[sno, 9] = new SourceGrid.Cells.Cell("", txtpid);
            grdpayroll[sno, 9].Value = pid;
            //grdpayroll[sno, 9] = new SourceGrid.Cells.Cell(vnumber);

            SourceGrid.Cells.Editors.TextBox txtrowid = new SourceGrid.Cells.Editors.TextBox(typeof(string));
            txtrowid.EditableMode = SourceGrid.EditableMode.None;
            grdpayroll[sno, 10] = new SourceGrid.Cells.Cell("", txtrowid);
            grdpayroll[sno, 10].Value = rowid;

            sno++;
        }
        private void cleargrid()
        {
            clearall();
            grdpayroll.Redim(2, 8);
            AddGridHeader();
            AddRowParyroll(1); 
        }
        private void clearall()
        {
            grdpayroll.Rows.Clear();
            cboCategoryName.SelectedIndex = 0;
        }

        private void btnbulkposting_Click(object sender, EventArgs e)
        {
            BusinessLogic.Accounting.BulkPosting bpid = new BusinessLogic.Accounting.BulkPosting();
            DataTable dtvouchertype = new DataTable();
            //int CurRow = grdpayroll.Selection.GetSelectionRegion().GetRowsIndex()[0];
            //SourceGrid.CellContext CRowID = new SourceGrid.CellContext(grdpayroll,new SourceGrid.Position(CurRow,10));
            //int rowid = Convert.ToInt32(CRowID.Value.ToString());

            liCatID = (ListItem)cboCategoryName.SelectedItem;
            int catid = liCatID.ID;
            int rowid = bpid.GetRowID(catid);
            txtVchNo.Enabled = true;
            DataTable dt = new DataTable();
            dtvouchertype = bpid.GetVoucherType(catid);
            for (int j = 0; j < dtvouchertype.Rows.Count; j++)
            {
                DataRow drvoucher = dtvouchertype.Rows[j];
                switch (drvoucher["VoucherType"].ToString())
                {
                    case "BANK_PMNT":
                      dt = bpid.GetSeriesID(catid, "BANK_PMNT");
                        DataRow drbankpayment = dt.Rows[0];
                        int bankpaymentseriesid = Convert.ToInt32(drbankpayment["SeriesID"].ToString());
                        string bankpaymentNumberingType = m_VouConfig.GetVouNumberingType(bankpaymentseriesid);
                        if (bankpaymentNumberingType == "AUTOMATIC")
                        {
                            object m_vounum = m_VouConfig.GenerateVouNumType(bankpaymentseriesid);
                            if (m_vounum == null)
                            {
                                MessageBox.Show("Your voucher numbers are totally finished!");
                                return;
                            }
                            txtVchNo.Text = m_vounum.ToString();
                            txtVchNo.Enabled = false;
                        }
                        dtaccclass = bpid.GetAccClassIDs(rowid, "BANK_PMNT");
                        string BankPaymentXMLString = ReadAllBankPaymentEntry(catid, drvoucher["VoucherType"].ToString());
                        if (BankPaymentXMLString == string.Empty)
                        {
                            MessageBox.Show("Unable to cast journal entry to XML!");
                            return;
                        }
                        #region Save xml data to Database
                        using (System.IO.StringWriter swStringWriter = new StringWriter())
                        {
                            using (SqlCommand dbCommand = new SqlCommand("Acc.xmlBankPaymentInsert", Global.m_db.cn))
                            {
                                // we are going to use store procedure  
                                dbCommand.CommandType = CommandType.StoredProcedure;
                                // Add input parameter and set its properties.
                                SqlParameter parameter = new SqlParameter();
                                // Store procedure parameter name  
                                parameter.ParameterName = "@bankpayment";
                                // Parameter type as XML 
                                parameter.DbType = DbType.Xml;
                                parameter.Direction = ParameterDirection.Input; // Input Parameter  
                                parameter.Value = BankPaymentXMLString; // XML string as parameter value  
                                // Add the parameter in Parameters collection.
                                dbCommand.Parameters.Add(parameter);
                               //Global.m_db.cn.Open();
                                int intRetValue = dbCommand.ExecuteNonQuery();
                            }
                        }
                        #endregion
                        break;

                    case "BANK_RCPT":
                        dt = bpid.GetSeriesID(catid, "BANK_RCPT");
                        DataRow drbankreceipt = dt.Rows[0];
                        int bankreceiptseriesid = Convert.ToInt32(drbankreceipt["SeriesID"].ToString());
                        string bankreceiptNumberingType = m_VouConfig.GetVouNumberingType(bankreceiptseriesid);
                        if (bankreceiptNumberingType == "AUTOMATIC")
                        {
                            object m_vounum = m_VouConfig.GenerateVouNumType(bankreceiptseriesid);
                            if (m_vounum == null)
                            {
                                MessageBox.Show("Your voucher numbers are totally finished!");
                                return;
                            }
                            txtVchNo.Text = m_vounum.ToString();
                            txtVchNo.Enabled = false;
                        }
                        dtaccclass = bpid.GetAccClassIDs(rowid, "BANK_RCPT");
                        string BankReceiptXMLString = ReadAllBankReceiptEntry(catid, drvoucher["VoucherType"].ToString());
                        if (BankReceiptXMLString == string.Empty)
                        {
                            MessageBox.Show("Unable to cast journal entry to XML!");
                            return;
                        }
                        #region Save xml data to Database
                        using (System.IO.StringWriter swStringWriter = new StringWriter())
                        {
                            using (SqlCommand dbCommand = new SqlCommand("Acc.xmlBankReceiptInsert", Global.m_db.cn))
                            {
                                // we are going to use store procedure  
                                dbCommand.CommandType = CommandType.StoredProcedure;
                                // Add input parameter and set its properties.
                                SqlParameter parameter = new SqlParameter();
                                // Store procedure parameter name  
                                parameter.ParameterName = "@bankreceipt";
                                // Parameter type as XML 
                                parameter.DbType = DbType.Xml;
                                parameter.Direction = ParameterDirection.Input; // Input Parameter  
                                parameter.Value = BankReceiptXMLString; // XML string as parameter value  
                                // Add the parameter in Parameters collection.
                                dbCommand.Parameters.Add(parameter);
                               // Global.m_db.cn.Open();
                                int intRetValue = dbCommand.ExecuteNonQuery();
                                //MessageBox.Show(intRetValue.ToString());                                
                            }
                        }
                        #endregion
                        break;

                    case "CASH_PMNT":
                        dt = bpid.GetSeriesID(catid, "CASH_PMNT");
                        DataRow drcashpayment = dt.Rows[0];
                        int  cashpaymentseriesid = Convert.ToInt32(drcashpayment["SeriesID"].ToString());
                        string cashpaymentNumberingType = m_VouConfig.GetVouNumberingType(cashpaymentseriesid);
                        if (cashpaymentNumberingType == "AUTOMATIC")
                        {
                            object m_vounum = m_VouConfig.GenerateVouNumType(cashpaymentseriesid);
                            if (m_vounum == null)
                            {
                                MessageBox.Show("Your voucher numbers are totally finished!");
                                return;
                            }
                            txtVchNo.Text = m_vounum.ToString();
                            txtVchNo.Enabled = false;
                        }
                        dtaccclass = bpid.GetAccClassIDs(rowid, "CASH_PMNT");
                        string CashPaymentXMLString = ReadAllJournalEntryforCashpayment(catid, drvoucher["VoucherType"].ToString());
                        if (CashPaymentXMLString == string.Empty)
                        {
                            MessageBox.Show("Unable to cast journal entry to XML!");
                            return;
                        }
                        #region Save xml data to Database
                        using (System.IO.StringWriter swStringWriter = new StringWriter())
                        {
                            using (SqlCommand dbCommand = new SqlCommand("Acc.xmlCashPaymentInsert", Global.m_db.cn))
                            {
                                // we are going to use store procedure  
                                dbCommand.CommandType = CommandType.StoredProcedure;
                                // Add input parameter and set its properties.
                                SqlParameter parameter = new SqlParameter();
                                // Store procedure parameter name  
                                parameter.ParameterName = "@cashpayment";
                                // Parameter type as XML 
                                parameter.DbType = DbType.Xml;
                                parameter.Direction = ParameterDirection.Input; // Input Parameter  
                                parameter.Value = CashPaymentXMLString; // XML string as parameter value  
                                // Add the parameter in Parameters collection.
                                dbCommand.Parameters.Add(parameter);
                                  //Global.m_db.cn.Open();
                                int intRetValue = dbCommand.ExecuteNonQuery();
                                //MessageBox.Show(intRetValue.ToString());                                
                            }
                        }
                        #endregion
                        break;

                    case "CASH_RCPT":
                        dt = bpid.GetSeriesID(catid, "CASH_RCPT");
                        DataRow drcashreceipt = dt.Rows[0];
                        int cashreceiptseriesid = Convert.ToInt32(drcashreceipt["SeriesID"].ToString());
                        string cashreceiptNumberingType = m_VouConfig.GetVouNumberingType(cashreceiptseriesid);
                        if (cashreceiptNumberingType == "AUTOMATIC")
                        {
                            object m_vounum = m_VouConfig.GenerateVouNumType(cashreceiptseriesid);
                            if (m_vounum == null)
                            {
                                MessageBox.Show("Your voucher numbers are totally finished!");
                                return;
                            }
                            txtVchNo.Text = m_vounum.ToString();
                            txtVchNo.Enabled = false;
                        }
                        dtaccclass = bpid.GetAccClassIDs(rowid, "CASH_PMNT");
                        string CashReceiptXMLString = ReadAllCashReceiptEntry(catid, drvoucher["VoucherType"].ToString());
                        //    string CashReceiptXMLString = ReadAllCashReceiptEntry();
                        if (CashReceiptXMLString == string.Empty)
                        {
                            MessageBox.Show("Unable to cast cash receipt entry to XML!");
                            return;
                        }    
                        #region Save xml data to Database
                        using (System.IO.StringWriter swStringWriter = new StringWriter())
                        {
                            using (SqlCommand dbCommand = new SqlCommand("Acc.xmlCashReceiptInsert", Global.m_db.cn))
                            {
                                // we are going to use store procedure  
                                dbCommand.CommandType = CommandType.StoredProcedure;
                                // Add input parameter and set its properties.
                                SqlParameter parameter = new SqlParameter();
                                // Store procedure parameter name  
                                parameter.ParameterName = "@cashreceipt";
                                // Parameter type as XML 
                                parameter.DbType = DbType.Xml;
                                parameter.Direction = ParameterDirection.Input; // Input Parameter  
                                parameter.Value = CashReceiptXMLString; // XML string as parameter value  
                                // Add the parameter in Parameters collection.
                                dbCommand.Parameters.Add(parameter);
                                //Global.m_db.cn.Open();
                                int intRetValue = dbCommand.ExecuteNonQuery();
                                //MessageBox.Show(intRetValue.ToString());                                
                            }
                        }
                        #endregion
                        break;

                    case "JRNL":
                        dt = bpid.GetSeriesID(catid, "JRNL");
                        DataRow dr = dt.Rows[0];
                        int serieid = Convert.ToInt32(dr["SeriesID"].ToString());
                        string NumberingType = m_VouConfig.GetVouNumberingType(serieid);
                        if (NumberingType == "AUTOMATIC")
                        {
                            object m_vounum = m_VouConfig.GenerateVouNumType(serieid);
                            if (m_vounum == null)
                            {
                                MessageBox.Show("Your voucher numbers are totally finished!");
                                return;
                            }
                            txtVchNo.Text = m_vounum.ToString();
                            txtVchNo.Enabled = false;
                        }
                        dtaccclass = bpid.GetAccClassIDs(rowid, "JRNL");
                        string JournalXMLString = ReadAllJournalEntry();
                        if (JournalXMLString == string.Empty)
                        {
                            MessageBox.Show("Unable to cast BulkVoucher entry to XML!");
                            return;
                        }
                        #region Save xml data to Database
                        using (System.IO.StringWriter swStringWriter = new StringWriter())
                        {
                            using (SqlCommand dbCommand = new SqlCommand("Acc.xmlJournalInsert", Global.m_db.cn))
                            {
                                // we are going to use store procedure  
                                dbCommand.CommandType = CommandType.StoredProcedure;
                                // Add input parameter and set its properties.
                                SqlParameter parameter = new SqlParameter();
                                // Store procedure parameter name  
                                parameter.ParameterName = "@journal";
                                // Parameter type as XML 
                                parameter.DbType = DbType.Xml;
                                parameter.Direction = ParameterDirection.Input; // Input Parameter  
                                parameter.Value = JournalXMLString; // XML string as parameter value  
                                // Add the parameter in Parameters collection.
                                dbCommand.Parameters.Add(parameter);
                                Global.m_db.cn.Open();
                                int intRetValue = dbCommand.ExecuteNonQuery();
                                //MessageBox.Show(intRetValue.ToString());                                
                            }
                        }
                        #endregion
                        break;
                }

            }
            
            Global.Msg("Bulk Voucher  created successfully!");


        }

        private void btnSave_Click(object sender, EventArgs e)
        {
           liCatID = (ListItem)cboCategoryName.SelectedItem;
           int catid = liCatID.ID;
           string s = grdpayroll[0 + 1, 8].Value.ToString();
          DataTable SaveCurrentState=new DataTable();
         // SaveCurrentState.Columns.Add("bulkpostingmasterid");
          SaveCurrentState.Columns.Add("particulars");
          SaveCurrentState.Columns.Add("drcr");
          SaveCurrentState.Columns.Add("amount");
          SaveCurrentState.Columns.Add("ledgerid");
          SaveCurrentState.Columns.Add("vouchertype");
          SaveCurrentState.Columns.Add("vouchernumber");
          SaveCurrentState.Columns.Add("remarks");
          SaveCurrentState.Columns.Add("sid");
          SaveCurrentState.Columns.Add("pid");
          SaveCurrentState.Columns.Add("rowid");
          for (int i = 0; i < grdpayroll.Rows.Count - 2;i++ )
          {
              SaveCurrentState.Rows.Add(grdpayroll[i + 1, 1].Value, grdpayroll[i + 1, 2].Value, grdpayroll[i + 1, 3].Value, grdpayroll[i + 1, 5].Value, grdpayroll[i + 1, 6].Value, grdpayroll[i + 1, 7].Value, grdpayroll[i + 1, 4].Value, grdpayroll[i + 1, 8].Value, grdpayroll[i + 1, 9].Value, grdpayroll[i + 1, 10].Value);
          }
          BusinessLogic.Accounting.BulkPosting CBP = new BusinessLogic.Accounting.BulkPosting();
          CBP.ChangeBulkPostingInfo(SaveCurrentState,catid);
          Global.Msg("DATA SAVED SUCCESSFULLY");
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();

        }

        private string ReadAllJournalEntry()
        {
            System.Text.Encoding AEncoder = System.Text.Encoding.Unicode;
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            System.Xml.XmlTextWriter tw = new System.Xml.XmlTextWriter(ms, AEncoder);
           // int SeriesID =Convert.ToInt32(grdpayroll[1, 8].Value.ToString());
            int liProjectID = Convert.ToInt32(grdpayroll[1, 9].Value.ToString());

            //validate grid entry
            if (!ValidateGrid())
                return string.Empty;  

            tw.WriteStartDocument();
            #region  Journal
            tw.WriteStartElement("JOURNAL");
            {
                ///For Journal Master Section
                ///int JournalID = System.Convert.ToInt32(9); // Auto increment  
                int SeriesID = Convert.ToInt32(grdpayroll[1, 8].Value.ToString());
                //int SID = System.Convert.ToInt32(SeriesID.ID);
                string Voucher_No = System.Convert.ToString(txtVchNo.Text);
                DateTime Journal_Date = Date.ToDotNet(txtDate.Text);
               // string Remarks = System.Convert.ToString(txtRemarks.Text);
                string Remarks = "Bulk Posting Info";
                //int ProjectID = System.Convert.ToInt32(liProjectID.ID);
                int ProjectID = Convert.ToInt32(grdpayroll[1, 9].Value.ToString());
                DateTime Created_Date = System.Convert.ToDateTime(DateTime.Now);
                string Created_By = System.Convert.ToString("Admin");
                DateTime Modified_Date = System.Convert.ToDateTime(DateTime.Now);
                string Modified_By = System.Convert.ToString("Admin");
                tw.WriteStartElement("JOURNALMASTER");
                tw.WriteElementString("SeriesID", SeriesID.ToString());
                tw.WriteElementString("Voucher_No", Voucher_No.ToString());
                tw.WriteElementString("Journal_Date", Date.ToDB(Journal_Date));
                tw.WriteElementString("Remarks", Remarks.ToString());
                tw.WriteElementString("ProjectID", ProjectID.ToString());
                tw.WriteElementString("Created_Date", Date.ToDB(Created_Date));
                tw.WriteElementString("Created_By", Created_By.ToString());
                tw.WriteElementString("Modified_Date", Date.ToDB(Modified_Date));
                tw.WriteElementString("Modified_By", Modified_By.ToString());
                tw.WriteEndElement();
                ///For journal Detail Section             
                int JournalID = 0;
                int LedgerID = 0;
                Decimal Amount = 0;
                string DrCr = "";
                string RemarksDetail = "";
                tw.WriteStartElement("JOURNALDETAIL");
                for (int i = 0; i < OnlyReqdDetailRows; i++)
                {
                    //JournalID = System.Convert.ToInt32(288);
                   // LedgerID = System.Convert.ToInt32(grdJournal[i + 1, 7].Value);
                    LedgerID = System.Convert.ToInt32(grdpayroll[i + 1, 5].Value);
                    Amount = System.Convert.ToDecimal(grdpayroll[i + 1, 3].Value);
                    DrCr = System.Convert.ToString(grdpayroll[i + 1, 2].Value);
                    RemarksDetail = System.Convert.ToString(grdpayroll[i + 1, 4].Value);
                    tw.WriteStartElement("DETAIL");
                    tw.WriteElementString("JournalID", JournalID.ToString());
                    tw.WriteElementString("LedgerID", LedgerID.ToString());
                    tw.WriteElementString("Amount", Amount.ToString());
                    tw.WriteElementString("DrCr", DrCr.ToString());
                    tw.WriteElementString("Remarks", RemarksDetail.ToString());
                    tw.WriteEndElement();
                }
                tw.WriteEndElement();
                //Write Checked Accounting class ID
                try
                {
                    //ArrayList arrNode = treeAccClass.GetCheckedNodes(true);
                    //tw.WriteStartElement("ACCCLASSIDS");
                    //foreach (string tag in arrNode)
                    //{
                    //    AccClassID.Add(Convert.ToInt32(tag));
                    //    tw.WriteElementString("AccID", Convert.ToInt32(tag).ToString());
                    //}
                    tw.WriteStartElement("ACCCLASSIDS");
                    for (int i = 0; i < dtaccclass.Rows.Count; i++ )
                    {
                        DataRow dr=dtaccclass.Rows[i];
                        //AccClassID.Add(Convert.ToInt32(tag));
                        tw.WriteElementString("AccID",dr["AccClassID"].ToString());
                    }

                    tw.WriteEndElement();
                }
                catch
                { }

            }
            tw.WriteFullEndElement();
            #endregion
            tw.WriteEndDocument();
            tw.Flush();
            tw.Close();
            string strXML = AEncoder.GetString(ms.ToArray());
            // MessageBox.Show(strXML);       
            return strXML;
        }

        //It Validates all the entry in the grid Only valid rows are count and validate
        private bool ValidateGrid()
        {
            int[] LdrID = new int[20];
            decimal[] Amt = new decimal[20];

            //Validate input grid record
            for (int i = 0; i < grdpayroll.Rows.Count - 1; i++)
            {
                try
                {
                    //if ledger ID repeats then message it
                    // if LedgerID is not present in between them
                    int tempValue = 0;
                    decimal tempDecValue = 0;
                    try
                    {
                        tempValue = System.Convert.ToInt32(grdpayroll[i + 1, 5].Value);
                    }
                    catch (Exception ex)
                    {
                        tempValue = 0;
                    }
                    try
                    {
                        tempDecValue = System.Convert.ToDecimal(grdpayroll[i + 1, 3].Value);
                    }
                    catch (Exception ex)
                    {
                        tempDecValue = 0;
                    }

                    if (tempValue != 0 && tempDecValue == 0)
                    {
                        return false;
                    }

                    if (LdrID.Contains(tempValue))
                    {
                        if (i + 2 == grdpayroll.Rows.Count && grdpayroll[i + 1, 2].Value.ToString() == "Debit")
                        {
                            //Do Nothing
                        }
                        else
                            return false;
                    }
                    else
                        LdrID[i] = tempValue;

                    if (i + 2 == grdpayroll.Rows.Count && grdpayroll[i + 1, 2].Value.ToString() == "Debit")
                    {
                        //Donothing
                    }
                    else
                        Amt[i] = tempDecValue;
                }

                catch
                {
                    return false;
                }
            }
            OnlyReqdDetailRows = LdrID.Count(i => i != 0);
            return true;
        }

        private string ReadAllJournalEntryforCashpayment(int bulkmasterid,string vouchertype)
        {
            System.Text.Encoding AEncoder = System.Text.Encoding.Unicode;
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            System.Xml.XmlTextWriter tw = new System.Xml.XmlTextWriter(ms, AEncoder);
            //SeriesID = (ListItem)cboSeriesName.SelectedItem;
            //liProjectID = (ListItem)cboProjectName.SelectedItem;
            int liProjectID = Convert.ToInt32(grdpayroll[1, 9].Value.ToString());
        //liCashID = (ListItem)cmboCashAccount.SelectedItem;
            BusinessLogic.Accounting.BulkPosting bpcp = new BusinessLogic.Accounting.BulkPosting();
           int cashid= bpcp.GetCashID(bulkmasterid,vouchertype);

            //validate grid entry
            if (!ValidateGrid())
                return string.Empty;

            tw.WriteStartDocument();
            #region  Journal
            tw.WriteStartElement("CASHPAYMENT");
            {
                ///For Journal Master Section 
                int SeriesID = Convert.ToInt32(grdpayroll[1, 8].Value.ToString());
                //int SID = System.Convert.ToInt32(SeriesID.ID);
                string Voucher_No = System.Convert.ToString(txtVchNo.Text);
                DateTime Journal_Date = Date.ToDotNet(txtDate.Text);
                string Remarks = "Bulk Posting Info";
                
                int ProjectID = Convert.ToInt32(grdpayroll[1, 9].Value.ToString());
               // int SID = System.Convert.ToInt32(SeriesID.ID);
                int CashID = System.Convert.ToInt32(cashid);
               // string Voucher_No = System.Convert.ToString(txtVchNo.Text);
                DateTime CashPayment_Date = Date.ToDotNet(txtDate.Text);
                //string Remarks = System.Convert.ToString(txtRemarks.Text);
                //int ProjectID = System.Convert.ToInt32(liProjectID.ID);
                DateTime Created_Date = System.Convert.ToDateTime(DateTime.Now);
                string Created_By = System.Convert.ToString("Admin");
                DateTime Modified_Date = System.Convert.ToDateTime(DateTime.Now);
                string Modified_By = System.Convert.ToString("Admin");
                tw.WriteStartElement("CASHPAYMENTMASTER");
                tw.WriteElementString("SeriesID", SeriesID.ToString());
                tw.WriteElementString("LedgerID", cashid.ToString());
                tw.WriteElementString("Voucher_No", Voucher_No.ToString());
                tw.WriteElementString("CashPayment_Date", Date.ToDB(CashPayment_Date));
                tw.WriteElementString("Remarks", Remarks.ToString());
                tw.WriteElementString("ProjectID", ProjectID.ToString());
                tw.WriteElementString("Created_Date", Date.ToDB(Created_Date));
                tw.WriteElementString("Created_By", Created_By.ToString());
                tw.WriteElementString("Modified_Date", Date.ToDB(Modified_Date));
                tw.WriteElementString("Modified_By", Modified_By.ToString());
                tw.WriteEndElement();
                ///For journal Detail Section             
                int CashPaymentID = 0;
                int LedgerID = 0;
                Decimal Amount = 0;
                string DrCr = "";
                string RemarksDetail = "";
                tw.WriteStartElement("CASHPAYMENTDETAIL");
                for (int i = 0; i < OnlyReqdDetailRows; i++)
                {
                    //JournalID = System.Convert.ToInt32(288);
                    LedgerID = System.Convert.ToInt32(grdpayroll[i + 1, 5].Value);
                    Amount = System.Convert.ToDecimal(grdpayroll[i + 1, 3].Value);
                    RemarksDetail = System.Convert.ToString(grdpayroll[i + 1, 4].Value);
                    tw.WriteStartElement("DETAIL");
                    tw.WriteElementString("CashPaymentID", CashPaymentID.ToString());
                    tw.WriteElementString("LedgerID", LedgerID.ToString());
                    tw.WriteElementString("Amount", Amount.ToString());
                    tw.WriteElementString("Remarks", RemarksDetail.ToString());
                    tw.WriteEndElement();
                }
                tw.WriteEndElement();
                //Write Checked Accounting class ID
                try
                {
                   
                    tw.WriteStartElement("ACCCLASSIDS");
                    for (int i = 0; i < dtaccclass.Rows.Count; i++)
                    {
                        DataRow dr = dtaccclass.Rows[i];
                        tw.WriteElementString("AccID", dr["AccClassID"].ToString());
                    }
                    tw.WriteEndElement();
                }
                catch
                { }

            }
            tw.WriteFullEndElement();
            #endregion
            tw.WriteEndDocument();
            tw.Flush();
            tw.Close();
            string strXML = AEncoder.GetString(ms.ToArray());
            // MessageBox.Show(strXML);
            return strXML;
        }

        private string ReadAllBankPaymentEntry(int bulkmasterid, string vouchertype)
        {
            System.Text.Encoding AEncoder = System.Text.Encoding.Unicode;
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            System.Xml.XmlTextWriter tw = new System.Xml.XmlTextWriter(ms, AEncoder);
           // SeriesID = (ListItem)cboSeriesName.SelectedItem;
            //liProjectID = (ListItem)cboProjectName.SelectedItem;
            int liProjectID = Convert.ToInt32(grdpayroll[1, 9].Value.ToString());
            //liBankID = (ListItem)cmboBankAccount.SelectedItem;
            BusinessLogic.Accounting.BulkPosting bpcp = new BusinessLogic.Accounting.BulkPosting();
            int bankid = bpcp.GetBankID(bulkmasterid, vouchertype);

            //validate grid entry
            if (!ValidateGrid())
                return string.Empty;

            tw.WriteStartDocument();
            #region  Bank Payment
            tw.WriteStartElement("BANKPAYMENT");
            {
                ///For Bank Payment Master Section
                ///int JournalID = System.Convert.ToInt32(9); // Auto increment               
                //int SID = System.Convert.ToInt32(SeriesID.ID);
                //int LedgerID = System.Convert.ToInt32(liBankID.ID);

                int SeriesID = Convert.ToInt32(grdpayroll[1, 8].Value.ToString());
                //int SID = System.Convert.ToInt32(SeriesID.ID);
                string Voucher_No = System.Convert.ToString(txtVchNo.Text);
                DateTime Journal_Date = Date.ToDotNet(txtDate.Text);
                string Remarks = "Bulk Posting Info";
                int ProjectID = Convert.ToInt32(grdpayroll[1, 9].Value.ToString());
                DateTime BankPayment_Date = Date.ToDotNet(txtDate.Text);
               // string Remarks = System.Convert.ToString(txtRemarks.Text);
               // int ProjectID = System.Convert.ToInt32(liProjectID.ID);
                DateTime Created_Date = System.Convert.ToDateTime(DateTime.Now);
                //DateTime Created_Date = Date.GetServerDate();
                //string Created_By = System.Convert.ToString("Admin");
                string Created_By = User.CurrentUserName;
                DateTime Modified_Date = System.Convert.ToDateTime(DateTime.Now);
                //DateTime Modified_Date = Date.GetServerDate();
                //string Modified_By = System.Convert.ToString("Admin");
                string Modified_By = User.CurrentUserName;

                tw.WriteStartElement("BANKPAYMENTMASTER");
                tw.WriteElementString("SeriesID", SeriesID.ToString());
                tw.WriteElementString("LedgerID", bankid.ToString());
                tw.WriteElementString("Voucher_No", Voucher_No.ToString());
                tw.WriteElementString("BankPayment_Date", Date.ToDB(BankPayment_Date));
                tw.WriteElementString("Remarks", Remarks.ToString());
                tw.WriteElementString("ProjectID", ProjectID.ToString());
                tw.WriteElementString("Created_Date", Date.ToDB(Created_Date));
                tw.WriteElementString("Created_By", Created_By.ToString());
                tw.WriteElementString("Modified_Date", Date.ToDB(Modified_Date));
                tw.WriteElementString("Modified_By", Modified_By.ToString());
                tw.WriteEndElement();
                ///For journal Detail Section                           
                int LedgerID1 = 0;
                int bankpaymentid = 0;
                Decimal Amount = 0;
                string RemarksDetail = "";
                string ChequeNumber = "";
                string ChequeDate = "";
                tw.WriteStartElement("BANKPAYMENTDETAIL");
                for (int i = 0; i < OnlyReqdDetailRows; i++)
                {
                   
                    //JournalID = System.Convert.ToInt32(288);
                    LedgerID1 = System.Convert.ToInt32(grdpayroll[i + 1, 5].Value);
                    Amount = System.Convert.ToDecimal(grdpayroll[i + 1, 3].Value);
                    RemarksDetail = System.Convert.ToString(grdpayroll[i + 1, 4].Value);

                    tw.WriteStartElement("DETAIL");
                    tw.WriteElementString("BankPaymentID", bankpaymentid.ToString());
                    tw.WriteElementString("LedgerID", LedgerID1.ToString());
                    tw.WriteElementString("Amount", Amount.ToString());
                    tw.WriteElementString("Remarks", RemarksDetail.ToString());
                  
                    tw.WriteEndElement();
                }
                tw.WriteEndElement();
                //Write Checked Accounting class ID
                try
                {
                    tw.WriteStartElement("ACCCLASSIDS");
                    for (int i = 0; i < dtaccclass.Rows.Count; i++)
                    {
                        DataRow dr = dtaccclass.Rows[i];
                        tw.WriteElementString("AccID", dr["AccClassID"].ToString());
                    }
                    tw.WriteEndElement();
                }
                catch
                { }

            }
            tw.WriteFullEndElement();
            #endregion
            tw.WriteEndDocument();
            tw.Flush();
            tw.Close();
            string strXML = AEncoder.GetString(ms.ToArray());
            // MessageBox.Show(strXML);
            return strXML;
        }

        private string ReadAllBankReceiptEntry(int bulkmasterid, string vouchertype)
        {
            System.Text.Encoding AEncoder = System.Text.Encoding.Unicode;
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            System.Xml.XmlTextWriter tw = new System.Xml.XmlTextWriter(ms, AEncoder);
            //SeriesID = (ListItem)cboSeriesName.SelectedItem;
            //liProjectID = (ListItem)cboProjectName.SelectedItem;
            //liBankID = (ListItem)comboBankAccount.SelectedItem;

            int liProjectID = Convert.ToInt32(grdpayroll[1, 9].Value.ToString());
            BusinessLogic.Accounting.BulkPosting bpcp = new BusinessLogic.Accounting.BulkPosting();
            //int bankid = bpcp.GetBankID(bulkmasterid, vouchertype);
            int bankid = bpcp.GetBankReceiptID(bulkmasterid, vouchertype);

            //validate grid entry
            if (!ValidateGrid())
                return string.Empty;

            tw.WriteStartDocument();
            #region  Journal
            tw.WriteStartElement("BANKRECEIPT");
            {
                int SeriesID = Convert.ToInt32(grdpayroll[1, 8].Value.ToString());
                string Voucher_No = System.Convert.ToString(txtVchNo.Text);
                DateTime Journal_Date = Date.ToDotNet(txtDate.Text);
                string Remarks = "Bulk Posting Info";
                int ProjectID = Convert.ToInt32(grdpayroll[1, 9].Value.ToString());
                ///For Bank Receipt Master Section                   
                //int SID = System.Convert.ToInt32(SeriesID.ID);
          // int BankID = System.Convert.ToInt32(liBankID.ID);
               // string Voucher_No = System.Convert.ToString(txtVchNo.Text);
                DateTime BankReceipt_Date = Date.ToDotNet(txtDate.Text);
               // string Remarks = System.Convert.ToString(txtRemarks.Text);
               // int ProjectID = System.Convert.ToInt32(liProjectID.ID);
                DateTime Created_Date = System.Convert.ToDateTime(DateTime.Now);
                string Created_By = System.Convert.ToString("Admin");
                DateTime Modified_Date = System.Convert.ToDateTime(DateTime.Now);
                string Modified_By = System.Convert.ToString("Admin");
                tw.WriteStartElement("BANKRECEIPTMASTER");
                tw.WriteElementString("SeriesID", SeriesID.ToString());
                tw.WriteElementString("LedgerID", bankid.ToString());
                tw.WriteElementString("Voucher_No", Voucher_No.ToString());
                tw.WriteElementString("BankReceipt_Date", Date.ToDB(BankReceipt_Date));
                tw.WriteElementString("Remarks", Remarks.ToString());
                tw.WriteElementString("ProjectID", ProjectID.ToString());
                tw.WriteElementString("Created_Date", Date.ToDB(Created_Date));
                tw.WriteElementString("Created_By", Created_By.ToString());
                tw.WriteElementString("Modified_Date", Date.ToDB(Modified_Date));
                tw.WriteElementString("Modified_By", Modified_By.ToString());
                tw.WriteEndElement();
                ///For journal Detail Section             
                int BankReceiptID = 0;
                int LedgerID = 0;
                Decimal Amount = 0;
                string RemarksDetail = "";
                //string ChequeNumber = "";
                //string ChequeBank = "";
                //string ChequeDate = "";
                tw.WriteStartElement("BANKRECEIPTDETAIL");
                for (int i = 0; i < OnlyReqdDetailRows; i++)
                {
                    LedgerID = System.Convert.ToInt32(grdpayroll[i + 1, 5].Value);
                    Amount = System.Convert.ToDecimal(grdpayroll[i + 1, 3].Value);
                    RemarksDetail = System.Convert.ToString(grdpayroll[i + 1, 4].Value);
                    tw.WriteStartElement("DETAIL");
                    tw.WriteElementString("BankReceiptID", BankReceiptID.ToString());
                    tw.WriteElementString("LedgerID", LedgerID.ToString());
                    tw.WriteElementString("Amount", Amount.ToString());
                    tw.WriteElementString("Remarks", RemarksDetail.ToString());
                    tw.WriteEndElement();
                }
                tw.WriteEndElement();
                //Write Checked Accounting class ID
                try
                {
                    tw.WriteStartElement("ACCCLASSIDS");
                    for (int i = 0; i < dtaccclass.Rows.Count; i++)
                    {
                        DataRow dr = dtaccclass.Rows[i];
                        tw.WriteElementString("AccID", dr["AccClassID"].ToString());
                    }
                    tw.WriteEndElement();
                }
                catch
                { }

            }
            tw.WriteFullEndElement();
            #endregion
            tw.WriteEndDocument();
            tw.Flush();
            tw.Close();
            string strXML = AEncoder.GetString(ms.ToArray());
           // MessageBox.Show(strXML);
            return strXML;
        }

        private string ReadAllCashReceiptEntry(int bulkmasterid, string vouchertype)
        {
            System.Text.Encoding AEncoder = System.Text.Encoding.Unicode;
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            System.Xml.XmlTextWriter tw = new System.Xml.XmlTextWriter(ms, AEncoder);
            //SeriesID = (ListItem)cboSeriesName.SelectedItem;
            //liProjectID = (ListItem)cboProjectName.SelectedItem;
            //liCashID = (ListItem)cmboCashAccount.SelectedItem;
            int liProjectID = Convert.ToInt32(grdpayroll[1, 9].Value.ToString());
            BusinessLogic.Accounting.BulkPosting bpcp = new BusinessLogic.Accounting.BulkPosting();
            int cashid = bpcp.GetCashReceiptID(bulkmasterid, vouchertype);

            //validate grid entry
            if (!ValidateGrid())
                return string.Empty;

            tw.WriteStartDocument();
            #region  Journal
            tw.WriteStartElement("CASHRECEIPT");
            {

                int SeriesID = Convert.ToInt32(grdpayroll[1, 8].Value.ToString());
                string Voucher_No = System.Convert.ToString(txtVchNo.Text);
                DateTime Journal_Date = Date.ToDotNet(txtDate.Text);
                string Remarks = "Bulk Posting Info";
                int ProjectID = Convert.ToInt32(grdpayroll[1, 9].Value.ToString());
                ///For Journal Master Section                  
                //int SID = System.Convert.ToInt32(SeriesID.ID);
                //int CashID = System.Convert.ToInt32(liCashID.ID);
                //string Voucher_No = System.Convert.ToString(txtVchNo.Text);
                DateTime CashReceipt_Date = Date.ToDotNet(txtDate.Text);
                //string Remarks = System.Convert.ToString(txtRemarks.Text);
                //int ProjectID = System.Convert.ToInt32(liProjectID.ID);
                DateTime Created_Date = System.Convert.ToDateTime(DateTime.Now);
                string Created_By = System.Convert.ToString("Admin");
                DateTime Modified_Date = System.Convert.ToDateTime(DateTime.Now);
                string Modified_By = System.Convert.ToString("Admin");
                tw.WriteStartElement("CASHRECEIPTMASTER");
                tw.WriteElementString("SeriesID",SeriesID.ToString());
                tw.WriteElementString("LedgerID", cashid.ToString());
                tw.WriteElementString("Voucher_No", Voucher_No.ToString());
                tw.WriteElementString("CashReceipt_Date", Date.ToDB(CashReceipt_Date));
                tw.WriteElementString("Remarks", Remarks.ToString());
                tw.WriteElementString("ProjectID", ProjectID.ToString());
                tw.WriteElementString("Created_Date", Date.ToDB(Created_Date));
                tw.WriteElementString("Created_By", Created_By.ToString());
                tw.WriteElementString("Modified_Date", Date.ToDB(Modified_Date));
                tw.WriteElementString("Modified_By", Modified_By.ToString());
                tw.WriteEndElement();
                ///For journal Detail Section             
                int CashReceiptID = 0;
                int LedgerID = 0;
                Decimal Amount = 0;
                string RemarksDetail = "";
                tw.WriteStartElement("CASHRECEIPTDETAIL");
                for (int i = 0; i < OnlyReqdDetailRows; i++)
                {

                    LedgerID = System.Convert.ToInt32(grdpayroll[i + 1, 5].Value);
                    Amount = System.Convert.ToDecimal(grdpayroll[i + 1, 3].Value);
                    RemarksDetail = System.Convert.ToString(grdpayroll[i + 1, 4].Value);
                    //JournalID = System.Convert.ToInt32(288);
                    //LedgerID = System.Convert.ToInt32(grdCashReceipt[i + 1, 6].Value);
                    //Amount = System.Convert.ToDecimal(grdCashReceipt[i + 1, 3].Value);
                    //RemarksDetail = System.Convert.ToString(grdCashReceipt[i + 1, 5].Value);
                    tw.WriteStartElement("DETAIL");
                    tw.WriteElementString("CashReceiptID", CashReceiptID.ToString());
                    tw.WriteElementString("LedgerID", LedgerID.ToString());
                    tw.WriteElementString("Amount", Amount.ToString());
                    tw.WriteElementString("Remarks", RemarksDetail.ToString());
                    tw.WriteEndElement();
                }
                tw.WriteEndElement();
                //Write Checked Accounting class ID
                try
                {
                    tw.WriteStartElement("ACCCLASSIDS");
                    for (int i = 0; i < dtaccclass.Rows.Count; i++)
                    {
                        DataRow dr = dtaccclass.Rows[i];
                        tw.WriteElementString("AccID", dr["AccClassID"].ToString());
                    }
                    tw.WriteEndElement();
                }
                catch
                { }

            }
            tw.WriteFullEndElement();
            #endregion
            tw.WriteEndDocument();
            tw.Flush();
            tw.Close();
            string strXML = AEncoder.GetString(ms.ToArray());
            // MessageBox.Show(strXML);
            return strXML;
        }
    }
}
