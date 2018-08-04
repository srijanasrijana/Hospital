using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;
using System.Windows.Forms;
using DBLogic;
using DateManager;

namespace BusinessLogic.Accounting
{
    public class ChequeReceipt
    {
        public static DataTable GetChequeCashDetails()
        {
            try
            {

                DataTable dt = new DataTable();
                string strQuery = "";
                strQuery = "select CM.*,CD.ledgerID,cd.amount,CD.chequeNumber,CD.chequeBank,CD.chequeDate,Cd.chequeCashDate" + " " +
                               " from Acc.tblChequeReceiptMaster CM, Acc.tblChequeReceiptDetail CD where IsChequeCash=0 and CM.chequeReceiptID=CD.chequeReceiptID";
                return Global.m_db.SelectQry(strQuery, "tblchequecash");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            

        }
        public  DataTable GetChequeCashAccClass(int ChequeReceiptID)
        {
            DataTable dt = new DataTable();
            string strQuery = "";
            strQuery = " Select AccClassID from Acc.tblChequeReceiptAccClass where ChequeReceiptID='"+ChequeReceiptID+"'";
            return Global.m_db.SelectQry(strQuery, "tblAccClassIDs");
        }
         public static void UpdateChequeReceiptInformation(int ChequeReceiptID,int ledgerid)
        {
            string SQL = "update Acc.tblChequeReceiptDetail set IsChequeCash=1 where ChequeReceiptID='"+ChequeReceiptID+"' and LedgerID='"+ledgerid+"' ";
            Global.m_db.InsertUpdateQry(SQL);
        }
         public bool RemoveChequeReceiptEntry(int ChequeReceiptID)
         {
             try
             {
                 Global.m_db.ClearParameter();
                 Global.m_db.setCommandType(CommandType.StoredProcedure);
                 Global.m_db.setCommandText("Acc.spRemoveChequeReceiptEntry");
                 Global.m_db.AddParameter("@chequeReceiptID", SqlDbType.Int, ChequeReceiptID);
                 //system.Data.SqlClient.SqlParameter objReturn = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                 Global.m_db.ProcessParameter();
                 return true;
             }
             catch
             {
                 return false;
             }
         }

         public DataTable NavigateChequeReceiptMaster(int CurrentID, Navigate NavTo)
         {
             Global.m_db.ClearParameter();
             Global.m_db.setCommandType(CommandType.StoredProcedure);
             Global.m_db.setCommandText("Acc.spGetChequeReceiptNavigate");
             Global.m_db.AddParameter("@CurrentID", SqlDbType.Int, CurrentID);
             string strNavigate = "FIRST";
             switch (NavTo)
             {
                 case Navigate.First:
                     strNavigate = "FIRST";
                     break;
                 case Navigate.Last:
                     strNavigate = "LAST";
                     break;
                 case Navigate.Next:
                     strNavigate = "NEXT";
                     break;
                 case Navigate.Prev:
                     strNavigate = "PREV";
                     break;
                 case Navigate.ID:
                     strNavigate = "ID";
                     break;
             }
             Global.m_db.AddParameter("@NavigateTo", SqlDbType.NVarChar, 20, strNavigate);
             DataTable dtChequeReceiptMst = Global.m_db.GetDataTable();
             return dtChequeReceiptMst;
         }

         public DataTable GetChequeReceiptDetail(int MasterID)
         {
             Global.m_db.ClearParameter();
             Global.m_db.setCommandType(CommandType.StoredProcedure);
             Global.m_db.setCommandText("Acc.spGetChequeDetail");
             Global.m_db.AddParameter("@MasterID", SqlDbType.Int, MasterID);

             string Language = "ENGLISH";
             switch (LangMgr.DefaultLanguage)
             {
                 case Lang.English:
                     Language = "ENGLISH";
                     break;
                 case Lang.Nepali:
                     Language = "NEPALI";
                     break;
             }

             Global.m_db.AddParameter("@Language", SqlDbType.NVarChar, 20, Language);
             DataTable dtChequeReceiptDtl = Global.m_db.GetDataTable();
             return dtChequeReceiptDtl;
         }

         public int GetSeriesIDFromMasterID(int MasterID)
         {
             object returnID;
             returnID = Global.m_db.GetScalarValue("SELECT SeriesID FROM Acc.tblChequeReceiptMaster WHERE chequeReceiptID ='" + MasterID + "'");
             return Convert.ToInt32(returnID);
         }
         public DataTable GetChequeReceiptMaster(int ChequeReceiptID)
         {
             return Global.m_db.SelectQry("SELECT * FROM Acc.tblChequeReceiptMaster WHERE chequeReceiptID ='" + ChequeReceiptID + "'", "tblChequeReceiptMaster");
         }
         public DataTable GetLedgerID()
         {
             return Global.m_db.SelectQry("select * from Acc.tblChequeReceiptDetail where IsChequeCash=0", "tblChequeReceiptDetail");
         }
         public DataTable GetVouch(int ChequeReceiptID)
         {
             return Global.m_db.SelectQry("select * from Acc.tblChequeReceiptMaster where ChequeReceiptID=" + ChequeReceiptID + "", "tblChequeReceiptDetail");
         }

         public DataTable GetLedgerTransactWithAccountClassAndProject(int LedgerID, DateTime? FromDate, DateTime? ToDate, ArrayList AccClassID)
         {
             string TransactDateSQL = "";
             if (FromDate != null && ToDate != null)
                 TransactDateSQL = " AND ChequeDate BETWEEN '" + Date.ToDB(Convert.ToDateTime(FromDate)) + "' AND '" + Date.ToDB(Convert.ToDateTime(ToDate)) + "'";
             else if (FromDate != null)
                 TransactDateSQL = " AND ChequeDate>='" + Date.ToDB(Convert.ToDateTime(FromDate)) + "'";
             else if (ToDate != null)
                 TransactDateSQL = " AND ChequeDate<='" + Date.ToDB(Convert.ToDateTime(ToDate)) + "'";

             //For Accounting Class Information
             string AccClassID1 = "";
             string AccClassIDSQL = "";
             int i = 0;
             if (AccClassID != null)
             {
                 foreach (object j in AccClassID)
                 {
                     if (i == 0)// for first GroupID
                         AccClassID1 = "'" + j.ToString() + "'";
                     else  //Separating Other GroupID by commas
                         AccClassID1 += "," + "'" + j.ToString() + "'";
                     i++;
                 }
             }

             if (AccClassID1 == "")
                 AccClassID1 = "1";
             AccClassIDSQL = "AND AccClassID IN (" + (AccClassID1) + ")";

          

             string strFinalQuery = "";
             strFinalQuery = "SELECT distinct T.* FROM Acc.tblChequeReceiptDetail T,Acc.tblChequeReceiptAccClass TC WHERE IsChequeCash=0 and LedgerID='" + LedgerID + "' and T.chequeReceiptID=TC.ChequeReceiptID" + " " + TransactDateSQL + AccClassIDSQL;

             return Global.m_db.SelectQry(strFinalQuery, "ledgerTransact");
         }

    }
}
