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
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using DateManager;
using System.Threading;
using Common;
using Accounts.Reports;

namespace Accounts
{
    public partial class frmBalanceSheet : Form
    {
        private SourceGrid.Cells.Controllers.CustomEvents dblClick;
        private bool isMax = true;
        private BalanceSheetSettings m_BS;
        private Accounts.Model.dsBalanceSheet dsBalanceSheet = new Model.dsBalanceSheet();
        private int AssetRowsCount, LiabilitiesRowsCount;
        private int prntDirect = 0;
        private string FileName = "";
        ArrayList AccClassID = new ArrayList();
        private double AdditionalExpences = 0;
        //Different grid views
        private SourceGrid.Cells.Views.Cell HeaderView;
        private SourceGrid.Cells.Views.Cell GroupView;
        private SourceGrid.Cells.Views.Cell LedgerView;
        private SourceGrid.Cells.Views.Cell SubGroupView;
        private DataTable dtTemp;
        DataTable dtLiabilities;
        DataTable dtAssets;
        DataTable dtCrReport;
        DataTable dtStock = new DataTable();

        //For Export Menu
        ContextMenu Menu_Export;
        IMDIMainForm m_MDIMain;
        public frmBalanceSheet(BalanceSheetSettings BS, IMDIMainForm frm)//Constructor having class as argumenet
        {
            m_MDIMain = frm;
            // Try catch block for error handling
            try
            {
                #region BLOCK FOR INITIALIZING THE CONSTRUCTOR PARAMETER
                // Initilizing the constructor's parameter
                InitializeComponent();
                m_BS = new BalanceSheetSettings();//dynamic memory allocation of object of class  BalanceSheetSettings object
                m_BS.FromDate = BS.FromDate;
                m_BS.ToDate = BS.ToDate;
                m_BS.AccClassID = BS.AccClassID;
                m_BS.HasDateRange = BS.HasDateRange;
                m_BS.ShowZeroBalance = BS.ShowZeroBalance;
                m_BS.Detail = BS.Detail;
                m_BS.AllGroups = BS.AllGroups;
                m_BS.OnlyPrimaryGroups = BS.OnlyPrimaryGroups;
                m_BS.ShowSecondLevelDtl = BS.ShowSecondLevelDtl;
                m_BS.DispFormat = BS.DispFormat;
                m_BS.ProjectID = BS.ProjectID;
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
            if (m_BS.AccClassID.Count > 0)
            {
                //Find Root Class
                DataTable dtTemp = AccountClass.GetRootAccClass(Convert.ToInt32(m_BS.AccClassID[0]));
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
        private void MakeHeader()//This is the block for both asset as well as liabilites for T-format
        {
            #region BLOCK FOR ASSET HEDADER PART
            //Write header part
            //SourceGrid.Cells.Views.Cell head = new SourceGrid.Cells.Views.Cell();
            //head.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Color.LightBlue);

            grdBalanceSheet.Rows.Insert(0);
            grdBalanceSheet.Rows.Insert(1);

            grdBalanceSheet[0, 0] = new MyHeader("Assets");
            grdBalanceSheet[0, 0].ColumnSpan = 3;
            // grdBalanceSheet[0, 0].View = new SourceGrid.Cells.Views.Cell(head);
            grdBalanceSheet[0, 5] = new MyHeader("Liabilities");
            grdBalanceSheet[0, 5].ColumnSpan = 3;
            //grdBalanceSheet[0, 5].View = new SourceGrid.Cells.Views.Cell(head);
            grdBalanceSheet[1, 0] = new MyHeader("S.No.");
            // grdBalanceSheet[1, 0].View = new SourceGrid.Cells.Views.Cell(head);
            grdBalanceSheet[1, 1] = new MyHeader("Account Name");
            // grdBalanceSheet[1, 1].View = new SourceGrid.Cells.Views.Cell(head);
            grdBalanceSheet[1, 2] = new MyHeader("Amount");
            //grdBalanceSheet[1, 2].View = new SourceGrid.Cells.Views.Cell(head);
            grdBalanceSheet[1, 3] = new MyHeader("ID");
            grdBalanceSheet[1, 4] = new MyHeader("Type");

            //Define the width of column size

            grdBalanceSheet[1, 0].Column.Width = 45; //S.No.
            grdBalanceSheet[1, 1].Column.Width = 352;//Particulars
            grdBalanceSheet[1, 2].Column.Width = 90;//Amount


            //Code for making column invisible

            grdBalanceSheet.Columns[3].Visible = false;// making third column invisible and using it in  programming     
            grdBalanceSheet.Columns[4].Visible = false;

            #endregion

            #region BLOCK FOR LIABILITIES HEADER PART
            //FOR LIABILITIES
            grdBalanceSheet[1, 5] = new MyHeader("S.No.");
            grdBalanceSheet[1, 6] = new MyHeader("Account Name");
            grdBalanceSheet[1, 7] = new MyHeader("Amount");
            grdBalanceSheet[1, 8] = new MyHeader("ID");
            grdBalanceSheet[1, 9] = new MyHeader("Type");

            //Define width of column size
            grdBalanceSheet[1, 5].Column.Width = 45;
            grdBalanceSheet[1, 6].Column.Width = 352;
            grdBalanceSheet[1, 7].Column.Width = 90;

            //Code for making column invisible
            grdBalanceSheet.Columns[8].Visible = false;// making forth column invisible and using it in programming     
            grdBalanceSheet.Columns[9].Visible = false;

            #endregion
        }

        private void MakeAssetVerticalHeader()//This is Asset Header for Vertical format
        {
            int row = grdBalanceSheet.RowsCount;
            SourceGrid.Cells.Views.Cell head = new SourceGrid.Cells.Views.Cell();
            head.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Color.AliceBlue);
            grdBalanceSheet.Rows.Insert(row);

            grdBalanceSheet[row, 0] = new MyHeader("S.No.");
            //grdBalanceSheet[row, 0].View = new SourceGrid.Cells.Views.Cell(head);

            grdBalanceSheet[row, 1] = new MyHeader("Asset");
            //grdBalanceSheet[row, 1].View = new SourceGrid.Cells.Views.Cell(head);

            grdBalanceSheet[row, 2] = new MyHeader("Amount");
            // grdBalanceSheet[row, 2].View = new SourceGrid.Cells.Views.Cell(head);

            grdBalanceSheet[row, 3] = new MyHeader("ID");
            //grdBalanceSheet[row, 3].View = new SourceGrid.Cells.Views.Cell(head);

            grdBalanceSheet[row, 4] = new MyHeader("Type");
            // grdBalanceSheet[row, 4].View = new SourceGrid.Cells.Views.Cell(head);


            //Define the width of column size
            //grdBalanceSheet[0, 0].Column.Width = 50;
            //grdBalanceSheet[0, 1].Column.Width = 300;
            //grdBalanceSheet[0, 2].Column.Width = 250;
            //grdBalanceSheet[0, 3].Column.Width = 150;
            //grdBalanceSheet[0, 4].Column.Width = 150;

            //Code for making column invisible
            grdBalanceSheet.Columns[3].Visible = false;// making third  and fourth column invisible and using it in  programming     
            grdBalanceSheet.Columns[4].Visible = false;

        }

        private void MakeLiabilitiesVerticalHeader()//This is Liabilities Header for Vertical format
        {
            int RowCount = grdBalanceSheet.Rows.Count;
            SourceGrid.Cells.Views.Cell head = new SourceGrid.Cells.Views.Cell();
            head.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Color.AliceBlue);
            grdBalanceSheet.Rows.Insert(RowCount);

            grdBalanceSheet[RowCount, 0] = new MyHeader("S.No.");
            // grdBalanceSheet[RowCount, 0].View = new SourceGrid.Cells.Views.Cell(head);

            grdBalanceSheet[RowCount, 1] = new MyHeader("Liabilities");
            //grdBalanceSheet[RowCount, 1].View = new SourceGrid.Cells.Views.Cell(head);

            grdBalanceSheet[RowCount, 2] = new MyHeader("Amount");
            // grdBalanceSheet[RowCount, 2].View = new SourceGrid.Cells.Views.Cell(head);

            grdBalanceSheet[RowCount, 3] = new MyHeader("ID");
            //grdBalanceSheet[RowCount, 3].View = new SourceGrid.Cells.Views.Cell(head);

            grdBalanceSheet[RowCount, 4] = new MyHeader("Type");
            // grdBalanceSheet[RowCount, 4].View = new SourceGrid.Cells.Views.Cell(head);

            //Define the width of column size
            grdBalanceSheet[RowCount, 0].Column.Width = 50;
            grdBalanceSheet[RowCount, 1].Column.Width = 665;
            grdBalanceSheet[RowCount, 2].Column.Width = 250;
            grdBalanceSheet[RowCount, 3].Column.Width = 150;
            grdBalanceSheet[RowCount, 4].Column.Width = 150;

            //Code for making column invisible
            grdBalanceSheet.Columns[3].Visible = false;// making third  and fourth column invisible and using it in  programming     
            grdBalanceSheet.Columns[4].Visible = false;

        }




        private void WriteAssets(int SNo, string Name, int GroupID, string Balance, string Type)
        {

            HeaderView = new SourceGrid.Cells.Views.Cell();
            if (AssetRowsCount % 2 == 0)
            {
                HeaderView.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Color.White);
            }
            else
            {
                HeaderView.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Global.Grid_Color);
            }
            HeaderView.Font = new Font(LangMgr.GetFont(), FontStyle.Bold);

            GroupView = new SourceGrid.Cells.Views.Cell();
            if (AssetRowsCount % 2 == 0)
            {
                GroupView.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Color.White);
            }
            else
            {
                GroupView.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Global.Grid_Color);
            }
            GroupView.Font = new Font(LangMgr.GetFont(), FontStyle.Bold);

            //Text format for Ledgers
            LedgerView = new SourceGrid.Cells.Views.Cell();
            if (AssetRowsCount % 2 == 0)
            {
                LedgerView.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Color.White);
            }
            else
            {
                LedgerView.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Global.Grid_Color);
            }
            LedgerView.Font = new Font(LangMgr.GetFont(), FontStyle.Italic);
            LedgerView.ForeColor = Color.Blue;

            //Text format for SubGroup
            SubGroupView = new SourceGrid.Cells.Views.Cell();
            if (AssetRowsCount % 2 == 0)
            {
                SubGroupView.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Color.White);
            }
            else
            {
                SubGroupView.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Global.Grid_Color);
            }
            SubGroupView.Font = new Font(LangMgr.GetFont(), FontStyle.Regular);
            //Insert a row to the grid for a new row to be written
            if (grdBalanceSheet.RowsCount <= AssetRowsCount)
                grdBalanceSheet.Rows.Insert(grdBalanceSheet.RowsCount);
            // Block for getting GroupName 
            SourceGrid.Cells.Views.Cell alternate = new SourceGrid.Cells.Views.Cell();
            if (AssetRowsCount % 2 == 0)
            {
                //alternate.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Color.LightCoral);
            }
            else
            {
                alternate.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Global.Grid_Color);
            }
            string strSNo = (SNo == 0 ? "" : SNo.ToString());
            grdBalanceSheet[AssetRowsCount, 0] = new SourceGrid.Cells.Cell(strSNo);
            grdBalanceSheet[AssetRowsCount, 1] = new SourceGrid.Cells.Cell(Name);
            grdBalanceSheet[AssetRowsCount, 3] = new SourceGrid.Cells.Cell(GroupID.ToString());//Adding GroupID of each row in fourth column as invisible for further use
            grdBalanceSheet[AssetRowsCount, 4] = new SourceGrid.Cells.Cell(Type);
            grdBalanceSheet[AssetRowsCount, 2] = new SourceGrid.Cells.Cell(Balance.ToString());


            //To store the current view types accourding to the row type(Ledger, Group etc)
            SourceGrid.Cells.Views.Cell CurrentView = new SourceGrid.Cells.Views.Cell();

            switch (Type)
            {
                case "GROUP":
                    CurrentView = HeaderView;
                    break;
                case "LEDGER":
                    CurrentView = LedgerView;
                    grdBalanceSheet[AssetRowsCount, 1].Value = "    " + grdBalanceSheet[AssetRowsCount, 1].Value; //Give a little space for ledger so that it appears it is inside its parent group
                    break;
                case "SUBGROUP":
                    CurrentView = SubGroupView;
                    grdBalanceSheet[AssetRowsCount, 1].Value = "  " + grdBalanceSheet[AssetRowsCount, 1].Value; //Give a little space for subgroup so that it appears it is inside its parent group
                    break;
                default:
                    CurrentView = SubGroupView; //Because it is the normal formatting without any makeups
                    break;
            }

            grdBalanceSheet[AssetRowsCount, 0].AddController(dblClick);
            grdBalanceSheet[AssetRowsCount, 0].View = CurrentView;

            grdBalanceSheet[AssetRowsCount, 1].AddController(dblClick);
            grdBalanceSheet[AssetRowsCount, 1].View = new SourceGrid.Cells.Views.Cell(CurrentView);
            grdBalanceSheet[AssetRowsCount, 1].View.TextAlignment = DevAge.Drawing.ContentAlignment.TopLeft;

            grdBalanceSheet[AssetRowsCount, 2].AddController(dblClick);
            grdBalanceSheet[AssetRowsCount, 2].View = new SourceGrid.Cells.Views.Cell(CurrentView);
            grdBalanceSheet[AssetRowsCount, 2].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;

            //Increment assets rows count
            AssetRowsCount++;
        }
        /// <summary>
        /// This method is for writting grid of Asset after finishing Liabilites part in Vertical format
        /// soo use Liabilities rows count over here because count or grid is counted after finishing Liabilities part 
        /// </summary>
        private void WriteVerticalAsset(int SNo, string AccName, int GroupID, string Balance, string AccType, string GroupType, bool IsCrystalReport)
        {
            if (!IsCrystalReport)//When need to write records in Grid
            {
                int RowsCount = grdBalanceSheet.RowsCount;

                HeaderView = new SourceGrid.Cells.Views.Cell();
                if (RowsCount % 2 == 0)
                {
                    HeaderView.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Color.White);
                }
                else
                {
                    HeaderView.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Global.Grid_Color);
                }
                HeaderView.Font = new Font(LangMgr.GetFont(), FontStyle.Bold);

                GroupView = new SourceGrid.Cells.Views.Cell();
                if (RowsCount % 2 == 0)
                {
                    GroupView.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Color.White);
                }
                else
                {
                    GroupView.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Global.Grid_Color);
                }
                GroupView.Font = new Font(LangMgr.GetFont(), FontStyle.Bold);

                //Text format for Ledgers
                LedgerView = new SourceGrid.Cells.Views.Cell();
                if (RowsCount % 2 == 0)
                {
                    LedgerView.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Color.White);
                }
                else
                {
                    LedgerView.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Global.Grid_Color);
                }
                LedgerView.Font = new Font(LangMgr.GetFont(), FontStyle.Italic);
                LedgerView.ForeColor = Color.Blue;

                //Text format for SubGroup
                SubGroupView = new SourceGrid.Cells.Views.Cell();
                if (RowsCount % 2 == 0)
                {
                    SubGroupView.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Color.White);
                }
                else
                {
                    SubGroupView.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Global.Grid_Color);
                }
                SubGroupView.Font = new Font(LangMgr.GetFont(), FontStyle.Regular);
                SourceGrid.Cells.Views.Cell alternate = new SourceGrid.Cells.Views.Cell();
                if (RowsCount % 2 == 0)
                {
                    alternate.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Global.Grid_Color);
                }
                else
                {
                    //alternate.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Color.Cornsilk);
                }
                grdBalanceSheet.Rows.Insert(RowsCount);
                string strSNo = (SNo == 0 ? "" : SNo.ToString());
                grdBalanceSheet[RowsCount, 0] = new SourceGrid.Cells.Cell(strSNo);
                grdBalanceSheet[RowsCount, 1] = new SourceGrid.Cells.Cell(AccName);
                grdBalanceSheet[RowsCount, 3] = new SourceGrid.Cells.Cell(GroupID.ToString());//Adding GroupID of each row in fourth column as invisible for further use
                grdBalanceSheet[RowsCount, 4] = new SourceGrid.Cells.Cell(AccType);
                grdBalanceSheet[RowsCount, 2] = new SourceGrid.Cells.Cell(Balance.ToString());
                //To store the current view types accourding to the row type(Ledger, Group etc)
                SourceGrid.Cells.Views.Cell CurrentView = new SourceGrid.Cells.Views.Cell();

                switch (AccType)
                {
                    case "GROUP":
                        CurrentView = HeaderView;
                        break;
                    case "LEDGER":
                        CurrentView = LedgerView;
                        grdBalanceSheet[RowsCount, 1].Value = "    " + grdBalanceSheet[RowsCount, 1].Value; //Give a little space for ledger so that it appears it is inside its parent group
                        break;
                    case "SUBGROUP":
                        CurrentView = SubGroupView;
                        grdBalanceSheet[RowsCount, 1].Value = "  " + grdBalanceSheet[RowsCount, 1].Value; //Give a little space for subgroup so that it appears it is inside its parent group
                        break;
                    default:
                        CurrentView = SubGroupView; //Because it is the normal formatting without any makeups
                        break;
                }

                grdBalanceSheet[RowsCount, 0].AddController(dblClick);
                grdBalanceSheet[RowsCount, 0].View = CurrentView;
                //grdBalanceSheet[RowsCount, 0].View = new SourceGrid.Cells.Views.Cell(alternate);

                grdBalanceSheet[RowsCount, 1].AddController(dblClick);
                grdBalanceSheet[RowsCount, 1].View = new SourceGrid.Cells.Views.Cell(CurrentView);
                // grdBalanceSheet[RowsCount, 1].View = new SourceGrid.Cells.Views.Cell(alternate);
                grdBalanceSheet[RowsCount, 1].View.TextAlignment = DevAge.Drawing.ContentAlignment.TopLeft;

                grdBalanceSheet[RowsCount, 2].AddController(dblClick);
                grdBalanceSheet[RowsCount, 2].View = new SourceGrid.Cells.Views.Cell(CurrentView);
                // grdBalanceSheet[RowsCount, 2].View = new SourceGrid.Cells.Views.Cell(alternate);
                grdBalanceSheet[RowsCount, 2].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;


            }
            else if (IsCrystalReport)//When need to show records in Crystal Report
            {
                //string strSNo = (SNo == 0 ? "" : SNo.ToString());
                switch (GroupType)
                {
                    case "Liabilities"://Asset is displayed below Liabilities soo its GroupNumber is 2
                        if (AccType == "GROUP")
                        {
                            dsBalanceSheet.Tables["tblBalanceSheet"].Rows.Add(SNo, AccName, Balance, 1, "Group");

                        }
                        else if (AccType == "SUB_GROUP")
                        {
                            dsBalanceSheet.Tables["tblBalanceSheet"].Rows.Add(SNo, AccName, Balance, 1, "Sub_Group");
                        }
                        else if (AccType == "LEDGER")
                        {
                            dsBalanceSheet.Tables["tblBalanceSheet"].Rows.Add(SNo, AccName, Balance, 1, "Ledger");
                        }

                        break;
                    case "Asset":
                        if (AccType == "GROUP")
                        {
                            dsBalanceSheet.Tables["tblBalanceSheet"].Rows.Add(SNo, AccName, Balance, 2, "Group");
                        }
                        else if (AccType == "SUB_GROUP")
                        {
                            dsBalanceSheet.Tables["tblBalanceSheet"].Rows.Add(SNo, AccName, Balance, 2, "Sub_Group");
                        }
                        else if (AccType == "LEDGER")
                        {
                            dsBalanceSheet.Tables["tblBalanceSheet"].Rows.Add(SNo, AccName, Balance, 2, "Ledger");
                        }
                        break;
                }

            }


        }

        private void WriteDetails(int GroupID, AccountType Type, bool IsCrystalReport)
        {
            try
            {
                DataTable dtDtlLedgerID = AccountGroup.GetDetailLedgerID(GroupID, true);
                foreach (DataRow drDtlLedgerID in dtDtlLedgerID.Rows)
                {
                    double DebBal = 0;
                    double CreBal = 0;
                    if (m_BS.HasDateRange == true)//If DateRange is checked
                    {
                        Transaction.GetLedgerBalance(m_BS.FromDate, m_BS.ToDate, Convert.ToInt32(drDtlLedgerID["LedgerID"]), ref  DebBal, ref CreBal, m_BS.AccClassID, m_BS.ProjectID);
                    }
                    else //Otherwise
                    {
                        Transaction.GetLedgerBalance(null, null, Convert.ToInt32(drDtlLedgerID["LedgerID"]), ref  DebBal, ref CreBal, m_BS.AccClassID, m_BS.ProjectID);

                    }
                    if (m_BS.ShowZeroBalance == false && DebBal == 0 && CreBal == 0)
                        return;
                    if ((Type == AccountType.Assets) && (m_BS.DispFormat == BalanceSheetSettings.DisplayFormat.TFormat) && (!IsCrystalReport))
                    {
                        WriteAssets(0, "- " + drDtlLedgerID["EngName"].ToString(), Convert.ToInt32(drDtlLedgerID["LedgerID"]), (DebBal - CreBal).ToString(), "LEDGER");
                    }
                    else if (Type == AccountType.Assets && (m_BS.DispFormat == BalanceSheetSettings.DisplayFormat.Vertical))
                    {
                        WriteVerticalAsset(0, "- " + drDtlLedgerID["EngName"].ToString(), Convert.ToInt32(drDtlLedgerID["LedgerID"]), (DebBal - CreBal).ToString(), "LEDGER", "Asset", IsCrystalReport);
                    }
                    else if ((Type == AccountType.Liabilities) && (m_BS.DispFormat == BalanceSheetSettings.DisplayFormat.TFormat) && (!IsCrystalReport))
                    {
                        WriteLiabilities(0, "- " + drDtlLedgerID["EngName"].ToString(), Convert.ToInt32(drDtlLedgerID["LedgerID"]), (CreBal - DebBal).ToString(), "LEDGER");
                    }
                    else if (Type == AccountType.Liabilities && (m_BS.DispFormat == BalanceSheetSettings.DisplayFormat.Vertical))
                    {
                        WriteVerticalLiabilities(0, "- " + drDtlLedgerID["EngName"].ToString(), Convert.ToInt32(drDtlLedgerID["LedgerID"]), (CreBal - DebBal).ToString(), "LEDGER", "Liabilities", IsCrystalReport);
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void WriteSecondLevel(int GroupID, AccountType Type, bool IsCrystalReport)
        {
            DataTable dtSecDtl = AccountGroup.GetGroupTable(GroupID);
            foreach (DataRow dr1 in dtSecDtl.Rows)
            {
                double m_dbal1 = 0;
                double m_cbal1 = 0;
                //Check whether DateRange is checked or not?
                if (m_BS.HasDateRange == true)//If DateRange is checked
                {
                    Transaction.GetGroupBalance(m_BS.FromDate, m_BS.ToDate, Convert.ToInt32(dr1["GroupID"]), true, ref m_dbal1, ref m_cbal1, m_BS.AccClassID, m_BS.ProjectID);
                }
                else //Otherwise
                {
                    Transaction.GetGroupBalance(null, null, Convert.ToInt32(dr1["GroupID"]), true, ref m_dbal1, ref m_cbal1, m_BS.AccClassID, m_BS.ProjectID);
                }
                if (m_BS.ShowZeroBalance == false && m_dbal1 == 0 && m_cbal1 == 0)
                    continue;
                //Checking whether debit balance or credit balance?
                double Balance1;
                // Block for getting GroupName 
                string EngName1 = AccountGroup.GetEngName((dr1["GroupID"].ToString()));  //calling this funtion for getting EngName from tblGroup corresponding to GroupID having corresponding GroupID

                if ((Type == AccountType.Assets) && (m_BS.DispFormat == BalanceSheetSettings.DisplayFormat.TFormat) && (!IsCrystalReport))
                {
                    WriteAssets(0, "- " + EngName1, Convert.ToInt32(dr1["GroupID"]), (m_dbal1 - m_cbal1).ToString(), "GROUP");
                }
                else if (Type == AccountType.Assets && (m_BS.DispFormat == BalanceSheetSettings.DisplayFormat.Vertical))
                {
                    WriteVerticalAsset(0, "- " + EngName1, Convert.ToInt32(dr1["GroupID"]), (m_dbal1 - m_cbal1).ToString(), "SUB_GROUP", "Asset", IsCrystalReport);
                }
                else if ((Type == AccountType.Liabilities) && (m_BS.DispFormat == BalanceSheetSettings.DisplayFormat.TFormat) && (!IsCrystalReport))
                {
                    WriteLiabilities(0, "- " + EngName1, Convert.ToInt32(dr1["GroupID"]), (m_cbal1 - m_dbal1).ToString(), "GROUP");
                }
                else if (Type == AccountType.Liabilities && (m_BS.DispFormat == BalanceSheetSettings.DisplayFormat.Vertical))
                {
                    WriteVerticalLiabilities(0, "- " + EngName1, Convert.ToInt32(dr1["GroupID"]), (m_cbal1 - m_dbal1).ToString(), "SUB_GROUP", "Liabilities", IsCrystalReport);
                }
            }
        }

        private void WriteLedger(int GroupID, AccountType Type, ref double assetbal, ref double liabilitiesbal, bool IsGrid, bool IsCrystalReport)
        {
            //Ledger processing starts for Asset
            Transaction Transaction = new Transaction();
            DataTable dtledg = Ledger.GetLedgerTable(GroupID);
            foreach (DataRow drledger in dtledg.Rows)
            {
                double m_dbal1 = 0;
                double m_cbal1 = 0;
                // Whether DateRange is checked or not?
                if (m_BS.HasDateRange == true)// when DateRange is checked
                {
                    Transaction.GetLedgerBalance(m_BS.FromDate, m_BS.ToDate, Convert.ToInt32(drledger["LedgerID"]), ref m_dbal1, ref m_cbal1, m_BS.AccClassID, m_BS.ProjectID);
                }
                else // otherwise
                {
                    Transaction.GetLedgerBalance(null, null, Convert.ToInt32(drledger["LedgerID"]), ref m_dbal1, ref m_cbal1, m_BS.AccClassID, m_BS.ProjectID);
                }
                if (m_BS.ShowZeroBalance == false && m_dbal1 == 0 && m_cbal1 == 0)
                    continue;
                grdBalanceSheet.Rows.Insert(AssetRowsCount);
                // Block for getting LedgerName

                DataTable dtLedgerInfo = Ledger.GetLedgerInfo(Convert.ToInt32(drledger["LedgerID"]), LangMgr.DefaultLanguage);

                DataRow drLedgerInfo = dtLedgerInfo.Rows[0];

                if ((Type == AccountType.Assets) && (m_BS.DispFormat == BalanceSheetSettings.DisplayFormat.TFormat) && (!IsCrystalReport))//for both T-format and vertical format in Asset
                {
                    double AssetBal = (m_dbal1 - m_cbal1);//always remember asset and Expenditure are debit i.e.(debit-credit)
                    assetbal = AssetBal;
                    if (IsGrid == true)
                    {
                        WriteAssets(0, drLedgerInfo["LedName"].ToString(), Convert.ToInt32(drledger["LedgerID"]), AssetBal.ToString(), "LEDGER");
                    }
                }
                else if (Type == AccountType.Assets && m_BS.DispFormat == BalanceSheetSettings.DisplayFormat.Vertical)
                {
                    double AssetBal = (m_dbal1 - m_cbal1);//always remember asset and Expenditure are debit i.e.(debit-credit)
                    assetbal = AssetBal;
                    if (IsGrid == true)
                    {
                        WriteVerticalAsset(0, drLedgerInfo["LedName"].ToString(), Convert.ToInt32(drledger["LedgerID"]), AssetBal.ToString(), "LEDGER", "Asset", IsCrystalReport);
                    }

                }
                else if ((Type == AccountType.Liabilities) && (m_BS.DispFormat == BalanceSheetSettings.DisplayFormat.TFormat) && (!IsCrystalReport))//For T-format Liabities
                {
                    double LiabilitiesBal = (m_cbal1 - m_dbal1);//always remember Liabities and Income are debit i.e. (credit-debit)
                    liabilitiesbal = LiabilitiesBal;
                    if (IsGrid == true)
                    {
                        WriteLiabilities(0, drLedgerInfo["LedName"].ToString(), Convert.ToInt32(drledger["LedgerID"]), LiabilitiesBal.ToString(), "LEDGER");
                    }
                }
                else if (Type == AccountType.Liabilities && (m_BS.DispFormat == BalanceSheetSettings.DisplayFormat.Vertical))//For Vertical Liabilites
                {
                    double LiabilitiesBal = (m_cbal1 - m_dbal1);//always remember Liabities and Income are debit
                    liabilitiesbal = LiabilitiesBal;
                    if (IsGrid == true)
                    {
                        WriteVerticalLiabilities(0, drLedgerInfo["LedName"].ToString(), Convert.ToInt32(drledger["LedgerID"]), LiabilitiesBal.ToString(), "LEDGER", "Liabilities", IsCrystalReport);
                    }

                }
            }
        }

        private void WriteLiabilities(int SNo, string Name, int accoutID, string Balance, string Type)
        {
            HeaderView = new SourceGrid.Cells.Views.Cell();
            if (LiabilitiesRowsCount % 2 == 0)
            {
                HeaderView.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Color.White);
            }
            else
            {
                HeaderView.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Global.Grid_Color);
            }
            HeaderView.Font = new Font(LangMgr.GetFont(), FontStyle.Bold);

            GroupView = new SourceGrid.Cells.Views.Cell();
            if (LiabilitiesRowsCount % 2 == 0)
            {
                GroupView.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Color.White);
            }
            else
            {
                GroupView.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Global.Grid_Color);
            }
            GroupView.Font = new Font(LangMgr.GetFont(), FontStyle.Bold);

            //Text format for Ledgers
            LedgerView = new SourceGrid.Cells.Views.Cell();
            if (LiabilitiesRowsCount % 2 == 0)
            {
                LedgerView.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Color.White);
            }
            else
            {
                LedgerView.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Global.Grid_Color);
            }
            LedgerView.Font = new Font(LangMgr.GetFont(), FontStyle.Italic);
            LedgerView.ForeColor = Color.Blue;

            //Text format for SubGroup
            SubGroupView = new SourceGrid.Cells.Views.Cell();
            if (LiabilitiesRowsCount % 2 == 0)
            {
                SubGroupView.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Color.White);
            }
            else
            {
                SubGroupView.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Global.Grid_Color);
            }
            SubGroupView.Font = new Font(LangMgr.GetFont(), FontStyle.Regular);
            //Insert a row to the grid for a new row to be written
            if (grdBalanceSheet.RowsCount <= LiabilitiesRowsCount)
                grdBalanceSheet.Rows.Insert(grdBalanceSheet.RowsCount);
            SourceGrid.Cells.Views.Cell alternate = new SourceGrid.Cells.Views.Cell();
            if (LiabilitiesRowsCount % 2 == 0)
            {
                //alternate.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Color.LightCoral);
            }
            else
            {
                alternate.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Global.Grid_Color);
            }
            // Block for getting GroupName 

            string strSNo = (SNo == 0 ? "" : SNo.ToString());//Show blank if sno is 0
            grdBalanceSheet[LiabilitiesRowsCount, 5] = new SourceGrid.Cells.Cell(strSNo);
            grdBalanceSheet[LiabilitiesRowsCount, 6] = new SourceGrid.Cells.Cell(Name);
            grdBalanceSheet[LiabilitiesRowsCount, 8] = new SourceGrid.Cells.Cell(accoutID.ToString());//Adding GroupID of each row in fourth column as invisible for further use
            grdBalanceSheet[LiabilitiesRowsCount, 9] = new SourceGrid.Cells.Cell(Type);
            grdBalanceSheet[LiabilitiesRowsCount, 7] = new SourceGrid.Cells.Cell(Misc.GetDecimalValueByDecimalPlaces(Convert.ToDecimal(Balance)));

            //To store the current view types accourding to the row type(Ledger, Group etc)
            SourceGrid.Cells.Views.Cell CurrentView = new SourceGrid.Cells.Views.Cell();

            switch (Type)
            {
                case "GROUP":
                    CurrentView = HeaderView;
                    break;
                case "LEDGER":
                    CurrentView = LedgerView;
                    grdBalanceSheet[LiabilitiesRowsCount, 6].Value = "    " + grdBalanceSheet[LiabilitiesRowsCount, 6].Value; //Give a little space for ledger so that it appears it is inside its parent group
                    break;
                case "SUBGROUP":
                    CurrentView = SubGroupView;
                    grdBalanceSheet[LiabilitiesRowsCount, 6].Value = "  " + grdBalanceSheet[LiabilitiesRowsCount, 6].Value; //Give a little space for subgroup so that it appears it is inside its parent group
                    break;
                default:
                    CurrentView = SubGroupView; //Because it is the normal formatting without any makeups
                    break;
            }


            grdBalanceSheet[LiabilitiesRowsCount, 5].AddController(dblClick);
            grdBalanceSheet[LiabilitiesRowsCount, 5].View = CurrentView;
            // grdBalanceSheet[LiabilitiesRowsCount, 5].View = new SourceGrid.Cells.Views.Cell(alternate);

            grdBalanceSheet[LiabilitiesRowsCount, 6].AddController(dblClick);
            grdBalanceSheet[LiabilitiesRowsCount, 6].View = new SourceGrid.Cells.Views.Cell(CurrentView);
            // grdBalanceSheet[LiabilitiesRowsCount, 6].View = new SourceGrid.Cells.Views.Cell(alternate);
            grdBalanceSheet[LiabilitiesRowsCount, 6].View.TextAlignment = DevAge.Drawing.ContentAlignment.TopLeft;

            grdBalanceSheet[LiabilitiesRowsCount, 7].AddController(dblClick);
            grdBalanceSheet[LiabilitiesRowsCount, 7].View = new SourceGrid.Cells.Views.Cell(CurrentView);
            //   grdBalanceSheet[LiabilitiesRowsCount, 7].View = new SourceGrid.Cells.Views.Cell(alternate);
            grdBalanceSheet[LiabilitiesRowsCount, 7].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;

            //Increment Liablities rows count
            LiabilitiesRowsCount++;
        }

        private void WriteVerticalLiabilities(int SNo, string AccName, int GroupID, string Balance, string AccType, string GroupType, bool IsCrystalReport)
        {

            if (!IsCrystalReport)//When need to write records in Grid
            {
                int RowNum = grdBalanceSheet.RowsCount;

                HeaderView = new SourceGrid.Cells.Views.Cell();
                if (RowNum % 2 == 0)
                {
                    HeaderView.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Color.White);
                }
                else
                {
                    HeaderView.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Global.Grid_Color);
                }
                HeaderView.Font = new Font(LangMgr.GetFont(), FontStyle.Bold);

                GroupView = new SourceGrid.Cells.Views.Cell();
                if (RowNum % 2 == 0)
                {
                    GroupView.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Color.White);
                }
                else
                {
                    GroupView.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Global.Grid_Color);
                }
                GroupView.Font = new Font(LangMgr.GetFont(), FontStyle.Bold);

                //Text format for Ledgers
                LedgerView = new SourceGrid.Cells.Views.Cell();
                if (RowNum % 2 == 0)
                {
                    LedgerView.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Color.White);
                }
                else
                {
                    LedgerView.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Global.Grid_Color);
                }
                LedgerView.Font = new Font(LangMgr.GetFont(), FontStyle.Italic);
                LedgerView.ForeColor = Color.Blue;

                //Text format for SubGroup
                SubGroupView = new SourceGrid.Cells.Views.Cell();
                if (RowNum % 2 == 0)
                {
                    SubGroupView.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Color.White);
                }
                else
                {
                    SubGroupView.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Global.Grid_Color);
                }
                SubGroupView.Font = new Font(LangMgr.GetFont(), FontStyle.Regular);
                SourceGrid.Cells.Views.Cell alternate = new SourceGrid.Cells.Views.Cell();
                if (RowNum % 2 == 0)
                {
                    alternate.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Color.White);
                }
                else
                {
                    alternate.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Global.Grid_Color);
                }

                grdBalanceSheet.Rows.Insert(RowNum);
                string strSNo = (SNo == 0 ? "" : SNo.ToString());//Show blank if sno is 0
                grdBalanceSheet[RowNum, 0] = new SourceGrid.Cells.Cell(strSNo);
                grdBalanceSheet[RowNum, 1] = new SourceGrid.Cells.Cell(AccName);
                grdBalanceSheet[RowNum, 3] = new SourceGrid.Cells.Cell(GroupID.ToString());//Adding GroupID of each row in fourth column as invisible for further use
                grdBalanceSheet[RowNum, 4] = new SourceGrid.Cells.Cell(AccType);
                grdBalanceSheet[RowNum, 2] = new SourceGrid.Cells.Cell(Balance.ToString());

                //To store the current view types accourding to the row type(Ledger, Group etc)
                SourceGrid.Cells.Views.Cell CurrentView = new SourceGrid.Cells.Views.Cell();

                switch (AccType)
                {
                    case "GROUP":
                        CurrentView = HeaderView;
                        break;
                    case "LEDGER":
                        CurrentView = LedgerView;
                        grdBalanceSheet[RowNum, 1].Value = "    " + grdBalanceSheet[RowNum, 1].Value; //Give a little space for ledger so that it appears it is inside its parent group
                        break;
                    case "SUBGROUP":
                        CurrentView = SubGroupView;
                        grdBalanceSheet[RowNum, 1].Value = "  " + grdBalanceSheet[RowNum, 1].Value; //Give a little space for subgroup so that it appears it is inside its parent group
                        break;
                    case "HEADER":
                        CurrentView = HeaderView;
                        grdBalanceSheet[RowNum, 0].View.TextAlignment = DevAge.Drawing.ContentAlignment.TopCenter;
                        grdBalanceSheet[RowNum, 1].View.TextAlignment = DevAge.Drawing.ContentAlignment.TopCenter;
                        grdBalanceSheet[RowNum, 2].View.TextAlignment = DevAge.Drawing.ContentAlignment.TopCenter;
                        break;
                    default:
                        CurrentView = SubGroupView; //Because it is the normal formatting without any makeups
                        break;
                }

                grdBalanceSheet[RowNum, 0].AddController(dblClick);
                grdBalanceSheet[RowNum, 0].View = CurrentView;
                //grdBalanceSheet[RowNum, 0].View = new SourceGrid.Cells.Views.Cell(alternate);

                grdBalanceSheet[RowNum, 1].AddController(dblClick);
                grdBalanceSheet[RowNum, 1].View = new SourceGrid.Cells.Views.Cell(CurrentView);
                // grdBalanceSheet[RowNum, 1].View = new SourceGrid.Cells.Views.Cell(alternate);
                grdBalanceSheet[RowNum, 1].View.TextAlignment = DevAge.Drawing.ContentAlignment.TopLeft;

                grdBalanceSheet[RowNum, 2].AddController(dblClick);
                grdBalanceSheet[RowNum, 2].View = new SourceGrid.Cells.Views.Cell(CurrentView);
                //  grdBalanceSheet[RowNum, 2].View = new SourceGrid.Cells.Views.Cell(alternate);
                grdBalanceSheet[RowNum, 2].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;
                //LiabilitiesRowsCount++; 
            }
            else if (IsCrystalReport)//When need to show records in Crystal Report
            {
                //string strSNo = (SNo == 0 ? "" : SNo.ToString());
                switch (GroupType)
                {
                    case "Liabilities"://Asset is displayed below Liabilities soo its GroupNumber is 2
                        if (AccType == "GROUP")
                        {
                            dsBalanceSheet.Tables["tblBalanceSheet"].Rows.Add(SNo, AccName, Balance, 1, "Group");

                        }
                        else if (AccType == "SUB_GROUP")
                        {
                            dsBalanceSheet.Tables["tblBalanceSheet"].Rows.Add(SNo, AccName, Balance, 1, "Sub_Group");
                        }
                        else if (AccType == "LEDGER")
                        {
                            dsBalanceSheet.Tables["tblBalanceSheet"].Rows.Add(SNo, AccName, Balance, 1, "Ledger");
                        }

                        break;
                    case "Asset":
                        if (AccType == "GROUP")
                        {
                            dsBalanceSheet.Tables["tblBalanceSheet"].Rows.Add(SNo, AccName, Balance, 2, "Group");
                        }
                        else if (AccType == "SUB_GROUP")
                        {
                            dsBalanceSheet.Tables["tblBalanceSheet"].Rows.Add(SNo, AccName, Balance, 2, "Sub_Group");
                        }
                        else if (AccType == "LEDGER")
                        {
                            dsBalanceSheet.Tables["tblBalanceSheet"].Rows.Add(SNo, AccName, Balance, 2, "Ledger");
                        }
                        break;

                }

            }
        }

        private double GetOpeningBalanceSummary(int RootAccClassID, AccountType Type)
        {
            int GrpID = 0;

            if (Type == AccountType.Assets)
                GrpID = AccountGroup.GetGroupIDFromGroupNumber(1);//Finding the GroupID of Asset AccountGroup
            else if (Type == AccountType.Liabilities)
                GrpID = AccountGroup.GetGroupIDFromGroupNumber(2);
            double TotalDrOpBal, TotalCrOpBal;
            TotalDrOpBal = TotalCrOpBal = 0;

            DataTable dtAllLedgersInfo = OpeningBalance.GetAccClassOpeningBalance(RootAccClassID, -1); //Collect all ledger information first
            //double TotalDrOpBal, TotalCrOpBal;
            TotalDrOpBal = TotalCrOpBal = 0;
            foreach (DataRow drAllLedgersInfo in dtAllLedgersInfo.Rows)
            {
                if (!drAllLedgersInfo.IsNull("OpenBal"))
                {
                    if (drAllLedgersInfo["OpenBalDrCr"].ToString() == "DEBIT")
                        TotalDrOpBal += (Convert.ToDouble(drAllLedgersInfo["OpenBal"]));
                    else if (drAllLedgersInfo["OpenBalDrCr"].ToString() == "CREDIT")
                        TotalCrOpBal += (Convert.ToDouble(drAllLedgersInfo["OpenBal"]));
                }
            }


            if (Type == AccountType.Assets)
                return (TotalDrOpBal - TotalCrOpBal);
            else if (Type == AccountType.Liabilities)
                return (TotalCrOpBal - TotalDrOpBal);
            //Default return 
            return (TotalDrOpBal - TotalCrOpBal);



        }
        #region old code
        //private void DisplayVerticalBalanceSheet(bool IsCrystalReport)
        //{
        //    frmProgress ProgressForm = new frmProgress();
        //    // Initialize the thread that will handle the background process
        //    Thread backgroundThread = new Thread(
        //        new ThreadStart(() =>
        //        {

        //            ProgressForm.ShowDialog();
        //        }
        //    ));

        //    backgroundThread.Start();
        //    ProgressForm.UpdateProgress(20, "Initializing report...");

        //    #region LOOK AND FEEL
        //    //Text format for Total
        //    HeaderView = new SourceGrid.Cells.Views.Cell();
        //    HeaderView.Font = new Font(LangMgr.GetFont(), FontStyle.Bold);

        //    GroupView = new SourceGrid.Cells.Views.Cell();
        //    GroupView.Font = new Font(LangMgr.GetFont(), FontStyle.Bold);

        //    //Text format for Ledgers
        //    LedgerView = new SourceGrid.Cells.Views.Cell();
        //    LedgerView.Font = new Font(LangMgr.GetFont(), FontStyle.Italic);
        //    LedgerView.ForeColor = Color.Blue;

        //    //Text format for SubGroup
        //    SubGroupView = new SourceGrid.Cells.Views.Cell();
        //    SubGroupView.Font = new Font(LangMgr.GetFont(), FontStyle.Regular);
        //    //Add a double click handler. When user dblclicks the cell, will use this function to send the parent form the Currency code
        //    dblClick = new SourceGrid.Cells.Controllers.CustomEvents();
        //    dblClick.DoubleClick += new EventHandler(grd_BalanceSheetDoubleClick);

        //    //Let the whole row to be selected
        //    grdBalanceSheet.SelectionMode = SourceGrid.GridSelectionMode.Row;

        //    #endregion


        //    double AssetSum, LiabilitiesSum;
        //    AssetSum = LiabilitiesSum = 0;
        //    double assetbal, liabilitiesbal;
        //    assetbal = liabilitiesbal = 0;
        //    if (!IsCrystalReport)//need only to show for grid not for crystal report
        //    {
        //        grdBalanceSheet.ColumnsCount = 5;
        //        //grdBalanceSheet.FixedRows = 1;
        //        MakeLiabilitiesVerticalHeader();//For Liabilites Header section;
        //    }
        //    ProgressForm.UpdateProgress(40, "Calculating difference in opening balance...");

        //    #region Closing Stock Value
        //    //Closing Stock Values Check For Closing Stock Quantity and Make Closing Stock Amount
        //    double ProductValue = 0;
        //    Product allproduct = new Product();
        //    DataTable dtGetAllProduct = allproduct.getProductByGroupID();
        //    string AccClassIDsXMLString = ReadAllAccClassID();
        //    string ProjectIDsXMLString = ReadAllProjectID();
        //    int closingQuantity = 0;
        //    AdditionalExpences = 0;
        //    foreach (DataRow drProduct in dtGetAllProduct.Rows)
        //    {
        //        bool isTrans = false;
        //        if (drProduct["IsInventoryApplicable"].ToString() == "1")
        //        {
        //            DataTable dtOpeningStockStatusInfo = StockStatusBook.GetOpeningStockStatusBook(null, Convert.ToInt32(drProduct["ProductID"].ToString()), " ", m_BS.ToDate, true, StockStatusType.OpeningStock, AccClassIDsXMLString);
        //            DataTable dtTransactionStockStatusInfo = StockStatusBook.GetStockStatusBook1(null, Convert.ToInt32(drProduct["ProductID"].ToString()), "", m_BS.ToDate, true, StockStatusType.ClosingStock, AccClassIDsXMLString, ProjectIDsXMLString);
        //            if (dtTransactionStockStatusInfo.Rows.Count != 0)
        //            {
        //                foreach (DataRow drOpeningStockStatusInfo in dtOpeningStockStatusInfo.Rows)
        //                {
        //                    // if (dtTransactionStockStatusInfo.Rows.Count != 0)
        //                    //{
        //                    foreach (DataRow drTransactionStockStatusInfo in dtTransactionStockStatusInfo.Rows)
        //                    {
        //                        if (Convert.ToInt32(drTransactionStockStatusInfo["ProductID"]) == Convert.ToInt32(drOpeningStockStatusInfo["ProductID"]))
        //                        {
        //                            isTrans = true;
        //                            closingQuantity = Convert.ToInt32(drTransactionStockStatusInfo["Quantity"]) + Convert.ToInt32(drOpeningStockStatusInfo["Quantity"]);
        //                            double ProdPrice = Product.GetProductPurchasePrice(Convert.ToInt32(drTransactionStockStatusInfo["ProductID"]), Global.ParentAccClassID);
        //                            //double AddtionalCost = Product.GetFreightandCustomDuty(Convert.ToInt32(drTransactionStockStatusInfo["rowid"]));
        //                            ProductValue += ProdPrice * closingQuantity;
        //                            // ProductValue += AddtionalCost;
        //                            // AdditionalExpences = AdditionalExpences + AddtionalCost;
        //                        }

        //                    }
        //                    if (isTrans == false)
        //                    {
        //                        closingQuantity = Convert.ToInt32(drOpeningStockStatusInfo["Quantity"]);
        //                        double ProdPrice = Product.GetProductPurchasePrice(Convert.ToInt32(drOpeningStockStatusInfo["ProductID"]), Global.ParentAccClassID);
        //                        ProductValue += ProdPrice * closingQuantity;
        //                    }

        //                }

        //            }
        //            else
        //            {
        //                DataTable dtOpeningStockStatusInfo1 = StockStatusBook.GetOpeningStockStatusBook(null, Convert.ToInt32(drProduct["ProductID"].ToString()), " ", m_BS.ToDate, true, StockStatusType.OpeningStock, AccClassIDsXMLString);
        //                if (dtOpeningStockStatusInfo1.Rows.Count > 0)
        //                {
        //                    DataRow dropen = dtOpeningStockStatusInfo1.Rows[0];
        //                    closingQuantity = Convert.ToInt32(dropen["Quantity"].ToString());
        //                    double ProdPrice = Product.GetProductPurchasePrice(Convert.ToInt32(drProduct["ProductID"].ToString()), Global.ParentAccClassID);
        //                    ProductValue += ProdPrice * closingQuantity;
        //                }
        //            }
        //        }

        //    }


        //    #region LIABILITES COLUMN PROCESSING
        //    DataTable dtLiab = AccountGroup.GetGroupTable(AccountGroup.GetIDFromType(AccountType.Liabilities));//GroupID contain 1 for Asset
        //    int SnoLiab = 1;
        //    foreach (DataRow dr in dtLiab.Rows)
        //    {
        //        double m_dbal = 0;
        //        double m_cbal = 0;
        //        // Block for DateTime range selection
        //        if (m_BS.HasDateRange == true)//When datetime is selected
        //            Transaction.GetGroupBalance(m_BS.FromDate, m_BS.ToDate, Convert.ToInt32(dr["GroupID"]), true, ref m_dbal, ref m_cbal, m_BS.AccClassID, m_BS.ProjectID);
        //        else//Otherwise
        //            Transaction.GetGroupBalance(null, null, Convert.ToInt32(dr["GroupID"]), true, ref m_dbal, ref m_cbal, m_BS.AccClassID, m_BS.ProjectID);

        //        if (m_BS.ShowZeroBalance == false && m_dbal == 0 && m_cbal == 0) //In case of zero balance
        //        {
        //            //Do nothing
        //        }
        //        else
        //        {
        //            string EngName = AccountGroup.GetEngName(Convert.ToInt32(dr["GroupID"]).ToString());  //calling this funtion for getting EngName from tblGroup corresponding to GroupID having corresponding GroupID
        //            double Balance = (m_cbal - m_dbal);//For Liabilites remember it is always credit soo (credit-debit)
        //            WriteVerticalLiabilities(SnoLiab, EngName, Convert.ToInt32(dr["GroupID"]), Balance.ToString(), "GROUP", "Liabilities", IsCrystalReport);
        //            LiabilitiesSum += Balance;

        //            //If Second level group is selected, show them
        //            if (m_BS.ShowSecondLevelDtl == true)
        //                WriteSecondLevel(Convert.ToInt32(dr["GroupID"]), AccountType.Liabilities, IsCrystalReport);
        //            //If details is selected, show details i.e. ledgers present inside
        //            if (m_BS.Detail == true)
        //                WriteDetails(Convert.ToInt32(dr["GroupID"]), AccountType.Liabilities, IsCrystalReport);



        //        }//End of zero balance check

        //        SnoLiab++;
        //    }//End for loop
        //    //Display the ledger just under the account

        //    WriteLedger(AccountGroup.GetIDFromType(AccountType.Liabilities), AccountType.Liabilities, ref assetbal, ref liabilitiesbal, true, IsCrystalReport);
        //    LiabilitiesSum += liabilitiesbal;


        //    //block for calculating opening balance and posting on Liabilites side
        //    #region DIFFERENCE IN OPENENING BALANCE
        //    //Display Difference in Opening Balance
        //    double OpBalAssets = GetOpeningBalanceSummary(GetRootAccClassID(), AccountType.Assets);
        //    //  OpBalAssets = OpBalAssets + ProductValue;
        //    //Display Difference in Opening Balance
        //    //double OpBalLiab = GetOpeningBalanceSummary(GetRootAccClassID(), AccountType.Liabilities);
        //    //if (OpBalAssets > OpBalLiab)//If asset opening balance is greater than Liabilites opening balance then post this difference value to Liabilities side
        //    //{
        //    //    double OpBalDiff = OpBalAssets - OpBalLiab;
        //    //    LiabilitiesSum += OpBalDiff;
        //    //    //LiabilitiesSum += OpBalAssets;

        //    //WriteVerticalLiabilities(0, "Difference in Opening Balance", 0, OpBalAssets.ToString(), "GROUP", "Liabilities", IsCrystalReport);
        //    //}
        //    if (OpBalAssets > 0)//If asset opening balance is greater than Liabilites opening balance then post this difference value to Liabilities side
        //    {

        //        LiabilitiesSum += OpBalAssets;

        //        WriteVerticalLiabilities(0, "Difference in Opening Balance", 0, OpBalAssets.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), "GROUP", "Liabilities", IsCrystalReport);
        //    }
        //    #endregion

        //    //Block for posting 
        //    #region BLOCK FOR ADDING  PROFIT OBTAINING FROM PROFIT AND LOSS ACCOUNT
        //    //This is the block for adding either profit or loss obtaining from Profit and loass account
        //    //Calling the method for finding either profit or loss from P/L account
        //    frmProfitLossAcc m_profitLoss = new frmProfitLossAcc();
        //    double TotalProfit, TotalLoss;
        //    TotalProfit = TotalLoss = 0;
        //    ProfitLossAccSettings m_ProfitLoassSettings = new ProfitLossAccSettings();
        //    int IncomeGrpID = AccountGroup.GetIDFromType(AccountType.Income);
        //    int ExpenditureGrpID = AccountGroup.GetIDFromType(AccountType.Expenditure);
        //    m_profitLoss.Cal_TotalProfitLoss(ref TotalProfit, ref TotalLoss, IncomeGrpID, ExpenditureGrpID, m_BS.HasDateRange, m_BS.FromDate, m_BS.ToDate, m_BS.AccClassID, m_BS.ShowZeroBalance, false, m_BS.ProjectID);
        //    if (TotalProfit > 0)//Incase of Profit from p/l account
        //    {
        //        //If there is Profit then add this profit balance to Liabilities side for balancing puropose
        //        WriteVerticalLiabilities(0, "Reserve Fund and Accumulated Profit/Loss: (Current Year) ", 0, TotalProfit.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), "GROUP", "Liabilities", IsCrystalReport);
        //        LiabilitiesSum += TotalProfit;
        //    }
        //    //else if (TotalLoss > 0)
        //    //else if (Convert.ToDouble(TotalLoss.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces))) - Convert.ToDouble(AdditionalExpences.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces))) > 0)//Incase of Loss
        //    else if (TotalLoss > 0)
        //    {
        //        #region BLOCK FOR ADDING  LOSS OBTAINING FROM BALANCESHEET
        //        //If there is LOSS then add this loss balance to asset side for balancing puropose
        //        // WriteVerticalAsset(0, "Reserve Fund and Accumulated Profit/Loss: (Current Year)", 0, "-" + TotalLoss.ToString(), "GROUP", "Liabilities", IsCrystalReport);
        //        WriteVerticalLiabilities(0, "Reserve Fund and Accumulated Profit/Loss: (Current Year)", 0, "-" + TotalLoss.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), "GROUP", "Liabilities", IsCrystalReport);
        //        LiabilitiesSum -= TotalLoss;

        //        #endregion
        //    }
        //    //string Plvalue=Settings.GetSettings("P/L_AMOUNT");
        //    //if(Convert.ToDouble(Plvalue)>0)
        //    //{
        //    //    WriteVerticalLiabilities(0, "Reserve Fund and Accumulated Profit/Loss: (PreviousYear)", 0, TotalProfit.ToString(), "GROUP", "Liabilities", IsCrystalReport);
        //    //    LiabilitiesSum +=Convert.ToDouble( Plvalue);
        //    //}
        //    //else if (Convert.ToDouble(Plvalue) < 0)
        //    //{
        //    //    WriteVerticalAsset(0, "Reserve Fund and Accumulated Profit/Loss: (PreviousYear)", 0, "-" + TotalLoss.ToString(), "GROUP", "Asset", IsCrystalReport);
        //    //    LiabilitiesSum -= Convert.ToDouble(Plvalue);
        //    //}
        //    //else
        //    //{
        //    //    WriteVerticalAsset(0, "Reserve Fund and Accumulated Profit/Loss: (PreviousYear)", 0, 0.ToString(), "GROUP", "Asset", IsCrystalReport);
        //    //}


        //    #endregion
        //    if (!IsCrystalReport)//Total is just need to write for grid not for Crystal Report
        //        WriteVerticalLiabilities(0, "LIABILITIES TOTAL", 0, LiabilitiesSum.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), "GROUP", "Liabilities", IsCrystalReport);

        //    #endregion
        //    //This block for making Asset Header
        //    //grdBalanceSheet.Rows.Insert(LiabilitiesRowsCount);
        //    //grdBalanceSheet.Redim(2, 5);
        //    //MakeAssetVerticalHeader();
        //    if (!IsCrystalReport)
        //    {
        //        WriteVerticalLiabilities(0, "", 0, "", "HEADER", "Liabilities", IsCrystalReport);
        //        //WriteVerticalLiabilities(0, "Asset: ", 0, "Amount", "HEADER","Asset",IsCrystalReport);
        //    }

        //    ProgressForm.UpdateProgress(80, "Calculating  profit or loss...");

        //    #region ASSETS COLUMN PROCESSING
        //    if (!IsCrystalReport)
        //    {
        //        MakeAssetVerticalHeader();
        //    }
        //    //AccountGroup processing starts for Asset
        //    DataTable dt = AccountGroup.GetGroupTable(AccountGroup.GetIDFromType(AccountType.Assets));//GroupID contain 1 for Asset
        //    int Sno = 1;
        //    foreach (DataRow dr in dt.Rows)
        //    {
        //        double m_dbal = 0;
        //        double m_cbal = 0;
        //        // Block for DateTime range selection
        //        if (m_BS.HasDateRange == true)//When datetime is selected
        //            Transaction.GetGroupBalance(m_BS.FromDate, m_BS.ToDate, Convert.ToInt32(dr["GroupID"]), true, ref m_dbal, ref m_cbal, m_BS.AccClassID, m_BS.ProjectID);
        //        else//Otherwise
        //            // Transaction.GetGroupBalance(Convert.ToInt32(dr["GroupID"]), true, ref m_dbal, ref m_cbal, m_BS.AccClassID, m_BS.ProjectID);
        //            Transaction.GetGroupBalance(null, null, Convert.ToInt32(dr["GroupID"]), true, ref m_dbal, ref m_cbal, m_BS.AccClassID, m_BS.ProjectID);

        //        if (m_BS.ShowZeroBalance == false && m_dbal == 0 && m_cbal == 0) //In case of zero balance
        //        {
        //            //Do nothing
        //        }
        //        else
        //        {
        //            string EngName = AccountGroup.GetEngName(Convert.ToInt32(dr["GroupID"]).ToString());  //calling this funtion for getting EngName from tblGroup corresponding to GroupID having corresponding GroupID
        //            double Balance = (m_dbal - m_cbal);//For Asset[Debit Balance - Credit Balance]
        //            WriteVerticalAsset(Sno, EngName, Convert.ToInt32(dr["GroupID"]), Balance.ToString(), "GROUP", "Asset", IsCrystalReport);
        //            AssetSum += Balance;
        //            //If Second level group is selected, show them
        //            if (m_BS.ShowSecondLevelDtl == true)
        //                WriteSecondLevel(Convert.ToInt32(dr["GroupID"]), AccountType.Assets, IsCrystalReport);
        //            //If details is selected, show details i.e. ledgers present inside
        //            if (m_BS.Detail == true)
        //                WriteDetails(Convert.ToInt32(dr["GroupID"]), AccountType.Assets, IsCrystalReport);

        //        }//End of zero balance check

        //        Sno++;
        //    }//End for loop

        //    //Ledgers just beneath the Assets
        //    WriteLedger(AccountGroup.GetIDFromType(AccountType.Assets), AccountType.Assets, ref assetbal, ref liabilitiesbal, true, IsCrystalReport);//Here we have to write on grid soo make it true
        //    AssetSum += assetbal;//adding assetbal to Total Asset
        //    //#region Closing Stock Value
        //    ////Closing Stock Values Check For Closing Stock Quantity and Make Closing Stock Amount
        //    //double ProductValue = 0;
        //    //Product allproduct = new Product();
        //    //DataTable dtGetAllProduct = allproduct.getProductByGroupID();
        //    //string AccClassIDsXMLString = ReadAllAccClassID();
        //    //string ProjectIDsXMLString = ReadAllProjectID();
        //    //int closingQuantity = 0;

        //    //foreach (DataRow drProduct in dtGetAllProduct.Rows)
        //    //{
        //    //    bool isTrans = false;
        //    //    if (drProduct["IsInventoryApplicable"].ToString() == "1")
        //    //    {
        //    //        DataTable dtOpeningStockStatusInfo = StockStatusBook.GetOpeningStockStatusBook(null, Convert.ToInt32(drProduct["ProductID"].ToString()), " ", m_BS.ToDate, true, StockStatusType.OpeningStock, AccClassIDsXMLString);
        //    //        DataTable dtTransactionStockStatusInfo = StockStatusBook.GetStockStatusBook1(null, Convert.ToInt32(drProduct["ProductID"].ToString()), "", m_BS.ToDate, true, StockStatusType.ClosingStock, AccClassIDsXMLString, ProjectIDsXMLString);
        //    //        if (dtTransactionStockStatusInfo.Rows.Count != 0)
        //    //        {
        //    //            foreach (DataRow drOpeningStockStatusInfo in dtOpeningStockStatusInfo.Rows)
        //    //            {
        //    //                // if (dtTransactionStockStatusInfo.Rows.Count != 0)
        //    //                //{
        //    //                foreach (DataRow drTransactionStockStatusInfo in dtTransactionStockStatusInfo.Rows)
        //    //                {
        //    //                    if (Convert.ToInt32(drTransactionStockStatusInfo["ProductID"]) == Convert.ToInt32(drOpeningStockStatusInfo["ProductID"]))
        //    //                    {
        //    //                        isTrans = true;
        //    //                        closingQuantity = Convert.ToInt32(drTransactionStockStatusInfo["Quantity"]) + Convert.ToInt32(drOpeningStockStatusInfo["Quantity"]);
        //    //                        double ProdPrice = Product.GetProductPurchasePrice(Convert.ToInt32(drTransactionStockStatusInfo["ProductID"]), Global.ParentAccClassID);
        //    //                        double AddtionalCost = Product.GetFreightandCustomDuty(Convert.ToInt32(drTransactionStockStatusInfo["rowid"]));
        //    //                        ProductValue += ProdPrice * closingQuantity;
        //    //                        ProductValue += AddtionalCost;
        //    //                    }

        //    //                }
        //    //                if (isTrans == false)
        //    //                {
        //    //                    closingQuantity = Convert.ToInt32(drOpeningStockStatusInfo["Quantity"]);
        //    //                    double ProdPrice = Product.GetProductPurchasePrice(Convert.ToInt32(drOpeningStockStatusInfo["ProductID"]), Global.ParentAccClassID);
        //    //                    ProductValue += ProdPrice * closingQuantity;
        //    //                }
        //    //
        //    //            }
        //    //
        //    //        }
        //    //        else
        //    //        {
        //    //            DataTable dtOpeningStockStatusInfo1 = StockStatusBook.GetOpeningStockStatusBook(null, Convert.ToInt32(drProduct["ProductID"].ToString()), " ", m_BS.ToDate, true, StockStatusType.OpeningStock, AccClassIDsXMLString);
        //    //            if (dtOpeningStockStatusInfo1.Rows.Count > 0)
        //    //            {
        //    //                DataRow dropen = dtOpeningStockStatusInfo1.Rows[0];
        //    //                closingQuantity = Convert.ToInt32(dropen["Quantity"].ToString());
        //    //                double ProdPrice = Product.GetProductPurchasePrice(Convert.ToInt32(drProduct["ProductID"].ToString()), Global.ParentAccClassID);
        //    //                ProductValue += ProdPrice * closingQuantity;
        //    //            }
        //    //        }
        //    //    }

        //    //}
        //    WriteVerticalAsset(Sno, "Closing Stock", 0, ProductValue.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), "GROUP", "Asset", IsCrystalReport);
        //    AssetSum += ProductValue;
        //    #endregion
        //    double closingQuantity1 = 0;
        //    double productvalue1 = 0;
        //    foreach (DataRow drProduct in dtGetAllProduct.Rows)//For Removing the Opening Different
        //    {
        //        DataTable dtOpeningStockStatusInfo1 = StockStatusBook.GetOpeningStockStatusBook(null, Convert.ToInt32(drProduct["ProductID"].ToString()), " ", m_BS.ToDate, true, StockStatusType.OpeningStock, AccClassIDsXMLString);
        //        if (dtOpeningStockStatusInfo1.Rows.Count > 0)
        //        {
        //            DataRow dropen = dtOpeningStockStatusInfo1.Rows[0];
        //            closingQuantity1 = Convert.ToInt32(dropen["Quantity"].ToString());
        //            double ProdPrice = Product.GetProductPurchasePrice(Convert.ToInt32(drProduct["ProductID"].ToString()), Global.ParentAccClassID);
        //            productvalue1 += ProdPrice * closingQuantity1;
        //        }
        //    }
        //    //block for calculating opening balance and posting on ASSET side
        //    #region DIFFERENCE IN OPENENING BALANCE
        //    //if (OpBalAssets < OpBalLiab)//If asset opening balance is greater than Liabilites opening balance then post this difference value to Liabilities side
        //    //{
        //    //    double OpBalDiff = OpBalLiab - OpBalAssets;
        //    //    AssetSum += OpBalDiff;
        //    //    WriteVerticalAsset(0, "Difference in Opening Balance: ", 0, OpBalDiff.ToString(), "GROUP","Asset",IsCrystalReport);
        //    //}

        //    //MessageBox.Show(OpBalAssets.ToString());
        //    //MessageBox.Show(ProductValue.ToString());
        //    //OpBalAssets = OpBalAssets+ProductValue;
        //    //MessageBox.Show(OpBalAssets.ToString());
        //    if (OpBalAssets + productvalue1 < 0)//If asset opening balance is greater than Liabilites opening balance then post this difference value to Liabilities side
        //    {
        //        //double OpBalDiff = OpBalLiab - OpBalAssets;
        //        double oPBalDiff = (-1) * OpBalAssets;
        //        AssetSum += oPBalDiff;
        //        WriteVerticalAsset(0, "Difference in Opening Balance: ", 0, oPBalDiff.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), "GROUP", "Asset", IsCrystalReport);
        //    }
        //    #endregion

        //    //Block for posting 
        //    //#region BLOCK FOR ADDING  LOSS OBTAINING FROM BALANCESHEET
        //    //if (TotalLoss > 0)//Incase of Profit from p/l account
        //    //{
        //    //    //If there is LOSS then add this loss balance to asset side for balancing puropose
        //    //    WriteVerticalAsset(0, "Total Loss: ", 0, TotalLoss.ToString(), "GROUP","Asset",IsCrystalReport);
        //    //    AssetSum += TotalLoss;
        //    //}
        //    //#endregion
        //    //block for writting total amount of asset
        //    if (!IsCrystalReport)
        //        WriteVerticalAsset(0, "ASSETS TOTAL", 0, AssetSum.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), "GROUP", "Asset", IsCrystalReport);
        //    #endregion

        //    ProgressForm.UpdateProgress(100, "Preparing report for display...");

        //    // Close the dialog if it hasn't been already

        //    if (ProgressForm.InvokeRequired)
        //        ProgressForm.BeginInvoke(new Action(() => ProgressForm.Close()));
        //}
        #endregion
        private void CreateColforTemp()
        {
            dtTemp = new DataTable();
            dtTemp.Columns.Add("RowID", typeof(string));
            dtTemp.Columns.Add("S.No", typeof(string));
            dtTemp.Columns.Add("Account", typeof(string));
            dtTemp.Columns.Add("Amount", typeof(string));
            dtTemp.Columns.Add("AccountID", typeof(string));
            dtTemp.Columns.Add("Type", typeof(string));
            dtTemp.Columns.Add("CategoryID", typeof(string));
        }

        decimal ClosingStock = 0;
        decimal OpeningStock = 0;
        decimal Total = 0;
        DataSet ds;
        DataTable dtData;
        DataTable dtfinal;
        #region datable for standard format
        DataTable dtCA;
        DataTable dtFA;
        DataTable dtOwnerFund;
        DataTable dtLoanFund;
        DataTable dtCL;
        #endregion
        int rowID = 0;
        private void FillGridWithData()
        {
            OpeningStock = 0;
            ClosingStock = 0;
            Total = 0;
            rowID = 0;
            grdBalanceSheet.Visible = false;
            dgBalanceSheet.Visible = true;
            dgBalanceSheet.SelectionMode = SourceGrid.GridSelectionMode.Cell;
            dgBalanceSheet.Selection.EnableMultiSelection = false;
            CreateData();
            #region Data binding
            DataView view = new DataView(dtfinal);
            view.AllowDelete = false;
            view.AllowEdit = false;
            view.AllowNew = false;
            dgBalanceSheet.FixedRows = 1;

            // dgProfitLossAcc.hea
            DevAge.ComponentModel.IBoundList bd = new DevAge.ComponentModel.BoundDataView(view);
            CreateDataGridColumns(dgBalanceSheet.Columns, bd);
            dgBalanceSheet.DataSource = bd;
            dgBalanceSheet.Width = dgBalanceSheet.Width - 5;
            dgBalanceSheet.Width = this.Width - 25;

            #endregion

        }
        DataTable dummy = new DataTable();
        private void CreateData()
        {
            decimal prevYearPnL=Convert.ToDecimal(Settings.GetSettings("PREV_YR_PROFIT"));
            ds = new DataSet();
            dtAssets = new DataTable();
            dtLiabilities = new DataTable();
            dtData = new DataTable();
            dtCrReport = new DataTable();
            #region instantiating standard format datatale
            dtCA = new DataTable();
            dtFA = new DataTable();
            dtCL = new DataTable();
            dtOwnerFund = new DataTable();
            dtLoanFund = new DataTable();
            #endregion

            try
            {
                string fspace = "            ";
                decimal assetTotal = 0;
                decimal liabilityTotal = 0;
                decimal incomeTotal = 0;
                decimal expTotal = 0;
                decimal openBalDr = 0;
                decimal openBalCr = 0;
                decimal netPnL = 0;

             #region variables for standard farmat
                decimal CATotal=0;
                decimal CLTotal=0;
                decimal OwnerFundTotal=0;
                decimal LoanFundTotal=0;
                decimal FATotal=0;
              #endregion

                //git opening and closing stock of products
              dummy=  Product.GetAllProductOpnNCloStock("<AccClassIDSettings><AccClassID>1</AccClassID></AccClassIDSettings>", "<ProjectIDSettings><ProjectID>1</ProjectID></ProjectIDSettings>",null, m_BS.ToDate,0,0,  ref OpeningStock,ref ClosingStock);
                //if (dtStock.Rows.Count > 0)
                //{
                //    foreach (DataRow dr in dtStock.Rows)
                //    {
                //        OpeningStock += (Convert.ToDecimal(dr["OpenPurchaseQty"]) * Convert.ToDecimal(dr["OpenPurchaseRate"]));
                //        ClosingStock += (Convert.ToDecimal(dr["ClosingStock"]) * Convert.ToDecimal(dr["OpenPurchaseRate"]));
                //    }
                //}

                //get all ledger and group balance 
                dtData = Ledger.GetAllGroupNLedgerBal("<AccClassIDSettings><AccClassID>1</AccClassID></AccClassIDSettings>", "<ProjectIDSettings><ProjectID>1</ProjectID></ProjectIDSettings>", null, m_BS.ToDate, 0, null);
                ds.Tables.Add(dtData);
                //add relation between column accountid and parentgroupid
                ds.Relations.Add("ChildRows", ds.Tables[0].Columns["AccountID"], ds.Tables[0].Columns["ParentGroupID"], false);

                foreach (DataRow dr in dtData.Rows)
                {
                    //for Assets account get all of its child group and ledger
                    if (Convert.ToInt32(dr["ParentGroupID"]) == 0 && Convert.ToInt32(dr["AccountID"]) == 1)
                    {
                        rowID = 1;
                        CreateColforTemp();
                        assetTotal = (Convert.ToDecimal(dr["OpenBalDr"]) + Convert.ToDecimal(dr["DebitTotal"])) - (Convert.ToDecimal(dr["OpenBalCr"]) + Convert.ToDecimal(dr["CreditTotal"]));
                        openBalDr += Convert.ToDecimal(dr["OpenBalDr"]);
                        openBalCr += Convert.ToDecimal(dr["OpenBalCr"]);

                         if (m_BS.DispFormat == BalanceSheetSettings.DisplayFormat.Standard)
                        {
                            DataRow[] AChildRows = dr.GetChildRows("ChildRows");
                            foreach(DataRow Arow in AChildRows)
                            {
                                if (Convert.ToInt32(Arow["AccountID"]) == 6 && Arow["AccountType"].ToString() == "GROUP")
                                {
                                    CATotal = (Convert.ToDecimal(Arow["OpenBalDr"]) + Convert.ToDecimal(Arow["DebitTotal"])) - (Convert.ToDecimal(Arow["OpenBalCr"]) + Convert.ToDecimal(Arow["CreditTotal"]));
                                   
                                    dtTemp.Rows.Add(rowID, "-"," Current Assets", " ", Arow["AccountID"].ToString(), Arow["AccountType"].ToString(), "3");
                                    rowID++;
                                    FormatDataInTable(Arow, 1, 0);
                                    if (ClosingStock == 0)
                                    {
                                   if(m_BS.ShowZeroBalance)
                                      {
                                      dtTemp.Rows.Add(rowID.ToString(), "-", fspace + " Closing Stock", ClosingStock.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), " ", "GROUP", "3");
                                      }
                                    }
                                    else
                                    {
                                        dtTemp.Rows.Add(rowID.ToString(), "-", fspace + " Closing Stock", ClosingStock.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), " ", "GROUP", "3");
                                    }
                                        CATotal += ClosingStock;
                                        rowID++;
                                    dtTemp.Rows.Add(rowID.ToString(), "-"," Net Current Assets", CATotal.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), " ", "GROUP", "3");
                                    rowID++;
                                    dtCA = dtTemp.Copy();
                                    //to add  value 3 for categoryID column
                                    dtCA.Columns.Remove("CategoryID");
                                    DataColumn newColumn = new DataColumn("CategoryID", typeof(string));
                                    newColumn.DefaultValue="3";
                                    dtCA.Columns.Add(newColumn);

                                    dtTemp.Clear();
                                }
                                if (Convert.ToInt32(Arow["AccountID"]) == 14 && Arow["AccountType"].ToString() == "GROUP")
                                {
                                    FATotal = (Convert.ToDecimal(Arow["OpenBalDr"]) + Convert.ToDecimal(Arow["DebitTotal"])) - (Convert.ToDecimal(Arow["OpenBalCr"]) + Convert.ToDecimal(Arow["CreditTotal"]));
                                    dtTemp.Rows.Add(rowID, "-", " Fixed Assets", " ", Arow["AccountID"].ToString(), Arow["AccountType"].ToString(), "5");
                                    rowID++;
                                    FormatDataInTable(Arow, 1, 0);
                                    dtTemp.Rows.Add(rowID.ToString(), "-"," Net Fixed Assets", FATotal.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), " ", "GROUP", "5");
                                    rowID++;
                                    dtFA = dtTemp.Copy();
                                    //to add  value 3 for categoryID column
                                    dtFA.Columns.Remove("CategoryID");
                                    DataColumn newColumn = new DataColumn("CategoryID", typeof(string));
                                    newColumn.DefaultValue = "5";
                                    dtFA.Columns.Add(newColumn);
                                    dtTemp.Clear();
                                }
                            }
                        }
                        else
                        {
                            dtTemp.Rows.Add(rowID, "-", dr["AccountName"].ToString(), assetTotal.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), dr["AccountID"].ToString(), dr["AccountType"].ToString(), "2");
                            rowID++;
                            //format all child in tree structure
                            FormatDataInTable(dr, 1, 0);
                            if (ClosingStock == 0)
                            {
                                if (m_BS.ShowZeroBalance)
                                    dtTemp.Rows.Add(rowID.ToString(), "-", "Closing Stock", 0.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), " ", "GROUP", "2");
                            }
                            else
                            {
                                dtTemp.Rows.Add(rowID.ToString(), "-", "Closing Stock", ClosingStock.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), " ", "GROUP", "2");
                            }
                            assetTotal += ClosingStock;
                            rowID++;
                            //fill dtexpenditure with formated expenditure data
                            dtAssets = dtTemp.Copy();
                            dtTemp.Clear();
                        }
                    }
                    //for Liabilities account get all of its child and group and ledger
                    if (Convert.ToInt32(dr["ParentGroupID"]) == 0 && Convert.ToInt32(dr["AccountID"]) == 8)
                    {
                        rowID = 1;
                        CreateColforTemp();
                        rowID++;
                        liabilityTotal = (Convert.ToDecimal(dr["OpenBalCr"]) + Convert.ToDecimal(dr["CreditTotal"])) - (Convert.ToDecimal(dr["OpenBalDr"]) + Convert.ToDecimal(dr["DebitTotal"]));
                        openBalDr += Convert.ToDecimal(dr["OpenBalDr"]);
                        openBalCr += Convert.ToDecimal(dr["OpenBalCr"]);

                        if (m_BS.DispFormat == BalanceSheetSettings.DisplayFormat.Standard)
                        {
                            decimal  authorisedvalue = Convert.ToDecimal(Settings.GetSettings("AUTHORISED_CAPITAL"));
                            decimal issuedvalue = Convert.ToDecimal(Settings.GetSettings("ISSUED_CAPITAL"));

                            DataRow[] AChildRows = dr.GetChildRows("ChildRows");
                            foreach (DataRow Arow in AChildRows)
                            {
                                if (Convert.ToInt32(Arow["AccountID"]) == 27 && Arow["AccountType"].ToString() == "GROUP")
                                {
                                    OwnerFundTotal = (Convert.ToDecimal(Arow["OpenBalCr"]) + Convert.ToDecimal(Arow["CreditTotal"])) - (Convert.ToDecimal(Arow["OpenBalDr"]) + Convert.ToDecimal(Arow["DebitTotal"]));
                                    dtTemp.Rows.Add(rowID, "-"," Owners Funds"," ", Arow["AccountID"].ToString(), Arow["AccountType"].ToString(), "1");
                                    rowID++;
                                    if (authorisedvalue == 0)
                                    {
                                        if (m_BS.ShowZeroBalance)
                                        {
                                            dtTemp.Rows.Add(rowID, "-", fspace + " Athorized Capital", authorisedvalue.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), " ", "GROUP", "1");
                                            rowID++;
                                        }
                                    }
                                    else
                                    {
                                        dtTemp.Rows.Add(rowID, "-", fspace + " Athorized Capital", authorisedvalue.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), " ", "GROUP", "1");
                                        rowID++;
                                   }
                                    OwnerFundTotal += authorisedvalue;
                                    if (issuedvalue == 0)
                                    {
                                        if (m_BS.ShowZeroBalance)
                                        {
                                            dtTemp.Rows.Add(rowID, "-", fspace + " Issued Capital", issuedvalue.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), " ", "GROUP", "1");
                                            rowID++;
                                        }
                                    }
                                    else
                                    {
                                        dtTemp.Rows.Add(rowID, "-", fspace + " Issued Capital", issuedvalue.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), " ", "GROUP", "1");
                                        rowID++;
                                    }
                                        OwnerFundTotal += issuedvalue;                                   
                                    FormatDataInTable(Arow, 1, 1);
                                    dtOwnerFund = dtTemp.Copy();
                                    //to add  value 3 for categoryID column
                                    dtOwnerFund.Columns.Remove("CategoryID");
                                    DataColumn newColumn = new DataColumn("CategoryID", typeof(string));
                                    newColumn.DefaultValue = "1";
                                    dtOwnerFund.Columns.Add(newColumn);
                                    dtTemp.Clear();
                                }
                                else if (Convert.ToInt32(Arow["AccountID"]) == 28 && Arow["AccountType"].ToString() == "GROUP")
                                {
                                    LoanFundTotal = (Convert.ToDecimal(Arow["OpenBalCr"]) + Convert.ToDecimal(Arow["CreditTotal"])) - (Convert.ToDecimal(Arow["OpenBalDr"]) + Convert.ToDecimal(Arow["DebitTotal"]));
                                    dtTemp.Rows.Add(rowID, "-"," Loan Funds"," ", Arow["AccountID"].ToString(), Arow["AccountType"].ToString(), "1");
                                    rowID++;
                                    FormatDataInTable(Arow, 1, 1);
                                    dtLoanFund = dtTemp.Copy();
                                    //to add  value 3 for categoryID column
                                    dtLoanFund.Columns.Remove("CategoryID");
                                    DataColumn newColumn = new DataColumn("CategoryID", typeof(string));
                                    newColumn.DefaultValue = "2";
                                    dtLoanFund.Columns.Add(newColumn);
                                    dtTemp.Clear();
                                }
                                else if (Convert.ToInt32(Arow["AccountID"]) == 118 && Arow["AccountType"].ToString() == "GROUP")
                                {
                                    CLTotal = (Convert.ToDecimal(Arow["OpenBalCr"]) + Convert.ToDecimal(Arow["CreditTotal"])) - (Convert.ToDecimal(Arow["OpenBalDr"]) + Convert.ToDecimal(Arow["DebitTotal"]));
                                    dtTemp.Rows.Add(rowID, "-"," LESS : Current Liabilities"," ", Arow["AccountID"].ToString(), Arow["AccountType"].ToString(), "2");
                                    rowID++;
                                    FormatDataInTable(Arow, 1, 2);
                                    dtTemp.Rows.Add(rowID, "-"," Net Current Liabilities","("+ CLTotal.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces))+")"," ", Arow["AccountType"].ToString(), "2");
                                    rowID++;
                                    dtCL = dtTemp.Copy();
                                    //to add  value 3 for categoryID column
                                    dtCL.Columns.Remove("CategoryID");
                                    DataColumn newColumn = new DataColumn("CategoryID", typeof(string));
                                    newColumn.DefaultValue = "4";
                                    dtCL.Columns.Add(newColumn);
                                    dtTemp.Clear();
                                }
                            }
                        }
                        else
                        {
                            dtTemp.Rows.Add(rowID.ToString(), "-", dr["AccountName"].ToString(), liabilityTotal.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), dr["AccountID"].ToString(), dr["AccountType"].ToString(), "1");
                            rowID++;
                            //format all child in tree structure
                            FormatDataInTable(dr, 1, 1);
                            //fill dtIncome with formated Income data
                            dtLiabilities = dtTemp.Copy();
                            dtTemp.Clear();
                        }
                    }

                    //for calculation net profit or loss
                   if (Convert.ToInt32(dr["ParentGroupID"]) == 0 && Convert.ToInt32(dr["AccountID"]) == 4)//expenditure
                    {
                        expTotal = (Convert.ToDecimal(dr["OpenBalDr"]) + Convert.ToDecimal(dr["DebitTotal"])) - (Convert.ToDecimal(dr["OpenBalCr"]) + Convert.ToDecimal(dr["CreditTotal"]));
                        openBalDr += Convert.ToDecimal(dr["OpenBalDr"]);
                        openBalCr += Convert.ToDecimal(dr["OpenBalCr"]);
                    }
                    if (Convert.ToInt32(dr["ParentGroupID"]) == 0 && Convert.ToInt32(dr["AccountID"]) == 3)//income
                    {
                        incomeTotal = (Convert.ToDecimal(dr["OpenBalCr"]) + Convert.ToDecimal(dr["CreditTotal"])) - (Convert.ToDecimal(dr["OpenBalDr"]) + Convert.ToDecimal(dr["DebitTotal"]));
                        openBalDr += Convert.ToDecimal(dr["OpenBalDr"]);
                        openBalCr += Convert.ToDecimal(dr["OpenBalCr"]);
                    }
                }
                netPnL = (ClosingStock + incomeTotal) - (OpeningStock + expTotal);
                if (m_BS.DispFormat == BalanceSheetSettings.DisplayFormat.Standard)
                {
                    if (netPnL > 0)
                    {
                        dtOwnerFund.Rows.Add(" ", "-",fspace+" Reserve Fund and Accumulated Profit/Profit", netPnL.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), " ", "GROUP", "1");
                    }
                    else if (netPnL < 0)
                    {
                        dtOwnerFund.Rows.Add(" ", "-",fspace+" Reserve Fund and Accumulated Profit/Profit", "(" + (0 - netPnL).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + ")", " ", "GROUP", "1");
                    }
                    else
                    {
                        if(m_BS.ShowZeroBalance)
                            dtOwnerFund.Rows.Add(" ", "-", fspace + " Reserve Fund and Accumulated Profit/Profit", netPnL.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), " ", "GROUP", "1");
                    }
                    OwnerFundTotal += netPnL;

                    dtAssets.Clear();
                    dtLiabilities.Clear();
                    dtAssets = dtCA.Clone();
                    dtAssets.Merge(dtCA);
                    dtAssets.Merge(dtCL);
                    dtAssets.Rows.Add(" ", "-"," NET WORKING CAPITAL", (CATotal - CLTotal).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), " ", "GROUP", "4");
                    dtAssets.Merge(dtFA);
                    dtLiabilities = dtOwnerFund.Clone();
                    dtLiabilities.Merge(dtOwnerFund);
                    dtLiabilities.Merge(dtLoanFund);
                    dtLiabilities.Rows.Add(" ", "-"," TOTAL CAPITAL FUND", (OwnerFundTotal + LoanFundTotal).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), " ", "GROUP", "2");
                    dtAssets.Rows.Add(" ", "-"," TOTAL ASSETS", (CATotal - CLTotal + FATotal).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), " ", "GROUP", "5");

                    dtfinal = dtLiabilities.Clone();
                    dtfinal.Rows.Add(" ", "H", "CAPITAL", " ", " ", " ", " ");
                    dtfinal.Merge(dtLiabilities);
                    //add row for income in grid
                    dtfinal.Rows.Add(" ", "H", "REPRESENTED BY", " ", " ", " ", " ");
                    dtfinal.Merge(dtAssets);//this merge wil simply append income data to the dtfinal with no change in table structure

                }
                else
                {
                    if (netPnL >= 0)
                    {
                        dtLiabilities.Rows.Add(" ", "-", "Net Profit", netPnL.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), " ", "GROUP", "1");
                    }
                    else
                    {
                        dtLiabilities.Rows.Add(" ", "-", "Net Loss", "(" + (0 - netPnL).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + ")", " ", "GROUP", "1");
                    }
                    liabilityTotal += netPnL;

                    if (prevYearPnL >= 0)
                    {
                        dtLiabilities.Rows.Add(" ", "-", " Previous Year P/L (Profit)", prevYearPnL.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), " ", "GROUP", "1");
                    }
                    else
                    {
                        dtLiabilities.Rows.Add(" ", "-", " Previous Year P/L (Loss)", "(" + (0 - prevYearPnL).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + ")", " ", "GROUP", "1");
                    }
                    liabilityTotal += prevYearPnL;

                    if ((openBalCr+prevYearPnL) == (openBalDr + OpeningStock))
                    {
                        if (m_BS.ShowZeroBalance)
                            dtLiabilities.Rows.Add(" ", "-", "Difference in Opening Balance", 0.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), " ", "GROUP", "1");
                    }
                    else if ((openBalDr + OpeningStock) > (openBalCr + prevYearPnL))
                    {
                        dtLiabilities.Rows.Add(" ", "-", "Difference in Opening Balance", ((openBalDr + OpeningStock) - (openBalCr + prevYearPnL)).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), " ", "GROUP", "1");
                        liabilityTotal += ((openBalDr + OpeningStock) - (openBalCr + prevYearPnL));
                    }
                    else
                    {
                        dtAssets.Rows.Add(" ", "-", "Difference in Opening Balance", ((openBalCr + prevYearPnL) - (openBalDr + OpeningStock)).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), " ", "GROUP", "2");
                        assetTotal +=( (openBalCr+prevYearPnL) - (openBalDr + OpeningStock));
                    }

                    dtfinal = dtLiabilities.Clone();
                    if (m_BS.DispFormat == BalanceSheetSettings.DisplayFormat.TFormat)
                    {
                        //for T-Format make sure both tables number of row is equal by adding empty rows if neccessary
                        int Alength = dtAssets.Rows.Count;
                        int Llength = dtLiabilities.Rows.Count;
                        if (Alength > Llength)
                        {
                            for (int i = Llength; i < Alength; i++)
                            {
                                dtLiabilities.Rows.Add(" ", " ", " ", " ", " ", " ", " ");
                            }
                        }
                        else
                        {
                            for (int i = Alength; i < Llength; i++)
                            {
                                dtAssets.Rows.Add(" ", " ", " ", " ", " ", " ", " ");
                            }
                        }
                        //add total row at last of both tables to display total value
                        dtAssets.Rows.Add(" ", "-", "Total", assetTotal.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), " ", "TOTAL", "2");
                        dtLiabilities.Rows.Add(" ", "-", "Total", liabilityTotal.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), " ", "TOTAL", "1");

                        //finally merge data from dtliabilities and dtassets into dtfinal
                        foreach (DataColumn col in dtAssets.Columns)
                        {
                            string newColumnName = col.ColumnName;
                            int colNum = 1;
                            while (dtfinal.Columns.Contains(newColumnName))
                            {
                                newColumnName = string.Format("{0}_{1}", col.ColumnName, ++colNum);
                            }
                            dtfinal.Columns.Add(newColumnName, col.DataType);
                        }
                        var mergedRows = dtLiabilities.AsEnumerable().Zip(dtAssets.AsEnumerable(),
                        (r1, r2) => r1.ItemArray.Concat(r2.ItemArray).ToArray());
                        foreach (object[] rowFields in mergedRows)
                            dtfinal.Rows.Add(rowFields);
                    }
                    else
                    {
                        //add total row at last of both tables to display total value
                        dtAssets.Rows.Add(" ", "-", "Total", assetTotal.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), " ", "TOTAL", "2");
                        dtLiabilities.Rows.Add(" ", "-", "Total", liabilityTotal.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), " ", "TOTAL", "1");
                        //add row for expenditure heading in grid
                        dtfinal.Rows.Add(" ", "H", "LIABILITIES", " ", " ", " ", " ");
                        dtfinal.Merge(dtLiabilities);
                        //add row for income in grid
                        dtfinal.Rows.Add(" ", "H", "ASSETS", " ", " ", " ", " ");
                        dtfinal.Merge(dtAssets);//this merge wil simply append income data to the dtfinal with no change in table structure
                    }
                }
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }

        }

        private void FormatDataInTable(DataRow dr, int level, int flag)//level is for adding space infront of name and flag is for deciding wheather it is debit or credit balance to calculate
        {
            string space = "            ";
            for (int i = 2; i <= level; i++)
            {
                space += "            ";
            }
            decimal balance = 0;
            //get all immediate child rows of current row
            DataRow[] ChildRows = dr.GetChildRows("ChildRows");
            foreach (DataRow childRow in ChildRows)
            {
                balance = 0;
                if (flag == 0)//calculate balance for expenditure i.e. debit side balance
                    balance = (Convert.ToDecimal(childRow["OpenBalDr"]) + Convert.ToDecimal(childRow["DebitTotal"])) - (Convert.ToDecimal(childRow["OpenBalCr"]) + Convert.ToDecimal(childRow["CreditTotal"]));
                else//calculate balance for income i.e. credit side balance
                    balance = (Convert.ToDecimal(childRow["OpenBalCr"]) + Convert.ToDecimal(childRow["CreditTotal"])) - (Convert.ToDecimal(childRow["OpenBalDr"]) + Convert.ToDecimal(childRow["DebitTotal"]));

                if (balance == 0)
                {
                    if (!m_BS.ShowZeroBalance)
                    {
                        continue;
                    }
                }

                if (childRow["AccountType"].ToString() == "GROUP")
                {
                    if (flag == 1)
                        dtTemp.Rows.Add(rowID.ToString(), "-", space + childRow["AccountName"].ToString(), balance.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), childRow["AccountID"].ToString(), childRow["AccountType"].ToString(), "1");
                    else
                        dtTemp.Rows.Add(rowID.ToString(), "-", space + childRow["AccountName"].ToString(), balance.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), childRow["AccountID"].ToString(), childRow["AccountType"].ToString(), "2");

                    rowID++;
                    if (childRow.GetChildRows("ChildRows").Length > 0)//if it has child rows then add childs under the group 
                    {
                        level++;
                        FormatDataInTable(childRow, level, flag);
                        level--;//retain the old value of level for adding rest of the childs

                    }

                }
                else if (childRow["AccountType"].ToString() == "LEDGER")//if ledger
                {
                    if (m_BS.Detail)
                    {
                        if (flag == 1)
                            dtTemp.Rows.Add(rowID.ToString(), "1", space + childRow["AccountName"].ToString(), balance.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), childRow["AccountID"].ToString(), childRow["AccountType"].ToString(), "1");
                        else
                            dtTemp.Rows.Add(rowID.ToString(), "1", space + childRow["AccountName"].ToString(), balance.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), childRow["AccountID"].ToString(), childRow["AccountType"].ToString(), "2");

                        rowID++;
                    }
                }
            }
        }

        private void CreateDataGridColumns(SourceGrid.DataGridColumns columns, DevAge.ComponentModel.IBoundList boundList)
        {
            //text format for column header
            SourceGrid.Cells.Views.ColumnHeader viewColumnHeader = new SourceGrid.Cells.Views.ColumnHeader();
            viewColumnHeader.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleCenter;
            viewColumnHeader.Font = new Font(LangMgr.GetFont().FontFamily, LangMgr.GetFont().Size + 1, FontStyle.Bold);
            viewColumnHeader.BackColor = Color.LightGray;


            //Text format for Ledgers
            SourceGrid.Cells.Views.Cell ldgView = new SourceGrid.Cells.Views.Cell();
            ldgView.Font = new Font(LangMgr.GetFont(), FontStyle.Regular);
            ldgView.ForeColor = Color.Blue;
            // ldgView.BackColor = Color.DarkSeaGreen;

            //text format for ledger amount
            SourceGrid.Cells.Views.Cell amountLdg = new SourceGrid.Cells.Views.Cell();
            amountLdg.Font = new Font("Arial", 8, FontStyle.Regular);
            amountLdg.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;
            amountLdg.ForeColor = Color.Blue;
            //amountLdg.BackColor = Color.DarkSeaGreen;

            //text format for group
            SourceGrid.Cells.Views.Cell boldView = new SourceGrid.Cells.Views.Cell();
            boldView.Font = new Font(LangMgr.GetFont(), FontStyle.Bold);
            //  boldView.BackColor = Color.DarkSeaGreen;

            //text format for group amount
            SourceGrid.Cells.Views.Cell amountBold = new SourceGrid.Cells.Views.Cell();
            amountBold.Font = new Font(LangMgr.GetFont(), FontStyle.Bold);
            amountBold.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;
            // amountBold.BackColor = Color.DarkSeaGreen;

            //text format for header rows of vertical format
            SourceGrid.Cells.Views.Cell HeaderForVFormat = new SourceGrid.Cells.Views.Cell();
            HeaderForVFormat.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleCenter;
            HeaderForVFormat.Font = new Font(LangMgr.GetFont().FontFamily, LangMgr.GetFont().Size + 1, FontStyle.Bold);
            HeaderForVFormat.BackColor = Color.LightGray;

            #region specify condition for all above views
            SourceGrid.Conditions.ConditionView con_HeadViewforVFormat = new SourceGrid.Conditions.ConditionView(HeaderForVFormat);
            con_HeadViewforVFormat.EvaluateFunction = delegate(SourceGrid.DataGridColumn column, int gridRow, object itemRow)
            {
                try
                {
                    DataRowView row = (DataRowView)itemRow;
                    return (string)row["S.No"] == "H";

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return false;
                }
            };

            SourceGrid.Conditions.ConditionView con_LdgView = new SourceGrid.Conditions.ConditionView(ldgView);
            con_LdgView.EvaluateFunction = delegate(SourceGrid.DataGridColumn column, int gridRow, object itemRow)
            {
                try
                {
                    int i = column.Index;
                    DataRowView row = (DataRowView)itemRow;
                    if (i <= 5)
                    {
                        return (string)row["S.No"] != "-";
                    }
                    else
                    {
                        return (string)row["S.No_2"] != "-";
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return false;
                }
            };

            SourceGrid.Conditions.ConditionView con_secondHead = new SourceGrid.Conditions.ConditionView(viewColumnHeader);
            con_secondHead.EvaluateFunction = delegate(SourceGrid.DataGridColumn column, int gridRow, object itemRow)
            {
                try
                {
                    int i = column.Index;
                    DataRowView row = (DataRowView)itemRow;
                    if (i <= 5)
                    {
                        return (string)row["S.No"] == "---";
                    }
                    else
                    {
                        return (string)row["S.No_2"] == "---";
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return false;
                }
            };

            SourceGrid.Conditions.ConditionView con_amountLdg = new SourceGrid.Conditions.ConditionView(amountLdg);
            con_amountLdg.EvaluateFunction = delegate(SourceGrid.DataGridColumn column, int gridRow, object itemRow)
            {
                try
                {
                    int i = column.Index;
                    DataRowView row = (DataRowView)itemRow;
                    if (i <= 5)
                    {
                        return (string)row["S.No"] != "-";
                    }
                    else
                    {
                        return (string)row["S.No_2"] != "-";
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return false;
                }
            };

            // SourceGrid.Conditions.ConditionBuilder test = new SourceGrid.Conditions.ConditionBuilder();

            SourceGrid.Conditions.ConditionCell val = new SourceGrid.Conditions.ConditionCell(new SourceGrid.Cells.Virtual.CellVirtual());
            val.Cell.Model.AddModel(new SourceGrid.Cells.Models.ValueModel(" "));
            // val.Cell.View.BackColor = Color.Gray;
            val.EvaluateFunction = delegate(SourceGrid.DataGridColumn column, int gridRow, object itemRow)
            {
                int i = column.Index;
                DataRowView row = (DataRowView)itemRow;
                if (i <= 5)
                    if ((string)row["Account"] == " " || (string)row["S.No"] == "H")
                        return true;
                    else
                        return false;

                else
                    return (string)row["Account_2"] == " ";
            };

            SourceGrid.Conditions.ConditionView con_BoldView = new SourceGrid.Conditions.ConditionView(boldView);
            con_BoldView.EvaluateFunction = delegate(SourceGrid.DataGridColumn column, int gridRow, object itemRow)
            {
                try
                {
                    int i = column.Index;
                    DataRowView row = (DataRowView)itemRow;
                    if (i <= 5)
                    {
                        return (string)row["S.No"] == "-";
                    }
                    else
                    {
                        return (string)row["S.No_2"] == "-";
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return false;
                }
            };

            SourceGrid.Conditions.ConditionView con_amountBold = new SourceGrid.Conditions.ConditionView(amountBold);
            con_amountBold.EvaluateFunction = delegate(SourceGrid.DataGridColumn column, int gridRow, object itemRow)
            {
                try
                {
                    int i = column.Index;
                    DataRowView row = (DataRowView)itemRow;
                    if (i <= 5)
                    {
                        return (string)row["S.No"] == "-";
                    }
                    else
                    {
                        return (string)row["S.No_2"] == "-";
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return false;
                }
            };

            #endregion

            SourceGrid.DataGridColumn gridColumn;

            gridColumn = dgBalanceSheet.Columns.Add("RowID", "RowID", new SourceGrid.Cells.DataGrid.Cell());
            gridColumn.HeaderCell.View = viewColumnHeader;
            gridColumn.Width = 50;
            gridColumn.Visible = false;

            gridColumn = dgBalanceSheet.Columns.Add("S.No", "S.No", new SourceGrid.Cells.DataGrid.Cell());
            gridColumn.HeaderCell.View = viewColumnHeader;
            gridColumn.Width = 50;
            gridColumn.Visible = false;

            gridColumn = dgBalanceSheet.Columns.Add("Account", "Account Name", new SourceGrid.Cells.DataGrid.Cell());
            gridColumn.HeaderCell.View = viewColumnHeader;
            gridColumn.Conditions.Add(con_HeadViewforVFormat);
            gridColumn.DataCell.AddController(dblClick);
            gridColumn.Conditions.Add(con_LdgView);
            gridColumn.Conditions.Add(con_BoldView);
            gridColumn.Width = 400;
            gridColumn.AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize | SourceGrid.AutoSizeMode.EnableStretch;

            gridColumn = dgBalanceSheet.Columns.Add("Amount", "Amount", new SourceGrid.Cells.DataGrid.Cell());
            gridColumn.HeaderCell.View = viewColumnHeader;
            gridColumn.DataCell.AddController(dblClick);
            gridColumn.Conditions.Add(con_HeadViewforVFormat);
            gridColumn.Conditions.Add(con_amountLdg);
            gridColumn.Conditions.Add(con_amountBold);
            gridColumn.Width = 150;
            gridColumn.AutoSizeMode = SourceGrid.AutoSizeMode.None;

            gridColumn = dgBalanceSheet.Columns.Add("AccountID", "AccountID", new SourceGrid.Cells.DataGrid.Cell());
            gridColumn.HeaderCell.View = viewColumnHeader;
            gridColumn.Width = 50;
            gridColumn.Visible = false;

            gridColumn = dgBalanceSheet.Columns.Add("Type", "Type", new SourceGrid.Cells.DataGrid.Cell());
            gridColumn.HeaderCell.View = viewColumnHeader;
            gridColumn.Width = 50;
            gridColumn.Visible = false;

            gridColumn = dgBalanceSheet.Columns.Add("CategoryID", "CategoryID", new SourceGrid.Cells.DataGrid.Cell());
            gridColumn.HeaderCell.View = viewColumnHeader;
            gridColumn.Width = 50;
            gridColumn.Visible = false;

            //add rows for T-Format
            if (m_BS.DispFormat == BalanceSheetSettings.DisplayFormat.TFormat)
            {
                gridColumn = dgBalanceSheet.Columns.Add("RowID_2", "RowID", new SourceGrid.Cells.DataGrid.Cell());
                gridColumn.HeaderCell.View = viewColumnHeader;
                gridColumn.Width = 50;
                gridColumn.Visible = false;

                gridColumn = dgBalanceSheet.Columns.Add("S.No_2", "S.No", new SourceGrid.Cells.DataGrid.Cell());
                gridColumn.HeaderCell.View = viewColumnHeader;
                gridColumn.Width = 50;
                gridColumn.Visible = false;

                gridColumn = dgBalanceSheet.Columns.Add("Account_2", "Account Name", new SourceGrid.Cells.DataGrid.Cell());
                gridColumn.HeaderCell.View = viewColumnHeader;
                gridColumn.DataCell.AddController(dblClick);
                gridColumn.Conditions.Add(con_secondHead);
                gridColumn.Conditions.Add(con_LdgView);
                gridColumn.Conditions.Add(con_BoldView);
                gridColumn.Width = 400;
                gridColumn.AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize | SourceGrid.AutoSizeMode.EnableStretch;

                gridColumn = dgBalanceSheet.Columns.Add("Amount_2", "Amount", new SourceGrid.Cells.DataGrid.Cell());
                gridColumn.HeaderCell.View = viewColumnHeader;
                gridColumn.DataCell.AddController(dblClick);
                gridColumn.Conditions.Add(con_secondHead);
                gridColumn.Conditions.Add(con_amountLdg);
                gridColumn.Conditions.Add(con_amountBold);
                gridColumn.Width = 150;
                gridColumn.AutoSizeMode = SourceGrid.AutoSizeMode.None;


                gridColumn = dgBalanceSheet.Columns.Add("AccountID_2", "AccountID", new SourceGrid.Cells.DataGrid.Cell());
                gridColumn.HeaderCell.View = viewColumnHeader;
                gridColumn.Width = 50;
                gridColumn.Visible = false;

                gridColumn = dgBalanceSheet.Columns.Add("Type_2", "Type", new SourceGrid.Cells.DataGrid.Cell());
                gridColumn.HeaderCell.View = viewColumnHeader;
                gridColumn.Width = 50;
                gridColumn.Visible = false;

                gridColumn = dgBalanceSheet.Columns.Add("CategoryID", "CategoryID", new SourceGrid.Cells.DataGrid.Cell());
                gridColumn.HeaderCell.View = viewColumnHeader;
                gridColumn.Width = 50;
                gridColumn.Visible = false;

            }

            dgBalanceSheet.AutoStretchColumnsToFitWidth = true;
        }


        private void frmBalanceSheet_Load(object sender, EventArgs e)
        {
            DisplayBannar();
            string AccClassIDsXMLString = ReadAllAccClassID();
            string ProjectIDsXMLString = ReadAllProjectID();
            dblClick = new SourceGrid.Cells.Controllers.CustomEvents();
            dblClick.DoubleClick += new EventHandler(grd_BalanceSheetDoubleClick);
            dblClick.KeyUp += new KeyEventHandler(dgBalanceSheet_KeyUp);

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
            FillGridWithData();
            dgBalanceSheet.EnableSort = false; 
            ProgressForm.UpdateProgress(100, "Preparing report for display...");
            if (ProgressForm.InvokeRequired)
                ProgressForm.BeginInvoke(new Action(() => ProgressForm.Close()));
            this.WindowState = FormWindowState.Minimized;
            this.WindowState = FormWindowState.Maximized;

            #region Old Code

            //#region BLOCK FOR T-FORMAT
            //if (m_BS.DispFormat== BalanceSheetSettings.DisplayFormat.TFormat)
            //{
            //    AdditionalExpences = 0;
            //    frmProgress ProgressForm = new frmProgress();
            //    // Initialize the thread that will handle the background process
            //    Thread backgroundThread = new Thread(
            //        new ThreadStart(() =>
            //        {
            //            ProgressForm.ShowDialog();
            //        }
            //    ));

            //    backgroundThread.Start();
            //    ProgressForm.UpdateProgress(20, "Initializing report...");

            //    LiabilitiesRowsCount = 2; //Since headers are already there in two rows
            //    AssetRowsCount = 2;
            //    #region BLOCK FOR ORIENTATION PURPOSE FOR ASSET AND LIABILITIES
            //    double AsssetOpBalDifference, LiabilitiesOpBalDifference;
            //    AsssetOpBalDifference = LiabilitiesOpBalDifference = 0;

            //    AccountGroup AccountGroup = new AccountGroup();
            //    Transaction Transaction = new Transaction();

            //    double AssetSum, LiabilitiesSum;
            //    AssetSum = LiabilitiesSum = 0;

            //    double assetbal, liabilitiesbal;
            //    assetbal = liabilitiesbal = 0;

            //    //Text format for Total
            //    HeaderView = new SourceGrid.Cells.Views.Cell();
            //    HeaderView.Font = new Font(LangMgr.GetFont(), FontStyle.Bold);

            //    //Text format for Ledgers
            //    LedgerView = new SourceGrid.Cells.Views.Cell();
            //    LedgerView.Font = new Font(LangMgr.GetFont(), FontStyle.Italic);
            //    LedgerView.ForeColor = Color.Blue;

            //    //Text format for SubGroup
            //    SubGroupView = new SourceGrid.Cells.Views.Cell();
            //    SubGroupView.Font = new Font(LangMgr.GetFont(), FontStyle.Regular);
            //    //Add a double click handler. When user dblclicks the cell, will use this function to send the parent form the Currency code

            //    // double click for Asset

            //    dblClick = new SourceGrid.Cells.Controllers.CustomEvents();
            //    dblClick.DoubleClick += new EventHandler(grd_BalanceSheetDoubleClick);

            //    //Let the whole row to be selected
            //    //grdBalanceSheet.SelectionMode = SourceGrid.GridSelectionMode.Row;



            //    //Disable multiple selection

            //    grdBalanceSheet.Selection.EnableMultiSelection = false;

            //    grdBalanceSheet.Redim(2, 10);

            //    grdBalanceSheet.FixedRows = 2;
            //    #endregion

            //    MakeHeader();
            //    #region ASSETS COLUMN PROCESSING
            //    //AccountGroup processing starts for Asset
            //    DataTable dt = AccountGroup.GetGroupTable(AccountGroup.GetIDFromType(AccountType.Assets));//GroupID contain 1 for Asset
            //    int Sno = 1;
            //    foreach (DataRow dr in dt.Rows)
            //    {
            //        double m_dbal = 0;
            //        double m_cbal = 0;
            //        // Block for DateTime range selection
            //        if (m_BS.HasDateRange == true)//When datetime is selected
            //            Transaction.GetGroupBalance(m_BS.FromDate, m_BS.ToDate, Convert.ToInt32(dr["GroupID"]), true, ref m_dbal, ref m_cbal, m_BS.AccClassID, m_BS.ProjectID);
            //        else//Otherwise
            //            Transaction.GetGroupBalance(null,null,Convert.ToInt32(dr["GroupID"]), true, ref m_dbal, ref m_cbal, m_BS.AccClassID, m_BS.ProjectID);

            //        if (m_BS.ShowZeroBalance == false && m_dbal == 0 && m_cbal == 0) //In case of zero balance
            //        {
            //            //Do nothing
            //        }
            //        else
            //        {
            //            string EngName = AccountGroup.GetEngName(Convert.ToInt32(dr["GroupID"]).ToString());  //calling this funtion for getting EngName from tblGroup corresponding to GroupID having corresponding GroupID
            //            double Balance = (m_dbal - m_cbal);//For Asset[Debit Balance - Credit Balance]
            //            WriteAssets(Sno, EngName, Convert.ToInt32(dr["GroupID"]), Balance.ToString(), "GROUP");
            //            AssetSum += Balance;
            //            //If details is selected, show details i.e. ledgers present inside
            //            if (m_BS.Detail == true)
            //                WriteDetails(Convert.ToInt32(dr["GroupID"]), AccountType.Assets,false);

            //            //If Second level group is selected, show them
            //            if (m_BS.ShowSecondLevelDtl == true)
            //                WriteSecondLevel(Convert.ToInt32(dr["GroupID"]), AccountType.Assets,false);

            //        }//End of zero balance check

            //        Sno++;
            //    }//End for loop

            //    //Ledgers just beneath the Assets
            //    WriteLedger(AccountGroup.GetIDFromType(AccountType.Assets), AccountType.Assets, ref assetbal, ref liabilitiesbal, true,false);//Here we have to write on grid soo make it true
            //    AssetSum += assetbal;//adding assetbal to Total Asset
            //    #endregion

            //    #region Closing Stock Value
            //    //Closing Stock Values Check For Closing Stock Quantity and Make Closing Stock Amount
            //    double ProductValue = 0;
            //    Product allproduct = new Product();
            //    DataTable dtGetAllProduct = allproduct.getProductByGroupID();
            //    string AccClassIDsXMLString = ReadAllAccClassID();
            //    string ProjectIDsXMLString = ReadAllProjectID();
            //    int closingQuantity = 0;

            //    foreach (DataRow drProduct in dtGetAllProduct.Rows)
            //    {
            //        bool isTrans = false;
            //        if (drProduct["IsInventoryApplicable"].ToString() == "1")
            //        {
            //            DataTable dtOpeningStockStatusInfo = StockStatusBook.GetOpeningStockStatusBook(null, Convert.ToInt32(drProduct["ProductID"].ToString()), " ", m_BS.ToDate, true, StockStatusType.OpeningStock, AccClassIDsXMLString);
            //            DataTable dtTransactionStockStatusInfo = StockStatusBook.GetStockStatusBook1(null, Convert.ToInt32(drProduct["ProductID"].ToString()), "", m_BS.ToDate, true, StockStatusType.ClosingStock, AccClassIDsXMLString, ProjectIDsXMLString);
            //            if (dtTransactionStockStatusInfo.Rows.Count != 0)
            //            {
            //                foreach (DataRow drOpeningStockStatusInfo in dtOpeningStockStatusInfo.Rows)
            //                {
            //                    // if (dtTransactionStockStatusInfo.Rows.Count != 0)
            //                    //{
            //                    foreach (DataRow drTransactionStockStatusInfo in dtTransactionStockStatusInfo.Rows)
            //                    {
            //                        if (Convert.ToInt32(drTransactionStockStatusInfo["ProductID"]) == Convert.ToInt32(drOpeningStockStatusInfo["ProductID"]))
            //                        {
            //                            isTrans = true;
            //                            closingQuantity = Convert.ToInt32(drTransactionStockStatusInfo["Quantity"]) + Convert.ToInt32(drOpeningStockStatusInfo["Quantity"]);
            //                            double ProdPrice = Product.GetProductPurchasePrice(Convert.ToInt32(drTransactionStockStatusInfo["ProductID"]), Global.ParentAccClassID);
            //                           // double AddtionalCost = Product.GetFreightandCustomDuty(Convert.ToInt32(drTransactionStockStatusInfo["rowid"]));
            //                            ProductValue += ProdPrice * closingQuantity;
            //                           // ProductValue += AddtionalCost;
            //                           // AdditionalExpences = AdditionalExpences + AddtionalCost;
            //                        }

            //                    }
            //                    if (isTrans == false)
            //                    {
            //                        closingQuantity = Convert.ToInt32(drOpeningStockStatusInfo["Quantity"]);
            //                        double ProdPrice = Product.GetProductPurchasePrice(Convert.ToInt32(drOpeningStockStatusInfo["ProductID"]), Global.ParentAccClassID);
            //                        ProductValue += ProdPrice * closingQuantity;
            //                    }

            //                }

            //            }
            //            else
            //            {
            //                DataTable dtOpeningStockStatusInfo1 = StockStatusBook.GetOpeningStockStatusBook(null, Convert.ToInt32(drProduct["ProductID"].ToString()), " ", m_BS.ToDate, true, StockStatusType.OpeningStock, AccClassIDsXMLString);
            //                if (dtOpeningStockStatusInfo1.Rows.Count > 0) 
            //                {
            //                    DataRow dropen = dtOpeningStockStatusInfo1.Rows[0];
            //                    closingQuantity = Convert.ToInt32(dropen["Quantity"].ToString());
            //                    double ProdPrice = Product.GetProductPurchasePrice(Convert.ToInt32(drProduct["ProductID"].ToString()), Global.ParentAccClassID);
            //                    ProductValue += ProdPrice * closingQuantity;
            //                }
            //            }
            //        }

            //    }
            //    WriteAssets(Sno, "Closing Stock", 0, ProductValue.ToString(), "GROUP");
            //   // WriteVerticalAsset(Sno, "Closing Stock", 0, ProductValue.ToString(), "GROUP", "Asset", IsCrystalReport);
            //    AssetSum += ProductValue;
            //    #endregion


            //    #region LIABILITES COLUMN PROCESSING
            //    DataTable dtLiab = AccountGroup.GetGroupTable(AccountGroup.GetIDFromType(AccountType.Liabilities));//GroupID contain 1 for Asset
            //    int SnoLiab = 1;
            //    foreach (DataRow dr in dtLiab.Rows)
            //    {
            //        double m_dbal = 0;
            //        double m_cbal = 0;
            //        // Block for DateTime range selection
            //        if (m_BS.HasDateRange == true)//When datetime is selected
            //            Transaction.GetGroupBalance(m_BS.FromDate, m_BS.ToDate, Convert.ToInt32(dr["GroupID"]), true, ref m_dbal, ref m_cbal, m_BS.AccClassID, m_BS.ProjectID);
            //        else//Otherwise
            //            Transaction.GetGroupBalance(null,null,Convert.ToInt32(dr["GroupID"]), true, ref m_dbal, ref m_cbal, m_BS.AccClassID, m_BS.ProjectID);

            //        if (m_BS.ShowZeroBalance == false && m_dbal == 0 && m_cbal == 0) //In case of zero balance
            //        {
            //            //Do nothing
            //        }
            //        else
            //        {
            //            string EngName = AccountGroup.GetEngName(Convert.ToInt32(dr["GroupID"]).ToString());  //calling this funtion for getting EngName from tblGroup corresponding to GroupID having corresponding GroupID
            //            double Balance = (m_cbal - m_dbal);//For Liabilites remember it is always credit soo (credit-debit)
            //            WriteLiabilities(SnoLiab, EngName, Convert.ToInt32(dr["GroupID"]), Balance.ToString(), "GROUP");
            //            LiabilitiesSum += Balance;
            //            //If details is selected, show details i.e. ledgers present inside
            //            if (m_BS.Detail == true)
            //                WriteDetails(Convert.ToInt32(dr["GroupID"]), AccountType.Liabilities,false);

            //            //If Second level group is selected, show them
            //            if (m_BS.ShowSecondLevelDtl == true)
            //                WriteSecondLevel(Convert.ToInt32(dr["GroupID"]), AccountType.Liabilities,false);

            //        }//End of zero balance check

            //        SnoLiab++;
            //    }//End for loop

            //    //Display the ledger just under the account
            //    WriteLedger(AccountGroup.GetIDFromType(AccountType.Liabilities), AccountType.Liabilities,ref assetbal,ref liabilitiesbal,true,false);
            //    LiabilitiesSum += liabilitiesbal;
            //    #endregion

            //    ProgressForm.UpdateProgress(40, "Calculating difference in opening balance...");

            //    #region DIFFERENCE IN OPENENING BALANCE
            //    //Display Difference in Opening Balance
            //    double closingQuantity1 = 0;
            //    double productvalue1 = 0;
            //    foreach (DataRow drProduct in dtGetAllProduct.Rows)//For Removing the Opening Different
            //    {
            //        DataTable dtOpeningStockStatusInfo1 = StockStatusBook.GetOpeningStockStatusBook(null, Convert.ToInt32(drProduct["ProductID"].ToString()), " ", m_BS.ToDate, true, StockStatusType.OpeningStock, AccClassIDsXMLString);
            //        if (dtOpeningStockStatusInfo1.Rows.Count > 0)
            //        {
            //            DataRow dropen = dtOpeningStockStatusInfo1.Rows[0];
            //            closingQuantity1 = Convert.ToInt32(dropen["Quantity"].ToString());
            //            double ProdPrice = Product.GetProductPurchasePrice(Convert.ToInt32(drProduct["ProductID"].ToString()), Global.ParentAccClassID);
            //            productvalue1 += ProdPrice * closingQuantity1;
            //        }
            //    }
            //    double OpBalAssets = GetOpeningBalanceSummary(GetRootAccClassID(), AccountType.Assets);
            //    if (OpBalAssets > 0)//If asset opening balance is greater than Liabilites opening balance then post this difference value to Liabilities side
            //    {

            //        LiabilitiesSum += OpBalAssets;

            //        WriteLiabilities(0, "Difference in Opening Balance", 0, OpBalAssets.ToString(), "GROUP");
            //    }
            //    if (OpBalAssets+productvalue1 < 0)//If asset opening balance is greater than Liabilites opening balance then post this difference value to Liabilities side
            //    {
            //        //double OpBalDiff = OpBalLiab - OpBalAssets;
            //        double oPBalDiff = (-1) * OpBalAssets;
            //        AssetSum += oPBalDiff;
            //        WriteAssets(0, "Difference in Opening Balance: ", 0, oPBalDiff.ToString(), "GROUP");
            //        //WriteVerticalAsset(0, "Difference in Opening Balance: ", 0, oPBalDiff.ToString(), "GROUP", "Asset", IsCrystalReport);
            //    }

            //    //Display Difference in Opening Balance
            //    //double OpBalLiab = GetOpeningBalanceSummary(GetRootAccClassID(), AccountType.Liabilities);

            //    //if (OpBalAssets > OpBalLiab)
            //    //{
            //    //    double OpBalDiff = OpBalAssets - OpBalLiab;

            //    //    LiabilitiesSum += OpBalDiff;

            //    //    WriteLiabilities(0, "Difference in Opening Balance", 0, OpBalDiff.ToString(), "GROUP");

            //    //}
            //    //else if (OpBalAssets < OpBalLiab)
            //    //{
            //    //    double OpBalDiff = OpBalLiab - OpBalAssets;
            //    //    AssetSum += OpBalDiff;
            //    //    WriteAssets(0, "Difference in Opening Balance", 0, OpBalDiff.ToString(), "GROUP");

            //    //}
            //    #endregion

            //    #region BLOCK FOR ADDING EITHER PROFIT OR LOSS OBTAINING FROM BALANCESHEET
            //    //This is the block for adding either profit or loss obtaining from Profit and loass account
            //    //Calling the method for finding either profit or loss from P/L account
            //    frmProfitLossAcc m_profitLoss = new frmProfitLossAcc();
            //    double TotalProfit, TotalLoss;
            //    TotalProfit = TotalLoss = 0;
            //    ProfitLossAccSettings m_ProfitLoassSettings = new ProfitLossAccSettings();
            //    int IncomeGrpID = AccountGroup.GetIDFromType(AccountType.Income);
            //    int ExpenditureGrpID = AccountGroup.GetIDFromType(AccountType.Expenditure);
            //    m_profitLoss.Cal_TotalProfitLoss(ref TotalProfit, ref TotalLoss, IncomeGrpID, ExpenditureGrpID, m_BS.HasDateRange, m_BS.FromDate, m_BS.ToDate, m_BS.AccClassID, m_BS.ShowZeroBalance,false,m_BS.ProjectID);
            //    if (TotalProfit > 0)//Incase of Profit from p/l account
            //    { 
            //        //If there is Profit then add this profit balance to Liabilities side for balancing puropose
            //        //WriteLiabilities(0, "Total Profit: ", 0, TotalProfit.ToString(), "GROUP");
            //        WriteLiabilities(0, "Reserve Fund and Accumulated Profit/Loss: ", 0, TotalProfit.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), "GROUP");
            //        LiabilitiesSum += TotalProfit;

            //    }
            //    //else if (Convert.ToDouble( TotalLoss.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces))) -Convert.ToDouble( AdditionalExpences.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces))) > 0)//Incase of Loss
            //    else if(TotalLoss>0)
            //    {
            //        //If there is loss then add this Loss balance to Asset side
            //        //WriteAssets(0, "Total Loss: ", 0, TotalLoss.ToString(), "GROUP");
            //        WriteLiabilities(0, "Reserve Fund and Accumulated Profit/Loss: ", 0, "-" + TotalLoss.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), "GROUP");
            //        LiabilitiesSum -= TotalLoss;

            //    }
            //    #endregion



            //    ProgressForm.UpdateProgress(80, "Calculating  profit or loss...");

            //    #region BALANCE ASSETS AND LIABILITIES ROWS FOR BLANK CELLS
            //    //Balance the assets and liabilites rows using blank cells. Assets and Liab rows may not be same. So need to insert blank cells
            //    if (AssetRowsCount > LiabilitiesRowsCount)
            //    {
            //        while (AssetRowsCount > LiabilitiesRowsCount)
            //        {
            //            grdBalanceSheet[LiabilitiesRowsCount, 5] = new SourceGrid.Cells.Cell("");
            //            grdBalanceSheet[LiabilitiesRowsCount, 6] = new SourceGrid.Cells.Cell("");
            //            grdBalanceSheet[LiabilitiesRowsCount, 7] = new SourceGrid.Cells.Cell("");
            //            grdBalanceSheet[LiabilitiesRowsCount, 8] = new SourceGrid.Cells.Cell("");
            //            grdBalanceSheet[LiabilitiesRowsCount, 9] = new SourceGrid.Cells.Cell("");
            //            LiabilitiesRowsCount++;
            //        }
            //    }
            //    else if (LiabilitiesRowsCount > AssetRowsCount)
            //    {
            //        while (LiabilitiesRowsCount > AssetRowsCount)
            //        {
            //            grdBalanceSheet[AssetRowsCount, 0] = new SourceGrid.Cells.Cell("");
            //            grdBalanceSheet[AssetRowsCount, 1] = new SourceGrid.Cells.Cell("");
            //            grdBalanceSheet[AssetRowsCount, 2] = new SourceGrid.Cells.Cell("");
            //            grdBalanceSheet[AssetRowsCount, 3] = new SourceGrid.Cells.Cell("");
            //            grdBalanceSheet[AssetRowsCount, 4] = new SourceGrid.Cells.Cell("");
            //            AssetRowsCount++;
            //        }
            //    }
            //    #endregion

            //    #region BLOCK FOR TOTAL AMOUNT CALCULATION FOR ASSET AND LIABILITIES
            //    WriteAssets(0, "ASSETS TOTAL", 0, AssetSum.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), "GROUP");
            //    WriteLiabilities(0, "LIABILITIES TOTAL", 0, LiabilitiesSum.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), "GROUP");
            //    #endregion

            //    ProgressForm.UpdateProgress(100, "Preparing report for display...");

            //    // Close the dialog if it hasn't been already

            //    if (ProgressForm.InvokeRequired)
            //        ProgressForm.BeginInvoke(new Action(() => ProgressForm.Close()));
            //}
            //#endregion

            //#region BLOCK FOR VERTICAL FORMAT
            //else if (m_BS.DispFormat== BalanceSheetSettings.DisplayFormat.Vertical)
            //{
            //    DisplayVerticalBalanceSheet(false);
            //}
            //#endregion
            #endregion

        }//End function

        private void DisplayBannar()
        {

            if (m_BS.HasDateRange)
            {
                lblAsonDate.Text = "As on Date: " + Date.ToSystem((DateTime)m_BS.ToDate);
            }
            else//if date range is not selected then siimply pass the current date time
            {
                lblAsonDate.Text = "As on Date: " + Date.ToSystem(DateTime.Today);
            }

            CompanyDetails m_CmpDtl = CompanyInfo.GetInfo();
            if (m_CmpDtl.FYFrom != null)
                // lblAllSettings.Text = "Fiscal Year: " + Convert.ToDateTime(m_CmpDtl.FYFrom).ToShortDateString();
                //lblAllSettings.Text = "Fiscal Year: " + Date.ToSystem(m_CmpDtl.FYFrom);
                lblAllSettings.Text = "Fiscal Year: " + m_CmpDtl.FiscalYear;

            DataTable dtProjectInfo = Project.GetProjectByID(Convert.ToInt32(m_BS.ProjectID), LangMgr.DefaultLanguage);
            if (m_BS.ProjectID != 0)
            {
                DataRow drProjectInfo = dtProjectInfo.Rows[0];
                lblProjectName.Text = " Project: " + drProjectInfo["Name"].ToString();
            }
            else
            {
                lblProjectName.Text = " Project: " + "All";
            }
        }
        private void dgBalanceSheet_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                grd_BalanceSheetDoubleClick(sender, null);
        }

        private void grd_BalanceSheetDoubleClick(object sender, EventArgs e)
        {
            int curColumn = dgBalanceSheet.Selection.GetSelectionRegion().GetColumnsIndex()[0];
            int curRow = dgBalanceSheet.Selection.GetSelectionRegion().GetRowsIndex()[0];
            string Type = " ";
            string ID = " ";
            if (curColumn <= 6)
            {
                SourceGrid.CellContext cellType = new SourceGrid.CellContext(dgBalanceSheet, new SourceGrid.Position(curRow, 5));
                SourceGrid.CellContext cellTypeID = new SourceGrid.CellContext(dgBalanceSheet, new SourceGrid.Position(curRow, 4));
                Type = cellType.Value.ToString();
                ID = cellTypeID.Value.ToString();
            }
            else
            {
                SourceGrid.CellContext cellType = new SourceGrid.CellContext(dgBalanceSheet, new SourceGrid.Position(curRow, 12));
                SourceGrid.CellContext cellTypeID = new SourceGrid.CellContext(dgBalanceSheet, new SourceGrid.Position(curRow, 11));
                Type = cellType.Value.ToString();
                ID = cellTypeID.Value.ToString();

            }
            if (Type == " " || ID == " ")
                return;
            #region Old Code
            //int curColumn = grdBalanceSheet.Selection.GetSelectionRegion().GetColumnsIndex()[0];
            //int curRow = grdBalanceSheet.Selection.GetSelectionRegion().GetRowsIndex()[0];

            //string Type = "";
            //int groupid, ledgerid;
            //groupid = ledgerid = 0;
            //if (m_BS.DispFormat== BalanceSheetSettings.DisplayFormat.Vertical)//for vertical Balancesheet           
            //{
            //    SourceGrid.CellContext cellType = new SourceGrid.CellContext(grdBalanceSheet, new SourceGrid.Position(curRow, 4));
            //    Type = (cellType.Value).ToString();
            //    if (Type == "GROUP")
            //    {
            //        SourceGrid.CellContext cellid = new SourceGrid.CellContext(grdBalanceSheet, new SourceGrid.Position(curRow, 3));
            //        groupid = Convert.ToInt32(cellid.Value);
            //    }
            //    else if (Type == "LEDGER")
            //    {
            //        SourceGrid.CellContext cellid = new SourceGrid.CellContext(grdBalanceSheet, new SourceGrid.Position(curRow, 3));
            //        ledgerid = Convert.ToInt32(cellid.Value);
            //    }

            //}
            //else if (m_BS.DispFormat== BalanceSheetSettings.DisplayFormat.TFormat)//For T- format Balancesheet
            //{
            //    //find colulmn position at first

            //    if (curColumn <= 4)//This is the portion of Asset side
            //    {
            //        SourceGrid.CellContext cellType = new SourceGrid.CellContext(grdBalanceSheet, new SourceGrid.Position(curRow, 4));
            //         Type = (cellType.Value).ToString();
            //         if (Type == "GROUP")
            //        {
            //            SourceGrid.CellContext cellid = new SourceGrid.CellContext(grdBalanceSheet, new SourceGrid.Position(curRow, 3));
            //            groupid = Convert.ToInt32(cellid.Value);
            //        }
            //         else if (Type == "LEDGER")
            //        {
            //            SourceGrid.CellContext cellid = new SourceGrid.CellContext(grdBalanceSheet, new SourceGrid.Position(curRow, 3));
            //            ledgerid = Convert.ToInt32(cellid.Value);
            //        }                                     
            //    }
            //    else if (curColumn >= 5)//This is the portion of Liabilities
            //    {
            //        SourceGrid.CellContext cellType = new SourceGrid.CellContext(grdBalanceSheet, new SourceGrid.Position(curRow, 9));
            //        Type = (cellType.Value).ToString();
            //        if (Type == "GROUP")
            //        {
            //            SourceGrid.CellContext cellid = new SourceGrid.CellContext(grdBalanceSheet, new SourceGrid.Position(curRow, 8));
            //            groupid = Convert.ToInt32(cellid.Value);
            //        }
            //        else if (Type == "LEDGER")
            //        {
            //            SourceGrid.CellContext cellid = new SourceGrid.CellContext(grdBalanceSheet, new SourceGrid.Position(curRow, 8));
            //            ledgerid = Convert.ToInt32(cellid.Value);
            //        }
            //    }

            //}
            #endregion

            //Check which type is clicked whether Group or Ledger?
            if (Convert.ToInt32(ID) > 0)//Double cick event is only for either Group account or Ledger account ...not for Total amount or Opening balance
            {
                if (Type == "GROUP")// If GroupType is clicked
                {
                    GroupBalanceSettings m_GBS = new GroupBalanceSettings();
                    m_GBS.HasDateRange = m_BS.HasDateRange;
                    m_GBS.ShowZeroBalance = m_BS.ShowZeroBalance;
                    m_GBS.FromDate = m_BS.FromDate;
                    m_GBS.ToDate = m_BS.ToDate;
                    m_GBS.AllGroups = m_BS.AllGroups;
                    m_GBS.AccClassID = m_BS.AccClassID;
                    m_GBS.ProjectID = m_BS.ProjectID;
                    m_GBS.OnlyPrimaryGroups = m_BS.OnlyPrimaryGroups;
                    m_GBS.GroupID = Convert.ToInt32(ID);//Store the GroupID value on object which achieve while double clicking the corresponding row of cell
                    frmGroupBalance m_GrpBal = new frmGroupBalance(m_GBS, m_MDIMain);
                    m_GrpBal.ShowDialog();
                }
                else if (Type == "LEDGER")//If LedgerType is clicked
                {
                    TransactSettings m_TS = new TransactSettings();
                    m_TS.HasDateRange = m_BS.HasDateRange;
                    m_TS.FromDate = m_BS.FromDate;
                    m_TS.ToDate = m_BS.ToDate;
                    m_TS.AccClassID = m_BS.AccClassID;
                    m_TS.ProjectID = m_BS.ProjectID;
                    m_TS.LedgerID = Convert.ToInt32(ID);
                    m_TS.ProjectID = m_BS.ProjectID;
                    frmTransaction m_Transact = new frmTransaction(m_TS, m_MDIMain);
                    m_Transact.ShowDialog();
                }
            }



        }

        private void btnPrintPreview_Click_1(object sender, EventArgs e)
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

            dsBalanceSheet.Clear();//It clear the previous records of dataset on crystal report...when this button is pressed

            //otherwise it populate the records again and again

            double DebitSum, CreditSum;
            DebitSum = CreditSum = 0;
            ReportClass rpt;
            if (m_BS.DispFormat == BalanceSheetSettings.DisplayFormat.Standard)
            {
                rpt = new rptBalanceSheetStandard();
            }
            else
            {
                rpt = new rptBalanceSheet();
            }
            //Fill the logo on the report
            Misc.WriteLogo(dsBalanceSheet, "tblImage");
            //Set DataSource to be dsTrial dataset on the report
            rpt.SetDataSource(dsBalanceSheet);
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
            int uid = User.CurrUserID;
            DataTable dtroleinfo = User.GetUserInfo(uid);
            DataRow drrole = dtroleinfo.Rows[0];
            int roleid = Convert.ToInt32(drrole["AccessRoleID"].ToString());

            if (roleid == 37)//if user is root, get information from tblCompany
            {
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
            }
            else //if user is not root, take information from tblUserPreference
            {
                string companyname = UserPreference.GetValue("COMPANY_NAME", uid);
                string companyaddress = UserPreference.GetValue("COMPANY_ADDRESS", uid);
                string companycity = UserPreference.GetValue("COMPANY_CITY", uid);
                string companypan = UserPreference.GetValue("COMPANY_PAN", uid);
                string companyphone = UserPreference.GetValue("COMPANY_PHONE", uid);
                string companyslogan = UserPreference.GetValue("COMPANY_SLOGAN", uid);

                pdvCompany_Name.Value = companyname;
                pvCollection.Clear();
                pvCollection.Add(pdvCompany_Name);
                rpt.DataDefinition.ParameterFields["Company_Name"].ApplyCurrentValues(pvCollection);

                pdvCompany_Address.Value = companyaddress + ((companyaddress.Trim().Length > 0) && (companycity.Trim().Length > 0) ? ", " : "") + companycity; //Display comma in the middle only if both are available
                pvCollection.Clear();
                pvCollection.Add(pdvCompany_Address);
                rpt.DataDefinition.ParameterFields["Company_Address"].ApplyCurrentValues(pvCollection);

                pdvCompany_PAN.Value = companypan;
                pvCollection.Clear();
                pvCollection.Add(pdvCompany_PAN);
                rpt.DataDefinition.ParameterFields["Company_PAN"].ApplyCurrentValues(pvCollection);

                pdvCompany_Phone.Value = "Phone No.: " + companyphone;
                pvCollection.Clear();
                pvCollection.Add(pdvCompany_Phone);
                rpt.DataDefinition.ParameterFields["Company_Phone"].ApplyCurrentValues(pvCollection);

                pdvCompany_Slogan.Value = companyslogan;
                pvCollection.Clear();
                pvCollection.Add(pdvCompany_Slogan);
                rpt.DataDefinition.ParameterFields["Company_Slogan"].ApplyCurrentValues(pvCollection);

            }
            pdvReport_Head.Value = "Balance Sheet";
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

            if (m_BS.ToDate != null)
                pdvReport_Date.Value = "As On Date:" + Date.ToSystem((DateTime)m_BS.ToDate);
            else
                pdvReport_Date.Value = "As On Date:" + Date.ToSystem(Date.GetServerDate());
            //if (m_BS.FromDate != null && m_BS.ToDate != null)
            //{
            //    lblAsonDate.Text = "From: " + Date.ToSystem(Convert.ToDateTime(m_BS.FromDate)) + " To: " + Date.ToSystem(Convert.ToDateTime(m_BS.ToDate));
            //}
            //if (m_BS.ToDate != null)
            //{
            //    lblAsonDate.Text = "As on Date: " + Date.ToSystem(Convert.ToDateTime(m_BS.ToDate));
            //}
            //if (m_BS.FromDate != null)
            //{
            //    //This is actually not applicable
            //    lblAsonDate.Text = "From: " + Date.ToSystem(Convert.ToDateTime(m_BS.FromDate));
            //}
            ////else//both are null
            ////{
            ////    pdvReport_Date.Value = "As of now";

            ////}
            //if (m_BS.FromDate == null && m_BS.ToDate == null)
            //{
            //    lblAsonDate.Text = "As of now";
            //}

            pvCollection.Clear();
            pvCollection.Add(pdvReport_Date);
            rpt.DataDefinition.ParameterFields["Report_Date"].ApplyCurrentValues(pvCollection);

            dtCrReport.Clear();
            dtCrReport = dtLiabilities.Clone();
            dtCrReport = dtLiabilities.Copy();
            dtCrReport.Merge(dtAssets);

            dtCrReport.Columns["RowID"].ColumnName = "BalanceSheetID";
            dtCrReport.Columns["Account"].ColumnName = "Name";
            dtCrReport.Columns["Amount"].ColumnName = "Amount";
            dtCrReport.Columns["Type"].ColumnName = "Type";
            dtCrReport.Columns["CategoryID"].ColumnName = "GroupID";

            dtCrReport.Columns.Remove("S.No");

            if (m_BS.DispFormat == BalanceSheetSettings.DisplayFormat.Standard)
            {
                dsBalanceSheet.Tables["tblHeader"].Rows.Add(2, "Represented By");
                dsBalanceSheet.Tables["tblHeader"].Rows.Add(1, "Capital");

                dsBalanceSheet.Tables["tblGroupStandard"].Rows.Add(1, "Owners Fund", 1);
                dsBalanceSheet.Tables["tblGroupStandard"].Rows.Add(2, "Loan Funds", 1);
                dsBalanceSheet.Tables["tblGroupStandard"].Rows.Add(3, "Current Assets", 2);
                dsBalanceSheet.Tables["tblGroupStandard"].Rows.Add(4, "Less: Current Liabilities", 2);
                dsBalanceSheet.Tables["tblGroupStandard"].Rows.Add(5, "Fixed Assets", 2);

                DataRow[] rows = dtCrReport.Select("(AccountID='6' or AccountID='14' or AccountID='27'or AccountID='28'or AccountID='118') and Type='GROUP'");
                foreach (DataRow r in rows)
                {
                    dtCrReport.Rows.Remove(r);
                }
                dtCrReport.Columns.Remove("AccountID");
                dsBalanceSheet.Tables["tblBalanceSheetStandard"].Merge(dtCrReport);


            }
            else
            {
                dsBalanceSheet.Tables["tblGroup"].Rows.Add(2, "Asset");
                dsBalanceSheet.Tables["tblGroup"].Rows.Add(1, "Liabilities");
                // DisplayVerticalBalanceSheet(true);
                //Real programming start from here
                dtCrReport.Columns.Remove("AccountID");
                dsBalanceSheet.Tables["tblBalanceSheet"].Merge(dtCrReport);


            }

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

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            //Clear all rows
            // grdBalanceSheet.Redim(0, 0);
            dgBalanceSheet.Columns.Clear();

            this.Cursor = Cursors.WaitCursor;
            //Load all over again
            frmBalanceSheet_Load(sender, e);

            this.Cursor = Cursors.Default;
            this.WindowState = FormWindowState.Maximized;
            //Show complete notification
            //Global.Msg("Reload Complete!");
        }


        private void frmBalanceSheet_KeyDown(object sender, KeyEventArgs e)
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
                    prntDirect = 2;
                    btnPrintPreview_Click_1(sender, e);
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
                    btnPrintPreview_Click_1(sender, e);
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
                    btnPrintPreview_Click_1(sender, e);
                    break;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            prntDirect = 1;
            btnPrintPreview_Click_1(sender, e);
        }

        private void lblAsonDate_Click(object sender, EventArgs e)
        {

        }
        private string ReadAllAccClassID()
        {
            #region  AccountingClassID

            ArrayList arrChildAccClassIDs = new ArrayList();
            foreach (object j in m_BS.AccClassID)
            {
                AccountClass.GetChildIDs(Convert.ToInt32(j), ref arrChildAccClassIDs);

            }
            m_BS.AccClassID.AddRange(arrChildAccClassIDs);

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
                    foreach (string tag in m_BS.AccClassID)
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
            Project.GetChildProjects(Convert.ToInt32(m_BS.ProjectID), ref arrchildProjectIds);
            ArrayList ProjectIDCollection = new ArrayList();
            foreach (object obj in arrchildProjectIds)
            {
                int p = (int)obj;
                ProjectIDCollection.Add(p.ToString());
            }

            string ProjectIDS = "'" + m_BS.ProjectID + "'";

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
                    tw.WriteElementString("PID", Convert.ToInt32(m_BS.ProjectID).ToString());
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

        private void frmBalanceSheet_Resize(object sender, EventArgs e)
        {
            //  int RowCount = grdBalanceSheet.Rows.Count;
            SourceGrid.Cells.Views.Cell head = new SourceGrid.Cells.Views.Cell();
            head.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Color.AliceBlue);
            // grdBalanceSheet.Rows.Insert(RowCount);
            if (m_BS.DispFormat == BalanceSheetSettings.DisplayFormat.Vertical)
            {
                if (isMax == true)
                {
                    grdBalanceSheet[0, 0] = new MyHeader("S.No.");
                    // grdBalanceSheet[RowCount, 0].View = new SourceGrid.Cells.Views.Cell(head);

                    grdBalanceSheet[0, 1] = new MyHeader("Liabilities");
                    //grdBalanceSheet[RowCount, 1].View = new SourceGrid.Cells.Views.Cell(head);

                    grdBalanceSheet[0, 2] = new MyHeader("Amount");
                    // grdBalanceSheet[RowCount, 2].View = new SourceGrid.Cells.Views.Cell(head);

                    grdBalanceSheet[0, 3] = new MyHeader("ID");
                    //grdBalanceSheet[RowCount, 3].View = new SourceGrid.Cells.Views.Cell(head);

                    grdBalanceSheet[0, 4] = new MyHeader("Type");
                    // grdBalanceSheet[RowCount, 4].View = new SourceGrid.Cells.Views.Cell(head);

                    //Define the width of column size
                    grdBalanceSheet[0, 0].Column.Width = 80;
                    grdBalanceSheet[0, 1].Column.Width = 980;
                    grdBalanceSheet[0, 2].Column.Width = 350;
                    grdBalanceSheet[0, 3].Column.Width = 150;
                    grdBalanceSheet[0, 4].Column.Width = 150;

                    //Code for making column invisible
                    grdBalanceSheet.Columns[3].Visible = false;// making third  and fourth column invisible and using it in  programming     
                    grdBalanceSheet.Columns[4].Visible = false;
                    isMax = false;
                }
                else
                {
                    grdBalanceSheet[0, 0] = new MyHeader("S.No.");
                    // grdBalanceSheet[RowCount, 0].View = new SourceGrid.Cells.Views.Cell(head);

                    grdBalanceSheet[0, 1] = new MyHeader("Liabilities");
                    //grdBalanceSheet[RowCount, 1].View = new SourceGrid.Cells.Views.Cell(head);

                    grdBalanceSheet[0, 2] = new MyHeader("Amount");
                    // grdBalanceSheet[RowCount, 2].View = new SourceGrid.Cells.Views.Cell(head);

                    grdBalanceSheet[0, 3] = new MyHeader("ID");
                    //grdBalanceSheet[RowCount, 3].View = new SourceGrid.Cells.Views.Cell(head);

                    grdBalanceSheet[0, 4] = new MyHeader("Type");
                    // grdBalanceSheet[RowCount, 4].View = new SourceGrid.Cells.Views.Cell(head);

                    //Define the width of column size
                    grdBalanceSheet[0, 0].Column.Width = 50;
                    grdBalanceSheet[0, 1].Column.Width = 665;
                    grdBalanceSheet[0, 2].Column.Width = 250;
                    grdBalanceSheet[0, 3].Column.Width = 150;
                    grdBalanceSheet[0, 4].Column.Width = 150;

                    //Code for making column invisible
                    grdBalanceSheet.Columns[3].Visible = false;// making third  and fourth column invisible and using it in  programming     
                    grdBalanceSheet.Columns[4].Visible = false;

                    isMax = true;
                }
            }
            if (m_BS.DispFormat == BalanceSheetSettings.DisplayFormat.TFormat)
            {
                if (isMax == true)
                {
                    grdBalanceSheet[0, 0] = new MyHeader("Assets");
                    grdBalanceSheet[0, 0].ColumnSpan = 3;
                    // grdBalanceSheet[0, 0].View = new SourceGrid.Cells.Views.Cell(head);
                    grdBalanceSheet[0, 5] = new MyHeader("Liabilities");
                    grdBalanceSheet[0, 5].ColumnSpan = 3;
                    //grdBalanceSheet[0, 5].View = new SourceGrid.Cells.Views.Cell(head);
                    grdBalanceSheet[1, 0] = new MyHeader("S.No.");
                    // grdBalanceSheet[1, 0].View = new SourceGrid.Cells.Views.Cell(head);
                    grdBalanceSheet[1, 1] = new MyHeader("Account Name");
                    // grdBalanceSheet[1, 1].View = new SourceGrid.Cells.Views.Cell(head);
                    grdBalanceSheet[1, 2] = new MyHeader("Amount");
                    //grdBalanceSheet[1, 2].View = new SourceGrid.Cells.Views.Cell(head);
                    grdBalanceSheet[1, 3] = new MyHeader("ID");
                    grdBalanceSheet[1, 4] = new MyHeader("Type");

                    //Define the width of column size

                    grdBalanceSheet[1, 0].Column.Width = 60; //S.No.
                    grdBalanceSheet[1, 1].Column.Width = 500;//Particulars
                    grdBalanceSheet[1, 2].Column.Width = 150;//Amount



                    //FOR LIABILITIES
                    grdBalanceSheet[1, 5] = new MyHeader("S.No.");
                    grdBalanceSheet[1, 6] = new MyHeader("Account Name");
                    grdBalanceSheet[1, 7] = new MyHeader("Amount");
                    grdBalanceSheet[1, 8] = new MyHeader("ID");
                    grdBalanceSheet[1, 9] = new MyHeader("Type");

                    //Define width of column size
                    grdBalanceSheet[1, 5].Column.Width = 60;
                    grdBalanceSheet[1, 6].Column.Width = 500;
                    grdBalanceSheet[1, 7].Column.Width = 150;

                    //Code for making column invisible
                    grdBalanceSheet.Columns[8].Visible = false;// making forth column invisible and using it in programming     
                    grdBalanceSheet.Columns[9].Visible = false;

                    //Code for making column invisible

                    grdBalanceSheet.Columns[3].Visible = false;// making third column invisible and using it in  programming     
                    grdBalanceSheet.Columns[4].Visible = false;
                    isMax = false;
                }
                else
                {
                    grdBalanceSheet[0, 0] = new MyHeader("Assets");
                    grdBalanceSheet[0, 0].ColumnSpan = 3;
                    // grdBalanceSheet[0, 0].View = new SourceGrid.Cells.Views.Cell(head);
                    grdBalanceSheet[0, 5] = new MyHeader("Liabilities");
                    grdBalanceSheet[0, 5].ColumnSpan = 3;
                    //grdBalanceSheet[0, 5].View = new SourceGrid.Cells.Views.Cell(head);
                    grdBalanceSheet[1, 0] = new MyHeader("S.No.");
                    // grdBalanceSheet[1, 0].View = new SourceGrid.Cells.Views.Cell(head);
                    grdBalanceSheet[1, 1] = new MyHeader("Account Name");
                    // grdBalanceSheet[1, 1].View = new SourceGrid.Cells.Views.Cell(head);
                    grdBalanceSheet[1, 2] = new MyHeader("Amount");
                    //grdBalanceSheet[1, 2].View = new SourceGrid.Cells.Views.Cell(head);
                    grdBalanceSheet[1, 3] = new MyHeader("ID");
                    grdBalanceSheet[1, 4] = new MyHeader("Type");

                    //Define the width of column size

                    grdBalanceSheet[1, 0].Column.Width = 45; //S.No.
                    grdBalanceSheet[1, 1].Column.Width = 352;//Particulars
                    grdBalanceSheet[1, 2].Column.Width = 90;//Amount

                    //FOR LIABILITIES
                    grdBalanceSheet[1, 5] = new MyHeader("S.No.");
                    grdBalanceSheet[1, 6] = new MyHeader("Account Name");
                    grdBalanceSheet[1, 7] = new MyHeader("Amount");
                    grdBalanceSheet[1, 8] = new MyHeader("ID");
                    grdBalanceSheet[1, 9] = new MyHeader("Type");

                    //Define width of column size
                    grdBalanceSheet[1, 5].Column.Width = 45;
                    grdBalanceSheet[1, 6].Column.Width = 352;
                    grdBalanceSheet[1, 7].Column.Width = 90;

                    //Code for making column invisible
                    grdBalanceSheet.Columns[8].Visible = false;// making forth column invisible and using it in programming     
                    grdBalanceSheet.Columns[9].Visible = false;

                    //Code for making column invisible

                    grdBalanceSheet.Columns[3].Visible = false;// making third column invisible and using it in  programming     
                    grdBalanceSheet.Columns[4].Visible = false;
                    isMax = true;
                }
            }

        }

    }
}
