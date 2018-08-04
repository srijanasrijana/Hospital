using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BusinessLogic.Accounting;
using BusinessLogic;
using DateManager;
using System.Collections;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;

namespace AccSwift.Forms
{
    public partial class frmChequeCashReminder : Form
    {
        private string FilterString = "";
        BusinessLogic.Accounting.ChequeReceipt m_chequeReceipt = new BusinessLogic.Accounting.ChequeReceipt();
        List<int> AccClassID = new List<int>();
        private DataRow[] drFound;
        private DataTable dTable;
        private int CurrRowPos = 0;
        public frmChequeCashReminder()
        {
            InitializeComponent();
        }

        private void frmChequeCashReminder_Load(object sender, EventArgs e)
        {
            dTable = ChequeReceipt.GetChequeCashDetails();
            drFound = dTable.Select(FilterString);
            //Let the whole row to be selected
            grdchequecashreminder.SelectionMode = SourceGrid.GridSelectionMode.Row;
            //Set the border width of the selection to thin
           // DevAge.Drawing.RectangleBorder b = grdchequecashreminder.Selection.b;
          //  b.SetWidth(1);
           // grdchequecashreminder.Selection.Border = b;
            //Disable multiple selection
            grdchequecashreminder.Selection.EnableMultiSelection = false;
            FillGrid();
        }
        private void FillGrid()
        {
            try
            {
                grdchequecashreminder.Rows.Clear();
                grdchequecashreminder.Redim(drFound.Count() + 1, 15);
                WriteHeader();
                for (int i = 1; i <= drFound.Count(); i++)
                {
                    DataRow dr = drFound[i - 1];
                    grdchequecashreminder[i, 0] = new SourceGrid.Cells.Cell(Date.ToSystem(Convert.ToDateTime( dr["chequeReceiptDate"].ToString())));

                    string ledgername = Ledger.GetLedgerNameFromID(Convert.ToInt32( dr["LedgerID"].ToString()));
                    grdchequecashreminder[i, 1] = new SourceGrid.Cells.Cell(ledgername);
                   
                    grdchequecashreminder[i, 2] = new SourceGrid.Cells.Cell(dr["bankName"].ToString());
                   
                    grdchequecashreminder[i, 3] = new SourceGrid.Cells.Cell(Convert.ToDouble( dr["amount"].ToString()).ToString(Misc.FormatNumber(Global.Comma_Separated,Global.DecimalPlaces)));
                   
                    grdchequecashreminder[i, 4] = new SourceGrid.Cells.Cell(dr["chequeNumber"].ToString());
                   
                    grdchequecashreminder[i, 5] = new SourceGrid.Cells.Cell(Date.ToSystem(Convert.ToDateTime(dr["chequeDate"].ToString())));

                    grdchequecashreminder[i, 6] = new SourceGrid.Cells.Cell(Date.ToSystem(Convert.ToDateTime(dr["chequeCashDate"].ToString())));

                    grdchequecashreminder[i, 7] = new SourceGrid.Cells.Cell(dr["LedgerID"].ToString());

                    grdchequecashreminder[i, 8] = new SourceGrid.Cells.Cell(dr["BankID"].ToString());

                    grdchequecashreminder[i, 9] = new SourceGrid.Cells.Cell(dr["ChequeReceiptID"].ToString());

                    grdchequecashreminder[i, 10] = new SourceGrid.Cells.Cell(dr["SeriesID"].ToString());

                    grdchequecashreminder[i, 11] = new SourceGrid.Cells.Cell(dr["ProjectID"].ToString());

                    grdchequecashreminder[i, 12] = new SourceGrid.Cells.Cell(dr["voucherNo"].ToString());

                    grdchequecashreminder[i, 13] = new SourceGrid.Cells.Cell(dr["remarks"].ToString());

                    grdchequecashreminder[i, 14] = new SourceGrid.Cells.Cell(dr["chequeBank"].ToString());
                    
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        //Fills the header of the grid with the required column names and also sets the width
        private void WriteHeader()
        {
            grdchequecashreminder[0, 0] = new SourceGrid.Cells.ColumnHeader("ChequeReceipt Date");
            grdchequecashreminder[0, 1] = new SourceGrid.Cells.ColumnHeader("Account");
            grdchequecashreminder[0, 2] = new SourceGrid.Cells.ColumnHeader("Bank");
            grdchequecashreminder[0, 3] = new SourceGrid.Cells.ColumnHeader("Amount");
            grdchequecashreminder[0, 4] = new SourceGrid.Cells.ColumnHeader("Cheque Number");
            grdchequecashreminder[0, 5] = new SourceGrid.Cells.ColumnHeader("ChequeDate");
            grdchequecashreminder[0, 6] = new SourceGrid.Cells.ColumnHeader("ChequeCash Date");
            grdchequecashreminder[0, 7] = new SourceGrid.Cells.ColumnHeader("LedgerID");
            grdchequecashreminder[0, 8] = new SourceGrid.Cells.ColumnHeader("BankID");
            grdchequecashreminder[0, 9] = new SourceGrid.Cells.ColumnHeader("ChequeReceiptID");
            grdchequecashreminder[0, 10] = new SourceGrid.Cells.ColumnHeader("SeriesID");
            grdchequecashreminder[0, 11] = new SourceGrid.Cells.ColumnHeader("ProjectID");
            grdchequecashreminder[0, 12] = new SourceGrid.Cells.ColumnHeader("VoucherNo");
            grdchequecashreminder[0, 13] = new SourceGrid.Cells.ColumnHeader("Remarks");
            grdchequecashreminder[0, 14] = new SourceGrid.Cells.ColumnHeader("chequebank");

            grdchequecashreminder[0, 0].Column.Width = 120;
            grdchequecashreminder[0, 1].Column.Width = 130;
            grdchequecashreminder[0, 2].Column.Width = 100;
            grdchequecashreminder[0, 3].Column.Width = 100;
            grdchequecashreminder[0, 4].Column.Width = 100;
            grdchequecashreminder[0, 5].Column.Width = 100;
            grdchequecashreminder[0, 6].Column.Width = 120;
            grdchequecashreminder[0, 7].Column.Width = 3;
            grdchequecashreminder[0, 8].Column.Width = 3;
            grdchequecashreminder[0, 9].Column.Width = 3;
            grdchequecashreminder[0, 10].Column.Width = 3;
            grdchequecashreminder[0, 11].Column.Width = 3;
            grdchequecashreminder[0, 12].Column.Width = 3;
            grdchequecashreminder[0, 13].Column.Width = 3;
            grdchequecashreminder[0, 14].Column.Width = 3;


            grdchequecashreminder[0, 7].Column.Visible = false;
            grdchequecashreminder[0, 8].Column.Visible = false;
            grdchequecashreminder[0, 9].Column.Visible = false;
            grdchequecashreminder[0, 10].Column.Visible = false;
            grdchequecashreminder[0, 11].Column.Visible = false;
            grdchequecashreminder[0, 12].Column.Visible = false;
            grdchequecashreminder[0, 13].Column.Visible = false;
            grdchequecashreminder[0, 14].Column.Visible = false;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btndeposit_Click(object sender, EventArgs e)
        {
            int CurRow = grdchequecashreminder.Selection.GetSelectionRegion().GetRowsIndex()[0];
            string BankReceiptXMLString = ReadAllBankReceiptEntry(CurRow);
            if (BankReceiptXMLString == string.Empty)
            {
                MessageBox.Show("Unable to cast bank receipt entry to XML!");
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
                    Global.m_db.cn.Open();
                    int intRetValue = dbCommand.ExecuteNonQuery();

                    //MessageBox.Show(intRetValue.ToString());                                
                }
            }
            #endregion
            int   LedgerID = System.Convert.ToInt32(grdchequecashreminder[CurRow, 7].Value.ToString());
            int ChequeReceiptID = System.Convert.ToInt32(grdchequecashreminder[CurRow, 9].Value.ToString());
            ChequeReceipt.UpdateChequeReceiptInformation(ChequeReceiptID,LedgerID);

            Global.Msg("Cheque Deposited successfully!");
            frmChequeCashReminder_Load(sender,e);


        }
        private string ReadAllBankReceiptEntry(int CurrentRow)
        {
            System.Text.Encoding AEncoder = System.Text.Encoding.Unicode;
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            System.Xml.XmlTextWriter tw = new System.Xml.XmlTextWriter(ms, AEncoder);
           


            tw.WriteStartDocument();
            #region  Journal
            tw.WriteStartElement("BANKRECEIPT");
            {
                ///For Bank Receipt Master Section                   
                int SID = Convert.ToInt32(grdchequecashreminder[CurrentRow, 10].Value.ToString());
                int BankID = Convert.ToInt32(grdchequecashreminder[CurrentRow, 8].Value.ToString());
                string Voucher_No = grdchequecashreminder[CurrentRow, 12].ToString();
                DateTime BankReceipt_Date =Date.ToDotNet( grdchequecashreminder[CurrentRow, 5].Value.ToString());
                string Remarks = grdchequecashreminder[CurrentRow, 13].ToString();
                int ProjectID = Convert.ToInt32(grdchequecashreminder[CurrentRow, 11].Value.ToString());
                DateTime Created_Date = System.Convert.ToDateTime(Date.GetServerDate());
                string Created_By = User.CurrentUserName;
                DateTime Modified_Date = System.Convert.ToDateTime(Date.GetServerDate());
               
                string Modified_By = User.CurrentUserName;
                tw.WriteStartElement("BANKRECEIPTMASTER");
                tw.WriteElementString("SeriesID", SID.ToString());
                tw.WriteElementString("LedgerID", BankID.ToString());
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
                string ChequeNumber = "";
                string ChequeBank = "";
                string ChequeDate = "";
                DateTime ChequeDateValue;
                tw.WriteStartElement("BANKRECEIPTDETAIL");
               
                    LedgerID = System.Convert.ToInt32(grdchequecashreminder[CurrentRow, 7].Value.ToString());
                    Amount = System.Convert.ToDecimal(grdchequecashreminder[CurrentRow, 3].Value.ToString());
                    RemarksDetail = System.Convert.ToString(grdchequecashreminder[CurrentRow, 13].ToString());
                    ChequeNumber = System.Convert.ToString(grdchequecashreminder[CurrentRow, 4].Value.ToString());
                    ChequeBank = System.Convert.ToString(grdchequecashreminder[CurrentRow, 14].Value.ToString());
                    if (ChequeNumber == "")
                    {
                        ChequeDate = "";
                        ChequeDateValue = Date.GetServerDate();
                    }
                    //ChequeDate = null;
                    else
                    {
                        //ChequeDate = System.Convert.ToString(grdBankReceipt[i + 1, 8].Value);
                        ChequeDate = grdchequecashreminder[CurrentRow, 5].Value.ToString();
                        ChequeDateValue = Date.ToDotNet(ChequeDate);
                    }

                    tw.WriteStartElement("DETAIL");
                    tw.WriteElementString("BankReceiptID", BankReceiptID.ToString());
                    tw.WriteElementString("LedgerID", LedgerID.ToString());
                    tw.WriteElementString("Amount", Amount.ToString());
                    tw.WriteElementString("Remarks", RemarksDetail.ToString());
                    tw.WriteElementString("ChequeNumber", ChequeNumber.ToString());
                    tw.WriteElementString("ChequeBank", ChequeBank.ToString());
                    // DateTime BankReceipt_Date = Date.ToDotNet(txtDate.Text);
                    // tw.WriteElementString("BankReceipt_Date", Date.ToDB(BankReceipt_Date));
                    if (ChequeDate != "")
                        // tw.WriteElementString("ChequeDate", Date.ToDotNet(ChequeDate).ToString());
                        tw.WriteElementString("ChequeDate", Date.ToDB(ChequeDateValue));
                    tw.WriteEndElement();
              
                tw.WriteEndElement();
                //Write Checked Accounting class ID
                try
                {
                    DataTable dt = m_chequeReceipt.GetChequeCashAccClass(Convert.ToInt32(grdchequecashreminder[CurrentRow, 9].Value.ToString()));
                   // ArrayList arrNode = treeAccClass.GetCheckedNodes(true);
                    tw.WriteStartElement("ACCCLASSIDS");
                    foreach (DataRow dr in dt.Rows)
                    {
                        tw.WriteElementString("AccID", Convert.ToInt32(dr["AccClassID"]).ToString());
                    }
                    tw.WriteEndElement();
                }
                catch
                { }
                string MacAddress = ""; ;
                foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
                {

                    if (nic.OperationalStatus == OperationalStatus.Up && (!nic.Description.Contains("Virtual") && !nic.Description.Contains("Pseudo")))
                    {
                        if (nic.GetPhysicalAddress().ToString() != "")
                        {
                            MacAddress = nic.GetPhysicalAddress().ToString();
                        }
                    }
                }
                string ComputerName = Environment.MachineName;
               
                string IpAddress = Dns.GetHostAddresses(Environment.MachineName)[0].ToString();
                tw.WriteStartElement("COMPUTERDETAILS");
                tw.WriteElementString("CompDetails", ComputerName.ToString());
                tw.WriteElementString("MacAddress", MacAddress.ToString());
                tw.WriteElementString("IpAddress", IpAddress.ToString());
                tw.WriteEndElement();

              

            }
            tw.WriteFullEndElement();
            #endregion
            tw.WriteEndDocument();
            tw.Flush();
            tw.Close();
            string strXML = AEncoder.GetString(ms.ToArray());
            //MessageBox.Show(strXML);
            return strXML;
        }
    }
}
