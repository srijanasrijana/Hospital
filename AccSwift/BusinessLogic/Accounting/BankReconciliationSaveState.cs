using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace BusinessLogic.Accounting
{
   public class BankReconciliationSaveState
    {
       public void BankReconciliationSavestate(DataTable dt, string fromdate, string todate,int ledgerid)
       {
           
           try
           {
               Global.m_db.BeginTransaction();
               //string strQuery = "delete  savestate WHERE ledgerid='" + ledgerid + "'";
               //Global.m_db.SelectQry(strQuery,"tbl");
               string strQuery = "delete  Acc.tblBankReconciliationSaveState WHERE ledgerid='" + ledgerid + "'";
               Global.m_db.InsertUpdateQry(strQuery);

               for (int i = 0; i < dt.Rows.Count;i++ )
               {    
                   DataRow dr = dt.Rows[i];
                   object myobj = dr["chequedate"].ToString();
                   Global.m_db.ClearParameter();
                   Global.m_db.setCommandType(CommandType.StoredProcedure);
                   Global.m_db.setCommandText("Acc.spBankReconciliationDetailStatus");
                   Global.m_db.AddParameter("@Sn", SqlDbType.Int, Convert.ToInt32(dr["Sn"]));
                   Global.m_db.AddParameter("@Date", SqlDbType.DateTime,Convert.ToDateTime(dr["Date"]));
                   Global.m_db.AddParameter("@partyname", SqlDbType.NVarChar,20, dr["PartyName"].ToString());
                   Global.m_db.AddParameter("@debitamount", SqlDbType.Float, dr["DebitAmount"]);
                   Global.m_db.AddParameter("@creditamount", SqlDbType.Float, dr["CreditAmount"]);
                   Global.m_db.AddParameter("@chequenumber", SqlDbType.NVarChar, dr["chequenumber"].ToString());
                   Global.m_db.AddParameter("@chequebank", SqlDbType.NVarChar, 20, dr["chequebank"].ToString());
                   Global.m_db.AddParameter("@fromdate", SqlDbType.DateTime, 20,Convert.ToDateTime(fromdate));
                   Global.m_db.AddParameter("@todate", SqlDbType.DateTime, 20,Convert.ToDateTime(todate));

                   if (myobj.ToString() == " ")
                   {
                       Global.m_db.AddParameter("@chequedate", SqlDbType.DateTime, null);
                   }
                   else
                   {
                       Global.m_db.AddParameter("@chequedate", SqlDbType.DateTime, Convert.ToDateTime(dr["chequedate"]));
                   }
                       if (dr["matched"].ToString() == "False")
                       {
                           Global.m_db.AddParameter("@matched", SqlDbType.Char, 1, "N");
                       }
                       else
                       {
                           Global.m_db.AddParameter("@matched", SqlDbType.Char, 1, "Y");
                       }
                   Global.m_db.AddParameter("@vouchertype", SqlDbType.NVarChar, 20, dr["vouchertype"].ToString());
                   Global.m_db.AddParameter("@rowid", SqlDbType.Int,  dr["rowid"]);
                   Global.m_db.AddParameter("@userid", SqlDbType.Int,  dr["userid"]);
                   Global.m_db.AddParameter("@ledgerid", SqlDbType.Int,  dr["ledgerid"]);
                   System.Data.SqlClient.SqlParameter paramReturn = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                   Global.m_db.ProcessParameter();

                   //if (paramReturn.Value.ToString() != "SUCCESS")
                   //{
                   //    throw new Exception("Unable to save Bank Reconciliation Information");
                   //}

                   //string query="insert into SaveState(sn,date,debit_amount,credit_amount,chequenumber,chequebank,chequedate,matched,vouchertype,rowid,userid,ledgerid) values('" + Convert.ToInt32(dr["sn"]) + "','" +  Convert.ToDateTime(dr["Date"]) + "','" + dr["PartyName"] + "','" + Convert.ToDouble(dr["DebitAmount"]) + "','" + Convert.ToDouble(dr["CreditAmount"]) + "','" + Convert.ToDouble(dr["ChequeNumber"]) + "','" + dr["ChequeBank"] + "','" +Convert.ToDateTime(dr["ChequeDate"]) + "''" + dr["Matched"] + "','" + dr["VoucherType"] + "','" + Convert.ToDouble(dr["rowid"]) + "','" + Convert.ToInt32(dr["userid"]) + "','"+Convert.ToInt32(dr["ledgerid"]);
                   //Global.m_db.InsertUpdateQry("insert into SaveState(sn,date,partyname,debit_amount,credit_amount,chequenumber,chequebank,chequedate,matched,vouchertype,rowid,userid,ledgerid) values('" + Convert.ToInt32(dr["sn"]) + "','" +  Convert.ToDateTime(dr["Date"]) + "','" + dr["PartyName"] + "','" + Convert.ToDouble(dr["DebitAmount"]) + "','" + Convert.ToDouble(dr["CreditAmount"]) + "','" + Convert.ToDouble(dr["ChequeNumber"]) + "','" + dr["ChequeBank"] + "','" +Convert.ToDateTime(dr["ChequeDate"]) + "''" + dr["Matched"] + "','" + dr["VoucherType"] + "','" + Convert.ToDouble(dr["rowid"]) + "','" + Convert.ToInt32(dr["userid"]) + "','"+Convert.ToInt32(dr["ledgerid"])+"')");
               }
               Global.m_db.CommitTransaction();
           }
           catch (Exception ex)
           {
               Global.m_db.RollBackTransaction();
               MessageBox.Show(ex.Message);  
           }
       }

       public DataTable CheckLedgerState(int ledgerId)
       {
          // SELECT * FROM savestate WHERE LedgerID=363 and fromdate='2009/01/24' and todate='2013/08/13'
           string strQuery = "SELECT * FROM Acc.tblBankReconciliationSaveState WHERE LedgerID='" + ledgerId + "'";
           DataTable dt = Global.m_db.SelectQry(strQuery, "tbl");
           return dt;

       }
       public static DataTable RetrieveState(int bankID, string AccClassIDsXMLString)
       {
           try
           {
               DataTable dtFinal = new DataTable();
               //DataTable dt = Global.m_db.SelectQry("select * from Acc.tblBankReconSaveStateMaster where BankID ="+bankID+"", "tbl");
               //DataTable dtTemp = Transaction.GetLedgerTransactionWithChequeDetails(bankID, "<?xml version=\"1.0\" encoding=\"utf-16\"?><LEDGERTRANSACT><ACCCLASSIDS><AccID>1</AccID></ACCCLASSIDS></LEDGERTRANSACT>", Convert.ToDateTime(dt.Rows[0]["FromDate"].ToString()), Convert.ToDateTime(dt.Rows[0]["ToDate"]), Convert.ToInt32(dt.Rows[0]["PartyID"].ToString()), Convert.ToInt32(dt.Rows[0]["PmtRcpTypeID"].ToString()));
               //dt = Global.m_db.SelectQry("select * from Acc.tblBankReconSaveStateDetails", "tbl");

               ////dtFinal = (from t1 in dtTemp.Rows.Cast<DataRow>()
               ////              join t2 in dt.Rows.Cast<DataRow>() on t1["RowID"] equals t2["RowID"] 
               ////              select t1).CopyToDataTable();

               //var query = (from a in dt.AsEnumerable()
               //             join b in dtTemp.AsEnumerable() on a.Field<int>("RowID") equals b.Field<int>("RowID")
               //             join c in dtTemp.AsEnumerable() on a.Field<int>("VoucherType") equals c.Field<int>("VoucherType")                          
               //             select a).ToList();
               ////If You are selecting a single table then you can convert to table on the following way
               //if (query.Count > 0)
               //{
               //    dtFinal = query.CopyToDataTable();
               //}
               Global.m_db.ClearParameter();
               Global.m_db.setCommandText("Acc.spRetrieveBankReconciliation");
               Global.m_db.setCommandType(CommandType.StoredProcedure);

               Global.m_db.AddParameter("@BankID", SqlDbType.Int, bankID);
               AccClassIDsXMLString = "<?xml version=\"1.0\" encoding=\"utf-16\"?><LEDGERTRANSACT><ACCCLASSIDS><AccID>1</AccID></ACCCLASSIDS></LEDGERTRANSACT>";
               Global.m_db.AddParameter("@AccountClassIDsSettings", SqlDbType.Xml, AccClassIDsXMLString);

               dtFinal = Global.m_db.GetDataTable();
               return dtFinal;
           }
           catch (Exception ex)
           {
              // Global.m_db.RollBackTransaction();
               throw ex;
           }
       }

       public static DataTable RetrieveBankReconMaster(int bankID)
       {
           try
           {
               return Global.m_db.SelectQry("select * from Acc.tblBankReconSaveStateMaster where BankID ="+bankID+"", "tbl");
           }
           catch (Exception ex)
           {
               // Global.m_db.RollBackTransaction();
               throw ex;
           }
       }
       public static void InsertBankReconciliationSaveState(DataTable dt, string fromDate, string toDate, int bankID, int partyID, int pmtRcpTypeID, decimal balance)
       {

           try
           {
               Global.m_db.BeginTransaction();

               string strQuery = "delete from  Acc.tblBankReconSaveStateDetails WHERE SaveStateID in ( select  SaveStateID from Acc.tblBankReconSaveStateMaster where BankID = " + bankID + ")";
               Global.m_db.InsertUpdateQry(strQuery);

               strQuery = "delete from Acc.tblBankReconSaveStateMaster WHERE BankID =" + bankID + "";
               Global.m_db.InsertUpdateQry(strQuery);

               Global.m_db.ClearParameter();
               Global.m_db.AddParameter("@BankID", SqlDbType.Int, bankID);
               Global.m_db.AddParameter("@PartyID", SqlDbType.NVarChar, 20, partyID);
               Global.m_db.AddParameter("@PmtRcpID", SqlDbType.Float, pmtRcpTypeID);
               Global.m_db.AddParameter("@Balance", SqlDbType.Float,balance);
               Global.m_db.AddParameter("@FromDate", SqlDbType.DateTime, 20, fromDate);
               Global.m_db.AddParameter("@ToDate", SqlDbType.DateTime, 20, toDate);

               strQuery = "insert into Acc.tblBankReconSaveStateMaster(BankID, PartyID, PmtRcpTypeID, Balance, FromDate, ToDate) values(@BankID, @PartyID, @PmtRcpID, @Balance, @FromDate, @ToDate)";
               Global.m_db.InsertUpdateQry(strQuery);
               int SaveStateID = Convert.ToInt32(Global.m_db.GetScalarValue("select max(SaveStateID) from Acc.tblBankReconSaveStateMaster"));
               for (int i = 0; i < dt.Rows.Count; i++)
               {
                   Global.m_db.ClearParameter();
                   DataRow dr = dt.Rows[i];
                   Global.m_db.AddParameter("@SaveStateID", SqlDbType.Int, SaveStateID);
                   Global.m_db.AddParameter("@VoucherType", SqlDbType.NVarChar, 20, dr["VoucherType"]);
                   Global.m_db.AddParameter("@RowID", SqlDbType.Int, dr["RowID"]);
                   Global.m_db.AddParameter("@Matched", SqlDbType.Bit, 20, dr["Matched"]);

                   strQuery = "insert into Acc.tblBankReconSaveStateDetails(SaveStateID, VoucherType, RowID, Matched) values(@SaveStateID, @VoucherType, @RowID, @Matched)";
                   Global.m_db.InsertUpdateQry(strQuery);
               }
               Global.m_db.CommitTransaction();
           }
           catch (Exception ex)
           {
               Global.m_db.RollBackTransaction();
               MessageBox.Show(ex.Message);
           }
       }
    }
}
