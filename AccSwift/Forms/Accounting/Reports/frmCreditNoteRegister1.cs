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

namespace AccSwift
{
    public partial class frmCreditNoteRegister : Form
    {
        private SourceGrid.Cells.Controllers.CustomEvents dblClick;//Double click for DebitNote Register
        private CreditNoteRegisterSettings m_CreditNoteRegister;
        public frmCreditNoteRegister(CreditNoteRegisterSettings CredNoteRegister)
        {
            InitializeComponent();
            m_CreditNoteRegister = new CreditNoteRegisterSettings();
            m_CreditNoteRegister.FromDate = CredNoteRegister.FromDate;
            m_CreditNoteRegister.ToDate = CredNoteRegister.ToDate;
            m_CreditNoteRegister.AccClassID = CredNoteRegister.AccClassID;
        }

        private void frmCreditNoteRegister_Load(object sender, EventArgs e)
        {

       
            //Add a double click handler. When user dblclicks the cell, will use this function to send the parent form the Currency code

            // double click for DebitNoteRegister

            dblClick = new SourceGrid.Cells.Controllers.CustomEvents();
            dblClick.DoubleClick += new EventHandler(CreditNoteRegister_DoubleClick);

            //Disable multiple selection

            grdCreditNoteRegister.Selection.EnableMultiSelection = false;

            //Disable multiple selection
            grdCreditNoteRegister.Selection.EnableMultiSelection = false;

            grdCreditNoteRegister.Redim(1, 6);

            WriteHeader();//Calling this function for Writting Header of SourceGridView

            DataTable dtCreditNoteMasterInfo = CreditNote.GetCreditNoteMasterInfo(m_CreditNoteRegister.FromDate, m_CreditNoteRegister.ToDate, m_CreditNoteRegister.AccClassID, "CR_NOT");

            for (int i = 0; i < dtCreditNoteMasterInfo.Rows.Count; i++)
            {
                DataRow drCreditNoteMasterInfo = dtCreditNoteMasterInfo.Rows[i];

                int rows = grdCreditNoteRegister.Rows.Count;

                grdCreditNoteRegister.Rows.Insert(rows);

                //Display VoucherNo on grid
                grdCreditNoteRegister[rows, 0] = new SourceGrid.Cells.Cell(rows.ToString());
                grdCreditNoteRegister[rows, 0].AddController(dblClick);

                grdCreditNoteRegister[rows, 1] = new SourceGrid.Cells.Cell(Date.DBToSystem(drCreditNoteMasterInfo["CreditNote_Date"].ToString()));
                //grdDebitNoteRegister[rows, 1] = new SourceGrid.Cells.Cell(Date.DBToSystem(drDebNoteMasterInfo["DebitNote_Date"].ToString()));
                grdCreditNoteRegister[rows, 1].AddController(dblClick);

                grdCreditNoteRegister[rows, 2] = new SourceGrid.Cells.Cell(drCreditNoteMasterInfo["Voucher_No"].ToString());
                grdCreditNoteRegister[rows, 2].AddController(dblClick);

                grdCreditNoteRegister[rows, 5] = new SourceGrid.Cells.Cell(drCreditNoteMasterInfo["CreditNoteID"].ToString());
                grdCreditNoteRegister[rows, 5].Column.Visible = false;
                grdCreditNoteRegister[rows, 5].Column.Visible = false;



                //Block for getting the information of tblCreditNoteMaster according to CreditNoteID

                DataTable dtCreditNoteDtlInfo = CreditNote.GetCredNoteDtlInfo(Convert.ToInt32(drCreditNoteMasterInfo["CreditNoteID"]));   //According to CreditNoteID find the information of tblDebitNoteDetail where DeCr=Debit
                double TotalCreditAmt = 0;
                for (int k = 1; k <= dtCreditNoteDtlInfo.Rows.Count; k++)
                {

                    DataRow dr = dtCreditNoteDtlInfo.Rows[k - 1];
                    TotalCreditAmt += Convert.ToDouble(dr["Amount"]);

                }

                grdCreditNoteRegister[rows, 4] = new SourceGrid.Cells.Cell(TotalCreditAmt.ToString());
                grdCreditNoteRegister[rows, 4].AddController(dblClick);



                //Among All ledgers where DrCr= Debit,collect only thoes ledgers which consists under Creditor AccountGroup 
                for (int j = 1; j <= dtCreditNoteDtlInfo.Rows.Count; j++)
                {

                    DataRow drCreditNoteDtlInfo = dtCreditNoteDtlInfo.Rows[j - 1];

                    bool Found = false;

                    Found = AccountGroup.IsLedgerUnderGroup(Convert.ToInt32(drCreditNoteDtlInfo["LedgerID"]), 29);//Manually passing the GroupID of Creditor Which is equivalent to 29
                    if (Found == true)//If the ledger is associated under the Creditor AccountGroup then display the one Ledger Name in SourceGridView,then out from  for loop by adjusting for loop such that showing only the first row of datarow
                    {


                        //Write the name in the grid
                        DataTable dt = Ledger.GetLedgerInfo(Convert.ToInt32(drCreditNoteDtlInfo["LedgerID"]), LangMgr.DefaultLanguage);

                        DataRow dr = dt.Rows[0];

                        grdCreditNoteRegister[rows, 3] = new SourceGrid.Cells.Cell(dr["LedName"].ToString());
                        grdCreditNoteRegister[rows, 3].AddController(dblClick);

                        j = dtCreditNoteDtlInfo.Rows.Count;//For showing only one LedgerName of Creditor on grid and to be out from this loop

                    }
                }      
            }

            //for (int i = 1; i <= dtCredNoteMasterInfo.Rows.Count; i++)
            //{

            //    DataRow drCredNoteMasterInfo = dtCredNoteMasterInfo.Rows[i - 1];

            //    int rows = grdCreditNoteRegister.Rows.Count;

            //    grdCreditNoteRegister.Rows.Insert(rows);

            //    //Display VoucherNo on grid
            //    grdCreditNoteRegister[rows, 0] = new SourceGrid.Cells.Cell(rows.ToString());
            //    grdCreditNoteRegister[rows, 0].AddController(dblClick);

            //    grdCreditNoteRegister[rows, 1] = new SourceGrid.Cells.Cell(Date.DBToSystem(drCredNoteMasterInfo["CreditNote_Date"].ToString()));
            //    //grdDebitNoteRegister[rows, 1] = new SourceGrid.Cells.Cell(Date.DBToSystem(drDebNoteMasterInfo["DebitNote_Date"].ToString()));
            //    grdCreditNoteRegister[rows, 1].AddController(dblClick);

            //    grdCreditNoteRegister[rows, 2] = new SourceGrid.Cells.Cell(drCredNoteMasterInfo["Voucher_No"].ToString());
            //    grdCreditNoteRegister[rows, 2].AddController(dblClick);

            //    grdCreditNoteRegister[rows, 5] = new SourceGrid.Cells.Cell(drCredNoteMasterInfo["CreditNoteID"].ToString());
            //    grdCreditNoteRegister[rows, 5].Column.Visible = false;
            //    grdCreditNoteRegister[rows, 5].Column.Visible = false;
               


            //    //Block for getting the information of tblCreditNoteMaster according to CreditNoteID

            //    DataTable dtCreditNoteDtlInfo = CreditNote.GetCredNoteDtlInfo(Convert.ToInt32(drCredNoteMasterInfo["CreditNoteID"]));   //According to CreditNoteID find the information of tblDebitNoteDetail where DeCr=Debit
            //    double TotalCreditAmt = 0;
            //    for (int k = 1; k <= dtCreditNoteDtlInfo.Rows.Count; k++)
            //    {

            //        DataRow dr = dtCreditNoteDtlInfo.Rows[k - 1];
            //        TotalCreditAmt += Convert.ToDouble(dr["Amount"]);



            //    }

            //    grdCreditNoteRegister[rows, 4] = new SourceGrid.Cells.Cell(TotalCreditAmt.ToString());
            //    grdCreditNoteRegister[rows, 4].AddController(dblClick);



            //    //Among All ledgers where DrCr= Debit,collect only thoes ledgers which consists under Creditor AccountGroup 
            //    for (int j = 1; j <= dtCreditNoteDtlInfo.Rows.Count; j++)
            //    {

            //        DataRow drCreditNoteDtlInfo = dtCreditNoteDtlInfo.Rows[j - 1];

            //        bool Found = false;
                   
            //        Found = AccountGroup.IsLedgerUnderGroup(Convert.ToInt32(drCreditNoteDtlInfo["LedgerID"]), 29);//Manually passing the GroupID of Creditor Which is equivalent to 29
            //        if (Found == true)//If the ledger is associated under the Creditor AccountGroup then display the one Ledger Name in SourceGridView,then out from  for loop by adjusting for loop such that showing only the first row of datarow
            //        {


            //            //Write the name in the grid
            //            DataTable dt = Ledger.GetLedgerInfo(Convert.ToInt32(drCreditNoteDtlInfo["LedgerID"]), LangMgr.Language);

            //            DataRow dr = dt.Rows[0];

            //            grdCreditNoteRegister[rows, 3] = new SourceGrid.Cells.Cell(dr["LedName"].ToString());
            //            grdCreditNoteRegister[rows, 3].AddController(dblClick);

            //            j = dtCreditNoteDtlInfo.Rows.Count;//For showing only one LedgerName of Creditor on grid and to be out from this loop



            //        }


            //    }

            //}

        }
        //Function for Writting Header Part of SourceGridView
        private void WriteHeader()
        {

            //Define the HeaderPart of sourceGridView

            grdCreditNoteRegister[0, 0] = new SourceGrid.Cells.ColumnHeader("S. No.");
            grdCreditNoteRegister[0, 1] = new SourceGrid.Cells.ColumnHeader("Date");
            grdCreditNoteRegister[0, 2] = new SourceGrid.Cells.ColumnHeader("Voucher No");
            grdCreditNoteRegister[0, 3] = new SourceGrid.Cells.ColumnHeader("Account");
            grdCreditNoteRegister[0, 4] = new SourceGrid.Cells.ColumnHeader("Total Amount");
            grdCreditNoteRegister[0, 5] = new SourceGrid.Cells.ColumnHeader("CreditNoteID");

            //Define size of column of Grid
            grdCreditNoteRegister[0, 0].Column.Width = 50;
            grdCreditNoteRegister[0, 1].Column.Width = 150;
            grdCreditNoteRegister[0, 2].Column.Width = 150;
            grdCreditNoteRegister[0, 3].Column.Width = 200;
            grdCreditNoteRegister[0, 4].Column.Width = 150;
            grdCreditNoteRegister[0, 5].Column.Width = 100;



        }


        private void CreditNoteRegister_DoubleClick(object sender, EventArgs e)
        {
            try
            {


                //Get the Selected Row

                int CurRow = grdCreditNoteRegister.Selection.GetSelectionRegion().GetRowsIndex()[0];//

                SourceGrid.CellContext cellType = new SourceGrid.CellContext(grdCreditNoteRegister, new SourceGrid.Position(CurRow, 5));

                int CreditNoteID = Convert.ToInt32(cellType.Value);
                frmCreditNote frm = new frmCreditNote(CreditNoteID);
                frm.Show();

            }
            catch (Exception ex)
            {

                Global.Msg(ex.Message);
            }
        }

        private void frmCreditNoteRegister_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }
    }
}
