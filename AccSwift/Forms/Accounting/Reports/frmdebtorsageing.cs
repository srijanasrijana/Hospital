using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BusinessLogic.Accounting;
using System.Threading;
using DateManager;
using BusinessLogic;
using System.Collections;
using CrystalDecisions.Shared;
using Inventory.CrystalReports;

namespace Inventory.Forms.Accounting.Reports
{
    public partial class frmdebtorsageing : Form
    {
        private int prntDirect = 0;
        string FileName = " ";
        //For Export Menu
        ContextMenu Menu_Export;
        DebtorsSettings m_DS = new DebtorsSettings();
        private DataSet.dsDebtorsAeging dsDebtorsAeging =new DataSet.dsDebtorsAeging();
        private DataSet.dsBillsWiseAeging dsBillWiseAging = new DataSet.dsBillsWiseAeging();
        private SourceGrid.Cells.Views.Cell LedgerView;
        private SourceGrid.Cells.Views.Cell LedgerHeadView;
        private SourceGrid.Cells.Views.Cell TotalView;
        private int DebtorsRowsCount = 0;
        
        double DebitSum = 0;
        double CreditSum = 0;
        ArrayList AccClassID = new ArrayList();
        public frmdebtorsageing()
        {
            InitializeComponent();
        }

        public frmdebtorsageing(DebtorsSettings ds)
        {
            m_DS = ds;
            InitializeComponent();
        }

        private void frmdebtorsageing_Load(object sender, EventArgs e)
        {
            DisplayDebtorsAgeing(false);
        }
        private void DisplayDebtorsAgeing(bool IsCrystalReport)
        {
            string AccClassIDsXMLString = ReadAllAccClassID();
            string ProjectIDsXMLString = ReadAllProjectID();
            frmProgress ProgressForm = new frmProgress();
            // Initialize the thread that will handle the background process
            Thread backgroundThread = new Thread(
                new ThreadStart(() =>
                {

                    ProgressForm.ShowDialog();
                }
            ));

            backgroundThread.Start();

            ProgressForm.UpdateProgress(20, "Initializing report...");
            if (!IsCrystalReport)//Need only for writting on grid...not for crystal reports
            {
                DisplayBanner();
                //Let the whole row to be selected
                grddebtorsageing.SelectionMode = SourceGrid.GridSelectionMode.Row;
                //Set the border width of the selection to thin
 

                //Disable multiple selection
                grddebtorsageing.Selection.EnableMultiSelection = false;
                if(m_DS.Ageing==true)
                    grddebtorsageing.Redim(1, 9);
                else if(m_DS.BillwiseAgeing==true)
                    grddebtorsageing.Redim(1, 12);
                grddebtorsageing.FixedRows = 1;
                //int rows = grddebtorsageing.Rows.Count;
                MakeHeader();
                ProgressForm.UpdateProgress(40, "Initializing report...");
            }
                #region if Ageing Checked
                if (m_DS.Ageing == true)
                {
                    #region For Particular Debtors ONly
                    if (m_DS.ShowAllDebtores == false)
                    {

                        double FirstPeriod1 = 0;
                        double SecondPeriod1 = 0;
                        double ThirdPeriod1 = 0;
                        double FourthPeriod1 = 0;
                        double GreaterThanFouthPeriod1 = 0;
                        int LedgerID1 = m_DS.DebtorsID;
                        DataTable dtLedgerInfo1 = Ledger.GetAllLedgerDetailsByIDForAgeing(LedgerID1);
                        DataRow drledgerinfo1 = dtLedgerInfo1.Rows[0];
                        string ledgercode1 = drledgerinfo1["LedgerCode"].ToString();
                        string ledgername1 = drledgerinfo1["EngName"].ToString();
                        DataTable dt1 = Transaction.GetLedgerTransaction(LedgerID1, AccClassIDsXMLString, m_DS.FromDate, m_DS.ToDate, ProjectIDsXMLString);
                        foreach (DataRow drledg in dt1.Rows)
                        {
                            double ReceiptAmount = 0;
                            if (drledg["VoucherType"].ToString() == "SALES")
                            {
                                DataTable dtDebtorReceipt = Debtors.GetCashReceiptDetails1("SALES", drledg["VoucherNumber"].ToString(), LedgerID1);
                                if (dtDebtorReceipt.Rows.Count > 0)
                                {
                                    DataRow drcashreceipt = dtDebtorReceipt.Rows[0];
                                    ReceiptAmount =ReceiptAmount+ Convert.ToDouble(drcashreceipt["Amount"].ToString());
                                }
                                DataTable dtDebtorBankReceipt = Debtors.GetBankReceiptDetails1("SALES", drledg["VoucherNumber"].ToString(), LedgerID1);
                                if (dtDebtorBankReceipt.Rows.Count > 0)
                                {
                                    DataRow drbankreceipt = dtDebtorBankReceipt.Rows[0];
                                    ReceiptAmount = ReceiptAmount + Convert.ToDouble(drbankreceipt["Amount"].ToString());
                                }
                            }
                            if (drledg["VoucherType"].ToString() == "JRNL")
                            {
                                DataTable dtDebtorReceipt = Debtors.GetCashReceiptDetails1("JRNL", drledg["VoucherNumber"].ToString(), LedgerID1);
                                if (dtDebtorReceipt.Rows.Count > 0)
                                {
                                    DataRow drcashreceipt = dtDebtorReceipt.Rows[0];
                                    ReceiptAmount = ReceiptAmount + Convert.ToDouble(drcashreceipt["Amount"].ToString());
                                }
                                DataTable dtDebtorBankReceipt = Debtors.GetBankReceiptDetails1("JRNL", drledg["VoucherNumber"].ToString(), LedgerID1);
                                if (dtDebtorBankReceipt.Rows.Count > 0)
                                {
                                    DataRow drbankreceipt = dtDebtorBankReceipt.Rows[0];
                                    ReceiptAmount = ReceiptAmount + Convert.ToDouble(drbankreceipt["Amount"].ToString());
                                }
                            }
                            if (drledg["VoucherType"].ToString() == "CASH_PMNT")
                            {
                                DataTable dtDebtorReceipt = Debtors.GetCashReceiptDetails1("CASH_PMNT", drledg["VoucherNumber"].ToString(), LedgerID1);
                                if (dtDebtorReceipt.Rows.Count > 0)
                                {
                                    DataRow drcashreceipt = dtDebtorReceipt.Rows[0];
                                    ReceiptAmount = ReceiptAmount + Convert.ToDouble(drcashreceipt["Amount"].ToString());
                                }
                                DataTable dtDebtorBankReceipt = Debtors.GetBankReceiptDetails1("CASH_PMNT", drledg["VoucherNumber"].ToString(), LedgerID1);
                                if (dtDebtorBankReceipt.Rows.Count > 0)
                                {
                                    DataRow drbankreceipt = dtDebtorBankReceipt.Rows[0];
                                    ReceiptAmount = ReceiptAmount + Convert.ToDouble(drbankreceipt["Amount"].ToString());
                                }
                            }
                            if (drledg["VoucherType"].ToString() == "BANK_PMNT")
                            {
                                DataTable dtDebtorReceipt = Debtors.GetCashReceiptDetails1("BANK_PMNT", drledg["VoucherNumber"].ToString(), LedgerID1);
                                if (dtDebtorReceipt.Rows.Count > 0)
                                {
                                    DataRow drcashreceipt = dtDebtorReceipt.Rows[0];
                                    ReceiptAmount = ReceiptAmount + Convert.ToDouble(drcashreceipt["Amount"].ToString());
                                }
                                DataTable dtDebtorBankReceipt = Debtors.GetBankReceiptDetails1("BANK_PMNT", drledg["VoucherNumber"].ToString(), LedgerID1);
                                if (dtDebtorBankReceipt.Rows.Count > 0)
                                {
                                    DataRow drbankreceipt = dtDebtorBankReceipt.Rows[0];
                                    ReceiptAmount = ReceiptAmount + Convert.ToDouble(drbankreceipt["Amount"].ToString());
                                }
                            }
                            DateTime TodaysDate = System.Convert.ToDateTime(Date.GetServerDate());
                            DateTime LedgerDate = System.Convert.ToDateTime(drledg["LedgerDate"].ToString());
                            TimeSpan servicePeriod = TodaysDate - System.Convert.ToDateTime(drledg["LedgerDate"].ToString());
                            double Period = servicePeriod.TotalDays;
                            if (Period < m_DS.FirstPeriod)
                            {
                                if (Convert.ToDouble(drledg["Debit"].ToString()) > 0)
                                {
                                    FirstPeriod1 += Convert.ToDouble(drledg["Debit"].ToString()) - ReceiptAmount;
                                }
                                //if (Convert.ToDouble(drledg["Credit"].ToString()) > 0)
                                //{
                                //    FirstPeriod1 -= Convert.ToDouble(drledg["Credit"].ToString());
                                //}
                            }
                            if (Period >= m_DS.FirstPeriod && Period < m_DS.SecondPeriod)
                            {
                                if (Convert.ToDouble(drledg["Debit"].ToString()) > 0)
                                {
                                    SecondPeriod1 += Convert.ToDouble(drledg["Debit"].ToString()) - ReceiptAmount;
                                }
                                //if (Convert.ToDouble(drledg["Credit"].ToString()) > 0)
                                //{
                                //    SecondPeriod1 -= Convert.ToDouble(drledg["Credit"].ToString());
                                //}
                            }
                            if (Period >= m_DS.SecondPeriod && Period < m_DS.ThirdPeriod)
                            {
                                if (Convert.ToDouble(drledg["Debit"].ToString()) > 0)
                                {
                                    ThirdPeriod1 += Convert.ToDouble(drledg["Debit"].ToString()) - ReceiptAmount;
                                }
                                //if (Convert.ToDouble(drledg["Credit"].ToString()) > 0)
                                //{
                                //    ThirdPeriod1 -= Convert.ToDouble(drledg["Credit"].ToString());
                                //}
                            }
                            if (Period >= m_DS.ThirdPeriod && Period < m_DS.FourthPeriod)
                            {
                                if (Convert.ToDouble(drledg["Debit"].ToString()) > 0)
                                {
                                    FourthPeriod1 += Convert.ToDouble(drledg["Debit"].ToString()) - ReceiptAmount;
                                }
                                //if (Convert.ToDouble(drledg["Credit"].ToString()) > 0)
                                //{
                                //    FourthPeriod1 -= Convert.ToDouble(drledg["Credit"].ToString());
                                //}
                            }
                            if (Period >= m_DS.FourthPeriod)
                            {
                                if (Convert.ToDouble(drledg["Debit"].ToString()) > 0)
                                {
                                    GreaterThanFouthPeriod1 += Convert.ToDouble(drledg["Debit"].ToString()) - ReceiptAmount;
                                }
                                //if (Convert.ToDouble(drledg["Credit"].ToString()) > 0)
                                //{
                                //    GreaterThanFouthPeriod1 -= Convert.ToDouble(drledg["Credit"].ToString());
                                //}
                            }


                       }
                        LoadAllDebtors(LedgerID1, ledgercode1, ledgername1, FirstPeriod1, SecondPeriod1, ThirdPeriod1, FourthPeriod1, GreaterThanFouthPeriod1, IsCrystalReport);
                    }
                    #endregion
                    #region For All Debtors
                    if (m_DS.ShowAllDebtores == true)
                    {
                       m_DS.FromDate =Convert.ToDateTime(DateTime.Now);
                        DataTable dtLedgerIDsByGrpID = Ledger.GetAllLedger(29);//Get All Ledgers Under Debtors
                        foreach (DataRow drLedgerIDsByGrpID in dtLedgerIDsByGrpID.Rows)
                        {
                           bool IsDebtorsZero = true;
                           double FirstPeriod = 0;
                           double SecondPeriod = 0;
                           double ThirdPeriod = 0;
                           double FourthPeriod = 0;
                           double GreaterThanFouthPeriod = 0;
                           int LedgerID = Convert.ToInt32(drLedgerIDsByGrpID["LedgerID"]);
                           DataTable dtLedgerInfo = Ledger.GetAllLedgerDetailsByIDForAgeing(LedgerID);
                           DataRow drledgerinfo = dtLedgerInfo.Rows[0];
                           string ledgercode = drledgerinfo["LedgerCode"].ToString();
                           string ledgername = drledgerinfo["EngName"].ToString();
                           DataTable dt = Transaction.GetLedgerTransaction(LedgerID, AccClassIDsXMLString, m_DS.FromDate, m_DS.ToDate, ProjectIDsXMLString);
                           foreach (DataRow drledg in dt.Rows)
                           {
                               double ReceiptAmount = 0;
                               IsDebtorsZero = true;
                               if (drledg["VoucherType"].ToString() == "SALES") 
                               {
                                   DataTable dtDebtorReceipt = Debtors.GetCashReceiptDetails1("SALES",  drledg["VoucherNumber"].ToString(), LedgerID);
                                   if (dtDebtorReceipt.Rows.Count > 0)
                                   {
                                       DataRow drcashreceipt = dtDebtorReceipt.Rows[0];
                                       ReceiptAmount += Convert.ToDouble(drcashreceipt["Amount"].ToString());
                                   }
                                   DataTable dtDebtorBankReceipt = Debtors.GetBankReceiptDetails1("SALES", drledg["VoucherNumber"].ToString(), LedgerID);
                                   if (dtDebtorBankReceipt.Rows.Count > 0)
                                   {
                                       DataRow drbankreceipt = dtDebtorBankReceipt.Rows[0];
                                       ReceiptAmount = ReceiptAmount + Convert.ToDouble(drbankreceipt["Amount"].ToString());
                                   }
                               }
                               if (drledg["VoucherType"].ToString() == "JRNL")
                               {
                                   DataTable dtDebtorReceipt = Debtors.GetCashReceiptDetails1("JRNL", drledg["VoucherNumber"].ToString(), LedgerID);
                                   if (dtDebtorReceipt.Rows.Count > 0)
                                   {
                                       DataRow drcashreceipt = dtDebtorReceipt.Rows[0];
                                       ReceiptAmount = ReceiptAmount + Convert.ToDouble(drcashreceipt["Amount"].ToString());
                                   }
                                   DataTable dtDebtorBankReceipt = Debtors.GetBankReceiptDetails1("JRNL", drledg["VoucherNumber"].ToString(), LedgerID);
                                   if (dtDebtorBankReceipt.Rows.Count > 0)
                                   {
                                       DataRow drbankreceipt = dtDebtorBankReceipt.Rows[0];
                                       ReceiptAmount = ReceiptAmount + Convert.ToDouble(drbankreceipt["Amount"].ToString());
                                   }
                               }
                               if (drledg["VoucherType"].ToString() == "CASH_PMNT")
                               {
                                   DataTable dtDebtorReceipt = Debtors.GetCashReceiptDetails1("CASH_PMNT", drledg["VoucherNumber"].ToString(), LedgerID);
                                   if (dtDebtorReceipt.Rows.Count > 0)
                                   {
                                       DataRow drcashreceipt = dtDebtorReceipt.Rows[0];
                                       ReceiptAmount = ReceiptAmount + Convert.ToDouble(drcashreceipt["Amount"].ToString());
                                   }
                                   DataTable dtDebtorBankReceipt = Debtors.GetBankReceiptDetails1("CASH_PMNT", drledg["VoucherNumber"].ToString(), LedgerID);
                                   if (dtDebtorBankReceipt.Rows.Count > 0)
                                   {
                                       DataRow drbankreceipt = dtDebtorBankReceipt.Rows[0];
                                       ReceiptAmount = ReceiptAmount + Convert.ToDouble(drbankreceipt["Amount"].ToString());
                                   }
                               }
                               if (drledg["VoucherType"].ToString() == "BANK_PMNT")
                               {
                                   DataTable dtDebtorReceipt = Debtors.GetCashReceiptDetails1("BANK_PMNT", drledg["VoucherNumber"].ToString(), LedgerID);
                                   if (dtDebtorReceipt.Rows.Count > 0)
                                   {
                                       DataRow drcashreceipt = dtDebtorReceipt.Rows[0];
                                       ReceiptAmount = ReceiptAmount + Convert.ToDouble(drcashreceipt["Amount"].ToString());
                                   }
                                   DataTable dtDebtorBankReceipt = Debtors.GetBankReceiptDetails1("BANK_PMNT", drledg["VoucherNumber"].ToString(), LedgerID);
                                   if (dtDebtorBankReceipt.Rows.Count > 0)
                                   {
                                       DataRow drbankreceipt = dtDebtorBankReceipt.Rows[0];
                                       ReceiptAmount = ReceiptAmount + Convert.ToDouble(drbankreceipt["Amount"].ToString());
                                   }
                               }
                               if (drledg["Debit"].ToString() != "0.00" || drledg["Credit"].ToString() != "0.00")
                               {
                                   IsDebtorsZero = false;
                                   DateTime TodaysDate = System.Convert.ToDateTime(Date.GetServerDate());
                                   DateTime LedgerDate = System.Convert.ToDateTime(drledg["LedgerDate"].ToString());
                                   TimeSpan servicePeriod = TodaysDate - System.Convert.ToDateTime(drledg["LedgerDate"].ToString());
                                   double Period = servicePeriod.TotalDays;
                                   if (Period < m_DS.FirstPeriod)
                                   {
                                       if (Convert.ToDouble(drledg["Debit"].ToString()) > 0)
                                       {
                                          // double Amt=Convert.ToDouble(drledg["Debit"].ToString());
                                          // MessageBox.Show(FirstPeriod.ToString());
                                           FirstPeriod += (Convert.ToDouble(drledg["Debit"].ToString())) - ReceiptAmount;
                                       }
                                       //if (Convert.ToDouble(drledg["Credit"].ToString()) + ReceiptAmount > 0)
                                       //{
                                       //    FirstPeriod -= Convert.ToDouble(drledg["Credit"].ToString());
                                       //}
                                   }
                                   if (Period >= m_DS.FirstPeriod && Period < m_DS.SecondPeriod)
                                   {
                                       if (Convert.ToDouble(drledg["Debit"].ToString()) > 0)
                                       {
                                           SecondPeriod += Convert.ToDouble(drledg["Debit"].ToString())-ReceiptAmount;
                                       }
                                       //if (Convert.ToDouble(drledg["Credit"].ToString()) + ReceiptAmount > 0)
                                       //{
                                       //    SecondPeriod -= Convert.ToDouble(drledg["Credit"].ToString());
                                       //}
                                   }
                                   if (Period >= m_DS.SecondPeriod && Period < m_DS.ThirdPeriod)
                                   {
                                       if (Convert.ToDouble(drledg["Debit"].ToString()) > 0)
                                       {
                                           ThirdPeriod += Convert.ToDouble(drledg["Debit"].ToString()) - ReceiptAmount;
                                       }
                                       //if (Convert.ToDouble(drledg["Credit"].ToString()) > 0)
                                       //{
                                       //    ThirdPeriod -= Convert.ToDouble(drledg["Credit"].ToString());
                                       //}
                                   }
                                   if (Period >= m_DS.ThirdPeriod && Period < m_DS.FourthPeriod)
                                   {
                                       if (Convert.ToDouble(drledg["Debit"].ToString()) > 0)
                                       {
                                           FourthPeriod += Convert.ToDouble(drledg["Debit"].ToString())-ReceiptAmount ;
                                       }
                                       //if (Convert.ToDouble(drledg["Credit"].ToString()) + ReceiptAmount > 0)
                                       //{
                                       //    FourthPeriod -= Convert.ToDouble(drledg["Credit"].ToString());
                                       //}
                                   }
                                   if (Period >= m_DS.FourthPeriod)
                                   {
                                       if (Convert.ToDouble(drledg["Debit"].ToString()) > 0)
                                       {
                                           GreaterThanFouthPeriod += Convert.ToDouble(drledg["Debit"].ToString()) - ReceiptAmount;
                                       }
                                       //if (Convert.ToDouble(drledg["Credit"].ToString()) + ReceiptAmount > 0)
                                       //{
                                       //    GreaterThanFouthPeriod -= Convert.ToDouble(drledg["Credit"].ToString());
                                       //}
                                   }

                               }
                               
                                
                           }
                           if (IsDebtorsZero == false)
                           {
                               LoadAllDebtors(LedgerID, ledgercode, ledgername, FirstPeriod, SecondPeriod, ThirdPeriod, FourthPeriod, GreaterThanFouthPeriod, IsCrystalReport);
                           }
                          
                        }
                    }
                    #endregion
                }
                #endregion
                #region if BillWise Ageing Checked
                if(m_DS.BillwiseAgeing==true)
                {
                    #region For Particular Debtors
                    if (m_DS.ShowAllDebtores==false)
                    {
                        double FirstPeriod1 = 0;
                        double SecondPeriod1 = 0;
                        double ThirdPeriod1 = 0;
                        double FourthPeriod1 = 0;
                        double GreaterThanFouthPeriod1 = 0;
                        double FirstPeriodRemaining = 0;
                        double SecondPeriodRemaining = 0;
                        double ThirdPeriodRemaining = 0;
                        double FourthPeriodRemaining = 0;
                        double GreaterThanFouthPeriodRemaining = 0;
                        int LedgerID1 = m_DS.DebtorsID;
                        DataTable dtLedgerInfo1 = Ledger.GetAllLedgerDetailsByIDForAgeing(LedgerID1);
                        DataRow drledgerinfo1 = dtLedgerInfo1.Rows[0];
                        string ledgercode1 = drledgerinfo1["LedgerCode"].ToString();
                        string ledgername1 = drledgerinfo1["EngName"].ToString();
                        DataTable dt1 = Transaction.GetLedgerTransaction(LedgerID1, AccClassIDsXMLString, m_DS.FromDate, m_DS.ToDate, ProjectIDsXMLString);
                        foreach (DataRow drledg in dt1.Rows)
                        {
                            int CheckPeriod = 0;
                            DateTime TodaysDate = System.Convert.ToDateTime(Date.GetServerDate());
                            DateTime LedgerDate = System.Convert.ToDateTime(drledg["LedgerDate"].ToString());
                            TimeSpan servicePeriod = TodaysDate - System.Convert.ToDateTime(drledg["LedgerDate"].ToString());
                            double Period = servicePeriod.TotalDays;
                            FirstPeriod1 = 0;
                            SecondPeriod1 = 0;
                            ThirdPeriod1 = 0;
                            FourthPeriod1 = 0;
                            GreaterThanFouthPeriod1 = 0;
                            if (Period < m_DS.FirstPeriod)
                            {
                                if (Convert.ToDouble(drledg["Debit"].ToString()) > 0)
                                {
                                    CheckPeriod = 1;
                                    FirstPeriod1 += Convert.ToDouble(drledg["Debit"].ToString()) ;
                                    FirstPeriodRemaining = FirstPeriodRemaining + FirstPeriod1;
                                    LoadAllDebtorsBillWise(LedgerID1, ledgercode1, ledgername1, FirstPeriod1, SecondPeriod1, ThirdPeriod1, FourthPeriod1, GreaterThanFouthPeriod1, Date.DBToSystem(drledg["LedgerDate"].ToString()), drledg["VoucherType"].ToString(), drledg["VoucherNumber"].ToString(), "LEDGERHEAD", Convert.ToInt32(drledg["RowID"].ToString()), IsCrystalReport);
                                }
                              
                            }
                            if (Period >= m_DS.FirstPeriod && Period < m_DS.SecondPeriod)
                            {
                                if (Convert.ToDouble(drledg["Debit"].ToString()) > 0)
                                {
                                    CheckPeriod = 2;
                                    SecondPeriod1 += Convert.ToDouble(drledg["Debit"].ToString());
                                    SecondPeriodRemaining = SecondPeriodRemaining + SecondPeriod1;
                                    LoadAllDebtorsBillWise(LedgerID1, ledgercode1, ledgername1, FirstPeriod1, SecondPeriod1, ThirdPeriod1, FourthPeriod1, GreaterThanFouthPeriod1, Date.DBToSystem(drledg["LedgerDate"].ToString()), drledg["VoucherType"].ToString(), drledg["VoucherNumber"].ToString(), "LEDGERHEAD", Convert.ToInt32(drledg["RowID"].ToString()), IsCrystalReport);
                                }
                                
                            }
                            if (Period >= m_DS.SecondPeriod && Period < m_DS.ThirdPeriod)
                            {
                                if (Convert.ToDouble(drledg["Debit"].ToString()) > 0)
                                {
                                    CheckPeriod = 3;
                                    ThirdPeriod1 += Convert.ToDouble(drledg["Debit"].ToString()) ;
                                    ThirdPeriodRemaining = ThirdPeriodRemaining + ThirdPeriod1;
                                    LoadAllDebtorsBillWise(LedgerID1, ledgercode1, ledgername1, FirstPeriod1, SecondPeriod1, ThirdPeriod1, FourthPeriod1, GreaterThanFouthPeriod1, Date.DBToSystem(drledg["LedgerDate"].ToString()), drledg["VoucherType"].ToString(), drledg["VoucherNumber"].ToString(), "LEDGERHEAD", Convert.ToInt32(drledg["RowID"].ToString()), IsCrystalReport);
                                }
                              
                            }
                            if (Period >= m_DS.ThirdPeriod && Period < m_DS.FourthPeriod)
                            {
                                if (Convert.ToDouble(drledg["Debit"].ToString()) > 0)
                                {
                                    CheckPeriod = 4;
                                    FourthPeriod1 += Convert.ToDouble(drledg["Debit"].ToString()) ;
                                    FourthPeriodRemaining = FourthPeriodRemaining + FourthPeriod1;
                                    LoadAllDebtorsBillWise(LedgerID1, ledgercode1, ledgername1, FirstPeriod1, SecondPeriod1, ThirdPeriod1, FourthPeriod1, GreaterThanFouthPeriod1, Date.DBToSystem(drledg["LedgerDate"].ToString()), drledg["VoucherType"].ToString(), drledg["VoucherNumber"].ToString(), "LEDGERHEAD", Convert.ToInt32(drledg["RowID"].ToString()), IsCrystalReport);
                                }
                               
                            }
                            if (Period >= m_DS.FourthPeriod)
                            {
                                if (Convert.ToDouble(drledg["Debit"].ToString()) > 0)
                                {
                                    CheckPeriod = 5;
                                    GreaterThanFouthPeriod1 += Convert.ToDouble(drledg["Debit"].ToString()) ;
                                    GreaterThanFouthPeriodRemaining = GreaterThanFouthPeriodRemaining + GreaterThanFouthPeriod1;
                                    LoadAllDebtorsBillWise(LedgerID1, ledgercode1, ledgername1, FirstPeriod1, SecondPeriod1, ThirdPeriod1, FourthPeriod1, GreaterThanFouthPeriod1, Date.DBToSystem(drledg["LedgerDate"].ToString()), drledg["VoucherType"].ToString(), drledg["VoucherNumber"].ToString(), "LEDGERHEAD", Convert.ToInt32(drledg["RowID"].ToString()), IsCrystalReport);
                                }
                                
                            }

                            //LoadAllDebtorsBillWise(LedgerID1, ledgercode1, ledgername1, FirstPeriod1, SecondPeriod1, ThirdPeriod1, FourthPeriod1, GreaterThanFouthPeriod1, Date.DBToSystem(drledg["LedgerDate"].ToString()), drledg["VoucherType"].ToString(), drledg["VoucherNumber"].ToString(), "LEDGERHEAD", false);

                            double ReceiptAmount = 0;
                            double TotalReceiptAmount = 0;
                            double Balance = 0;
                            double DrBal = 0;
                            if (drledg["VoucherType"].ToString() == "SALES")
                            {
                                DataTable dtDebtorReceipt = Debtors.GetCashReceiptDetails("SALES", drledg["VoucherNumber"].ToString(), LedgerID1);
                                if (dtDebtorReceipt.Rows.Count > 0)
                                {
                                    for (int i = 0; i < dtDebtorReceipt.Rows.Count; i++)
                                    {
                                         FirstPeriod1 = 0;
                                         SecondPeriod1 = 0;
                                         ThirdPeriod1 = 0;
                                         FourthPeriod1 = 0;
                                         GreaterThanFouthPeriod1 = 0;
                                        DataRow drcashreceipt = dtDebtorReceipt.Rows[i];
                                        string TLedgerName = Ledger.GetLedgerNameFromID(Convert.ToInt32(drcashreceipt["LedgerID"].ToString()));
                                        TotalReceiptAmount = TotalReceiptAmount + Convert.ToDouble(drcashreceipt["Amount"].ToString());
                                        ReceiptAmount = Convert.ToDouble(drcashreceipt["Amount"].ToString());
                                        Balance = DrBal - TotalReceiptAmount;
                                        if (CheckPeriod == 1)
                                            FirstPeriod1 =ReceiptAmount*-1;
                                        else if (CheckPeriod == 2)
                                            SecondPeriod1 =  ReceiptAmount*-1;
                                        else if (CheckPeriod == 3)
                                            ThirdPeriod1 =  ReceiptAmount*-1;
                                        else if (CheckPeriod == 4)
                                            FourthPeriod1 =  ReceiptAmount*-1;
                                        else if (CheckPeriod == 5)
                                            GreaterThanFouthPeriod1 =  ReceiptAmount*-1;

                                        FirstPeriodRemaining += FirstPeriod1;
                                        SecondPeriodRemaining += SecondPeriod1;
                                        ThirdPeriodRemaining += ThirdPeriod1;
                                        FourthPeriodRemaining += FourthPeriod1;
                                        GreaterThanFouthPeriodRemaining += GreaterThanFouthPeriod1;
                                        LoadAllDebtorsBillWise(LedgerID1, ledgercode1, TLedgerName, FirstPeriod1, SecondPeriod1, ThirdPeriod1, FourthPeriod1, GreaterThanFouthPeriod1, Date.DBToSystem(drcashreceipt["LedgerDate"].ToString()), "CASH_RCPT", drcashreceipt["Voucher_No"].ToString(), "LEDGER", Convert.ToInt32(drcashreceipt["RowID"].ToString()), IsCrystalReport); 
                                    }
                                }
                                DataTable dtDebtorBankReceipt = Debtors.GetBankReceiptDetails("SALES", drledg["VoucherNumber"].ToString(), LedgerID1);
                                if (dtDebtorBankReceipt.Rows.Count > 0)
                                {
                                    for (int i = 0; i < dtDebtorBankReceipt.Rows.Count; i++)
                                    {
                                        FirstPeriod1 = 0;
                                        SecondPeriod1 = 0;
                                        ThirdPeriod1 = 0;
                                        FourthPeriod1 = 0;
                                        GreaterThanFouthPeriod1 = 0;
                                        DataRow drbankreceipt = dtDebtorBankReceipt.Rows[i];
                                        string TLedgerName = Ledger.GetLedgerNameFromID(Convert.ToInt32(drbankreceipt["LedgerID"].ToString()));
                                        TotalReceiptAmount = TotalReceiptAmount + Convert.ToDouble(drbankreceipt["Amount"].ToString());
                                        ReceiptAmount = Convert.ToDouble(drbankreceipt["Amount"].ToString());
                                        Balance = DrBal - TotalReceiptAmount;
                                        if (CheckPeriod == 1)
                                            FirstPeriod1 = ReceiptAmount * -1;
                                        else if (CheckPeriod == 2)
                                            SecondPeriod1 = ReceiptAmount * -1;
                                        else if (CheckPeriod == 3)
                                            ThirdPeriod1 = ReceiptAmount * -1;
                                        else if (CheckPeriod == 4)
                                            FourthPeriod1 = ReceiptAmount * -1;
                                        else if (CheckPeriod == 5)
                                            GreaterThanFouthPeriod1 = ReceiptAmount * -1;

                                        FirstPeriodRemaining += FirstPeriod1;
                                        SecondPeriodRemaining += SecondPeriod1;
                                        ThirdPeriodRemaining += ThirdPeriod1;
                                        FourthPeriodRemaining += FourthPeriod1;
                                        GreaterThanFouthPeriodRemaining += GreaterThanFouthPeriod1;
                                        LoadAllDebtorsBillWise(LedgerID1, ledgercode1, TLedgerName, FirstPeriod1, SecondPeriod1, ThirdPeriod1, FourthPeriod1, GreaterThanFouthPeriod1, Date.DBToSystem(drbankreceipt["LedgerDate"].ToString()), "BANK_RCPT", drbankreceipt["Voucher_No"].ToString(), "LEDGER",Convert.ToInt32( drbankreceipt["RowID"].ToString()), IsCrystalReport); 
                                        
                                    }
                                }

                            }

                            if (drledg["VoucherType"].ToString() == "JRNL")
                            {
                                DataTable dtDebtorReceipt = Debtors.GetCashReceiptDetails("JRNL", drledg["VoucherNumber"].ToString(), LedgerID1);
                                if (dtDebtorReceipt.Rows.Count > 0)
                                {
                                    for (int i = 0; i < dtDebtorReceipt.Rows.Count; i++)
                                    {
                                        FirstPeriod1 = 0;
                                        SecondPeriod1 = 0;
                                        ThirdPeriod1 = 0;
                                        FourthPeriod1 = 0;
                                        GreaterThanFouthPeriod1 = 0;
                                        DataRow drcashreceipt = dtDebtorReceipt.Rows[0];
                                        string TLedgerName = Ledger.GetLedgerNameFromID(Convert.ToInt32(drcashreceipt["LedgerID"].ToString()));
                                        TotalReceiptAmount = TotalReceiptAmount + Convert.ToDouble(drcashreceipt["Amount"].ToString());
                                        ReceiptAmount = Convert.ToDouble(drcashreceipt["Amount"].ToString());
                                        Balance = DrBal - TotalReceiptAmount;
                                        if (CheckPeriod == 1)
                                            FirstPeriod1 = ReceiptAmount * -1;
                                        else if (CheckPeriod == 2)
                                            SecondPeriod1 = ReceiptAmount * -1;
                                        else if (CheckPeriod == 3)
                                            ThirdPeriod1 = ReceiptAmount * -1;
                                        else if (CheckPeriod == 4)
                                            FourthPeriod1 = ReceiptAmount * -1;
                                        else if (CheckPeriod == 5)
                                            GreaterThanFouthPeriod1 = ReceiptAmount * -1;
                                        FirstPeriodRemaining += FirstPeriod1;
                                        SecondPeriodRemaining += SecondPeriod1;
                                        ThirdPeriodRemaining += ThirdPeriod1;
                                        FourthPeriodRemaining += FourthPeriod1;
                                        GreaterThanFouthPeriodRemaining += GreaterThanFouthPeriod1;
                                        LoadAllDebtorsBillWise(LedgerID1, ledgercode1, TLedgerName, FirstPeriod1, SecondPeriod1, ThirdPeriod1, FourthPeriod1, GreaterThanFouthPeriod1, Date.DBToSystem(drcashreceipt["LedgerDate"].ToString()), "CASH_RCPT", drcashreceipt["Voucher_No"].ToString(), "LEDGER", Convert.ToInt32(drcashreceipt["RowID"].ToString()), IsCrystalReport); 
                                      
                                    }
                                }
                                DataTable dtDebtorBankReceipt = Debtors.GetBankReceiptDetails("JRNL", drledg["VoucherNumber"].ToString(), LedgerID1);
                                if (dtDebtorBankReceipt.Rows.Count > 0)
                                {
                                    for (int i = 0; i < dtDebtorBankReceipt.Rows.Count; i++)
                                    {
                                        FirstPeriod1 = 0;
                                        SecondPeriod1 = 0;
                                        ThirdPeriod1 = 0;
                                        FourthPeriod1 = 0;
                                        GreaterThanFouthPeriod1 = 0;
                                        DataRow drbankreceipt = dtDebtorBankReceipt.Rows[0];
                                        string TLedgerName = Ledger.GetLedgerNameFromID(Convert.ToInt32(drbankreceipt["LedgerID"].ToString()));
                                        TotalReceiptAmount = TotalReceiptAmount + Convert.ToDouble(drbankreceipt["Amount"].ToString());
                                        ReceiptAmount = Convert.ToDouble(drbankreceipt["Amount"].ToString());
                                        Balance = DrBal - TotalReceiptAmount;
                                        if (CheckPeriod == 1)
                                            FirstPeriod1 = ReceiptAmount * -1;
                                        else if (CheckPeriod == 2)
                                            SecondPeriod1 = ReceiptAmount * -1;
                                        else if (CheckPeriod == 3)
                                            ThirdPeriod1 = ReceiptAmount * -1;
                                        else if (CheckPeriod == 4)
                                            FourthPeriod1 = ReceiptAmount * -1;
                                        else if (CheckPeriod == 5)
                                            GreaterThanFouthPeriod1 = ReceiptAmount * -1;
                                        FirstPeriodRemaining += FirstPeriod1;
                                        SecondPeriodRemaining += SecondPeriod1;
                                        ThirdPeriodRemaining += ThirdPeriod1;
                                        FourthPeriodRemaining += FourthPeriod1;
                                        GreaterThanFouthPeriodRemaining += GreaterThanFouthPeriod1;
                                        LoadAllDebtorsBillWise(LedgerID1, ledgercode1, TLedgerName, FirstPeriod1, SecondPeriod1, ThirdPeriod1, FourthPeriod1, GreaterThanFouthPeriod1, Date.DBToSystem(drbankreceipt["LedgerDate"].ToString()), "BANK_RCPT", drbankreceipt["Voucher_No"].ToString(), "LEDGER", Convert.ToInt32(drbankreceipt["RowID"].ToString()), IsCrystalReport); 
                                        
                                    }
                                }
                               
                              
                               
                            }
                            if (drledg["VoucherType"].ToString() == "CASH_PMNT")
                            {
                                DataTable dtDebtorReceipt = Debtors.GetCashReceiptDetails("CASH_PMNT", drledg["VoucherNumber"].ToString(), LedgerID1);
                                if (dtDebtorReceipt.Rows.Count > 0)
                                {
                                    for (int i = 0; i < dtDebtorReceipt.Rows.Count; i++)
                                    {
                                        FirstPeriod1 = 0;
                                        SecondPeriod1 = 0;
                                        ThirdPeriod1 = 0;
                                        FourthPeriod1 = 0;
                                        GreaterThanFouthPeriod1 = 0;
                                        DataRow drcashreceipt = dtDebtorReceipt.Rows[0];
                                        string TLedgerName = Ledger.GetLedgerNameFromID(Convert.ToInt32(drcashreceipt["LedgerID"].ToString()));
                                        TotalReceiptAmount = TotalReceiptAmount + Convert.ToDouble(drcashreceipt["Amount"].ToString());
                                        ReceiptAmount = Convert.ToDouble(drcashreceipt["Amount"].ToString());
                                        Balance = DrBal - TotalReceiptAmount;
                                        if (CheckPeriod == 1)
                                            FirstPeriod1 = ReceiptAmount * -1;
                                        else if (CheckPeriod == 2)
                                            SecondPeriod1 = ReceiptAmount * -1;
                                        else if (CheckPeriod == 3)
                                            ThirdPeriod1 = ReceiptAmount * -1;
                                        else if (CheckPeriod == 4)
                                            FourthPeriod1 = ReceiptAmount * -1;
                                        else if (CheckPeriod == 5)
                                            GreaterThanFouthPeriod1 = ReceiptAmount * -1;

                                        FirstPeriodRemaining += FirstPeriod1;
                                        SecondPeriodRemaining += SecondPeriod1;
                                        ThirdPeriodRemaining += ThirdPeriod1;
                                        FourthPeriodRemaining += FourthPeriod1;
                                        GreaterThanFouthPeriodRemaining += GreaterThanFouthPeriod1;
                                        LoadAllDebtorsBillWise(LedgerID1, ledgercode1, TLedgerName, FirstPeriod1, SecondPeriod1, ThirdPeriod1, FourthPeriod1, GreaterThanFouthPeriod1, Date.DBToSystem(drcashreceipt["LedgerDate"].ToString()), "CASH_RCPT", drcashreceipt["Voucher_No"].ToString(), "LEDGER", Convert.ToInt32(drcashreceipt["RowID"].ToString()), IsCrystalReport); 
                                     
                                    }
                                }
                                DataTable dtDebtorBankReceipt = Debtors.GetBankReceiptDetails("CASH_PMNT", drledg["VoucherNumber"].ToString(), LedgerID1);
                                if (dtDebtorBankReceipt.Rows.Count > 0)
                                {
                                    for (int i = 0; i < dtDebtorBankReceipt.Rows.Count; i++)
                                    {
                                        FirstPeriod1 = 0;
                                        SecondPeriod1 = 0;
                                        ThirdPeriod1 = 0;
                                        FourthPeriod1 = 0;
                                        GreaterThanFouthPeriod1 = 0;
                                        DataRow drbankreceipt = dtDebtorBankReceipt.Rows[0];
                                        string TLedgerName = Ledger.GetLedgerNameFromID(Convert.ToInt32(drbankreceipt["LedgerID"].ToString()));
                                        ReceiptAmount = Convert.ToDouble(drbankreceipt["Amount"].ToString());
                                        TotalReceiptAmount = TotalReceiptAmount + Convert.ToDouble(drbankreceipt["Amount"].ToString());
                                        Balance = DrBal - TotalReceiptAmount;
                                        if (CheckPeriod == 1)
                                            FirstPeriod1 = ReceiptAmount * -1;
                                        else if (CheckPeriod == 2)
                                            SecondPeriod1 = ReceiptAmount * -1;
                                        else if (CheckPeriod == 3)
                                            ThirdPeriod1 = ReceiptAmount * -1;
                                        else if (CheckPeriod == 4)
                                            FourthPeriod1 = ReceiptAmount * -1;
                                        else if (CheckPeriod == 5)
                                            GreaterThanFouthPeriod1 = ReceiptAmount * -1;

                                        FirstPeriodRemaining += FirstPeriod1;
                                        SecondPeriodRemaining += SecondPeriod1;
                                        ThirdPeriodRemaining += ThirdPeriod1;
                                        FourthPeriodRemaining += FourthPeriod1;
                                        GreaterThanFouthPeriodRemaining += GreaterThanFouthPeriod1;
                                        LoadAllDebtorsBillWise(LedgerID1, ledgercode1, TLedgerName, FirstPeriod1, SecondPeriod1, ThirdPeriod1, FourthPeriod1, GreaterThanFouthPeriod1, Date.DBToSystem(drbankreceipt["LedgerDate"].ToString()), "BANK_RCPT", drbankreceipt["Voucher_No"].ToString(), "LEDGER", Convert.ToInt32(drbankreceipt["RowID"].ToString()), IsCrystalReport); 
                                       
                                    }
                                }
                               
                                
                               
                            }
                            if (drledg["VoucherType"].ToString() == "BANK_PMNT")
                            {
                                DataTable dtDebtorReceipt = Debtors.GetCashReceiptDetails("BANK_PMNT", drledg["VoucherNumber"].ToString(), LedgerID1);
                                if (dtDebtorReceipt.Rows.Count > 0)
                                {
                                    for (int i = 0; i < dtDebtorReceipt.Rows.Count; i++)
                                    {
                                        FirstPeriod1 = 0;
                                        SecondPeriod1 = 0;
                                        ThirdPeriod1 = 0;
                                        FourthPeriod1 = 0;
                                        GreaterThanFouthPeriod1 = 0;
                                        DataRow drcashreceipt = dtDebtorReceipt.Rows[0];
                                        string TLedgerName = Ledger.GetLedgerNameFromID(Convert.ToInt32(drcashreceipt["LedgerID"].ToString()));
                                        TotalReceiptAmount = TotalReceiptAmount + Convert.ToDouble(drcashreceipt["Amount"].ToString());
                                        ReceiptAmount = Convert.ToDouble(drcashreceipt["Amount"].ToString());
                                        Balance = DrBal - TotalReceiptAmount;
                                        if (CheckPeriod == 1)
                                            FirstPeriod1 = ReceiptAmount * -1;
                                        else if (CheckPeriod == 2)
                                            SecondPeriod1 = ReceiptAmount * -1;
                                        else if (CheckPeriod == 3)
                                            ThirdPeriod1 = ReceiptAmount * -1;
                                        else if (CheckPeriod == 4)
                                            FourthPeriod1 = ReceiptAmount * -1;
                                        else if (CheckPeriod == 5)
                                            GreaterThanFouthPeriod1 = ReceiptAmount * -1;

                                        FirstPeriodRemaining += FirstPeriod1;
                                        SecondPeriodRemaining += SecondPeriod1;
                                        ThirdPeriodRemaining += ThirdPeriod1;
                                        FourthPeriodRemaining += FourthPeriod1;
                                        GreaterThanFouthPeriodRemaining += GreaterThanFouthPeriod1;
                                        LoadAllDebtorsBillWise(LedgerID1, ledgercode1, TLedgerName, FirstPeriod1, SecondPeriod1, ThirdPeriod1, FourthPeriod1, GreaterThanFouthPeriod1, Date.DBToSystem(drcashreceipt["LedgerDate"].ToString()), "CASH_RCPT", drcashreceipt["Voucher_No"].ToString(), "LEDGER", Convert.ToInt32(drcashreceipt["RowID"].ToString()), IsCrystalReport); 
                                       
                                    }
                                }
                                DataTable dtDebtorBankReceipt = Debtors.GetBankReceiptDetails("BANK_PMNT", drledg["VoucherNumber"].ToString(), LedgerID1);
                                if (dtDebtorBankReceipt.Rows.Count > 0)
                                {
                                    for (int i = 0; i < dtDebtorBankReceipt.Rows.Count; i++)
                                    {
                                        FirstPeriod1 = 0;
                                        SecondPeriod1 = 0;
                                        ThirdPeriod1 = 0;
                                        FourthPeriod1 = 0;
                                        GreaterThanFouthPeriod1 = 0;
                                        DataRow drbankreceipt = dtDebtorBankReceipt.Rows[0];
                                        string TLedgerName = Ledger.GetLedgerNameFromID(Convert.ToInt32(drbankreceipt["LedgerID"].ToString()));
                                        TotalReceiptAmount = TotalReceiptAmount + Convert.ToDouble(drbankreceipt["Amount"].ToString());
                                        ReceiptAmount = Convert.ToDouble(drbankreceipt["Amount"].ToString());
                                        Balance = DrBal - TotalReceiptAmount;
                                        if (CheckPeriod == 1)
                                            FirstPeriod1 = ReceiptAmount * -1;
                                        else if (CheckPeriod == 2)
                                            SecondPeriod1 = ReceiptAmount * -1;
                                        else if (CheckPeriod == 3)
                                            ThirdPeriod1 = ReceiptAmount * -1;
                                        else if (CheckPeriod == 4)
                                            FourthPeriod1 = ReceiptAmount * -1;
                                        else if (CheckPeriod == 5)
                                            GreaterThanFouthPeriod1 = ReceiptAmount * -1;

                                        FirstPeriodRemaining += FirstPeriod1;
                                        SecondPeriodRemaining += SecondPeriod1;
                                        ThirdPeriodRemaining += ThirdPeriod1;
                                        FourthPeriodRemaining += FourthPeriod1;
                                        GreaterThanFouthPeriodRemaining += GreaterThanFouthPeriod1;
                                        LoadAllDebtorsBillWise(LedgerID1, ledgercode1, TLedgerName, FirstPeriod1, SecondPeriod1, ThirdPeriod1, FourthPeriod1, GreaterThanFouthPeriod1, Date.DBToSystem(drbankreceipt["LedgerDate"].ToString()), "BANK_RCPT", drbankreceipt["Voucher_No"].ToString(), "LEDGER",Convert.ToInt32( drbankreceipt["RowID"].ToString()), IsCrystalReport); 
                                       
                                    }
                                }
                               
                               
                            }
                            
                        }
                        LoadAllDebtorsBillWise(0, "", "", FirstPeriodRemaining, SecondPeriodRemaining, ThirdPeriodRemaining, FourthPeriodRemaining, GreaterThanFouthPeriodRemaining, "", "", "Total", "LEDGERHEAD", 0, false); 
                    }
                    #endregion
                    #region For All Debtors
                    else if (m_DS.ShowAllDebtores == true)
                    {
                       m_DS.FromDate = Convert.ToDateTime(DateTime.Now);
                        DataTable dtLedgerIDsByGrpID = Ledger.GetAllLedger(29);//Get All Ledgers Under Debtors
                        foreach (DataRow drLedgerIDsByGrpID in dtLedgerIDsByGrpID.Rows)
                        {
                            bool IsDebtorsZero = true;
                            double FirstPeriod1 = 0;
                            double SecondPeriod1 = 0;
                            double ThirdPeriod1 = 0;
                            double FourthPeriod1 = 0;
                            double GreaterThanFouthPeriod1 = 0;
                            double FirstPeriodRemaining = 0;
                            double SecondPeriodRemaining = 0;
                            double ThirdPeriodRemaining = 0;
                            double FourthPeriodRemaining = 0;
                            double GreaterThanFouthPeriodRemaining = 0;
                            int LedgerID1 = Convert.ToInt32(drLedgerIDsByGrpID["LedgerID"]);
                            DataTable dtLedgerInfo1 = Ledger.GetAllLedgerDetailsByIDForAgeing(LedgerID1);
                            DataRow drledgerinfo1 = dtLedgerInfo1.Rows[0];
                            string ledgercode1 = drledgerinfo1["LedgerCode"].ToString();
                            string ledgername1 = drledgerinfo1["EngName"].ToString();
                            DataTable dt1 = Transaction.GetLedgerTransaction(LedgerID1, AccClassIDsXMLString, m_DS.FromDate, m_DS.ToDate, ProjectIDsXMLString);
                            foreach (DataRow drledg in dt1.Rows)
                            {
                                IsDebtorsZero = false;
                                int CheckPeriod = 0;
                                DateTime TodaysDate = System.Convert.ToDateTime(Date.GetServerDate());
                                DateTime LedgerDate = System.Convert.ToDateTime(drledg["LedgerDate"].ToString());
                                TimeSpan servicePeriod = TodaysDate - System.Convert.ToDateTime(drledg["LedgerDate"].ToString());
                                double Period = servicePeriod.TotalDays;
                                FirstPeriod1 = 0;
                                SecondPeriod1 = 0;
                                ThirdPeriod1 = 0;
                                FourthPeriod1 = 0;
                                GreaterThanFouthPeriod1 = 0;
                                if (Period < m_DS.FirstPeriod)
                                {
                                    if (Convert.ToDouble(drledg["Debit"].ToString()) > 0)
                                    {
                                        CheckPeriod = 1;
                                        FirstPeriod1 += Convert.ToDouble(drledg["Debit"].ToString());
                                        FirstPeriodRemaining = FirstPeriodRemaining + FirstPeriod1;
                                        LoadAllDebtorsBillWise(LedgerID1, ledgercode1, ledgername1, FirstPeriod1, SecondPeriod1, ThirdPeriod1, FourthPeriod1, GreaterThanFouthPeriod1, Date.DBToSystem(drledg["LedgerDate"].ToString()), drledg["VoucherType"].ToString(), drledg["VoucherNumber"].ToString(), "LEDGERHEAD", Convert.ToInt32(drledg["RowID"].ToString()), IsCrystalReport);
                                    }

                                }
                                if (Period >= m_DS.FirstPeriod && Period < m_DS.SecondPeriod)
                                {
                                    if (Convert.ToDouble(drledg["Debit"].ToString()) > 0)
                                    {
                                        CheckPeriod = 2;
                                        SecondPeriod1 += Convert.ToDouble(drledg["Debit"].ToString());
                                        SecondPeriodRemaining = SecondPeriodRemaining + SecondPeriod1;
                                        LoadAllDebtorsBillWise(LedgerID1, ledgercode1, ledgername1, FirstPeriod1, SecondPeriod1, ThirdPeriod1, FourthPeriod1, GreaterThanFouthPeriod1, Date.DBToSystem(drledg["LedgerDate"].ToString()), drledg["VoucherType"].ToString(), drledg["VoucherNumber"].ToString(), "LEDGERHEAD", Convert.ToInt32(drledg["RowID"].ToString()), IsCrystalReport);
                                    }

                                }
                                if (Period >= m_DS.SecondPeriod && Period < m_DS.ThirdPeriod)
                                {
                                    if (Convert.ToDouble(drledg["Debit"].ToString()) > 0)
                                    {
                                        CheckPeriod = 3;
                                        ThirdPeriod1 += Convert.ToDouble(drledg["Debit"].ToString());
                                        ThirdPeriodRemaining = ThirdPeriodRemaining + ThirdPeriod1;
                                        LoadAllDebtorsBillWise(LedgerID1, ledgercode1, ledgername1, FirstPeriod1, SecondPeriod1, ThirdPeriod1, FourthPeriod1, GreaterThanFouthPeriod1, Date.DBToSystem(drledg["LedgerDate"].ToString()), drledg["VoucherType"].ToString(), drledg["VoucherNumber"].ToString(), "LEDGERHEAD", Convert.ToInt32(drledg["RowID"].ToString()), IsCrystalReport);
                                    }

                                }
                                if (Period >= m_DS.ThirdPeriod && Period < m_DS.FourthPeriod)
                                {
                                    if (Convert.ToDouble(drledg["Debit"].ToString()) > 0)
                                    {
                                        CheckPeriod = 4;
                                        FourthPeriod1 += Convert.ToDouble(drledg["Debit"].ToString());
                                        FourthPeriodRemaining = FourthPeriodRemaining + FourthPeriod1;
                                        LoadAllDebtorsBillWise(LedgerID1, ledgercode1, ledgername1, FirstPeriod1, SecondPeriod1, ThirdPeriod1, FourthPeriod1, GreaterThanFouthPeriod1, Date.DBToSystem(drledg["LedgerDate"].ToString()), drledg["VoucherType"].ToString(), drledg["VoucherNumber"].ToString(), "LEDGERHEAD", Convert.ToInt32(drledg["RowID"].ToString()), IsCrystalReport);
                                    }

                                }
                                if (Period >= m_DS.FourthPeriod)
                                {
                                    if (Convert.ToDouble(drledg["Debit"].ToString()) > 0)
                                    {
                                        CheckPeriod = 5;
                                        GreaterThanFouthPeriod1 += Convert.ToDouble(drledg["Debit"].ToString());
                                        GreaterThanFouthPeriodRemaining = GreaterThanFouthPeriodRemaining + GreaterThanFouthPeriod1;
                                        LoadAllDebtorsBillWise(LedgerID1, ledgercode1, ledgername1, FirstPeriod1, SecondPeriod1, ThirdPeriod1, FourthPeriod1, GreaterThanFouthPeriod1, Date.DBToSystem(drledg["LedgerDate"].ToString()), drledg["VoucherType"].ToString(), drledg["VoucherNumber"].ToString(), "LEDGERHEAD", Convert.ToInt32(drledg["RowID"].ToString()), IsCrystalReport);
                                    }

                                }

                                //LoadAllDebtorsBillWise(LedgerID1, ledgercode1, ledgername1, FirstPeriod1, SecondPeriod1, ThirdPeriod1, FourthPeriod1, GreaterThanFouthPeriod1, Date.DBToSystem(drledg["LedgerDate"].ToString()), drledg["VoucherType"].ToString(), drledg["VoucherNumber"].ToString(), "LEDGERHEAD", false);

                                double ReceiptAmount = 0;
                                double TotalReceiptAmount = 0;
                                double Balance = 0;
                                double DrBal = 0;
                                if (drledg["VoucherType"].ToString() == "SALES")
                                {
                                    DataTable dtDebtorReceipt = Debtors.GetCashReceiptDetails("SALES", drledg["VoucherNumber"].ToString(), LedgerID1);
                                    if (dtDebtorReceipt.Rows.Count > 0)
                                    {
                                        for (int i = 0; i < dtDebtorReceipt.Rows.Count; i++)
                                        {
                                            FirstPeriod1 = 0;
                                            SecondPeriod1 = 0;
                                            ThirdPeriod1 = 0;
                                            FourthPeriod1 = 0;
                                            GreaterThanFouthPeriod1 = 0;
                                            DataRow drcashreceipt = dtDebtorReceipt.Rows[i];
                                            string TLedgerName = Ledger.GetLedgerNameFromID(Convert.ToInt32(drcashreceipt["LedgerID"].ToString()));
                                            TotalReceiptAmount = TotalReceiptAmount + Convert.ToDouble(drcashreceipt["Amount"].ToString());
                                            ReceiptAmount = Convert.ToDouble(drcashreceipt["Amount"].ToString());
                                            Balance = DrBal - TotalReceiptAmount;
                                            if (CheckPeriod == 1)
                                                FirstPeriod1 = ReceiptAmount * -1;
                                            else if (CheckPeriod == 2)
                                                SecondPeriod1 = ReceiptAmount * -1;
                                            else if (CheckPeriod == 3)
                                                ThirdPeriod1 = ReceiptAmount * -1;
                                            else if (CheckPeriod == 4)
                                                FourthPeriod1 = ReceiptAmount * -1;
                                            else if (CheckPeriod == 5)
                                                GreaterThanFouthPeriod1 = ReceiptAmount * -1;

                                            FirstPeriodRemaining += FirstPeriod1;
                                            SecondPeriodRemaining += SecondPeriod1;
                                            ThirdPeriodRemaining += ThirdPeriod1;
                                            FourthPeriodRemaining += FourthPeriod1;
                                            GreaterThanFouthPeriodRemaining += GreaterThanFouthPeriod1;
                                            LoadAllDebtorsBillWise(LedgerID1, ledgercode1, TLedgerName, FirstPeriod1, SecondPeriod1, ThirdPeriod1, FourthPeriod1, GreaterThanFouthPeriod1, Date.DBToSystem(drcashreceipt["LedgerDate"].ToString()), "CASH_RCPT", drcashreceipt["Voucher_No"].ToString(), "LEDGER", Convert.ToInt32(drcashreceipt["RowID"].ToString()), IsCrystalReport);
                                        }
                                    }
                                    DataTable dtDebtorBankReceipt = Debtors.GetBankReceiptDetails("SALES", drledg["VoucherNumber"].ToString(), LedgerID1);
                                    if (dtDebtorBankReceipt.Rows.Count > 0)
                                    {
                                        for (int i = 0; i < dtDebtorBankReceipt.Rows.Count; i++)
                                        {
                                            FirstPeriod1 = 0;
                                            SecondPeriod1 = 0;
                                            ThirdPeriod1 = 0;
                                            FourthPeriod1 = 0;
                                            GreaterThanFouthPeriod1 = 0;
                                            DataRow drbankreceipt = dtDebtorBankReceipt.Rows[i];
                                            string TLedgerName = Ledger.GetLedgerNameFromID(Convert.ToInt32(drbankreceipt["LedgerID"].ToString()));
                                            TotalReceiptAmount = TotalReceiptAmount + Convert.ToDouble(drbankreceipt["Amount"].ToString());
                                            ReceiptAmount = Convert.ToDouble(drbankreceipt["Amount"].ToString());
                                            Balance = DrBal - TotalReceiptAmount;
                                            if (CheckPeriod == 1)
                                                FirstPeriod1 = ReceiptAmount * -1;
                                            else if (CheckPeriod == 2)
                                                SecondPeriod1 = ReceiptAmount * -1;
                                            else if (CheckPeriod == 3)
                                                ThirdPeriod1 = ReceiptAmount * -1;
                                            else if (CheckPeriod == 4)
                                                FourthPeriod1 = ReceiptAmount * -1;
                                            else if (CheckPeriod == 5)
                                                GreaterThanFouthPeriod1 = ReceiptAmount * -1;

                                            FirstPeriodRemaining += FirstPeriod1;
                                            SecondPeriodRemaining += SecondPeriod1;
                                            ThirdPeriodRemaining += ThirdPeriod1;
                                            FourthPeriodRemaining += FourthPeriod1;
                                            GreaterThanFouthPeriodRemaining += GreaterThanFouthPeriod1;
                                            LoadAllDebtorsBillWise(LedgerID1, ledgercode1, TLedgerName, FirstPeriod1, SecondPeriod1, ThirdPeriod1, FourthPeriod1, GreaterThanFouthPeriod1, Date.DBToSystem(drbankreceipt["LedgerDate"].ToString()), "BANK_RCPT", drbankreceipt["Voucher_No"].ToString(), "LEDGER", Convert.ToInt32(drbankreceipt["RowID"].ToString()), IsCrystalReport);

                                        }
                                    }

                                }

                                if (drledg["VoucherType"].ToString() == "JRNL")
                                {
                                    DataTable dtDebtorReceipt = Debtors.GetCashReceiptDetails("JRNL", drledg["VoucherNumber"].ToString(), LedgerID1);
                                    if (dtDebtorReceipt.Rows.Count > 0)
                                    {
                                        for (int i = 0; i < dtDebtorReceipt.Rows.Count; i++)
                                        {
                                            FirstPeriod1 = 0;
                                            SecondPeriod1 = 0;
                                            ThirdPeriod1 = 0;
                                            FourthPeriod1 = 0;
                                            GreaterThanFouthPeriod1 = 0;
                                            DataRow drcashreceipt = dtDebtorReceipt.Rows[0];
                                            string TLedgerName = Ledger.GetLedgerNameFromID(Convert.ToInt32(drcashreceipt["LedgerID"].ToString()));
                                            TotalReceiptAmount = TotalReceiptAmount + Convert.ToDouble(drcashreceipt["Amount"].ToString());
                                            ReceiptAmount = Convert.ToDouble(drcashreceipt["Amount"].ToString());
                                            Balance = DrBal - TotalReceiptAmount;
                                            if (CheckPeriod == 1)
                                                FirstPeriod1 = ReceiptAmount * -1;
                                            else if (CheckPeriod == 2)
                                                SecondPeriod1 = ReceiptAmount * -1;
                                            else if (CheckPeriod == 3)
                                                ThirdPeriod1 = ReceiptAmount * -1;
                                            else if (CheckPeriod == 4)
                                                FourthPeriod1 = ReceiptAmount * -1;
                                            else if (CheckPeriod == 5)
                                                GreaterThanFouthPeriod1 = ReceiptAmount * -1;
                                            FirstPeriodRemaining += FirstPeriod1;
                                            SecondPeriodRemaining += SecondPeriod1;
                                            ThirdPeriodRemaining += ThirdPeriod1;
                                            FourthPeriodRemaining += FourthPeriod1;
                                            GreaterThanFouthPeriodRemaining += GreaterThanFouthPeriod1;
                                            LoadAllDebtorsBillWise(LedgerID1, ledgercode1, TLedgerName, FirstPeriod1, SecondPeriod1, ThirdPeriod1, FourthPeriod1, GreaterThanFouthPeriod1, Date.DBToSystem(drcashreceipt["LedgerDate"].ToString()), "CASH_RCPT", drcashreceipt["Voucher_No"].ToString(), "LEDGER", Convert.ToInt32(drcashreceipt["RowID"].ToString()), IsCrystalReport);

                                        }
                                    }
                                    DataTable dtDebtorBankReceipt = Debtors.GetBankReceiptDetails("JRNL", drledg["VoucherNumber"].ToString(), LedgerID1);
                                    if (dtDebtorBankReceipt.Rows.Count > 0)
                                    {
                                        for (int i = 0; i < dtDebtorBankReceipt.Rows.Count; i++)
                                        {
                                            FirstPeriod1 = 0;
                                            SecondPeriod1 = 0;
                                            ThirdPeriod1 = 0;
                                            FourthPeriod1 = 0;
                                            GreaterThanFouthPeriod1 = 0;
                                            DataRow drbankreceipt = dtDebtorBankReceipt.Rows[0];
                                            string TLedgerName = Ledger.GetLedgerNameFromID(Convert.ToInt32(drbankreceipt["LedgerID"].ToString()));
                                            TotalReceiptAmount = TotalReceiptAmount + Convert.ToDouble(drbankreceipt["Amount"].ToString());
                                            ReceiptAmount = Convert.ToDouble(drbankreceipt["Amount"].ToString());
                                            Balance = DrBal - TotalReceiptAmount;
                                            if (CheckPeriod == 1)
                                                FirstPeriod1 = ReceiptAmount * -1;
                                            else if (CheckPeriod == 2)
                                                SecondPeriod1 = ReceiptAmount * -1;
                                            else if (CheckPeriod == 3)
                                                ThirdPeriod1 = ReceiptAmount * -1;
                                            else if (CheckPeriod == 4)
                                                FourthPeriod1 = ReceiptAmount * -1;
                                            else if (CheckPeriod == 5)
                                                GreaterThanFouthPeriod1 = ReceiptAmount * -1;
                                            FirstPeriodRemaining += FirstPeriod1;
                                            SecondPeriodRemaining += SecondPeriod1;
                                            ThirdPeriodRemaining += ThirdPeriod1;
                                            FourthPeriodRemaining += FourthPeriod1;
                                            GreaterThanFouthPeriodRemaining += GreaterThanFouthPeriod1;
                                            LoadAllDebtorsBillWise(LedgerID1, ledgercode1, TLedgerName, FirstPeriod1, SecondPeriod1, ThirdPeriod1, FourthPeriod1, GreaterThanFouthPeriod1, Date.DBToSystem(drbankreceipt["LedgerDate"].ToString()), "BANK_RCPT", drbankreceipt["Voucher_No"].ToString(), "LEDGER", Convert.ToInt32(drbankreceipt["RowID"].ToString()), IsCrystalReport);

                                        }
                                    }



                                }
                                if (drledg["VoucherType"].ToString() == "CASH_PMNT")
                                {
                                    DataTable dtDebtorReceipt = Debtors.GetCashReceiptDetails("CASH_PMNT", drledg["VoucherNumber"].ToString(), LedgerID1);
                                    if (dtDebtorReceipt.Rows.Count > 0)
                                    {
                                        for (int i = 0; i < dtDebtorReceipt.Rows.Count; i++)
                                        {
                                            FirstPeriod1 = 0;
                                            SecondPeriod1 = 0;
                                            ThirdPeriod1 = 0;
                                            FourthPeriod1 = 0;
                                            GreaterThanFouthPeriod1 = 0;
                                            DataRow drcashreceipt = dtDebtorReceipt.Rows[0];
                                            string TLedgerName = Ledger.GetLedgerNameFromID(Convert.ToInt32(drcashreceipt["LedgerID"].ToString()));
                                            TotalReceiptAmount = TotalReceiptAmount + Convert.ToDouble(drcashreceipt["Amount"].ToString());
                                            ReceiptAmount = Convert.ToDouble(drcashreceipt["Amount"].ToString());
                                            Balance = DrBal - TotalReceiptAmount;
                                            if (CheckPeriod == 1)
                                                FirstPeriod1 = ReceiptAmount * -1;
                                            else if (CheckPeriod == 2)
                                                SecondPeriod1 = ReceiptAmount * -1;
                                            else if (CheckPeriod == 3)
                                                ThirdPeriod1 = ReceiptAmount * -1;
                                            else if (CheckPeriod == 4)
                                                FourthPeriod1 = ReceiptAmount * -1;
                                            else if (CheckPeriod == 5)
                                                GreaterThanFouthPeriod1 = ReceiptAmount * -1;

                                            FirstPeriodRemaining += FirstPeriod1;
                                            SecondPeriodRemaining += SecondPeriod1;
                                            ThirdPeriodRemaining += ThirdPeriod1;
                                            FourthPeriodRemaining += FourthPeriod1;
                                            GreaterThanFouthPeriodRemaining += GreaterThanFouthPeriod1;
                                            LoadAllDebtorsBillWise(LedgerID1, ledgercode1, TLedgerName, FirstPeriod1, SecondPeriod1, ThirdPeriod1, FourthPeriod1, GreaterThanFouthPeriod1, Date.DBToSystem(drcashreceipt["LedgerDate"].ToString()), "CASH_RCPT", drcashreceipt["Voucher_No"].ToString(), "LEDGER", Convert.ToInt32(drcashreceipt["RowID"].ToString()), IsCrystalReport);

                                        }
                                    }
                                    DataTable dtDebtorBankReceipt = Debtors.GetBankReceiptDetails("CASH_PMNT", drledg["VoucherNumber"].ToString(), LedgerID1);
                                    if (dtDebtorBankReceipt.Rows.Count > 0)
                                    {
                                        for (int i = 0; i < dtDebtorBankReceipt.Rows.Count; i++)
                                        {
                                            FirstPeriod1 = 0;
                                            SecondPeriod1 = 0;
                                            ThirdPeriod1 = 0;
                                            FourthPeriod1 = 0;
                                            GreaterThanFouthPeriod1 = 0;
                                            DataRow drbankreceipt = dtDebtorBankReceipt.Rows[0];
                                            string TLedgerName = Ledger.GetLedgerNameFromID(Convert.ToInt32(drbankreceipt["LedgerID"].ToString()));
                                            ReceiptAmount = Convert.ToDouble(drbankreceipt["Amount"].ToString());
                                            TotalReceiptAmount = TotalReceiptAmount + Convert.ToDouble(drbankreceipt["Amount"].ToString());
                                            Balance = DrBal - TotalReceiptAmount;
                                            if (CheckPeriod == 1)
                                                FirstPeriod1 = ReceiptAmount * -1;
                                            else if (CheckPeriod == 2)
                                                SecondPeriod1 = ReceiptAmount * -1;
                                            else if (CheckPeriod == 3)
                                                ThirdPeriod1 = ReceiptAmount * -1;
                                            else if (CheckPeriod == 4)
                                                FourthPeriod1 = ReceiptAmount * -1;
                                            else if (CheckPeriod == 5)
                                                GreaterThanFouthPeriod1 = ReceiptAmount * -1;

                                            FirstPeriodRemaining += FirstPeriod1;
                                            SecondPeriodRemaining += SecondPeriod1;
                                            ThirdPeriodRemaining += ThirdPeriod1;
                                            FourthPeriodRemaining += FourthPeriod1;
                                            GreaterThanFouthPeriodRemaining += GreaterThanFouthPeriod1;
                                            LoadAllDebtorsBillWise(LedgerID1, ledgercode1, TLedgerName, FirstPeriod1, SecondPeriod1, ThirdPeriod1, FourthPeriod1, GreaterThanFouthPeriod1, Date.DBToSystem(drbankreceipt["LedgerDate"].ToString()), "BANK_RCPT", drbankreceipt["Voucher_No"].ToString(), "LEDGER", Convert.ToInt32(drbankreceipt["RowID"].ToString()), IsCrystalReport);

                                        }
                                    }



                                }
                                if (drledg["VoucherType"].ToString() == "BANK_PMNT")
                                {
                                    DataTable dtDebtorReceipt = Debtors.GetCashReceiptDetails("BANK_PMNT", drledg["VoucherNumber"].ToString(), LedgerID1);
                                    if (dtDebtorReceipt.Rows.Count > 0)
                                    {
                                        for (int i = 0; i < dtDebtorReceipt.Rows.Count; i++)
                                        {
                                            FirstPeriod1 = 0;
                                            SecondPeriod1 = 0;
                                            ThirdPeriod1 = 0;
                                            FourthPeriod1 = 0;
                                            GreaterThanFouthPeriod1 = 0;
                                            DataRow drcashreceipt = dtDebtorReceipt.Rows[0];
                                            string TLedgerName = Ledger.GetLedgerNameFromID(Convert.ToInt32(drcashreceipt["LedgerID"].ToString()));
                                            TotalReceiptAmount = TotalReceiptAmount + Convert.ToDouble(drcashreceipt["Amount"].ToString());
                                            ReceiptAmount = Convert.ToDouble(drcashreceipt["Amount"].ToString());
                                            Balance = DrBal - TotalReceiptAmount;
                                            if (CheckPeriod == 1)
                                                FirstPeriod1 = ReceiptAmount * -1;
                                            else if (CheckPeriod == 2)
                                                SecondPeriod1 = ReceiptAmount * -1;
                                            else if (CheckPeriod == 3)
                                                ThirdPeriod1 = ReceiptAmount * -1;
                                            else if (CheckPeriod == 4)
                                                FourthPeriod1 = ReceiptAmount * -1;
                                            else if (CheckPeriod == 5)
                                                GreaterThanFouthPeriod1 = ReceiptAmount * -1;

                                            FirstPeriodRemaining += FirstPeriod1;
                                            SecondPeriodRemaining += SecondPeriod1;
                                            ThirdPeriodRemaining += ThirdPeriod1;
                                            FourthPeriodRemaining += FourthPeriod1;
                                            GreaterThanFouthPeriodRemaining += GreaterThanFouthPeriod1;
                                            LoadAllDebtorsBillWise(LedgerID1, ledgercode1, TLedgerName, FirstPeriod1, SecondPeriod1, ThirdPeriod1, FourthPeriod1, GreaterThanFouthPeriod1, Date.DBToSystem(drcashreceipt["LedgerDate"].ToString()), "CASH_RCPT", drcashreceipt["Voucher_No"].ToString(), "LEDGER", Convert.ToInt32(drcashreceipt["RowID"].ToString()), IsCrystalReport);

                                        }
                                    }
                                    DataTable dtDebtorBankReceipt = Debtors.GetBankReceiptDetails("BANK_PMNT", drledg["VoucherNumber"].ToString(), LedgerID1);
                                    if (dtDebtorBankReceipt.Rows.Count > 0)
                                    {
                                        for (int i = 0; i < dtDebtorBankReceipt.Rows.Count; i++)
                                        {
                                            FirstPeriod1 = 0;
                                            SecondPeriod1 = 0;
                                            ThirdPeriod1 = 0;
                                            FourthPeriod1 = 0;
                                            GreaterThanFouthPeriod1 = 0;
                                            DataRow drbankreceipt = dtDebtorBankReceipt.Rows[0];
                                            string TLedgerName = Ledger.GetLedgerNameFromID(Convert.ToInt32(drbankreceipt["LedgerID"].ToString()));
                                            TotalReceiptAmount = TotalReceiptAmount + Convert.ToDouble(drbankreceipt["Amount"].ToString());
                                            ReceiptAmount = Convert.ToDouble(drbankreceipt["Amount"].ToString());
                                            Balance = DrBal - TotalReceiptAmount;
                                            if (CheckPeriod == 1)
                                                FirstPeriod1 = ReceiptAmount * -1;
                                            else if (CheckPeriod == 2)
                                                SecondPeriod1 = ReceiptAmount * -1;
                                            else if (CheckPeriod == 3)
                                                ThirdPeriod1 = ReceiptAmount * -1;
                                            else if (CheckPeriod == 4)
                                                FourthPeriod1 = ReceiptAmount * -1;
                                            else if (CheckPeriod == 5)
                                                GreaterThanFouthPeriod1 = ReceiptAmount * -1;

                                            FirstPeriodRemaining += FirstPeriod1;
                                            SecondPeriodRemaining += SecondPeriod1;
                                            ThirdPeriodRemaining += ThirdPeriod1;
                                            FourthPeriodRemaining += FourthPeriod1;
                                            GreaterThanFouthPeriodRemaining += GreaterThanFouthPeriod1;
                                            LoadAllDebtorsBillWise(LedgerID1, ledgercode1, TLedgerName, FirstPeriod1, SecondPeriod1, ThirdPeriod1, FourthPeriod1, GreaterThanFouthPeriod1, Date.DBToSystem(drbankreceipt["LedgerDate"].ToString()), "BANK_RCPT", drbankreceipt["Voucher_No"].ToString(), "LEDGER", Convert.ToInt32(drbankreceipt["RowID"].ToString()), IsCrystalReport);

                                        }
                                    }
                                }
                            }
                            if (IsDebtorsZero == false)
                            {
                                LoadAllDebtorsBillWise(0, "", "", FirstPeriodRemaining, SecondPeriodRemaining, ThirdPeriodRemaining, FourthPeriodRemaining, GreaterThanFouthPeriodRemaining, "", "", "Total", "LEDGERHEAD", 0, IsCrystalReport);
                            }
                        }
                    }
                    #endregion 
                }
                #endregion

                ProgressForm.UpdateProgress(100, "Preparing report for display...");
                // Close the dialog if it hasn't been already
                if (ProgressForm.InvokeRequired)
                    ProgressForm.BeginInvoke(new Action(() => ProgressForm.Close()));
           // }
        }

        private void LoadAllDebtors(int ledgerid,string ledgercode,string ledgername,double FirstPeriod,double SecondPeriod,double ThirdPeriod,double FourthPeriod,double FifthPeriod,bool isCrystalReport)
        {
            if (m_DS.Ageing == true)
            {
                if (!isCrystalReport)
                {
                    DebtorsRowsCount = grddebtorsageing.Rows.Count - 1;
                    LedgerView = new SourceGrid.Cells.Views.Cell();
                    if (DebtorsRowsCount % 2 == 1)
                    {
                        LedgerView.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Color.White);
                    }
                    else
                    {
                        LedgerView.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Global.Grid_Color);
                    }
                    grddebtorsageing.Rows.Insert(grddebtorsageing.RowsCount);


                    grddebtorsageing[DebtorsRowsCount, 0] = new SourceGrid.Cells.Cell(ledgercode);
                    grddebtorsageing[DebtorsRowsCount, 0].View = new SourceGrid.Cells.Views.Cell(LedgerView);
                    grddebtorsageing[DebtorsRowsCount, 0].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleLeft;

                    grddebtorsageing[DebtorsRowsCount, 1] = new SourceGrid.Cells.Cell(ledgername);
                    grddebtorsageing[DebtorsRowsCount, 1].View = new SourceGrid.Cells.Views.Cell(LedgerView);
                    grddebtorsageing[DebtorsRowsCount, 1].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleLeft;

                    if (FirstPeriod >= 0)
                        grddebtorsageing[DebtorsRowsCount, 2] = new SourceGrid.Cells.Cell(FirstPeriod.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));//First Period
                    else if (FirstPeriod < 0)
                        grddebtorsageing[DebtorsRowsCount, 2] = new SourceGrid.Cells.Cell("-" + FirstPeriod.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));//First Period

                    grddebtorsageing[DebtorsRowsCount, 2].View = new SourceGrid.Cells.Views.Cell(LedgerView);
                    grddebtorsageing[DebtorsRowsCount, 2].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;

                    if (SecondPeriod >= 0)
                        grddebtorsageing[DebtorsRowsCount, 3] = new SourceGrid.Cells.Cell(SecondPeriod.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));//Second Period
                    else if (SecondPeriod < 0)
                        grddebtorsageing[DebtorsRowsCount, 3] = new SourceGrid.Cells.Cell("-" + SecondPeriod.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));//Second Period
                    grddebtorsageing[DebtorsRowsCount, 3].View = new SourceGrid.Cells.Views.Cell(LedgerView);
                    grddebtorsageing[DebtorsRowsCount, 3].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;


                    if (ThirdPeriod >= 0)
                        grddebtorsageing[DebtorsRowsCount, 4] = new SourceGrid.Cells.Cell(ThirdPeriod.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));//Third Period
                    else if (ThirdPeriod < 0)
                        grddebtorsageing[DebtorsRowsCount, 4] = new SourceGrid.Cells.Cell("-" + ThirdPeriod.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));//Third Period
                    grddebtorsageing[DebtorsRowsCount, 4].View = new SourceGrid.Cells.Views.Cell(LedgerView);
                    grddebtorsageing[DebtorsRowsCount, 4].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;


                    if (FourthPeriod >= 0)
                        grddebtorsageing[DebtorsRowsCount, 5] = new SourceGrid.Cells.Cell(FourthPeriod.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));//Fourth Period
                    else if (FourthPeriod < 0)
                        grddebtorsageing[DebtorsRowsCount, 5] = new SourceGrid.Cells.Cell("-" + FourthPeriod.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));//Fourth Period
                    grddebtorsageing[DebtorsRowsCount, 5].View = new SourceGrid.Cells.Views.Cell(LedgerView);
                    grddebtorsageing[DebtorsRowsCount, 5].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;


                    if (FifthPeriod >= 0)
                        grddebtorsageing[DebtorsRowsCount, 6] = new SourceGrid.Cells.Cell(FifthPeriod.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));
                    else if (FifthPeriod < 0)
                        grddebtorsageing[DebtorsRowsCount, 6] = new SourceGrid.Cells.Cell("-" + FifthPeriod.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));
                    grddebtorsageing[DebtorsRowsCount, 6].View = new SourceGrid.Cells.Views.Cell(LedgerView);
                    grddebtorsageing[DebtorsRowsCount, 6].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;


                    grddebtorsageing[DebtorsRowsCount, 7] = new SourceGrid.Cells.Cell(ledgerid);

                    grddebtorsageing[DebtorsRowsCount, 8] = new SourceGrid.Cells.Cell("");

                }
                if (isCrystalReport)
                {
                    dsDebtorsAeging.Tables["tblDebtorsDAging"].Rows.Add(ledgercode, ledgername, FirstPeriod, SecondPeriod, ThirdPeriod, FourthPeriod, FifthPeriod);
                }
            }
           
        }

        private void LoadAllDebtorsBillWise(int ledgerid, string ledgercode, string ledgername, double FirstPeriod, double SecondPeriod, double ThirdPeriod, double FourthPeriod, double FifthPeriod,string VoucherDate,string VoucherType,string VoucherNumber,string AccType,int rowid, bool isCrystalReport)
        {
            if (m_DS.BillwiseAgeing == true)
            {
                if (!isCrystalReport)
                {
                    DebtorsRowsCount = grddebtorsageing.Rows.Count - 1;
                    if (AccType == "LEDGER")
                    {
                        LedgerView = new SourceGrid.Cells.Views.Cell();
                        if (DebtorsRowsCount % 2 == 1)
                        {
                            LedgerView.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Color.White);
                        }
                        else
                        {
                            LedgerView.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Global.Grid_Color);
                        }
                    }

                    if (AccType == "LEDGERHEAD")
                    {
                        //Ledger header
                        LedgerHeadView = new SourceGrid.Cells.Views.Cell();
                        LedgerHeadView.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Color.LightYellow);
                        LedgerHeadView.ForeColor = Color.FromKnownColor(KnownColor.ActiveCaptionText);
                        LedgerHeadView.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleCenter;
                        LedgerHeadView.Border = DevAge.Drawing.RectangleBorder.NoBorder;
                        LedgerHeadView.Font = new Font(LangMgr.GetFont().FontFamily, LangMgr.GetFont().Size + 1, FontStyle.Bold);
                    }
                    SourceGrid.Cells.Views.Cell CurrentView = new SourceGrid.Cells.Views.Cell();
                    switch (AccType)
                    {
                        case "LEDGER":
                            CurrentView = LedgerView;
                            break;
                        case "LEDGERHEAD":
                            CurrentView = LedgerHeadView;
                            break;
                        case "TOTAL":
                            CurrentView = TotalView;
                            break;
                    }
                    grddebtorsageing.Rows.Insert(grddebtorsageing.RowsCount);
                    grddebtorsageing[DebtorsRowsCount, 0] = new SourceGrid.Cells.Cell(ledgercode);
                    grddebtorsageing[DebtorsRowsCount, 0].View = new SourceGrid.Cells.Views.Cell(CurrentView);
                    grddebtorsageing[DebtorsRowsCount, 0].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleLeft;

                    grddebtorsageing[DebtorsRowsCount, 1] = new SourceGrid.Cells.Cell(ledgername);
                    grddebtorsageing[DebtorsRowsCount, 1].View = new SourceGrid.Cells.Views.Cell(CurrentView);
                    grddebtorsageing[DebtorsRowsCount, 1].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleLeft;

                    grddebtorsageing[DebtorsRowsCount, 2] = new SourceGrid.Cells.Cell(VoucherType);
                    grddebtorsageing[DebtorsRowsCount, 2].View = new SourceGrid.Cells.Views.Cell(CurrentView);
                    grddebtorsageing[DebtorsRowsCount, 2].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleLeft;

                    grddebtorsageing[DebtorsRowsCount, 3] = new SourceGrid.Cells.Cell(VoucherNumber);
                    grddebtorsageing[DebtorsRowsCount, 3].View = new SourceGrid.Cells.Views.Cell(CurrentView);
                    grddebtorsageing[DebtorsRowsCount, 3].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleLeft;

                    grddebtorsageing[DebtorsRowsCount, 4] = new SourceGrid.Cells.Cell(VoucherDate);
                    grddebtorsageing[DebtorsRowsCount, 4].View = new SourceGrid.Cells.Views.Cell(CurrentView);
                    grddebtorsageing[DebtorsRowsCount, 4].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleLeft;

                    if (FirstPeriod >= 0)
                        grddebtorsageing[DebtorsRowsCount, 5] = new SourceGrid.Cells.Cell(FirstPeriod.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));//First Period
                    else if (FirstPeriod < 0)
                        grddebtorsageing[DebtorsRowsCount, 5] = new SourceGrid.Cells.Cell("-" + FirstPeriod.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));//First Period

                    grddebtorsageing[DebtorsRowsCount, 5].View = new SourceGrid.Cells.Views.Cell(CurrentView);
                    grddebtorsageing[DebtorsRowsCount, 5].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;

                    if (SecondPeriod >= 0)
                        grddebtorsageing[DebtorsRowsCount, 6] = new SourceGrid.Cells.Cell(SecondPeriod.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));//Second Period
                    else if (SecondPeriod < 0)
                        grddebtorsageing[DebtorsRowsCount, 6] = new SourceGrid.Cells.Cell("-" + SecondPeriod.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));//Second Period
                    grddebtorsageing[DebtorsRowsCount, 6].View = new SourceGrid.Cells.Views.Cell(CurrentView);
                    grddebtorsageing[DebtorsRowsCount, 6].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;


                    if (ThirdPeriod >= 0)
                        grddebtorsageing[DebtorsRowsCount, 7] = new SourceGrid.Cells.Cell(ThirdPeriod.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));//Third Period
                    else if (ThirdPeriod < 0)
                        grddebtorsageing[DebtorsRowsCount, 7] = new SourceGrid.Cells.Cell("-" + ThirdPeriod.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));//Third Period
                    grddebtorsageing[DebtorsRowsCount, 7].View = new SourceGrid.Cells.Views.Cell(CurrentView);
                    grddebtorsageing[DebtorsRowsCount, 7].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;


                    if (FourthPeriod >= 0)
                        grddebtorsageing[DebtorsRowsCount, 8] = new SourceGrid.Cells.Cell(FourthPeriod.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));//Fourth Period
                    else if (FourthPeriod < 0)
                        grddebtorsageing[DebtorsRowsCount, 8] = new SourceGrid.Cells.Cell("-" + FourthPeriod.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));//Fourth Period
                    grddebtorsageing[DebtorsRowsCount, 8].View = new SourceGrid.Cells.Views.Cell(CurrentView);
                    grddebtorsageing[DebtorsRowsCount, 8].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;


                    if (FifthPeriod >= 0)
                        grddebtorsageing[DebtorsRowsCount, 9] = new SourceGrid.Cells.Cell(FifthPeriod.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));
                    else if (FifthPeriod < 0)
                        grddebtorsageing[DebtorsRowsCount, 9] = new SourceGrid.Cells.Cell("-" + FifthPeriod.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));
                    grddebtorsageing[DebtorsRowsCount, 9].View = new SourceGrid.Cells.Views.Cell(CurrentView);
                    grddebtorsageing[DebtorsRowsCount, 9].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;


                    grddebtorsageing[DebtorsRowsCount, 10] = new SourceGrid.Cells.Cell(ledgerid);

                    grddebtorsageing[DebtorsRowsCount, 11] = new SourceGrid.Cells.Cell(rowid);



                }
                if (isCrystalReport)
                {
                    dsBillWiseAging.Tables["tblDebtorsBillWiseAging"].Rows.Add(ledgername, VoucherType, VoucherNumber, VoucherDate, FirstPeriod, SecondPeriod, ThirdPeriod, FourthPeriod, FifthPeriod);
                }
            }
        }
        private void MakeHeader()
        {
            if (m_DS.Ageing == true)
            {
                grddebtorsageing.Rows.Insert(0);
                grddebtorsageing[0, 0] = new MyHeader("Code");
                grddebtorsageing[0, 1] = new MyHeader("Account Name");
                grddebtorsageing[0, 2] = new MyHeader("<" + m_DS.FirstPeriod + "Days");
                grddebtorsageing[0, 3] = new MyHeader("<" + m_DS.SecondPeriod + "Days");
                grddebtorsageing[0, 4] = new MyHeader("<" + m_DS.ThirdPeriod + "Days");
                grddebtorsageing[0, 5] = new MyHeader("<" + m_DS.FourthPeriod + "Days");
                grddebtorsageing[0, 6] = new MyHeader(">=" + m_DS.FourthPeriod + "Days");
                grddebtorsageing[0, 7] = new MyHeader("LedgerID");
                grddebtorsageing[0, 8] = new MyHeader("RowID");

                //Define the width of column size
                grddebtorsageing[0, 0].Column.Width = 75;
                grddebtorsageing[0, 1].Column.Width = 225;
                grddebtorsageing[0, 2].Column.Width = 110;
                grddebtorsageing[0, 3].Column.Width = 110;
                grddebtorsageing[0, 4].Column.Width = 110;
                grddebtorsageing[0, 5].Column.Width = 110;
                grddebtorsageing[0, 6].Column.Width = 110;
                grddebtorsageing[0, 7].Column.Width = 1;
                grddebtorsageing[0, 8].Column.Width = 1;

                //Code for making column invisible
                grddebtorsageing.Columns[7].Visible = false;
                
                grddebtorsageing.Columns[8].Visible = false;
            }
            else if(m_DS.BillwiseAgeing==true)
            {
                grddebtorsageing.Rows.Insert(0);
                grddebtorsageing[0, 0] = new MyHeader("Code");
                grddebtorsageing[0, 1] = new MyHeader("Account Name");
                grddebtorsageing[0, 2] = new MyHeader("V. Type");
                grddebtorsageing[0, 3] = new MyHeader("Voucher No.");
                grddebtorsageing[0, 4] = new MyHeader("V. Date");
                grddebtorsageing[0, 5] = new MyHeader("<" + m_DS.FirstPeriod + "Days");
                grddebtorsageing[0, 6] = new MyHeader("<" + m_DS.SecondPeriod + "Days");
                grddebtorsageing[0, 7] = new MyHeader("<" + m_DS.ThirdPeriod + "Days");
                grddebtorsageing[0, 8] = new MyHeader("<" + m_DS.FourthPeriod + "Days");
                grddebtorsageing[0, 9] = new MyHeader(">=" + m_DS.FourthPeriod + "Days");
                grddebtorsageing[0, 10] = new MyHeader("LedgerID");
                grddebtorsageing[0, 11] = new MyHeader("RowID");

                //Define the width of column size
                grddebtorsageing[0, 0].Column.Width = 75;
                grddebtorsageing[0, 1].Column.Width = 220;
                grddebtorsageing[0, 2].Column.Width = 80;
                grddebtorsageing[0, 3].Column.Width = 90;
                grddebtorsageing[0, 4].Column.Width = 80;
                grddebtorsageing[0, 5].Column.Width = 70;
                grddebtorsageing[0, 6].Column.Width = 70;
                grddebtorsageing[0, 7].Column.Width = 70;
                grddebtorsageing[0, 8].Column.Width = 70;
                grddebtorsageing[0, 9].Column.Width = 75;
                grddebtorsageing[0, 10].Column.Width = 1;
                grddebtorsageing[0, 11].Column.Width = 1;

                //Code for making column invisible
                grddebtorsageing.Columns[0].Visible = false;
                grddebtorsageing.Columns[10].Visible = false;
                grddebtorsageing.Columns[11].Visible = false;
            }
        }

        private class MyHeader : SourceGrid.Cells.ColumnHeader
        {
            public MyHeader(object value)
                : base(value)
            {
                //1 Header Row
                SourceGrid.Cells.Views.ColumnHeader view = new SourceGrid.Cells.Views.ColumnHeader();
                view.Font = new Font("Arial", 10, FontStyle.Bold);
                view.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleCenter;
                View = view;

                AutomaticSortEnabled = false;
            }

        }
        private void DisplayBanner()
        {
            if (m_DS.ToDate != null)
            {
                lblAsonDate.Text = "As on Date: " + Date.ToSystem((DateTime)m_DS.ToDate);
            }
            else//if date range is not selected then siimply pass the current date time
            {
                lblAsonDate.Text = "As on Date: " + Date.ToSystem(DateTime.Today);
            }
        }

        private string ReadAllAccClassID()
        {
            #region  AccountingClassID

            ArrayList arrChildAccClassIDs = new ArrayList();
            foreach (object j in m_DS.AccClassID)
            {
                AccountClass.GetChildIDs(Convert.ToInt32(j), ref arrChildAccClassIDs);

            }
            m_DS.AccClassID.AddRange(arrChildAccClassIDs);

            #endregion

            System.Text.Encoding AEncoder = System.Text.Encoding.Unicode;
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            System.Xml.XmlTextWriter tw = new System.Xml.XmlTextWriter(ms, AEncoder);

            tw.WriteStartDocument();
            #region  Accountclass
            tw.WriteStartElement("LEDGERTRANSACT");
            {
                //Write Checked Accounting class ID
                try
                {
                    tw.WriteStartElement("ACCCLASSIDS");
                    foreach (string tag in m_DS.AccClassID)
                    {
                        AccClassID.Add(Convert.ToInt32(tag));
                        tw.WriteElementString("AccID", Convert.ToInt32(tag).ToString());
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

        private string ReadAllProjectID()
        {
            #region  ProjectID
            ArrayList arrchildProjectIds = new ArrayList();
            Project.GetChildProjects(Convert.ToInt32(m_DS.ProjectID), ref arrchildProjectIds);
            ArrayList ProjectIDCollection = new ArrayList();
            foreach (object obj in arrchildProjectIds)
            {
                int p = (int)obj;
                ProjectIDCollection.Add(p.ToString());
            }

            string ProjectIDS = "'" + m_DS.ProjectID + "'";

            for (int iproject = 0; iproject < ProjectIDCollection.Count; iproject++)
            {
                ProjectIDS += "," + "'" + (ProjectIDCollection[iproject].ToString()) + "'";
            }
            #endregion

            System.Text.Encoding AEncoder = System.Text.Encoding.Unicode;
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            System.Xml.XmlTextWriter tw = new System.Xml.XmlTextWriter(ms, AEncoder);

            tw.WriteStartDocument();
            #region  Accountclass
            tw.WriteStartElement("LEDGERTRANSACT");
            {
                //Write Checked Accounting class ID
                try
                {
                    tw.WriteStartElement("PROJECTIDS");
                    tw.WriteElementString("PID", Convert.ToInt32(m_DS.ProjectID).ToString());
                    foreach (string tag in ProjectIDCollection)
                    {
                        //AccClassID.Add(Convert.ToInt32(tag));
                        tw.WriteElementString("PID", Convert.ToInt32(tag).ToString());
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
            return strXML;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            grddebtorsageing.Redim(0, 0);

            this.Cursor = Cursors.WaitCursor;
            //Load all over again
            frmdebtorsageing_Load(sender, e);

            this.Cursor = Cursors.Default;
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
            if (m_DS.Ageing == true)
                dsDebtorsAeging.Clear();//It clear the previous records of dataset on crystal report...when this button is pressed
            else if (m_DS.BillwiseAgeing == true)
                dsBillWiseAging.Clear();

            //otherwise it populate the records again and again

            double DebitSum, CreditSum;
            DebitSum = CreditSum = 0;
            rptDebtorsAeging rpt = new rptDebtorsAeging();
            rptDebtorsBillWiseAeging rpt1 = new rptDebtorsBillWiseAeging();
            //Fill the logo on the report
            if (m_DS.Ageing == true)
            {
                
                Misc.WriteLogo(dsDebtorsAeging, "tblImage");
                //Set DataSource to be dsTrial dataset on the report
                rpt.SetDataSource(dsDebtorsAeging);
            }
            else if (m_DS.BillwiseAgeing == true)
            {
              
                Misc.WriteLogo(dsBillWiseAging, "tblImage");
                rpt1.SetDataSource(dsBillWiseAging);
            }
           
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
            CrystalDecisions.Shared.ParameterDiscreteValue pdvPreparedBy = new CrystalDecisions.Shared.ParameterDiscreteValue();
            CrystalDecisions.Shared.ParameterDiscreteValue pdvCheckedBy = new CrystalDecisions.Shared.ParameterDiscreteValue();
            CrystalDecisions.Shared.ParameterDiscreteValue pdvApprovedBy = new CrystalDecisions.Shared.ParameterDiscreteValue();

            CrystalDecisions.Shared.ParameterDiscreteValue pdvFont = new CrystalDecisions.Shared.ParameterDiscreteValue();

            //Update the progressbar
            ProgressForm.UpdateProgress(50, "Initializing Report Viewer...");

            if (m_DS.Ageing == true)
            {
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

                pdvReport_Head.Value = "DebtorsAeging";
                pvCollection.Clear();
                pvCollection.Add(pdvReport_Head);
                rpt.DataDefinition.ParameterFields["Report_Head"].ApplyCurrentValues(pvCollection);

                // pdvFiscal_Year.Value = "Fiscal Year:" +Date.ToSystem( m_CompanyDetails.FYFrom);
                pdvFiscal_Year.Value = "Fiscal Year:" + m_CompanyDetails.FiscalYear;
                pvCollection.Clear();
                pvCollection.Add(pdvFiscal_Year);
                rpt.DataDefinition.ParameterFields["Fiscal_Year"].ApplyCurrentValues(pvCollection);

                //Update the progressbar
                ProgressForm.UpdateProgress(80, "Calculating Parameters...");

                if (m_DS.ToDate != null)
                    pdvReport_Date.Value = "As On Date:" + Date.ToSystem((DateTime)m_DS.ToDate);
                else
                    pdvReport_Date.Value = "As On Date:" + Date.ToSystem(Date.GetServerDate());
                pvCollection.Clear();
                pvCollection.Add(pdvReport_Date);
                rpt.DataDefinition.ParameterFields["Report_Date"].ApplyCurrentValues(pvCollection);

                pdvPreparedBy.Value = Settings.GetSettings("PREPARED_BY");
                pvCollection.Clear();
                pvCollection.Add(pdvPreparedBy);
                rpt.DataDefinition.ParameterFields["Prepared_By"].ApplyCurrentValues(pvCollection);

                pdvCheckedBy.Value = Settings.GetSettings("CHECKED_BY");
                pvCollection.Clear();
                pvCollection.Add(pdvCheckedBy);
                rpt.DataDefinition.ParameterFields["Checked_By"].ApplyCurrentValues(pvCollection);

                pdvApprovedBy.Value = Settings.GetSettings("APPROVED_BY");
                pvCollection.Clear();
                pvCollection.Add(pdvApprovedBy);
                rpt.DataDefinition.ParameterFields["Approved_By"].ApplyCurrentValues(pvCollection);


                DisplayDebtorsAgeing(true);


                CrystalDecisions.Shared.ExportOptions CrExportOptions = new CrystalDecisions.Shared.ExportOptions();
                DiskFileDestinationOptions CrDiskFileDestinationOptions = new DiskFileDestinationOptions();
                CrDiskFileDestinationOptions.DiskFileName = FileName;

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
                frm.WindowState = FormWindowState.Maximized;
            }
            else if (m_DS.BillwiseAgeing == true)
            {
                pdvFont.Value = "Arial";
                pvCollection.Clear();
                pvCollection.Add(pdvFont);
                rpt1.DataDefinition.ParameterFields["Font"].ApplyCurrentValues(pvCollection);

                CompanyDetails m_CompanyDetails = CompanyInfo.GetInfo();

                pdvCompany_Name.Value = m_CompanyDetails.CompanyName;
                pvCollection.Clear();
                pvCollection.Add(pdvCompany_Name);
                rpt1.DataDefinition.ParameterFields["Company_Name"].ApplyCurrentValues(pvCollection);

                pdvCompany_Address.Value = m_CompanyDetails.Address1;
                pvCollection.Clear();
                pvCollection.Add(pdvCompany_Address);
                rpt1.DataDefinition.ParameterFields["Company_Address"].ApplyCurrentValues(pvCollection);

                pdvCompany_PAN.Value = m_CompanyDetails.PAN;
                pvCollection.Clear();
                pvCollection.Add(pdvCompany_PAN);
                rpt1.DataDefinition.ParameterFields["Company_PAN"].ApplyCurrentValues(pvCollection);

                pdvCompany_Phone.Value = "Phone No.: " + m_CompanyDetails.Telephone;
                pvCollection.Clear();
                pvCollection.Add(pdvCompany_Phone);
                rpt1.DataDefinition.ParameterFields["Company_Phone"].ApplyCurrentValues(pvCollection);

                pdvCompany_Slogan.Value = m_CompanyDetails.Website;
                pvCollection.Clear();
                pvCollection.Add(pdvCompany_Slogan);
                rpt1.DataDefinition.ParameterFields["Company_Slogan"].ApplyCurrentValues(pvCollection);

                pdvReport_Head.Value = "DebtorsAeging";
                pvCollection.Clear();
                pvCollection.Add(pdvReport_Head);
                rpt1.DataDefinition.ParameterFields["Report_Head"].ApplyCurrentValues(pvCollection);

                // pdvFiscal_Year.Value = "Fiscal Year:" +Date.ToSystem( m_CompanyDetails.FYFrom);
                pdvFiscal_Year.Value = "Fiscal Year:" + m_CompanyDetails.FiscalYear;
                pvCollection.Clear();
                pvCollection.Add(pdvFiscal_Year);
                rpt1.DataDefinition.ParameterFields["Fiscal_Year"].ApplyCurrentValues(pvCollection);

                //Update the progressbar
                ProgressForm.UpdateProgress(80, "Calculating Parameters...");

                if (m_DS.ToDate != null)
                    pdvReport_Date.Value = "As On Date:" + Date.ToSystem((DateTime)m_DS.ToDate);
                else
                    pdvReport_Date.Value = "As On Date:" + Date.ToSystem(Date.GetServerDate());
                pvCollection.Clear();
                pvCollection.Add(pdvReport_Date);
                rpt1.DataDefinition.ParameterFields["Report_Date"].ApplyCurrentValues(pvCollection);

                pdvPreparedBy.Value = Settings.GetSettings("PREPARED_BY");
                pvCollection.Clear();
                pvCollection.Add(pdvPreparedBy);
                rpt1.DataDefinition.ParameterFields["Prepared_By"].ApplyCurrentValues(pvCollection);

                pdvCheckedBy.Value = Settings.GetSettings("CHECKED_BY");
                pvCollection.Clear();
                pvCollection.Add(pdvCheckedBy);
                rpt1.DataDefinition.ParameterFields["Checked_By"].ApplyCurrentValues(pvCollection);

                pdvApprovedBy.Value = Settings.GetSettings("APPROVED_BY");
                pvCollection.Clear();
                pvCollection.Add(pdvApprovedBy);
                rpt1.DataDefinition.ParameterFields["Approved_By"].ApplyCurrentValues(pvCollection);


                DisplayDebtorsAgeing(true);


                CrystalDecisions.Shared.ExportOptions CrExportOptions = new CrystalDecisions.Shared.ExportOptions();
                DiskFileDestinationOptions CrDiskFileDestinationOptions = new DiskFileDestinationOptions();
                CrDiskFileDestinationOptions.DiskFileName = FileName;

                //Finally, show the report form
                frmReportViewer frm = new frmReportViewer();
                frm.SetReportSource(rpt1);
                //Update the progressbar
                ProgressForm.UpdateProgress(100, "Showing Report...");

                // Close the dialog
                ProgressForm.CloseForm();

                switch (prntDirect)
                {
                    case 1:
                        rpt1.PrintOptions.PrinterName = "";
                        rpt1.PrintToPrinter(1, false, 0, 0);
                        prntDirect = 0;
                        return;
                    case 2:
                        ExcelFormatOptions CrFormatTypeOptions = new ExcelFormatOptions();
                        CrExportOptions = rpt.ExportOptions;
                        CrExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                        CrExportOptions.ExportFormatType = ExportFormatType.Excel;
                        CrExportOptions.DestinationOptions = CrDiskFileDestinationOptions;
                        CrExportOptions.FormatOptions = CrFormatTypeOptions;
                        rpt1.Export();
                        rpt1.Close();
                        return;
                    case 3:
                        PdfFormatOptions CrFormatTypeOptionsPdf = new PdfFormatOptions();
                        CrExportOptions = rpt.ExportOptions;
                        CrExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                        CrExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                        CrExportOptions.DestinationOptions = CrDiskFileDestinationOptions;
                        CrExportOptions.FormatOptions = CrFormatTypeOptionsPdf;
                        rpt1.Export();
                        rpt1.Close();
                        return;
                    case 4:
                        ExcelFormatOptions CrFormatTypeOptionsEmail = new ExcelFormatOptions();
                        CrExportOptions = rpt.ExportOptions;
                        CrExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                        CrExportOptions.ExportFormatType = ExportFormatType.Excel;
                        CrExportOptions.DestinationOptions = CrDiskFileDestinationOptions;
                        CrExportOptions.FormatOptions = CrFormatTypeOptionsEmail;
                        rpt1.Export();
                        frmemail sendemail = new frmemail(FileName, 1);
                        sendemail.ShowDialog();
                        rpt1.Close();
                        return;
                    default:
                        frm.Show();
                        frm.WindowState = FormWindowState.Maximized;
                        break;
                }
                frm.WindowState = FormWindowState.Maximized;
            }
            
        }

        private void btnRefresh_Click_1(object sender, EventArgs e)
        {
            grddebtorsageing.Redim(0, 0);

            this.Cursor = Cursors.WaitCursor;
            //Load all over again
            frmdebtorsageing_Load(sender, e);

            this.Cursor = Cursors.Default;
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            prntDirect = 1;
            btnPrintPreview_Click(sender, e);
        }
        

    }
}
