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
using Common;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using DateManager;
using System.Threading;
using Accounts.Reports;

namespace Accounts
{
    public partial class frmGroupBalance : Form
    {       
        private SourceGrid.Cells.Controllers.CustomEvents dblClick;
        private GroupBalanceSettings m_GBS;
        private int GroupBalanceRowsCount;
        private Accounts.Model.dsTrialBalance dsTrial = new Model.dsTrialBalance();
        private int prntDirect = 0;
        private string FileName = "";


        //Total variables
        double TotalDebitSum = 0;
        double TotalCreditSum = 0;

        double GrandDebitSum = 0;
        double GrandCreditSum=0;

        //Different grid views
        private SourceGrid.Cells.Views.Cell GroupView;
        private SourceGrid.Cells.Views.Cell LedgerView;
        private SourceGrid.Cells.Views.Cell SubGroupView;
        //For Export Menu
        ContextMenu Menu_Export;
        private IMDIMainForm m_MDIForm;

        public frmGroupBalance(GroupBalanceSettings GBS)
        {
            try
            {
                InitializeComponent();
                m_GBS = new GroupBalanceSettings();

                #region BLOCK FOR INITIALIZING THE CONSTRUCTOR PARAMETER
                m_GBS.FromDate = GBS.FromDate;
                m_GBS.ToDate = GBS.ToDate;
                m_GBS.GroupID = GBS.GroupID;
                m_GBS.AccClassID = GBS.AccClassID;
                m_GBS.HasDateRange = GBS.HasDateRange;
                m_GBS.ShowZeroBalance = GBS.ShowZeroBalance;
                m_GBS.ShowSecondLevelGroupDtl = GBS.ShowSecondLevelGroupDtl;
                m_GBS.Details = GBS.Details;
                m_GBS.AllGroups = GBS.AllGroups;
                m_GBS.OnlyPrimaryGroups = GBS.OnlyPrimaryGroups;
                m_GBS.ProjectID = GBS.ProjectID;                            

                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public frmGroupBalance(GroupBalanceSettings GBS, IMDIMainForm frm)
        {
            m_MDIForm = frm;
            try
            {
                InitializeComponent();
                m_GBS = new GroupBalanceSettings();

                #region BLOCK FOR INITIALIZING THE CONSTRUCTOR PARAMETER
                m_GBS.FromDate = GBS.FromDate;
                m_GBS.ToDate = GBS.ToDate;
                m_GBS.GroupID = GBS.GroupID;
                m_GBS.AccClassID = GBS.AccClassID;
                m_GBS.HasDateRange = GBS.HasDateRange;
                m_GBS.ShowZeroBalance = GBS.ShowZeroBalance;
                m_GBS.ShowSecondLevelGroupDtl = GBS.ShowSecondLevelGroupDtl;
                m_GBS.Details = GBS.Details;
                m_GBS.AllGroups = GBS.AllGroups;
                m_GBS.OnlyPrimaryGroups = GBS.OnlyPrimaryGroups;
                m_GBS.ProjectID = GBS.ProjectID;

                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void MakeHeader()
        {
            //Write header part
            grdGroupBalance.Rows.Insert(0);
            grdGroupBalance[0, 0] = new MyHeader("S.No.");
            grdGroupBalance[0, 1] = new MyHeader("Account Name");
            grdGroupBalance[0, 2] = new MyHeader(" Debit Amount");
            grdGroupBalance[0, 3] = new MyHeader("Credit Amount");
            grdGroupBalance[0, 4] = new MyHeader("ID");
            grdGroupBalance[0, 5] = new MyHeader("Type");

            //Define the width of column size
            grdGroupBalance[0, 0].Column.Width = 50;
            grdGroupBalance[0, 1].Column.Width = 300;
            grdGroupBalance[0, 2].Column.Width = 250;
            grdGroupBalance[0, 3].Column.Width = 150;
            grdGroupBalance[0, 4].Column.Width = 150;
            grdGroupBalance[0, 5].Column.Width = 100;

            //Code for making column invisible
            grdGroupBalance.Columns[4].Visible = false;// making third column invisible and using it in  programming     
            grdGroupBalance.Columns[5].Visible = false;
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
       
        private void WriteGroupBalance(int SNo, string Name, int GroupID, double DrBal, double CrBal, string Type)
        {
            grdGroupBalance.Rows.Insert(grdGroupBalance.RowsCount);
            // Block for getting GroupName          
            string strSNo = (SNo == 0 ? "" : SNo.ToString());           
            grdGroupBalance[GroupBalanceRowsCount, 0] = new SourceGrid.Cells.Cell(strSNo);
            grdGroupBalance[GroupBalanceRowsCount, 0].AddController(dblClick);

            grdGroupBalance[GroupBalanceRowsCount, 1] = new SourceGrid.Cells.Cell(Name);

            grdGroupBalance[GroupBalanceRowsCount, 4] = new SourceGrid.Cells.Cell(GroupID.ToString());//Adding GroupID of each row in fourth column as invisible for further use

            grdGroupBalance[GroupBalanceRowsCount, 5] = new SourceGrid.Cells.Cell(Type);

            grdGroupBalance[GroupBalanceRowsCount, 2] = new SourceGrid.Cells.Cell(DrBal.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));


            grdGroupBalance[GroupBalanceRowsCount, 3] = new SourceGrid.Cells.Cell(CrBal.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));

            //To store the current view types accourding to the row type(Ledger, Group etc)
            SourceGrid.Cells.Views.Cell CurrentView = new SourceGrid.Cells.Views.Cell();

            switch (Type)
            {
                case "GROUP":
                    CurrentView = GroupView;
                    break;
                case "TOTAL":
                    CurrentView = GroupView;
                    break;
                case "LEDGER":
                    CurrentView = LedgerView;
                    grdGroupBalance[GroupBalanceRowsCount, 1].Value = "    " + grdGroupBalance[GroupBalanceRowsCount, 1].Value; //Give a little space for ledger so that it appears it is inside its parent group
                    break;
                case "SUBGROUP":
                    CurrentView = SubGroupView;
                    grdGroupBalance[GroupBalanceRowsCount, 1].Value = "  " + grdGroupBalance[GroupBalanceRowsCount, 1].Value; //Give a little space for subgroup so that it appears it is inside its parent group
                    break;
                default:
                    CurrentView = SubGroupView; //Because it is the normal formatting without any makeups
                    break;
            }


            grdGroupBalance[GroupBalanceRowsCount, 1].AddController(dblClick);
            grdGroupBalance[GroupBalanceRowsCount, 1].View = CurrentView;

            grdGroupBalance[GroupBalanceRowsCount, 2].AddController(dblClick);
            grdGroupBalance[GroupBalanceRowsCount, 2].View = new SourceGrid.Cells.Views.Cell(CurrentView);
            grdGroupBalance[GroupBalanceRowsCount, 2].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;

            grdGroupBalance[GroupBalanceRowsCount, 3].AddController(dblClick);
            grdGroupBalance[GroupBalanceRowsCount, 3].View = new SourceGrid.Cells.Views.Cell(CurrentView);
            grdGroupBalance[GroupBalanceRowsCount, 3].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;
            //Increment assets rows count
            GroupBalanceRowsCount++;

            //Sum the drBal to TotalDebit and crBal to TotalCredit
            try
            {
                //Calculate totals if it is Group and Ledger only
                if (Type == "GROUP" || Type == "LEDGER")
                {
                    TotalDebitSum += DrBal;
                    TotalCreditSum += CrBal;
                }
            }

            catch (Exception ex)
            {
                Global.MsgError("Unable to convert debit and credit balance");
            }


           

        }
     
        private void WriteDetails(int GroupID, bool CrystalReport)
        {
            try
            {
                DataTable dtDtlLedgerID = AccountGroup.GetDetailLedgerID(GroupID, true);
                int Sno = 1;
                foreach (DataRow drDtlLedgerID in dtDtlLedgerID.Rows)
                {
                    double DebBal = 0;
                    double CreBal = 0;
                    Transaction.GetLedgerBalance(m_GBS.FromDate, m_GBS.ToDate, Convert.ToInt32(drDtlLedgerID["LedgerID"]), ref  DebBal, ref CreBal, m_GBS.AccClassID,m_GBS.ProjectID);

                    //By default date range is selected soo we dont need method which doesnot need date range parameter
                    //Soo use just thoes method which need date range parameter soo just commenting below's previous code 
                    #region BLOCK FOR CALLING METHOD ACCORDING TO DATE RANGE
                    //if (m_TBS.HasDateRange == true)//If DateRange is checked
                    //{
                    //    Transaction.GetLedgerBalance(m_TBS.FromDate, m_TBS.ToDate, Convert.ToInt32(drDtlLedgerID["LedgerID"]), ref  DebBal, ref CreBal, m_TBS.AccClassID);
                    //}
                    //else //Otherwise
                    //{
                    //    Transaction.GetLedgerBalance(Convert.ToInt32(drDtlLedgerID["LedgerID"]), ref  DebBal, ref CreBal, m_TBS.AccClassID);
                    //}
                    #endregion

                    if (m_GBS.ShowZeroBalance == false && DebBal == 0 && CreBal == 0)
                        //return;
                        continue;//It cotinue the loop except below code
                    //If cystal we are going to write records on crystal report then call the corresponding method



                    
                    if (CrystalReport == true)
                    {
                        WriteGroupBalanceOnCrystalRpt(0, "-" + drDtlLedgerID["EngName"].ToString(), Convert.ToInt32(drDtlLedgerID["LedgerID"]), DebBal.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), CreBal.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), "LEDGER");
                    }
                    else
                    {
                        WriteGroupBalance(Sno, "-" + drDtlLedgerID["EngName"].ToString(), Convert.ToInt32(drDtlLedgerID["LedgerID"]), DebBal, CreBal, "LEDGER");
                        
                    }
                    Sno++;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void WriteLedger(int GroupID, ref double TotalDrBal, ref double TotalCrBal, bool CrystalReport)
        {          
            Transaction Transaction = new Transaction();         
            DataTable dtledg = Ledger.GetLedgerTable(GroupID);
            int Sno = 1;
            foreach (DataRow drledger in dtledg.Rows)
            {
                DataTable dtLedgerInfo = Ledger.GetLedgerInfo(Convert.ToInt32(drledger["LedgerID"]), LangMgr.DefaultLanguage);
                DataRow drLedgerInfo = dtLedgerInfo.Rows[0];
                double DrOpBal = 0;
                double CrOpBal = 0;
             
                //write also opening balance of corresponding LedgerID
                Transaction.GetOpeningBalance(Convert.ToInt32(drledger["LedgerID"]), m_GBS.AccClassID, ref DrOpBal, ref CrOpBal);
    
                //getting transaction of corresponding Ledger according to AccountClass and DateRange
                double DrBal, CrBal;
                DrBal = CrBal=0;
          
                if(m_GBS.HasDateRange)
                {
                    Transaction.GetLedgerBalance(m_GBS.FromDate, m_GBS.ToDate, Convert.ToInt32(drledger["LedgerID"]), ref DrBal, ref CrBal, m_GBS.AccClassID, m_GBS.ProjectID);
                }
                else
                {
                    Transaction.GetLedgerBalance(null,null, Convert.ToInt32(drledger["LedgerID"]), ref DrBal, ref CrBal, m_GBS.AccClassID, m_GBS.ProjectID);  
                }
                //
                if (m_GBS.ShowZeroBalance == false && DrBal == 0 && CrBal == 0)
                {
                    //write only opening balance
                    if (DrOpBal > 0 || CrOpBal > 0)//IF ledger has no transaction and it has only opening Balance then
                    {
                       //TotalDrBal += DrOpBal;
                       // TotalCrBal += CrOpBal;
                        WriteGroupBalance(Sno, drLedgerInfo["LedName"].ToString(), Convert.ToInt32(drledger["LedgerID"]), DrOpBal, CrOpBal, "LEDGER");

                    }
                }
                else
                {
                   // TotalDrBal += DrBal;
                    //TotalCrBal += CrBal;
                    WriteGroupBalance(Sno, drLedgerInfo["LedName"].ToString(), Convert.ToInt32(drledger["LedgerID"]), DrBal,CrBal, "LEDGER");
                }

                Sno++;


            }
        }


        private void WriteGroupBalanceOnCrystalRpt(int SNo, string Name, int GroupID, string DrBal, string CrBal, string Type)
        {
            dsTrial.Tables["tblGroup"].Rows.Add(SNo, Name, DrBal, CrBal, Type);
        }

        private void DisplayBannar()
        {
            CompanyDetails m_CompanyDetails = CompanyInfo.GetInfo();
            lblCompanyName.Text = m_CompanyDetails.CompanyName;
            lblCompanyAddress.Text = m_CompanyDetails.Address1 + " " + m_CompanyDetails.City + " " + m_CompanyDetails.District;
            lblContact.Text = "Contact: " + m_CompanyDetails.Telephone ;
            lblwebsite.Text = "Website: " + m_CompanyDetails.Website;
            lblPanNo.Text = "PAN No.: " + m_CompanyDetails.PAN;

            DataTable dtProjectInfo = Project.GetProjectByID(Convert.ToInt32(m_GBS.ProjectID), LangMgr.DefaultLanguage);

            if (m_GBS.FromDate != null && m_GBS.ToDate != null)
            {
                lblAllSettings.Text = "From: " + Date.ToSystem(Convert.ToDateTime(m_GBS.FromDate)) + " To: " + Date.ToSystem(Convert.ToDateTime(m_GBS.ToDate));
            }
            if (m_GBS.ToDate != null)
            {
                lblAllSettings.Text = "As on Date: " + Date.ToSystem(Convert.ToDateTime(m_GBS.ToDate));
            }
            if (m_GBS.FromDate != null)
            {
                //This is actually not applicable
                lblAllSettings.Text = "From: " + Date.ToSystem(Convert.ToDateTime(m_GBS.FromDate));
            }


            if (m_GBS.ProjectID != 0)
            {
                DataRow drProjectInfo = dtProjectInfo.Rows[0];
                

                lblProjectName.Text = " Project: " + drProjectInfo["Name"].ToString();
            }
            else
            {
                lblProjectName.Text = " Project: " + "All";
            }
        }

        private void frmGroupBalance_Load(object sender, EventArgs e)
        {
            DisplayBannar();

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
    
            #region BLOCK FOR ORIENTATION PURPOSE
            GroupBalanceRowsCount = 1;
            
            AccountGroup AccountGroup = new AccountGroup();// Intializing the object of AccountGroup [Dynamic memory allocation of an object]
            Transaction Transaction = new Transaction();
            
            
            //Text format for Total
            SourceGrid.Cells.Views.Cell categoryView = new SourceGrid.Cells.Views.Cell();
            categoryView.Background = new DevAge.Drawing.VisualElements.BackgroundLinearGradient(Color.RoyalBlue, Color.LightBlue, 0);
            categoryView.ForeColor = Color.FromKnownColor(KnownColor.ActiveCaptionText);
            categoryView.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleCenter;
            categoryView.Border = DevAge.Drawing.RectangleBorder.NoBorder;
            categoryView.Font = new Font(Font, FontStyle.Bold);

            //Text format for Total
            GroupView = new SourceGrid.Cells.Views.Cell();
            GroupView.Font = new Font(LangMgr.GetFont(), FontStyle.Bold);

            //Text format for Ledgers
            LedgerView = new SourceGrid.Cells.Views.Cell();
            LedgerView.Font = new Font(LangMgr.GetFont(), FontStyle.Italic);

            //Text format for SubGroup
            SubGroupView = new SourceGrid.Cells.Views.Cell();
            SubGroupView.Font = new Font(LangMgr.GetFont(), FontStyle.Regular);

            //Add a double click handler. When user dblclicks the cell, will use this function to send the parent form the Currency code
            dblClick = new SourceGrid.Cells.Controllers.CustomEvents();
            dblClick.DoubleClick += new EventHandler(grdTrial_DoubleClick);
            
            //Let the whole row to be selected
            grdGroupBalance.SelectionMode = SourceGrid.GridSelectionMode.Row;


            //Disable multiple selection
            grdGroupBalance.Selection.EnableMultiSelection = false;

            grdGroupBalance.Redim(1, 6);
            grdGroupBalance.FixedRows = 1;
            //int rows = grdTrial.Rows.Count;
            MakeHeader();
            #endregion

            bool HasSubGroup = false;//If Group has subgroup 
            DataTable dt = AccountGroup.GetGroupTable(m_GBS.GroupID);// i have commented it
            int Sno = 1;

            foreach (DataRow dr in dt.Rows)//If following group has sub groups
            {
                HasSubGroup = true;
                double m_dbal = 0;
                double m_cbal = 0;

                // Block for DateTime range selection,here bydefault data range is selected,soo just call thoes method which have daterange parameter
                //with backup we have also method which doesnot concern date range
                //But in this case we have date range by default soo i used this one.....
                Transaction.GetGroupBalance(m_GBS.FromDate, m_GBS.ToDate, Convert.ToInt32(dr["GroupID"]), true, ref m_dbal, ref m_cbal, m_GBS.AccClassID, m_GBS.ProjectID);
               
                
                #region BLOCK FOR CALLING METHOD ACCORDING TO DATE RANGE SELECTION
                //if (m_TBS.HasDateRange == true)//When datetime is selected
                  
                //    Transaction.GetGroupBalance(m_TBS.FromDate, m_TBS.ToDate, Convert.ToInt32(dr["GroupID"]), true, ref m_dbal, ref m_cbal, m_TBS.AccClassID);
                //else//Otherwise
                //    Transaction.GetGroupBalance(Convert.ToInt32(dr["GroupID"]), true, ref m_dbal, ref m_cbal, m_TBS.AccClassID);
                #endregion// It does not need now //It does not need now
               
                if (m_GBS.ShowZeroBalance == false && m_dbal == 0 && m_cbal == 0) //In case of zero balance
                {
                    //Do nothing
                }
                else
                {
                    string EngName = AccountGroup.GetEngName(Convert.ToInt32(dr["GroupID"]).ToString());  //calling this funtion for getting EngName from tblGroup corresponding to GroupID having corresponding GroupID
                    double Balance = (m_dbal - m_cbal);//For Asset[Debit Balance - Credit Balance]
                    WriteGroupBalance(Sno, EngName, Convert.ToInt32(dr["GroupID"]), m_dbal, m_cbal, "GROUP");
                    GrandDebitSum += m_dbal;
                    GrandCreditSum += m_cbal;

                    //If details is selected, show details i.e. ledgers present inside
                    if (m_GBS.Details == true)
                        //WriteDetails(Convert.ToInt32(dr["GroupID"]), AccountType.Assets);
                        WriteDetails(Convert.ToInt32(dr["GroupID"]), false);


                }//End of zero balance check
                Sno++;
            }//End for loop

            ProgressForm.UpdateProgress(40, "Calculating group balance...");

            double debitsum,creditsum;
            debitsum=creditsum=0;
            WriteLedger(m_GBS.GroupID, ref debitsum, ref creditsum, false);
            GrandDebitSum += debitsum;
            GrandCreditSum += creditsum;           

            #region BLOCK FOR CALCULATING DIFFERENCE IN OPENING BALANCE
            double TotalDrOpBal, TotalCrOpBal;
            TotalDrOpBal = TotalCrOpBal = 0;
            Transaction.GetOpeningBalanceFromGroup(m_GBS.GroupID, m_GBS.AccClassID, ref TotalDrOpBal, ref TotalCrOpBal);
           // GetOpeningBalanceSummary(ref TotalDrOpBal, ref TotalCrOpBal);

            if ((TotalDrOpBal != TotalCrOpBal))
            {
                if (TotalDrOpBal > TotalCrOpBal)//Total Opening balance in Debit side is greater so balance by adding this amount in Credit Balance
                {
                    GrandCreditSum += (TotalDrOpBal - TotalCrOpBal);
                    WriteGroupBalance(0, "DIFFERENCE IN OPENING BALANCE:", 0, (0), (TotalDrOpBal - TotalCrOpBal), "TOTAL");
                }
                else if (TotalDrOpBal < TotalCrOpBal)//Total Opening balance in Debit side is greater so balance by adding this amount in Credit Balance
                {
                    GrandDebitSum += (TotalCrOpBal - TotalDrOpBal);
                    WriteGroupBalance(0, "DIFFERENCE IN OPENING BALANCE:", 0, (TotalCrOpBal - TotalDrOpBal), (0), "TOTAL");
                }
            }
            #endregion
            
            ProgressForm.UpdateProgress(80, "Calculating diffence in opening balance for display...");









#region BLOCK FOR TOTAL AMOUNT CALCULATION FOR GROUP BALANCE
            /*
            if(HasSubGroup)
            {
                WriteGroupBalance(0, "GROUP BALANCE TOTAL", 0, TotalDebitSum, TotalCreditSum, "GROUP");
                if ((TotalDebitSum - TotalCreditSum) > 0)
                {
                    lblClosingBalance.Text = (TotalDebitSum - TotalCreditSum).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + "Dr";
                }
                else if((GrandCreditSum-GrandDebitSum)>0)
                {
                    lblClosingBalance.Text = (TotalCreditSum - TotalDebitSum).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + "Cr";
                }
            }
            else if(!HasSubGroup)
            {
                WriteGroupBalance(0, "GROUP BALANCE TOTAL", 0, TotalDebitSum, (TotalCreditSum), "GROUP");
                if ((TotalDebitSum - TotalCreditSum) > 0)
                {
                    lblClosingBalance.Text = (TotalDebitSum - TotalCreditSum).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + "Dr";
                }
                else if ((TotalCreditSum - TotalDebitSum) > 0)
                {

                    lblClosingBalance.Text = (TotalCreditSum - TotalDebitSum).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + "Cr";
                }
            }
             */


            WriteGroupBalance(0, "GROUP BALANCE TOTAL", 0, TotalDebitSum, TotalCreditSum, "TOTAL");
            if ((TotalDebitSum - TotalCreditSum) > 0)
            {
                lblClosingBalance.Text = "Closing Balance: " + (TotalDebitSum - TotalCreditSum).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + " Dr";
            }
            else if ((TotalCreditSum - TotalDebitSum) > 0)
            {
                lblClosingBalance.Text = "Closing Balance: " + (TotalCreditSum - TotalDebitSum).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + " Cr";
            }


#endregion 
 
            ProgressForm.UpdateProgress(100, "Preparing report for display...");
            if (ProgressForm.InvokeRequired)
                ProgressForm.BeginInvoke(new Action(() => ProgressForm.Close())); 
           
        }

        private void grdTrial_DoubleClick(object sender, EventArgs e)
        {
            try
            {               
                //Get the Selected Row
                int CurRow = grdGroupBalance.Selection.GetSelectionRegion().GetRowsIndex()[0];
                SourceGrid.CellContext cellType = new SourceGrid.CellContext(grdGroupBalance, new SourceGrid.Position(CurRow, 5));
                SourceGrid.CellContext cellTypeID = new SourceGrid.CellContext(grdGroupBalance, new SourceGrid.Position(CurRow, 4));
                string Type = (cellType.Value).ToString();
                //read id
                string ID = (cellTypeID.Value).ToString();//I
                if (ID == "")//IF ID is blank means there must be total or Opening balance case
                    return;
                
                if (Type == "GROUP")
                {
                    int CurRow1 = grdGroupBalance.Selection.GetSelectionRegion().GetRowsIndex()[0];
                    SourceGrid.CellContext cellID = new SourceGrid.CellContext(grdGroupBalance, new SourceGrid.Position(CurRow1, 4));
                    GroupBalanceSettings m_GBS1 = new GroupBalanceSettings();
                    m_GBS1.HasDateRange = m_GBS.HasDateRange;//
                    m_GBS1.AccClassID = m_GBS.AccClassID;
                    m_GBS1.ProjectID = m_GBS.ProjectID;
                    m_GBS1.FromDate = m_GBS.FromDate;
                    m_GBS1.ToDate = m_GBS.ToDate;
                    m_GBS1.OnlyPrimaryGroups = m_GBS.OnlyPrimaryGroups;
                    m_GBS1.ShowZeroBalance = m_GBS1.ShowZeroBalance;
                    m_GBS1.AllGroups = m_GBS.AllGroups;
                    m_GBS1.GroupID = Convert.ToInt32(cellID.Value);//Store the GroupID value on object which achieve while double clicking the corresponding row of cell
                    frmGroupBalance m_GrpBal = new frmGroupBalance(m_GBS1,m_MDIForm);//Passing the object as argument of Constructor
                    m_GrpBal.ShowDialog();

                }
                else if (Type == "LEDGER")
                {
                    int CurRow2 = grdGroupBalance.Selection.GetSelectionRegion().GetRowsIndex()[0];
                    SourceGrid.CellContext cellID = new SourceGrid.CellContext(grdGroupBalance, new SourceGrid.Position(CurRow2, 4));
                    string LedgerID = (cellID.Value).ToString();
                    TransactSettings m_TS = new TransactSettings();
                    m_TS.LedgerID = Convert.ToInt32(cellID.Value);
                    m_TS.HasDateRange = m_GBS.HasDateRange;
                    m_TS.AccClassID = m_GBS.AccClassID;
                    m_TS.ProjectID = m_GBS.ProjectID;
                    m_TS.FromDate = m_GBS.FromDate;
                    m_TS.ToDate = m_GBS.ToDate;
                    m_TS.ShowZeroBalance = m_GBS.ShowZeroBalance;
                    m_TS.LedgerID = Convert.ToInt32(cellID.Value);
                    frmTransaction m_Transact = new frmTransaction(m_TS,m_MDIForm);
                    m_Transact.ShowDialog();
                }
            }
            catch (Exception ex)
            {

                Global.Msg(ex.Message);
            }
        }
   


        private void frmGroupBalance_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
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

            dsTrial.Clear();//It clear the previous records of dataset on crystal report...when this button is pressed
            //otherwise it populate the records again and again

            double DebitSum, CreditSum;
            DebitSum = CreditSum = 0;
            rptTrialBalance rpt = new rptTrialBalance();
            //Fill the logo on the report
            Misc.WriteLogo(dsTrial, "tblImage");
            //Set DataSource to be dsTrial dataset on the report
            rpt.SetDataSource(dsTrial);
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

            pdvReport_Head.Value = "Gropu Balance";
            pvCollection.Clear();
            pvCollection.Add(pdvReport_Head);
            rpt.DataDefinition.ParameterFields["Report_Head"].ApplyCurrentValues(pvCollection);

            pdvFiscal_Year.Value = "Fiscal Year:" + m_CompanyDetails.FYFrom;
            pvCollection.Clear();
            pvCollection.Add(pdvFiscal_Year);
            rpt.DataDefinition.ParameterFields["Fiscal_Year"].ApplyCurrentValues(pvCollection);

            //Update the progressbar
            ProgressForm.UpdateProgress(80, "Calculating Parameters...");

            //Display the date in crystal report according to available from and to dates
            if (m_GBS.FromDate != null && m_GBS.ToDate != null)
            {
                pdvReport_Date.Value = "From: " + Date.ToSystem(Convert.ToDateTime(m_GBS.FromDate)) + " To: " + Date.ToSystem(Convert.ToDateTime(m_GBS.ToDate));
            }
            if (m_GBS.ToDate != null)
            {
                pdvReport_Date.Value = "As on Date: " + Date.ToSystem(Convert.ToDateTime(m_GBS.ToDate));
            }
            if (m_GBS.FromDate != null)
            {
                //This is actually not applicable
                pdvReport_Date.Value = "From: " + Date.ToSystem(Convert.ToDateTime(m_GBS.FromDate));
            }

            pvCollection.Clear();
            pvCollection.Add(pdvReport_Date);
            rpt.DataDefinition.ParameterFields["Report_Date"].ApplyCurrentValues(pvCollection);

            DataTable dt = AccountGroup.GetGroupTable(m_GBS.GroupID);//
            int Sno = 1;
            foreach (DataRow dr in dt.Rows)
            {
                double m_dbal = 0;
                double m_cbal = 0;

                // Block for DateTime range selection,here bydefault data range is selected,soo just call thoes method which have daterange parameter
                //with backup we have also method which doesnot concern date range
                //But in this case we have date range by default soo i used this one.....
                Transaction.GetGroupBalance(m_GBS.FromDate, m_GBS.ToDate, Convert.ToInt32(dr["GroupID"]), true, ref m_dbal, ref m_cbal, m_GBS.AccClassID, m_GBS.ProjectID);
                #region BLOCK FOR CALLING METHOD ACCORDING TO DATE RANGE SELECTION
                //if (m_TBS.HasDateRange == true)//When datetime is selected

                //    Transaction.GetGroupBalance(m_TBS.FromDate, m_TBS.ToDate, Convert.ToInt32(dr["GroupID"]), true, ref m_dbal, ref m_cbal, m_TBS.AccClassID);
                //else//Otherwise
                //    Transaction.GetGroupBalance(Convert.ToInt32(dr["GroupID"]), true, ref m_dbal, ref m_cbal, m_TBS.AccClassID);
                #endregion// It does not need now //It does not need now
                if (m_GBS.ShowZeroBalance == false && m_dbal == 0 && m_cbal == 0) //In case of zero balance
                {
                    //Do nothing
                }
                else
                {
                    string EngName = AccountGroup.GetEngName(Convert.ToInt32(dr["GroupID"]).ToString());  //calling this funtion for getting EngName from tblGroup corresponding to GroupID having corresponding GroupID
                    double Balance = (m_dbal - m_cbal);//For Asset[Debit Balance - Credit Balance]
                    WriteGroupBalanceOnCrystalRpt(Sno, EngName, Convert.ToInt32(dr["GroupID"]), m_dbal.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), m_cbal.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), "GROUP");

                    DebitSum += m_dbal;
                    CreditSum += m_cbal;

                    //If details is selected, show details i.e. ledgers present inside
                    if (m_GBS.Details == true)
                        //WriteDetails(Convert.ToInt32(dr["GroupID"]), AccountType.Assets);
                        WriteDetails(Convert.ToInt32(dr["GroupID"]), true);

                }//End of zero balance check
                Sno++;
            }//End for loop
            double debitsum, creditsum;
            debitsum = creditsum = 0;

            WriteLedger(m_GBS.GroupID, ref debitsum, ref creditsum, true);

            #region BLOCK FOR CALCULATING DIFFERENCE IN OPENING BALANCE
            double TotalDrOpBal, TotalCrOpBal;
            TotalDrOpBal = TotalCrOpBal = 0;


            Transaction.GetOpeningBalanceFromGroup(m_GBS.GroupID, m_GBS.AccClassID, ref TotalDrOpBal, ref TotalCrOpBal);

            if ((TotalDrOpBal != TotalCrOpBal))
            {
                if (TotalDrOpBal > TotalCrOpBal)//Total Opening balance in Debit side is greater so balance by adding this amount in Credit Balance
                {
                    CreditSum += (TotalDrOpBal - TotalCrOpBal);
                    WriteGroupBalanceOnCrystalRpt(0, "DIFFERENCE IN OPENING BALANCE:", 0, (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), (TotalDrOpBal - TotalCrOpBal).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), "SUBGROUP");
                }
                else if (TotalDrOpBal < TotalCrOpBal)//Total Opening balance in Debit side is greater so balance by adding this amount in Credit Balance
                {
                    DebitSum += (TotalCrOpBal - TotalDrOpBal);

                    WriteGroupBalanceOnCrystalRpt(0, "DIFFERENCE IN OPENING BALANCE:", 0, (TotalCrOpBal - TotalDrOpBal).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), "SUBGROUP");
                }
            }
            #endregion

            #region BLOCK FOR TOTAL AMOUNT CALCULATION FOR TRIAL BALANCE
            WriteGroupBalanceOnCrystalRpt(0, "GROUP BALANCE TOTAL", 0, DebitSum.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), CreditSum.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), "GROUP");
            #endregion

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
                default:
                    frm.Show();
                    frm.WindowState = FormWindowState.Maximized;
                    break;
            }
            frm.WindowState = FormWindowState.Maximized;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    
      
    }
}
