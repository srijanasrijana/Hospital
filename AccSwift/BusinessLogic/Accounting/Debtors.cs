using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;
using System.Windows.Forms;
using DateManager;

namespace BusinessLogic.Accounting
{
    public class Debtors
    {
        public static DataTable GetDebtorsSalesPeriod()
        {
            string str = "select SM.CashPartyLedgerID LedgerID,Voucher_No,DueDays,SM.SalesInvoice_Date,(Total_Amount+VAT+Tax1+Tax2+Tax3)DebtorsCost from Inv.tblSalesInvoiceMaster  SM where DueDays<>''";
            return Global.m_db.SelectQry(str, "tblDebtorsSalesInfo");
        }
        public static DataTable GetCashReceiptDetails(string VoucherType,string VoucherNumber,int LedgerID)
        {
            string str = "select SUM(Amount) Amount,VoucherNumber,CRM.LedgerID,CRM.Voucher_No,CRM.CashReceipt_Date LedgerDate,CRM.CashReceiptID RowID from Acc.tblCashReceiptDetails CRD,Acc.tblCashReceiptMaster CRM where VoucherNumber='" + VoucherNumber + "' and CRD.LedgerID='" + LedgerID + "' and VoucherType='" + VoucherType + "' AND CRM.CashReceiptID=CRD.CashReceiptID group by VoucherNumber,CRM.LedgerID,CRM.Voucher_No,CRM.CashReceipt_Date,CRM.CashReceiptID";
            return Global.m_db.SelectQry(str, "tblDebtorsCashReceiptInfo");
        }
        public static DataTable GetBankReceiptDetails(string VoucherType, string VoucherNumber, int LedgerID)
        {
            string str = "select SUM(Amount) Amount,VoucherNumber,BRM.LedgerID,BRM.Voucher_No,BRM.BankReceipt_Date LedgerDate,BRM.BankReceiptID RowID from Acc.tblBankReceiptDetails BRD,Acc.tblBankReceiptMaster BRM  where VoucherNumber='" + VoucherNumber + "' and BRD.LedgerID='" + LedgerID + "' and VoucherType='" + VoucherType + "' and BRM.BankReceiptID=BRD.BankReceiptID group by VoucherNumber,BRM.LedgerID,BRM.Voucher_No,BRM.BankReceipt_Date,BRM.BankReceiptID";
            return Global.m_db.SelectQry(str, "tblDebtorsCashReceiptInfo");
        }
        public static DataTable GetCashReceiptDetails1(string VoucherType, string VoucherNumber, int LedgerID)
        {
            string str = "select SUM(Amount) Amount,VoucherNumber from Acc.tblCashReceiptDetails where VoucherNumber='" + VoucherNumber + "' and LedgerID='" + LedgerID + "' and VoucherType='" + VoucherType + "'  group by VoucherNumber";
            return Global.m_db.SelectQry(str, "tblDebtorsCashReceiptInfo");
        }
        public static DataTable GetBankReceiptDetails1(string VoucherType, string VoucherNumber, int LedgerID)
        {
            string str = "select SUM(Amount) Amount,VoucherNumber from Acc.tblBankReceiptDetails   where VoucherNumber='" + VoucherNumber + "' and LedgerID='" + LedgerID + "' and VoucherType='" + VoucherType + "'  group by VoucherNumber";
            return Global.m_db.SelectQry(str, "tblDebtorsCashReceiptInfo");
        }

        public static DataTable GetDebtorsAgeing(DateTime? StartDate,DateTime? AsonDate,int LedgerID,string AccClassIDs,string ProjectIDs,int GroupID)
        {
            try
            {
                DataTable dt = new DataTable();
                //DataSet ds=new DataSet();
                Global.m_db.ClearParameter();
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("[Acc].[spGetDebtorAging]");
                if (StartDate != null)
                    Global.m_db.AddParameter("@Transaction_Start_Date", SqlDbType.DateTime, Date.ToDB(Convert.ToDateTime(StartDate)));
                else
                    Global.m_db.AddParameter("@Transaction_Start_Date", SqlDbType.DateTime, DBNull.Value);
                if (AsonDate != null)
                    Global.m_db.AddParameter("@Transaction_End_Date", SqlDbType.DateTime, Date.ToDB(Convert.ToDateTime(AsonDate)));
                else
                    Global.m_db.AddParameter("@Transaction_End_Date", SqlDbType.DateTime, DBNull.Value);
                if (LedgerID != null)
                    Global.m_db.AddParameter("@LedgerID", SqlDbType.Int, LedgerID);
                else
                    Global.m_db.AddParameter("@LedgerID", SqlDbType.Int, DBNull.Value);
                Global.m_db.AddParameter("@AccountClassIDsSettings", SqlDbType.NVarChar, AccClassIDs);
                Global.m_db.AddParameter("@ProjectIDsSettings", SqlDbType.NVarChar, ProjectIDs);

                if (GroupID != null)
                    Global.m_db.AddParameter("@GroupID", SqlDbType.Int, GroupID);
                else
                    Global.m_db.AddParameter("@GroupID", SqlDbType.Int, DBNull.Value);



                dt = Global.m_db.GetDataTable();
                

                return dt;
            }
            catch (Exception ex)
            {              
                throw;
            }

        }
    }
}
