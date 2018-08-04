using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;

namespace BusinessLogic
{
    public class CashFlow
    {
        public static DataTable GetAllLedgerExceptCashBankByGroupID(int m_GroupID, string strCashBankLedgers)
        {
            //To show the cash flow according to groupwise...we have to find out the all ledgers associated with this group
            //Group may contain sub group too soo we have to consider this part too
            //Making the method which take GroupID as input and collect sub groupID of corresponding GroupID and ultimately find out the All Ledgers excepet CashBank associated with this group using proper query
             DataTable dtAllLedgerExceptCashBank = new DataTable();
            ArrayList tmpGroupID = new ArrayList();//Dyanamically allocating array type of variable tmpGroupID   
            tmpGroupID.Add(m_GroupID);// it add itself too           
            AccountGroup.GetAccountsUnder(m_GroupID, tmpGroupID);//Calling this function for collecting subGroupsID which fall under GroupID and storing on arrylist         
            string GroupID = "";
            int K = 0;
            foreach (object j in tmpGroupID)
            {
                if (K == 0)// for first GroupID
                    GroupID = "'" + j.ToString() + "'";
                else  //Separating Other GroupID by commas
                    GroupID += "," + "'" + j.ToString() + "'";
                K++;
            }
            DataTable dt = Global.m_db.SelectQry("SELECT * FROM Acc.tblLedger WHERE GroupID IN (" + (GroupID) + ")", "GroupID");
            string LedgerID = "";
            for (int i1 = 0; i1 < dt.Rows.Count; i1++)
            {
                DataRow dr = dt.Rows[i1];
                if (i1 == 0)//for First LedgerID
                    LedgerID = "'" + (dr["LedgerID"].ToString()) + "'";
                else  //separating other LedgerID by comma
                    LedgerID += "," + "'" + (dr["LedgerID"].ToString()) + "'";
            }

            //Finally write the query which will gives thoes types of ledgers which should not contains cashbank ledgers
            string strQuery = "";
            if (LedgerID != "" && strCashBankLedgers != "")
            {
                strQuery = "SELECT DISTINCT LedgerID FROM Acc.tblTransaction WHERE LedgerID IN (" + (LedgerID) + ") AND  LedgerID NOT IN (" + strCashBankLedgers + ")";
                dtAllLedgerExceptCashBank = Global.m_db.SelectQry(strQuery, "tbl");
            }
            return dtAllLedgerExceptCashBank;

        }

        public static DataTable GetCFReportDataForDataGrid(DateTime? stDate, DateTime? enDate, string xmlAccountingClassIDs, string xmlProjectIDs)
        {
           
                DataTable dt = new DataTable();
                Global.m_db.ClearParameter();
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("Acc.spCashFlowReport");
                Global.m_db.AddParameter("@AccountClassIDsSettings", SqlDbType.NVarChar, xmlAccountingClassIDs);
                Global.m_db.AddParameter("@ProjectIDsSettings", SqlDbType.NVarChar, xmlProjectIDs);
                if (stDate != null)
                    Global.m_db.AddParameter("@Transaction_Start_Date", SqlDbType.DateTime, Convert.ToDateTime(stDate));
                else
                    Global.m_db.AddParameter("@Transaction_Start_Date", SqlDbType.DateTime, null); //DBNull.Value);
                if (enDate != null)
                    Global.m_db.AddParameter("@Transaction_End_Date", SqlDbType.DateTime, Convert.ToDateTime(enDate));
                else
                    Global.m_db.AddParameter("@Transaction_End_Date", SqlDbType.DateTime, null);//DBNull.Value);      

                dt = Global.m_db.GetDataTable();
                return dt;
            
           
        }

    }
}
