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
using Inventory.CrystalReports;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using DateManager;
using System.Threading;
using System.IO;
using System.Data.SqlClient;
using Inventory.Forms;
using BusinessLogic.Accounting.Reports;

namespace Inventory.Forms.Accounting.Reports
{
    public partial class frmSTransaction : Form
    {
        private int? LedgerID = null;
        private int? FromDate = null;
        private int? ToDate = null;
        private enum AccountType
        {
            ALL_LEDGERS, PARTICULAR_LEDGER, PARTICULAR_GROUP

        }

        private enum PrintType
        {
            None,
            DirectPrint,
            Excel,
            PDF,
            CrystalReport,
            Email
        }

        private DataSet.dsTransaction1 dsTransaction = new DataSet.dsTransaction1();
        private SourceGrid.Cells.Controllers.CustomEvents dblClick;//Double click for Transaction
        private double TotalDebitAmount = 0;
        private double TotalCreditAmount = 0;
        private int TransactRowsCount;
        private TransactionSettingForSummmary m_TSS;
        string m_AccountTypes = "";
        Transaction m_Transaction = new Transaction();
        //Different grid views
        private SourceGrid.Cells.Views.Cell GroupView;
        private SourceGrid.Cells.Views.Cell LedgerView;
        private SourceGrid.Cells.Views.Cell LedgerHeadView;
        ArrayList AccClassID = new ArrayList();
        private int prntDirect = 0;
        private string FileName = "";
        string ProjectIDsXMLString = "";

        //For Export Menu
        ContextMenu Menu_Export;
        //public frmSTransaction()
        //{
        //    InitializeComponent();
        //} 

         public frmSTransaction(TransactionSettingForSummmary TSS)
        {
            try
            {
                //This is the constructor which handles the transaction of particular Ledger only
                InitializeComponent();
                m_TSS = new TransactionSettingForSummmary();
                m_TSS.LedgerID = TSS.LedgerID;
                m_AccountTypes = "PARTICULAR_LEDGER";//Set account type as Particular Ledger for generating information of Particular Ledger
                m_TSS.FromDate = TSS.FromDate;
                m_TSS.ToDate = TSS.ToDate;
                m_TSS.HasDateRange = TSS.HasDateRange;
                m_TSS.AccClassID = TSS.AccClassID;
                m_TSS.ProjectID = TSS.ProjectID;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

         public frmSTransaction(TransactionSettingForSummmary TSS, string AccountType)
        {
            try
            {
                InitializeComponent();
                m_TSS = new TransactionSettingForSummmary();
                if (AccountType == "ALL_LEDGERS")//If all ledgers the simply pass the 0 ID for LedgerID
                {
                    m_TSS.LedgerID = 0;//If 0 ledgerID is passed then simply we understand it call thoes method we concern with all ledgers
                    m_AccountTypes = AccountType;
                    m_TSS.AccClassID = TSS.AccClassID;
                }
                else//For particular Group Account         
                {
                    m_TSS.AccountGroupID = TSS.AccountGroupID;
                    m_AccountTypes = AccountType;
                }
                m_TSS.AccClassID = TSS.AccClassID;
                m_TSS.FromDate = TSS.FromDate;
                m_TSS.ToDate = TSS.ToDate;
                m_TSS.HasDateRange = TSS.HasDateRange;
                m_TSS.ProjectID = TSS.ProjectID;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void DisplayBannarForSummary()
         {
             CompanyDetails m_CompanyDetails = CompanyInfo.GetInfo();
             if (m_TSS.FromDate != null && m_TSS.ToDate != null)
             {
                 lblAllSettings.Text = "From: " + Date.ToSystem(Convert.ToDateTime(m_TSS.FromDate)) + " To: " + Date.ToSystem(Convert.ToDateTime(m_TSS.ToDate));
             }
             if (m_TSS.ToDate != null)
             {
                 lblAllSettings.Text = "As on Date: " + Date.ToSystem(Convert.ToDateTime(m_TSS.ToDate));
             }
             if (m_TSS.FromDate != null)
             {
                 //This is actually not applicable
                 lblAllSettings.Text = "From: " + Date.ToSystem(Convert.ToDateTime(m_TSS.FromDate));
             }

             DataTable dtProjectInfo = Project.GetProjectByID(Convert.ToInt32(m_TSS.ProjectID), LangMgr.DefaultLanguage);
             if (m_TSS.ProjectID != 0)
             {
                 DataRow drProjectInfo = dtProjectInfo.Rows[0];
                 lblProjectName.Text = " Project: " + drProjectInfo["Name"].ToString();
             }
             else
             {
                 lblProjectName.Text = " Project: " + "All";
             }
         }       

     //Customized header
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

        /// <summary>
        /// Gets the selected root accounting class ID
        /// </summary>
        /// <returns></returns>
        private int GetRootAccClassID()
        {
            if (m_TSS.AccClassID.Count > 0)
            {
                //Find Root Class
                DataTable dtTemp = AccountClass.GetRootAccClass(Convert.ToInt32(m_TSS.AccClassID[0]));
                return Convert.ToInt32(dtTemp.Rows[0]["AccClassID"]);

            }

            return 1;//The default root class ID
        }

 
        /// <summary>
        /// Read all Accountclass 
        /// </summary>
        /// <returns></returns>
        private string ReadAllAccClassID()
        {
            #region  AccountingClassID

            ArrayList arrChildAccClassIDs = new ArrayList();
            foreach (object j in m_TSS.AccClassID)
            {
                AccountClass.GetChildIDs(Convert.ToInt32(j), ref arrChildAccClassIDs);

            }
            m_TSS.AccClassID.AddRange(arrChildAccClassIDs);      

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
                    foreach (string tag in m_TSS.AccClassID)
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

        private void DisplayTransaction(bool IsCrystalReport)
        {   
            TransactRowsCount = 1;
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
            string AccClassIDsXMLString = ReadAllAccClassID();
            string ProjectIDsXMLString = ReadAllProjectID();
            DataTable dtLedgerTransactInfo = new DataTable();

            //Make both dates null if date range is not selected
            if (!m_TSS.HasDateRange)
            {
                m_TSS.FromDate =null;   
                m_TSS.ToDate = null;
            }     
            double m_dbal = 0;
            double m_cbal = 0;
            try
            {
                #region for All Ledgers

                if (m_AccountTypes == "ALL_LEDGERS")//for all ledger
                {
                     ProgressForm.UpdateProgress(40, "Calculating ledger balance...");
                    if (!IsCrystalReport)//just for  Grid not for Crystal report
                        Orientation();//orientation purpose for grid only
                  
                    //use loop
                    DataTable dtLedgerInfo = Ledger.GetLedgerInfo(-1, LangMgr.DefaultLanguage);
                    foreach (DataRow drLedgerInfol in dtLedgerInfo.Rows)                    
                    {
                        LedgerID = Convert.ToInt32(drLedgerInfol["LedgerID"]);

                        //For to write ledger name 
                        double DrOpBal, CrOpBal;
                        DrOpBal = CrOpBal = 0;

                        #region BLOCK FOR OPENING BALANCE
                        //Show the opening Balance of corresponding Ledger

                        DataTable dtOpLedgerInfo = OpeningBalance.GetAccClassOpeningBalance(GetRootAccClassID(), LedgerID);
                        foreach (DataRow drLedgerInfo in dtOpLedgerInfo.Rows)
                        {
                            //If date range is selected. Get all transaction details before that FROM date.
                            #region When datetime is selected
                            if (m_TSS.FromDate != null && m_TSS.ToDate != null)
                            {

                                if (drLedgerInfo["OpenBalDrCr"].ToString().ToUpper() == "DEBIT")//IF ledger has Debit openig balance
                                {
                                    DrOpBal += (Convert.ToDouble(drLedgerInfo["OpenBal"]));
                                }

                                else if (drLedgerInfo["OpenBalDrCr"].ToString().ToUpper() == "CREDIT")//IF ledger has credit Opening balance
                                {
                                    CrOpBal += (Convert.ToDouble(drLedgerInfo["OpenBal"]));
                                }  


                                if (m_TSS.HasDateRange == true)
                                {
                                    Transaction.GetLedgerBalanceForAccountLedger(null, m_TSS.FromDate, Convert.ToInt32(drLedgerInfo["LedgerID"].ToString()), ref m_dbal, ref m_cbal, m_TSS.AccClassID, m_TSS.ProjectID);
                                }
     
                                DrOpBal = m_dbal;
                                CrOpBal = m_cbal;

                                m_dbal = 0;//For to make the current balance as opening balance dr
                                m_cbal = 0;//For to make the current balance as opening balance cr
                            }

                            #endregion

                            #region When datetime is Not selected

                            else if (m_TSS.FromDate == null && m_TSS.ToDate == null)
                            {
                                if (drLedgerInfo["OpenBalDrCr"].ToString().ToUpper() == "DEBIT")//IF ledger has Debit openig balance
                                {
                                    DrOpBal += (Convert.ToDouble(drLedgerInfo["OpenBal"]));
                                }
                                else if (drLedgerInfo["OpenBalDrCr"].ToString().ToUpper() == "CREDIT")//IF ledger has credit Opening balance
                                {
                                    CrOpBal += (Convert.ToDouble(drLedgerInfo["OpenBal"]));
                                }
                            }

                            #endregion

                        }

                        #endregion


     #region details of transactions
                        //To check whether the Opening balance should be DR OR CR
                        DataTable dt = Transaction.GetLedgerTransaction(LedgerID, AccClassIDsXMLString, m_TSS.FromDate, m_TSS.ToDate, ProjectIDsXMLString);
                        if(dt.Rows.Count>0) //If only it has records
                        {
                            DataTable dtLedgerName = Ledger.GetLedgerInfo(LedgerID, LangMgr.DefaultLanguage);
                            foreach (DataRow drLedgerName in dtLedgerName.Rows)
                            {
                                string OpBalanceDrCr = "DR";
                                double OpeningBal = 0;
                                
                                if (DrOpBal > 0 && CrOpBal > 0 && DrOpBal > CrOpBal)
                                {
                                    OpBalanceDrCr = "DR";
                                    OpeningBal = (DrOpBal - CrOpBal);
                                }
                                else if (DrOpBal > 0 && CrOpBal > 0 && DrOpBal < CrOpBal)
                                {
                                    OpBalanceDrCr = "CR";
                                    OpeningBal =(CrOpBal - DrOpBal);
                                }
                                else if (DrOpBal == CrOpBal)
                                {
                                    OpBalanceDrCr = "DR";
                                    OpeningBal = 0;
                                }
                                else if (DrOpBal > 0)
                                {
                                    OpBalanceDrCr = "DR";
                                    OpeningBal = DrOpBal;
                                }
                                else if (CrOpBal > 0)
                                {
                                    OpBalanceDrCr = "CR";
                                    OpeningBal = CrOpBal;
                                }
                                ShowLedgerTransaction(dt, LedgerID, OpeningBal, OpBalanceDrCr, IsCrystalReport);
                            }

                        }
     #endregion
                    }
                }
  #endregion

                     
                #region for Particular Group
                else if (m_AccountTypes == "PARTICULAR_GROUP")//For generating transaction of Specific Account Group
                {
                    if (!IsCrystalReport)//just need to write on grid
                        Orientation();


                    DataTable dtLedgerIDsByGrpID = Ledger.GetAllLedger(Convert.ToInt32(m_TSS.AccountGroupID));//Get all ledger info according to GroupID...Here,we will obtain all the ledger given groupid and its uder subgroupid
                    foreach (DataRow drLedgerIDsByGrpID in dtLedgerIDsByGrpID.Rows)
                    {
                        LedgerID = Convert.ToInt32(drLedgerIDsByGrpID["LedgerID"]);

                        ///for to write ledger name

                        double DrOpBal, CrOpBal;
                        DrOpBal = CrOpBal = 0;


                        #region BLOCK FOR OPENING BALANCE
                        //Show the opening Balance of corresponding Ledger

                        DataTable dtOpLedgerInfo = OpeningBalance.GetAccClassOpeningBalance(GetRootAccClassID(), LedgerID);
                        foreach (DataRow drLedgerInfo in dtOpLedgerInfo.Rows)
                        {
                            //If date range is selected. Get all transaction details before that FROM date.

                            if (m_TSS.FromDate != null && m_TSS.ToDate != null)
                            {

                            #region   When datetime is selected
                                if (m_TSS.HasDateRange == true)                                
                                {
                                    Transaction.GetLedgerBalanceForAccountLedger(null, m_TSS.FromDate, Convert.ToInt32(drLedgerInfo["LedgerID"].ToString()), ref m_dbal, ref m_cbal, m_TSS.AccClassID, m_TSS.ProjectID);
                                }

                                if (drLedgerInfo["OpenBalDrCr"].ToString().ToUpper() == "DEBIT")//IF ledger has Debit openig balance
                                {
                                    DrOpBal += (Convert.ToDouble(drLedgerInfo["OpenBal"]));
                                }
                                else if (drLedgerInfo["OpenBalDrCr"].ToString().ToUpper() == "CREDIT")//IF ledger has credit Opening balance
                                {
                                    CrOpBal += (Convert.ToDouble(drLedgerInfo["OpenBal"]));
                                }

                                DrOpBal = m_dbal;
                                CrOpBal = m_cbal;
                                m_dbal = 0;
                                m_cbal = 0;
                                }         
                           #endregion

                            #region For not date range selection
                            else if (m_TSS.FromDate == null && m_TSS.ToDate == null)
                            {
                                if (drLedgerInfo["OpenBalDrCr"].ToString().ToUpper() == "DEBIT")//IF ledger has Debit openig balance
                                {
                                    DrOpBal += (Convert.ToDouble(drLedgerInfo["OpenBal"]));
                                }
                                else if (drLedgerInfo["OpenBalDrCr"].ToString().ToUpper() == "CREDIT")//IF ledger has credit Opening balance
                                {
                                    CrOpBal += (Convert.ToDouble(drLedgerInfo["OpenBal"]));
                                }
                        
                        
                            }
                        }
                       
                        #endregion
                        #endregion


                        //Write Ledger Transaction Details
                       #region Write Ledger transaction details
                        DataTable dt = Transaction.GetLedgerTransaction(LedgerID, AccClassIDsXMLString, m_TSS.FromDate, m_TSS.ToDate, ProjectIDsXMLString);
                        if (dt.Rows.Count > 0) //If only it has records
                        {
                            DataTable dtLedgerName = Ledger.GetLedgerInfo(LedgerID, LangMgr.DefaultLanguage);
                            foreach (DataRow drLedgerName in dtLedgerName.Rows)
                            {
                                string OpBalanceDrCr = "DR";
                                double OpeningBal = 0;

                                if (DrOpBal > 0 && CrOpBal > 0 && DrOpBal > CrOpBal)
                                {
                                    OpBalanceDrCr = "DR";
                                    OpeningBal = (DrOpBal - CrOpBal);
                                }

                                else if (DrOpBal > 0 && CrOpBal > 0 && DrOpBal < CrOpBal)
                                {
                                    OpBalanceDrCr = "CR";
                                    OpeningBal = CrOpBal - DrOpBal;
                                }

                                else if (DrOpBal == CrOpBal)
                                {
                                    OpBalanceDrCr = "DR";
                                    OpeningBal = 0;
                                }

                                else if (DrOpBal > 0)
                                {
                                    OpBalanceDrCr = "DR";
                                    OpeningBal = DrOpBal;
                                }

                                else if (CrOpBal > 0)
                                {
                                    OpBalanceDrCr = "CR";
                                    OpeningBal = CrOpBal;
                                }
                                ShowLedgerTransaction(dt, LedgerID, OpeningBal, OpBalanceDrCr, IsCrystalReport);
                            }
                        }
#endregion

                    }

                }

                #endregion


                #region for Particular Ledgers
                else if (m_AccountTypes == "PARTICULAR_LEDGER")
                {
                    if (!IsCrystalReport)//just for grid display
                    {
                       //for the orientation purpose..like header defination  
                        Orientation();
                        LedgerID = m_TSS.LedgerID;
                    }
                    //For To write Particular Ledger Name
                    double DrOpBal, CrOpBal;
                    DrOpBal = CrOpBal = 0;

                 
                    #region BLOCK FOR OPENING BALANCE
                    //Show the opening Balance of corresponding Ledger
                   
                    DataTable dtOpLedgerInfo = OpeningBalance.GetAccClassOpeningBalance(GetRootAccClassID(), LedgerID);
                    foreach (DataRow drLedgerInfo in dtOpLedgerInfo.Rows)
                    {

                        //For Date Range Selection    
                        if (m_TSS.FromDate != null && m_TSS.ToDate != null)
                        {
                            if (drLedgerInfo["OpenBalDrCr"].ToString().ToUpper() == "DEBIT")//IF ledger has Debit openig balance
                            {
                                DrOpBal += (Convert.ToDouble(drLedgerInfo["OpenBal"]));
                            }
                            else if (drLedgerInfo["OpenBalDrCr"].ToString().ToUpper() == "CREDIT")//IF ledger has credit Opening balance
                            {
                                CrOpBal += (Convert.ToDouble(drLedgerInfo["OpenBal"]));
                            }

                        #region If date range is selected. Get all transaction details before that FROM date.
                            if (m_TSS.HasDateRange == true)//When datetime is selected
                            {
                                Transaction.GetLedgerBalanceForAccountLedger(null, m_TSS.FromDate, Convert.ToInt32(drLedgerInfo["LedgerID"].ToString()), ref m_dbal, ref m_cbal, m_TSS.AccClassID, m_TSS.ProjectID);
                            }
                            DrOpBal = m_dbal;
                            CrOpBal = m_cbal;
                        }
                            #endregion

                        #region  For DateRange NOt Selection
                        else if (m_TSS.FromDate == null && m_TSS.ToDate == null)
                        {
                            if (drLedgerInfo["OpenBalDrCr"].ToString().ToUpper() == "DEBIT")//IF ledger has Debit openig balance
                            {
                                DrOpBal += (Convert.ToDouble(drLedgerInfo["OpenBal"]));
                            }
                            else if (drLedgerInfo["OpenBalDrCr"].ToString().ToUpper() == "CREDIT")//IF ledger has credit Opening balance
                            {
                                CrOpBal += (Convert.ToDouble(drLedgerInfo["OpenBal"]));
                            }

                        }
                    }

                    #endregion



                    #endregion

                    #region  Display ledger transaction details

                    DataTable dt = Transaction.GetLedgerTransaction(LedgerID, AccClassIDsXMLString, m_TSS.FromDate, m_TSS.ToDate, ProjectIDsXMLString);
                    if (dt.Rows.Count > 0) //If only it has records
                    {
                        DataTable dtLedgerName = Ledger.GetLedgerInfo(LedgerID, LangMgr.DefaultLanguage);
                        foreach (DataRow drLedgerName in dtLedgerName.Rows)
                        {
                            string OpBalanceDrCr = "DR";
                            double OpeningBal = 0;

                            if (DrOpBal > 0 && CrOpBal > 0 && DrOpBal > CrOpBal)
                            {
                                OpBalanceDrCr = "DR";
                                OpeningBal = (DrOpBal - CrOpBal);

                            }
                            else if (DrOpBal > 0 && CrOpBal > 0 && DrOpBal < CrOpBal)
                            {
                                OpBalanceDrCr = "CR";
                                OpeningBal = CrOpBal - DrOpBal;
                            }

                            else if (DrOpBal == CrOpBal)
                            {
                                OpBalanceDrCr = "DR";
                                OpeningBal = 0;
                            }

                            else if (DrOpBal > 0)
                            {
                                OpBalanceDrCr = "DR";
                                OpeningBal = DrOpBal;
                            }
                            else if (CrOpBal > 0)
                            {
                                OpBalanceDrCr = "CR";
                                OpeningBal = CrOpBal;
                            }
                            ShowLedgerTransaction(dt, LedgerID, OpeningBal, OpBalanceDrCr, IsCrystalReport);
                        }
                    }
                }

            }
                    #endregion
                #endregion


            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            ProgressForm.UpdateProgress(100, "Preparing report for display...");

            if (ProgressForm.InvokeRequired)
                ProgressForm.BeginInvoke(new Action(() => ProgressForm.Close()));
        }
                
        /// <summary>
        /// This block shows the transaction of particular Ledger with others except ownself
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="LedgerID"></param>
        /// <param name="IsCrystalReport"></param>

        private void ShowLedgerTransaction(DataTable dt, int? LedgerID, double OpeningBalance, string OpeningBalanceDrCr, bool IsCrystalReport)
        {
            DataTable dtVoucherInfo = new DataTable();
            DataTable dtledInfo = new DataTable();
            double Balance = 0;
            double TotalDrAmt, TotalCrAmt;
            TotalDrAmt = TotalCrAmt = 0;
            double DrBal, CrBal;
            DrBal = CrBal = 0;
            string AccClassIDsXMLString = ReadAllAccClassID();
            string ProjectIDsXMLString = ReadAllProjectID();
            int Count = 1;
            // int LedgerId = m_TSS.LedgerID;
            foreach (DataRow dr in dt.Rows)
            {
                DrBal = Convert.ToDouble(dr["Debit"]);
                CrBal = Convert.ToDouble(dr["Credit"]);
                TotalDrAmt += Convert.ToDouble(dr["Debit"]);
                TotalCrAmt += Convert.ToDouble(dr["Credit"]);

                if (Count == 1) //For to calculate with opening balance with first row either dr or cr
                {
                    if (OpeningBalanceDrCr.ToUpper() == "DR")
                    {
                        DrBal += OpeningBalance;
                    }

                    else if (OpeningBalanceDrCr.ToUpper() == "CR")
                    {
                        CrBal += OpeningBalance;
                    }
                }
                Balance += (DrBal - CrBal);
                Count++;

            }
            #region BLOCK FOR TOTAL AMOUNT CALCULATION

            if (OpeningBalanceDrCr.ToUpper() == "DR")
            {
                TotalDrAmt += OpeningBalance;
            }
            else if (OpeningBalanceDrCr.ToUpper() == "CR")
            {
                TotalCrAmt += OpeningBalance;
            }        
           
            DataTable dtt = Transaction.GetLedgerTransaction(LedgerID, AccClassIDsXMLString, m_TSS.FromDate, m_TSS.ToDate, ProjectIDsXMLString);
            if (dtt.Rows.Count > 0) //If only it has records
            {
                // For to find respective ledger name 
                DataTable dtLedgerNamee = Ledger.GetLedgerInfo(LedgerID, LangMgr.DefaultLanguage);
                foreach (DataRow drLedgerNamee in dtLedgerNamee.Rows)
                {
                    if (!IsCrystalReport)//only for Grid not for crystal report
                    {
                        if (Balance > 0)
                        {
                            WriteTransaction(0, drLedgerNamee["LedName"].ToString(), "", "", (TotalDrAmt).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), (TotalCrAmt).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), "", (Balance).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + "Dr", "GROUP", LedgerID.ToString(), IsCrystalReport);
                        }
                        else if (Balance <= 0)
                        {
                            WriteTransaction(0, drLedgerNamee["LedName"].ToString(), "", "", (TotalDrAmt).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), (TotalCrAmt).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), "", (-Balance).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + "Cr", "GROUP", LedgerID.ToString(), IsCrystalReport);                           
                        }
                    }
                    else if (IsCrystalReport)//For Crystal Report
                    {
                        //Check condition and write with ledgername dr cr and closing balance in dr
                        if (Balance > 0 && TotalDrAmt > TotalCrAmt || TotalDrAmt == TotalCrAmt)
                        {
                            WriteTransaction(0, drLedgerNamee["LedName"].ToString(), "", "", (TotalDrAmt).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), (TotalCrAmt).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), "", (Balance).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + "Dr", "GROUP", LedgerID.ToString(), IsCrystalReport);
                        }
                        //Check condition and write with ledgername dr cr and closing balance in cr
                        else if (Balance <= 0 && TotalDrAmt < TotalCrAmt)
                        {
                            WriteTransaction(0, drLedgerNamee["LedName"].ToString(), "", "", (TotalDrAmt).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), (TotalCrAmt).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), "", (-Balance).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + "Cr", "GROUP", LedgerID.ToString(), IsCrystalReport);
                        }
                    }

            #endregion

                }
            }
            #region For Total Closing Balance dispalying at the last in label TotalClosingBalance.

            decimal sumdebit = Convert.ToDecimal(lblTotalDebitAmount.Text);
            decimal sumcredit = Convert.ToDecimal(lblTotalCreditAmount.Text);
            decimal TotalClosingBalance = sumdebit - sumcredit;
            if (sumdebit > sumcredit || sumdebit == sumcredit)
            {
                lblTotalClosingBalance.Text = TotalClosingBalance.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + " Dr";
            }
            else if (sumcredit > sumdebit)
            {
                lblTotalClosingBalance.Text = (-TotalClosingBalance).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + " Cr";
            }
            #endregion
        }

        private void DisplayBannar()
        {
            CompanyDetails m_CompanyDetails = CompanyInfo.GetInfo();
            if (m_TSS.FromDate != null && m_TSS.ToDate != null)
            {
                lblAllSettings.Text = "From: " + Date.ToSystem(Convert.ToDateTime(m_TSS.FromDate)) + " To: " + Date.ToSystem(Convert.ToDateTime(m_TSS.ToDate));
            }
            if (m_TSS.ToDate != null)
            {
                lblAllSettings.Text = "As on Date: " + Date.ToSystem(Convert.ToDateTime(m_TSS.ToDate));
            }
            if (m_TSS.FromDate != null)
            {
                //This is actually not applicable
                lblAllSettings.Text = "From: " + Date.ToSystem(Convert.ToDateTime(m_TSS.FromDate));
            }
            
            DataTable dtProjectInfo = Project.GetProjectByID(Convert.ToInt32(m_TSS.ProjectID), LangMgr.DefaultLanguage);
            if (m_TSS.ProjectID != 0)
            {
                DataRow drProjectInfo = dtProjectInfo.Rows[0];
                lblProjectName.Text = " Project: " + drProjectInfo["Name"].ToString();
            }
            else
            {
                lblProjectName.Text = " Project: " + "All";
            }
        }

        private void frmSTransaction_Load(object sender, EventArgs e)
        {
             DisplayBannarForSummary();
            //Text format for Total
            GroupView = new SourceGrid.Cells.Views.Cell();
            GroupView.Font = new Font(LangMgr.GetFont(), FontStyle.Bold);

            //Text format for Ledgers
            LedgerView = new SourceGrid.Cells.Views.Cell();
            LedgerView.Font = new Font(LangMgr.GetFont(), FontStyle.Italic);
            LedgerView.ForeColor = Color.Blue;

            //Ledger header
            LedgerHeadView = new SourceGrid.Cells.Views.Cell();
            LedgerHeadView.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Color.LightYellow);
            LedgerHeadView.ForeColor = Color.FromKnownColor(KnownColor.ActiveCaptionText);
            LedgerHeadView.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleCenter;
            LedgerHeadView.Border = DevAge.Drawing.RectangleBorder.NoBorder;
            LedgerHeadView.Font = new Font(LangMgr.GetFont().FontFamily, LangMgr.GetFont().Size + 1, FontStyle.Bold);

            //Finally show the transactions
            DisplayTransaction(false);     
        }

        private void WriteTransaction(int Sno, string Ledger, string TransactDate,string VoucherName, string DrBal, string CrBal, string VoucherType, string Balance,string AccType,string LedgerID,bool IsCrystalReport)
        {
            if (!IsCrystalReport)//for Grid display purpose
            {
                try
                {
                    SourceGrid.Cells.Views.Cell alternate = new SourceGrid.Cells.Views.Cell();
                    if (TransactRowsCount % 2 == 0)
                    {
                        LedgerView.Background = new DevAge .Drawing.VisualElements.BackgroundSolid(Color.White);
                    }
                    else
                    {
                        LedgerView.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Global.Grid_Color);
                    }
                    string strSNo = (Sno == 0 ? "-" : Sno.ToString());
                    if (AccType == "LEDGERHEAD")
                    {
                        //There should be only one column
                        grdTransaction[TransactRowsCount, 0] = new SourceGrid.Cells.Cell(Ledger);
                        grdTransaction[TransactRowsCount, 0].ColumnSpan = grdTransaction.ColumnsCount - 1;

                    }
                    else
                    {
                        if (Ledger == "TOTAL AMOUNT")
                        {
                            TotalDebitAmount += Convert.ToDouble(DrBal);

                            TotalCreditAmount += Convert.ToDouble(CrBal);
                        }
                        grdTransaction[TransactRowsCount, 0] = new SourceGrid.Cells.Cell(strSNo);
                        grdTransaction[TransactRowsCount, 1] = new SourceGrid.Cells.Cell(Ledger);
                        grdTransaction[TransactRowsCount, 2] = new SourceGrid.Cells.Cell(TransactDate);
                        grdTransaction[TransactRowsCount, 3] = new SourceGrid.Cells.Cell(VoucherName);
                        grdTransaction[TransactRowsCount, 4] = new SourceGrid.Cells.Cell(DrBal);
                        grdTransaction[TransactRowsCount, 5] = new SourceGrid.Cells.Cell(CrBal);
                        grdTransaction[TransactRowsCount, 6] = new SourceGrid.Cells.Cell(VoucherType);
                        grdTransaction[TransactRowsCount, 7] = new SourceGrid.Cells.Cell(Balance);
                        grdTransaction[TransactRowsCount, 8] = new SourceGrid.Cells.Cell(LedgerID);//Instead of RowId LedgerID is used to display the details of ledger directly by clicking the respective ledger.
                    }
                    //To store the current view types accourding to the row type(Ledger, Group etc)
                    SourceGrid.Cells.Views.Cell CurrentView = new SourceGrid.Cells.Views.Cell();

                    switch (AccType)
                    {
                        case "GROUP":
                            CurrentView = GroupView;
                            break;
                        case "LEDGER":
                            CurrentView = LedgerView;
                            break;
                        case "LEDGERHEAD":
                            CurrentView = LedgerHeadView;

                            break;
                        default:
                            CurrentView = GroupView; //Because it is the normal formatting without any makeups
                            break;
                    }


                    if (AccType != "LEDGERHEAD")//This is not ledgerhead
                    {
                        //To display total dr and total cr in the end 
                        TotalDebitAmount += Convert.ToDouble(DrBal);
                        TotalCreditAmount += Convert.ToDouble(CrBal);

                        grdTransaction[TransactRowsCount, 0].AddController(dblClick);
                        grdTransaction[TransactRowsCount, 0].View = new SourceGrid.Cells.Views.Cell(CurrentView);
                        grdTransaction[TransactRowsCount, 0].View = new SourceGrid.Cells.Views.Cell(alternate);

                        grdTransaction[TransactRowsCount, 1].AddController(dblClick);
                        grdTransaction[TransactRowsCount, 1].View = new SourceGrid.Cells.Views.Cell(CurrentView);
                        //grdTransaction[TransactRowsCount, 1].View = new SourceGrid.Cells.Views.Cell(alternate);

                        grdTransaction[TransactRowsCount, 2].AddController(dblClick);
                        grdTransaction[TransactRowsCount, 2].View = new SourceGrid.Cells.Views.Cell(CurrentView);
                        // grdTransaction[TransactRowsCount, 2].View = new SourceGrid.Cells.Views.Cell(alternate);

                        grdTransaction[TransactRowsCount, 3].AddController(dblClick);
                        grdTransaction[TransactRowsCount, 3].View = new SourceGrid.Cells.Views.Cell(CurrentView);
                        // grdTransaction[TransactRowsCount, 3].View = new SourceGrid.Cells.Views.Cell(alternate);

                        grdTransaction[TransactRowsCount, 4].AddController(dblClick);
                        grdTransaction[TransactRowsCount, 4].View = new SourceGrid.Cells.Views.Cell(CurrentView);
                        // grdTransaction[TransactRowsCount, 4].View = new SourceGrid.Cells.Views.Cell(alternate);

                        grdTransaction[TransactRowsCount, 5].AddController(dblClick);
                        grdTransaction[TransactRowsCount, 5].View = new SourceGrid.Cells.Views.Cell(CurrentView);
                        // grdTransaction[TransactRowsCount, 5].View = new SourceGrid.Cells.Views.Cell(alternate);

                        grdTransaction[TransactRowsCount, 6].AddController(dblClick);
                        grdTransaction[TransactRowsCount, 6].View = new SourceGrid.Cells.Views.Cell(CurrentView);
                        // grdTransaction[TransactRowsCount, 6].View = new SourceGrid.Cells.Views.Cell(alternate);

                        grdTransaction[TransactRowsCount, 7].AddController(dblClick);
                        grdTransaction[TransactRowsCount, 7].View = new SourceGrid.Cells.Views.Cell(CurrentView);
                        // grdTransaction[TransactRowsCount, 7].View = new SourceGrid.Cells.Views.Cell(alternate);

                        grdTransaction[TransactRowsCount, 8].AddController(dblClick);
                        grdTransaction[TransactRowsCount, 8].View = new SourceGrid.Cells.Views.Cell(CurrentView);
                        // grdTransaction[TransactRowsCount, 8].View = new SourceGrid.Cells.Views.Cell(alternate);

                    }
                    else
                    {
                        //If this is LEDGERHEAD, apply the currentview
                        grdTransaction[TransactRowsCount, 0].View = new SourceGrid.Cells.Views.Cell(CurrentView);
                    }
                    grdTransaction.Rows.Insert(grdTransaction.RowsCount);
                    TransactRowsCount++;
                    lblTotalDebitAmount.Text = TotalDebitAmount.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                    lblTotalCreditAmount.Text = TotalCreditAmount.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                }
                catch (Exception ex)
                {

                    Global.Msg(ex.Message);
                }

            }
            else if (IsCrystalReport)//crystal report purpose
            {
                dsTransaction.Tables["tblTransaction"].Rows.Add(Ledger, TransactDate, VoucherName, DrBal, CrBal, VoucherType, Balance);
            }
       
        } 
       
        /// This is method for Crystal Report.To generate report in printable form.
        private void WriteTransactionOnCrystalRpt(string DrBal,string CrBal,string Balance)
        { 
            dsTransaction.Tables["tblTransaction"].Rows.Add(DrBal, CrBal,  Balance);
        }

        /// This is the method for Double click event.Call the Ledger form according to  LedgerID]
        /// no need to go Voucher form for Opening Balance/Total Amount/

        private void Tranasaction_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                //Get the Selected Row           
                int CurRow = grdTransaction.Selection.GetSelectionRegion().GetRowsIndex()[0];
                SourceGrid.CellContext cellType = new SourceGrid.CellContext(grdTransaction, new SourceGrid.Position(CurRow, 1));//Represents Ledger
                SourceGrid.CellContext cellTypeID = new SourceGrid.CellContext(grdTransaction, new SourceGrid.Position(CurRow, 8));//Represents LedgerID 

                //read id  
                string LedgerID = (cellTypeID.Value).ToString();//Here comes LedgerID

                int CurRow2 = grdTransaction.Selection.GetSelectionRegion().GetRowsIndex()[0];
                SourceGrid.CellContext cellID = new SourceGrid.CellContext(grdTransaction, new SourceGrid.Position(CurRow2, 8));
                TransactSettings m_TS = new TransactSettings();
                m_TS.HasDateRange = m_TSS.HasDateRange;
                m_TS.ShowZeroBalance = m_TSS.ShowZeroBalance;
                m_TS.FromDate = m_TSS.FromDate;
                m_TS.ToDate = m_TSS.ToDate;
                m_TS.AccClassID = m_TSS.AccClassID;
                m_TS.LedgerID = Convert.ToInt32(cellTypeID.Value);//Store the LedgerID value on object which achieve while double clicking the corresponding row of cell
                frmTransaction m_Transact = new frmTransaction(m_TS);
                m_Transact.ShowDialog();
                 
            }
            catch (Exception ex)
            {              
                Global.Msg(ex.Message);
            }
        }

        private void MakeHeader()
        {
            //To make Grid Header 
            //rows = grdTransaction.RowsCount;
            grdTransaction.Rows.Insert(0);
            grdTransaction[0, 0] = new MyHeader("S.No.");
            grdTransaction[0, 1] = new MyHeader("Ledger Name");
            grdTransaction[0, 2] = new MyHeader("Date");
            grdTransaction[0, 3] = new MyHeader("Vch #");
            grdTransaction[0, 4] = new MyHeader("Debit Amount");
            grdTransaction[0, 5] = new MyHeader("Credit Amount");
            grdTransaction[0, 6] = new MyHeader("Type");
            grdTransaction[0, 7] = new MyHeader("Closing Balance");
            grdTransaction[0, 8] = new MyHeader("RowID");

            grdTransaction[0, 0].Column.Width = 50;
            grdTransaction.Columns[0].Visible = false;
            grdTransaction[0, 1].Column.Width = 400;
            grdTransaction[0, 2].Column.Width = 170;
            grdTransaction.Columns[2].Visible = false;
            grdTransaction[0, 3].Column.Width = 80;
            grdTransaction.Columns[3].Visible = false;
            grdTransaction[0, 4].Column.Width = 200;
            grdTransaction[0, 5].Column.Width = 200;
            grdTransaction[0, 6].Column.Width = 80;
            grdTransaction.Columns[6].Visible = false;
            grdTransaction[0, 8].Column.Width = 70;
            grdTransaction[0, 7].Column.Width = 200;
            grdTransaction.Columns[8].Visible = false;
        }

        private void Orientation()
        {
            dblClick = new SourceGrid.Cells.Controllers.CustomEvents();
            dblClick.DoubleClick += new EventHandler(Tranasaction_DoubleClick);

            Transaction m_Transaction = new Transaction();
            //Text format for Total
            SourceGrid.Cells.Views.Cell categoryView = new SourceGrid.Cells.Views.Cell();
            categoryView.Background = new DevAge.Drawing.VisualElements.BackgroundLinearGradient(Color.RoyalBlue, Color.LightBlue, 0);
            categoryView.ForeColor = Color.FromKnownColor(KnownColor.ActiveCaptionText);
            categoryView.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleCenter;
            categoryView.Border = DevAge.Drawing.RectangleBorder.NoBorder;
            categoryView.Font = new Font(Font, FontStyle.Bold);

            //Text format for Ledgers
            SourceGrid.Cells.Views.Cell LedgerView1 = new SourceGrid.Cells.Views.Cell();
            LedgerView1.ForeColor = Color.BlueViolet;
            LedgerView1.Font = new Font(Font, FontStyle.Italic);

            ////Let the whole row to be selected
            grdTransaction.SelectionMode = SourceGrid.GridSelectionMode.Row;

            //SourceGrid.Cells.Views.Cell LedgerView1 = new SourceGrid.Cells.Views.Cell();
            SourceGrid.Cells.Views.Cell LedgerView = new SourceGrid.Cells.Views.Cell();
            LedgerView.ForeColor = Color.BlueViolet;
            LedgerView.Font = new Font(Font, FontStyle.Italic);

            ////Disable multiple selection
            grdTransaction.Selection.EnableMultiSelection = false;
            grdTransaction.Redim(1, 9);
            grdTransaction.FixedRows = 1;
            MakeHeader();
        
        }

        private string GetVoucherName(DataTable dtVoucherInfo)
        {
            if (dtVoucherInfo.Rows.Count > 0)
            {
                DataRow drVoucherName = dtVoucherInfo.Rows[0];
                return drVoucherName["Voucher_No"].ToString();
            }
            else
            {
                return  "";
            }
        }

        private string GetLedgerName(DataTable dtLedgerInfo)
        {
            if (dtLedgerInfo.Rows.Count > 0)
            {
                DataRow drLedName = dtLedgerInfo.Rows[0];
                return drLedName["LedName"].ToString();
            }
            else
            {
                return "";
            }
        }


        private void PrintPreviewCR(PrintType myPrintType)
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

            dsTransaction.Clear();//It clear the previous records of dataset on crystal report...when this button is pressed

            //otherwise it populate the records again and again
            double DebitSum, CreditSum;
            DebitSum = CreditSum = 0;
            rptTransaction1 rpt = new rptTransaction1();
            //Fill the logo on the report
            Misc.WriteLogo(dsTransaction, "tblImage");
            //Set DataSource to be dsTrial dataset on the report
            rpt.SetDataSource(dsTransaction);
            DisplayTransaction(true);

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
            CrystalDecisions.Shared.ParameterDiscreteValue pdvDebit_Total = new CrystalDecisions.Shared.ParameterDiscreteValue();
            CrystalDecisions.Shared.ParameterDiscreteValue pdvCredit_Total = new CrystalDecisions.Shared.ParameterDiscreteValue();
            CrystalDecisions.Shared.ParameterDiscreteValue pdvTotal_Closing_Balance = new CrystalDecisions.Shared.ParameterDiscreteValue();
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

            pdvReport_Head.Value = "Transaction Summary";
            pvCollection.Clear();
            pvCollection.Add(pdvReport_Head);
            rpt.DataDefinition.ParameterFields["Report_Head"].ApplyCurrentValues(pvCollection);

            pdvFiscal_Year.Value = "Fiscal Year:" + m_CompanyDetails.FYFrom;
            pvCollection.Clear();
            pvCollection.Add(pdvFiscal_Year);
            rpt.DataDefinition.ParameterFields["Fiscal_Year"].ApplyCurrentValues(pvCollection);

            pdvDebit_Total.Value = lblTotalDebitAmount.Text;
            pvCollection.Clear();
            pvCollection.Add(pdvDebit_Total);
            rpt.DataDefinition.ParameterFields["Total_DebitAmount"].ApplyCurrentValues(pvCollection);

            pdvCredit_Total.Value = lblTotalCreditAmount.Text;
            pvCollection.Clear();
            pvCollection.Add(pdvCredit_Total);
            rpt.DataDefinition.ParameterFields["Total_CreditAmount"].ApplyCurrentValues(pvCollection);

            //For closing 
            pdvTotal_Closing_Balance.Value = lblTotalClosingBalance.Text;
            pvCollection.Clear();
            pvCollection.Add(pdvTotal_Closing_Balance);
            rpt.DataDefinition.ParameterFields["Total_Closing_Balance"].ApplyCurrentValues(pvCollection);


            //Update the progressbar
            ProgressForm.UpdateProgress(80, "Calculating Parameters...");

            //Display the date in crystal report according to available from and to dates
            if (m_TSS.FromDate != null && m_TSS.ToDate != null)
            {
                pdvReport_Date.Value = "From: " + Date.ToSystem(Convert.ToDateTime(m_TSS.FromDate)) + " To: " + Date.ToSystem(Convert.ToDateTime(m_TSS.ToDate));
            }
            if (m_TSS.ToDate != null)
            {
                pdvReport_Date.Value = "As on Date: " + Date.ToSystem(Convert.ToDateTime(m_TSS.ToDate));
            }
            if (m_TSS.FromDate != null)
            {
                //This is actually not applicable
                pdvReport_Date.Value = "From: " + Date.ToSystem(Convert.ToDateTime(m_TSS.FromDate));
            }
            else //Both FromDate and Todate not provided
            {
                pdvReport_Date.Value = "All";
            }

            pvCollection.Clear();
            pvCollection.Add(pdvReport_Date);
            rpt.DataDefinition.ParameterFields["Report_Date"].ApplyCurrentValues(pvCollection);



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


        private void btnPrintPreview_Click(object sender, EventArgs e)
        {
            PrintPreviewCR(PrintType.CrystalReport);
        }

        //private void InitializeComponent()
        //{
        //    this.SuspendLayout();
        //    // 
        //    // frmTransaction
        //    // 
        //    this.ClientSize = new System.Drawing.Size(284, 261);
        //    this.Name = "frmTransaction";
        //    this.Load += new System.EventHandler(this.frmTransaction_Load_1);
        //    this.ResumeLayout(false);

        //}

        private void frmTransaction_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
            
        }

        private void btnExport_Click_1(object sender, EventArgs e)
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

        private string ReadAllProjectID()
        {
            #region  ProjectID
            ArrayList arrchildProjectIds = new ArrayList();
            Project.GetChildProjects(Convert.ToInt32(m_TSS.ProjectID), ref arrchildProjectIds);
            ArrayList ProjectIDCollection = new ArrayList();
            foreach (object obj in arrchildProjectIds)
            {
                int p = (int)obj;
                ProjectIDCollection.Add(p.ToString());
            }

            string ProjectIDS = "'" + m_TSS.ProjectID + "'";

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
                    tw.WriteElementString("PID", Convert.ToInt32(m_TSS.ProjectID).ToString());
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

        private void btnRefresh_Click_1(object sender, EventArgs e)
        {

            //Clear all rows
            grdTransaction.Redim(0, 0);

            //Reset the total variables
            TotalCreditAmount = TotalDebitAmount = 0;


            this.Cursor = Cursors.WaitCursor;
            //Load all over again
            frmSTransaction_Load(sender, e);

            this.Cursor = Cursors.Default;
            //Show complete notification
            //Global.Msg("Reload Complete!");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            PrintPreviewCR(PrintType.CrystalReport);
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

    }
       
    }

