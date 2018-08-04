using System;
using System.Windows;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
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
using Inventory.Forms;

namespace Inventory
{
    public partial class frmTrial : Form
    {
        private DataSet.dsTrialBalance dsTrial = new DataSet.dsTrialBalance();
        private DataSet.dsTrailBalancePreviousYear dsTrialPY = new DataSet.dsTrailBalancePreviousYear();
       
        private SourceGrid.Cells.Controllers.CustomEvents dblClick;//Double click for DebitNote Register
        private TrialBalanceSettings m_TBS;
        private int TrialRowsCount;
        double DValue = 0;
        double Cvalue = 0;
        double DValuePY = 0;
        double CvaluePY = 0;

        //For debit and credit total storage
        double m_DebitTotal=0;
        double m_CreditTotal = 0;
        double m_DebitTotalPY = 0;
        double m_CreditTotalPY = 0;

        ArrayList AccClassID = new ArrayList();
        DataTable dtGetAllProduct = new DataTable();
        string AccClassIDsXMLString ="";
        string ProjectIDsXMLString = "";
        int Sno=0;
        
        private enum PrintType
        {
            None,
            DirectPrint,
            Excel,
            PDF,
            CrystalReport,
            Email
        }
        private string FileName = "";
        public frmTrialBalanceSetting setcontrols;
        //Different grid views
        private SourceGrid.Cells.Views.Cell GroupView;
        private SourceGrid.Cells.Views.Cell LedgerView;
        private SourceGrid.Cells.Views.Cell SubGroupView;
        //For Export Menu
        ContextMenu Menu_Export;
        public frmTrial(TrialBalanceSettings TBS,Form accesscontrol)//Constructor having class object as argument
        {
            setcontrols = accesscontrol as frmTrialBalanceSetting;
            try
            {
                #region BLOCK FOR INTIALIZING THE CONSTRUCTOR PARAMETERS
                InitializeComponent();
                m_TBS = new TrialBalanceSettings();
                m_TBS.FromDate = TBS.FromDate;
                m_TBS.ToDate = TBS.ToDate;
                m_TBS.GroupID = TBS.GroupID;
                m_TBS.AccClassID = TBS.AccClassID;
                m_TBS.HasDateRange = TBS.HasDateRange;
                m_TBS.ShowZeroBalance = TBS.ShowZeroBalance;
                m_TBS.ShowSecondLevelGroupDtl = TBS.ShowSecondLevelGroupDtl;
                m_TBS.Details = TBS.Details;
                m_TBS.AllGroups = TBS.AllGroups;
                m_TBS.OnlyPrimaryGroups = TBS.OnlyPrimaryGroups;
                m_TBS.ProjectID = TBS.ProjectID;
                m_TBS.LedgerOnly = TBS.LedgerOnly;
                m_TBS.ShowPreviousYear = TBS.ShowPreviousYear;
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Gets the selected root accounting class ID
        /// </summary>
        /// <returns></returns>
        private int GetRootAccClassID()
        {
            if (m_TBS.AccClassID.Count > 0)
            {
                //Find Root Class
                DataTable dtTemp = AccountClass.GetRootAccClass(Convert.ToInt32(m_TBS.AccClassID[0]));
                return Convert.ToInt32(dtTemp.Rows[0]["AccClassID"]);

            }

            return 1;//The default root class ID
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

        //Writes the header on the grid
        private void MakeHeader()
        {
            if (m_TBS.ShowPreviousYear == true)
            {
                //Write header part
                grdTrial.Rows.Insert(0);
                grdTrial[1, 0] = new MyHeader("Account Code");
                grdTrial[1, 1] = new MyHeader("Account Name");
                grdTrial[1, 2] = new MyHeader("Debit Amount");
                grdTrial[1, 3] = new MyHeader("Credit Amount");
                grdTrial[1, 4] = new MyHeader("ID");
                grdTrial[1, 5] = new MyHeader("Type");
                grdTrial[1, 6] = new MyHeader("Debit Amount");
                grdTrial[1, 7] = new MyHeader("Credit Amount");

                //Define the width of column size
                grdTrial[1, 0].Column.Width = 120;
                grdTrial[1, 1].Column.Width = 405;
                grdTrial[1, 2].Column.Width = 120;
                grdTrial[1, 3].Column.Width = 120;
                grdTrial[1, 4].Column.Width = 0;
                grdTrial[1, 5].Column.Width = 0;
                grdTrial[1, 6].Column.Width = 105;
                grdTrial[1, 7].Column.Width = 105;

                //Code for making column invisible
                grdTrial.Columns[4].Visible = false;// making third column invisible and using it in  programming     
                grdTrial.Columns[5].Visible = false;
            }
            else
            {
                grdTrial.Rows.Insert(0);
                grdTrial[0, 0] = new MyHeader("Account Code");
                grdTrial[0, 1] = new MyHeader("Account Name");
                grdTrial[0, 2] = new MyHeader("Debit Amount");
                grdTrial[0, 3] = new MyHeader("Credit Amount");
                grdTrial[0, 4] = new MyHeader("ID");
                grdTrial[0, 5] = new MyHeader("Type");
                //grdTrial[0, 6] = new MyHeader("Debit Amount");
               // grdTrial[0, 7] = new MyHeader("Credit Amount");

                //Define the width of column size
                grdTrial[0, 0].Column.Width = 130;
               
                grdTrial[0, 1].Column.Width = 515;
                grdTrial[0, 2].Column.Width = 160;
                grdTrial[0, 3].Column.Width = 160;
                grdTrial[0, 4].Column.Width = 0;
                grdTrial[0, 5].Column.Width = 0;
               // grdTrial[0, 6].Column.Width = 105;
               // grdTrial[0, 7].Column.Width = 105;

                //Code for making column invisible
                grdTrial.Columns[4].Visible = false;// making third column invisible and using it in  programming     
                grdTrial.Columns[5].Visible = false;
            }
        }

        private void WriteTrial(string SNo, string Name, int GroupID, string DrBal, string CrBal, string Type,bool IsCrystalReport)
        {
            try
            {
                if (!IsCrystalReport)//for writting on grid
                {
                    if (Type == "GROUP")
                    {
                        GroupView = new SourceGrid.Cells.Views.Cell();
                        if (TrialRowsCount % 2 == 0)
                        {
                            GroupView.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Color.White);
                        }
                        else
                        {
                            GroupView.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Global.Grid_Color);
                        }
                        //GroupView.Background = new DevAge.Drawing.VisualElements.BackgroundLinearGradient(Color.RoyalBlue, Color.LightBlue, 0);
                        //GroupView.ForeColor = Color.FromKnownColor(KnownColor.ActiveCaptionText);
                        //GroupView.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleCenter;
                        //GroupView.Border = DevAge.Drawing.RectangleBorder.NoBorder;
                        //GroupView.Font = new Font(LangMgr.GetFont(), FontStyle.Bold);
                        GroupView.Font = new Font("Arial",10, FontStyle.Bold);

                    }
                    if (Type == "LEDGER")
                    {
                        //Text format for Ledgers
                        LedgerView = new SourceGrid.Cells.Views.Cell();
                        if (TrialRowsCount % 2 == 0)
                        {
                            LedgerView.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Color.White);
                        }
                        else
                        {
                            LedgerView.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Global.Grid_Color);
                        }
                       // LedgerView.Font = new Font(LangMgr.GetFont(), FontStyle.Italic);
                        LedgerView.Font = new Font("Arial", 9, FontStyle.Italic);
                        LedgerView.ForeColor = Color.Blue;
                    }
                    if (Type == "SUBGROUP")
                    {
                        //Text format for SubGroup
                        SubGroupView = new SourceGrid.Cells.Views.Cell();
                        if (TrialRowsCount % 2 == 0)
                        {
                            SubGroupView.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Color.White);
                        }
                        else
                        {
                            SubGroupView.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Global.Grid_Color);
                        }
                        SubGroupView.Font = new Font(LangMgr.GetFont(), FontStyle.Regular);
                    }

                    SourceGrid.Cells.Views.Cell alternate = new SourceGrid.Cells.Views.Cell();
                    if (TrialRowsCount % 2 == 0)
                    {
                        alternate.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Color.White);
                    }
                    else
                    {
                        alternate.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Global.Grid_Color);
                    }

                    grdTrial.Rows.Insert(grdTrial.RowsCount);
                    // Block for getting GroupName            
                   // string strSNo = (SNo == 0 ? "" : SNo.ToString());
                    
                    grdTrial[TrialRowsCount, 0] = new SourceGrid.Cells.Cell(((SNo.Trim().Length>0)||(SNo.Trim()=="0")?SNo:""));
                    grdTrial[TrialRowsCount, 0].AddController(dblClick);
                    grdTrial[TrialRowsCount, 0].View = new SourceGrid.Cells.Views.Cell(alternate);

                    grdTrial[TrialRowsCount, 1] = new SourceGrid.Cells.Cell(Name);

                    grdTrial[TrialRowsCount, 4] = new SourceGrid.Cells.Cell(GroupID.ToString());//Adding GroupID of each row in fourth column as invisible for further use

                    grdTrial[TrialRowsCount, 5] = new SourceGrid.Cells.Cell(Type);

                    grdTrial[TrialRowsCount, 2] = new SourceGrid.Cells.Cell(DrBal.ToString());
                    grdTrial[TrialRowsCount, 3] = new SourceGrid.Cells.Cell(CrBal.ToString());



                    //To store the current view types accourding to the row type(Ledger, Group etc)
                    SourceGrid.Cells.Views.Cell CurrentView = new SourceGrid.Cells.Views.Cell();

                   
                    switch (Type)
                    {
                        case "GROUP":   
                            CurrentView = GroupView;
                            break;
                        case "LEDGER":
                            CurrentView = LedgerView;
                            grdTrial[TrialRowsCount, 1].Value = "    " + grdTrial[TrialRowsCount, 1].Value; //Give a little space for ledger so that it appears it is inside its parent group
                            break;
                        case "SUBGROUP":
                            CurrentView = SubGroupView;
                            grdTrial[TrialRowsCount, 1].Value = "  " + grdTrial[TrialRowsCount, 1].Value; //Give a little space for subgroup so that it appears it is inside its parent group
                            break;
                        default:
                            CurrentView = SubGroupView; //Because it is the normal formatting without any makeups
                            break;
                    }

                    grdTrial[TrialRowsCount, 1].AddController(dblClick); 
                    grdTrial[TrialRowsCount, 1].View = CurrentView;
                   // grdTrial[TrialRowsCount, 1].View = new SourceGrid.Cells.Views.Cell(alternate);
                      
                  

                    grdTrial[TrialRowsCount, 2].AddController(dblClick);
                    grdTrial[TrialRowsCount, 2].View = new SourceGrid.Cells.Views.Cell(CurrentView);
                    //grdTrial[TrialRowsCount, 2].View = new SourceGrid.Cells.Views.Cell(alternate);
                    grdTrial[TrialRowsCount, 2].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;

                    grdTrial[TrialRowsCount, 3].AddController(dblClick);
                    grdTrial[TrialRowsCount, 3].View = new SourceGrid.Cells.Views.Cell(CurrentView);
                   // grdTrial[TrialRowsCount, 3].View = new SourceGrid.Cells.Views.Cell(alternate);
                    grdTrial[TrialRowsCount, 3].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;
                    //Increment assets rows count
                    TrialRowsCount++;

                }
                else if (IsCrystalReport)//writing on crystal reports
                {
                    //string strSNo = (SNo == 0 ? "" : SNo.ToString());
                    if (Name == "TRIAL TOTAL")
                    {
                        m_DebitTotal =Convert.ToDouble( DrBal);
                        m_CreditTotal =Convert.ToDouble( CrBal);
                    }
                    else
                    {

                        string strSNo = SNo;
                        dsTrial.Tables["tblGroup"].Rows.Add(strSNo, Name, DrBal, CrBal, Type);
                    }

                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void WriteTrialPY(string SNo, string Name, int GroupID, string DrBal, string CrBal, string Type, bool IsCrystalReport, string DrBalPY, string CrBalPY)
        {
            if (!IsCrystalReport)//for writting on grid
            {
                if (Type == "GROUP")
                {
                    GroupView = new SourceGrid.Cells.Views.Cell();
                    if (TrialRowsCount % 2 == 0)
                    {
                        GroupView.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Color.White);
                    }
                    else
                    {
                        GroupView.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Global.Grid_Color);
                    }
                    //GroupView.Background = new DevAge.Drawing.VisualElements.BackgroundLinearGradient(Color.RoyalBlue, Color.LightBlue, 0);
                    //GroupView.ForeColor = Color.FromKnownColor(KnownColor.ActiveCaptionText);
                    //GroupView.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleCenter;
                    //GroupView.Border = DevAge.Drawing.RectangleBorder.NoBorder;
                   // GroupView.Font = new Font(LangMgr.GetFont(), FontStyle.Bold);
                    GroupView.Font = new Font("Arial", 10, FontStyle.Bold);

                }
                if (Type == "LEDGER")
                {
                    //Text format for Ledgers
                    LedgerView = new SourceGrid.Cells.Views.Cell();
                    if (TrialRowsCount % 2 == 0)
                    {
                        LedgerView.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Color.White);
                    }
                    else
                    {
                        LedgerView.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Global.Grid_Color);
                    }
                   // LedgerView.Font = new Font(LangMgr.GetFont(), FontStyle.Italic);
                    LedgerView.Font = new Font("Arial", 9, FontStyle.Italic);
                    LedgerView.ForeColor = Color.Blue;
                }
                if (Type == "SUBGROUP")
                {
                    //Text format for SubGroup
                    SubGroupView = new SourceGrid.Cells.Views.Cell();
                    if (TrialRowsCount % 2 == 0)
                    {
                        SubGroupView.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Color.White);
                    }
                    else
                    {
                        SubGroupView.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Global.Grid_Color);
                    }
                    SubGroupView.Font = new Font(LangMgr.GetFont(), FontStyle.Regular);
                }

                SourceGrid.Cells.Views.Cell alternate = new SourceGrid.Cells.Views.Cell();
                if (TrialRowsCount % 2 == 0)
                {
                    alternate.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Color.White);
                }
                else
                {
                    alternate.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Global.Grid_Color);
                }
                grdTrial.Rows.Insert(grdTrial.RowsCount);
                // Block for getting GroupName            
                //string strSNo = (SNo == 0 ? "" : SNo.ToString());
                string strSNo = SNo;
               
                SourceGrid.Cells.Views.Cell header = new SourceGrid.Cells.Views.Cell();
                header.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Color.Cornsilk);
                header.ForeColor = Color.FromKnownColor(KnownColor.ActiveCaptionText);
                header.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleCenter;
                header.Border = DevAge.Drawing.RectangleBorder.NoBorder;
                header.Font = new Font(LangMgr.GetFont().FontFamily, LangMgr.GetFont().Size + 1, FontStyle.Bold);
               
                grdTrial[0, 0] = new SourceGrid.Cells.Cell("");
                grdTrial[0, 0].View = new SourceGrid.Cells.Views.Cell(header);
                grdTrial[0, 1] = new SourceGrid.Cells.Cell("");
                grdTrial[0, 1].View = new SourceGrid.Cells.Views.Cell(header);
                grdTrial[0, 2] = new SourceGrid.Cells.Cell("This Year");
                grdTrial[0, 2].View = new SourceGrid.Cells.Views.Cell(header);
                grdTrial[0, 2].ColumnSpan = grdTrial.ColumnsCount - 7;
                //grdTrial[0, 3].ColumnSpan = grdTrial.ColumnsCount - 6;
                grdTrial[0, 3] = new SourceGrid.Cells.Cell("");
                grdTrial[0, 3].View = new SourceGrid.Cells.Views.Cell(header);
              
                grdTrial[0, 4] = new SourceGrid.Cells.Cell("");
                grdTrial[0, 4].View = new SourceGrid.Cells.Views.Cell(header);
                grdTrial[0, 5] = new SourceGrid.Cells.Cell("");
                grdTrial[0, 5].View = new SourceGrid.Cells.Views.Cell(header);
                grdTrial[0, 6] = new SourceGrid.Cells.Cell("Previous Year");
                grdTrial[0, 6].View = new SourceGrid.Cells.Views.Cell(header);
                grdTrial[0, 6].ColumnSpan = grdTrial.ColumnsCount - 7;
                grdTrial[0, 7] = new SourceGrid.Cells.Cell("");
                grdTrial[0, 7].View = new SourceGrid.Cells.Views.Cell(header);
               
              
                grdTrial[TrialRowsCount+1, 0] = new SourceGrid.Cells.Cell(strSNo);
                grdTrial[TrialRowsCount+1, 0].AddController(dblClick);
                grdTrial[TrialRowsCount + 1, 0].View = new SourceGrid.Cells.Views.Cell(alternate);

                grdTrial[TrialRowsCount+1, 1] = new SourceGrid.Cells.Cell(Name);

                grdTrial[TrialRowsCount+1, 4] = new SourceGrid.Cells.Cell(GroupID.ToString());//Adding GroupID of each row in fourth column as invisible for further use

                grdTrial[TrialRowsCount+1, 5] = new SourceGrid.Cells.Cell(Type);

                grdTrial[TrialRowsCount+1, 2] = new SourceGrid.Cells.Cell(DrBal.ToString());
                grdTrial[TrialRowsCount+1, 3] = new SourceGrid.Cells.Cell(CrBal.ToString());
                grdTrial[TrialRowsCount+1, 6] = new SourceGrid.Cells.Cell(DrBalPY.ToString());//Adding GroupID of each row in fourth column as invisible for further use

                grdTrial[TrialRowsCount+1, 7] = new SourceGrid.Cells.Cell(CrBalPY.ToString());



                //To store the current view types accourding to the row type(Ledger, Group etc)
                SourceGrid.Cells.Views.Cell CurrentView = new SourceGrid.Cells.Views.Cell();

                switch (Type)
                {
                    case "GROUP":
                        CurrentView = GroupView;
                        break;
                    case "LEDGER":
                        CurrentView = LedgerView;
                        grdTrial[TrialRowsCount, 1].Value = "    " + grdTrial[TrialRowsCount, 1].Value; //Give a little space for ledger so that it appears it is inside its parent group
                        break;
                    case "SUBGROUP":
                        CurrentView = SubGroupView;
                        grdTrial[TrialRowsCount, 1].Value = "  " + grdTrial[TrialRowsCount, 1].Value; //Give a little space for subgroup so that it appears it is inside its parent group
                        break;
                    default:
                        CurrentView = SubGroupView; //Because it is the normal formatting without any makeups
                        break;
                }

                grdTrial[TrialRowsCount+1, 1].AddController(dblClick);
                grdTrial[TrialRowsCount+1, 1].View = CurrentView;
               // grdTrial[TrialRowsCount+1, 1].View = new SourceGrid.Cells.Views.Cell(alternate);

                grdTrial[TrialRowsCount+1, 2].AddController(dblClick);
                grdTrial[TrialRowsCount+1, 2].View = new SourceGrid.Cells.Views.Cell(CurrentView);
              //  grdTrial[TrialRowsCount + 1, 2].View = new SourceGrid.Cells.Views.Cell(alternate);
                grdTrial[TrialRowsCount+1, 2].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;

                grdTrial[TrialRowsCount+1, 3].AddController(dblClick);
                grdTrial[TrialRowsCount+1, 3].View = new SourceGrid.Cells.Views.Cell(CurrentView);
                //grdTrial[TrialRowsCount + 1, 3].View = new SourceGrid.Cells.Views.Cell(alternate);
                grdTrial[TrialRowsCount+1, 3].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;

                grdTrial[TrialRowsCount+1, 6].AddController(dblClick);
                grdTrial[TrialRowsCount+1, 6].View = new SourceGrid.Cells.Views.Cell(CurrentView);
                //grdTrial[TrialRowsCount + 1, 6].View = new SourceGrid.Cells.Views.Cell(alternate);
                grdTrial[TrialRowsCount+1, 6].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;

                grdTrial[TrialRowsCount+1, 7].AddController(dblClick);
                grdTrial[TrialRowsCount+1, 7].View = new SourceGrid.Cells.Views.Cell(CurrentView);
                //grdTrial[TrialRowsCount + 1, 7].View = new SourceGrid.Cells.Views.Cell(alternate);
                grdTrial[TrialRowsCount+1, 7].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;
                //Increment assets rows count
                TrialRowsCount++;
            }
            else if (IsCrystalReport)//writing on crystal reports
            {
                if (Name == "TRIAL TOTAL")
                {
                    m_DebitTotal = Convert.ToDouble(DrBal);
                    m_CreditTotal = Convert.ToDouble(CrBal);
                    m_DebitTotalPY = Convert.ToDouble(DrBalPY);
                    m_CreditTotalPY = Convert.ToDouble(CrBalPY);
                }
                else
                {
                    //string strSNo = (SNo == 0 ? "" : SNo.ToString());
                    string strSNo = SNo;
                    dsTrialPY.Tables["tblGroup"].Rows.Add(strSNo, Name, DrBal, CrBal, Type, DrBalPY, CrBalPY);
                }
            }

        }

        /// <summary>
        /// For displaying Ledger
        /// </summary>
        /// <param name="GroupID"></param>
        /// <param name="IsCrystalReport"></param>
        private void WriteDetails(int GroupID,bool IsCrystalReport)
        {
            if (Global.isOpeningTrial == false)
            {
                try
                {
                    DataTable dtDtlLedgerID = AccountGroup.GetDetailLedgerID(GroupID, true);
                    int Sno = 1;
                    foreach (DataRow drDtlLedgerID in dtDtlLedgerID.Rows)
                    {
                        double DebBal = 0;
                        double CreBal = 0;

                        Transaction.GetLedgerBalance(null, m_TBS.ToDate, Convert.ToInt32(drDtlLedgerID["LedgerID"]), ref  DebBal, ref CreBal, m_TBS.AccClassID, m_TBS.ProjectID);
                        if (m_TBS.ShowZeroBalance == false && DebBal == 0 && CreBal == 0)
                            //return;
                            continue;//It cotinue the loop except below code              
                        WriteTrial(drDtlLedgerID["LedgerCode"].ToString(), drDtlLedgerID["EngName"].ToString(), Convert.ToInt32(drDtlLedgerID["LedgerID"]), DebBal.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), CreBal.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), "LEDGER", IsCrystalReport);
                        Sno++;

                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                try
                {
                    DataTable dtDtlLedgerID = AccountGroup.GetDetailLedgerID(GroupID, true);
                    int Sno = 1;
                    foreach (DataRow drDtlLedgerID in dtDtlLedgerID.Rows)
                    {
                        double DebBal = 0;
                        double CreBal = 0;

                        Transaction.GetOpeningLedgerBalance(null, m_TBS.ToDate, Convert.ToInt32(drDtlLedgerID["LedgerID"]), ref  DebBal, ref CreBal, m_TBS.AccClassID, m_TBS.ProjectID);
                        if (m_TBS.ShowZeroBalance == false && DebBal == 0 && CreBal == 0)
                            //return;
                            continue;//It cotinue the loop except below code              
                        WriteTrial(drDtlLedgerID["LedgerCode"].ToString(), drDtlLedgerID["EngName"].ToString(), Convert.ToInt32(drDtlLedgerID["LedgerID"]), DebBal.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), CreBal.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), "LEDGER", IsCrystalReport);
                        Sno++;

                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void WriteSecondLevel(int GroupID,bool IsCrystalReport)
        {
            DataTable dtSecDtl = AccountGroup.GetGroupTable(GroupID);
            int Sno = 1;
            foreach (DataRow dr1 in dtSecDtl.Rows)
            {
                double m_dbal1 = 0;
                double m_cbal1 = 0;

                //just calling the method which need only date range parameter
                if (m_TBS.HasDateRange)
                {
                    Transaction.GetGroupBalance(m_TBS.FromDate, m_TBS.ToDate, Convert.ToInt32(dr1["GroupID"]), true, ref m_dbal1, ref m_cbal1, m_TBS.AccClassID, m_TBS.ProjectID);
                }
                else
                {                    
                    Transaction.GetGroupBalance(Convert.ToDateTime("01 / 01 / 1900"), Convert.ToDateTime("01 / 01 / 1900"), Convert.ToInt32(dr1["GroupID"]), true, ref m_dbal1, ref m_cbal1, m_TBS.AccClassID, m_TBS.ProjectID);
                }
                if (m_TBS.ShowZeroBalance == false && m_dbal1 == 0 && m_cbal1 == 0)
                    continue;

                ////Checking whether debit balance or credit balance?
                //double Balance1;
                //Balance1 = m_dbal1 - m_cbal1;
                // Block for getting GroupName 
                string EngName1 = AccountGroup.GetEngName((dr1["GroupID"].ToString()));  //calling this funtion for getting EngName from tblGroup corresponding to GroupID having corresponding GroupID

                //if (Type == AccountType.Assets)
                //    WriteAssets(0, "- " + EngName1, Convert.ToInt32(dr1["GroupID"]), Balance1.ToString(), "GROUP");
                //else if (Type == AccountType.Liabilities)
                //    WriteLiabilities(0, "- " + EngName1, Convert.ToInt32(dr1["GroupID"]), Balance1.ToString(), "GROUP");

                WriteTrial(dr1["LedgerCode"].ToString(), "* " + EngName1, Convert.ToInt32(dr1["GroupID"]), m_dbal1.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), m_cbal1.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), "SUBGROUP", IsCrystalReport);
                    Sno++;
                
            }
        }

        private void WriteLedger(int GroupID,bool IsCrystalReport)
        {
            //Ledger processing starts for Asset
            Transaction Transaction = new Transaction();
            DataTable dtledg = Ledger.GetLedgerTable(GroupID);
            int Sno = 1;
            foreach (DataRow drledger in dtledg.Rows)
            {
                double m_dbal1 = 0;
                double m_cbal1 = 0;

                //Just calling the method which need only need the date range parameter
                Transaction.GetLedgerBalance(m_TBS.FromDate, m_TBS.ToDate, Convert.ToInt32(drledger["LedgerID"]), ref m_dbal1, ref m_cbal1, m_TBS.AccClassID,m_TBS.ProjectID);
       
                if (m_TBS.ShowZeroBalance == false && m_dbal1 == 0 && m_cbal1 == 0)
                    continue;
                //grdBalanceSheet.Rows.Insert(AssetRowsCount);
                grdTrial.Rows.Insert(TrialRowsCount);
                // Block for getting LedgerName
                DataTable dtLedgerInfo = Ledger.GetLedgerInfo(Convert.ToInt32(drledger["LedgerID"]), LangMgr.DefaultLanguage);

                DataRow drLedgerInfo = dtLedgerInfo.Rows[0];
                //double Balance = (m_dbal1 - m_cbal1);//for Asset[ Balance =(Debit Balance-Credit Balance)]

                //if (Type == AccountType.Assets)
                //    WriteAssets(0, drLedgerInfo["LedName"].ToString(), Convert.ToInt32(drledger["LedgerID"]), Balance.ToString(), "LEDGER");
                //else if (Type == AccountType.Liabilities)
                //    WriteLiabilities(0, drLedgerInfo["LedName"].ToString(), Convert.ToInt32(drledger["LedgerID"]), Balance.ToString(), "LEDGER");

                WriteTrial(drledger["LedgerCode"].ToString(), drledger["LedName"].ToString(), Convert.ToInt32(drledger["LedgerID"]), m_dbal1.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), m_cbal1.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), "LEDGER", IsCrystalReport);
                    Sno++;
                              
            }
        }

        private void GetOpeningBalanceSummary(int AccClassID, ref double TotalDrOpBal,ref double  TotalCrOpBal)
        {
            DataTable dtAllLedgersInfo = OpeningBalance.GetAccClassOpeningBalance(AccClassID, -1); //Collect all ledger information first
            //double TotalDrOpBal, TotalCrOpBal;
            TotalDrOpBal = TotalCrOpBal = 0;
            foreach(DataRow drAllLedgersInfo in dtAllLedgersInfo.Rows)
            {       
                if (!drAllLedgersInfo.IsNull("OpenBal"))
                {
                    if (drAllLedgersInfo["OpenBalDrCr"].ToString() == "DEBIT")
                        TotalDrOpBal += (Convert.ToDouble(drAllLedgersInfo["OpenBal"]));
                    else if (drAllLedgersInfo["OpenBalDrCr"].ToString() == "CREDIT")
                        TotalCrOpBal += (Convert.ToDouble(drAllLedgersInfo["OpenBal"]));
                }
            }
            //TotalDrOpBal = 5999;
        }

        private void DisplayTrialBlance(bool IsCrystalReport)
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

            ProgressForm.UpdateProgress(20, "Initializing report...");

            if(!IsCrystalReport)//Need only for writting on grid...not for crystal reports
            {
                DisplayBannar();
                #region BLOCK FOR ORIENTATION PURPOSE
              
                //Text format for Total
                //GroupView = new SourceGrid.Cells.Views.Cell();
                ////GroupView.Background = new DevAge.Drawing.VisualElements.BackgroundLinearGradient(Color.RoyalBlue, Color.LightBlue, 0);
                ////GroupView.ForeColor = Color.FromKnownColor(KnownColor.ActiveCaptionText);
                ////GroupView.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleCenter;
                ////GroupView.Border = DevAge.Drawing.RectangleBorder.NoBorder;
                //GroupView.Font = new Font(LangMgr.GetFont(), FontStyle.Bold);

                ////Text format for Ledgers
                //LedgerView = new SourceGrid.Cells.Views.Cell();
                //LedgerView.Font = new Font(LangMgr.GetFont(), FontStyle.Italic);
                //LedgerView.ForeColor = Color.Blue;

                ////Text format for SubGroup
                //SubGroupView = new SourceGrid.Cells.Views.Cell();
                //SubGroupView.Font = new Font(LangMgr.GetFont(), FontStyle.Regular);


                //Add a double click handler. When user dblclicks the cell, will use this function to send the parent form the Currency code
                dblClick = new SourceGrid.Cells.Controllers.CustomEvents();
                dblClick.DoubleClick += new EventHandler(grdTrial_DoubleClick);

                //Let the whole row to be selected
                grdTrial.SelectionMode = SourceGrid.GridSelectionMode.Row;


                //Disable multiple selection
                grdTrial.Selection.EnableMultiSelection = false;
                if(m_TBS.ShowPreviousYear==true)
                    grdTrial.Redim(1, 8);
                else
                    grdTrial.Redim(1, 6);
                grdTrial.FixedRows = 1;
                //int rows = grdTrial.Rows.Count;
                MakeHeader();
                #endregion 
            }

            TrialRowsCount = 1;
            AccountGroup AccountGroup = new AccountGroup();// Intializing the object of AccountGroup [Dynamic memory allocation of an object]
            Transaction Transaction = new Transaction();
            double DebitSum, CreditSum;
            DebitSum = CreditSum = 0;
            double DebitSumPY, CreditSumPY;
            DebitSumPY = CreditSumPY = 0;

            ProgressForm.UpdateProgress(40, "Initializing report...");

            #region Show Only Ledgers
            if(m_TBS.LedgerOnly==true)
            {
                double ProductValue1 = 0;

                int closingQuantity1 = 0;
                foreach (DataRow drProduct in dtGetAllProduct.Rows)
                {
                    DataTable dtOpeningStockStatusInfo1 = StockStatusBook.GetOpeningStockStatusBook(null, Convert.ToInt32(drProduct["ProductID"].ToString()), " ", m_TBS.ToDate, true, StockStatusType.OpeningStock, AccClassIDsXMLString);
                    if (dtOpeningStockStatusInfo1.Rows.Count > 0)
                    {
                        DataRow dropen = dtOpeningStockStatusInfo1.Rows[0];
                        closingQuantity1 = Convert.ToInt32(dropen["Quantity"].ToString());
                        double ProdPrice = Product.GetProductPurchasePrice(Convert.ToInt32(drProduct["ProductID"].ToString()), Global.ParentAccClassID);
                        ProductValue1 += ProdPrice * closingQuantity1;
                    }
                }

                if (ProductValue1 > 0)
                {
                    WriteDetailsForLedgerOnly("", 0, "Opening Stock", ProductValue1, 0, IsCrystalReport);
                    DValue += ProductValue1;
                }
                    //#region BLOCK FOR SUMMARY TRIAL BALANCE
                    DataTable dtLedgerOnly = Ledger.GetAllLedgerValues();
                    int Sno = 1;
                    foreach (DataRow dr in dtLedgerOnly.Rows)
                    {
                        double m_dbal = 0;
                        double m_cbal = 0;
                        double m_dbalPY = 0;
                        double m_cbalPY = 0;
                        //Calling thoes method which need only date range parameter
                        // Transaction.GetGroupBalance(m_TBS.FromDate, m_TBS.ToDate, Convert.ToInt32(dr["GroupID"]), false, ref m_dbal, ref m_cbal, m_TBS.AccClassID, m_TBS.ProjectID);

                        #region BLOCK FOR DATE RANGE SELECTION
                        //Block for DateTime range selection
                        if (m_TBS.HasDateRange == true)//When datetime is selected
                        {
                            Transaction.GetLedgerBalance(null, m_TBS.ToDate, Convert.ToInt32(dr["LedgerID"].ToString()), ref m_dbal, ref m_cbal, m_TBS.AccClassID, m_TBS.ProjectID);
                           
                            if(m_TBS.ShowPreviousYear==true)
                            {
                                Transaction.GetLedgerBalancePY(null, Global.PYToDate, Convert.ToInt32(dr["LedgerID"].ToString()), ref m_dbalPY, ref m_cbalPY, m_TBS.AccClassID, m_TBS.ProjectID);
                            }
                        }

                        else//Otherwise
                        {
                            Transaction.GetLedgerBalance(null, null, Convert.ToInt32(dr["LedgerID"].ToString()), ref m_dbal, ref m_cbal, m_TBS.AccClassID, m_TBS.ProjectID);
                            if (m_TBS.ShowPreviousYear == true)
                            {
                                Transaction.GetLedgerBalancePY(null, null, Convert.ToInt32(dr["LedgerID"].ToString()), ref m_dbalPY, ref m_cbalPY, m_TBS.AccClassID, m_TBS.ProjectID);
                            }
                        }
                        #endregion
                        if (m_TBS.ShowZeroBalance == false && m_dbal == 0 && m_cbal == 0) //In case of zero balance
                        {
                            //Do nothing
                        }
                        else
                        {
                            //string EngName = AccountGroup.GetEngName(Convert.ToInt32(dr["GroupID"]).ToString());  //calling this funtion for getting EngName from tblGroup corresponding to GroupID having corresponding GroupID
                            //double Balance = (m_dbal - m_cbal);//For Asset[Debit Balance - Credit Balance]
                            ////WriteAssets(Sno, EngName, Convert.ToInt32(dr["GroupID"]), Balance.ToString(), "GROUP");
                            //WriteTrial(Sno, EngName, Convert.ToInt32(dr["GroupID"]), m_dbal.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), m_cbal.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), "GROUP", IsCrystalReport);
                            //Sno++;
                            double Balance = (m_dbal - m_cbal);//For Asset[Debit Balance - Credit Balance]
                            DebitSum += m_dbal;
                            CreditSum += m_cbal;

                            double BalancePY = (m_dbalPY - m_cbalPY);//For Asset[Debit Balance - Credit Balance]
                            DebitSumPY += m_dbalPY;
                            CreditSumPY += m_cbalPY;
                            //If details is selected, show details i.e. ledgers present inside
                           // if (m_TBS.Details == true)
                            if (Balance > 0)
                            {
                                if(m_TBS.ShowPreviousYear==true)
                                {
                                    WriteDetailsForLedgerOnlyPY(dr["LedgerCode"].ToString(),Convert.ToInt32(dr["LedgerID"]), dr["EngName"].ToString(), Balance, 0, IsCrystalReport, BalancePY, 0);
                                    double checkBalance = 0;
                                    double checkBalancePY = 0;
                                    checkBalance = (m_dbal - m_cbal);
                                    checkBalancePY = (m_dbalPY - m_cbalPY);
                                    DValue += checkBalance;
                                    DValuePY += checkBalancePY;
                                }
                                else
                                {
                                WriteDetailsForLedgerOnly(dr["LedgerCode"].ToString(), Convert.ToInt32(dr["LedgerID"]), dr["EngName"].ToString(), Balance, 0, IsCrystalReport);
                                double checkBalance = 0;
                                checkBalance = (m_dbal - m_cbal);
                                DValue += checkBalance;
                                }
                            }
                            else
                            {
                                if (m_TBS.ShowPreviousYear == true)
                                {
                                    WriteDetailsForLedgerOnlyPY(dr["LedgerCode"].ToString(),Convert.ToInt32(dr["LedgerID"]), dr["EngName"].ToString(), Balance, 0, IsCrystalReport, 0, (BalancePY * -1));
                                    double checkBalance = 0;
                                    double checkBalancePY = 0;
                                    checkBalance = (m_cbal - m_dbal);
                                    checkBalancePY = (m_cbalPY - m_dbalPY);
                                    Cvalue += checkBalance;
                                    CvaluePY += checkBalancePY;
                                }
                                else
                                {
                                    WriteDetailsForLedgerOnly(dr["LedgerCode"].ToString(),Convert.ToInt32(dr["LedgerID"]), dr["EngName"].ToString(), 0, (Balance * -1), IsCrystalReport);
                                    double checkBalance = 0;
                                    checkBalance = (m_cbal - m_dbal);
                                    Cvalue += checkBalance;
                                }
                            }

                        }//End of zero balance check

                    }   
                  
            }
            #endregion

            #region BLOCK FOR ONLY PRIMARY GROUPS
            if (m_TBS.OnlyPrimaryGroups == true)
            {
                //For Opening Quantity
                double ProductValue1 = 0;

                int closingQuantity1 = 0;
                foreach (DataRow drProduct in dtGetAllProduct.Rows)
                {
                    DataTable dtOpeningStockStatusInfo1 = StockStatusBook.GetOpeningStockStatusBook(null, Convert.ToInt32(drProduct["ProductID"].ToString()), " ", m_TBS.ToDate, true, StockStatusType.OpeningStock, AccClassIDsXMLString);
                    if (dtOpeningStockStatusInfo1.Rows.Count > 0)
                    {
                        DataRow dropen = dtOpeningStockStatusInfo1.Rows[0];
                        closingQuantity1 = Convert.ToInt32(dropen["Quantity"].ToString());
                        double ProdPrice = Product.GetProductPurchasePrice(Convert.ToInt32(drProduct["ProductID"].ToString()), Global.ParentAccClassID);
                        ProductValue1 += ProdPrice * closingQuantity1;
                    }
                }

                if (ProductValue1 > 0)
                {
                    WriteTrial("", "Opening Stock", 0, ProductValue1.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), 0.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), "GROUP", IsCrystalReport);
                    DebitSum += ProductValue1;
                }
                DataTable dt = AccountGroup.GetGroupTable(m_TBS.GroupID);//
                int Sno = 1;
                foreach (DataRow dr in dt.Rows)
                {
                    double m_dbal = 0;
                    double m_cbal = 0;
                    double m_dbalPY = 0;
                    double m_cbalPY = 0;
                    //By default date range is selected,so just call the method which need the daterange parameter
                    if (m_TBS.HasDateRange)
                    {
                        Transaction.GetGroupBalance(null, m_TBS.ToDate, Convert.ToInt32(dr["GroupID"]), true, ref m_dbal, ref m_cbal, m_TBS.AccClassID, m_TBS.ProjectID);
                        if (m_TBS.ShowPreviousYear == true)
                        {
                            Transaction.GetGroupBalancePY(null,Global.PYToDate, Convert.ToInt32(dr["GroupID"]), true, ref m_dbalPY, ref m_cbalPY, m_TBS.AccClassID, m_TBS.ProjectID);
                        }
                    }
                    else
                    {
                        Transaction.GetGroupBalance(null, null, Convert.ToInt32(dr["GroupID"]), true, ref m_dbal, ref m_cbal, m_TBS.AccClassID, m_TBS.ProjectID);
                        if (m_TBS.ShowPreviousYear == true)
                        {
                            Transaction.GetGroupBalancePY(null, null, Convert.ToInt32(dr["GroupID"]), true, ref m_dbalPY, ref m_cbalPY, m_TBS.AccClassID, m_TBS.ProjectID);
                        }
                    }
 
                    if (m_TBS.ShowZeroBalance == false && m_dbal == 0 && m_cbal == 0) //In case of zero balance
                    {
                        //Do nothing
                    }
                    else
                    {
                        string EngName = AccountGroup.GetEngName(Convert.ToInt32(dr["GroupID"]).ToString());  //calling this funtion for getting EngName from tblGroup corresponding to GroupID having corresponding GroupID
                        double Balance = (m_dbal - m_cbal);//For Asset[Debit Balance - Credit Balance]
                        //WriteAssets(Sno, EngName, Convert.ToInt32(dr["GroupID"]), Balance.ToString(), "GROUP");
                        if (m_TBS.ShowPreviousYear == true)
                        {
                            WriteTrialPY(dr["LedgerCode"].ToString(), EngName, Convert.ToInt32(dr["GroupID"]), m_dbal.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), m_cbal.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), "GROUP", IsCrystalReport, m_dbalPY.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), m_cbalPY.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));
                            DebitSumPY += m_dbalPY;
                            CreditSumPY += m_cbalPY;
                        }
                        else
                        {
                            WriteTrial(dr["LedgerCode"].ToString(), EngName, Convert.ToInt32(dr["GroupID"]), m_dbal.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), m_cbal.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), "GROUP", IsCrystalReport);
                        }
                        DebitSum += m_dbal;
                        CreditSum += m_cbal;

                        //If details is selected, show details i.e. ledgers present inside
                        if (m_TBS.Details == true)
                        {
                            if (m_TBS.ShowPreviousYear == false)
                            {
                                //WriteDetails(Convert.ToInt32(dr["GroupID"]), AccountType.Assets);
                                WriteDetails(Convert.ToInt32(dr["GroupID"]), IsCrystalReport);
                            }
                            else
                            {
                                WriteDetailsPY(Convert.ToInt32(dr["GroupID"]), IsCrystalReport);
                            }
                        }

                        //If Second level group is selected, show them

                        if (m_TBS.ShowSecondLevelGroupDtl == true)
                        {
                            if (m_TBS.ShowPreviousYear == false)
                                WriteSecondLevel(Convert.ToInt32(dr["GroupID"]), IsCrystalReport);
                            else
                                WriteSecondLevelPY(Convert.ToInt32(dr["GroupID"]), IsCrystalReport);
                        }
                           
                        Sno++;
                    }//End of zero balance check

                }//End for loop
                if (m_TBS.ShowPreviousYear == true)
                {
                    WriteLedgerPY(m_TBS.GroupID, IsCrystalReport);
                }
                else
                {
                    WriteLedger(m_TBS.GroupID, IsCrystalReport);
                }

            }
            #endregion

            #region BLOCK FOR ALL GROUPS
            else if (m_TBS.AllGroups == true)
            {
                //First gathering the information of all Account Groups from Acc.tblGroup
                try
                {
                    //For Opening Quantity
                    double ProductValue1 = 0;

                    int closingQuantity1 = 0;
                    foreach (DataRow drProduct in dtGetAllProduct.Rows)
                    {
                        DataTable dtOpeningStockStatusInfo1 = StockStatusBook.GetOpeningStockStatusBook(null, Convert.ToInt32(drProduct["ProductID"].ToString()), " ", m_TBS.ToDate, true, StockStatusType.OpeningStock, AccClassIDsXMLString);
                        if (dtOpeningStockStatusInfo1.Rows.Count > 0)
                        {
                            DataRow dropen = dtOpeningStockStatusInfo1.Rows[0];
                            closingQuantity1 = Convert.ToInt32(dropen["Quantity"].ToString());
                            double ProdPrice = Product.GetProductPurchasePrice(Convert.ToInt32(drProduct["ProductID"].ToString()), Global.ParentAccClassID);
                            ProductValue1 += ProdPrice * closingQuantity1;
                        }
                    }

                    if (ProductValue1 > 0)
                    {
                        WriteTrial("", "Opening Stock", 0, ProductValue1.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), 0.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), "GROUP", IsCrystalReport);
                        DebitSum += ProductValue1;
                    }
                    //#region BLOCK FOR SUMMARY TRIAL BALANCE
                    DataTable dt = AccountGroup.GetGroupTable(-1);//Collecting the all groups form Acc.tblGroup
                   
                    int Sno = 1;
                    foreach (DataRow dr in dt.Rows)
                    {
                        double m_dbal = 0;
                        double m_cbal = 0;
                        double m_dbalPY = 0;
                        double m_cbalPY = 0;
                        //Calling thoes method which need only date range parameter
                        // Transaction.GetGroupBalance(m_TBS.FromDate, m_TBS.ToDate, Convert.ToInt32(dr["GroupID"]), false, ref m_dbal, ref m_cbal, m_TBS.AccClassID, m_TBS.ProjectID);

                        //Also Check For Available Child Of The Respective Group If no child then only proceed orelse terminate
                        DataTable CheckForItsChild = AccountGroup.GetChildGroupsFromParentID(dr["GroupID"].ToString());

                        if (CheckForItsChild.Rows.Count<1)
                        {
                        #region BLOCK FOR DATE RANGE SELECTION
                        //Block for DateTime range selection
                            if (m_TBS.HasDateRange == true)//When datetime is selected
                            {
                                Transaction.GetGroupBalance(null, m_TBS.ToDate, Convert.ToInt32(dr["GroupID"]), false, ref m_dbal, ref m_cbal, m_TBS.AccClassID, m_TBS.ProjectID);
                                if(m_TBS.ShowPreviousYear==true)
                                {
                                    Transaction.GetGroupBalancePY(null, Global.PYToDate, Convert.ToInt32(dr["GroupID"]), false, ref m_dbalPY, ref m_cbalPY, m_TBS.AccClassID, m_TBS.ProjectID);
                                }
                            }
                            else//Otherwise
                            {
                                //Transaction.GetGroupBalance(Convert.ToInt32(dr["GroupID"]), true, ref m_dbal, ref m_cbal, m_TBS.AccClassID, m_TBS.ProjectID);
                                Transaction.GetGroupBalance(null, null, Convert.ToInt32(dr["GroupID"]), false, ref m_dbal, ref m_cbal, m_TBS.AccClassID, m_TBS.ProjectID);
                                if (m_TBS.ShowPreviousYear == true)
                                {
                                    Transaction.GetGroupBalancePY(null, null, Convert.ToInt32(dr["GroupID"]), false, ref m_dbalPY, ref m_cbalPY, m_TBS.AccClassID, m_TBS.ProjectID);
                                }
                            }
                        #endregion






                        if (m_TBS.ShowZeroBalance == false && m_dbal == 0 && m_cbal == 0) //In case of zero balance
                        {
                            //Do nothing
                        }
                        else
                        {
                            string EngName = AccountGroup.GetEngName(Convert.ToInt32(dr["GroupID"]).ToString());  //calling this funtion for getting EngName from tblGroup corresponding to GroupID having corresponding GroupID
                            double Balance = (m_dbal - m_cbal);//For Asset[Debit Balance - Credit Balance]
                            double BalancePY = (m_dbalPY - m_cbalPY);//For Asset[Debit Balance - Credit Balance]
                            if (m_TBS.ShowPreviousYear == true)
                            {
                                WriteTrialPY(dr["LedgerCode"].ToString(), EngName, Convert.ToInt32(dr["GroupID"]), m_dbal.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), m_cbal.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), "GROUP", IsCrystalReport, m_dbalPY.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), m_cbalPY.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));
                                DebitSumPY += m_dbalPY;
                                CreditSumPY += m_cbalPY;
                            }
                            else
                            {
                                //WriteAssets(Sno, EngName, Convert.ToInt32(dr["GroupID"]), Balance.ToString(), "GROUP");
                                WriteTrial(dr["LedgerCode"].ToString(), EngName, Convert.ToInt32(dr["GroupID"]), m_dbal.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), m_cbal.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), "GROUP", IsCrystalReport);
                            }
                            Sno++;
                            DebitSum += m_dbal;
                            CreditSum += m_cbal;


                            //If details is selected, show details i.e. ledgers present inside
                            if (m_TBS.Details == true)
                            {
                                if (m_TBS.ShowPreviousYear == true)
                                {
                                    WriteDetailsPY(Convert.ToInt32(dr["GroupID"]), IsCrystalReport);
                                }
                                else
                                {
                                    WriteDetails(Convert.ToInt32(dr["GroupID"]), IsCrystalReport);
                                }
                            }

                        }//End of zero balance check

                        }
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }



            }
            #endregion


            ProgressForm.UpdateProgress(80, "Calculating difference in opening balance...");

            #region BLOCK FOR CALCULATING DIFFERENCE IN OPENING BALANCE
            double closingQuantity2 = 0;
            double productvalue1 = 0;
            foreach (DataRow drProduct in dtGetAllProduct.Rows)//For Removing the Opening Different
            {
                DataTable dtOpeningStockStatusInfo1 = StockStatusBook.GetOpeningStockStatusBook(null, Convert.ToInt32(drProduct["ProductID"].ToString()), " ", m_TBS.ToDate, true, StockStatusType.OpeningStock, AccClassIDsXMLString);
                if (dtOpeningStockStatusInfo1.Rows.Count>0)
                {
                DataRow dropen = dtOpeningStockStatusInfo1.Rows[0];
                closingQuantity2 = Convert.ToInt32(dropen["Quantity"].ToString());
                double ProdPrice = Product.GetProductPurchasePrice(Convert.ToInt32(drProduct["ProductID"].ToString()), Global.ParentAccClassID);
                productvalue1 += ProdPrice * closingQuantity2;
                }
            }
            double TotalDrOpBal, TotalCrOpBal;
            TotalDrOpBal = TotalCrOpBal = 0;
            GetOpeningBalanceSummary(GetRootAccClassID(), ref TotalDrOpBal, ref TotalCrOpBal);
            //Global.Msg("TotalDrOpBal" + TotalDrOpBal.ToString());
            TotalDrOpBal += productvalue1;
            if ((TotalDrOpBal != TotalCrOpBal))
            {
                if (TotalDrOpBal > TotalCrOpBal)//Total Opening balance in Debit side is greater so balance by adding this amount in Credit Balance
                {
                    CreditSum += (TotalDrOpBal - TotalCrOpBal);

                    WriteTrial("0", "DIFFERENCE IN OPENING BALANCE:", 0, (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), (TotalDrOpBal - TotalCrOpBal).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), "GROUP", IsCrystalReport);

                }
                else if (TotalDrOpBal < TotalCrOpBal)//Total Opening balance in Debit side is greater so balance by adding this amount in Credit Balance
                {
                    DebitSum += (TotalCrOpBal - TotalDrOpBal);

                    WriteTrial("0", "DIFFERENCE IN OPENING BALANCE:", 0, (TotalCrOpBal - TotalDrOpBal).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), "GROUP", IsCrystalReport);
                }
            }
            #endregion

            #region BLOCK FOR TOTAL AMOUNT CALCULATION FOR TRIAL BALANCE
            if (m_TBS.LedgerOnly == true)
            {
                if(m_TBS.ShowPreviousYear==false)
                    WriteTrial(" ", "TRIAL TOTAL", 0, DValue.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), Cvalue.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), "GROUP", IsCrystalReport);
                else
                    WriteTrialPY(" ", "TRIAL TOTAL", 0, DValue.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), Cvalue.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), "GROUP", IsCrystalReport, DValuePY.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), CvaluePY.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));
            }
            else
            {
                if (m_TBS.ShowPreviousYear == false)
                    WriteTrial(" ", "TRIAL TOTAL", 0, DebitSum.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), CreditSum.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), "GROUP", IsCrystalReport);
                else
                    WriteTrialPY(" ", "TRIAL TOTAL", 0, DebitSum.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), CreditSum.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), "GROUP", IsCrystalReport, DebitSumPY.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), CreditSumPY.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));
            }
            #endregion

            ProgressForm.UpdateProgress(100, "Preparing report for display...");

            // Close the dialog if it hasn't been already

            if (ProgressForm.InvokeRequired)
                ProgressForm.BeginInvoke(new Action(() => ProgressForm.Close()));
        }
     
        private void frmTrial_Load(object sender, EventArgs e)
        {
            Product allproduct = new Product();
             dtGetAllProduct = allproduct.getProductByGroupID();
             AccClassIDsXMLString = ReadAllAccClassID();
             ProjectIDsXMLString = ReadAllProjectID();

            if (Global.isOpeningTrial == false)
            {
                DisplayTrialBlance(false);
            }
            else
            {
                DisplayOpeningTrial(false);
            }
        }

        private void DisplayBannar()
        {

            if (m_TBS.FromDate != null && m_TBS.ToDate != null)
            {
                lblAsonDate.Text = "From: " + Date.ToSystem(Convert.ToDateTime(m_TBS.FromDate)) + " To: " + Date.ToSystem(Convert.ToDateTime(m_TBS.ToDate));
            }
            if (m_TBS.ToDate != null)
            {
                lblAsonDate.Text = "As on Date: " + Date.ToSystem(Convert.ToDateTime(m_TBS.ToDate));
            }
            if (m_TBS.FromDate != null)
            {
                //This is actually not applicable
                lblAsonDate.Text = "From: " + Date.ToSystem(Convert.ToDateTime(m_TBS.FromDate));
            }
            //else//both are null
            //{
            //    pdvReport_Date.Value = "As of now";

            //}
            if (m_TBS.FromDate == null && m_TBS.ToDate == null)
            {
                lblAsonDate.Text = "As of now";
            }

            ////If it has a date range
            //if(m_TBS.ToDate!=null)
            //{
            //    lblAsonDate.Text = "As on Date: " + Date.ToSystem((DateTime)m_TBS.ToDate);
            //}
            //else//if date range is not selected then siimply pass the current date time
            //{
            //    lblAsonDate.Text = "As on Date: " + Date.ToSystem(DateTime.Today);
            //}

            CompanyDetails m_CmpDtl = CompanyInfo.GetInfo();
            if(m_CmpDtl.FYFrom!=null)
               // lblAllSettings.Text = "Fiscal Year: " + Date.ToSystem(Convert.ToDateTime(m_CmpDtl.FiscalYear));
                lblAllSettings.Text = "Fiscal Year: " + m_CmpDtl.FiscalYear;
            
            DataTable dtProjectInfo = Project.GetProjectByID(Convert.ToInt32(m_TBS.ProjectID), LangMgr.DefaultLanguage);
            if (m_TBS.ProjectID != 0)
            {
                DataRow drProjectInfo = dtProjectInfo.Rows[0];
                lblProjectName.Text = " Project: " + drProjectInfo["Name"].ToString();
            }
            else
            {
                lblProjectName.Text = " Project: " + "All";
            }
        }

      //Function for Double click event
        private void grdTrial_DoubleClick(object sender, EventArgs e)
        {
            try
            {               
                //Get the Selected Row           
                int CurRow = grdTrial.Selection.GetSelectionRegion().GetRowsIndex()[0];
                SourceGrid.CellContext cellType = new SourceGrid.CellContext(grdTrial,new SourceGrid.Position(CurRow,5));
                SourceGrid.CellContext cellTypeID = new SourceGrid.CellContext(grdTrial, new SourceGrid.Position(CurRow, 4));
                string Type = (cellType.Value).ToString();
                //read id                                     
                string ID = (cellTypeID.Value).ToString();//
                //if (ID == "0")//IF ID is blank means there must be total or Opening balance case             
                //    return;             
                if (Type == "GROUP")
                {
                    int CurRow1 = grdTrial.Selection.GetSelectionRegion().GetRowsIndex()[0];
                    SourceGrid.CellContext cellID = new SourceGrid.CellContext(grdTrial, new SourceGrid.Position(CurRow1, 4));
                    GroupBalanceSettings m_GBS = new GroupBalanceSettings();
                    m_GBS.HasDateRange = m_TBS.HasDateRange;
                    m_GBS.ShowZeroBalance = m_TBS.ShowZeroBalance;
                    m_GBS.FromDate = m_TBS.FromDate;
                    m_GBS.ToDate = m_TBS.ToDate;
                    m_GBS.AllGroups = m_TBS.AllGroups;
                    m_GBS.AccClassID = m_TBS.AccClassID;
                    m_GBS.OnlyPrimaryGroups = m_TBS.OnlyPrimaryGroups;
                    m_GBS.GroupID = Convert.ToInt32(cellID.Value);//Store the GroupID value on object which achieve while double clicking the corresponding row of cell
                    frmGroupBalance m_GrpBal = new frmGroupBalance(m_GBS);                                                       
                    m_GrpBal.ShowDialog();                                      
                }
                else if(Type=="LEDGER")
                {   
                    int CurRow2 = grdTrial.Selection.GetSelectionRegion().GetRowsIndex()[0];
                    SourceGrid.CellContext cellID = new SourceGrid.CellContext(grdTrial, new SourceGrid.Position(CurRow2, 4));
                    string LedgerID = (cellID.Value).ToString();
                    TransactSettings m_TS = new TransactSettings();
                    m_TS.HasDateRange = m_TBS.HasDateRange;
                    m_TS.ShowZeroBalance = m_TBS.ShowZeroBalance;
                    m_TS.FromDate = m_TBS.FromDate;
                    m_TS.ToDate = m_TBS.ToDate;
                    m_TS.AccClassID = m_TBS.AccClassID;
                    m_TS.LedgerID = Convert.ToInt32(LedgerID);                 
                    frmTransaction m_Transact = new frmTransaction(m_TS);                                              
                    m_Transact.ShowDialog();                  
                }
            
            }
            catch (Exception ex)
            {               
                Global.Msg(ex.Message);
            }
        }
   
        private void btnPrintPreview_Click(object sender, EventArgs e)
        {
            PrintPreviewCR(PrintType.CrystalReport);

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
            if (m_TBS.ShowPreviousYear == true)
            {
                dsTrialPY.Clear();
            }
            else
            {
                dsTrial.Clear();//It clear the previous records of dataset on crystal report...when this button is pressed
            }
            //otherwise it populate the records again and again
            double DebitSum, CreditSum;
            DebitSum = CreditSum = 0;
            double DebitSumPY, CreditSumPY;
            DebitSumPY = CreditSumPY = 0;
            rptTrialBalance rpt = new rptTrialBalance();
            rptTrialBalancePY rpt1 = new rptTrialBalancePY();
            //Fill the logo on the report
            if (m_TBS.ShowPreviousYear == false)
            {
               
                Misc.WriteLogo(dsTrial, "tblImage");
                //Set DataSource to be dsTrial dataset on the report
                rpt.SetDataSource(dsTrial);
            }
            else
            {
              
                Misc.WriteLogo(dsTrialPY, "tblImage");
                //Set DataSource to be dsTrial dataset on the report
                rpt1.SetDataSource(dsTrialPY);
            }


            DisplayTrialBlance(true);


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
            CrystalDecisions.Shared.ParameterDiscreteValue pdvDebit_TotalPY = new CrystalDecisions.Shared.ParameterDiscreteValue();
            CrystalDecisions.Shared.ParameterDiscreteValue pdvCredit_TotalPY = new CrystalDecisions.Shared.ParameterDiscreteValue();

            CrystalDecisions.Shared.ParameterDiscreteValue pdvFont = new CrystalDecisions.Shared.ParameterDiscreteValue();

            //Update the progressbar
            ProgressForm.UpdateProgress(50, "Initializing Report Viewer...");
            if (m_TBS.ShowPreviousYear == false)
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


                //Company Address
                pdvCompany_Address.Value = m_CompanyDetails.Address1 + ((m_CompanyDetails.Address1.Trim().Length>0)&&(m_CompanyDetails.City.Trim().Length>0)? ", ":"") +  m_CompanyDetails.City; //Display comma in the middle only if both are available

                pvCollection.Clear();
                pvCollection.Add(pdvCompany_Address);
                rpt.DataDefinition.ParameterFields["Company_Address"].ApplyCurrentValues(pvCollection);


                //PAN
                pdvCompany_PAN.Value = m_CompanyDetails.PAN;
                pvCollection.Clear();
                pvCollection.Add(pdvCompany_PAN);
                rpt.DataDefinition.ParameterFields["Company_PAN"].ApplyCurrentValues(pvCollection);


                //Phone No.
                pdvCompany_Phone.Value = "Phone No.: " + m_CompanyDetails.Telephone;
                pvCollection.Clear();
                pvCollection.Add(pdvCompany_Phone);
                rpt.DataDefinition.ParameterFields["Company_Phone"].ApplyCurrentValues(pvCollection);

                pdvCompany_Slogan.Value = m_CompanyDetails.Website;
                pvCollection.Clear();
                pvCollection.Add(pdvCompany_Slogan);
                rpt.DataDefinition.ParameterFields["Company_Slogan"].ApplyCurrentValues(pvCollection);


                pdvDebit_Total.Value = m_DebitTotal;
                pvCollection.Clear();
                pvCollection.Add(pdvDebit_Total);
                rpt.DataDefinition.ParameterFields["Debit_Total"].ApplyCurrentValues(pvCollection);

                pdvCredit_Total.Value = m_CreditTotal;
                pvCollection.Clear();
                pvCollection.Add(pdvCredit_Total);
                rpt.DataDefinition.ParameterFields["Credit_Total"].ApplyCurrentValues(pvCollection);



                pdvReport_Head.Value = "Trial Balance";
                pvCollection.Clear();
                pvCollection.Add(pdvReport_Head);
                rpt.DataDefinition.ParameterFields["Report_Head"].ApplyCurrentValues(pvCollection);
                if (m_CompanyDetails.FYFrom != null)
                    //pdvFiscal_Year.Value = "Fiscal Year:" + Date.ToSystem(Convert.ToDateTime(m_CompanyDetails.FYFrom));
                    pdvFiscal_Year.Value = "Fiscal Year:" + m_CompanyDetails.FiscalYear;

                pvCollection.Clear();
                pvCollection.Add(pdvFiscal_Year);
                rpt.DataDefinition.ParameterFields["Fiscal_Year"].ApplyCurrentValues(pvCollection);

                //Update the progressbar
                ProgressForm.UpdateProgress(80, "Calculating Parameters...");

                if (m_TBS.FromDate != null && m_TBS.ToDate != null)
                {
                    pdvReport_Date.Value = "From: " + Date.ToSystem(Convert.ToDateTime(m_TBS.FromDate)) + " To: " + Date.ToSystem(Convert.ToDateTime(m_TBS.ToDate));
                }
                if (m_TBS.ToDate != null)
                {
                    pdvReport_Date.Value = "As on Date: " + Date.ToSystem(Convert.ToDateTime(m_TBS.ToDate));
                }
                if (m_TBS.FromDate != null)
                {
                    //This is actually not applicable
                    pdvReport_Date.Value = "From: " + Date.ToSystem(Convert.ToDateTime(m_TBS.FromDate));
                }
                //else//both are null
                //{
                //    pdvReport_Date.Value = "As of now";

                //}
                if (m_TBS.FromDate == null && m_TBS.ToDate == null)
                {
                    pdvReport_Date.Value = "As of now";
                }


                pvCollection.Clear();

                pvCollection.Add(pdvReport_Date);
                rpt.DataDefinition.ParameterFields["Report_Date"].ApplyCurrentValues(pvCollection);


            }
            else
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

                pdvDebit_Total.Value = m_DebitTotal;
                pvCollection.Clear();
                pvCollection.Add(pdvDebit_Total);
                rpt1.DataDefinition.ParameterFields["Total_DebitAmount"].ApplyCurrentValues(pvCollection);

                pdvCredit_Total.Value = m_CreditTotal;
                pvCollection.Clear();
                pvCollection.Add(pdvCredit_Total);
                rpt1.DataDefinition.ParameterFields["Total_CreditAmount"].ApplyCurrentValues(pvCollection);

                pdvDebit_TotalPY.Value = m_DebitTotalPY;
                pvCollection.Clear();
                pvCollection.Add(pdvDebit_TotalPY);
                rpt1.DataDefinition.ParameterFields["Total_DebitAmountPY"].ApplyCurrentValues(pvCollection);

                pdvCredit_TotalPY.Value = m_CreditTotalPY;
                pvCollection.Clear();
                pvCollection.Add(pdvCredit_TotalPY);
                rpt1.DataDefinition.ParameterFields["Total_CreditAmountPY"].ApplyCurrentValues(pvCollection);

                pdvReport_Head.Value = "Trial Balance";
                pvCollection.Clear();
                pvCollection.Add(pdvReport_Head);
                rpt1.DataDefinition.ParameterFields["Report_Head"].ApplyCurrentValues(pvCollection);
                if (m_CompanyDetails.FYFrom != null)
                   // pdvFiscal_Year.Value = "Fiscal Year:" + Date.ToSystem(Convert.ToDateTime(m_CompanyDetails.FYFrom));
                    pdvFiscal_Year.Value = "Fiscal Year:" + m_CompanyDetails.FiscalYear;

                pvCollection.Clear();
                pvCollection.Add(pdvFiscal_Year);
                rpt1.DataDefinition.ParameterFields["Fiscal_Year"].ApplyCurrentValues(pvCollection);

                //Update the progressbar
                ProgressForm.UpdateProgress(80, "Calculating Parameters...");

                if (m_TBS.FromDate != null && Global.PYToDate != null)
                {
                    pdvReport_Date.Value = "From: " + Date.ToSystem(Convert.ToDateTime(m_TBS.FromDate)) + " To: " + Date.ToSystem(Convert.ToDateTime(Global.PYToDate));
                }
                if (m_TBS.ToDate != null)
                {
                    pdvReport_Date.Value = "As on Date: " + Date.ToSystem(Convert.ToDateTime(Global.PYToDate));
                }
                if (m_TBS.FromDate != null)
                {
                    //This is actually not applicable
                    pdvReport_Date.Value = "From: " + Date.ToSystem(Convert.ToDateTime(m_TBS.FromDate));
                }
                else//both are null
                {
                    pdvReport_Date.Value = "As of now";

                }


                pvCollection.Clear();

                pvCollection.Add(pdvReport_Date);
                rpt1.DataDefinition.ParameterFields["Report_Date"].ApplyCurrentValues(pvCollection);
            }
                
            //dsTrialPY.Tables["tblGroup"].Rows.Add("1", "Test", "200", "300", "CrystalReport");


                CrystalDecisions.Shared.ExportOptions CrExportOptions = new CrystalDecisions.Shared.ExportOptions();
                DiskFileDestinationOptions CrDiskFileDestinationOptions = new DiskFileDestinationOptions();
                CrDiskFileDestinationOptions.DiskFileName = FileName;
                //rpt.Export(opt);
                //return;
                //Finally, show the report form
                frmReportViewer frm = new frmReportViewer();
                if (m_TBS.ShowPreviousYear == false)
                    frm.SetReportSource(rpt);
                else
                    frm.SetReportSource(rpt1);

                //Update the progressbar
                ProgressForm.UpdateProgress(100, "Showing Report...");

                // Close the dialog
                ProgressForm.CloseForm();
            
            switch (myPrintType)
            {
                case PrintType.DirectPrint: //Direct Printer
                    if (m_TBS.ShowPreviousYear == false)
                    {
                        rpt.PrintOptions.PrinterName = "";
                        rpt.PrintToPrinter(1, false, 0, 0);
                    }
                    else
                    {
                        rpt1.PrintOptions.PrinterName = "";
                        rpt1.PrintToPrinter(1, false, 0, 0);
                    }
                    return;
                case PrintType.Excel: //Excel
                    if (m_TBS.ShowPreviousYear == false)
                    {
                        ExcelFormatOptions CrFormatTypeOptions = new ExcelFormatOptions();
                        CrExportOptions = rpt.ExportOptions;
                        CrExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                        CrExportOptions.ExportFormatType = ExportFormatType.Excel;
                        CrExportOptions.DestinationOptions = CrDiskFileDestinationOptions;
                        CrExportOptions.FormatOptions = CrFormatTypeOptions;
                        rpt.Export();
                        rpt.Close();
                    }
                    else
                    {
                        ExcelFormatOptions CrFormatTypeOptions = new ExcelFormatOptions();
                        CrExportOptions = rpt.ExportOptions;
                        CrExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                        CrExportOptions.ExportFormatType = ExportFormatType.Excel;
                        CrExportOptions.DestinationOptions = CrDiskFileDestinationOptions;
                        CrExportOptions.FormatOptions = CrFormatTypeOptions;
                        rpt1.Export();
                        rpt1.Close();
                    }
                    return;
                case PrintType.PDF: //PDF
                    if (m_TBS.ShowPreviousYear == false)
                    {
                        PdfFormatOptions CrFormatTypeOptionsPdf = new PdfFormatOptions();
                        CrExportOptions = rpt.ExportOptions;
                        CrExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                        CrExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                        CrExportOptions.DestinationOptions = CrDiskFileDestinationOptions;
                        CrExportOptions.FormatOptions = CrFormatTypeOptionsPdf;
                        rpt.Export();
                        rpt.Close();
                    }
                    else
                    {
                        PdfFormatOptions CrFormatTypeOptionsPdf = new PdfFormatOptions();
                        CrExportOptions = rpt.ExportOptions;
                        CrExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                        CrExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                        CrExportOptions.DestinationOptions = CrDiskFileDestinationOptions;
                        CrExportOptions.FormatOptions = CrFormatTypeOptionsPdf;
                        rpt1.Export();
                        rpt1.Close();
                    }
                    return;
                case PrintType.Email: //Excel
                    if (m_TBS.ShowPreviousYear == false)
                    {
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
                       
                    }
                    else
                    {
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
                       
                    }
                    return;
                default: //Crystal Report
                    
                        frm.Show();
                        frm.WindowState = FormWindowState.Maximized;
                  
                    break;
            }

            this.Cursor = Cursors.Default;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            //Clear all rows
            grdTrial.Redim(0, 0);

            this.Cursor = Cursors.WaitCursor;
            //Load all over again
            frmTrial_Load(sender, e);

            this.Cursor = Cursors.Default;
            //Show complete notification
            //Global.Msg("Reload Complete!");

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

            Menu_Export.MenuItems.Add(mnuExcel);
            Menu_Export.MenuItems.Add(mnuPDF);
            Menu_Export.MenuItems.Add(mnuEmail);
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
                    PrintPreviewCR(PrintType.Excel);
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
                      PrintPreviewCR(PrintType.PDF);
                    break;
                case "mnuEmail":
                    //Code for excel export
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
                PrintPreviewCR(PrintType.Email);
                break;
               
            }
        }

        private void frmTrial_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            PrintPreviewCR(PrintType.DirectPrint);
        }

        //private void InvokePrint(object sender, RoutedEventArgs e)
        //{
        //    // Create the print dialog object and set options
        //    PrintDialog pDialog = new PrintDialog();
        //    pDialog.PageRangeSelection = PageRangeSelection.AllPages;
        //    pDialog.UserPageRangeEnabled = true;

        //    // Display the dialog. This returns true if the user presses the Print button.
        //    Nullable<Boolean> print = pDialog.ShowDialog();
        //    if (print == true)
        //    {
        //        XpsDocument xpsDocument = new XpsDocument("C:\\FixedDocumentSequence.xps", FileAccess.ReadWrite);
        //        FixedDocumentSequence fixedDocSeq = xpsDocument.GetFixedDocumentSequence();
        //        pDialog.PrintDocument(fixedDocSeq.DocumentPaginator, "Test print job");
        //    }
        //}

        private void WriteDetailsForLedgerOnly(string LedgerCode, int LedgerID,string LedgerName,double DebitSum,Double CreditSum, bool IsCrystalReport)
        {
            try
            {
                    Sno =Sno+ 1;
                    double DebBal = 0;
                    double CreBal = 0;
                    WriteTrial(LedgerCode, LedgerName, LedgerID, DebitSum.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), CreditSum.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), "LEDGER", IsCrystalReport);
                   // Sno++;     
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void WriteDetailsForLedgerOnlyPY(string LedgerCode,int LedgerID, string LedgerName, double DebitSum, Double CreditSum, bool IsCrystalReport, double DebitSumPY, Double CreditSumPY)
        {
            try
            {
                //Sno = Sno + 1;
                double DebBal = 0;
                double CreBal = 0;
                WriteTrialPY(LedgerCode, LedgerName, LedgerID, DebitSum.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), CreditSum.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), "LEDGER", IsCrystalReport, DebitSumPY.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), CreditSumPY.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));
                // Sno++;     
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
       
        private void DisplayOpeningTrial(bool IsCrystalReport)
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

            ProgressForm.UpdateProgress(20, "Initializing report...");

            if (!IsCrystalReport)//Need only for writting on grid...not for crystal reports
            {
                DisplayBannar();
                #region BLOCK FOR ORIENTATION PURPOSE

                //Text format for Total
                GroupView = new SourceGrid.Cells.Views.Cell();
                //GroupView.Background = new DevAge.Drawing.VisualElements.BackgroundLinearGradient(Color.RoyalBlue, Color.LightBlue, 0);
                //GroupView.ForeColor = Color.FromKnownColor(KnownColor.ActiveCaptionText);
                //GroupView.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleCenter;
                //GroupView.Border = DevAge.Drawing.RectangleBorder.NoBorder;
                GroupView.Font = new Font(LangMgr.GetFont(), FontStyle.Bold);

                //Text format for Ledgers
                LedgerView = new SourceGrid.Cells.Views.Cell();
                LedgerView.Font = new Font(LangMgr.GetFont(), FontStyle.Italic);
                LedgerView.ForeColor = Color.Blue;

                //Text format for SubGroup
                SubGroupView = new SourceGrid.Cells.Views.Cell();
                SubGroupView.Font = new Font(LangMgr.GetFont(), FontStyle.Regular);


                //Add a double click handler. When user dblclicks the cell, will use this function to send the parent form the Currency code
                dblClick = new SourceGrid.Cells.Controllers.CustomEvents();
                dblClick.DoubleClick += new EventHandler(grdTrial_DoubleClick);

                //Let the whole row to be selected
                grdTrial.SelectionMode = SourceGrid.GridSelectionMode.Row;


                //Disable multiple selection
                grdTrial.Selection.EnableMultiSelection = false;

                if (m_TBS.ShowPreviousYear == true)
                {
                    grdTrial.Redim(1, 8);
                }
                else
                {
                    grdTrial.Redim(1, 6);
                }
                grdTrial.FixedRows = 1;
                //int rows = grdTrial.Rows.Count;
                MakeHeader();
                #endregion
            }

            TrialRowsCount = 1;
            AccountGroup AccountGroup = new AccountGroup();// Intializing the object of AccountGroup [Dynamic memory allocation of an object]
            Transaction Transaction = new Transaction();
            double DebitSum, CreditSum;
            DebitSum = CreditSum = 0;
            double DebitSumPY, CreditSumPY;
            DebitSumPY = CreditSumPY = 0;


            ProgressForm.UpdateProgress(40, "Initializing report...");

            #region Show Only Ledgers
            if (m_TBS.LedgerOnly == true)
            {
                //For Opening Quantity
                double ProductValue1 = 0;
               
                int closingQuantity1 = 0;
                foreach (DataRow drProduct in dtGetAllProduct.Rows)
                {
                    DataTable dtOpeningStockStatusInfo1 = StockStatusBook.GetOpeningStockStatusBook(null, Convert.ToInt32(drProduct["ProductID"].ToString()), " ", m_TBS.ToDate, true, StockStatusType.OpeningStock, AccClassIDsXMLString);
                    if (dtOpeningStockStatusInfo1.Rows.Count > 0)
                    {
                        DataRow dropen = dtOpeningStockStatusInfo1.Rows[0];
                        closingQuantity1 = Convert.ToInt32(dropen["Quantity"].ToString());
                        double ProdPrice = Product.GetProductPurchasePrice(Convert.ToInt32(drProduct["ProductID"].ToString()), Global.ParentAccClassID);
                        ProductValue1 += ProdPrice * closingQuantity1;
                    }
                }

                if (ProductValue1 > 0)
                {
                    WriteDetailsForLedgerOnly("", 0, "Opening Stock", ProductValue1, 0, IsCrystalReport);
                    DValue += ProductValue1;
                }
                //#region BLOCK FOR SUMMARY TRIAL BALANCE
                DataTable dtLedgerOnly = Ledger.GetAllLedgerValues();
                int Sno = 1;
                foreach (DataRow dr in dtLedgerOnly.Rows)
                {
                    double m_dbal = 0;
                    double m_cbal = 0;
                    
                        double m_dbalPY = 0;
                        double m_cbalPY = 0;
                    
                    //Calling thoes method which need only date range parameter
                    // Transaction.GetGroupBalance(m_TBS.FromDate, m_TBS.ToDate, Convert.ToInt32(dr["GroupID"]), false, ref m_dbal, ref m_cbal, m_TBS.AccClassID, m_TBS.ProjectID);

                    #region BLOCK FOR DATE RANGE SELECTION
                    //Block for DateTime range selection
                    if (m_TBS.HasDateRange == true)//When datetime is selected
                    {
                        Transaction.GetOpeningLedgerBalance(null, m_TBS.ToDate, Convert.ToInt32(dr["LedgerID"].ToString()), ref m_dbal, ref m_cbal, m_TBS.AccClassID, m_TBS.ProjectID);
                       if(m_TBS.ShowPreviousYear==true)//For Showing Previous Year Data
                       {
                        Transaction.GetOpeningLedgerBalancePY(null, Global.PYToDate, Convert.ToInt32(dr["LedgerID"].ToString()), ref m_dbalPY, ref m_cbalPY, m_TBS.AccClassID, m_TBS.ProjectID);
                        }
                    }

                    else//Otherwise
                    {
                        Transaction.GetOpeningLedgerBalance(null, null, Convert.ToInt32(dr["LedgerID"].ToString()), ref m_dbal, ref m_cbal, m_TBS.AccClassID, m_TBS.ProjectID);
                        if (m_TBS.ShowPreviousYear == true)
                        {
                            Transaction.GetOpeningLedgerBalancePY(null, null, Convert.ToInt32(dr["LedgerID"].ToString()), ref m_dbalPY, ref m_cbalPY, m_TBS.AccClassID, m_TBS.ProjectID);
                        }
                    }
                    #endregion
                    if (m_TBS.ShowZeroBalance == false && m_dbal == 0 && m_cbal == 0) //In case of zero balance
                    {
                        //Do nothing
                    }
                    else
                    {
                        double Balance = (m_dbal - m_cbal);//For Asset[Debit Balance - Credit Balance]
                        DebitSum += m_dbal;
                        CreditSum += m_cbal;

                        //If details is selected, show details i.e. ledgers present inside
                        // if (m_TBS.Details == true)
                        if (Balance > 0)
                        {
                            if (m_TBS.ShowPreviousYear == true)
                            {
                                WriteDetailsForLedgerOnlyPY(dr["LedgerCode"].ToString(), Convert.ToInt32(dr["LedgerID"]), dr["EngName"].ToString(), m_dbal, 0, IsCrystalReport, m_dbalPY, 0);
                                double checkBalance = 0;
                                double checkBalancePY = 0;
                                checkBalance = (m_dbal - m_cbal);
                                checkBalancePY = (m_dbalPY - m_cbalPY);
                                DValue += checkBalance;
                                DValuePY += checkBalancePY;
                            }
                            else
                            {
                                WriteDetailsForLedgerOnly(dr["LedgerCode"].ToString(), Convert.ToInt32(dr["LedgerID"]), dr["EngName"].ToString(), m_dbal, 0, IsCrystalReport);
                                double checkBalance = 0;              
                                checkBalance = (m_dbal - m_cbal);                             
                                DValue += checkBalance;
                            }
                        }
                        else
                        {
                            if (m_TBS.ShowPreviousYear == true)
                            {
                                WriteDetailsForLedgerOnlyPY(dr["LedgerCode"].ToString(), Convert.ToInt32(dr["LedgerID"]), dr["EngName"].ToString(), 0, m_cbal, IsCrystalReport, 0, m_cbalPY);
                                double checkBalance = 0;
                                double checkBalancePY = 0;
                                checkBalance = (m_cbal - m_dbal);
                                checkBalancePY = (m_cbalPY - m_dbalPY);
                                Cvalue += checkBalance;
                                CvaluePY += checkBalancePY;
                            }
                            else
                            {
                                WriteDetailsForLedgerOnly(dr["LedgerCode"].ToString(),Convert.ToInt32(dr["LedgerID"]), dr["EngName"].ToString(), 0, m_cbal, IsCrystalReport);
                                double checkBalance = 0;
                                //double checkBalancePY = 0;
                                checkBalance = (m_cbal - m_dbal);
                               // checkBalancePY = (m_cbalPY - m_dbalPY);
                                Cvalue += checkBalance;
                               // CvaluePY += checkBalancePY;
                            }
                        }
                        if (m_TBS.ShowPreviousYear == true)
                        {
                            double BalancePY = (m_dbalPY - m_cbalPY);//For Asset[Debit Balance - Credit Balance]
                            DebitSumPY += m_dbalPY;
                            CreditSumPY += m_cbalPY;
                        }
                        else
                        {
                            //double Balance = (m_dbal - m_cbal);//For Asset[Debit Balance - Credit Balance]
                            //DebitSum += m_dbal;
                            //CreditSum += m_cbal;
                        }

                        //If details is selected, show details i.e. ledgers present inside
                        // if (m_TBS.Details == true)
                        //if (BalancePY > 0)
                        //{
                        //    WriteDetailsForLedgerOnlyPY(Convert.ToInt32(dr["LedgerID"]), dr["EngName"].ToString(), m_dbalPY, 0, IsCrystalReport);
                        //    double checkBalance = 0;
                        //    checkBalance = (m_dbalPY - m_cbalPY);
                        //    DValuePY += checkBalance;
                        //}
                        //else
                        //{
                        //    WriteDetailsForLedgerOnlyPY(Convert.ToInt32(dr["LedgerID"]), dr["EngName"].ToString(), 0, m_cbalPY, IsCrystalReport);
                        //    double checkBalance = 0;
                        //    checkBalance = (m_cbalPY - m_dbalPY);
                        //    CvaluePY += checkBalance;
                        //}

                    }//End of zero balance check

                }

            }

            #endregion

            #region BLOCK FOR ONLY PRIMARY GROUPS
            if (m_TBS.OnlyPrimaryGroups == true)
            {
                //For Opening Quantity
                double ProductValue1 = 0;

                int closingQuantity1 = 0;
                foreach (DataRow drProduct in dtGetAllProduct.Rows)
                {
                    DataTable dtOpeningStockStatusInfo1 = StockStatusBook.GetOpeningStockStatusBook(null, Convert.ToInt32(drProduct["ProductID"].ToString()), " ", m_TBS.ToDate, true, StockStatusType.OpeningStock, AccClassIDsXMLString);
                    if (dtOpeningStockStatusInfo1.Rows.Count > 0)
                    {
                        DataRow dropen = dtOpeningStockStatusInfo1.Rows[0];
                        closingQuantity1 = Convert.ToInt32(dropen["Quantity"].ToString());
                        double ProdPrice = Product.GetProductPurchasePrice(Convert.ToInt32(drProduct["ProductID"].ToString()), Global.ParentAccClassID);
                        ProductValue1 += ProdPrice * closingQuantity1;
                    }
                }

                if (ProductValue1 > 0)
                {
                    WriteTrial("", "Opening Stock", 0, ProductValue1.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), 0.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)),"GROUP", IsCrystalReport);
                    DebitSum += ProductValue1;
                }
                DataTable dt = AccountGroup.GetGroupTable(m_TBS.GroupID);//
                int Sno = 1;
                foreach (DataRow dr in dt.Rows)
                {
                    double m_dbal = 0;
                    double m_cbal = 0;
                    double m_dbalPY = 0;
                    double m_cbalPY = 0;
                    //By default date range is selected,so just call the method which need the daterange parameter
                    if (m_TBS.HasDateRange)
                    {
                        Transaction.GetOpeningGroupBalance(null, m_TBS.ToDate, Convert.ToInt32(dr["GroupID"]), true, ref m_dbal, ref m_cbal, m_TBS.AccClassID, m_TBS.ProjectID);
                        if (m_TBS.ShowPreviousYear == true)
                        {
                            Transaction.GetOpeningGroupBalancePY(null, Global.PYToDate, Convert.ToInt32(dr["GroupID"]), true, ref m_dbalPY, ref m_cbalPY, m_TBS.AccClassID, m_TBS.ProjectID);
                           
                        }
                    }
                    else
                    {
                        Transaction.GetOpeningGroupBalance(null, null, Convert.ToInt32(dr["GroupID"]), true, ref m_dbal, ref m_cbal, m_TBS.AccClassID, m_TBS.ProjectID);
                        if (m_TBS.ShowPreviousYear == true)
                        {
                            Transaction.GetOpeningGroupBalancePY(null, null, Convert.ToInt32(dr["GroupID"]), true, ref m_dbalPY, ref m_cbalPY, m_TBS.AccClassID, m_TBS.ProjectID);
                        }
                    }

                    if (m_TBS.ShowZeroBalance == false && m_dbal == 0 && m_cbal == 0) //In case of zero balance
                    {
                        //Do nothing
                    }
                    else
                    {
                        string EngName = AccountGroup.GetEngName(Convert.ToInt32(dr["GroupID"]).ToString());  //calling this funtion for getting EngName from tblGroup corresponding to GroupID having corresponding GroupID
                        double Balance = (m_dbal - m_cbal);//For Asset[Debit Balance - Credit Balance]
                        double BalancePY = (m_dbalPY - m_cbalPY);
                        //WriteAssets(Sno, EngName, Convert.ToInt32(dr["GroupID"]), Balance.ToString(), "GROUP");
                        if (m_TBS.ShowPreviousYear == true)
                        {
                            WriteTrialPY(dr["LedgerCode"].ToString(), EngName, Convert.ToInt32(dr["GroupID"]), m_dbal.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), m_cbal.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), "GROUP", IsCrystalReport, m_dbalPY.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), m_cbalPY.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));
                        }
                        else
                        {
                            WriteTrial(dr["LedgerCode"].ToString(), EngName, Convert.ToInt32(dr["GroupID"]), m_dbal.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), m_cbal.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), "GROUP", IsCrystalReport);
                        }
                       // WriteTrial(Sno, EngName, Convert.ToInt32(dr["GroupID"]), m_dbal.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), m_cbal.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), "GROUP", IsCrystalReport);
                        DebitSum += m_dbal;
                        CreditSum += m_cbal;
                        DebitSumPY += m_dbalPY;
                        CreditSumPY += m_cbalPY;
                        //If details is selected, show details i.e. ledgers present inside
                        if (m_TBS.Details == true)
                        {
                            //WriteDetails(Convert.ToInt32(dr["GroupID"]), AccountType.Assets);
                            if (m_TBS.ShowPreviousYear == true)
                            {
                                WriteDetailsPY(Convert.ToInt32(dr["GroupID"]), IsCrystalReport);
                            }
                            else
                            {
                                WriteDetails(Convert.ToInt32(dr["GroupID"]), IsCrystalReport);
                            }
                        }

                        //If Second level group is selected, show them

                        if (m_TBS.ShowSecondLevelGroupDtl == true)
                        {
                            
                                WriteSecondLevel(Convert.ToInt32(dr["GroupID"]), IsCrystalReport);
                            
                        }
                        Sno++;
                    }//End of zero balance check

                }//End for loop
                if (m_TBS.ShowPreviousYear == true)
                {
                    WriteLedgerPY(m_TBS.GroupID, IsCrystalReport);
                }
                else
                {
                    WriteLedger(m_TBS.GroupID, IsCrystalReport);
                }

            }
            #endregion

            #region BLOCK FOR ALL GROUPS
            else if (m_TBS.AllGroups == true)
            {
                //For Opening Quantity
                double ProductValue1 = 0;

                int closingQuantity1 = 0;
                foreach (DataRow drProduct in dtGetAllProduct.Rows)
                {
                    DataTable dtOpeningStockStatusInfo1 = StockStatusBook.GetOpeningStockStatusBook(null, Convert.ToInt32(drProduct["ProductID"].ToString()), " ", m_TBS.ToDate, true, StockStatusType.OpeningStock, AccClassIDsXMLString);
                    if (dtOpeningStockStatusInfo1.Rows.Count > 0)
                    {
                        DataRow dropen = dtOpeningStockStatusInfo1.Rows[0];
                        closingQuantity1 = Convert.ToInt32(dropen["Quantity"].ToString());
                        double ProdPrice = Product.GetProductPurchasePrice(Convert.ToInt32(drProduct["ProductID"].ToString()), Global.ParentAccClassID);
                        ProductValue1 += ProdPrice * closingQuantity1;
                    }
                }

                if (ProductValue1 > 0)
                {
                    WriteTrial("", "Opening Stock", 0, ProductValue1.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), 0.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), "GROUP", IsCrystalReport);
                    DebitSum += ProductValue1;
                }
                //First gathering the information of all Account Groups from Acc.tblGroup
                try
                {
                    //#region BLOCK FOR SUMMARY TRIAL BALANCE
                    DataTable dt = AccountGroup.GetGroupTable(-1);//Collecting the all groups form Acc.tblGroup

                    int Sno = 1;
                    foreach (DataRow dr in dt.Rows)
                    {
                        double m_dbal = 0;
                        double m_cbal = 0;
                        double m_dbalPY = 0;
                        double m_cbalPY = 0;
                        //Calling thoes method which need only date range parameter
                        // Transaction.GetGroupBalance(m_TBS.FromDate, m_TBS.ToDate, Convert.ToInt32(dr["GroupID"]), false, ref m_dbal, ref m_cbal, m_TBS.AccClassID, m_TBS.ProjectID);

                        //Also Check For Available Child Of The Respective Group If no child then only proceed orelse terminate
                        DataTable CheckForItsChild = AccountGroup.GetChildGroupsFromParentID(dr["GroupID"].ToString());

                        if (CheckForItsChild.Rows.Count < 1)
                        {
                            #region BLOCK FOR DATE RANGE SELECTION
                            //Block for DateTime range selection
                            if (m_TBS.HasDateRange == true)//When datetime is selected
                            {
                                if (m_TBS.ShowPreviousYear == true)
                                {
                                    Transaction.GetOpeningGroupBalancePY(null, Global.PYToDate, Convert.ToInt32(dr["GroupID"]), false, ref m_dbalPY, ref m_cbalPY, m_TBS.AccClassID, m_TBS.ProjectID);
                                }
                                Transaction.GetOpeningGroupBalance(null, m_TBS.ToDate, Convert.ToInt32(dr["GroupID"]), false, ref m_dbal, ref m_cbal, m_TBS.AccClassID, m_TBS.ProjectID);
                            }
                            else//Otherwise
                            {
                                //Transaction.GetGroupBalance(Convert.ToInt32(dr["GroupID"]), true, ref m_dbal, ref m_cbal, m_TBS.AccClassID, m_TBS.ProjectID);
                                Transaction.GetOpeningGroupBalance(null, null, Convert.ToInt32(dr["GroupID"]), false, ref m_dbal, ref m_cbal, m_TBS.AccClassID, m_TBS.ProjectID);
                                if (m_TBS.ShowPreviousYear == true)
                                {
                                    Transaction.GetOpeningGroupBalance(null, null, Convert.ToInt32(dr["GroupID"]), false, ref m_dbalPY, ref m_cbalPY, m_TBS.AccClassID, m_TBS.ProjectID);
                                }
                            }
                            #endregion


                            if (m_TBS.ShowZeroBalance == false && m_dbal == 0 && m_cbal == 0) //In case of zero balance
                            {
                                //Do nothing
                            }
                            else
                            {
                                string EngName = AccountGroup.GetEngName(Convert.ToInt32(dr["GroupID"]).ToString());  //calling this funtion for getting EngName from tblGroup corresponding to GroupID having corresponding GroupID
                                double Balance = (m_dbal - m_cbal);//For Asset[Debit Balance - Credit Balance]
                                if (m_TBS.ShowPreviousYear == true)
                                {
                                    double BalancePY = (m_dbalPY - m_cbalPY);
                                    WriteTrialPY(dr["LedgerCode"].ToString(), EngName, Convert.ToInt32(dr["GroupID"]), m_dbal.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), m_cbal.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), "GROUP", IsCrystalReport, m_dbalPY.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), m_cbalPY.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));
                                }
                                else
                                {
                                    //WriteAssets(Sno, EngName, Convert.ToInt32(dr["GroupID"]), Balance.ToString(), "GROUP");
                                    WriteTrial(dr["LedgerCode"].ToString(), EngName, Convert.ToInt32(dr["GroupID"]), m_dbal.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), m_cbal.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), "GROUP", IsCrystalReport);
                                }
                                Sno++;
                                DebitSum += m_dbal;
                                CreditSum += m_cbal;
                                DebitSumPY += m_dbalPY;
                                CreditSumPY += m_cbalPY;


                                //If details is selected, show details i.e. ledgers present inside
                                if(m_dbal>0||m_cbal>0)
                                {
                                    if (m_TBS.Details == true)
                                    {
                                        if (m_TBS.ShowPreviousYear == true)
                                        {
                                            WriteDetailsPY(Convert.ToInt32(dr["GroupID"]), IsCrystalReport);
                                        }
                                        else
                                        {
                                            WriteDetails(Convert.ToInt32(dr["GroupID"]), IsCrystalReport);
                                        }
                                        
                                    }
                                }

                            }//End of zero balance check

                        }
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }



            }
            #endregion


            ProgressForm.UpdateProgress(80, "Calculating difference in opening balance...");

            #region BLOCK FOR CALCULATING DIFFERENCE IN OPENING BALANCE
            double closingQuantity2 = 0;
            double productvalue1 = 0;
            foreach (DataRow drProduct in dtGetAllProduct.Rows)//For Removing the Opening Different
            {
                DataTable dtOpeningStockStatusInfo1 = StockStatusBook.GetOpeningStockStatusBook(null, Convert.ToInt32(drProduct["ProductID"].ToString()), " ", m_TBS.ToDate, true, StockStatusType.OpeningStock, AccClassIDsXMLString);
                if (dtOpeningStockStatusInfo1.Rows.Count > 0)
                {
                    DataRow dropen = dtOpeningStockStatusInfo1.Rows[0];
                    closingQuantity2 = Convert.ToInt32(dropen["Quantity"].ToString());
                    double ProdPrice = Product.GetProductPurchasePrice(Convert.ToInt32(drProduct["ProductID"].ToString()), Global.ParentAccClassID);
                    productvalue1 += ProdPrice * closingQuantity2;
                }
            }
            double TotalDrOpBal, TotalCrOpBal;
            TotalDrOpBal = TotalCrOpBal = 0;
            GetOpeningBalanceSummary(GetRootAccClassID(), ref TotalDrOpBal, ref TotalCrOpBal);
            //Global.Msg("TotalDrOpBal" + TotalDrOpBal.ToString());
            TotalDrOpBal += productvalue1;
            if ((TotalDrOpBal != TotalCrOpBal))
            {
                if (TotalDrOpBal > TotalCrOpBal)//Total Opening balance in Debit side is greater so balance by adding this amount in Credit Balance
                {
                    CreditSum += (TotalDrOpBal - TotalCrOpBal);

                    WriteTrial("0", "DIFFERENCE IN OPENING BALANCE:", 0, (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), (TotalDrOpBal - TotalCrOpBal).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), "GROUP", IsCrystalReport);

                }
                else if (TotalDrOpBal < TotalCrOpBal)//Total Opening balance in Debit side is greater so balance by adding this amount in Credit Balance
                {
                    DebitSum += (TotalCrOpBal - TotalDrOpBal);

                    WriteTrial("0", "DIFFERENCE IN OPENING BALANCE:", 0, (TotalCrOpBal - TotalDrOpBal).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), "GROUP", IsCrystalReport);
                }
            }
            #endregion

            #region BLOCK FOR TOTAL AMOUNT CALCULATION FOR TRIAL BALANCE
            if (m_TBS.LedgerOnly == true)
            {
                if (m_TBS.ShowPreviousYear == true)
                {
                    //WriteTrial(0, "TRIAL TOTAL", 0, DValue.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), Cvalue.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), "GROUP", IsCrystalReport);
                    WriteTrialPY("0", "TRIAL TOTAL", 0, DValue.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), Cvalue.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), "GROUP", IsCrystalReport, DValuePY.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), CvaluePY.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));
                }
                else
                {
                    WriteTrial("0", "TRIAL TOTAL", 0, DValue.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), Cvalue.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), "GROUP", IsCrystalReport);
                }
            }
            else
            {
                if (m_TBS.ShowPreviousYear == true)
                {
                    WriteTrialPY("0", "TRIAL TOTAL", 0, DebitSum.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), CreditSum.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), "GROUP", IsCrystalReport, DebitSumPY.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), CreditSumPY.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));
                }
                else
                {
                    WriteTrial("0", "TRIAL TOTAL", 0, DebitSum.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), CreditSum.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), "GROUP", IsCrystalReport);
                }
            }
            #endregion

            ProgressForm.UpdateProgress(100, "Preparing report for display...");

            // Close the dialog if it hasn't been already

            if (ProgressForm.InvokeRequired)
                ProgressForm.BeginInvoke(new Action(() => ProgressForm.Close()));
        }

        private void WriteDetailsPY(int GroupID, bool IsCrystalReport)
        {
            if (Global.isOpeningTrial == false)
            {
                try
                {
                    DataTable dtDtlLedgerID = AccountGroup.GetDetailLedgerID(GroupID, true);
                    int Sno = 1;
                    foreach (DataRow drDtlLedgerID in dtDtlLedgerID.Rows)
                    {
                        double DebBal = 0;
                        double CreBal = 0;
                        double DebBalPY = 0;
                        double CreBalPY = 0;
                        if(m_TBS.ShowPreviousYear==true)
                        {
                            Transaction.GetLedgerBalancePY(null, Global.PYToDate, Convert.ToInt32(drDtlLedgerID["LedgerID"]), ref  DebBalPY, ref CreBalPY, m_TBS.AccClassID, m_TBS.ProjectID);
                        }
                        Transaction.GetLedgerBalance(null,m_TBS.ToDate, Convert.ToInt32(drDtlLedgerID["LedgerID"]), ref  DebBal, ref CreBal, m_TBS.AccClassID, m_TBS.ProjectID);
                        if (m_TBS.ShowZeroBalance == false && DebBal == 0 && CreBal == 0)
                            //return;
                            continue;//It cotinue the loop except below code  
                        if(m_TBS.ShowPreviousYear==true)
                        {
                           // WriteTrialPY(Sno, drDtlLedgerID["EngName"].ToString(), Convert.ToInt32(drDtlLedgerID["LedgerID"]), DebBal.ToString(), CreBal.ToString(), "LEDGER", IsCrystalReport);
                        }
                        else
                        {
                            if(m_TBS.ShowPreviousYear==false)
                                WriteTrial(drDtlLedgerID["LedgerCode"].ToString(), drDtlLedgerID["EngName"].ToString(), Convert.ToInt32(drDtlLedgerID["LedgerID"]), DebBal.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), CreBal.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), "LEDGER", IsCrystalReport);
                            else
                                WriteTrialPY(drDtlLedgerID["LedgerCode"].ToString(), drDtlLedgerID["EngName"].ToString(), Convert.ToInt32(drDtlLedgerID["LedgerID"]), DebBal.ToString(), CreBal.ToString(), "LEDGER", IsCrystalReport, DebBalPY.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), CreBalPY.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));
                        }
                        Sno++;

                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                try
                {
                    DataTable dtDtlLedgerID = AccountGroup.GetDetailLedgerID(GroupID, true);
                    int Sno = 1;
                    foreach (DataRow drDtlLedgerID in dtDtlLedgerID.Rows)
                    {
                        double DebBal = 0;
                        double CreBal = 0;
                        double DebBalPY = 0;
                        double CreBalPY = 0;

                        Transaction.GetOpeningLedgerBalance(null, m_TBS.ToDate, Convert.ToInt32(drDtlLedgerID["LedgerID"]), ref  DebBal, ref CreBal, m_TBS.AccClassID, m_TBS.ProjectID);
                        if (m_TBS.ShowPreviousYear == true)
                        {
                            Transaction.GetOpeningLedgerBalancePY(null, Global.PYToDate, Convert.ToInt32(drDtlLedgerID["LedgerID"]), ref  DebBalPY, ref CreBalPY, m_TBS.AccClassID, m_TBS.ProjectID);
                        }
                        if (m_TBS.ShowZeroBalance == false && DebBal == 0 && CreBal == 0)
                            //return;
                            continue;//It cotinue the loop except below code 
                        if (m_TBS.ShowPreviousYear == true)
                        {
                            WriteTrialPY(drDtlLedgerID["LedgerCode"].ToString(), drDtlLedgerID["EngName"].ToString(), Convert.ToInt32(drDtlLedgerID["LedgerID"]), DebBal.ToString(), CreBal.ToString(), "LEDGER", IsCrystalReport, DebBalPY.ToString(), CreBalPY.ToString());
                        }
                        else
                        {
                            WriteTrial(drDtlLedgerID["LedgerCode"].ToString(), drDtlLedgerID["EngName"].ToString(), Convert.ToInt32(drDtlLedgerID["LedgerID"]), DebBal.ToString(), CreBal.ToString(), "LEDGER", IsCrystalReport);
                        }
                        Sno++;

                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void WriteLedgerPY(int GroupID, bool IsCrystalReport)
        {
            //Ledger processing starts for Asset
            Transaction Transaction = new Transaction();
            DataTable dtledg = Ledger.GetLedgerTable(GroupID);
            int Sno = 1;
            foreach (DataRow drledger in dtledg.Rows)
            {
                double m_dbal1 = 0;
                double m_cbal1 = 0;
                double m_dbal1PY = 0;
                double m_cbal1PY = 0;

                //Just calling the method which need only need the date range parameter
                if (m_TBS.ShowPreviousYear == true)
                {
                    Transaction.GetLedgerBalancePY(Global.PYFromDate, Global.PYToDate, Convert.ToInt32(drledger["LedgerID"]), ref m_dbal1PY, ref m_cbal1PY, m_TBS.AccClassID, m_TBS.ProjectID);
                }
                Transaction.GetLedgerBalance(m_TBS.FromDate, m_TBS.ToDate, Convert.ToInt32(drledger["LedgerID"]), ref m_dbal1, ref m_cbal1, m_TBS.AccClassID, m_TBS.ProjectID);
                if (m_TBS.ShowZeroBalance == false && m_dbal1 == 0 && m_cbal1 == 0)
                    continue;
                //grdBalanceSheet.Rows.Insert(AssetRowsCount);
                grdTrial.Rows.Insert(TrialRowsCount);
                // Block for getting LedgerName
                DataTable dtLedgerInfo = Ledger.GetLedgerInfo(Convert.ToInt32(drledger["LedgerID"]), LangMgr.DefaultLanguage);

                DataRow drLedgerInfo = dtLedgerInfo.Rows[0];
                //double Balance = (m_dbal1 - m_cbal1);//for Asset[ Balance =(Debit Balance-Credit Balance)]

                //if (Type == AccountType.Assets)
                //    WriteAssets(0, drLedgerInfo["LedName"].ToString(), Convert.ToInt32(drledger["LedgerID"]), Balance.ToString(), "LEDGER");
                //else if (Type == AccountType.Liabilities)
                //    WriteLiabilities(0, drLedgerInfo["LedName"].ToString(), Convert.ToInt32(drledger["LedgerID"]), Balance.ToString(), "LEDGER");
                if (m_TBS.ShowPreviousYear == true)
                {
                    WriteTrialPY(drledger["LedgerCode"].ToString(), drledger["LedName"].ToString(), Convert.ToInt32(drledger["LedgerID"]), m_dbal1.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), m_cbal1.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), "LEDGER", IsCrystalReport, m_dbal1PY.ToString(), m_cbal1PY.ToString());
                }
                else
                {
                    WriteTrial(drledger["LedgerCode"].ToString(), drledger["LedName"].ToString(), Convert.ToInt32(drledger["LedgerID"]), m_dbal1.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), m_cbal1.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), "LEDGER", IsCrystalReport);
                }
                Sno++;

            }
        }

        private void WriteSecondLevelPY(int GroupID, bool IsCrystalReport)
        {
            DataTable dtSecDtl = AccountGroup.GetGroupTable(GroupID);
            int Sno = 1;
            foreach (DataRow dr1 in dtSecDtl.Rows)
            {
                double m_dbal1 = 0;
                double m_cbal1 = 0;
                double m_dbal1PY = 0;
                double m_cbal1PY = 0;

                //just calling the method which need only date range parameter
                if (m_TBS.HasDateRange)
                {
                    
                    Transaction.GetGroupBalance(m_TBS.FromDate, m_TBS.ToDate, Convert.ToInt32(dr1["GroupID"]), true, ref m_dbal1, ref m_cbal1, m_TBS.AccClassID, m_TBS.ProjectID);
                    if (m_TBS.ShowPreviousYear == true)
                    {
                        Transaction.GetGroupBalancePY(Global.PYFromDate, Global.PYToDate, Convert.ToInt32(dr1["GroupID"]), true, ref m_dbal1PY, ref m_cbal1PY, m_TBS.AccClassID, m_TBS.ProjectID);
                    }
                }
                else
                {
                    Transaction.GetGroupBalance(Convert.ToDateTime("01 / 01 / 1900"), Convert.ToDateTime("01 / 01 / 1900"), Convert.ToInt32(dr1["GroupID"]), true, ref m_dbal1, ref m_cbal1, m_TBS.AccClassID, m_TBS.ProjectID);
                   // Transaction.GetGroupBalance(null, null, Convert.ToInt32(dr1["GroupID"]), true, ref m_dbal1, ref m_cbal1, m_TBS.AccClassID, m_TBS.ProjectID);
                    if (m_TBS.ShowPreviousYear == true)
                    {
                        Transaction.GetGroupBalancePY(Convert.ToDateTime("01 / 01 / 1900"), Convert.ToDateTime("01 / 01 / 1900"), Convert.ToInt32(dr1["GroupID"]), true, ref m_dbal1PY, ref m_cbal1PY, m_TBS.AccClassID, m_TBS.ProjectID);
                    }
                }
                if (m_TBS.ShowZeroBalance == false && m_dbal1 == 0 && m_cbal1 == 0)
                    continue;

                ////Checking whether debit balance or credit balance?
                //double Balance1;
                //Balance1 = m_dbal1 - m_cbal1;
                // Block for getting GroupName 
                string EngName1 = AccountGroup.GetEngName((dr1["GroupID"].ToString()));  //calling this funtion for getting EngName from tblGroup corresponding to GroupID having corresponding GroupID

                //if (Type == AccountType.Assets)
                //    WriteAssets(0, "- " + EngName1, Convert.ToInt32(dr1["GroupID"]), Balance1.ToString(), "GROUP");
                //else if (Type == AccountType.Liabilities)
                //    WriteLiabilities(0, "- " + EngName1, Convert.ToInt32(dr1["GroupID"]), Balance1.ToString(), "GROUP");
                if(m_TBS.ShowPreviousYear==false)
                    WriteTrial(dr1["LedgerCode"].ToString(), "* " + EngName1, Convert.ToInt32(dr1["GroupID"]), m_dbal1.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), m_cbal1.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), "SUBGROUP", IsCrystalReport);
                else
                    WriteTrialPY(dr1["LedgerCode"].ToString(), "* " + EngName1, Convert.ToInt32(dr1["GroupID"]), m_dbal1.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), m_cbal1.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), "SUBGROUP", IsCrystalReport, m_dbal1PY.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), m_cbal1PY.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));
                Sno++;

            }
        }

        private void button1_Click(object sender, EventArgs e)
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
          
                dsTrialPY.Clear();
          
            //otherwise it populate the records again and again
            double DebitSum, CreditSum;
            DebitSum = CreditSum = 0;
            double DebitSumPY, CreditSumPY;
            DebitSumPY = CreditSumPY = 0;
            rptTrialBalance rpt = new rptTrialBalance();
            rptTrialBalancePY rpt1 = new rptTrialBalancePY();
            rptTBPY rpt2 = new rptTBPY();
            //Fill the logo on the report
           
                Misc.WriteLogo(dsTrialPY, "tblImage");
                //Set DataSource to be dsTrial dataset on the report
                rpt2.SetDataSource(dsTrialPY);
          
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

                //pdvCompany_Name.Value = m_CompanyDetails.CompanyName;
                //pvCollection.Clear();
                //pvCollection.Add(pdvCompany_Name);
                //rpt.DataDefinition.ParameterFields["Company_Name"].ApplyCurrentValues(pvCollection);

                //pdvCompany_Address.Value = m_CompanyDetails.Address1;
                //pvCollection.Clear();
                //pvCollection.Add(pdvCompany_Address);
                //rpt2.DataDefinition.ParameterFields["Company_Address"].ApplyCurrentValues(pvCollection);

                //pdvCompany_PAN.Value = m_CompanyDetails.PAN;
                //pvCollection.Clear();
                //pvCollection.Add(pdvCompany_PAN);
                //rpt2.DataDefinition.ParameterFields["Company_PAN"].ApplyCurrentValues(pvCollection);

                //pdvCompany_Phone.Value = "Phone No.: " + m_CompanyDetails.Telephone;
                //pvCollection.Clear();
                //pvCollection.Add(pdvCompany_Phone);
                //rpt.DataDefinition.ParameterFields["Company_Phone"].ApplyCurrentValues(pvCollection);

                //pdvCompany_Slogan.Value = m_CompanyDetails.Website;
                //pvCollection.Clear();
                //pvCollection.Add(pdvCompany_Slogan);
                //rpt2.DataDefinition.ParameterFields["Company_Slogan"].ApplyCurrentValues(pvCollection);

                //pdvReport_Head.Value = "Trial Balance";
                //pvCollection.Clear();
                //pvCollection.Add(pdvReport_Head);
                //rpt2.DataDefinition.ParameterFields["Report_Head"].ApplyCurrentValues(pvCollection);
                //if (m_CompanyDetails.FYFrom != null)
                //    pdvFiscal_Year.Value = "Fiscal Year:" + Date.ToSystem(Convert.ToDateTime(m_CompanyDetails.FYFrom));

                //pvCollection.Clear();
                //pvCollection.Add(pdvFiscal_Year);
                //rpt2.DataDefinition.ParameterFields["Fiscal_Year"].ApplyCurrentValues(pvCollection);

                //Update the progressbar
                //ProgressForm.UpdateProgress(80, "Calculating Parameters...");

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
                //else//both are null
                //{
                //    pdvReport_Date.Value = "As of now";

                //}


                //pvCollection.Clear();

                //pvCollection.Add(pdvReport_Date);
                //rpt2.DataDefinition.ParameterFields["Report_Date"].ApplyCurrentValues(pvCollection);

                DisplayTrialBlance(true);
               // dsTrialPY.Tables["tblGroup"].Rows.Add("1", "Test", "200", "300", "CrystalReport","500","600");


                CrystalDecisions.Shared.ExportOptions CrExportOptions = new CrystalDecisions.Shared.ExportOptions();
                DiskFileDestinationOptions CrDiskFileDestinationOptions = new DiskFileDestinationOptions();
                CrDiskFileDestinationOptions.DiskFileName = FileName;
                //rpt.Export(opt);
                //return;
                //Finally, show the report form
                frmReportViewer frm = new frmReportViewer();
                if (m_TBS.ShowPreviousYear == false)
                    frm.SetReportSource(rpt);
                else
                    frm.SetReportSource(rpt2);

                //Update the progressbar
                ProgressForm.UpdateProgress(100, "Showing Report...");

                // Close the dialog
                ProgressForm.CloseForm();
                PrintType myPrintType =PrintType.CrystalReport;
                switch (myPrintType)
                {
                    case PrintType.DirectPrint: //Direct Printer
                        if (m_TBS.ShowPreviousYear == false)
                        {
                            rpt.PrintOptions.PrinterName = "";
                            rpt.PrintToPrinter(1, false, 0, 0);
                        }
                        else
                        {
                            rpt1.PrintOptions.PrinterName = "";
                            rpt1.PrintToPrinter(1, false, 0, 0);
                        }
                        return;
                    case PrintType.Excel: //Excel
                        if (m_TBS.ShowPreviousYear == false)
                        {
                            ExcelFormatOptions CrFormatTypeOptions = new ExcelFormatOptions();
                            CrExportOptions = rpt.ExportOptions;
                            CrExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                            CrExportOptions.ExportFormatType = ExportFormatType.Excel;
                            CrExportOptions.DestinationOptions = CrDiskFileDestinationOptions;
                            CrExportOptions.FormatOptions = CrFormatTypeOptions;
                            rpt.Export();
                            rpt.Close();
                        }
                        else
                        {
                            ExcelFormatOptions CrFormatTypeOptions = new ExcelFormatOptions();
                            CrExportOptions = rpt.ExportOptions;
                            CrExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                            CrExportOptions.ExportFormatType = ExportFormatType.Excel;
                            CrExportOptions.DestinationOptions = CrDiskFileDestinationOptions;
                            CrExportOptions.FormatOptions = CrFormatTypeOptions;
                            rpt1.Export();
                            rpt1.Close();
                        }
                        return;
                    case PrintType.PDF: //PDF
                        if (m_TBS.ShowPreviousYear == false)
                        {
                            PdfFormatOptions CrFormatTypeOptionsPdf = new PdfFormatOptions();
                            CrExportOptions = rpt.ExportOptions;
                            CrExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                            CrExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                            CrExportOptions.DestinationOptions = CrDiskFileDestinationOptions;
                            CrExportOptions.FormatOptions = CrFormatTypeOptionsPdf;
                            rpt.Export();
                            rpt.Close();
                        }
                        else
                        {
                            PdfFormatOptions CrFormatTypeOptionsPdf = new PdfFormatOptions();
                            CrExportOptions = rpt.ExportOptions;
                            CrExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                            CrExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                            CrExportOptions.DestinationOptions = CrDiskFileDestinationOptions;
                            CrExportOptions.FormatOptions = CrFormatTypeOptionsPdf;
                            rpt1.Export();
                            rpt1.Close();
                        }
                        return;
                    default: //Crystal Report

                        frm.Show();
                        frm.WindowState = FormWindowState.Maximized;

                        break;
                }

                this.Cursor = Cursors.Default;
           
            
        }

        private string ReadAllAccClassID()
        {
            #region  AccountingClassID

            ArrayList arrChildAccClassIDs = new ArrayList();
            foreach (object j in m_TBS.AccClassID)
            {
                AccountClass.GetChildIDs(Convert.ToInt32(j), ref arrChildAccClassIDs);

            }
            m_TBS.AccClassID.AddRange(arrChildAccClassIDs);

            #endregion

            System.Text.Encoding AEncoder = System.Text.Encoding.Unicode;
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            System.Xml.XmlTextWriter tw = new System.Xml.XmlTextWriter(ms, AEncoder);

            tw.WriteStartDocument();
            #region  Accountclass
            tw.WriteStartElement("STOCKSTATUS");
            {
                //Write Checked Accounting class ID
                try
                {
                    tw.WriteStartElement("ACCCLASSIDS");
                    foreach (string tag in m_TBS.AccClassID)
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
            return strXML;
        }

        private string ReadAllProjectID()
        {
            #region  ProjectID
            ArrayList arrchildProjectIds = new ArrayList();
            Project.GetChildProjects(Convert.ToInt32(m_TBS.ProjectID), ref arrchildProjectIds);
            ArrayList ProjectIDCollection = new ArrayList();
            foreach (object obj in arrchildProjectIds)
            {
                int p = (int)obj;
                ProjectIDCollection.Add(p.ToString());
            }

            string ProjectIDS = "'" + m_TBS.ProjectID + "'";

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
            tw.WriteStartElement("STOCKSTATUS");
            {
                //Write Checked Accounting class ID
                try
                {
                    tw.WriteStartElement("PROJECTIDS");
                    tw.WriteElementString("PID", Convert.ToInt32(m_TBS.ProjectID).ToString());
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
        
    }
}
