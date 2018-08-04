using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Collections;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BusinessLogic;
using DateManager;
using Inventory.CrystalReports;
using System.Threading;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Inventory.Forms;

namespace Inventory
{
    public partial class frmChequeReport : Form
    {
        private ChequeRegisterSettings m_ChequeSettings;
        private SourceGrid.Cells.Controllers.CustomEvents dblClick;
        ArrayList AccClassID = new ArrayList();
        private DataSet.dsCheque dsCheque = new DataSet.dsCheque();
        private int prntDirect = 0;
        private string FileName = "";
        //For Export Menu
        ContextMenu Menu_Export;

        public frmChequeReport()
        {
            InitializeComponent();
        }

        public frmChequeReport(ChequeRegisterSettings ChequeSettings)//Constructor having class object as argument
        {
            try
            {
                #region BLOCK FOR INTIALIZING THE CONSTRUCTOR PARAMETERS
                InitializeComponent();
                m_ChequeSettings = new ChequeRegisterSettings();
                m_ChequeSettings.ChequeReceived = ChequeSettings.ChequeReceived;
                m_ChequeSettings.Status = ChequeSettings.Status;
                m_ChequeSettings.HasDateRange = ChequeSettings.HasDateRange;
                m_ChequeSettings.FromDate = ChequeSettings.FromDate;
                m_ChequeSettings.ToDate = ChequeSettings.ToDate;
                m_ChequeSettings.HasBank = ChequeSettings.HasBank;
                m_ChequeSettings.HasParty = ChequeSettings.HasParty;
                m_ChequeSettings.BankID = ChequeSettings.BankID;
                m_ChequeSettings.PartyID = ChequeSettings.PartyID;
                m_ChequeSettings.AccClassID = ChequeSettings.AccClassID;
                m_ChequeSettings.ProjectID = ChequeSettings.ProjectID;
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void frmChequeReport_Load(object sender, EventArgs e)

        {
            DisplayBannar();

            //Add a doube this function to send the parenle click handler. When user dblclicks the cell, will ust form the Currency code
            dblClick = new SourceGrid.Cells.Controllers.CustomEvents();
            dblClick.DoubleClick += new EventHandler(grdCheque_DoubleClick);
            //Let the whole row to be selected
            grdCheque.SelectionMode = SourceGrid.GridSelectionMode.Row;
       
            grdCheque.Selection.EnableMultiSelection = false;
            grdCheque.Redim(1, 9);
            grdCheque.FixedRows = 1;
            //int rows = grdTrial.Rows.Count;
            MakeHeader();
            int Sno = 1;
            dsCheque.Clear();
            #region All
            if (m_ChequeSettings.ChequeReceived == 0)
            {
                given();
                clear();
                unclear();
            }
            #endregion

            #region BLOCK FOR CHEQUE GIVEN
            //if (m_ChequeSettings.ChequeReceived == 1 || m_ChequeSettings.ChequeReceived == 0) //Display bank payments
            if (m_ChequeSettings.ChequeReceived == 1)
            {
                Transaction Transaction = new Transaction();
                int BankGroupID = AccountGroup.GetGroupIDFromGroupNumber(7); /////**************************** for bank only
              
                //Get all ledger under banks
                DataTable dt = AccountGroup.GetDetailLedgerID(BankGroupID, true); // Get all ledger ID and sub ledger IDs
                //Kist bank n himalayan bank in dt              

                //int Sno = 1;
                foreach (DataRow dr in dt.Rows)
                {

                    if (m_ChequeSettings.HasBank)
                    {
                        if (m_ChequeSettings.BankID != Convert.ToInt32(dr["LedgerID"])) // allows only selected bank
                            continue;                     
                    }                   
                    DataTable dtBankTransaction = new DataTable();
                    if(m_ChequeSettings.HasDateRange)
                    {                    
                         //dtBankTransaction = Transaction.GetLedgerTransact(Convert.ToInt32(dr["LedgerID"]),m_ChequeSettings.FromDate, m_ChequeSettings.ToDate);//Get the Transact Inforamtion of corresponding LedgerID according to datewise
                         dtBankTransaction = Transaction.GetLedgerTransactWithAccountClassAndProject(Convert.ToInt32(dr["LedgerID"]), m_ChequeSettings.FromDate, m_ChequeSettings.ToDate, m_ChequeSettings.AccClassID, m_ChequeSettings.ProjectID);//Get the Transact Inforamtion of corresponding LedgerID according to datewise
                    }
                    else
                    {
                         //dtBankTransaction = Transaction.GetLedgerTransact(Convert.ToInt32(dr["LedgerID"]), null, null);
                        dtBankTransaction = Transaction.GetLedgerTransactWithAccountClassAndProject(Convert.ToInt32(dr["LedgerID"]), null, null, m_ChequeSettings.AccClassID, m_ChequeSettings.ProjectID);
                    }

                    foreach (DataRow drBankTransaction in dtBankTransaction.Rows)
                    {
                        AccClassID.Add(1);
                        DataTable dtTransactInfo = new DataTable();                     
                            dtTransactInfo = Transaction.GetTransactionInfoAlongWithParty(drBankTransaction["RowID"].ToString(), "BANK_PMNT", 0, AccClassID);
                        for (int j = 0; j < dtTransactInfo.Rows.Count; j++)
                        {
                            DataRow drTransactInfo = dtTransactInfo.Rows[j];
                            DataTable dtLedgerAllInfo = Ledger.GetLedgerInfo(Convert.ToInt32(drTransactInfo["LedgerID"]), LangMgr.DefaultLanguage);
                            DataRow drLedgerAllInfo = dtLedgerAllInfo.Rows[0];

                            BankPayment m_BankPayment = new BankPayment();
                            if (Convert.ToInt32(dr["LedgerID"]) == Convert.ToInt32(drTransactInfo["LedgerID"]))//This is the transaction done by specified Ledger hit by user to other ledgers except own
                            {
                                DataTable dtVoucherName = new DataTable();
                                DataTable dtBankPaymentDetail = new DataTable();
                                dtVoucherName = m_BankPayment.GetBankPaymentMaster(Convert.ToInt32(drBankTransaction["RowID"]));
                                if (dtVoucherName.Rows.Count > 0)
                                {
                                    //Get bank payment detail from bank payment master using bankpaymentID
                                    dtBankPaymentDetail = m_BankPayment.GetBankPaymentDetail(Convert.ToInt32(drBankTransaction["RowID"]));
                                    foreach (DataRow drBankPaymentDetail in dtBankPaymentDetail.Rows)
                                    {                                       
                                        if (drBankPaymentDetail["ChequeNumber"] != "")
                                        {
                                            if (m_ChequeSettings.HasParty)
                                            {
                                                if (Convert.ToInt32(drBankPaymentDetail["LedgerID"]) == m_ChequeSettings.PartyID || m_ChequeSettings.PartyID == 0)
                                                {
                                                    WriteCheque(Sno, Date.DBToSystem(drBankPaymentDetail["ChequeDate"].ToString()), drBankPaymentDetail["ChequeNumber"].ToString(), dr["EngName"].ToString(), "", drBankPaymentDetail[0].ToString(), Convert.ToDecimal(drBankPaymentDetail["Amount"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), "BANK_PMNT", drBankTransaction["RowID"].ToString());
                                                    Sno++;
                                                }
                                            }
                                            else
                                            {
                                                WriteCheque(Sno, Date.DBToSystem(drBankPaymentDetail["ChequeDate"].ToString()), drBankPaymentDetail["ChequeNumber"].ToString(), dr["EngName"].ToString(), "", drBankPaymentDetail[0].ToString(), Convert.ToDecimal(drBankPaymentDetail["Amount"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), "BANK_PMNT", drBankTransaction["RowID"].ToString());
                                                Sno++;
                                            }
                                        }                                        
                                    }                                   
                                }
                            }
                        }
                    }
                }
            }
            #endregion                 

            #region BLOCK FOR CHEQUE RECEIVED
            
            //if (m_ChequeSettings.ChequeReceived == 2 || m_ChequeSettings.ChequeReceived ==0) //Display bank Receipt
            //{
            //    Transaction Transaction = new Transaction();
            //    int BankGroupID = AccountGroup.GetGroupIDFromGroupNumber(7); /////**************************** for bank only

            //    //Get all ledger under banks
            //    DataTable dt = AccountGroup.GetDetailLedgerID(BankGroupID, true); // Get all ledger ID and sub ledger IDs                           

            //    //int Sno = 1;
            //    foreach (DataRow dr in dt.Rows)
            //    {

            //        if (m_ChequeSettings.HasBank)
            //        {
            //            if (m_ChequeSettings.BankID != Convert.ToInt32(dr["LedgerID"])) // allows only selected bank
            //                continue;
            //        }
            //        DataTable dtBankTransaction = new DataTable();
            //        if (m_ChequeSettings.HasDateRange)
            //        {
            //           // GetLedgerTransactWithAccountClassAndProject
            //            //dtBankTransaction = Transaction.GetLedgerTransact(Convert.ToInt32(dr["LedgerID"]), m_ChequeSettings.FromDate, m_ChequeSettings.ToDate);//Get the Transact Inforamtion of corresponding LedgerID according to datewise
            //            dtBankTransaction = Transaction.GetLedgerTransactWithAccountClassAndProject(Convert.ToInt32(dr["LedgerID"]), m_ChequeSettings.FromDate, m_ChequeSettings.ToDate, m_ChequeSettings.AccClassID, m_ChequeSettings.ProjectID);//Get the Transact Inforamtion of corresponding LedgerID according to datewise
            //        }
            //        else
            //        {
            //            //dtBankTransaction = Transaction.GetLedgerTransact(Convert.ToInt32(dr["LedgerID"]), null, null);
            //            dtBankTransaction = Transaction.GetLedgerTransactWithAccountClassAndProject(Convert.ToInt32(dr["LedgerID"]), null, null, m_ChequeSettings.AccClassID, m_ChequeSettings.ProjectID);
            //        }

            //        foreach (DataRow drBankTransaction in dtBankTransaction.Rows)
            //        {
            //            AccClassID.Add(1);
            //            DataTable dtTransactInfo = new DataTable();
            //            dtTransactInfo = Transaction.GetTransactionInfoAlongWithParty(drBankTransaction["RowID"].ToString(), "BANK_RCPT", 0, AccClassID);
            //            for (int j = 0; j < dtTransactInfo.Rows.Count; j++)
            //            {
            //                DataRow drTransactInfo = dtTransactInfo.Rows[j];
            //                DataTable dtLedgerAllInfo = Ledger.GetLedgerInfo(Convert.ToInt32(drTransactInfo["LedgerID"]), LangMgr.DefaultLanguage);
            //                DataRow drLedgerAllInfo = dtLedgerAllInfo.Rows[0];

            //                BankReceipt m_BankReceipt = new BankReceipt();
            //                if (Convert.ToInt32(dr["LedgerID"]) == Convert.ToInt32(drTransactInfo["LedgerID"]))//This is the transaction done by specified Ledger hit by user to other ledgers except own
            //                {
            //                    DataTable dtVoucherName = new DataTable();
            //                    DataTable dtBankReceiptDetail = new DataTable();
            //                    dtVoucherName = m_BankReceipt.GetBankReceiptMaster(Convert.ToInt32(drBankTransaction["RowID"]));
            //                    if (dtVoucherName.Rows.Count > 0)
            //                    {
            //                        //Get bank payment detail from bank payment master using bankpaymentID
            //                        dtBankReceiptDetail = m_BankReceipt.GetBankReceiptDetail(Convert.ToInt32(drBankTransaction["RowID"]));
            //                        foreach (DataRow drBankPaymentDetail in dtBankReceiptDetail.Rows)
            //                        {
            //                            if (drBankPaymentDetail["ChequeNumber"] != "")
            //                            {
            //                                if (m_ChequeSettings.HasParty)
            //                                {
            //                                    if (Convert.ToInt32(drBankPaymentDetail["LedgerID"]) == m_ChequeSettings.PartyID || m_ChequeSettings.PartyID == 0)
            //                                    {
            //                                        WriteCheque(Sno, Date.DBToSystem(drBankPaymentDetail["ChequeDate"].ToString()), drBankPaymentDetail["ChequeNumber"].ToString(), "", dr["EngName"].ToString(), drBankPaymentDetail[0].ToString(), Convert.ToDecimal(drBankPaymentDetail["Amount"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), "BANK_RCPT", drBankTransaction["RowID"].ToString());
            //                                        Sno++;
            //                                    }
            //                                }
            //                                else
            //                                {
            //                                    WriteCheque(Sno, Date.DBToSystem(drBankPaymentDetail["ChequeDate"].ToString()), drBankPaymentDetail["ChequeNumber"].ToString(), "", dr["EngName"].ToString(), drBankPaymentDetail[0].ToString(), Convert.ToDecimal(drBankPaymentDetail["Amount"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), "BANK_RCPT", drBankTransaction["RowID"].ToString());
            //                                    Sno++;
            //                                }
            //                            }
            //                        }
            //                    }
            //                }
            //            }
            //        }
            //    }
            //}
            #endregion                 

            #region BLOCK FOR CHEQUE Receipt
            if (m_ChequeSettings.ChequeReceived == 2) //Display bank payments
            {
                if (m_ChequeSettings.Status == ChequeStatus.Uncleared)
                {
                    unclear();
                   // return;
                    
                }
                else if (m_ChequeSettings.Status == ChequeStatus.Cleared)
                {
                    clear();
                    //return;
                }
                else if (m_ChequeSettings.Status == ChequeStatus.Both)
                {
                    clear();
                    unclear();
                }
            }
            #endregion                 

        }

        private void given()
        {
            int Sno = 1;
                Transaction Transaction = new Transaction();
                int BankGroupID = AccountGroup.GetGroupIDFromGroupNumber(7); /////**************************** for bank only

                //Get all ledger under banks
                DataTable dt = AccountGroup.GetDetailLedgerID(BankGroupID, true); // Get all ledger ID and sub ledger IDs
                //Kist bank n himalayan bank in dt              

                //int Sno = 1;
                foreach (DataRow dr in dt.Rows)
                {

                    if (m_ChequeSettings.HasBank)
                    {
                        if (m_ChequeSettings.BankID != Convert.ToInt32(dr["LedgerID"])) // allows only selected bank
                            continue;
                    }
                    DataTable dtBankTransaction = new DataTable();
                    if (m_ChequeSettings.HasDateRange)
                    {
                        //dtBankTransaction = Transaction.GetLedgerTransact(Convert.ToInt32(dr["LedgerID"]),m_ChequeSettings.FromDate, m_ChequeSettings.ToDate);//Get the Transact Inforamtion of corresponding LedgerID according to datewise
                        dtBankTransaction = Transaction.GetLedgerTransactWithAccountClassAndProject(Convert.ToInt32(dr["LedgerID"]), m_ChequeSettings.FromDate, m_ChequeSettings.ToDate, m_ChequeSettings.AccClassID, m_ChequeSettings.ProjectID);//Get the Transact Inforamtion of corresponding LedgerID according to datewise
                    }
                    else
                    {
                        //dtBankTransaction = Transaction.GetLedgerTransact(Convert.ToInt32(dr["LedgerID"]), null, null);
                        dtBankTransaction = Transaction.GetLedgerTransactWithAccountClassAndProject(Convert.ToInt32(dr["LedgerID"]), null, null, m_ChequeSettings.AccClassID, m_ChequeSettings.ProjectID);
                    }

                    foreach (DataRow drBankTransaction in dtBankTransaction.Rows)
                    {
                        AccClassID.Add(1);
                        DataTable dtTransactInfo = new DataTable();
                        dtTransactInfo = Transaction.GetTransactionInfoAlongWithParty(drBankTransaction["RowID"].ToString(), "BANK_PMNT", 0, AccClassID);
                        for (int j = 0; j < dtTransactInfo.Rows.Count; j++)
                        {
                            DataRow drTransactInfo = dtTransactInfo.Rows[j];
                            DataTable dtLedgerAllInfo = Ledger.GetLedgerInfo(Convert.ToInt32(drTransactInfo["LedgerID"]), LangMgr.DefaultLanguage);
                            DataRow drLedgerAllInfo = dtLedgerAllInfo.Rows[0];

                            BankPayment m_BankPayment = new BankPayment();
                            if (Convert.ToInt32(dr["LedgerID"]) == Convert.ToInt32(drTransactInfo["LedgerID"]))//This is the transaction done by specified Ledger hit by user to other ledgers except own
                            {
                                DataTable dtVoucherName = new DataTable();
                                DataTable dtBankPaymentDetail = new DataTable();
                                dtVoucherName = m_BankPayment.GetBankPaymentMaster(Convert.ToInt32(drBankTransaction["RowID"]));
                                if (dtVoucherName.Rows.Count > 0)
                                {
                                    //Get bank payment detail from bank payment master using bankpaymentID
                                    dtBankPaymentDetail = m_BankPayment.GetBankPaymentDetail(Convert.ToInt32(drBankTransaction["RowID"]));
                                    foreach (DataRow drBankPaymentDetail in dtBankPaymentDetail.Rows)
                                    {
                                        if (drBankPaymentDetail["ChequeNumber"] != "")
                                        {
                                            if (m_ChequeSettings.HasParty)
                                            {
                                                if (Convert.ToInt32(drBankPaymentDetail["LedgerID"]) == m_ChequeSettings.PartyID || m_ChequeSettings.PartyID == 0)
                                                {
                                                    WriteCheque(Sno, Date.DBToSystem(drBankPaymentDetail["ChequeDate"].ToString()), drBankPaymentDetail["ChequeNumber"].ToString(), dr["EngName"].ToString(), "", drBankPaymentDetail[0].ToString(), Convert.ToDecimal(drBankPaymentDetail["Amount"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), "BANK_PMNT", drBankTransaction["RowID"].ToString());
                                                    Sno++;
                                                }
                                            }
                                            else
                                            {
                                                WriteCheque(Sno, Date.DBToSystem(drBankPaymentDetail["ChequeDate"].ToString()), drBankPaymentDetail["ChequeNumber"].ToString(), dr["EngName"].ToString(), "", drBankPaymentDetail[0].ToString(), Convert.ToDecimal(drBankPaymentDetail["Amount"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), "BANK_PMNT", drBankTransaction["RowID"].ToString());
                                                Sno++;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            
        }
        private void grdCheque_DoubleClick(object sender, EventArgs e)
        {           
            try
            {
                //Get the Selected Row
                int CurRow = grdCheque.Selection.GetSelectionRegion().GetRowsIndex()[0];
                SourceGrid.CellContext cellType = new SourceGrid.CellContext(grdCheque, new SourceGrid.Position(CurRow, 7));
                SourceGrid.CellContext RowIDFromCell = new SourceGrid.CellContext(grdCheque, new SourceGrid.Position(CurRow, 8));
                int RowID = Convert.ToInt32(RowIDFromCell.Value);
                string VoucherType = (cellType.Value).ToString();

                switch (VoucherType)
                {
                    case "BANK_PMNT":
                        frmBankPayment frm5 = new frmBankPayment(RowID);
                        frm5.Show();
                        break;
                    case "BANK_RCPT":
                        frmBankReceipt frm6 = new frmBankReceipt(RowID);
                        frm6.Show();
                        break;
                }
            }
            catch (Exception ex)
            {
                Global.Msg(ex.Message);
            }
        }

        private void WriteCheque(int Sno, string ChequeDate, string ChequeNumber, string ChequeBank, string DepositedBank, string Bearer, string Amount, string VoucherType,string RowID)
        {                          
            grdCheque.Rows.Insert(Sno);
            SourceGrid.Cells.Views.Cell alternate = new SourceGrid.Cells.Views.Cell();
            if (Sno % 2 == 0)
            {
                //alternate.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Color.LightCoral);
            }
            else
            {
                alternate.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Global.Grid_Color);
            }

            grdCheque[Sno, 0] = new SourceGrid.Cells.Cell(Sno.ToString());           
            grdCheque[Sno, 1] = new SourceGrid.Cells.Cell(ChequeDate);
            grdCheque[Sno, 2] = new SourceGrid.Cells.Cell(ChequeNumber);           
            grdCheque[Sno, 3] = new SourceGrid.Cells.Cell(ChequeBank);           
            grdCheque[Sno, 4] = new SourceGrid.Cells.Cell(DepositedBank);
            grdCheque[Sno, 5] = new SourceGrid.Cells.Cell(Bearer);
            grdCheque[Sno, 6] = new SourceGrid.Cells.Cell(Amount);           
            grdCheque[Sno, 7] = new SourceGrid.Cells.Cell(VoucherType);
            grdCheque[Sno, 8] = new SourceGrid.Cells.Cell(RowID);

            grdCheque[Sno, 0].AddController(dblClick);
            grdCheque[Sno, 0].View = new SourceGrid.Cells.Views.Cell(alternate);
            grdCheque[Sno, 1].AddController(dblClick);

            grdCheque[Sno, 1].View = new SourceGrid.Cells.Views.Cell(alternate);
            grdCheque[Sno, 2].AddController(dblClick);
            grdCheque[Sno, 2].View = new SourceGrid.Cells.Views.Cell(alternate);
            grdCheque[Sno, 3].AddController(dblClick);
            grdCheque[Sno, 3].View = new SourceGrid.Cells.Views.Cell(alternate);
            grdCheque[Sno, 4].AddController(dblClick);
            grdCheque[Sno, 4].View = new SourceGrid.Cells.Views.Cell(alternate);
            grdCheque[Sno, 5].AddController(dblClick);
            grdCheque[Sno, 5].View = new SourceGrid.Cells.Views.Cell(alternate);
            grdCheque[Sno, 6].AddController(dblClick);
            grdCheque[Sno, 6].View = new SourceGrid.Cells.Views.Cell(alternate);
            grdCheque[Sno, 7].AddController(dblClick);
            grdCheque[Sno, 7].View = new SourceGrid.Cells.Views.Cell(alternate);
            grdCheque[Sno, 8].AddController(dblClick);

            //This is for crystal reports
            dsCheque.Tables["ListCheques"].Rows.Add(Sno,ChequeDate,ChequeNumber, ChequeBank,DepositedBank,Bearer,Amount,VoucherType);

            Sno++;
        }           

        private void DisplayBannar()
        {
            CompanyDetails m_CompanyDetails = CompanyInfo.GetInfo();
            lblCompanyName.Text = m_CompanyDetails.CompanyName;
            lblCompanyAddress.Text = m_CompanyDetails.Address1 + " " + m_CompanyDetails.City + " " + m_CompanyDetails.District;
            lblContact.Text = "Contact: " + m_CompanyDetails.Telephone + "Website: " + m_CompanyDetails.Website;
            lblPanNo.Text = "PAN No.: " + m_CompanyDetails.PAN;          
        }

        //Customized header
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

        //Writes the header on the grid
        private void MakeHeader()
        {
            //Write header part
            grdCheque.Rows.Insert(0);
            grdCheque[0, 0] = new MyHeader("S.No.");
            grdCheque[0, 1] = new MyHeader("Date");
            grdCheque[0, 2] = new MyHeader("Cheque #");
            grdCheque[0, 3] = new MyHeader("Cheque Bank");
            grdCheque[0, 4] = new MyHeader("Deposited Bank");
            grdCheque[0, 5] = new MyHeader("Bearer");           
            grdCheque[0, 6] = new MyHeader("Amount");
            grdCheque[0, 7] = new MyHeader("Voucher Type");
            grdCheque[0, 8] = new MyHeader("RowID");

            //Define the width of column size
            grdCheque[0, 0].Column.Width = 40;
            grdCheque[0, 1].Column.Width = 75;
            grdCheque[0, 2].Column.Width = 90;
            grdCheque[0, 3].Column.Width = 150;
            grdCheque[0, 4].Column.Width = 140;
            grdCheque[0, 5].Column.Width = 120;
            grdCheque[0, 6].Column.Width = 90;
            grdCheque[0, 7].Column.Width = 90;
            grdCheque[0, 8].Column.Width = 0;
            //Code for making column invisible
            //grdCheque.Columns[4].Visible = false;// making third column invisible and using it in  programming     
            grdCheque.Columns[8].Visible = false;
        }

        private void grdCheque_DoubleClick_1(object sender, EventArgs e)
        {
           
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            Menu_Export = new ContextMenu();
            MenuItem mnuExcel = new MenuItem();
            mnuExcel.Name = "mnuExcel";
            mnuExcel.Text = "E&xcel";
            MenuItem mnuPDF = new MenuItem();
            mnuPDF.Name = "mnuPDF";
            mnuPDF.Text = "&PDF";
            MenuItem mnuEmail = new MenuItem();
            mnuEmail.Name = "mnuEmail";
            mnuEmail.Text = "E&mail";
            Menu_Export.MenuItems.Add(mnuEmail);
            Menu_Export.MenuItems.Add(mnuExcel);
            Menu_Export.MenuItems.Add(mnuPDF);
            Menu_Export.Show(btnExport, new Point(0, btnExport.Height));

            foreach (MenuItem Item in Menu_Export.MenuItems)
                Item.Click += new EventHandler(Menu_Click);
        }

        private void Menu_Click(object sender, EventArgs e)
        {
            switch (((MenuItem)sender).Name)
            {
                case "mnuExcel":
                    //Code for excel export
                    SaveFileDialog SaveFD = new SaveFileDialog();
                    SaveFD.InitialDirectory = "D:";
                    SaveFD.Title = "Enter Filename:";
                    SaveFD.Filter = "*.xls|*.xls";
                    if (SaveFD.ShowDialog() != DialogResult.Cancel)
                    {
                        string FileToRestore = SaveFD.FileName;
                        FileName = SaveFD.FileName;
                    }
                    else
                    {
                        return;
                    }
                    prntDirect = 2;
                    btnPrintPreview_Click(sender, e);
                    break;
                case "mnuPDF":
                    //Code for pdf export
                    SaveFileDialog SaveFDPdf = new SaveFileDialog();
                    SaveFDPdf.InitialDirectory = "D:";
                    SaveFDPdf.Title = "Enter Filename:";
                    SaveFDPdf.Filter = "*.pdf|*.pdf";
                    if (SaveFDPdf.ShowDialog() != DialogResult.Cancel)
                    {
                        string FileToRestore = SaveFDPdf.FileName;
                        FileName = SaveFDPdf.FileName;
                    }
                    else
                    {
                        return;
                    }
                    prntDirect = 3;
                    btnPrintPreview_Click(sender, e);
                    break;
                case "mnuEmail":
                    //Code for pdf export
                    SaveFileDialog SaveFDExcelEmail = new SaveFileDialog();
                    SaveFDExcelEmail.InitialDirectory = "D:";
                    SaveFDExcelEmail.Title = "Enter Filename:";
                    SaveFDExcelEmail.Filter = "*.xls|*.xls"; ;
                    if (SaveFDExcelEmail.ShowDialog() != DialogResult.Cancel)
                    {
                        string FileToRestore = SaveFDExcelEmail.FileName;
                        FileName = SaveFDExcelEmail.FileName;
                    }
                    else
                    {
                        return;
                    }
                    prntDirect = 4;
                    btnPrintPreview_Click(sender, e);
                    break;
            }
        }

        private void btnPrintPreview_Click(object sender, EventArgs e)
        {
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

           // dsCheque.Clear();//It clear the previous records of dataset on crystal report...when this button is pressed
            //otherwise it populate the records again and again 
            
            rptCheque rpt = new rptCheque();
            //Fill the logo on the report
            //Misc.WriteLogo(dsTrial, "tblImage");
            //Set DataSource to be dsTrial dataset on the report
            rpt.SetDataSource(dsCheque);
            //Provide values to the parameters on the report
            CrystalDecisions.Shared.ParameterValues pvCollection = new CrystalDecisions.Shared.ParameterValues();
            CrystalDecisions.Shared.ParameterDiscreteValue pdvCompany_Name = new CrystalDecisions.Shared.ParameterDiscreteValue();
            CrystalDecisions.Shared.ParameterDiscreteValue pdvCompany_Address = new CrystalDecisions.Shared.ParameterDiscreteValue();
            CrystalDecisions.Shared.ParameterDiscreteValue pdvCompany_Phone = new CrystalDecisions.Shared.ParameterDiscreteValue();
            CrystalDecisions.Shared.ParameterDiscreteValue pdvCompany_PAN = new CrystalDecisions.Shared.ParameterDiscreteValue();
            CrystalDecisions.Shared.ParameterDiscreteValue pdvCompany_Slogan = new CrystalDecisions.Shared.ParameterDiscreteValue();
            CrystalDecisions.Shared.ParameterDiscreteValue pdvReport_Head = new CrystalDecisions.Shared.ParameterDiscreteValue();
            CrystalDecisions.Shared.ParameterDiscreteValue pdvFiscal_Year = new CrystalDecisions.Shared.ParameterDiscreteValue();
            CrystalDecisions.Shared.ParameterDiscreteValue pdvReport_Date = new CrystalDecisions.Shared.ParameterDiscreteValue();

            CrystalDecisions.Shared.ParameterDiscreteValue pdvFont = new CrystalDecisions.Shared.ParameterDiscreteValue();

            //Update the progressbar
            ProgressForm.UpdateProgress(50, "Initializing Report Viewer...");

            pdvFont.Value = "Arial";
            pvCollection.Clear();
            pvCollection.Add(pdvFont);
            rpt.DataDefinition.ParameterFields["Font"].ApplyCurrentValues(pvCollection);

            CompanyDetails m_CompanyDetails = CompanyInfo.GetInfo();

            pdvCompany_Name.Value = m_CompanyDetails.CompanyName;
            pvCollection.Clear();
            pvCollection.Add(pdvCompany_Name);
            rpt.DataDefinition.ParameterFields["Company_Name"].ApplyCurrentValues(pvCollection);

            pdvCompany_Address.Value = m_CompanyDetails.Address1 + ((m_CompanyDetails.Address1.Trim().Length > 0) && (m_CompanyDetails.City.Trim().Length > 0) ? ", " : "") + m_CompanyDetails.City; //Display comma in the middle only if both are available
            pvCollection.Clear();
            pvCollection.Add(pdvCompany_Address);
            rpt.DataDefinition.ParameterFields["Company_Address"].ApplyCurrentValues(pvCollection);

            pdvCompany_PAN.Value = m_CompanyDetails.PAN;
            pvCollection.Clear();
            pvCollection.Add(pdvCompany_PAN);
            rpt.DataDefinition.ParameterFields["Company_PAN"].ApplyCurrentValues(pvCollection);

            pdvCompany_Phone.Value = "Phone No.: " + m_CompanyDetails.Telephone;
            pvCollection.Clear();
            pvCollection.Add(pdvCompany_Phone);
            rpt.DataDefinition.ParameterFields["Company_Phone"].ApplyCurrentValues(pvCollection);

            pdvCompany_Slogan.Value = m_CompanyDetails.Website;
            pvCollection.Clear();
            pvCollection.Add(pdvCompany_Slogan);
            rpt.DataDefinition.ParameterFields["Company_Slogan"].ApplyCurrentValues(pvCollection);

            pdvReport_Head.Value = "Cheque Report";
            pvCollection.Clear();
            pvCollection.Add(pdvReport_Head);
            rpt.DataDefinition.ParameterFields["Report_Head"].ApplyCurrentValues(pvCollection);
            //if (m_CompanyDetails.FYFrom != null)
            //    pdvFiscal_Year.Value = "Fiscal Year:" + Date.ToSystem(Convert.ToDateTime(m_CompanyDetails.FYFrom));

            //pvCollection.Clear();
            //pvCollection.Add(pdvFiscal_Year);
            //rpt.DataDefinition.ParameterFields["Fiscal_Year"].ApplyCurrentValues(pvCollection);

            //Update the progressbar
            ProgressForm.UpdateProgress(80, "Calculating Parameters...");

            //if (m_TBS.FromDate != null && m_TBS.ToDate != null)
            //{
            //    pdvReport_Date.Value = "From: " + Date.ToSystem(Convert.ToDateTime(m_TBS.FromDate)) + " To: " + Date.ToSystem(Convert.ToDateTime(m_TBS.ToDate));
            //}
            //if (m_TBS.ToDate != null)
            //{
            //    pdvReport_Date.Value = "As on Date: " + Date.ToSystem(Convert.ToDateTime(m_TBS.ToDate));
            //}
            //if (m_TBS.FromDate != null)
            //{
            //    //This is actually not applicable
            //    pdvReport_Date.Value = "From: " + Date.ToSystem(Convert.ToDateTime(m_TBS.FromDate));
            //}

            //pvCollection.Clear();
            //pvCollection.Add(pdvReport_Date);
            //rpt.DataDefinition.ParameterFields["Report_Date"].ApplyCurrentValues(pvCollection);

           // DisplayTrialBlance(true);


            CrystalDecisions.Shared.ExportOptions CrExportOptions = new CrystalDecisions.Shared.ExportOptions();
            DiskFileDestinationOptions CrDiskFileDestinationOptions = new DiskFileDestinationOptions();
            CrDiskFileDestinationOptions.DiskFileName = FileName;
            //rpt.Export(opt);
            //return;
            //Finally, show the report form
            frmReportViewer frm = new frmReportViewer();
            frm.SetReportSource(rpt);

            //Update the progressbar
            ProgressForm.UpdateProgress(100, "Showing Report...");

            // Close the dialog
            ProgressForm.CloseForm();

            switch (prntDirect)
            {
                case 1:
                    rpt.PrintOptions.PrinterName = "";
                    rpt.PrintToPrinter(1, false, 0, 0);
                    prntDirect = 0;
                    return;
                case 2:
                    ExcelFormatOptions CrFormatTypeOptions = new ExcelFormatOptions();
                    CrExportOptions = rpt.ExportOptions;
                    CrExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                    CrExportOptions.ExportFormatType = ExportFormatType.Excel;
                    CrExportOptions.DestinationOptions = CrDiskFileDestinationOptions;
                    CrExportOptions.FormatOptions = CrFormatTypeOptions;
                    rpt.Export();
                    rpt.Close();
                    return;
                case 3:
                    PdfFormatOptions CrFormatTypeOptionsPdf = new PdfFormatOptions();
                    CrExportOptions = rpt.ExportOptions;
                    CrExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                    CrExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                    CrExportOptions.DestinationOptions = CrDiskFileDestinationOptions;
                    CrExportOptions.FormatOptions = CrFormatTypeOptionsPdf;
                    rpt.Export();
                    rpt.Close();
                    return;
                case 4:
                    ExcelFormatOptions CrFormatTypeOptionsEmail = new ExcelFormatOptions();
                    CrExportOptions = rpt.ExportOptions;
                    CrExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                    CrExportOptions.ExportFormatType = ExportFormatType.Excel;
                    CrExportOptions.DestinationOptions = CrDiskFileDestinationOptions;
                    CrExportOptions.FormatOptions = CrFormatTypeOptionsEmail;
                    rpt.Export();
                    frmemail sendemail = new frmemail(FileName, 1);
                    sendemail.ShowDialog();
                    rpt.Close();
                    return;
                default:
                    frm.Show();
                    frm.WindowState = FormWindowState.Maximized;
                    break;
            }

            this.Cursor = Cursors.Default;
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            //Clear all rows
            grdCheque.Redim(0, 0);
            this.Cursor = Cursors.WaitCursor;
            //Load all over again
            frmChequeReport_Load(sender, e);
            this.Cursor = Cursors.Default;
        }

        private void unclear()
        {
            #region uncleared
            
                int Sno = 1;
                BusinessLogic.Accounting.ChequeReceipt chequerec = new BusinessLogic.Accounting.ChequeReceipt();



                DataTable dt = chequerec.GetLedgerID();
                //Kist bank n himalayan bank in dt             

                //int Sno = 1;
                foreach (DataRow dr in dt.Rows)
                {

                    if (m_ChequeSettings.HasBank)
                    {
                        if (m_ChequeSettings.BankID != Convert.ToInt32(dr["LedgerID"])) // allows only selected bank
                            continue;
                    }
                    DataTable dtChequeTransaction = new DataTable();
                    if (m_ChequeSettings.HasDateRange)
                    {
                        //dtBankTransaction = Transaction.GetLedgerTransact(Convert.ToInt32(dr["LedgerID"]),m_ChequeSettings.FromDate, m_ChequeSettings.ToDate);//Get the Transact Inforamtion of corresponding LedgerID according to datewise
                        dtChequeTransaction = chequerec.GetLedgerTransactWithAccountClassAndProject(Convert.ToInt32(dr["LedgerID"]), m_ChequeSettings.FromDate, m_ChequeSettings.ToDate, m_ChequeSettings.AccClassID);//Get the Transact Inforamtion of corresponding LedgerID according to datewise
                    }
                    else
                    {
                        //dtBankTransaction = Transaction.GetLedgerTransact(Convert.ToInt32(dr["LedgerID"]), null, null);
                        dtChequeTransaction = chequerec.GetLedgerTransactWithAccountClassAndProject(Convert.ToInt32(dr["LedgerID"]), null, null, m_ChequeSettings.AccClassID);
                    }

                    foreach (DataRow drChequeTransaction in dtChequeTransaction.Rows)
                    {
                        AccClassID.Add(1);
                        DataTable dtChequeTransactInfo = new DataTable();
                        dtChequeTransactInfo = chequerec.GetLedgerID();
                        for (int j = 0; j < dtChequeTransactInfo.Rows.Count; j++)
                        {
                            DataRow drTransactInfo = dtChequeTransactInfo.Rows[j];
                            DataTable dtLedgerAllInfo = chequerec.GetVouch(Convert.ToInt32(drTransactInfo["ChequeReceiptID"]));
                            DataRow drLedgerAllInfo = dtLedgerAllInfo.Rows[0];


                            if (Convert.ToInt32(dr["ChequeReceiptID"]) == Convert.ToInt32(drTransactInfo["ChequeReceiptID"]))//This is the transaction done by specified Ledger hit by user to other ledgers except own
                            {
                                DataTable dtVoucherName = new DataTable();
                                DataTable dtBankPaymentDetail = new DataTable();
                                dtVoucherName = chequerec.GetLedgerID();
                                if (dtVoucherName.Rows.Count > 0)
                                {
                                    //Get bank payment detail from bank payment master using bankpaymentID
                                    dtBankPaymentDetail = chequerec.GetChequeReceiptDetail(Convert.ToInt32(drTransactInfo["ChequeReceiptID"]));
                                    foreach (DataRow drBankPaymentDetail in dtBankPaymentDetail.Rows)
                                    {
                                        if (drBankPaymentDetail["ChequeNumber"] != "")
                                        {
                                            if (m_ChequeSettings.HasParty)
                                            {
                                                if (Convert.ToInt32(drBankPaymentDetail["LedgerID"]) == m_ChequeSettings.PartyID || m_ChequeSettings.PartyID == 0)
                                                {
                                                    WriteCheque(Sno, Date.DBToSystem(drBankPaymentDetail["ChequeDate"].ToString()), drBankPaymentDetail["ChequeNumber"].ToString(), dr["chequeBank"].ToString(), "", drBankPaymentDetail[0].ToString(), Convert.ToDecimal(drBankPaymentDetail["Amount"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), "CHQ_PMNT", drChequeTransaction["ChequeReceiptID"].ToString());
                                                    Sno++;
                                                }
                                            }
                                            else
                                            {
                                                WriteCheque(Sno, Date.DBToSystem(drBankPaymentDetail["ChequeDate"].ToString()), drBankPaymentDetail["ChequeNumber"].ToString(), dr["chequeBank"].ToString(), "", drBankPaymentDetail[0].ToString(), Convert.ToDecimal(drBankPaymentDetail["Amount"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), "CHQ_PMNT", drChequeTransaction["ChequeReceiptID"].ToString());
                                                Sno++;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            
            #endregion
        }
        private void clear()
        {
            #region BLOCK FOR CHEQUE RECEIVED

                int Sno = 1;
                Transaction Transaction = new Transaction();
                int BankGroupID = AccountGroup.GetGroupIDFromGroupNumber(7); /////**************************** for bank only

                //Get all ledger under banks
                DataTable dt = AccountGroup.GetDetailLedgerID(BankGroupID, true); // Get all ledger ID and sub ledger IDs                           

                //int Sno = 1;
                foreach (DataRow dr in dt.Rows)
                {

                    if (m_ChequeSettings.HasBank)
                    {
                        if (m_ChequeSettings.BankID != Convert.ToInt32(dr["LedgerID"])) // allows only selected bank
                            continue;
                    }
                    DataTable dtBankTransaction = new DataTable();
                    if (m_ChequeSettings.HasDateRange)
                    {
                        // GetLedgerTransactWithAccountClassAndProject
                        //dtBankTransaction = Transaction.GetLedgerTransact(Convert.ToInt32(dr["LedgerID"]), m_ChequeSettings.FromDate, m_ChequeSettings.ToDate);//Get the Transact Inforamtion of corresponding LedgerID according to datewise
                        dtBankTransaction = Transaction.GetLedgerTransactWithAccountClassAndProject(Convert.ToInt32(dr["LedgerID"]), m_ChequeSettings.FromDate, m_ChequeSettings.ToDate, m_ChequeSettings.AccClassID, m_ChequeSettings.ProjectID);//Get the Transact Inforamtion of corresponding LedgerID according to datewise
                    }
                    else
                    {
                        //dtBankTransaction = Transaction.GetLedgerTransact(Convert.ToInt32(dr["LedgerID"]), null, null);
                        dtBankTransaction = Transaction.GetLedgerTransactWithAccountClassAndProject(Convert.ToInt32(dr["LedgerID"]), null, null, m_ChequeSettings.AccClassID, m_ChequeSettings.ProjectID);
                    }

                    foreach (DataRow drBankTransaction in dtBankTransaction.Rows)
                    {
                        AccClassID.Add(1);
                        DataTable dtTransactInfo = new DataTable();
                        dtTransactInfo = Transaction.GetTransactionInfoAlongWithParty(drBankTransaction["RowID"].ToString(), "BANK_RCPT", 0, AccClassID);
                        for (int j = 0; j < dtTransactInfo.Rows.Count; j++)
                        {
                            DataRow drTransactInfo = dtTransactInfo.Rows[j];
                            DataTable dtLedgerAllInfo = Ledger.GetLedgerInfo(Convert.ToInt32(drTransactInfo["LedgerID"]), LangMgr.DefaultLanguage);
                            DataRow drLedgerAllInfo = dtLedgerAllInfo.Rows[0];

                            BankReceipt m_BankReceipt = new BankReceipt();
                            if (Convert.ToInt32(dr["LedgerID"]) == Convert.ToInt32(drTransactInfo["LedgerID"]))//This is the transaction done by specified Ledger hit by user to other ledgers except own
                            {
                                DataTable dtVoucherName = new DataTable();
                                DataTable dtBankReceiptDetail = new DataTable();
                                dtVoucherName = m_BankReceipt.GetBankReceiptMaster(Convert.ToInt32(drBankTransaction["RowID"]));
                                if (dtVoucherName.Rows.Count > 0)
                                {
                                    //Get bank payment detail from bank payment master using bankpaymentID
                                    dtBankReceiptDetail = m_BankReceipt.GetBankReceiptDetail(Convert.ToInt32(drBankTransaction["RowID"]));
                                    foreach (DataRow drBankPaymentDetail in dtBankReceiptDetail.Rows)
                                    {
                                        if (drBankPaymentDetail["ChequeNumber"] != "")
                                        {
                                            if (m_ChequeSettings.HasParty)
                                            {
                                                if (Convert.ToInt32(drBankPaymentDetail["LedgerID"]) == m_ChequeSettings.PartyID || m_ChequeSettings.PartyID == 0)
                                                {
                                                    WriteCheque(Sno, Date.DBToSystem(drBankPaymentDetail["ChequeDate"].ToString()), drBankPaymentDetail["ChequeNumber"].ToString(), "", dr["EngName"].ToString(), drBankPaymentDetail[0].ToString(), Convert.ToDecimal(drBankPaymentDetail["Amount"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), "BANK_RCPT", drBankTransaction["RowID"].ToString());
                                                    Sno++;
                                                }
                                            }
                                            else
                                            {
                                                WriteCheque(Sno, Date.DBToSystem(drBankPaymentDetail["ChequeDate"].ToString()), drBankPaymentDetail["ChequeNumber"].ToString(), "", dr["EngName"].ToString(), drBankPaymentDetail[0].ToString(), Convert.ToDecimal(drBankPaymentDetail["Amount"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), "BANK_RCPT", drBankTransaction["RowID"].ToString());
                                                Sno++;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            
            #endregion                 
        }
        }
    }

