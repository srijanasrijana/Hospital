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
using DateManager;
using System.Threading;
using CrystalDecisions.Shared;
using BusinessLogic;
using System.Collections;
using Inventory.DataSet;
using System.Threading;
using DateManager;
using Common;
using Accounts.Reports;


namespace Accounts
{
    public partial class frmVATReport : Form
    {
        string FileName = " ";
        private int prntDirect = 0;
        private IMDIMainForm m_MDIForm;
        //For Export Menu
        ContextMenu Menu_Export;
        private enum PrintType
        {
            None,
            DirectPrint,
            Excel,
            PDF,
            CrystalReport,
            Email
        }

        private int VatSummaryRowsCount;
        private int VatDetailsRowsCount;
        private int VatSummaryRowsCountPaid;
        private int VatDetailsRowsCountPaid;
        
        private string VType = "";
        private int RowID = 0;
        private VATReprotSettings m_VATReport;
        int strSNo = 1;
        int strSNoPaid = 1;
        private SourceGrid.Cells.Controllers.CustomEvents dblClick;

        private Accounts.Model.dsVATReport dsVatReport1 = new Model.dsVATReport();

        public frmVATReport(VATReprotSettings VATReport)
        {
            m_VATReport = new VATReprotSettings();
            m_VATReport.Summary = VATReport.Summary;
            m_VATReport.Detail = VATReport.Detail;
            m_VATReport.FromDate = VATReport.FromDate;
            m_VATReport.ToDate = VATReport.ToDate;
            m_VATReport.Collected = VATReport.Collected;
            m_VATReport.Paid = VATReport.Paid;
            m_VATReport.Detail = VATReport.Detail;
            m_VATReport.ProjectID = VATReport.ProjectID;
            m_VATReport.AccClassID = VATReport.AccClassID;
           InitializeComponent();
        }

        private void WriteHeaderForVatSummary()
        {
            //Define Header Part
            grdVATReport[0, 0] = new SourceGrid.Cells.ColumnHeader("Sales Amount");
            grdVATReport[0, 1] = new SourceGrid.Cells.ColumnHeader("VAT");
            grdVATReport[0, 2] = new SourceGrid.Cells.ColumnHeader("Total Amount");

            //Define width of column size
            grdVATReport[0, 0].Column.Width = 150;
            grdVATReport[0, 1].Column.Width = 150;
            grdVATReport[0, 2].Column.Width = 150;
        }

        private void WriteHeaderForVatSummaryPaid()
        {
            //Define Header Part
            grdvatpaid[0, 0] = new SourceGrid.Cells.ColumnHeader("Sales Amount");
            grdvatpaid[0, 1] = new SourceGrid.Cells.ColumnHeader("VAT");
            grdvatpaid[0, 2] = new SourceGrid.Cells.ColumnHeader("Total Amount");

            //Define width of column size
            grdvatpaid[0, 0].Column.Width = 150;
            grdvatpaid[0, 1].Column.Width = 150;
            grdvatpaid[0, 2].Column.Width = 150;
        }

        private void WriteHeaderForVatDetails()
        {
            //Define Header Part
            grdVATReport[0, 0] = new SourceGrid.Cells.ColumnHeader("S.No.");
            grdVATReport[0, 1] = new SourceGrid.Cells.ColumnHeader("Date");
            grdVATReport[0, 2] = new SourceGrid.Cells.ColumnHeader("Party Account");
            grdVATReport[0, 3] = new SourceGrid.Cells.ColumnHeader("Voucher No");
            grdVATReport[0, 4] = new SourceGrid.Cells.ColumnHeader("Net Amount");
            grdVATReport[0, 5] = new SourceGrid.Cells.ColumnHeader("VAT");
            grdVATReport[0, 6] = new SourceGrid.Cells.ColumnHeader("Total Amount");
            grdVATReport[0, 7] = new SourceGrid.Cells.ColumnHeader("VoucherType");
            grdVATReport[0, 8] = new SourceGrid.Cells.ColumnHeader("RowID");
           
            //Define width of column size
            grdVATReport[0, 0].Column.Width = 50;
            grdVATReport[0, 1].Column.Width = 100;

            grdVATReport[0, 2].Column.Width = 170;
            grdVATReport[0, 3].Column.Width = 100;
            grdVATReport[0, 4].Column.Width = 150;
            grdVATReport[0, 5].Column.Width = 150;
            grdVATReport[0, 6].Column.Width = 150;
            grdVATReport[0, 7].Column.Width = 100;
            grdVATReport[0, 8].Column.Width = 10;
            grdVATReport[0, 8].Column.Visible = false;
        }

        private void WriteHeaderForVatDetailsPaid()
        {
            //Define Header Part
            grdvatpaid[0, 0] = new SourceGrid.Cells.ColumnHeader("S.No.");
            grdvatpaid[0, 1] = new SourceGrid.Cells.ColumnHeader("Date");
            grdvatpaid[0, 2] = new SourceGrid.Cells.ColumnHeader("Party Account");
            grdvatpaid[0, 3] = new SourceGrid.Cells.ColumnHeader("Voucher No");
            grdvatpaid[0, 4] = new SourceGrid.Cells.ColumnHeader("Net Amount");
            grdvatpaid[0, 5] = new SourceGrid.Cells.ColumnHeader("VAT");
            grdvatpaid[0, 6] = new SourceGrid.Cells.ColumnHeader("Total Amount");
            grdvatpaid[0, 7] = new SourceGrid.Cells.ColumnHeader("VoucherType");
            grdvatpaid[0, 8] = new SourceGrid.Cells.ColumnHeader("RowID");


            //Define width of column size
            grdvatpaid[0, 0].Column.Width = 50;
            grdvatpaid[0, 1].Column.Width = 100;
            grdvatpaid[0, 2].Column.Width = 180;
            grdvatpaid[0, 3].Column.Width = 100;
            grdvatpaid[0, 4].Column.Width = 150;
            grdvatpaid[0, 5].Column.Width = 150;
            grdvatpaid[0, 6].Column.Width = 150;
            grdvatpaid[0, 7].Column.Width = 100;
            grdvatpaid[0, 8].Column.Width = 10;
            grdvatpaid[0, 8].Column.Visible = false;
        }
        private void WriteVatSummary(string TotalNetAmount,string TotalVatAmount,string GrandTotalAmount)
        {
            //for orientation purpose
            SourceGrid.Cells.Views.Cell categoryView = new SourceGrid.Cells.Views.Cell();
            categoryView.Background = new DevAge.Drawing.VisualElements.BackgroundLinearGradient(Color.RoyalBlue, Color.LightBlue, 0);
            categoryView.ForeColor = Color.FromKnownColor(KnownColor.ActiveCaptionText);
            categoryView.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleCenter;
            categoryView.Border = DevAge.Drawing.RectangleBorder.NoBorder;
            categoryView.Font = new Font(Font, FontStyle.Bold);

            grdVATReport.Rows.Insert(grdVATReport.RowsCount);
            grdVATReport[VatSummaryRowsCount, 0] = new SourceGrid.Cells.Cell(TotalNetAmount);
            //grdVATReport[VatSummaryRowsCount, 0].View = categoryView;
            grdVATReport[VatSummaryRowsCount, 1] = new SourceGrid.Cells.Cell(TotalVatAmount);
            //grdVATReport[VatSummaryRowsCount, 1].View = categoryView;
            grdVATReport[VatSummaryRowsCount, 2] = new SourceGrid.Cells.Cell(GrandTotalAmount);
           // grdVATReport[VatSummaryRowsCount, 2].View = categoryView;
            VatSummaryRowsCount++;
        }
        private void WriteVatSummaryPaid(string TotalNetAmount, string TotalVatAmount, string GrandTotalAmount)
        {
            //for orientation purpose
            SourceGrid.Cells.Views.Cell categoryView = new SourceGrid.Cells.Views.Cell();
            categoryView.Background = new DevAge.Drawing.VisualElements.BackgroundLinearGradient(Color.RoyalBlue, Color.LightBlue, 0);
            categoryView.ForeColor = Color.FromKnownColor(KnownColor.ActiveCaptionText);
            categoryView.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleCenter;
            categoryView.Border = DevAge.Drawing.RectangleBorder.NoBorder;
            categoryView.Font = new Font(Font, FontStyle.Bold);

            grdvatpaid.Rows.Insert(grdvatpaid.RowsCount);
            grdvatpaid[VatSummaryRowsCountPaid, 0] = new SourceGrid.Cells.Cell(TotalNetAmount);
            //grdVATReport[VatSummaryRowsCount, 0].View = categoryView;
            grdvatpaid[VatSummaryRowsCountPaid, 1] = new SourceGrid.Cells.Cell(TotalVatAmount);
            //grdVATReport[VatSummaryRowsCount, 1].View = categoryView;
            grdvatpaid[VatSummaryRowsCountPaid, 2] = new SourceGrid.Cells.Cell(GrandTotalAmount);
            // grdVATReport[VatSummaryRowsCount, 2].View = categoryView;
            VatSummaryRowsCountPaid++;
        }
        private void WriteVatDetails(int SNo,string TransactDate,string LedgerName,string VoucherNo,string NetAmount,string VatAmount,string TotalAmount,string VoucherType,int rowid)
        {
            //for orientation purpose
            SourceGrid.Cells.Views.Cell categoryView = new SourceGrid.Cells.Views.Cell();
            //categoryView.Background = new DevAge.Drawing.VisualElements.BackgroundLinearGradient(Color.RoyalBlue, Color.LightBlue, 0);
            categoryView.ForeColor = Color.FromKnownColor(KnownColor.ActiveCaptionText);
            categoryView.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleCenter;
            categoryView.Border = DevAge.Drawing.RectangleBorder.NoBorder;
            categoryView.Font = new Font(Font, FontStyle.Bold);

            grdVATReport.Rows.Insert(grdVATReport.RowsCount);

            SourceGrid.Cells.Views.Cell alternate = new SourceGrid.Cells.Views.Cell();
            if (VatDetailsRowsCount % 2 == 0)
            {
                //alternate.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Color.LightCoral);
            }
            else
            {
                alternate.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Global.Grid_Color);
            }
            //string strSNo = (SNo == 0 ? "***" : SNo.ToString());
            //strSNo = strSNo + 1; ;
            if (LedgerName != "TOTAL")
            {
                grdVATReport[VatDetailsRowsCount, 0] = new SourceGrid.Cells.Cell(strSNo);
                //grdVATReport[VatDetailsRowsCount, 0].View = categoryView;
                grdVATReport[VatDetailsRowsCount, 0].AddController(dblClick);
                grdVATReport[VatDetailsRowsCount, 0].View = new SourceGrid.Cells.Views.Cell(alternate);

                grdVATReport[VatDetailsRowsCount, 1] = new SourceGrid.Cells.Cell(TransactDate);
                //grdVATReport[VatDetailsRowsCount, 1].View = categoryView;
                grdVATReport[VatDetailsRowsCount, 1].AddController(dblClick);
                grdVATReport[VatDetailsRowsCount, 1].View = new SourceGrid.Cells.Views.Cell(alternate);

                grdVATReport[VatDetailsRowsCount, 2] = new SourceGrid.Cells.Cell(LedgerName);
                //grdVATReport[VatDetailsRowsCount, 2].View = categoryView;
                grdVATReport[VatDetailsRowsCount, 2].AddController(dblClick);
                grdVATReport[VatDetailsRowsCount, 2].View = new SourceGrid.Cells.Views.Cell(alternate);

                grdVATReport[VatDetailsRowsCount, 3] = new SourceGrid.Cells.Cell(VoucherNo);
                //grdVATReport[VatDetailsRowsCount, 3].View = categoryView;
                grdVATReport[VatDetailsRowsCount, 3].AddController(dblClick);
                grdVATReport[VatDetailsRowsCount, 3].View = new SourceGrid.Cells.Views.Cell(alternate);

                grdVATReport[VatDetailsRowsCount, 4] = new SourceGrid.Cells.Cell(NetAmount);
                //grdVATReport[VatDetailsRowsCount, 4].View = categoryView;
                grdVATReport[VatDetailsRowsCount, 4].AddController(dblClick);
                grdVATReport[VatDetailsRowsCount, 4].View = new SourceGrid.Cells.Views.Cell(alternate);

                grdVATReport[VatDetailsRowsCount, 5] = new SourceGrid.Cells.Cell(VatAmount);
                //grdVATReport[VatDetailsRowsCount, 5].View = categoryView;
                grdVATReport[VatDetailsRowsCount, 5].AddController(dblClick);
                grdVATReport[VatDetailsRowsCount, 5].View = new SourceGrid.Cells.Views.Cell(alternate);

                grdVATReport[VatDetailsRowsCount, 6] = new SourceGrid.Cells.Cell(TotalAmount);
                //grdVATReport[VatDetailsRowsCount, 6].View = categoryView;
                grdVATReport[VatDetailsRowsCount, 6].AddController(dblClick);
                grdVATReport[VatDetailsRowsCount, 6].View = new SourceGrid.Cells.Views.Cell(alternate);

                grdVATReport[VatDetailsRowsCount, 7] = new SourceGrid.Cells.Cell(VoucherType);
                grdVATReport[VatDetailsRowsCount, 7].AddController(dblClick);
                grdVATReport[VatDetailsRowsCount, 7].View = new SourceGrid.Cells.Views.Cell(alternate);

                grdVATReport[VatDetailsRowsCount, 8] = new SourceGrid.Cells.Cell(rowid);
                grdVATReport[VatDetailsRowsCount, 8].AddController(dblClick);
            }
            else if (LedgerName == "TOTAL")
            {
                grdVATReport[VatDetailsRowsCount, 0] = new SourceGrid.Cells.Cell("");
                grdVATReport[VatDetailsRowsCount, 0].View = categoryView;
                grdVATReport[VatDetailsRowsCount, 0].AddController(dblClick);
               

                grdVATReport[VatDetailsRowsCount, 1] = new SourceGrid.Cells.Cell(TransactDate);
                grdVATReport[VatDetailsRowsCount, 1].View = categoryView;
                grdVATReport[VatDetailsRowsCount, 1].AddController(dblClick);
                

                grdVATReport[VatDetailsRowsCount, 2] = new SourceGrid.Cells.Cell(LedgerName);
                grdVATReport[VatDetailsRowsCount, 2].View = categoryView;
                grdVATReport[VatDetailsRowsCount, 2].AddController(dblClick);
                

                grdVATReport[VatDetailsRowsCount, 3] = new SourceGrid.Cells.Cell(VoucherNo);
                grdVATReport[VatDetailsRowsCount, 3].View = categoryView;
                grdVATReport[VatDetailsRowsCount, 3].AddController(dblClick);
                

                grdVATReport[VatDetailsRowsCount, 4] = new SourceGrid.Cells.Cell(NetAmount);
                grdVATReport[VatDetailsRowsCount, 4].View = categoryView;
                grdVATReport[VatDetailsRowsCount, 4].AddController(dblClick);
               

                grdVATReport[VatDetailsRowsCount, 5] = new SourceGrid.Cells.Cell(VatAmount);
                grdVATReport[VatDetailsRowsCount, 5].View = categoryView;
                grdVATReport[VatDetailsRowsCount, 5].AddController(dblClick);
               

                grdVATReport[VatDetailsRowsCount, 6] = new SourceGrid.Cells.Cell(TotalAmount);
                grdVATReport[VatDetailsRowsCount, 6].View = categoryView;
                grdVATReport[VatDetailsRowsCount, 6].AddController(dblClick);
               

                grdVATReport[VatDetailsRowsCount, 7] = new SourceGrid.Cells.Cell(VoucherType);
                grdVATReport[VatDetailsRowsCount, 7].AddController(dblClick);
                grdVATReport[VatDetailsRowsCount, 7].View = categoryView;
                

                grdVATReport[VatDetailsRowsCount, 8] = new SourceGrid.Cells.Cell(rowid);
                grdVATReport[VatDetailsRowsCount, 8].AddController(dblClick);
            }
            VatDetailsRowsCount++;
            strSNo++;
        }

        private void frmVATReport_Load(object sender, EventArgs e)
        {
            DisplayBannar();
            //lblFromDate.Text = m_VATReport.FromDate.ToShortDateString();
            //lblToDate.Text = m_VATReport.ToDate.ToShortDateString();
            lblFromDate.Text = Date.ToSystem(m_VATReport.FromDate);
            lblToDate.Text = Date.ToSystem(m_VATReport.ToDate);
            dblClick = new SourceGrid.Cells.Controllers.CustomEvents();
            dblClick.DoubleClick += new EventHandler(grdVATReport_DoubleClick);
            //dblClick.DoubleClick += new EventHandler(grdVATReportPaid_DoubleClick);
           // string AccClassIDsXMLString = ReadAllAccClassID();
            //string ProjectIDsXMLString = ReadAllProjectID();
            



            String TotalAccClassIDs = "";
            TotalAccClassIDs = "'" + m_VATReport.AccClassID[0] + "'";
            for (int i = 0; i < m_VATReport.AccClassID.Count-1;i++ )
            {
                TotalAccClassIDs += ","+"'"+(m_VATReport.AccClassID[i+1].ToString())+"'";
            }
          
            //For ProjectID
           
            ArrayList arrchildProjectIds = new ArrayList();
            Project.GetChildProjects(Convert.ToInt32(m_VATReport.ProjectID), ref arrchildProjectIds);
            ArrayList ProjectIDCollection = new ArrayList();
            foreach (object obj in arrchildProjectIds)
            {
                int p = (int)obj;
                ProjectIDCollection.Add(p.ToString());
            }
                //Collect all Project  Which parent id is given projectid
              
                string ProjectIDS = "";

                ProjectIDS = "'" + m_VATReport.ProjectID + "'";

                for (int iproject = 0; iproject < ProjectIDCollection.Count; iproject++)
                {
                    ProjectIDS += "," + "'" + (ProjectIDCollection[iproject].ToString()) + "'";
                }
               // ProjectSQL = "AND ProjectID IN (" + (ProjectIDS) + ")";
               

          

            #region For Summary
            if (m_VATReport.Summary == true)
            {
                VatSummaryRowsCount = 1;
                VatDetailsRowsCountPaid = 1;
                VatSummaryRowsCountPaid = 1;
                //Disable multiple selection
                grdVATReport.Selection.EnableMultiSelection = false;
                grdvatpaid.Selection.EnableMultiSelection = false;

                if (m_VATReport.Collected==true)
                {
                grdVATReport.Redim(1, 7);
                }
                bool test = m_VATReport.Paid;
               // MessageBox.Show(test.ToString());
                grdvatpaid.Redim(1, 7);
                //Show the Summary
                if (m_VATReport.Collected == true)
                {
                    WriteHeaderForVatSummary();
                }
                if (m_VATReport.Paid == true)
                {
                    WriteHeaderForVatSummaryPaid();
                }
                Transaction m_TransactInfo = new Transaction();
                //DataTable dtTransactInfo = m_TransactInfo.GetLedgerTransact("154");//This function get the information of VAT from tblTransaction with help of VAT On Sale ledgerID(154)
                //DataTable dtTransactInfo = m_TransactInfo.GetLedgerTransact("412");
                DataTable dtTransactInfo = m_TransactInfo.GetLedgerTransactWithAccClassAndProjectID(Global.VATPayableID.ToString(), TotalAccClassIDs, ProjectIDS);
                double TotalNetAmt = 0;
                double TotalVatAmt = 0;
                double GrandTotalAmt = 0;
                double TotalNetAmtPaid = 0;
                double TotalVatAmtPaid = 0;
                double GrandTotalAmtPaid = 0;

                    foreach (DataRow drVatTransactInfo in dtTransactInfo.Rows)
                    {
                        if (Convert.ToDouble(drVatTransactInfo["Credit_Amount"].ToString()) > 0)//For Vat Collection Which We Have to Pay Like on Sales Invoice
                        {
                            if (m_VATReport.Collected == true)
                            {
                                lblvatcollected.Visible = true;
                                if (drVatTransactInfo["VoucherType"].ToString() == "SALES")
                                {
                                    double VatAmt = Convert.ToDouble(drVatTransactInfo["Credit_Amount"]);
                                    TotalVatAmt += Convert.ToDouble(drVatTransactInfo["Credit_Amount"]);
                                    //For obtaining NetAmount
                                    DataTable dt1 = m_TransactInfo.GetTransactionInfo(drVatTransactInfo["RowID"].ToString(), null, null, null);

                                    foreach (DataRow dr1 in dt1.Rows)
                                    {
                                        //if (Convert.ToDouble(dr1["Debit_Amount"]) > 0)
                                        if (Convert.ToDouble(dr1["Credit_Amount"]) > 0 && Convert.ToInt32(dr1["LedgerID"]) != 412)
                                        {
                                            TotalNetAmt += Convert.ToDouble(dr1["Credit_Amount"]);
                                        }
                                        if (Convert.ToDouble(dr1["Debit_Amount"]) > 0)
                                        {
                                            GrandTotalAmt += Convert.ToDouble(dr1["Debit_Amount"]);
                                        }
                                    }
                                }
                                if (drVatTransactInfo["VoucherType"].ToString() == "PURCH_RTN")
                                {
                                    double VatAmt = Convert.ToDouble(drVatTransactInfo["Credit_Amount"]);
                                    TotalVatAmt += Convert.ToDouble(drVatTransactInfo["Credit_Amount"]);
                                    //For obtaining NetAmount
                                    DataTable dt1 = m_TransactInfo.GetTransactionInfo(drVatTransactInfo["RowID"].ToString(), null, null, null);

                                    foreach (DataRow dr1 in dt1.Rows)
                                    {
                                        //if (Convert.ToDouble(dr1["Debit_Amount"]) > 0)
                                        if (Convert.ToDouble(dr1["Credit_Amount"]) > 0 && Convert.ToInt32(dr1["LedgerID"]) != 412)
                                        {
                                            TotalNetAmt += Convert.ToDouble(dr1["Credit_Amount"]);
                                        }
                                        if (Convert.ToDouble(dr1["Debit_Amount"]) > 0)
                                        {
                                            GrandTotalAmt += Convert.ToDouble(dr1["Debit_Amount"]);
                                        }
                                    }
                                }
                                if (drVatTransactInfo["VoucherType"].ToString() == "JRNL")
                                {
                                    double VatAmt = Convert.ToDouble(drVatTransactInfo["Credit_Amount"]);
                                    TotalVatAmt += Convert.ToDouble(drVatTransactInfo["Credit_Amount"]);
                                    //For obtaining NetAmount
                                    DataTable dt1 = m_TransactInfo.GetTransactionInfo(drVatTransactInfo["RowID"].ToString(), null, null, null);

                                    foreach (DataRow dr1 in dt1.Rows)
                                    {
                                        //if (Convert.ToDouble(dr1["Debit_Amount"]) > 0)
                                        if (Convert.ToDouble(dr1["Credit_Amount"]) > 0 && Convert.ToInt32(dr1["LedgerID"]) != 412)
                                        {
                                            TotalNetAmt += Convert.ToDouble(dr1["Credit_Amount"]);
                                        }
                                        if (Convert.ToDouble(dr1["Debit_Amount"]) > 0)
                                        {
                                            GrandTotalAmt += Convert.ToDouble(dr1["Debit_Amount"]);
                                        }
                                    }
                                    //TotalNetAmt = TotalNetAmt - VatAmt;
                                }
                                if (drVatTransactInfo["VoucherType"].ToString() == "CASH_RCPT")
                                {
                                    double VatAmt = Convert.ToDouble(drVatTransactInfo["Credit_Amount"]);
                                    TotalVatAmt += Convert.ToDouble(drVatTransactInfo["Credit_Amount"]);
                                    //For obtaining NetAmount
                                    DataTable dt1 = m_TransactInfo.GetTransactionInfo(drVatTransactInfo["RowID"].ToString(), null, null, null);

                                    foreach (DataRow dr1 in dt1.Rows)
                                    {
                                        //if (Convert.ToDouble(dr1["Debit_Amount"]) > 0)
                                        if (Convert.ToDouble(dr1["Credit_Amount"]) > 0 && Convert.ToInt32(dr1["LedgerID"]) != 412)
                                        {
                                            TotalNetAmt += Convert.ToDouble(dr1["Credit_Amount"]);
                                        }
                                        if (Convert.ToDouble(dr1["Debit_Amount"]) > 0)
                                        {
                                            GrandTotalAmt += Convert.ToDouble(dr1["Debit_Amount"]);
                                        }
                                    }
                                }
                                if (drVatTransactInfo["VoucherType"].ToString() == "BANK_RCPT")
                                {
                                    double VatAmt = Convert.ToDouble(drVatTransactInfo["Credit_Amount"]);
                                    TotalVatAmt += Convert.ToDouble(drVatTransactInfo["Credit_Amount"]);
                                    //For obtaining NetAmount
                                    DataTable dt1 = m_TransactInfo.GetTransactionInfo(drVatTransactInfo["RowID"].ToString(), null, null, null);

                                    foreach (DataRow dr1 in dt1.Rows)
                                    {
                                        //if (Convert.ToDouble(dr1["Debit_Amount"]) > 0)
                                        if (Convert.ToDouble(dr1["Credit_Amount"]) > 0 && Convert.ToInt32(dr1["LedgerID"]) != 412)
                                        {
                                            TotalNetAmt += Convert.ToDouble(dr1["Credit_Amount"]);
                                        }
                                        if (Convert.ToDouble(dr1["Debit_Amount"]) > 0)
                                        {
                                            GrandTotalAmt += Convert.ToDouble(dr1["Debit_Amount"]);
                                        }
                                    }
                                }
                            }
                            else
                            { lblvatcollected.Visible = false; }
                        
                        }
                        else if (Convert.ToDouble(drVatTransactInfo["Debit_Amount"].ToString()) > 0)
                        {
                            if (m_VATReport.Paid == true)
                            {
                                if (drVatTransactInfo["VoucherType"].ToString() == "PURCH")
                                {
                                    double VatAmt = Convert.ToDouble(drVatTransactInfo["Debit_Amount"]);
                                    TotalVatAmtPaid += Convert.ToDouble(drVatTransactInfo["Debit_Amount"]);
                                    //For obtaining NetAmount
                                    DataTable dt1 = m_TransactInfo.GetTransactionInfo(drVatTransactInfo["RowID"].ToString(), null, null, null);

                                    foreach (DataRow dr1 in dt1.Rows)
                                    {
                                        //if (Convert.ToDouble(dr1["Debit_Amount"]) > 0)
                                        if (Convert.ToDouble(dr1["Debit_Amount"]) > 0 && Convert.ToInt32(dr1["LedgerID"]) != 412)
                                        {
                                            TotalNetAmtPaid += Convert.ToDouble(dr1["Debit_Amount"]);
                                        }
                                        if (Convert.ToDouble(dr1["Credit_Amount"]) > 0)
                                        {
                                            GrandTotalAmtPaid += Convert.ToDouble(dr1["Credit_Amount"]);
                                        }
                                    }
                                }
                                if (drVatTransactInfo["VoucherType"].ToString() == "SLS_RTN")
                                {
                                    double VatAmt = Convert.ToDouble(drVatTransactInfo["Debit_Amount"]);
                                    TotalVatAmtPaid += Convert.ToDouble(drVatTransactInfo["Debit_Amount"]);
                                    //For obtaining NetAmount
                                    DataTable dt1 = m_TransactInfo.GetTransactionInfo(drVatTransactInfo["RowID"].ToString(), null, null, null);

                                    foreach (DataRow dr1 in dt1.Rows)
                                    {
                                        //if (Convert.ToDouble(dr1["Debit_Amount"]) > 0)
                                        if (Convert.ToDouble(dr1["Debit_Amount"]) > 0 && Convert.ToInt32(dr1["LedgerID"]) != 412)
                                        {
                                            TotalNetAmtPaid += Convert.ToDouble(dr1["Debit_Amount"]);
                                        }
                                        if (Convert.ToDouble(dr1["Credit_Amount"]) > 0)
                                        {
                                            GrandTotalAmtPaid += Convert.ToDouble(dr1["Credit_Amount"]);
                                        }
                                    }
                                }
                                if (drVatTransactInfo["VoucherType"].ToString() == "JRNL")
                                {
                                    double VatAmt = Convert.ToDouble(drVatTransactInfo["Debit_Amount"]);
                                    TotalVatAmtPaid += Convert.ToDouble(drVatTransactInfo["Debit_Amount"]);
                                    //For obtaining NetAmount
                                    DataTable dt1 = m_TransactInfo.GetTransactionInfo(drVatTransactInfo["RowID"].ToString(), null, null, null);

                                    foreach (DataRow dr1 in dt1.Rows)
                                    {
                                        //if (Convert.ToDouble(dr1["Debit_Amount"]) > 0)
                                        if (Convert.ToDouble(dr1["Debit_Amount"]) > 0 && Convert.ToInt32(dr1["LedgerID"]) != 412)
                                        {
                                            TotalNetAmtPaid += Convert.ToDouble(dr1["Debit_Amount"]);
                                        }
                                        if (Convert.ToDouble(dr1["Credit_Amount"]) > 0)
                                        {
                                            GrandTotalAmtPaid += Convert.ToDouble(dr1["Credit_Amount"]);
                                        }
                                    }

                                }

                                if (drVatTransactInfo["VoucherType"].ToString() == "CASH_PMNT")
                                {
                                    double VatAmt = Convert.ToDouble(drVatTransactInfo["Debit_Amount"]);
                                    TotalVatAmtPaid += Convert.ToDouble(drVatTransactInfo["Debit_Amount"]);
                                    //For obtaining NetAmount
                                    DataTable dt1 = m_TransactInfo.GetTransactionInfo(drVatTransactInfo["RowID"].ToString(), null, null, null);

                                    foreach (DataRow dr1 in dt1.Rows)
                                    {
                                        //if (Convert.ToDouble(dr1["Debit_Amount"]) > 0)
                                        if (Convert.ToDouble(dr1["Debit_Amount"]) > 0 && Convert.ToInt32(dr1["LedgerID"]) != 412)
                                        {
                                            TotalNetAmtPaid += Convert.ToDouble(dr1["Debit_Amount"]);
                                        }
                                        if (Convert.ToDouble(dr1["Credit_Amount"]) > 0)
                                        {
                                            GrandTotalAmtPaid += Convert.ToDouble(dr1["Credit_Amount"]);
                                        }
                                    }
                                }
                                if (drVatTransactInfo["VoucherType"].ToString() == "BANK_PMNT")
                                {
                                    double VatAmt = Convert.ToDouble(drVatTransactInfo["Debit_Amount"]);
                                    TotalVatAmtPaid += Convert.ToDouble(drVatTransactInfo["Debit_Amount"]);
                                    //For obtaining NetAmount
                                    DataTable dt1 = m_TransactInfo.GetTransactionInfo(drVatTransactInfo["RowID"].ToString(), null, null, null);

                                    foreach (DataRow dr1 in dt1.Rows)
                                    {
                                        //if (Convert.ToDouble(dr1["Debit_Amount"]) > 0)
                                        if (Convert.ToDouble(dr1["Debit_Amount"]) > 0 && Convert.ToInt32(dr1["LedgerID"]) != 412)
                                        {
                                            TotalNetAmtPaid += Convert.ToDouble(dr1["Debit_Amount"]);
                                        }
                                        if (Convert.ToDouble(dr1["Credit_Amount"]) > 0)
                                        {
                                            GrandTotalAmtPaid += Convert.ToDouble(dr1["Credit_Amount"]);
                                        }
                                    }
                                }
                                
                            }
                            else
                            {
                                lblvatpaid.Visible = false;
                            }

                        }
                        //if (drVatTransactInfo["VoucherType"].ToString()=="SALES")
                        //{
                        //    double VatAmt = Convert.ToDouble(drVatTransactInfo["Debit_Amount"]);
                        //    TotalVatAmt += Convert.ToDouble(drVatTransactInfo["Debit_Amount"]);
                        //    //For obtaining NetAmount
                        //    DataTable dt1 = m_TransactInfo.GetTransactionInfo(drVatTransactInfo["RowID"].ToString(), null, null, null);

                        //    foreach (DataRow dr1 in dt1.Rows)
                        //    {
                        //        //if (Convert.ToDouble(dr1["Debit_Amount"]) > 0)
                        //        if (Convert.ToDouble(dr1["Debit_Amount"]) > 0 && Convert.ToInt32(dr1["LedgerID"]) != 412)
                        //        {
                        //            TotalNetAmt += Convert.ToDouble(dr1["Debit_Amount"]);
                        //        }
                        //        if (Convert.ToDouble(dr1["Credit_Amount"]) > 0)
                        //        {
                        //            GrandTotalAmt += Convert.ToDouble(dr1["Credit_Amount"]);
                        //        }
                        //    }
                        //}
                        //if (drVatTransactInfo["VoucherType"].ToString() == "JRNL")
                        //{
                        //     double VatAmt = Convert.ToDouble(drVatTransactInfo["Credit_Amount"]);
                        //     TotalVatAmt += Convert.ToDouble(drVatTransactInfo["Credit_Amount"]);
                        //     //For obtaining NetAmount
                        //     DataTable dt1 = m_TransactInfo.GetTransactionInfo(drVatTransactInfo["RowID"].ToString(), null, null, null);

                        //     foreach (DataRow dr1 in dt1.Rows)
                        //     {
                        //         //if (Convert.ToDouble(dr1["Debit_Amount"]) > 0)
                        //         if (Convert.ToDouble(dr1["Debit_Amount"]) > 0 && Convert.ToInt32(dr1["LedgerID"]) != 412)
                        //         {
                        //             TotalNetAmt += Convert.ToDouble(dr1["Debit_Amount"]);
                        //         }
                        //         if (Convert.ToDouble(dr1["Credit_Amount"]) > 0)
                        //         {
                        //             GrandTotalAmt += Convert.ToDouble(dr1["Credit_Amount"]);
                        //         }
                        //     }
                        //     TotalNetAmt = TotalNetAmt - VatAmt;
                        //}
                        
                       
                      }                 

                //Code for the Total calculation
                if(m_VATReport.Collected==true)
                {
                    WriteVatSummary(TotalNetAmt.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), TotalVatAmt.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), GrandTotalAmt.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));
                }
                if (m_VATReport.Paid == true)
                {
                    WriteVatSummaryPaid(TotalNetAmtPaid.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), TotalVatAmtPaid.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), GrandTotalAmtPaid.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));
                }
            }
            #endregion

            #region Else For The Detail Section
            //Block for Detail of VATReport
            else if (m_VATReport.Detail == true)
            {

                VatDetailsRowsCount = 1;
                VatDetailsRowsCountPaid = 1;
                //Disable multiple selection
                grdVATReport.Selection.EnableMultiSelection = false;
                grdvatpaid.Selection.EnableMultiSelection = false;
                if (m_VATReport.Collected == true)
                {
                    grdVATReport.Redim(1, 9);
                    WriteHeaderForVatDetails();
                }
                if(m_VATReport.Paid==true)
                {
                grdvatpaid.Redim(1, 9);
                WriteHeaderForVatDetailsPaid();
                }
                //first collect  all the information from tblTransaction where ledgerID=154(LedgerID of VAT On Sale)
                Transaction m_TransactInfo = new Transaction();
                //int SalesLedgerID = AccountGroup.GetGroupIDFromGroupNumber(204);
                //int SalesLedgerID = AccountGroup.GetGroupIDFromGroupNumber(112);
                // DataTable dtTransactInfo = m_TransactInfo.GetLedgerTransact(SalesLedgerID.ToString());//This function get the information of VAT from tblTransaction with help of VAT On Sale ledgerID(154)
                //DataTable dtTransactInfo = m_TransactInfo.GetLedgerTransact("412");//This function get the information of VAT from tblTransaction with help of VAT On Sale ledgerID(154)
                DataTable dtTransactInfo = m_TransactInfo.GetLedgerTransactWithAccClassAndProjectID("412", TotalAccClassIDs, ProjectIDS);//This function get the information of VAT from tblTransaction with help of VAT On Sale ledgerID(154)
                double TotalNetAmt = 0;
                double TotalVatAmt = 0;
                double GrandTotalAmt = 0;
                double TotalNetAmtPaid = 0;
                double TotalVatAmtPaid = 0;
                double GrandTotalAmtPaid = 0;
                int SNo = 1;
                int SNoPaid = 1;
                foreach (DataRow drVatTransactInfo in dtTransactInfo.Rows)
                {
                    int rows = grdVATReport.Rows.Count;
                    grdVATReport.Rows.Insert(rows);
                    VType = drVatTransactInfo["VoucherType"].ToString();
                    //Check For The Different Voucher Types
                    if (Convert.ToDouble(drVatTransactInfo["Credit_Amount"].ToString()) > 0)//For Vat Collection Which We Have to Pay Like on Sales Invoice
                    {
                        if (m_VATReport.Collected == true)
                        {
                            //Start for VAtCollection
                            #region Check Here For The Sales Invoice
                            if (drVatTransactInfo["VoucherType"].ToString() == "SALES")
                            {
                                Sales m_Sale = new Sales();
                                DataTable dtSalesInvoiceMaster = m_Sale.GetSalesInvoiceMasterInfo(drVatTransactInfo["RowID"].ToString());//According to RowID which is equivalent to SalesInvoiceID in tblSalesInvoiceMaster,get the information of Voucher_No,Cash/party Account
                                string VoucherNo, LedgerName, NetAmount, TotalAmount, TransactDate;
                                VoucherNo = LedgerName = NetAmount = TotalAmount = TransactDate = "";//Initializing the varialbes
                                double tamt = 0;
                                double namt = 0;

                                foreach (DataRow drSalesInvoiceMaster in dtSalesInvoiceMaster.Rows)
                                {
                                    DataTable dt = Ledger.GetLedgerInfo(Convert.ToInt32(drSalesInvoiceMaster["CashPartyLedgerID"]), LangMgr.DefaultLanguage);
                                    if (dt.Rows.Count > 0)
                                    {
                                        DataRow dr = dt.Rows[0];
                                        LedgerName = (dr["LedName"].ToString());
                                    }
                                    VoucherNo = (drSalesInvoiceMaster["Voucher_No"].ToString());
                                }
                                double VatAmt = Convert.ToDouble(drVatTransactInfo["Credit_Amount"]);//
                                TotalVatAmt += Convert.ToDouble(drVatTransactInfo["Credit_Amount"]);

                                DataTable dt1 = m_TransactInfo.GetTransactionInfo(drVatTransactInfo["RowID"].ToString(), null, null, null);//
                                RowID = Convert.ToInt32(drVatTransactInfo["RowID"].ToString());
                                foreach (DataRow dr1 in dt1.Rows)
                                {
                                    TransactDate = Date.ToSystem(Convert.ToDateTime(dr1["TransactDate"].ToString()));
                                    if (Convert.ToDouble(dr1["Credit_Amount"]) > 0 && Convert.ToInt32(dr1["LedgerID"]) != 412)
                                    {
                                        //NetAmount = (dr1["Debit_Amount"].ToString());
                                        namt = namt + Convert.ToDouble(dr1["Credit_Amount"]);
                                        TotalNetAmt += Convert.ToDouble(dr1["Credit_Amount"]);
                                    }
                                    if (Convert.ToDouble(dr1["Debit_Amount"]) > 0)
                                    {
                                        tamt = tamt + Convert.ToDouble(dr1["Debit_Amount"]);
                                        //TotalAmount = (dr1["Credit_Amount"].ToString());
                                        GrandTotalAmt += Convert.ToDouble(dr1["Debit_Amount"]);
                                    }

                                }
                                WriteVatDetails(0, TransactDate, LedgerName, VoucherNo, namt.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), VatAmt.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), tamt.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), VType, RowID);

                                //  WriteVatDetails(0, TransactDate, LedgerName, VoucherNo, TotalNetAmt.ToString(), VatAmt.ToString(), TotalAmount);
                            }
                            #endregion
                            #region For Purchase Return
                            if (drVatTransactInfo["VoucherType"].ToString() == "PURCH_RTN")
                            {

                                PurchaseReturn m_purchasereturn = new BusinessLogic.PurchaseReturn();
                                DataTable dtPurchaseReturn = m_purchasereturn.GetPurchaseReturnMasterInfo(drVatTransactInfo["RowID"].ToString());


                                string VoucherNo, LedgerName, NetAmount, TotalAmount, TransactDate;
                                VoucherNo = LedgerName = NetAmount = TotalAmount = TransactDate = "";//Initializing the varialbes
                                double tamt = 0;
                                double namt = 0;

                                foreach (DataRow drPurchaseReturnMaster in dtPurchaseReturn.Rows)
                                {
                                    DataTable dt = Ledger.GetLedgerInfo(Convert.ToInt32(drPurchaseReturnMaster["CashPartyLedgerID"]), LangMgr.DefaultLanguage);
                                    if (dt.Rows.Count > 0)
                                    {
                                        DataRow dr = dt.Rows[0];
                                        LedgerName = (dr["LedName"].ToString());
                                    }
                                    VoucherNo = (drPurchaseReturnMaster["Voucher_No"].ToString());
                                }
                                double VatAmt = Convert.ToDouble(drVatTransactInfo["Credit_Amount"]);//
                                TotalVatAmt += Convert.ToDouble(drVatTransactInfo["Credit_Amount"]);

                                DataTable dt1 = m_TransactInfo.GetTransactionInfo(drVatTransactInfo["RowID"].ToString(), null, null, null);//
                                RowID = Convert.ToInt32(drVatTransactInfo["RowID"].ToString());
                                foreach (DataRow dr1 in dt1.Rows)
                                {
                                    TransactDate = Date.ToSystem(Convert.ToDateTime(dr1["TransactDate"].ToString()));
                                    if (Convert.ToDouble(dr1["Credit_Amount"]) > 0 && Convert.ToInt32(dr1["LedgerID"]) != 412)
                                    {
                                        //NetAmount = (dr1["Debit_Amount"].ToString());
                                        namt = namt + Convert.ToDouble(dr1["Credit_Amount"]);
                                        TotalNetAmt += Convert.ToDouble(dr1["Credit_Amount"]);
                                    }
                                    if (Convert.ToDouble(dr1["Debit_Amount"]) > 0)
                                    {
                                        tamt = tamt + Convert.ToDouble(dr1["Debit_Amount"]);
                                        //TotalAmount = (dr1["Credit_Amount"].ToString());
                                        GrandTotalAmt += Convert.ToDouble(dr1["Debit_Amount"]);
                                    }

                                }
                                WriteVatDetails(0, TransactDate, LedgerName, VoucherNo, namt.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), VatAmt.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), tamt.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), VType, RowID);
                            }
                            #endregion
                            #region For Journal Vat Entry
                            if (drVatTransactInfo["VoucherType"].ToString() == "JRNL")
                            {
                                //Sales m_Sale = new Sales();
                                Journal m_Journal = new Journal();
                                DataTable dtJournalMaster = m_Journal.GetJournalMasterInfo(drVatTransactInfo["RowID"].ToString());//According to RowID which is equivalent to SalesInvoiceID in tblSalesInvoiceMaster,get the information of Voucher_No,Cash/party Account
                                string VoucherNo, LedgerName, NetAmount, TotalAmount, TransactDate;
                                VoucherNo = LedgerName = NetAmount = TotalAmount = TransactDate = "";//Initializing the varialbes
                                double netvalue = 0;
                                double totalamt = 0;
                                foreach (DataRow drJournalMaster in dtJournalMaster.Rows)
                                {
                                    DataTable dt = Ledger.GetLedgerInfo(Convert.ToInt32(drJournalMaster["LedgerID"]), LangMgr.DefaultLanguage);
                                    if (dt.Rows.Count > 0)
                                    {
                                        DataRow dr = dt.Rows[0];
                                        LedgerName = (dr["LedName"].ToString());
                                    }
                                    VoucherNo = (drJournalMaster["Voucher_No"].ToString());
                                }
                                double VatAmt = Convert.ToDouble(drVatTransactInfo["Credit_Amount"]);//
                                TotalVatAmt += Convert.ToDouble(drVatTransactInfo["Credit_Amount"]);
                                //For obtaining NetAmount
                                //if (drVatTransactInfo["VoucherType"].ToString() == "SALES")
                                //{
                                DataTable dt1 = m_TransactInfo.GetTransactionInfo(drVatTransactInfo["RowID"].ToString(), null, null, null);//
                                RowID = Convert.ToInt32(drVatTransactInfo["RowID"].ToString());
                                foreach (DataRow dr1 in dt1.Rows)
                                {
                                    TransactDate = Date.ToSystem(Convert.ToDateTime(dr1["TransactDate"].ToString()));
                                    if (Convert.ToDouble(dr1["Credit_Amount"]) > 0)
                                    {

                                        netvalue = netvalue + Convert.ToDouble(dr1["Credit_Amount"].ToString());
                                        //NetAmount = NetAmount + Convert.ToDouble(dr1["Debit_Amount"].ToString());
                                        TotalNetAmt += Convert.ToDouble(dr1["Credit_Amount"]);
                                    }
                                    if (Convert.ToDouble(dr1["Debit_Amount"]) > 0)
                                    {
                                        // TotalAmount = (dr1["Credit_Amount"].ToString());
                                        totalamt = totalamt + Convert.ToDouble(dr1["Debit_Amount"]);
                                        GrandTotalAmt += Convert.ToDouble(dr1["Debit_Amount"]);
                                    }

                                }
                                netvalue = netvalue - VatAmt;
                                TotalNetAmt = TotalNetAmt - VatAmt;
                                WriteVatDetails(0, TransactDate, LedgerName, VoucherNo, netvalue.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), VatAmt.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), totalamt.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), VType, RowID);

                            }
                            #endregion
                            #region Check Here For The Cash Receipt
                            if (drVatTransactInfo["VoucherType"].ToString() == "CASH_RCPT")
                            {
                                //CashPayment m_CashPayment = new BusinessLogic.CashPayment();
                                CashReceipt m_CashReceipt = new BusinessLogic.CashReceipt();
                                //DataTable dtCashPaymentInvoiceMaster = m_CashPayment.GetCashPaymentMaster(Convert.ToInt32(drVatTransactInfo["RowID"].ToString()));
                                DataTable dtCashReceiptInvoiceMaster = m_CashReceipt.GetCashReceiptMaster(Convert.ToInt32(drVatTransactInfo["RowID"].ToString()));

                                string VoucherNo, LedgerName, NetAmount, TotalAmount, TransactDate;
                                VoucherNo = LedgerName = NetAmount = TotalAmount = TransactDate = "";//Initializing the varialbes
                                double tamt = 0;
                                double namt = 0;

                                foreach (DataRow drCashReceiptInvoiceMaster in dtCashReceiptInvoiceMaster.Rows)
                                {
                                    DataTable dt = Ledger.GetLedgerInfo(Convert.ToInt32(drCashReceiptInvoiceMaster["LedgerID"]), LangMgr.DefaultLanguage);
                                    if (dt.Rows.Count > 0)
                                    {
                                        DataRow dr = dt.Rows[0];
                                        LedgerName = (dr["LedName"].ToString());
                                    }
                                    VoucherNo = (drCashReceiptInvoiceMaster["Voucher_No"].ToString());
                                }
                                double VatAmt = Convert.ToDouble(drVatTransactInfo["Credit_Amount"]);//
                                TotalVatAmt += Convert.ToDouble(drVatTransactInfo["Credit_Amount"]);

                                DataTable dt1 = m_TransactInfo.GetTransactionInfo(drVatTransactInfo["RowID"].ToString(), null, null, null);//
                                RowID = Convert.ToInt32(drVatTransactInfo["RowID"].ToString());
                                foreach (DataRow dr1 in dt1.Rows)
                                {
                                    TransactDate = Date.ToSystem(Convert.ToDateTime(dr1["TransactDate"].ToString()));
                                    if (Convert.ToDouble(dr1["Credit_Amount"]) > 0 && Convert.ToInt32(dr1["LedgerID"]) != 412)
                                    {
                                        //NetAmount = (dr1["Debit_Amount"].ToString());
                                        namt = namt + Convert.ToDouble(dr1["Credit_Amount"]);
                                        TotalNetAmt += Convert.ToDouble(dr1["Credit_Amount"]);
                                    }
                                    if (Convert.ToDouble(dr1["Debit_Amount"]) > 0)
                                    {
                                        tamt = tamt + Convert.ToDouble(dr1["Debit_Amount"]);
                                        //TotalAmount = (dr1["Credit_Amount"].ToString());
                                        GrandTotalAmt += Convert.ToDouble(dr1["Debit_Amount"]);
                                    }

                                }
                                WriteVatDetails(0, TransactDate, LedgerName, VoucherNo, namt.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), VatAmt.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), tamt.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), VType, RowID);

                                //  WriteVatDetails(0, TransactDate, LedgerName, VoucherNo, TotalNetAmt.ToString(), VatAmt.ToString(), TotalAmount);
                            }
                            #endregion
                            #region Check Here For The Bank Receipt
                            if (drVatTransactInfo["VoucherType"].ToString() == "BANK_RCPT")
                            {
                                BankReceipt m_BankReceipt = new BusinessLogic.BankReceipt();
                                // Sales m_Sale = new Sales();
                                //DataTable dtSalesInvoiceMaster = m_Sale.GetSalesInvoiceMasterInfo(drVatTransactInfo["RowID"].ToString());//According to RowID which is equivalent to SalesInvoiceID in tblSalesInvoiceMaster,get the information of Voucher_No,Cash/party Account
                                DataTable dtBankReceiptInvoiceMaster = m_BankReceipt.GetBankReceiptMaster(Convert.ToInt32(drVatTransactInfo["RowID"].ToString()));
                                string VoucherNo, LedgerName, NetAmount, TotalAmount, TransactDate;
                                VoucherNo = LedgerName = NetAmount = TotalAmount = TransactDate = "";//Initializing the varialbes
                                double tamt = 0;
                                double namt = 0;

                                foreach (DataRow drBankReceiptInvoiceMaster in dtBankReceiptInvoiceMaster.Rows)
                                {
                                    DataTable dt = Ledger.GetLedgerInfo(Convert.ToInt32(drBankReceiptInvoiceMaster["LedgerID"]), LangMgr.DefaultLanguage);
                                    if (dt.Rows.Count > 0)
                                    {
                                        DataRow dr = dt.Rows[0];
                                        LedgerName = (dr["LedName"].ToString());
                                    }
                                    VoucherNo = (drBankReceiptInvoiceMaster["Voucher_No"].ToString());
                                }
                                double VatAmt = Convert.ToDouble(drVatTransactInfo["Credit_Amount"]);//
                                TotalVatAmt += Convert.ToDouble(drVatTransactInfo["Credit_Amount"]);

                                DataTable dt1 = m_TransactInfo.GetTransactionInfo(drVatTransactInfo["RowID"].ToString(), null, null, null);//
                                RowID = Convert.ToInt32(drVatTransactInfo["RowID"].ToString());
                                foreach (DataRow dr1 in dt1.Rows)
                                {
                                    TransactDate = Date.ToSystem(Convert.ToDateTime(dr1["TransactDate"].ToString()));
                                    if (Convert.ToDouble(dr1["Credit_Amount"]) > 0 && Convert.ToInt32(dr1["LedgerID"]) != 412)
                                    {
                                        //NetAmount = (dr1["Debit_Amount"].ToString());
                                        namt = namt + Convert.ToDouble(dr1["Credit_Amount"]);
                                        TotalNetAmt += Convert.ToDouble(dr1["Credit_Amount"]);
                                    }
                                    if (Convert.ToDouble(dr1["Debit_Amount"]) > 0)
                                    {
                                        tamt = tamt + Convert.ToDouble(dr1["Debit_Amount"]);
                                        //TotalAmount = (dr1["Credit_Amount"].ToString());
                                        GrandTotalAmt += Convert.ToDouble(dr1["Debit_Amount"]);
                                    }

                                }
                                WriteVatDetails(0, TransactDate, LedgerName, VoucherNo, namt.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), VatAmt.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), tamt.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), VType, RowID);

                                //  WriteVatDetails(0, TransactDate, LedgerName, VoucherNo, TotalNetAmt.ToString(), VatAmt.ToString(), TotalAmount);
                            }
                            #endregion
                            #region Previous Way
                            //#region Check Here For The Purchase Invoice
                            //if (drVatTransactInfo["VoucherType"].ToString() == "PURCH")
                            //{
                            //    //Sales m_Sale = new Sales();
                            //    Purchase m_Purchase = new BusinessLogic.Purchase();
                            //    // DataTable dtSalesInvoiceMaster = m_Sale.GetSalesInvoiceMasterInfo(drVatTransactInfo["RowID"].ToString());//According to RowID which is equivalent to SalesInvoiceID in tblSalesInvoiceMaster,get the information of Voucher_No,Cash/party Account
                            //    DataTable dtPurchaseInvoiceMaster = m_Purchase.GetPurchaseInvoiceMasterInfo(drVatTransactInfo["RowID"].ToString());
                            //    string VoucherNo, LedgerName, NetAmount, TotalAmount, TransactDate;
                            //    VoucherNo = LedgerName = NetAmount = TotalAmount = TransactDate = "";//Initializing the varialbes
                            //    double tamt = 0;
                            //    double namt = 0;

                            //    foreach (DataRow drPurchaseInvoiceMaster in dtPurchaseInvoiceMaster.Rows)
                            //    {
                            //        DataTable dt = Ledger.GetLedgerInfo(Convert.ToInt32(drPurchaseInvoiceMaster["CashPartyLedgerID"]), LangMgr.DefaultLanguage);
                            //        if (dt.Rows.Count > 0)
                            //        {
                            //            DataRow dr = dt.Rows[0];
                            //            LedgerName = (dr["LedName"].ToString());
                            //        }
                            //        VoucherNo = (drPurchaseInvoiceMaster["Voucher_No"].ToString());
                            //    }
                            //    double VatAmt = Convert.ToDouble(drVatTransactInfo["Debit_Amount"]);//
                            //    TotalVatAmt += Convert.ToDouble(drVatTransactInfo["Debit_Amount"]);

                            //    DataTable dt1 = m_TransactInfo.GetTransactionInfo(drVatTransactInfo["RowID"].ToString(), null, null, null);//
                            //    RowID = Convert.ToInt32(drVatTransactInfo["RowID"].ToString());
                            //    foreach (DataRow dr1 in dt1.Rows)
                            //    {
                            //        TransactDate = Date.ToSystem(Convert.ToDateTime(dr1["TransactDate"].ToString()));
                            //        if (Convert.ToDouble(dr1["Debit_Amount"]) > 0 && Convert.ToInt32(dr1["LedgerID"]) != 412)
                            //        {
                            //            //NetAmount = (dr1["Debit_Amount"].ToString());
                            //            namt = namt + Convert.ToDouble(dr1["Debit_Amount"]);
                            //            TotalNetAmt += Convert.ToDouble(dr1["Debit_Amount"]);
                            //        }
                            //        if (Convert.ToDouble(dr1["Credit_Amount"]) > 0)
                            //        {
                            //            tamt = tamt + Convert.ToDouble(dr1["Credit_Amount"]);
                            //            //TotalAmount = (dr1["Credit_Amount"].ToString());
                            //            GrandTotalAmt += Convert.ToDouble(dr1["Credit_Amount"]);
                            //        }

                            //    }
                            //    WriteVatDetails(0, TransactDate, LedgerName, VoucherNo, namt.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), VatAmt.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), tamt.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), VType, RowID);

                            //    //  WriteVatDetails(0, TransactDate, LedgerName, VoucherNo, TotalNetAmt.ToString(), VatAmt.ToString(), TotalAmount);
                            //}
                            //#endregion
                            //#region For Journal Vat Entry
                            //if (drVatTransactInfo["VoucherType"].ToString() == "JRNL")
                            //{
                            //    //Sales m_Sale = new Sales();
                            //    Journal m_Journal = new Journal();
                            //    DataTable dtJournalMaster = m_Journal.GetJournalMasterInfo(drVatTransactInfo["RowID"].ToString());//According to RowID which is equivalent to SalesInvoiceID in tblSalesInvoiceMaster,get the information of Voucher_No,Cash/party Account
                            //    string VoucherNo, LedgerName, NetAmount, TotalAmount, TransactDate;
                            //    VoucherNo = LedgerName = NetAmount = TotalAmount = TransactDate = "";//Initializing the varialbes
                            //    double netvalue = 0;
                            //    double totalamt = 0;
                            //    foreach (DataRow drJournalMaster in dtJournalMaster.Rows)
                            //    {
                            //        DataTable dt = Ledger.GetLedgerInfo(Convert.ToInt32(drJournalMaster["LedgerID"]), LangMgr.DefaultLanguage);
                            //        if (dt.Rows.Count > 0)
                            //        {
                            //            DataRow dr = dt.Rows[0];
                            //            LedgerName = (dr["LedName"].ToString());
                            //        }
                            //        VoucherNo = (drJournalMaster["Voucher_No"].ToString());
                            //    }
                            //    double VatAmt = Convert.ToDouble(drVatTransactInfo["Debit_Amount"]);//
                            //    TotalVatAmt += Convert.ToDouble(drVatTransactInfo["Debit_Amount"]);
                            //    //For obtaining NetAmount
                            //    //if (drVatTransactInfo["VoucherType"].ToString() == "SALES")
                            //    //{
                            //    DataTable dt1 = m_TransactInfo.GetTransactionInfo(drVatTransactInfo["RowID"].ToString(), null, null, null);//
                            //    RowID = Convert.ToInt32(drVatTransactInfo["RowID"].ToString());
                            //    foreach (DataRow dr1 in dt1.Rows)
                            //    {
                            //        TransactDate = Date.ToSystem(Convert.ToDateTime(dr1["TransactDate"].ToString()));
                            //        if (Convert.ToDouble(dr1["Debit_Amount"]) > 0)
                            //        {

                            //            netvalue = netvalue + Convert.ToDouble(dr1["Debit_Amount"].ToString());
                            //            //NetAmount = NetAmount + Convert.ToDouble(dr1["Debit_Amount"].ToString());
                            //            TotalNetAmt += Convert.ToDouble(dr1["Debit_Amount"]);
                            //        }
                            //        if (Convert.ToDouble(dr1["Credit_Amount"]) > 0)
                            //        {
                            //            // TotalAmount = (dr1["Credit_Amount"].ToString());
                            //            totalamt = totalamt + Convert.ToDouble(dr1["Credit_Amount"]);
                            //            GrandTotalAmt += Convert.ToDouble(dr1["Credit_Amount"]);
                            //        }

                            //    }
                            //    netvalue = netvalue - VatAmt;
                            //    TotalNetAmt = TotalNetAmt - VatAmt;
                            //    WriteVatDetails(0, TransactDate, LedgerName, VoucherNo, netvalue.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), VatAmt.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), totalamt.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), VType, RowID);


                            //}
                            //#endregion
                            //#region Check Here For The Cash Receipt
                            //if (drVatTransactInfo["VoucherType"].ToString() == "CASH_RCPT")
                            //{
                            //    //CashPayment m_CashPayment = new BusinessLogic.CashPayment();
                            //    CashReceipt m_CashReceipt = new BusinessLogic.CashReceipt();
                            //    //DataTable dtCashPaymentInvoiceMaster = m_CashPayment.GetCashPaymentMaster(Convert.ToInt32(drVatTransactInfo["RowID"].ToString()));
                            //    DataTable dtCashReceiptInvoiceMaster = m_CashReceipt.GetCashReceiptMaster(Convert.ToInt32(drVatTransactInfo["RowID"].ToString()));

                            //    string VoucherNo, LedgerName, NetAmount, TotalAmount, TransactDate;
                            //    VoucherNo = LedgerName = NetAmount = TotalAmount = TransactDate = "";//Initializing the varialbes
                            //    double tamt = 0;
                            //    double namt = 0;

                            //    foreach (DataRow drCashReceiptInvoiceMaster in dtCashReceiptInvoiceMaster.Rows)
                            //    {
                            //        DataTable dt = Ledger.GetLedgerInfo(Convert.ToInt32(drCashReceiptInvoiceMaster["LedgerID"]), LangMgr.DefaultLanguage);
                            //        if (dt.Rows.Count > 0)
                            //        {
                            //            DataRow dr = dt.Rows[0];
                            //            LedgerName = (dr["LedName"].ToString());
                            //        }
                            //        VoucherNo = (drCashReceiptInvoiceMaster["Voucher_No"].ToString());
                            //    }
                            //    double VatAmt = Convert.ToDouble(drVatTransactInfo["Debit_Amount"]);//
                            //    TotalVatAmt += Convert.ToDouble(drVatTransactInfo["Debit_Amount"]);

                            //    DataTable dt1 = m_TransactInfo.GetTransactionInfo(drVatTransactInfo["RowID"].ToString(), null, null, null);//
                            //    RowID = Convert.ToInt32(drVatTransactInfo["RowID"].ToString());
                            //    foreach (DataRow dr1 in dt1.Rows)
                            //    {
                            //        TransactDate = Date.ToSystem(Convert.ToDateTime(dr1["TransactDate"].ToString()));
                            //        if (Convert.ToDouble(dr1["Debit_Amount"]) > 0 && Convert.ToInt32(dr1["LedgerID"]) != 412)
                            //        {
                            //            //NetAmount = (dr1["Debit_Amount"].ToString());
                            //            namt = namt + Convert.ToDouble(dr1["Debit_Amount"]);
                            //            TotalNetAmt += Convert.ToDouble(dr1["Debit_Amount"]);
                            //        }
                            //        if (Convert.ToDouble(dr1["Credit_Amount"]) > 0)
                            //        {
                            //            tamt = tamt + Convert.ToDouble(dr1["Credit_Amount"]);
                            //            //TotalAmount = (dr1["Credit_Amount"].ToString());
                            //            GrandTotalAmt += Convert.ToDouble(dr1["Credit_Amount"]);
                            //        }

                            //    }
                            //    WriteVatDetails(0, TransactDate, LedgerName, VoucherNo, namt.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), VatAmt.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), tamt.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), VType, RowID);

                            //    //  WriteVatDetails(0, TransactDate, LedgerName, VoucherNo, TotalNetAmt.ToString(), VatAmt.ToString(), TotalAmount);
                            //}
                            //#endregion
                            //#region Check Here For The Bank Receipt
                            //if (drVatTransactInfo["VoucherType"].ToString() == "BANK_RCPT")
                            //{
                            //    BankReceipt m_BankReceipt = new BusinessLogic.BankReceipt();
                            //    // Sales m_Sale = new Sales();
                            //    //DataTable dtSalesInvoiceMaster = m_Sale.GetSalesInvoiceMasterInfo(drVatTransactInfo["RowID"].ToString());//According to RowID which is equivalent to SalesInvoiceID in tblSalesInvoiceMaster,get the information of Voucher_No,Cash/party Account
                            //    DataTable dtBankReceiptInvoiceMaster = m_BankReceipt.GetBankReceiptMaster(Convert.ToInt32(drVatTransactInfo["RowID"].ToString()));
                            //    string VoucherNo, LedgerName, NetAmount, TotalAmount, TransactDate;
                            //    VoucherNo = LedgerName = NetAmount = TotalAmount = TransactDate = "";//Initializing the varialbes
                            //    double tamt = 0;
                            //    double namt = 0;

                            //    foreach (DataRow drBankReceiptInvoiceMaster in dtBankReceiptInvoiceMaster.Rows)
                            //    {
                            //        DataTable dt = Ledger.GetLedgerInfo(Convert.ToInt32(drBankReceiptInvoiceMaster["LedgerID"]), LangMgr.DefaultLanguage);
                            //        if (dt.Rows.Count > 0)
                            //        {
                            //            DataRow dr = dt.Rows[0];
                            //            LedgerName = (dr["LedName"].ToString());
                            //        }
                            //        VoucherNo = (drBankReceiptInvoiceMaster["Voucher_No"].ToString());
                            //    }
                            //    double VatAmt = Convert.ToDouble(drVatTransactInfo["Debit_Amount"]);//
                            //    TotalVatAmt += Convert.ToDouble(drVatTransactInfo["Debit_Amount"]);

                            //    DataTable dt1 = m_TransactInfo.GetTransactionInfo(drVatTransactInfo["RowID"].ToString(), null, null, null);//
                            //    RowID = Convert.ToInt32(drVatTransactInfo["RowID"].ToString());
                            //    foreach (DataRow dr1 in dt1.Rows)
                            //    {
                            //        TransactDate = Date.ToSystem(Convert.ToDateTime(dr1["TransactDate"].ToString()));
                            //        if (Convert.ToDouble(dr1["Debit_Amount"]) > 0 && Convert.ToInt32(dr1["LedgerID"]) != 412)
                            //        {
                            //            //NetAmount = (dr1["Debit_Amount"].ToString());
                            //            namt = namt + Convert.ToDouble(dr1["Debit_Amount"]);
                            //            TotalNetAmt += Convert.ToDouble(dr1["Debit_Amount"]);
                            //        }
                            //        if (Convert.ToDouble(dr1["Credit_Amount"]) > 0)
                            //        {
                            //            tamt = tamt + Convert.ToDouble(dr1["Credit_Amount"]);
                            //            //TotalAmount = (dr1["Credit_Amount"].ToString());
                            //            GrandTotalAmt += Convert.ToDouble(dr1["Credit_Amount"]);
                            //        }

                            //    }
                            //    WriteVatDetails(0, TransactDate, LedgerName, VoucherNo, namt.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), VatAmt.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), tamt.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), VType, RowID);

                            //    //  WriteVatDetails(0, TransactDate, LedgerName, VoucherNo, TotalNetAmt.ToString(), VatAmt.ToString(), TotalAmount);
                            //}
                            //#endregion
                            #endregion
                        }
                        else
                        {
                            lblvatcollected.Visible = false;
                        }

                    }
                    else if (Convert.ToDouble(drVatTransactInfo["Debit_Amount"].ToString()) > 0)//For Vat Paid like on we paid vat for purchase of certain products
                    {
                        if (m_VATReport.Paid == true)
                        {
                           
                            //Start For VAT Paid
                            #region Check Here For The Purchase Invoice
                            if (drVatTransactInfo["VoucherType"].ToString() == "PURCH")
                            {
                               // string Party = "";
                                Purchase m_Purchase = new BusinessLogic.Purchase();
                                // DataTable dtSalesInvoiceMaster = m_Sale.GetSalesInvoiceMasterInfo(drVatTransactInfo["RowID"].ToString());//According to RowID which is equivalent to SalesInvoiceID in tblSalesInvoiceMaster,get the information of Voucher_No,Cash/party Account
                                DataTable dtPurchaseInvoiceMaster = m_Purchase.GetPurchaseInvoiceMasterInfo(drVatTransactInfo["RowID"].ToString());
                                string VoucherNo, LedgerName, NetAmount, TotalAmount, TransactDate;
                                VoucherNo = LedgerName = NetAmount = TotalAmount = TransactDate = "";//Initializing the varialbes
                                double tamt = 0;
                                double namt = 0;

                                foreach (DataRow drPurchaseInvoiceMaster in dtPurchaseInvoiceMaster.Rows)
                                {
                                    DataTable dt = Ledger.GetLedgerInfo(Convert.ToInt32(drPurchaseInvoiceMaster["CashPartyLedgerID"]), LangMgr.DefaultLanguage);
                                    if (dt.Rows.Count > 0)
                                    {
                                        DataRow dr = dt.Rows[0];
                                        LedgerName = (dr["LedName"].ToString());
                                    }
                                    VoucherNo = (drPurchaseInvoiceMaster["Voucher_No"].ToString());
                                   // Party=drPurchaseInvoiceMaster["Voucher_No"].ToString()
                                }
                                double VatAmt = Convert.ToDouble(drVatTransactInfo["Debit_Amount"]);//
                                TotalVatAmtPaid += Convert.ToDouble(drVatTransactInfo["Debit_Amount"]);

                                DataTable dt1 = m_TransactInfo.GetTransactionInfo(drVatTransactInfo["RowID"].ToString(), null, null, null);//
                                RowID = Convert.ToInt32(drVatTransactInfo["RowID"].ToString());
                                foreach (DataRow dr1 in dt1.Rows)
                                {
                                    TransactDate = Date.ToSystem(Convert.ToDateTime(dr1["TransactDate"].ToString()));
                                    if (Convert.ToDouble(dr1["Debit_Amount"]) > 0 && Convert.ToInt32(dr1["LedgerID"]) != 412)
                                    {
                                        //NetAmount = (dr1["Debit_Amount"].ToString());
                                        namt = namt + Convert.ToDouble(dr1["Debit_Amount"]);
                                        TotalNetAmtPaid += Convert.ToDouble(dr1["Debit_Amount"]);
                                    }
                                    if (Convert.ToDouble(dr1["Credit_Amount"]) > 0)
                                    {
                                        tamt = tamt + Convert.ToDouble(dr1["Credit_Amount"]);
                                        //TotalAmount = (dr1["Credit_Amount"].ToString());
                                        GrandTotalAmtPaid += Convert.ToDouble(dr1["Credit_Amount"]);
                                    }

                                }
                                WriteVatDetailsPaid(0, TransactDate, LedgerName, VoucherNo, namt.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), VatAmt.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), tamt.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), VType, RowID);

                                //  WriteVatDetails(0, TransactDate, LedgerName, VoucherNo, TotalNetAmt.ToString(), VatAmt.ToString(), TotalAmount);
                            }
                            #endregion
                            #region For Sales Return
                            if (drVatTransactInfo["VoucherType"].ToString() == "SLS_RTN")
                            {


                                SalesReturn m_SalesReturn = new BusinessLogic.SalesReturn();
                                DataTable dtSalesReturnMaster = m_SalesReturn.GetSalesReturnMasterInfo(drVatTransactInfo["RowID"].ToString());
                                string VoucherNo, LedgerName, NetAmount, TotalAmount, TransactDate;
                                VoucherNo = LedgerName = NetAmount = TotalAmount = TransactDate = "";//Initializing the varialbes
                                double tamt = 0;
                                double namt = 0;

                                foreach (DataRow drSalesReturnMaster in dtSalesReturnMaster.Rows)
                                {
                                    DataTable dt = Ledger.GetLedgerInfo(Convert.ToInt32(drSalesReturnMaster["CashPartyLedgerID"]), LangMgr.DefaultLanguage);
                                    if (dt.Rows.Count > 0)
                                    {
                                        DataRow dr = dt.Rows[0];
                                        LedgerName = (dr["LedName"].ToString());
                                    }
                                    VoucherNo = (drSalesReturnMaster["Voucher_No"].ToString());
                                }
                                double VatAmt = Convert.ToDouble(drVatTransactInfo["Debit_Amount"]);//
                                TotalVatAmtPaid += Convert.ToDouble(drVatTransactInfo["Debit_Amount"]);

                                DataTable dt1 = m_TransactInfo.GetTransactionInfo(drVatTransactInfo["RowID"].ToString(), null, null, null);//
                                RowID = Convert.ToInt32(drVatTransactInfo["RowID"].ToString());
                                foreach (DataRow dr1 in dt1.Rows)
                                {
                                    TransactDate = Date.ToSystem(Convert.ToDateTime(dr1["TransactDate"].ToString()));
                                    if (Convert.ToDouble(dr1["Debit_Amount"]) > 0 && Convert.ToInt32(dr1["LedgerID"]) != 412)
                                    {
                                        //NetAmount = (dr1["Debit_Amount"].ToString());
                                        namt = namt + Convert.ToDouble(dr1["Debit_Amount"]);
                                        TotalNetAmtPaid += Convert.ToDouble(dr1["Debit_Amount"]);
                                    }
                                    if (Convert.ToDouble(dr1["Credit_Amount"]) > 0)
                                    {
                                        tamt = tamt + Convert.ToDouble(dr1["Credit_Amount"]);
                                        //TotalAmount = (dr1["Credit_Amount"].ToString());
                                        GrandTotalAmtPaid += Convert.ToDouble(dr1["Credit_Amount"]);
                                    }

                                }
                                WriteVatDetailsPaid(0, TransactDate, LedgerName, VoucherNo, namt.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), VatAmt.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), tamt.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), VType, RowID);
                            }
                            #endregion
                            #region For Journal Vat Entry
                            if (drVatTransactInfo["VoucherType"].ToString() == "JRNL")
                            {
                                //Sales m_Sale = new Sales();
                                Journal m_Journal = new Journal();
                                DataTable dtJournalMaster = m_Journal.GetJournalMasterInfo(drVatTransactInfo["RowID"].ToString());//According to RowID which is equivalent to SalesInvoiceID in tblSalesInvoiceMaster,get the information of Voucher_No,Cash/party Account
                                string VoucherNo, LedgerName, NetAmount, TotalAmount, TransactDate;
                                VoucherNo = LedgerName = NetAmount = TotalAmount = TransactDate = "";//Initializing the varialbes
                                double netvalue = 0;
                                double totalamt = 0;
                                foreach (DataRow drJournalMaster in dtJournalMaster.Rows)
                                {
                                    DataTable dt = Ledger.GetLedgerInfo(Convert.ToInt32(drJournalMaster["LedgerID"]), LangMgr.DefaultLanguage);
                                    if (dt.Rows.Count > 0)
                                    {
                                        DataRow dr = dt.Rows[0];
                                        LedgerName = (dr["LedName"].ToString());
                                    }
                                    VoucherNo = (drJournalMaster["Voucher_No"].ToString());
                                }
                                double VatAmt = Convert.ToDouble(drVatTransactInfo["Debit_Amount"]);//
                                TotalVatAmtPaid += Convert.ToDouble(drVatTransactInfo["Debit_Amount"]);
                                //For obtaining NetAmount
                                //if (drVatTransactInfo["VoucherType"].ToString() == "SALES")
                                //{
                                DataTable dt1 = m_TransactInfo.GetTransactionInfo(drVatTransactInfo["RowID"].ToString(), null, null, null);//
                                RowID = Convert.ToInt32(drVatTransactInfo["RowID"].ToString());
                                foreach (DataRow dr1 in dt1.Rows)
                                {
                                    TransactDate = Date.ToSystem(Convert.ToDateTime(dr1["TransactDate"].ToString()));
                                    if (Convert.ToDouble(dr1["Debit_Amount"]) > 0)
                                    {

                                        netvalue = netvalue + Convert.ToDouble(dr1["Debit_Amount"].ToString());
                                        //NetAmount = NetAmount + Convert.ToDouble(dr1["Debit_Amount"].ToString());
                                        TotalNetAmtPaid += Convert.ToDouble(dr1["Debit_Amount"]);
                                    }
                                    if (Convert.ToDouble(dr1["Credit_Amount"]) > 0)
                                    {
                                        // TotalAmount = (dr1["Credit_Amount"].ToString());
                                        totalamt = totalamt + Convert.ToDouble(dr1["Credit_Amount"]);
                                        GrandTotalAmtPaid += Convert.ToDouble(dr1["Credit_Amount"]);
                                    }

                                }
                                netvalue = netvalue - VatAmt;
                                TotalNetAmt = TotalNetAmt - VatAmt;
                                WriteVatDetailsPaid(0, TransactDate, LedgerName, VoucherNo, netvalue.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), VatAmt.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), totalamt.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), VType, RowID);


                            }
                            #endregion
                            #region Check Here For The Cash Payment
                            if (drVatTransactInfo["VoucherType"].ToString() == "CASH_PMNT")
                            {
                                CashPayment m_CashPayment = new BusinessLogic.CashPayment();

                                DataTable dtCashPaymentInvoiceMaster = m_CashPayment.GetCashPaymentMaster(Convert.ToInt32(drVatTransactInfo["RowID"].ToString()));

                                string VoucherNo, LedgerName, NetAmount, TotalAmount, TransactDate;
                                VoucherNo = LedgerName = NetAmount = TotalAmount = TransactDate = "";//Initializing the varialbes
                                double tamt = 0;
                                double namt = 0;

                                foreach (DataRow drCashPaymentInvoiceMaster in dtCashPaymentInvoiceMaster.Rows)
                                {
                                    DataTable dt = Ledger.GetLedgerInfo(Convert.ToInt32(drCashPaymentInvoiceMaster["LedgerID"]), LangMgr.DefaultLanguage);
                                    if (dt.Rows.Count > 0)
                                    {
                                        DataRow dr = dt.Rows[0];
                                        LedgerName = (dr["LedName"].ToString());
                                    }
                                    VoucherNo = (drCashPaymentInvoiceMaster["Voucher_No"].ToString());
                                }
                                double VatAmt = Convert.ToDouble(drVatTransactInfo["Debit_Amount"]);//
                                TotalVatAmtPaid += Convert.ToDouble(drVatTransactInfo["Debit_Amount"]);

                                DataTable dt1 = m_TransactInfo.GetTransactionInfo(drVatTransactInfo["RowID"].ToString(), null, null, null);//
                                RowID = Convert.ToInt32(drVatTransactInfo["RowID"].ToString());
                                foreach (DataRow dr1 in dt1.Rows)
                                {
                                    TransactDate = Date.ToSystem(Convert.ToDateTime(dr1["TransactDate"].ToString()));
                                    if (Convert.ToDouble(dr1["Debit_Amount"]) > 0 && Convert.ToInt32(dr1["LedgerID"]) != 412)
                                    {
                                        //NetAmount = (dr1["Debit_Amount"].ToString());
                                        namt = namt + Convert.ToDouble(dr1["Debit_Amount"]);
                                        TotalNetAmtPaid += Convert.ToDouble(dr1["Debit_Amount"]);
                                    }
                                    if (Convert.ToDouble(dr1["Credit_Amount"]) > 0)
                                    {
                                        tamt = tamt + Convert.ToDouble(dr1["Credit_Amount"]);
                                        //TotalAmount = (dr1["Credit_Amount"].ToString());
                                        GrandTotalAmtPaid += Convert.ToDouble(dr1["Credit_Amount"]);
                                    }

                                }
                                WriteVatDetailsPaid(0, TransactDate, LedgerName, VoucherNo, namt.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), VatAmt.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), tamt.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), VType, RowID);

                                //  WriteVatDetails(0, TransactDate, LedgerName, VoucherNo, TotalNetAmt.ToString(), VatAmt.ToString(), TotalAmount);
                            }
                            #endregion
                            #region Check Here For The Bank Payment
                            if (drVatTransactInfo["VoucherType"].ToString() == "BANK_PMNT")
                            {
                                BankPayment m_BankPayment = new BusinessLogic.BankPayment();
                                // Sales m_Sale = new Sales();
                                //DataTable dtSalesInvoiceMaster = m_Sale.GetSalesInvoiceMasterInfo(drVatTransactInfo["RowID"].ToString());//According to RowID which is equivalent to SalesInvoiceID in tblSalesInvoiceMaster,get the information of Voucher_No,Cash/party Account
                                DataTable dtBankPaymentInvoiceMaster = m_BankPayment.GetBankPaymentMaster(Convert.ToInt32(drVatTransactInfo["RowID"].ToString()));
                                string VoucherNo, LedgerName, NetAmount, TotalAmount, TransactDate;
                                VoucherNo = LedgerName = NetAmount = TotalAmount = TransactDate = "";//Initializing the varialbes
                                double tamt = 0;
                                double namt = 0;

                                foreach (DataRow drBankPaymentInvoiceMaster in dtBankPaymentInvoiceMaster.Rows)
                                {
                                    DataTable dt = Ledger.GetLedgerInfo(Convert.ToInt32(drBankPaymentInvoiceMaster["LedgerID"]), LangMgr.DefaultLanguage);
                                    if (dt.Rows.Count > 0)
                                    {
                                        DataRow dr = dt.Rows[0];
                                        LedgerName = (dr["LedName"].ToString());
                                    }
                                    VoucherNo = (drBankPaymentInvoiceMaster["Voucher_No"].ToString());
                                }
                                double VatAmt = Convert.ToDouble(drVatTransactInfo["Debit_Amount"]);//
                                TotalVatAmtPaid += Convert.ToDouble(drVatTransactInfo["Debit_Amount"]);

                                DataTable dt1 = m_TransactInfo.GetTransactionInfo(drVatTransactInfo["RowID"].ToString(), null, null, null);//
                                RowID = Convert.ToInt32(drVatTransactInfo["RowID"].ToString());
                                foreach (DataRow dr1 in dt1.Rows)
                                {
                                    TransactDate = Date.ToSystem(Convert.ToDateTime(dr1["TransactDate"].ToString()));
                                    if (Convert.ToDouble(dr1["Debit_Amount"]) > 0 && Convert.ToInt32(dr1["LedgerID"]) != 412)
                                    {
                                        //NetAmount = (dr1["Debit_Amount"].ToString());
                                        namt = namt + Convert.ToDouble(dr1["Debit_Amount"]);
                                        TotalNetAmtPaid += Convert.ToDouble(dr1["Debit_Amount"]);
                                    }
                                    if (Convert.ToDouble(dr1["Credit_Amount"]) > 0)
                                    {
                                        tamt = tamt + Convert.ToDouble(dr1["Credit_Amount"]);
                                        //TotalAmount = (dr1["Credit_Amount"].ToString());
                                        GrandTotalAmtPaid += Convert.ToDouble(dr1["Credit_Amount"]);
                                    }

                                }
                                WriteVatDetailsPaid(0, TransactDate, LedgerName, VoucherNo, namt.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), VatAmt.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), tamt.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), VType, RowID);

                                //  WriteVatDetails(0, TransactDate, LedgerName, VoucherNo, TotalNetAmt.ToString(), VatAmt.ToString(), TotalAmount);
                            }
                            #endregion
                        }
                        else
                        {
                            lblvatpaid.Visible = false;
                        }

                    }
                   // break;
                }
                    ////Code for the Total calculation
                    if (m_VATReport.Collected == true)
                    {
                        if (TotalNetAmt != 0)
                        {
                            WriteVatDetails(0, "", "TOTAL", "", TotalNetAmt.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), TotalVatAmt.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), GrandTotalAmt.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), " ", 0);
                        }
                    }
                    if (m_VATReport.Paid == true)
                    {
                        if (TotalNetAmt != 0)
                        {
                            WriteVatDetailsPaid(0, "", "TOTAL", "", TotalNetAmtPaid.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), TotalVatAmtPaid.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), GrandTotalAmtPaid.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), " ", 0);
                        }
                    }
                    double vatpayable =Convert.ToDouble( TotalVatAmt.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces))) -Convert.ToDouble( TotalVatAmtPaid.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));
                    lblVatpayable.Text = vatpayable.ToString();  
  
                    #region Check Here For The Sales Invoice unused code
                    //if (drVatTransactInfo["VoucherType"].ToString()=="SALES")
                    //{
                    //Sales m_Sale = new Sales();
                    //DataTable dtSalesInvoiceMaster = m_Sale.GetSalesInvoiceMasterInfo(drVatTransactInfo["RowID"].ToString());//According to RowID which is equivalent to SalesInvoiceID in tblSalesInvoiceMaster,get the information of Voucher_No,Cash/party Account
                    //string VoucherNo, LedgerName, NetAmount, TotalAmount,TransactDate;
                    //VoucherNo=LedgerName=NetAmount=TotalAmount=TransactDate= "";//Initializing the varialbes
                    //double tamt = 0;
                    //double namt = 0;
                   
                    //foreach (DataRow drSalesInvoiceMaster in dtSalesInvoiceMaster.Rows)
                    //{
                    //    DataTable dt = Ledger.GetLedgerInfo(Convert.ToInt32(drSalesInvoiceMaster["CashPartyLedgerID"]), LangMgr.DefaultLanguage);
                    //    if (dt.Rows.Count > 0)
                    //    {
                    //        DataRow dr = dt.Rows[0];
                    //        LedgerName = (dr["LedName"].ToString());
                    //    }
                    //    VoucherNo = (drSalesInvoiceMaster["Voucher_No"].ToString());
                    //}
                    //double VatAmt = Convert.ToDouble(drVatTransactInfo["Debit_Amount"]);//
                    //TotalVatAmt += Convert.ToDouble(drVatTransactInfo["Debit_Amount"]);
                    ////double VatAmt = Convert.ToDouble(drVatTransactInfo["Credit_Amount"]);//
                    ////TotalVatAmt += Convert.ToDouble(drVatTransactInfo["Credit_Amount"]);
                    ////For obtaining NetAmount
                    ////if (drVatTransactInfo["VoucherType"].ToString() == "SALES")
                    ////{
                    //    DataTable dt1 = m_TransactInfo.GetTransactionInfo(drVatTransactInfo["RowID"].ToString(), null, null, null);//
                    //    RowID =Convert.ToInt32( drVatTransactInfo["RowID"].ToString());
                    //    foreach (DataRow dr1 in dt1.Rows)
                    //    {
                    //        TransactDate =Date.ToSystem(Convert.ToDateTime( dr1["TransactDate"].ToString()));
                    //         if (Convert.ToDouble(dr1["Debit_Amount"]) > 0 && Convert.ToInt32(dr1["LedgerID"]) != 412)
                    //        //if (Convert.ToDouble(dr1["Debit_Amount"]) > 0)
                    //        {
                    //            //NetAmount = (dr1["Debit_Amount"].ToString());
                    //            namt = namt + Convert.ToDouble(dr1["Debit_Amount"]);
                    //            TotalNetAmt += Convert.ToDouble(dr1["Debit_Amount"]);
                    //        }
                    //        if (Convert.ToDouble(dr1["Credit_Amount"]) > 0)
                    //        {
                    //            tamt = tamt + Convert.ToDouble(dr1["Credit_Amount"]);
                    //            //TotalAmount = (dr1["Credit_Amount"].ToString());
                    //            GrandTotalAmt += Convert.ToDouble(dr1["Credit_Amount"]);
                    //        }
                            
                    //    }
                        //WriteVatDetails(0, TransactDate, LedgerName, VoucherNo, namt.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), VatAmt.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), tamt.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)),VType,RowID);
                        //WriteVatDetails(0, TransactDate, LedgerName, VoucherNo, TotalNetAmt.ToString(), VatAmt.ToString(), TotalAmount);
                    //}
                  //  #endregion
                   // #region For Journal Vat Entry
                    //if (drVatTransactInfo["VoucherType"].ToString() == "JRNL")
                    //{
                    //    //Sales m_Sale = new Sales();
                    //    Journal m_Journal = new Journal();
                    //    DataTable dtJournalMaster = m_Journal.GetJournalMasterInfo(drVatTransactInfo["RowID"].ToString());//According to RowID which is equivalent to SalesInvoiceID in tblSalesInvoiceMaster,get the information of Voucher_No,Cash/party Account
                    //    string VoucherNo, LedgerName, NetAmount, TotalAmount, TransactDate;
                    //    VoucherNo = LedgerName = NetAmount = TotalAmount = TransactDate = "";//Initializing the varialbes
                    //    double netvalue=0;
                    //    double totalamt = 0;
                    //    foreach (DataRow drJournalMaster in dtJournalMaster.Rows)
                    //    {
                    //        DataTable dt = Ledger.GetLedgerInfo(Convert.ToInt32(drJournalMaster["LedgerID"]), LangMgr.DefaultLanguage);
                    //        if (dt.Rows.Count > 0)
                    //        {
                    //            DataRow dr = dt.Rows[0];
                    //            LedgerName = (dr["LedName"].ToString());
                    //        }
                    //        VoucherNo = (drJournalMaster["Voucher_No"].ToString());
                    //    }
                    //    double VatAmt = Convert.ToDouble(drVatTransactInfo["Credit_Amount"]);//
                    //    TotalVatAmt += Convert.ToDouble(drVatTransactInfo["Credit_Amount"]);
                    //    //For obtaining NetAmount
                    //    //if (drVatTransactInfo["VoucherType"].ToString() == "SALES")
                    //    //{
                    //    DataTable dt1 = m_TransactInfo.GetTransactionInfo(drVatTransactInfo["RowID"].ToString(), null, null, null);//
                    //    RowID = Convert.ToInt32(drVatTransactInfo["RowID"].ToString());
                    //    foreach (DataRow dr1 in dt1.Rows)
                    //    {
                    //        //TransactDate = dr1["TransactDate"].ToString();
                    //        TransactDate = Date.ToSystem(Convert.ToDateTime(dr1["TransactDate"].ToString()));
                    //        // if (Convert.ToDouble(dr1["Debit_Amount"]) > 0 && Convert.ToInt32(dr1["LedgerID"]) != 204)
                    //        if (Convert.ToDouble(dr1["Debit_Amount"]) > 0)
                    //        {
                               
                    //            netvalue = netvalue + Convert.ToDouble(dr1["Debit_Amount"].ToString());
                    //            //NetAmount = NetAmount + Convert.ToDouble(dr1["Debit_Amount"].ToString());
                    //            TotalNetAmt += Convert.ToDouble(dr1["Debit_Amount"]);
                    //        }
                    //        if (Convert.ToDouble(dr1["Credit_Amount"]) > 0)
                    //        {
                    //           // TotalAmount = (dr1["Credit_Amount"].ToString());
                    //            totalamt = totalamt + Convert.ToDouble(dr1["Credit_Amount"]);
                    //            GrandTotalAmt += Convert.ToDouble(dr1["Credit_Amount"]);
                    //        }
                           
                    //    }
                    //    netvalue = netvalue - VatAmt;
                    //    TotalNetAmt = TotalNetAmt - VatAmt;
                    //    WriteVatDetails(0, TransactDate, LedgerName, VoucherNo, netvalue.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), VatAmt.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), totalamt.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)),VType,RowID);

                    //}
                    #endregion
                
               
             }
            #endregion
                        
               
        }

        private void frmVATReport_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }
        private void DisplayBannar()
        {
            CompanyDetails m_CompanyDetails = CompanyInfo.GetInfo();
            lblCompanyName.Text = m_CompanyDetails.CompanyName;
            lblCompanyAddress.Text = m_CompanyDetails.Address1 + " " + m_CompanyDetails.City + " " + m_CompanyDetails.District;
            lblContact.Text = "Contact: " + m_CompanyDetails.Telephone;
            lblPanNo.Text = "PAN No.: " + m_CompanyDetails.PAN;
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

            CrystalDecisions.Shared.ExportOptions CrExportOptions = new CrystalDecisions.Shared.ExportOptions();
            DiskFileDestinationOptions CrDiskFileDestinationOptions = new DiskFileDestinationOptions();
            CrDiskFileDestinationOptions.DiskFileName = FileName;

            dsVatReport1.Clear();
            rptVATReport rpt = new rptVATReport();

            //Fill the logo on the report
            Misc.WriteLogo(dsVatReport1, "tblImage");
            rpt.SetDataSource(dsVatReport1);

            CrystalDecisions.Shared.ParameterValues pvCollection = new CrystalDecisions.Shared.ParameterValues();
            CrystalDecisions.Shared.ParameterDiscreteValue pdvCompany_Name = new CrystalDecisions.Shared.ParameterDiscreteValue();
            CrystalDecisions.Shared.ParameterDiscreteValue pdvCompany_Address = new CrystalDecisions.Shared.ParameterDiscreteValue();
            CrystalDecisions.Shared.ParameterDiscreteValue pdvCompany_Phone = new CrystalDecisions.Shared.ParameterDiscreteValue();
            CrystalDecisions.Shared.ParameterDiscreteValue pdvCompany_PAN = new CrystalDecisions.Shared.ParameterDiscreteValue();
            CrystalDecisions.Shared.ParameterDiscreteValue pdvCompany_Slogan = new CrystalDecisions.Shared.ParameterDiscreteValue();
            CrystalDecisions.Shared.ParameterDiscreteValue pdvReport_Date = new CrystalDecisions.Shared.ParameterDiscreteValue();
            CrystalDecisions.Shared.ParameterDiscreteValue pdvPreparedBy = new CrystalDecisions.Shared.ParameterDiscreteValue();
            CrystalDecisions.Shared.ParameterDiscreteValue pdvCheckedBy = new CrystalDecisions.Shared.ParameterDiscreteValue();
            CrystalDecisions.Shared.ParameterDiscreteValue pdvApprovedBy = new CrystalDecisions.Shared.ParameterDiscreteValue();
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
            DateTime rptdate;
            rptdate = DateTime.Now;
            pdvReport_Date.Value = Date.ToSystem(rptdate);
            pvCollection.Clear();
            pvCollection.Add(pdvReport_Date);
            rpt.DataDefinition.ParameterFields["Print_Date"].ApplyCurrentValues(pvCollection);

            pdvPreparedBy.Value = Settings.GetSettings("PREPARED_BY");
            pvCollection.Clear();
            pvCollection.Add(pdvPreparedBy);
            rpt.DataDefinition.ParameterFields["PreparedBy"].ApplyCurrentValues(pvCollection);

            pdvCheckedBy.Value = Settings.GetSettings("CHECKED_BY");
            pvCollection.Clear();
            pvCollection.Add(pdvCheckedBy);
            rpt.DataDefinition.ParameterFields["CheckedBy"].ApplyCurrentValues(pvCollection);

            pdvApprovedBy.Value = Settings.GetSettings("APPROVED_BY");
            pvCollection.Clear();
            pvCollection.Add(pdvApprovedBy);
            rpt.DataDefinition.ParameterFields["ApprovedBy"].ApplyCurrentValues(pvCollection);

            //Update the progressbar
            ProgressForm.UpdateProgress(80, "Calculating Parameters...");
            dsVatReport1.Tables["tblVATType"].Rows.Add(1, "VAT COLLECTED");
            dsVatReport1.Tables["tblVATType"].Rows.Add(2, "VAT PAID");
            FillReportData();

            frmReportViewer frm = new frmReportViewer();
            frm.SetReportSource(rpt);

            //Update the progressbar
            ProgressForm.UpdateProgress(100, "Showing Report...");

            ProgressForm.CloseForm();
            switch (myPrintType)
            {
                case PrintType.DirectPrint: //Direct Printer
                    rpt.PrintOptions.PrinterName = "";
                    rpt.PrintToPrinter(1, false, 0, 0);
                    return;
                case PrintType.Excel: //Excel
                    ExcelFormatOptions CrFormatTypeOptions = new ExcelFormatOptions();
                    CrExportOptions = rpt.ExportOptions;
                    CrExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                    CrExportOptions.ExportFormatType = ExportFormatType.Excel;
                    CrExportOptions.DestinationOptions = CrDiskFileDestinationOptions;
                    CrExportOptions.FormatOptions = CrFormatTypeOptions;
                    rpt.Export();
                    rpt.Close();
                    return;
                case PrintType.PDF: //PDF
                    PdfFormatOptions CrFormatTypeOptionsPdf = new PdfFormatOptions();
                    CrExportOptions = rpt.ExportOptions;
                    CrExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                    CrExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                    CrExportOptions.DestinationOptions = CrDiskFileDestinationOptions;
                    CrExportOptions.FormatOptions = CrFormatTypeOptionsPdf;
                    rpt.Export();
                    rpt.Close();
                    return;
                case PrintType.Email:
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
                default: //Crystal Report
                    frm.Show();
                    frm.WindowState = FormWindowState.Maximized;
                    break;
            }






            //frm.Show();
            //frm.WindowState = FormWindowState.Maximized;
            
        }

        private void FillReportData()
        {
            for (int i = 0; i < grdvatpaid.Rows.Count - 1; i++)
            {
                string date, PartyAccount, VoucherNo, NetAmt, VAT, TotalAmt,VoucherTypePrint;
                //dtvatrpt.Rows.Add(grdVATReport[i+1,1].Value.ToString());
                try
                {
                    date = grdvatpaid[i + 1, 1].Value.ToString();
                }
                catch
                {
                     date = " ";
                }
                try
                {
                    PartyAccount = grdvatpaid[i + 1, 2].Value.ToString();
                }
                catch
                {
                     PartyAccount = " ";
                }
                try
                {
                    VoucherNo = grdvatpaid[i + 1, 3].Value.ToString();
                }
                catch
                {
                     VoucherNo = " ";
                }
                try
                {
                    NetAmt = grdvatpaid[i + 1, 4].Value.ToString();
                }
                catch
                {
                     NetAmt = " ";
                }
                try
                {
                    VAT = grdvatpaid[i + 1, 5].Value.ToString();
                }
                catch
                {
                     VAT = " ";
                }
                try
                {
                    TotalAmt = grdvatpaid[i + 1, 6].Value.ToString();
                }
                catch
                {
                     TotalAmt = " ";
                }
                try
                {
                    VoucherTypePrint = grdvatpaid[i + 1, 7].Value.ToString();
                }
                catch
                {
                    VoucherTypePrint = " ";
                }
                dsVatReport1.Tables["tblVATDetails"].Rows.Add(date,PartyAccount,VoucherNo,NetAmt,VAT,TotalAmt,VoucherTypePrint,"2");
                //grdVATReport[i + 1, 1].Value.ToString(), grdVATReport[i + 1, 2].Value.ToString(),grdVATReport[i + 1, 3].Value.ToString(),grdVATReport[i + 1, 4].Value.ToString(),grdVATReport[i + 1, 5].Value.ToString(),grdVATReport[i + 1, 6].Value.ToString()
            }

            for (int i = 0; i < grdVATReport.Rows.Count - 1; i++)
            {
                string date, PartyAccount, VoucherNo, NetAmt, VAT, TotalAmt, VoucherTypePrint;
                //dtvatrpt.Rows.Add(grdVATReport[i+1,1].Value.ToString());
                try
                {
                    date = grdVATReport[i + 1, 1].Value.ToString();
                }
                catch
                {
                    date = " ";
                }
                try
                {
                    PartyAccount = grdVATReport[i + 1, 2].Value.ToString();
                }
                catch
                {
                    PartyAccount = " ";
                }
                try
                {
                    VoucherNo = grdVATReport[i + 1, 3].Value.ToString();
                }
                catch
                {
                    VoucherNo = " ";
                }
                try
                {
                    NetAmt = grdVATReport[i + 1, 4].Value.ToString();
                }
                catch
                {
                    NetAmt = " ";
                }
                try
                {
                    VAT = grdVATReport[i + 1, 5].Value.ToString();
                }
                catch
                {
                    VAT = " ";
                }
                try
                {
                    TotalAmt = grdVATReport[i + 1, 6].Value.ToString();
                }
                catch
                {
                    TotalAmt = " ";
                }
                try
                {
                    VoucherTypePrint = grdVATReport[i + 1, 7].Value.ToString();
                }
                catch
                {
                    VoucherTypePrint = " ";
                }
                dsVatReport1.Tables["tblVATDetails"].Rows.Add(date, PartyAccount, VoucherNo, NetAmt, VAT, TotalAmt, VoucherTypePrint, "1");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void grdVATReport_DoubleClick(object sender, EventArgs e)
        {
            int CurRow = grdVATReport.Selection.GetSelectionRegion().GetRowsIndex()[0];
            int RowID = Convert.ToInt32(grdVATReport[CurRow, 8].Value.ToString());
            string VoucherType=grdVATReport[CurRow, 7].Value.ToString();
        
            switch (VoucherType)
            {
                case "JRNL":
                    frmJournal frm = new frmJournal(RowID);
                    frm.Show();
                    break;
                case "DR_NOT":
                    frmDebitNote frm1 = new frmDebitNote(RowID);
                    frm1.Show();
                    break;
                case "CR_NOT":
                    frmCreditNote frm2 = new frmCreditNote(RowID);
                    frm2.Show();
                    break;
                case "CASH_PMNT":
                    frmCashPayment frm3 = new frmCashPayment(RowID);
                    frm3.Show();
                    break;
                case "CASH_RCPT":
                    frmCashReceipt frm4 = new frmCashReceipt(RowID);
                    frm4.Show();

                    break;
                case "BANK_PMNT":
                    frmBankPayment frm5 = new frmBankPayment(RowID);
                    frm5.Show();
                    break;
                case "BANK_RCPT":
                    frmBankReceipt frm6 = new frmBankReceipt(RowID);
                    frm6.Show();
                    break;
                case "CNTR":
                    frmContra frm7 = new frmContra(RowID);
                    frm7.Show();
                    break;
                case "SALES":
                    object[] param = new object[1];
                    param[0] = (RowID);
                    m_MDIForm.OpenFormArrayParam("frmSalesInvoice", param);
                    break;
            }
        }

        private void grdVATReportPaid_DoubleClick(object sender, EventArgs e)
        {
            int CurRow = grdVATReport.Selection.GetSelectionRegion().GetRowsIndex()[0];
            int RowID = Convert.ToInt32(grdVATReport[CurRow, 8].Value.ToString());
            string VoucherType = grdVATReport[CurRow, 7].Value.ToString();

            switch (VoucherType)
            {
                case "JRNL":
                    frmJournal frm = new frmJournal(RowID);
                    frm.Show();
                    break;
                case "DR_NOT":
                    frmDebitNote frm1 = new frmDebitNote(RowID);
                    frm1.Show();
                    break;
                case "CR_NOT":
                    frmCreditNote frm2 = new frmCreditNote(RowID);
                    frm2.Show();
                    break;
                case "CASH_PMNT":
                    frmCashPayment frm3 = new frmCashPayment(RowID);
                    frm3.Show();
                    break;
                case "CASH_RCPT":
                    frmCashReceipt frm4 = new frmCashReceipt(RowID);
                    frm4.Show();

                    break;
                case "BANK_PMNT":
                    frmBankPayment frm5 = new frmBankPayment(RowID);
                    frm5.Show();
                    break;
                case "BANK_RCPT":
                    frmBankReceipt frm6 = new frmBankReceipt(RowID);
                    frm6.Show();
                    break;
                case "CNTR":
                    frmContra frm7 = new frmContra(RowID);
                    frm7.Show();
                    break;
                case "SALES":
                    m_MDIForm.OpenFormArrayParam("frmSalesInvoice");
                    break;
            }
        }

        private void WriteVatDetailsPaid(int SNo, string TransactDate, string LedgerName, string VoucherNo, string NetAmount, string VatAmount, string TotalAmount, string VoucherType, int rowid)
        {
            SourceGrid.Cells.Views.Cell categoryView = new SourceGrid.Cells.Views.Cell();
            //categoryView.Background = new DevAge.Drawing.VisualElements.BackgroundLinearGradient(Color.RoyalBlue, Color.LightBlue, 0);
            categoryView.ForeColor = Color.FromKnownColor(KnownColor.ActiveCaptionText);
            categoryView.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleCenter;
            categoryView.Border = DevAge.Drawing.RectangleBorder.NoBorder;
            categoryView.Font = new Font(Font, FontStyle.Bold);

            grdvatpaid.Rows.Insert(grdvatpaid.RowsCount);
            SourceGrid.Cells.Views.Cell alternate = new SourceGrid.Cells.Views.Cell();
            if (VatDetailsRowsCountPaid % 2 == 0)
            {
                //alternate.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Color.LightCoral);
            }
            else
            {
                alternate.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Global.Grid_Color);
            }
            //string strSNo = (SNo == 0 ? "***" : SNo.ToString());
            //strSNo = strSNo + 1; ;
            if (LedgerName != "TOTAL")
            {
                grdvatpaid[VatDetailsRowsCountPaid, 0] = new SourceGrid.Cells.Cell(strSNoPaid);
                grdvatpaid[VatDetailsRowsCountPaid, 0].View = new SourceGrid.Cells.Views.Cell(alternate);

                //grdvatpaid[VatDetailsRowsCountPaid, 0].AddController(dblClick);

                grdvatpaid[VatDetailsRowsCountPaid, 1] = new SourceGrid.Cells.Cell(TransactDate);
                grdvatpaid[VatDetailsRowsCountPaid, 1].View = new SourceGrid.Cells.Views.Cell(alternate);
                //grdvatpaid[VatDetailsRowsCountPaid, 1].AddController(dblClick);

                grdvatpaid[VatDetailsRowsCountPaid, 2] = new SourceGrid.Cells.Cell(LedgerName);
                grdvatpaid[VatDetailsRowsCountPaid, 2].View = new SourceGrid.Cells.Views.Cell(alternate);
                //grdvatpaid[VatDetailsRowsCountPaid, 2].AddController(dblClick);

                grdvatpaid[VatDetailsRowsCountPaid, 3] = new SourceGrid.Cells.Cell(VoucherNo);
                grdvatpaid[VatDetailsRowsCountPaid, 3].View = new SourceGrid.Cells.Views.Cell(alternate);
                //grdvatpaid[VatDetailsRowsCountPaid, 3].AddController(dblClick);

                grdvatpaid[VatDetailsRowsCountPaid, 4] = new SourceGrid.Cells.Cell(NetAmount);
                grdvatpaid[VatDetailsRowsCountPaid, 4].View = new SourceGrid.Cells.Views.Cell(alternate);
                //grdvatpaid[VatDetailsRowsCountPaid, 4].AddController(dblClick);

                grdvatpaid[VatDetailsRowsCountPaid, 5] = new SourceGrid.Cells.Cell(VatAmount);
                grdvatpaid[VatDetailsRowsCountPaid, 5].View = new SourceGrid.Cells.Views.Cell(alternate);
                //grdvatpaid[VatDetailsRowsCountPaid, 5].AddController(dblClick);

                grdvatpaid[VatDetailsRowsCountPaid, 6] = new SourceGrid.Cells.Cell(TotalAmount);
                grdvatpaid[VatDetailsRowsCountPaid, 6].View = new SourceGrid.Cells.Views.Cell(alternate);
                //grdvatpaid[VatDetailsRowsCountPaid, 6].AddController(dblClick);

                grdvatpaid[VatDetailsRowsCountPaid, 7] = new SourceGrid.Cells.Cell(VoucherType);
                grdvatpaid[VatDetailsRowsCountPaid, 7].View = new SourceGrid.Cells.Views.Cell(alternate);
                //grdvatpaid[VatDetailsRowsCountPaid, 7].AddController(dblClick);

                grdvatpaid[VatDetailsRowsCountPaid, 8] = new SourceGrid.Cells.Cell(rowid);
                //grdvatpaid[VatDetailsRowsCountPaid, 8].AddController(dblClick);
            }
            else if (LedgerName == "TOTAL")
            {
                grdvatpaid[VatDetailsRowsCountPaid, 0] = new SourceGrid.Cells.Cell("");
                grdvatpaid[VatDetailsRowsCountPaid, 0].View = new SourceGrid.Cells.Views.Cell(categoryView);

                //grdvatpaid[VatDetailsRowsCountPaid, 0].AddController(dblClick);

                grdvatpaid[VatDetailsRowsCountPaid, 1] = new SourceGrid.Cells.Cell(TransactDate);
                grdvatpaid[VatDetailsRowsCountPaid, 1].View = new SourceGrid.Cells.Views.Cell(categoryView);
                //grdvatpaid[VatDetailsRowsCountPaid, 1].AddController(dblClick);

                grdvatpaid[VatDetailsRowsCountPaid, 2] = new SourceGrid.Cells.Cell(LedgerName);
                grdvatpaid[VatDetailsRowsCountPaid, 2].View = new SourceGrid.Cells.Views.Cell(categoryView);
                //grdvatpaid[VatDetailsRowsCountPaid, 2].AddController(dblClick);

                grdvatpaid[VatDetailsRowsCountPaid, 3] = new SourceGrid.Cells.Cell(VoucherNo);
                grdvatpaid[VatDetailsRowsCountPaid, 3].View = new SourceGrid.Cells.Views.Cell(categoryView);
                //grdvatpaid[VatDetailsRowsCountPaid, 3].AddController(dblClick);

                grdvatpaid[VatDetailsRowsCountPaid, 4] = new SourceGrid.Cells.Cell(NetAmount);
                grdvatpaid[VatDetailsRowsCountPaid, 4].View = new SourceGrid.Cells.Views.Cell(categoryView);
                //grdvatpaid[VatDetailsRowsCountPaid, 4].AddController(dblClick);

                grdvatpaid[VatDetailsRowsCountPaid, 5] = new SourceGrid.Cells.Cell(VatAmount);
                grdvatpaid[VatDetailsRowsCountPaid, 5].View = new SourceGrid.Cells.Views.Cell(categoryView);
                //grdvatpaid[VatDetailsRowsCountPaid, 5].AddController(dblClick);

                grdvatpaid[VatDetailsRowsCountPaid, 6] = new SourceGrid.Cells.Cell(TotalAmount);
                grdvatpaid[VatDetailsRowsCountPaid, 6].View = new SourceGrid.Cells.Views.Cell(categoryView);
                //grdvatpaid[VatDetailsRowsCountPaid, 6].AddController(dblClick);

                grdvatpaid[VatDetailsRowsCountPaid, 7] = new SourceGrid.Cells.Cell(VoucherType);
                grdvatpaid[VatDetailsRowsCountPaid, 7].View = new SourceGrid.Cells.Views.Cell(categoryView);
                //grdvatpaid[VatDetailsRowsCountPaid, 7].AddController(dblClick);

                grdvatpaid[VatDetailsRowsCountPaid, 8] = new SourceGrid.Cells.Cell(rowid);
                //grdvatpaid[VatDetailsRowsCountPaid, 8].AddController(dblClick);
            }

            VatDetailsRowsCountPaid++;
            strSNoPaid++;
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
                    //prntDirect = 4;
                    PrintPreviewCR(PrintType.Email);
                    break;
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            grdvatpaid.Redim(0, 0);

            this.Cursor = Cursors.WaitCursor;
            //Load all over again
            frmVATReport_Load(sender, e);

            this.Cursor = Cursors.Default;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            PrintPreviewCR(PrintType.DirectPrint);
        }

        private void grdVATReport_Paint(object sender, PaintEventArgs e)
        {

        }

        private void dgPurchaseReport_Paint(object sender, PaintEventArgs e)
        {

        }

        
    }
}
