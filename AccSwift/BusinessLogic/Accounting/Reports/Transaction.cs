using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;
using System.Windows.Forms;
using DateManager;

namespace BusinessLogic
{
    public class Transaction
    {
        /// <summary>
        /// Gets the selected root accounting class ID
        /// </summary>
        /// <returns></returns>
        /// 
        
        private static int GetRootAccClassID(ArrayList AccClassID)
        {
            if (AccClassID.Count > 0)
            {
                //Find Root Class
                DataTable dtTemp = AccountClass.GetRootAccClass(Convert.ToInt32(AccClassID[0]));
                return Convert.ToInt32(dtTemp.Rows[0]["AccClassID"]);

            }

            return 1;//The default root class ID
        } 

        public static void GetGroupBalance(DateTime? FromDate, DateTime? ToDate, int m_GroupID,bool IncludeSubgroups, ref double DebitBalance, ref double CreditBalance, ArrayList AccClassID, int ProjectID)
        {
            DataTable dt;
            if (m_GroupID != 0)
            {
                ArrayList tmpGroupID = new ArrayList();//Dyanamically allocating array type of variable tmpGroupID   
                tmpGroupID.Add(m_GroupID);// it add itself too           

                if (IncludeSubgroups == true)
                    AccountGroup.GetAccountsUnder(m_GroupID, tmpGroupID);//Calling this function for collecting subGroupsID which fall under GroupID and storing on arrylist         

                string GroupID = "";
                int i = 0;
                foreach (object j in tmpGroupID)
                {
                    if (i == 0)// for first GroupID
                        GroupID = "'" + j.ToString() + "'";
                    else  //Separating Other GroupID by commas
                        GroupID += "," + "'" + j.ToString() + "'";
                    i++;
                }


                //Collect all ledgers that fall under these groups
                 dt = Global.m_db.SelectQry("SELECT * FROM Acc.tblLedger WHERE GroupID IN (" + (GroupID) + ")", "GroupID");
            }
            else
            {
                //Collect all ledgers that fall under these groups
                dt = Global.m_db.SelectQry("SELECT * FROM Acc.tblLedger", "GroupID");
            }

            if (dt.Rows.Count <= 0)
            {
                DebitBalance = 0;
                CreditBalance = 0;
                return;
            }
            string LedgerID = "";

            for (int i1 = 0; i1 < dt.Rows.Count; i1++)
            {
                DataRow dr = dt.Rows[i1];
                if (i1 == 0)//for First LedgerID
                    LedgerID = "'" + (dr["LedgerID"].ToString()) + "'";
                else  //separating other LedgerID by comma

                    LedgerID += "," + "'" + (dr["LedgerID"].ToString()) + "'";
            }


            #region BLOCK FOR COLLECTING THE OPENING BALANCE OF LIST OF LEDGERS OF CORRESPONDING GROUPID

            //BLOCK FOR COLLECTING THE OPENING BALANCE OF LIST OF LEDGERS
            double TotalDrBal, TotalCrBal;
            TotalDrBal = TotalCrBal = 0;
            int RootID = GetRootAccClassID(AccClassID);

            DataTable dtGetOpBalance = Global.m_db.SelectQry("SELECT * FROM Acc.tblOpeningBalance WHERE LedgerID IN (" + LedgerID + ")  AND AccClassID='" + RootID.ToString() + "'", "table");
            for (int k = 0; k < dtGetOpBalance.Rows.Count; k++)
            {
                DataRow drGetOpBalance = dtGetOpBalance.Rows[k];
                if (!drGetOpBalance.IsNull("OpenBal"))
                {
                    if (drGetOpBalance["OpenBalDrCr"].ToString() == "DEBIT")
                        TotalDrBal += Convert.ToDouble(drGetOpBalance["OpenBal"]);
                    else if (drGetOpBalance["OpenBalDrCr"].ToString() == "CREDIT")
                        TotalCrBal += Convert.ToDouble(drGetOpBalance["OpenBal"]);
                }
            }
            #endregion

            //Convert AccClassID ArrayList to TransactionID ArrayList and Prepare for the SQL Injection
            string TransactionID = AccountClass.GetTransactIDFromAccClassID(AccClassID);
         
            //Selecting the all contents of tblTransaction with help of LedgerID, Date and Transaction
            string SQLTransactionID = "";
            if (TransactionID != "")//There are transaction in the selected accounting class
            {
                SQLTransactionID = " AND TransactionID IN (" + TransactionID + ") order by Debit_Amount";
            }
            else if(AccClassID.Count>0)//There are no transaction in the selected accounting class
            {
                SQLTransactionID = " AND TransactionID IS NULL order by Debit_Amount";
            }

            string strFinalQuery ="";

            string ProjectSQL = "";
            ArrayList arrchildProjectIds = new ArrayList();
            Project.GetChildProjects(Convert.ToInt32(ProjectID),ref arrchildProjectIds);
            ArrayList ProjectIDCollection = new ArrayList();
            foreach (object obj in arrchildProjectIds)
            {
                int p = (int)obj;
                ProjectIDCollection.Add(p.ToString());
            }

            if (ProjectID > 0) //A Project is selected
            {
                //Collect all Project  Which parent id is given projectid
                DataTable dtproject = Global.m_db.SelectQry("select * from Acc.tblProject WHERE ParentProjectID IN (" + (ProjectID) + ")", "GroupID");
                string ProjectIDS = "";

                ProjectIDS = "'" + ProjectID + "'";
             
                for (int iproject = 0; iproject < ProjectIDCollection.Count; iproject++)
                {                 
                    //separating other LedgerID by comma
                    ProjectIDS += "," + "'" + (ProjectIDCollection[iproject].ToString()) + "'";
                }
                ProjectSQL = "AND ProjectID IN (" + (ProjectIDS) + ")";

                // }

            }
            else
            {
                //ProjectSQL = "AND ProjectID = '"+ProjectID+"'";
                ProjectSQL = " ";
            }
            string DateSQL = "";
            if (FromDate != null && ToDate != null)//There are both from and to dates
            {
                DateSQL = " AND TransactDate BETWEEN '" + Date.ToDB(Convert.ToDateTime(FromDate)) + "' AND '" + Date.ToDB(Convert.ToDateTime(ToDate)) +"'";
            }
            else if (FromDate != null)//FromDate is not given but ToDate is given, in this case show all transaction prior than ToDate
            {
                DateSQL = " AND TransactDate>='" + Date.ToDB(Convert.ToDateTime(FromDate)) + "'";
            }
            else if (ToDate != null)
            {
                DateSQL = " AND TransactDate<='" + Date.ToDB(Convert.ToDateTime(ToDate)) + "'";
            }
            strFinalQuery = "SELECT * FROM Acc.tblTransaction WHERE LedgerID IN (" + (LedgerID) + ")" + ProjectSQL + DateSQL + SQLTransactionID;
            
            DataTable dt2 = Global.m_db.SelectQry(strFinalQuery, "LedgerID");
                

            for (int j = 0; j < dt2.Rows.Count; j++)
            {
                DataRow dr1 = dt2.Rows[j];

                    DebitBalance +=Convert.ToDouble(dr1["Debit_Amount"]);

                    CreditBalance += Convert.ToDouble(dr1["Credit_Amount"]);                   
            }
            if (TotalDrBal > TotalCrBal)
            {
                // DebitBalance += (TotalDrBal - TotalCrBal);
                DebitBalance += (TotalDrBal - CreditBalance);
                CreditBalance = 0;
            }
            else if (TotalDrBal < TotalCrBal)
            {
                // CreditBalance += (TotalCrBal - TotalDrBal);
                CreditBalance += (TotalCrBal - DebitBalance);
                DebitBalance = 0;
            }
            if (TotalDrBal == 0 && TotalCrBal == 0)
            {
                if (DebitBalance < CreditBalance)
                {
                    CreditBalance = (CreditBalance - DebitBalance);
                    DebitBalance = 0;
                }
                else
                {
                    DebitBalance = (DebitBalance - CreditBalance);
                    CreditBalance = 0;
                }
            }
        }

        public static void GetOpeningBalanceFromGroup(int GroupID, ArrayList AccClassID, ref double DrOpBal, ref double CrOpBal)
        {
            //Collect ledgers from the groupID
            DataTable dtLedgers=Ledger.GetAllLedger(GroupID);
            ArrayList strLedgerID=new ArrayList();
            foreach(DataRow dr in dtLedgers.Rows)
            {
                strLedgerID.Add(dr["LedgerID"]);
            }

            //BLOCK FOR COLLECTING THE OPENING BALANCE OF LIST OF LEDGERS
            double TotalDrBal, TotalCrBal;
            TotalDrBal = TotalCrBal = 0;
            int RootID = GetRootAccClassID(AccClassID);
            int[] strID=(int[])strLedgerID.ToArray(typeof(int));

            //Checking whether strLedgerID have Ledger or not?
            if (strLedgerID.Count > 0)//Only go throgh this bloch when there are ledgers otherwise no need to proceed below's code
            {
                string strQuery = "SELECT * FROM Acc.tblOpeningBalance WHERE LedgerID IN(" + String.Join(",", Array.ConvertAll((int[])strLedgerID.ToArray(typeof(int)), x => "'" + x.ToString() + "'")) + ") AND AccClassID='" + RootID.ToString() + "'";
                DataTable dtGetOpBalance = Global.m_db.SelectQry(strQuery, "table");
                if (dtGetOpBalance.Rows.Count > 0)
                {
                    for (int k = 0; k < dtGetOpBalance.Rows.Count; k++)
                    {
                        DataRow drGetOpBalance = dtGetOpBalance.Rows[k];
                        if (!drGetOpBalance.IsNull("OpenBal"))
                        {
                            if (drGetOpBalance["OpenBalDrCr"].ToString() == "DEBIT")
                                DrOpBal += Convert.ToDouble(drGetOpBalance["OpenBal"]);
                            else if (drGetOpBalance["OpenBalDrCr"].ToString() == "CREDIT")
                                CrOpBal += Convert.ToDouble(drGetOpBalance["OpenBal"]);
                        }
                    }
                }
            }

        }

        public static void GetOpeningBalance(int LedgerID,ArrayList AccClassID,ref double DrOpBal,ref double CrOpBal)
        {

            //BLOCK FOR COLLECTING THE OPENING BALANCE OF LIST OF LEDGERS
            double TotalDrBal, TotalCrBal;
            TotalDrBal = TotalCrBal = 0;
            int RootID = GetRootAccClassID(AccClassID);
            DataTable dtGetOpBalance = Global.m_db.SelectQry("SELECT * FROM Acc.tblOpeningBalance WHERE LedgerID ='" + LedgerID + "' AND AccClassID='" + RootID.ToString() + "'", "table");
            for (int k = 0; k < dtGetOpBalance.Rows.Count; k++)
            {
                DataRow drGetOpBalance = dtGetOpBalance.Rows[k];
                if (!drGetOpBalance.IsNull("OpenBal"))
                {
                    if (drGetOpBalance["OpenBalDrCr"].ToString() == "DEBIT")
                        DrOpBal += Convert.ToDouble(drGetOpBalance["OpenBal"]);
                    else if (drGetOpBalance["OpenBalDrCr"].ToString() == "CREDIT")
                        CrOpBal += Convert.ToDouble(drGetOpBalance["OpenBal"]);
                }
            }
            
        }

        public static void GetLedgerBalance(DateTime? FromDate, DateTime? ToDate, int LedgerID, ref double DebitBalance, ref double CreditBalance,ArrayList  AccClassID,int ProjectID)
        {         
            #region BLOCK FOR COLLECTING THE OPENING BALANCE OF LIST OF LEDGERS OF CORRESPONDING GROUPID
          
            //BLOCK FOR COLLECTING THE OPENING BALANCE OF LIST OF LEDGERS
            double TotalDrBal, TotalCrBal;
            TotalDrBal = TotalCrBal = 0;
            //int RootID = GetRootAccClassID(AccClassID);
            //DataTable dtGetOpBalance = Global.m_db.SelectQry("SELECT * FROM Acc.tblOpeningBalance WHERE LedgerID ='"+LedgerID+"' AND AccClassID='" + RootID.ToString() + "'", "table");
            //for (int k = 0; k < dtGetOpBalance.Rows.Count; k++)
            //{
            //    DataRow drGetOpBalance = dtGetOpBalance.Rows[k];
            //    if (!drGetOpBalance.IsNull("OpenBal"))
            //    {
            //        if (drGetOpBalance["OpenBalDrCr"].ToString() == "DEBIT")
            //            TotalDrBal += Convert.ToDouble(drGetOpBalance["OpenBal"]);
            //        else if (drGetOpBalance["OpenBalDrCr"].ToString() == "CREDIT")
            //            TotalCrBal += Convert.ToDouble(drGetOpBalance["OpenBal"]);
            //    }
            //}
            #endregion

            ////Convert AccClassID ArrayList to TransactionID ArrayList and Prepare for the SQL Injection
            //string TransactionID = AccountClass.GetTransactIDFromAccClassID(AccClassID);
          
            ////Selecting the all contents of tblTransaction with help of LedgerID
            //// formate in this way SELECT * FROM Acc.tblTransaction WHERE LedgerID IN ('41','40','32','36','33')
            
            //string SQLTransactionID="";
            //if (TransactionID != "")
            //{
            //    SQLTransactionID = " AND TransactionID IN (" + TransactionID +  ")";
            //}
            //else if (AccClassID.Count > 0)//There are no transaction in the selected accounting class
            //{
            //    SQLTransactionID = " AND TransactionID IS NULL";
            //}

            //string strFinalQuery = "";
            //string ProjectSQL = "";
            //ArrayList arrchildProjectIds = new ArrayList();
            //Project.GetChildProjects(Convert.ToInt32(ProjectID), ref arrchildProjectIds);
            //ArrayList ProjectIDCollection = new ArrayList();
            //foreach (object obj in arrchildProjectIds)
            //{
            //    int p = (int)obj;
            //    ProjectIDCollection.Add(p.ToString());
            //}

            //#region A Project is selected
            //if (ProjectID > 0)
                
            //{
            //    //Collect all Project  Which parent id is given projectid
            //    DataTable dtproject = Global.m_db.SelectQry("select * from Acc.tblProject WHERE ParentProjectID IN (" + (ProjectID) + ")", "GroupID");
            //    string ProjectIDS = "";

            //    ProjectIDS = "'" + ProjectID + "'";
               
            //    for (int iproject = 0; iproject < ProjectIDCollection.Count; iproject++)
            //    {
            //        ProjectIDS += "," + "'" + (ProjectIDCollection[iproject].ToString()) + "'";
            //    }
            //    ProjectSQL = "AND ProjectID IN (" + (ProjectIDS) + ")";
            //}
            //else
            //{
            //   // ProjectSQL = "AND ProjectID = '" + ProjectID + "'";
            //    ProjectSQL = " ";
            //}
            //#endregion


            //string DateSQL = "";

            //    //There are both from and to dates
            //if (FromDate != null && ToDate != null)
            //    {
            //        DateSQL = " AND TransactDate BETWEEN '" + Date.ToDB(Convert.ToDateTime(FromDate)) + "' AND '" + Date.ToDB(Convert.ToDateTime(ToDate)) + "'";
            //    }
            //    else if (FromDate != null)//FromDate is not given but ToDate is given, in this case show all transaction prior than ToDate
            //    {
            //        DateSQL = " AND TransactDate>='" + Date.ToDB(Convert.ToDateTime(FromDate)) + "'";
            //    }
            //    else if (ToDate != null)
            //    {
            //        DateSQL = " AND TransactDate<='" + Date.ToDB(Convert.ToDateTime(ToDate)) + "'";
            //    }

            //strFinalQuery = "SELECT * FROM Acc.tblTransaction WHERE  LedgerID='" + LedgerID + "'" + ProjectSQL + DateSQL + SQLTransactionID;
            //DataTable dt2 = Global.m_db.SelectQry(strFinalQuery, "LedgerID");

            //for (int j = 0; j < dt2.Rows.Count; j++)
            //{
            //    DataRow dr1 = dt2.Rows[j];
            //    DebitBalance += Convert.ToDouble(dr1["Debit_Amount"]);
            //    CreditBalance += Convert.ToDouble(dr1["Credit_Amount"]);
            //}
            //}
            // here make comment
            //if (TotalDrBal > TotalCrBal)
            //    DebitBalance += (TotalDrBal - TotalCrBal);
            //else if (TotalDrBal < TotalCrBal)
            //    CreditBalance += (TotalCrBal - TotalDrBal);

            //Just for a while, static variables
            string AccClassIDsXMLString="<AccClassIDSettings><AccClassID>1</AccClassID></AccClassIDSettings>";
            string ProjectIDsXMLString = "<ProjectIDSettings><ProjectID>1</ProjectID></ProjectIDSettings>";
            try
                {

                DataTable dtLedgerDetails = Ledger.GetLedgerDetails1(AccClassIDsXMLString, ProjectIDsXMLString, null, null, LedgerID,0, null);
                DataRow dr1 = dtLedgerDetails.Rows[0];
                DebitBalance += Convert.ToDouble(dr1["DebitTotal"]);
                CreditBalance += Convert.ToDouble(dr1["CreditTotal"]);
                if (dr1["OpenBalDr"] != DBNull.Value)
                    TotalDrBal += Convert.ToDouble(dr1["OpenBalDr"]);
                if (dr1["OpenBalCr"] != DBNull.Value)
                    TotalCrBal += Convert.ToDouble(dr1["OpenBalCr"]);

                // here make comment
                if (TotalDrBal > TotalCrBal)
                    DebitBalance += (TotalDrBal - TotalCrBal);
                else if (TotalDrBal < TotalCrBal)
                    CreditBalance += (TotalCrBal - TotalDrBal);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    // throw ex;
                     
                }
            finally
            {
                //Global.m_db.DisConnect();
                //Global.m_db.CommitTransaction();
                Global.m_db.cn.Close();
            }
        
               
        }

        public static void GetLedgerBalanceForAccountLedger(DateTime? FromDate, DateTime? ToDate, int LedgerID, ref double DebitBalance, ref double CreditBalance, ArrayList AccClassID, int ProjectID)
        {
            #region BLOCK FOR COLLECTING THE OPENING BALANCE OF LIST OF LEDGERS OF CORRESPONDING GROUPID

            //BLOCK FOR COLLECTING THE OPENING BALANCE OF LIST OF LEDGERS
            double TotalDrBal, TotalCrBal;
            TotalDrBal = TotalCrBal = 0;
            int RootID = GetRootAccClassID(AccClassID);
            DataTable dtGetOpBalance = Global.m_db.SelectQry("SELECT * FROM Acc.tblOpeningBalance WHERE LedgerID ='" + LedgerID + "' AND AccClassID='" + RootID.ToString() + "'", "table");
            for (int k = 0; k < dtGetOpBalance.Rows.Count; k++)
            {
                DataRow drGetOpBalance = dtGetOpBalance.Rows[k];
                if (!drGetOpBalance.IsNull("OpenBal"))
                {
                    if (drGetOpBalance["OpenBalDrCr"].ToString() == "DEBIT")
                        TotalDrBal += Convert.ToDouble(drGetOpBalance["OpenBal"]);
                    else if (drGetOpBalance["OpenBalDrCr"].ToString() == "CREDIT")
                        TotalCrBal += Convert.ToDouble(drGetOpBalance["OpenBal"]);
                }
            }
            #endregion

            //Convert AccClassID ArrayList to TransactionID ArrayList and Prepare for the SQL Injection
            string TransactionID = AccountClass.GetTransactIDFromAccClassID(AccClassID);

            //Selecting the all contents of tblTransaction with help of LedgerID
            // formate in this way SELECT * FROM Acc.tblTransaction WHERE LedgerID IN ('41','40','32','36','33')
            //string sql = "SELECT * FROM Acc.tblTransaction WHERE LedgerID IN (" + (LedgerID) + ") AND TransactDate BETWEEN '" + FromDate + "' AND '" + ToDate + "'";
            string SQLTransactionID = "";
            if (TransactionID != "")
            {
                SQLTransactionID = " AND TransactionID IN (" + TransactionID + ")";
            }
            else if (AccClassID.Count > 0)//There are no transaction in the selected accounting class
            {
                SQLTransactionID = " AND TransactionID IS NULL";
            }

            string strFinalQuery = "";
            string ProjectSQL = "";
            ArrayList arrchildProjectIds = new ArrayList();
            Project.GetChildProjects(Convert.ToInt32(ProjectID), ref arrchildProjectIds);
            ArrayList ProjectIDCollection = new ArrayList();
            foreach (object obj in arrchildProjectIds)
            {
                int p = (int)obj;
                ProjectIDCollection.Add(p.ToString());
            }

            #region A Project is selected
            if (ProjectID > 0)
            {
                //Collect all Project  Which parent id is given projectid
                DataTable dtproject = Global.m_db.SelectQry("select * from Acc.tblProject WHERE ParentProjectID IN (" + (ProjectID) + ")", "GroupID");
                string ProjectIDS = "";

                ProjectIDS = "'" + ProjectID + "'";

                for (int iproject = 0; iproject < ProjectIDCollection.Count; iproject++)
                {
                    ProjectIDS += "," + "'" + (ProjectIDCollection[iproject].ToString()) + "'";
                }
                ProjectSQL = "AND ProjectID IN (" + (ProjectIDS) + ")";


            }
            else
            {
                // ProjectSQL = "AND ProjectID = '" + ProjectID + "'";
                ProjectSQL = " ";
            }
            #endregion


            string DateSQL = "";

            //To Find the ledgerbalance upto one day ago
            //Represents the date one day earlier
            ToDate = (Convert.ToDateTime(ToDate).AddDays(-1));
            DateTime dtime = new DateTime();
            dtime = (Convert.ToDateTime(ToDate));


            //There are both from and to dates
            if (FromDate != null && dtime != null)
            {
                DateSQL = " AND TransactDate BETWEEN '" + Date.ToDB(Convert.ToDateTime(FromDate)) + "' AND '" + Date.ToDB(Convert.ToDateTime(dtime)) + "'";
            }
            else if (FromDate != null)//FromDate is not given but ToDate is given, in this case show all transaction prior than ToDate
            {
                DateSQL = " AND TransactDate>='" + Date.ToDB(Convert.ToDateTime(FromDate)) + "'";
            }
            else if (ToDate != null)
            {
                DateSQL = " AND TransactDate<='" + Date.ToDB(Convert.ToDateTime(dtime)) + "'";
            }

            strFinalQuery = "SELECT * FROM Acc.tblTransaction WHERE  LedgerID='" + LedgerID + "'" + ProjectSQL + DateSQL + SQLTransactionID;
            DataTable dt2 = Global.m_db.SelectQry(strFinalQuery, "LedgerID");

            for (int j = 0; j < dt2.Rows.Count; j++)
            {
                DataRow dr1 = dt2.Rows[j];
                DebitBalance += Convert.ToDouble(dr1["Debit_Amount"]);
                CreditBalance += Convert.ToDouble(dr1["Credit_Amount"]);
            }

            // here make comment
            if (TotalDrBal > TotalCrBal)
                DebitBalance += (TotalDrBal - TotalCrBal);
            else if (TotalDrBal < TotalCrBal)
                CreditBalance += (TotalCrBal - TotalDrBal);
        }
     
        public DataTable GetTransactionInfo(string RowID, string VoucherType,ArrayList AccClassID)
        {
            string TrasactIDByAccClass = AccountClass.GetTransactIDFromAccClassID(AccClassID);
            return Global.m_db.SelectQry("SELECT * FROM Acc.tblTransaction WHERE RowID='" + RowID + "' AND VoucherType ='" + VoucherType + "' AND TransactionID IN (" + (TrasactIDByAccClass) + ")", "TransactionInfo");
        }

        public DataTable GetTransactionInfoAlongWithParty(string RowID, string VoucherType,int PartyID, ArrayList AccClassID)
        {
            string TrasactIDByAccClass = AccountClass.GetTransactIDFromAccClassID(AccClassID);
                 string subqry = "";
            if(PartyID == 0)
                 subqry = "";
            else
                 subqry = "' AND LedgerID = '" + PartyID;
           // MessageBox.Show("SELECT * FROM Acc.tblTransaction WHERE RowID='" + RowID+ subqry + "' AND VoucherType ='" + VoucherType + "' AND TransactionID IN (" + (TrasactIDByAccClass) + ")", "TransactionInfo");
            return Global.m_db.SelectQry("SELECT * FROM Acc.tblTransaction WHERE RowID='" + RowID+ subqry + "' AND VoucherType ='" + VoucherType + "' AND TransactionID IN (" + (TrasactIDByAccClass) + ")", "TransactionInfo");
        }

        public DataTable GetTransactionInfo(string RowID, DateTime? FromDate, DateTime? ToDate,ArrayList AccClassID)
        {
            //Also filter according to AccountclassID
            //First find the TransactionIDs which falls under selected AccountClassIDs
            string TransactionIDsByAccClass = AccountClass.GetTransactIDFromAccClassID(AccClassID);   
            string strQuery = "";
            if ((FromDate == null) && (ToDate == null))//if date range is not selected
            {
                strQuery = "SELECT * FROM Acc.tblTransaction WHERE RowID='" + RowID + "' AND TransactionID IN (" + (TransactionIDsByAccClass) + ")";
                return Global.m_db.SelectQry(strQuery, "TransactionInfo");
            }
            else
            {
                strQuery = "SELECT * FROM Acc.tblTransaction WHERE RowID='" + RowID + "' AND TransactDate BETWEEN '" + Date.ToDB(Convert.ToDateTime(FromDate)) + "' AND '" + Date.ToDB(Convert.ToDateTime(ToDate)) + "' AND TransactionID IN (" + (TransactionIDsByAccClass) + ")";
                return Global.m_db.SelectQry(strQuery, "TransactionInfo");
            }
        }

        public DataTable GetTransactionInfoByAccClassAndProjectID(string RowID, DateTime? FromDate, DateTime? ToDate, ArrayList AccClassID, ArrayList ProjectID)
        {

            //Also filter according to AccountclassID
            //First find the TransactionIDs which falls under selected AccountClassIDs

            //string TransactionIDsByAccClass = AccountClass.GetTransactIDFromAccClassID(AccClassID);
            string TransactionIDsByAccClass = AccountClass.GetTransactIDFromAccClassIDAndProjectID(AccClassID, ProjectID);
            string strQuery = "";
            if ((FromDate == null) && (ToDate == null))//if date range is not selected
            {
                strQuery = "SELECT * FROM Acc.tblTransaction WHERE RowID='" + RowID + "' AND TransactionID IN (" + (TransactionIDsByAccClass) + ")";
                return Global.m_db.SelectQry(strQuery, "TransactionInfo");
            }
            else
            {
                strQuery = "SELECT * FROM Acc.tblTransaction WHERE RowID='" + RowID + "' AND TransactDate BETWEEN '" + Date.ToDB(Convert.ToDateTime(FromDate)) + "' AND '" + Date.ToDB(Convert.ToDateTime(ToDate)) + "' AND TransactionID IN (" + (TransactionIDsByAccClass) + ")";
                return Global.m_db.SelectQry(strQuery, "TransactionInfo");
            }

        }

        /// <summary>
        /// Returns the table of Transaction Details from the given parameter
        /// </summary>
        /// <param name="RowID"></param>
        /// <param name="LedgerID"></param>
        /// <param name="FromDate"></param>
        /// <param name="ToDate"></param>
        /// <param name="AccClassID"></param>
        /// <param name="ProjectID"></param>
        /// <returns></returns>
       
        public static DataTable GetVouchersListDetails(DateTime? FromDate, DateTime? ToDate, string AccClassIDsXMLString, string ProjectIDsXMLString)
        {
            // string TransactionIDsByAccClass = AccountClass.GetTransactIDFromAccClassIDAndProjectID(AccClassID,ProjectID);

            try
            {
                Global.m_db.ClearParameter();
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("Acc.spGetVouchersListDetails");

                // if(FromDate!=null)
                Global.m_db.AddParameter("@FromDate", SqlDbType.DateTime, FromDate);
                //if(ToDate!=null)
                Global.m_db.AddParameter("@ToDate", SqlDbType.DateTime, ToDate);
                Global.m_db.AddParameter("@AccountClassIDsSettings", SqlDbType.Xml, AccClassIDsXMLString);
                Global.m_db.AddParameter("@ProjectIDsSettings", SqlDbType.Xml, ProjectIDsXMLString);
                DataTable dtTransactionDetails = Global.m_db.GetDataTable();
                return dtTransactionDetails;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                // throw ex;
                return null;
            }
        }
        //checking
        public static DataTable GetVouchersDetailsJournal(DateTime? FromDate, DateTime? ToDate, string AccClassIDsXMLString, string ProjectIDsXMLString)
        {
            // string TransactionIDsByAccClass = AccountClass.GetTransactIDFromAccClassIDAndProjectID(AccClassID,ProjectID);

            try
            {

                Global.m_db.ClearParameter();
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("Acc.spGetVouchersDetailsJournal");

                // if(FromDate!=null)
                Global.m_db.AddParameter("@FromDate", SqlDbType.DateTime, FromDate);
                //if(ToDate!=null)
                Global.m_db.AddParameter("@ToDate", SqlDbType.DateTime, ToDate);      
                Global.m_db.AddParameter("@AccountClassIDsSettings", SqlDbType.Xml, AccClassIDsXMLString);
                Global.m_db.AddParameter("@ProjectIDsSettings", SqlDbType.Xml, ProjectIDsXMLString);
                DataTable dtTransactionDetails = Global.m_db.GetDataTable();
                return dtTransactionDetails;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                // throw ex;
                return null;
            }
        }

        public static DataTable GetVouchersDetailsContra(DateTime? FromDate, DateTime? ToDate, string AccClassIDsXMLString, string ProjectIDsXMLString)
        {
            // string TransactionIDsByAccClass = AccountClass.GetTransactIDFromAccClassIDAndProjectID(AccClassID,ProjectID);

            try
            {

                Global.m_db.ClearParameter();
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("Acc.spGetVouchersDetailsContra");

                // if(FromDate!=null)
                Global.m_db.AddParameter("@FromDate", SqlDbType.DateTime, FromDate);
                //if(ToDate!=null)
                Global.m_db.AddParameter("@ToDate", SqlDbType.DateTime, ToDate);
                Global.m_db.AddParameter("@AccountClassIDsSettings", SqlDbType.Xml, AccClassIDsXMLString);
                Global.m_db.AddParameter("@ProjectIDsSettings", SqlDbType.Xml, ProjectIDsXMLString);
                DataTable dtTransactionDetails = Global.m_db.GetDataTable();
                return dtTransactionDetails;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                // throw ex;
                return null;
            }
        }

        public static DataTable GetVouchersDetailsCashReceipt(DateTime? FromDate, DateTime? ToDate, string AccClassIDsXMLString, string ProjectIDsXMLString)
        {
            // string TransactionIDsByAccClass = AccountClass.GetTransactIDFromAccClassIDAndProjectID(AccClassID,ProjectID);

            try
            {

                Global.m_db.ClearParameter();
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("Acc.spGetVouchersDetailsCashReceipt");

                // if(FromDate!=null)
                Global.m_db.AddParameter("@FromDate", SqlDbType.DateTime, FromDate);
                //if(ToDate!=null)
                Global.m_db.AddParameter("@ToDate", SqlDbType.DateTime, ToDate);
                Global.m_db.AddParameter("@AccountClassIDsSettings", SqlDbType.Xml, AccClassIDsXMLString);
                Global.m_db.AddParameter("@ProjectIDsSettings", SqlDbType.Xml, ProjectIDsXMLString);
                DataTable dtTransactionDetails = Global.m_db.GetDataTable();
                return dtTransactionDetails;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                // throw ex;
                return null;
            }
        }

        public static DataTable GetVouchersDetailsCashPayment(DateTime? FromDate, DateTime? ToDate, string AccClassIDsXMLString, string ProjectIDsXMLString)
        {
            // string TransactionIDsByAccClass = AccountClass.GetTransactIDFromAccClassIDAndProjectID(AccClassID,ProjectID);

            try
            {

                Global.m_db.ClearParameter();
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("Acc.spGetVouchersDetailsCashPayment");

                // if(FromDate!=null)
                Global.m_db.AddParameter("@FromDate", SqlDbType.DateTime, FromDate);
                //if(ToDate!=null)
                Global.m_db.AddParameter("@ToDate", SqlDbType.DateTime, ToDate);
                Global.m_db.AddParameter("@AccountClassIDsSettings", SqlDbType.Xml, AccClassIDsXMLString);
                Global.m_db.AddParameter("@ProjectIDsSettings", SqlDbType.Xml, ProjectIDsXMLString);
                DataTable dtTransactionDetails = Global.m_db.GetDataTable();
                return dtTransactionDetails;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                // throw ex;
                return null;
            }
        }

        public static DataTable GetVouchersDetailsBankPayment(DateTime? FromDate, DateTime? ToDate, string AccClassIDsXMLString, string ProjectIDsXMLString)
        {
            // string TransactionIDsByAccClass = AccountClass.GetTransactIDFromAccClassIDAndProjectID(AccClassID,ProjectID);

            try
            {

                Global.m_db.ClearParameter();
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("Acc.spGetVouchersDetailsBankPayment");

                // if(FromDate!=null)
                Global.m_db.AddParameter("@FromDate", SqlDbType.DateTime, FromDate);
                //if(ToDate!=null)
                Global.m_db.AddParameter("@ToDate", SqlDbType.DateTime, ToDate);
                Global.m_db.AddParameter("@AccountClassIDsSettings", SqlDbType.Xml, AccClassIDsXMLString);
                Global.m_db.AddParameter("@ProjectIDsSettings", SqlDbType.Xml, ProjectIDsXMLString);
                DataTable dtTransactionDetails = Global.m_db.GetDataTable();
                return dtTransactionDetails;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                // throw ex;
                return null;
            }
        }

        public static DataTable GetVouchersDetailsBankReceipt(DateTime? FromDate, DateTime? ToDate, string AccClassIDsXMLString, string ProjectIDsXMLString)
        {
            // string TransactionIDsByAccClass = AccountClass.GetTransactIDFromAccClassIDAndProjectID(AccClassID,ProjectID);

            try
            {

                Global.m_db.ClearParameter();
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("Acc.spGetVouchersDetailsBankReceipt");

                // if(FromDate!=null)
                Global.m_db.AddParameter("@FromDate", SqlDbType.DateTime, FromDate);
                //if(ToDate!=null)
                Global.m_db.AddParameter("@ToDate", SqlDbType.DateTime, ToDate);
                Global.m_db.AddParameter("@AccountClassIDsSettings", SqlDbType.Xml, AccClassIDsXMLString);
                Global.m_db.AddParameter("@ProjectIDsSettings", SqlDbType.Xml, ProjectIDsXMLString);
                DataTable dtTransactionDetails = Global.m_db.GetDataTable();
                return dtTransactionDetails;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                // throw ex;
                return null;
            }
        }

        public static DataTable GetVouchersDetailsSales(DateTime? FromDate, DateTime? ToDate, string AccClassIDsXMLString, string ProjectIDsXMLString)
        {
            // string TransactionIDsByAccClass = AccountClass.GetTransactIDFromAccClassIDAndProjectID(AccClassID,ProjectID);

            try
            {

                Global.m_db.ClearParameter();
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("Inv.spGetVouchersDetailsSales");

                // if(FromDate!=null)
                Global.m_db.AddParameter("@FromDate", SqlDbType.DateTime, FromDate);
                //if(ToDate!=null)
                Global.m_db.AddParameter("@ToDate", SqlDbType.DateTime, ToDate);
                Global.m_db.AddParameter("@AccountClassIDsSettings", SqlDbType.Xml, AccClassIDsXMLString);
                Global.m_db.AddParameter("@ProjectIDsSettings", SqlDbType.Xml, ProjectIDsXMLString);
                DataTable dtTransactionDetails = Global.m_db.GetDataTable();
                return dtTransactionDetails;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                // throw ex;
                return null;
            }
        }

        public static DataTable GetVouchersDetailsSalesReturn(DateTime? FromDate, DateTime? ToDate, string AccClassIDsXMLString, string ProjectIDsXMLString)
        {
            // string TransactionIDsByAccClass = AccountClass.GetTransactIDFromAccClassIDAndProjectID(AccClassID,ProjectID);

            try
            {

                Global.m_db.ClearParameter();
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("Inv.spGetVouchersDetailsSalesReturn");

                // if(FromDate!=null)
                Global.m_db.AddParameter("@FromDate", SqlDbType.DateTime, FromDate);
                //if(ToDate!=null)
                Global.m_db.AddParameter("@ToDate", SqlDbType.DateTime, ToDate);
                Global.m_db.AddParameter("@AccountClassIDsSettings", SqlDbType.Xml, AccClassIDsXMLString);
                Global.m_db.AddParameter("@ProjectIDsSettings", SqlDbType.Xml, ProjectIDsXMLString);
                DataTable dtTransactionDetails = Global.m_db.GetDataTable();
                return dtTransactionDetails;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                // throw ex;
                return null;
            }
        }

        public static DataTable GetVouchersDetailsPurchase(DateTime? FromDate, DateTime? ToDate, string AccClassIDsXMLString, string ProjectIDsXMLString)
        {
            // string TransactionIDsByAccClass = AccountClass.GetTransactIDFromAccClassIDAndProjectID(AccClassID,ProjectID);

            try
            {

                Global.m_db.ClearParameter();
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("Inv.spGetVouchersDetailsPurchase");

                // if(FromDate!=null)
                Global.m_db.AddParameter("@FromDate", SqlDbType.DateTime, FromDate);
                //if(ToDate!=null)
                Global.m_db.AddParameter("@ToDate", SqlDbType.DateTime, ToDate);
                Global.m_db.AddParameter("@AccountClassIDsSettings", SqlDbType.Xml, AccClassIDsXMLString);
                Global.m_db.AddParameter("@ProjectIDsSettings", SqlDbType.Xml, ProjectIDsXMLString);
                DataTable dtTransactionDetails = Global.m_db.GetDataTable();
                return dtTransactionDetails;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                // throw ex;
                return null;
            }
        }

        public static DataTable GetVouchersDetailsPurchaseReturn(DateTime? FromDate, DateTime? ToDate, string AccClassIDsXMLString, string ProjectIDsXMLString)
        {
            // string TransactionIDsByAccClass = AccountClass.GetTransactIDFromAccClassIDAndProjectID(AccClassID,ProjectID);

            try
            {

                Global.m_db.ClearParameter();
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("Inv.spGetVouchersDetailsPurchaseReturn");

                // if(FromDate!=null)
                Global.m_db.AddParameter("@FromDate", SqlDbType.DateTime, FromDate);
                //if(ToDate!=null)
                Global.m_db.AddParameter("@ToDate", SqlDbType.DateTime, ToDate);
                Global.m_db.AddParameter("@AccountClassIDsSettings", SqlDbType.Xml, AccClassIDsXMLString);
                Global.m_db.AddParameter("@ProjectIDsSettings", SqlDbType.Xml, ProjectIDsXMLString);
                DataTable dtTransactionDetails = Global.m_db.GetDataTable();
                return dtTransactionDetails;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                // throw ex;
                return null;
            }
        }

        public DataTable GetDebitTransactionInfo(DateTime? FromDate, DateTime? ToDate,ArrayList AccClassID,ArrayList ProjectID)
        {
             
            //string TransactionIDsByAccClass = AccountClass.GetTransactIDFromAccClassID(AccClassID);
            string TransactionIDsByAccClass = AccountClass.GetTransactIDFromAccClassIDAndProjectID(AccClassID, ProjectID);
            string strQuery = "";
              if ((FromDate == null) && (ToDate == null))//if date range is not selected
              {
                 // strQuery = "SELECT * FROM Acc.tblTransaction WHERE Debit_Amount >0 AND TransactionID IN (" + (TransactionIDsByAccClass) + ")";
                  if (TransactionIDsByAccClass == "")
                      strQuery = "SELECT * FROM Acc.tblTransaction WHERE Debit_Amount >0 ";
                  else
                    strQuery = "SELECT * FROM Acc.tblTransaction WHERE Debit_Amount >0 AND TransactionID IN (" + (TransactionIDsByAccClass) + ")";
              }
              else
              {
                  if (TransactionIDsByAccClass == "")
                      strQuery = "SELECT * FROM Acc.tblTransaction WHERE Debit_Amount >0 AND TransactDate BETWEEN '" + Date.ToDB(Convert.ToDateTime(FromDate)) + "' AND '" + Date.ToDB(Convert.ToDateTime(ToDate)) + "' ";
                  else
                    strQuery = "SELECT * FROM Acc.tblTransaction WHERE Debit_Amount >0 AND TransactDate BETWEEN '" + Date.ToDB(Convert.ToDateTime(FromDate)) + "' AND '" + Date.ToDB(Convert.ToDateTime(ToDate)) + "' AND TransactionID IN (" + (TransactionIDsByAccClass) + ")";
              }

              return Global.m_db.SelectQry(strQuery, "table");
        }

        public DataTable GetCreditTransactionInfo(DateTime? FromDate, DateTime? ToDate, ArrayList AccClassID,ArrayList ProjectID)
        {


           // string TransactionIDsByAccClass = AccountClass.GetTransactIDFromAccClassID(AccClassID);
            string TransactionIDsByAccClass = AccountClass.GetTransactIDFromAccClassIDAndProjectID(AccClassID, ProjectID);
              string strQuery = "";
              if((FromDate == null) && (ToDate == null))//if date range is not selected
              {
                  if (TransactionIDsByAccClass=="")
                      strQuery = "SELECT * FROM Acc.tblTransaction WHERE Credit_Amount >0 ";
                  else
                    strQuery = "SELECT * FROM Acc.tblTransaction WHERE Credit_Amount >0 AND TransactionID IN (" + (TransactionIDsByAccClass) + ")";
              }
              else
              {
                  if (TransactionIDsByAccClass == "")
                      strQuery = "SELECT * FROM Acc.tblTransaction WHERE Credit_Amount >0 AND TransactDate BETWEEN '" + Date.ToDB(Convert.ToDateTime(FromDate)) + "' AND '" + Date.ToDB(Convert.ToDateTime(ToDate)) + "'";
                  else
                    strQuery = "SELECT * FROM Acc.tblTransaction WHERE Credit_Amount >0 AND TransactDate BETWEEN '" + Date.ToDB(Convert.ToDateTime(FromDate)) + "' AND '" + Date.ToDB(Convert.ToDateTime(ToDate)) + "' AND TransactionID IN (" + (TransactionIDsByAccClass) + ")";

              }

            return Global.m_db.SelectQry(strQuery, "table");
        }

        /// <summary>
        /// Gets the table of information under the tblTransaction of specified ledger ID
        /// </summary>
        /// <param name="LedgerID"></param>
        /// <param name="FromDate"></param>
        /// <param name="ToDate"></param>
        /// <returns></returns>
        public DataTable GetLedgerTransact(int LedgerID, DateTime? FromDate, DateTime? ToDate)
        {
            string TransactDateSQL = "";
            if (FromDate != null && ToDate != null)
                TransactDateSQL = " AND TransactDate BETWEEN '" + Date.ToDB(Convert.ToDateTime(FromDate)) + "' AND '" + Date.ToDB(Convert.ToDateTime(ToDate)) + "'";
            else if (FromDate != null)
                TransactDateSQL = " AND TransactDate>='" + Date.ToDB(Convert.ToDateTime(FromDate)) + "'";
            else if (ToDate != null)
                TransactDateSQL = " AND TransactDate<='" + Date.ToDB(Convert.ToDateTime(ToDate)) + "'";
            string strFinalQuery = "";
            strFinalQuery = "SELECT * FROM Acc.tblTransaction WHERE LedgerID='" + LedgerID + "'" + TransactDateSQL;

            return Global.m_db.SelectQry(strFinalQuery, "ledgerTransact");
        }
      
        public DataTable GetLedgerTransactWithAccountClassAndProject(int LedgerID, DateTime? FromDate, DateTime? ToDate,ArrayList AccClassID,int ProjectID)
        {
            string TransactDateSQL = "";
            if (FromDate != null && ToDate != null)
                TransactDateSQL = " AND TransactDate BETWEEN '" + Date.ToDB(Convert.ToDateTime(FromDate)) + "' AND '" + Date.ToDB(Convert.ToDateTime(ToDate)) + "'";
            else if (FromDate != null)
                TransactDateSQL = " AND TransactDate>='" + Date.ToDB(Convert.ToDateTime(FromDate)) + "'";
            else if (ToDate != null)
                TransactDateSQL = " AND TransactDate<='" + Date.ToDB(Convert.ToDateTime(ToDate)) + "'";

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

            //For Project 
            string ProjectSQL = "";
            string ProjectIDS = "";
            ArrayList arrchildProjectIds = new ArrayList();
            Project.GetChildProjects(Convert.ToInt32(ProjectID), ref arrchildProjectIds);
            ArrayList ProjectIDCollection = new ArrayList();
            foreach (object obj in arrchildProjectIds)
            {
                int p = (int)obj;
                ProjectIDCollection.Add(p.ToString());
            }

            ProjectIDS = "'" + ProjectID + "'";

            for (int iproject = 0; iproject < ProjectIDCollection.Count; iproject++)
            {
                ProjectIDS += "," + "'" + (ProjectIDCollection[iproject].ToString()) + "'";
            }
            ProjectSQL = "AND ProjectID IN (" + (ProjectIDS) + ")";

            string strFinalQuery = "";
            strFinalQuery = "SELECT distinct T.* FROM Acc.tblTransaction T,Acc.tblTransactionClass TC WHERE LedgerID='" + LedgerID + "' and T.TransactionID=TC.transactionid" + " "+TransactDateSQL+AccClassIDSQL+ProjectSQL ;

            return Global.m_db.SelectQry(strFinalQuery, "ledgerTransact");
        }

        /// <summary>
        /// Gets the table of information under the tblTransaction of specified ledger ID
        /// </summary>
        /// <param name="LedgerID"></param>
        /// <param name="FromDate"></param>
        /// <param name="ToDate"></param>
        /// <returns></returns> 
        public DataTable GetLedgerTransactWithParty(int LedgerID, DateTime? FromDate, DateTime? ToDate)
        {
            string TransactDateSQL = "";
            if (FromDate != null && ToDate != null)
                TransactDateSQL = " AND TransactDate BETWEEN '" + Date.ToDB(Convert.ToDateTime(FromDate)) + "' AND '" + Date.ToDB(Convert.ToDateTime(ToDate)) + "'";
            else if (FromDate != null)
                TransactDateSQL = " AND TransactDate>='" + Date.ToDB(Convert.ToDateTime(FromDate)) + "'";
            else if (ToDate != null)
                TransactDateSQL = " AND TransactDate<='" + Date.ToDB(Convert.ToDateTime(ToDate)) + "'";

            return Global.m_db.SelectQry("SELECT * FROM Acc.tblTransaction WHERE LedgerID='" + LedgerID + "'" + TransactDateSQL, "ledgerTransact");
        }

        public DataTable GetLedgerTransact(string LedgerID)
        {
            if (LedgerID == "")//For obtaining all Ledger Transactions
            {
                string strQuery = "SELECT * FROM Acc.tblTransaction";
                return Global.m_db.SelectQry(strQuery, "ledgerinfo");
            }
            else
            {
                string strQuery = "SELECT * FROM Acc.tblTransaction WHERE LedgerID='" + LedgerID + "'";
                //string strQuery = "SELECT * FROM Acc.tblTransaction WHERE GroupID='" + LedgerID + "'";

                return Global.m_db.SelectQry(strQuery, "ledgerinfo");
            }
        }

        public DataTable GetLedgerTransactWithAccClassAndProjectID(string LedgerID,String AccClass,String ProjectID)
        {
            if (LedgerID == "")//For obtaining all Ledger Transactions
            {
                string strQuery = "SELECT * FROM Acc.tblTransaction";
                return Global.m_db.SelectQry(strQuery, "ledgerinfo");

            }
            else
            {
                string strQuery = "SELECT Distinct T.* FROM Acc.tblTransaction  T,Acc.tblTransactionClass TC WHERE LedgerID='" + LedgerID + "' and T.ProjectID IN (" + (ProjectID) + ") and TC.AccClassID IN (" + (AccClass) + ") and T.TransactionID=TC.TransactionID";
                //string strQuery = "SELECT * FROM Acc.tblTransaction WHERE GroupID='" + LedgerID + "'";

                return Global.m_db.SelectQry(strQuery, "ledgerinfo");
            }
        }

        /// <summary>
        /// This method returns datatable which contains all ledgers except cashbank 
        /// 
        /// </summary>
        /// <param name="strCashBankLedgers"></param>
        /// <param name="dtGrpLedgersDtl"></param>
        /// <returns></returns>
        public static DataTable GetAllLedgersExeptCashBank(string strCashBankLedgers, DataTable dtGrpLedgersDtl)
        {
            DataTable dtAllLedgerExceptCashBank = new DataTable();
            string AllLedgerIdUnderAccGroup = "";//This variable contains all Ledgers under end Account Group
            if(dtGrpLedgersDtl.Rows.Count>0)//Incase of end GroupID,it contains direct leders under this group
            {               
                for (int i1 = 0; i1 < dtGrpLedgersDtl.Rows.Count; i1++)
                {
                    DataRow dr = dtGrpLedgersDtl.Rows[i1];
                    if (i1 == 0)//for First LedgerID
                        AllLedgerIdUnderAccGroup = "'" + (dr["LedgerID"].ToString()) + "'";
                    else  //separating other LedgerID by comma

                        AllLedgerIdUnderAccGroup += "," + "'" + (dr["LedgerID"].ToString()) + "'";
                }
            }         
            string strQuery = "";        
            if(strCashBankLedgers!="")//Cashflow is only meaningful when Cash or Bank will transact to other Ledgers,If there is no transaction of Cash or Bank to other transaction then just ignore CashFlow part
            {
              if(AllLedgerIdUnderAccGroup!="")//If we have to collect thoes ledgers except cashbank which are the direct ledgers of end Group
              {
                  strQuery = "SELECT DISTINCT LedgerID FROM Acc.tblTransaction WHERE LedgerID IN (" + AllLedgerIdUnderAccGroup + ") AND LedgerID NOT IN (" + strCashBankLedgers + ")";
                  dtAllLedgerExceptCashBank = Global.m_db.SelectQry(strQuery, "dt");
              }
              else//For cashflow according to Account Head-wise...it need all distinct ledgers except cashbank from transaction table
              {
                  strQuery = "SELECT DISTINCT LedgerID FROM Acc.tblTransaction WHERE LedgerID NOT IN (" + strCashBankLedgers + ")";
                  dtAllLedgerExceptCashBank = Global.m_db.SelectQry(strQuery, "dt");
              }
            }          
            if(dtAllLedgerExceptCashBank.Rows.Count>0)
            {
                return dtAllLedgerExceptCashBank;
            }
            return dtAllLedgerExceptCashBank;
        }

        public static DataTable  GetCashFlowTransactInfo(string LedgerID,DateTime FromDate, DateTime ToDate,string VoucherType,string CashBankLedgerID)
        {
            DataTable dt = new DataTable();
            string strQuery = "SELECT DISTINCT RowID FROM Acc.tblTransaction WHERE LedgerID IN ("+CashBankLedgerID +") AND "+
                               "RowID IN(SELECT RowID FROM Acc.tblTransaction WHERE VoucherType = '"+ VoucherType+"' AND "+
                              "LedgerID ='"+ LedgerID+"' AND TransactDate BETWEEN '" + Date.ToDB(FromDate) + "' AND '" + Date.ToDB(ToDate) + "')";
          
                dt = Global.m_db.SelectQry(strQuery, "CashFlow");
           
            return dt;

        }

        public static DataTable GetCashFlowTransactInfoByAccClassAndProjectID(string LedgerID, DateTime FromDate, DateTime ToDate, string VoucherType, string CashBankLedgerID,String TotalAccClassIDs,String TotalProjectIDs)
        {
            DataTable dt = new DataTable();
            string strQuery = "SELECT DISTINCT RowID FROM Acc.tblTransaction WHERE LedgerID IN (" + CashBankLedgerID + ") AND " +
                               "RowID IN(SELECT T.RowID FROM Acc.tblTransaction T,Acc.tblTransactionClass TC WHERE T.VoucherType = '" + VoucherType + "' AND " +
                              "T.LedgerID ='" + LedgerID + "' AND T.TransactDate BETWEEN '" + Date.ToDB(FromDate) + "' AND '" + Date.ToDB(ToDate) + "' and T.ProjectID in ("+(TotalProjectIDs)+") and TC.AccClassID IN ("+(TotalAccClassIDs)+") and T.TransactionID=TC.TransactionID)";

            dt = Global.m_db.SelectQry(strQuery, "CashFlow");

            return dt;

        }

        public DataTable GetAccountLedgerTransactInfo(string VoucherType, string RowID)
        {
            return Global.m_db.SelectQry("SELECT * FROM Acc.tblTransaction WHERE VoucherType='"+VoucherType+"' AND RowID='"+RowID+"'","table");
        }

        public DataTable GetDistinctLedgerID(int GroupID)
        {
            ArrayList tmpGroupID = new ArrayList();//Dyanamically allocating array type of variable tmpGroupID
            AccountGroup.GetAccountsUnder(GroupID, tmpGroupID);//Calling this function for collecting subGroupsID which fall under GroupID and storing on arrylist
            tmpGroupID.Add(GroupID);
            string GroupID1 = "";
            int i = 0;

            foreach (object j in tmpGroupID)
            {
                if (i == 0)// for first GroupID
                    GroupID1 = "'" + j.ToString() + "'";
                else  //Separating Other GroupID by commas
                    GroupID1 += "," + "'" + j.ToString() + "'";
                i++;
            }
            DataTable dt = Global.m_db.SelectQry("SELECT * FROM Acc.tblLedger WHERE GroupID IN (" + (GroupID1) + ")", "GroupID");

            if (dt.Rows.Count <= 0)
            {
                //Global.Msg("There is no Ledger information in corresponding Group ");
                throw new Exception("No ledger found!");
            }
            string LedgerID = "";

            for (int i1 = 0; i1 < dt.Rows.Count; i1++)
            {
                DataRow dr = dt.Rows[i1];
                if (i1 == 0)//for First LedgerID
                    LedgerID = "'" + (dr["LedgerID"].ToString()) + "'";
                else  //separating other LedgerID by comma

                    LedgerID += "," + "'" + (dr["LedgerID"].ToString()) + "'";

            }
            // If datatable doesnot have any value then return empty datatable
            DataTable dt1;
            //Global.Msg("SELECT DISTINCT(LedgerID) FROM Acc.tblTransaction WHERE LedgerID IN (" + (LedgerID) + ")");

            if (LedgerID.Length > 0)
                dt1 = Global.m_db.SelectQry("SELECT DISTINCT(LedgerID) FROM Acc.tblTransaction WHERE LedgerID IN (" + (LedgerID) + ")", "LedgerID");
            else
                dt1 = new DataTable();

            return dt1;
        }

        public DataTable GetAccountLedgerTransact(string LedgerID)
        {
            DataTable dtAccountLedger = new DataTable();
            dtAccountLedger.Columns.Add("LedgerID", typeof(int));
            dtAccountLedger.Columns.Add("Account_Name", typeof(string));
            dtAccountLedger.Columns.Add("Date", typeof(string));
            dtAccountLedger.Columns.Add("Type", typeof(string));
            dtAccountLedger.Columns.Add("VoucherNo", typeof(string));
            dtAccountLedger.Columns.Add("Debit_Amount", typeof(double));
            dtAccountLedger.Columns.Add("Credit_Amount", typeof(double));
            dtAccountLedger.Columns.Add("Balance", typeof(double));

            //dtAccountLedger.Columns.Add("ChequeNumber", typeof(string));
            //dtAccountLedger.Columns.Add("ChequeBank", typeof(string));
            //dtAccountLedger.Columns.Add("ChequeDate", typeof(Date));
            //Get information of corresponding LedgerID  from Acc.tblTransaction 
            Transaction m_Transaction = new Transaction();
            string getLedgerID = LedgerID;
            DataTable dt1 = m_Transaction.GetLedgerTransact(getLedgerID);//Calling this function for storing the information on datatable from acc.tblTransaction with help of LedgerID
            if (dt1.Rows.Count <= 0)//When datatable havenot value
            {
                //continue;
            }
            double Balance = 0;

            for (int j = 1; j <= dt1.Rows.Count; j++)
            {
                DataRow dr1 = dt1.Rows[j - 1];
                //Getting information of corresponding VoucherType and RowID from acc.tbleTransaction 
                DataTable dtAccountLedger1 = m_Transaction.GetAccountLedgerTransactInfo(dr1["VoucherType"].ToString(), dr1["RowID"].ToString());//Calling this function for storing the information from acc.tblTransaction by passing VoucherType and RowID as argument of function
                if (dtAccountLedger1.Rows.Count <= 0)//When datatable have a value
                {
                    continue;
                }
                for (int k = 1; k <= dtAccountLedger1.Rows.Count; k++)
                {
                    DataRow drAccountLedger = dtAccountLedger1.Rows[k - 1];

                    if (drAccountLedger["LedgerID"].ToString() == getLedgerID)
                        continue;

                    Journal m_JournalMasterDtl = new Journal();
                    //grdAccountLedger.Rows.Insert(rows);//Inserting row eachtime while increment of for loop

                    //For finding LedgerName with help of LedgerID
                    DataTable dtLedgerInfo1 = Ledger.GetLedgerInfo(Convert.ToInt32(drAccountLedger["LedgerID"]), LangMgr.DefaultLanguage);
                    DataRow drLedgerInfo1 = dtLedgerInfo1.Rows[0];

                    // Block for finding VoucherNo and Date
                    DataTable dt2 = m_JournalMasterDtl.GetJournalMasterDtl(drAccountLedger["RowID"].ToString());

                    if (dt2.Rows.Count <= 0)
                    {
                        //throw new Exception("Voucher No. not found!");
                    }
                    else
                    {
                        DataRow dr2 = dt2.Rows[0];
                        double DebBalance, CredBalance;
                        DebBalance = Convert.ToDouble(drAccountLedger["Debit_Amount"]);
                        CredBalance = Convert.ToDouble(drAccountLedger["Credit_Amount"]);
                        Balance += (DebBalance - CredBalance);
                        dtAccountLedger.Rows.Add(drLedgerInfo1["ID"],drLedgerInfo1["LedName"].ToString(), Date.DBToSystem(dr2["Journal_Date"].ToString()), drAccountLedger["VoucherType"].ToString(), dr2["Voucher_No"].ToString(), Convert.ToDouble(drAccountLedger["Debit_Amount"]), Convert.ToDouble(drAccountLedger["Credit_Amount"]), Balance.ToString());
                    }
                }
            }
            return dtAccountLedger;
        }

        public static bool CheckLedgerInTransaction(int ledgerId)
        {
            string strQuery = "SELECT * FROM Acc.tblTransaction WHERE LedgerID='" + ledgerId + "'";
            DataTable dt = Global.m_db.SelectQry(strQuery, "tbl");
            if (dt.Rows.Count > 0)
            {
                return true;
            }
            return false;

        }

        public static int CheckBuiltIn(string GroupName, Lang Language)
        {
            string LangField = "EngName";
            switch (Language)
            {
                case Lang.English:
                    LangField = "EngName";
                    break;
                case Lang.Nepali:
                    LangField = "NepName";
                    break;
            }
            GroupName = GroupName.Replace("'", "''");
            object objResult = Global.m_db.GetScalarValue("SELECT BuiltIn FROM Acc.tblLedger WHERE " + LangField + "='" + GroupName + "'");
            return Convert.ToInt32(objResult);
        }

        public static bool IsLedgerFallsInAccClass(int LedgerID,ArrayList AccClassID)
        {
            bool ReturnValue = false;
            ArrayList tmpTransactID = new ArrayList();
            string TransactID = "";
            string AccClassID1 = "";
            string QryTransactID = "SELECT TransactionID from Acc.tblTransaction WHERE LedgerID ='" + LedgerID + "'";
            DataTable dtTransactID = Global.m_db.SelectQry(QryTransactID, "tbl");//First get TransactionID according to ledgerID
            if (dtTransactID.Rows.Count > 0)
            {
                foreach (DataRow dr in dtTransactID.Rows)
                {
                    tmpTransactID.Add(Convert.ToInt32(dr["TransactionID"]));//One particular Ledger may have more than one transaction in tblTransaction soo just add it in array
                }

                int i = 0;
                foreach (object j in tmpTransactID)//split it for making suitable for query
                {
                    if (i == 0)// for first GroupID
                        TransactID = "'" + j.ToString() + "'";
                    else  //Separating Other GroupID by commas
                        TransactID += "," + "'" + j.ToString() + "'";
                    i++;
                }
            }
            //Finding the indivisual AccountclassID     
            if (AccClassID.Count > 0)
            {
                int i = 0;
                foreach (object j in AccClassID)
                {
                    if (i == 0)// for first GroupID
                        AccClassID1 = "'" + j.ToString() + "'";
                    else  //Separating Other GroupID by commas
                        AccClassID1 += "," + "'" + j.ToString() + "'";
                    i++;
                }
            }
            if(TransactID!="" && AccClassID1!="")
            {
                string strQuery = "SELECT * FROM Acc.tblTransactionClass WHERE TransactionID IN (" + (TransactID) + ") AND AccClassID IN (" + (AccClassID1) + ")";
                DataTable dt = Global.m_db.SelectQry(strQuery, "GroupID");
                if (dt.Rows.Count > 0)
                {
                    ReturnValue = true;
                }
            }
            else if (AccClassID.Count == 0)//If accountClass ID is not selected then just pass return value true because if there is no accountclass than it just ignore acountclass and continue other code
            {
                ReturnValue = true;
            
            }
            
            return ReturnValue;
        
        }

        public static DataTable GetDistinctVouTypeFromTransaction(string LedgerID)
        {
            string strQuery = "SELECT DISTINCT(VoucherType) FROM Acc.tblTransaction WHERE LedgerID ='" + LedgerID + "'";
            return Global.m_db.SelectQry(strQuery, "tbl");
        }

        public static DataTable GetLedgerTransaction(int? LedgerID, string AccClassIDsXMLString, DateTime? FromDate, DateTime? ToDate,string ProjectIDsXMLString)
        {
            #region Pass xml data to Database-
            try
            {
                Global.m_db.ClearParameter();
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("Acc.spGetLedgerTransactionNew");
                Global.m_db.AddParameter("@AccountClassIDsSettings", SqlDbType.Xml, AccClassIDsXMLString);
                Global.m_db.AddParameter("@LedgerID", SqlDbType.Int, LedgerID);
                Global.m_db.AddParameter("@FromDate", SqlDbType.DateTime, FromDate);
                Global.m_db.AddParameter("@ToDate", SqlDbType.DateTime, ToDate);
                Global.m_db.AddParameter("@ProjectIDsSettings", SqlDbType.Xml, ProjectIDsXMLString);
                DataTable dtLedgerTransactDtl = Global.m_db.GetDataTable();
                return dtLedgerTransactDtl;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            #endregion

        }

        public static DataTable GetLedgerTransactionWithChequeDetails(int? LedgerID, string AccClassIDsXMLString, DateTime? FromDate, DateTime? ToDate, int PartyID, int RecPmtID)
        {
            #region Pass xml data to Database
            try
            {
                Global.m_db.ClearParameter();
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("Acc.spGetBankReconciliationStatement");
                Global.m_db.AddParameter("@AccountClassIDsSettings", SqlDbType.Xml, AccClassIDsXMLString);
                Global.m_db.AddParameter("@LedgerID", SqlDbType.Int, LedgerID);
                Global.m_db.AddParameter("@PartyID", SqlDbType.Int, PartyID);
                Global.m_db.AddParameter("@RecPmtID", SqlDbType.Int, RecPmtID);
                Global.m_db.AddParameter("@FromDate", SqlDbType.DateTime, FromDate);
                Global.m_db.AddParameter("@ToDate", SqlDbType.DateTime, ToDate);
                DataTable dtLedgerTransactDtl = Global.m_db.GetDataTable();
                string sql = "select BankID from Acc.tblClosedBankReconciliation where BankID = " + LedgerID + " ";
                if (Global.m_db.SelectQry(sql, "Acc.tblBankReconciliationMaster").Rows.Count != 0)
                {
                    decimal DR = 0;
                    decimal CR = 0;
                    DR = BankReconciliation.GetOpeningBalUptoClosingDate(Convert.ToInt32(LedgerID));
                    if (DR < 0)
                    { 
                        CR = Math.Abs(DR);
                        DR = 0;
                    }
                    DataRow dr = dtLedgerTransactDtl.NewRow();
                    dr["LedgerDate"] =/*(Global.Default_Date.ToString() == "Nepali")?*/Date.DBToSystem(BankReconciliation.ClosingDate);//: BankReconciliation.ClosingDate;
                    dr["Account"] = "Opening balalnce  ";
                    dr["ChequeNumber"] = " ";
                    dr["LedgerID"] = "0";
                    dr["ChequeDate"] = "0";
                    dr["VoucherType"] = "0";
                    dr["RowID"] = "0";
                    dr["Debit"] = DR;
                    dr["Credit"] = CR;
                    dtLedgerTransactDtl.Rows.InsertAt(dr, 0);
                }
                return dtLedgerTransactDtl;
            }
            catch (Exception ex)
            {

                throw ex;
            }

            #endregion

        }

        public static DataTable CheckLedgerState(int ledgerId)
        {
            string strQuery = "SELECT * FROM select * from savestate WHERE LedgerID='" + ledgerId + "'";
            DataTable dt = Global.m_db.SelectQry(strQuery, "tbl");
            //if (dt.Rows.Count > 0)
            //{
            //    return true;
            //}
            //return false;
            return dt;

        }

        public static DataTable GetLedgerBalance1(DateTime? FromDate, DateTime? ToDate,  ref double DebitBalance, ref double CreditBalance, ArrayList AccClassID, int ProjectID,DataTable dtLedgerDetails)
        {
            DataTable dtShareHolderCollection = new DataTable();
            dtShareHolderCollection.Columns.Add("LedgerID");
            dtShareHolderCollection.Columns.Add("LedgerName");
            dtShareHolderCollection.Columns.Add("DebitBalance");
            dtShareHolderCollection.Columns.Add("CreditBalance");

            #region BLOCK FOR COLLECTING THE OPENING BALANCE OF LIST OF LEDGERS OF CORRESPONDING GROUPID
            foreach (DataRow drledger in dtLedgerDetails.Rows)
            {
                //string LedgerName = drledger["LedgerName"].ToString();
            //BLOCK FOR COLLECTING THE OPENING BALANCE OF LIST OF LEDGERS
            double TotalDrBal, TotalCrBal;
            TotalDrBal = TotalCrBal = 0;
            int RootID = GetRootAccClassID(AccClassID);

            DataTable dtGetOpBalance = Global.m_db.SelectQry("SELECT * FROM Acc.tblOpeningBalance WHERE LedgerID IN (" + drledger["LedgerID"].ToString() + ")  AND AccClassID='" + RootID.ToString() + "'", "table");
            for (int k = 0; k < dtGetOpBalance.Rows.Count; k++)
            {
                DataRow drGetOpBalance = dtGetOpBalance.Rows[k];
                if (!drGetOpBalance.IsNull("OpenBal"))
                {
                    if (drGetOpBalance["OpenBalDrCr"].ToString() == "DEBIT")
                        TotalDrBal += Convert.ToDouble(drGetOpBalance["OpenBal"]);
                    else if (drGetOpBalance["OpenBalDrCr"].ToString() == "CREDIT")
                        TotalCrBal += Convert.ToDouble(drGetOpBalance["OpenBal"]);
                }
            }
            #endregion

            //Convert AccClassID ArrayList to TransactionID ArrayList and Prepare for the SQL Injection
            string TransactionID = AccountClass.GetTransactIDFromAccClassID(AccClassID);

            //Selecting the all contents of tblTransaction with help of LedgerID, Date and Transaction
            string SQLTransactionID = "";
            if (TransactionID != "")//There are transaction in the selected accounting class
            {
                SQLTransactionID = " AND TransactionID IN (" + TransactionID + ")";
            }
            else if (AccClassID.Count > 0)//There are no transaction in the selected accounting class
            {
                SQLTransactionID = " AND TransactionID IS NULL";
            }

            string strFinalQuery = "";

            string ProjectSQL = "";
            ArrayList arrchildProjectIds = new ArrayList();
            Project.GetChildProjects(Convert.ToInt32(ProjectID), ref arrchildProjectIds);
            ArrayList ProjectIDCollection = new ArrayList();
            foreach (object obj in arrchildProjectIds)
            {
                int p = (int)obj;
                ProjectIDCollection.Add(p.ToString());
            }

            if (ProjectID > 0) //A Project is selected
            {
                //Collect all Project  Which parent id is given projectid
                DataTable dtproject = Global.m_db.SelectQry("select * from Acc.tblProject WHERE ParentProjectID IN (" + (ProjectID) + ")", "GroupID");
                string ProjectIDS = "";
                ProjectIDS = "'" + ProjectID + "'";
              
                for (int iproject = 0; iproject < ProjectIDCollection.Count; iproject++)
                {
                    //separating other LedgerID by comma
                    ProjectIDS += "," + "'" + (ProjectIDCollection[iproject].ToString()) + "'";
                }
                ProjectSQL = "AND ProjectID IN (" + (ProjectIDS) + ")";
                
            }
            else
            {
                ProjectSQL = "AND ProjectID = '" + ProjectID + "'";
            }
            string DateSQL = "";
            if (FromDate != null && ToDate != null)//There are both from and to dates
            {
                DateSQL = " AND TransactDate BETWEEN '" + Date.ToDB(Convert.ToDateTime(FromDate)) + "' AND '" + Date.ToDB(Convert.ToDateTime(ToDate)) + "'";
            }
            else if (FromDate != null)//FromDate is not given but ToDate is given, in this case show all transaction prior than ToDate
            {
                DateSQL = " AND TransactDate>='" + Date.ToDB(Convert.ToDateTime(FromDate)) + "'";
            }
            else if (ToDate != null)
            {
                DateSQL = " AND TransactDate<='" + Date.ToDB(Convert.ToDateTime(ToDate)) + "'";
            }
            strFinalQuery = "SELECT * FROM Acc.tblTransaction WHERE LedgerID IN (" + drledger["LedgerID"].ToString() + ")" + ProjectSQL + DateSQL + SQLTransactionID;

            DataTable dt2 = Global.m_db.SelectQry(strFinalQuery, "LedgerID");


            for (int j = 0; j < dt2.Rows.Count; j++)
            {
                DataRow dr1 = dt2.Rows[j];

                DebitBalance += Convert.ToDouble(dr1["Debit_Amount"]);

                CreditBalance += Convert.ToDouble(dr1["Credit_Amount"]);
            }
            if (TotalDrBal > TotalCrBal)
            {
                // DebitBalance += (TotalDrBal - TotalCrBal);
                DebitBalance += (TotalDrBal - CreditBalance);
                CreditBalance = 0;
            }
            else if (TotalDrBal < TotalCrBal)
            {
                // CreditBalance += (TotalCrBal - TotalDrBal);
                CreditBalance += (TotalCrBal - DebitBalance);
                DebitBalance = 0;
            }
            if (TotalDrBal == 0 && TotalCrBal == 0)
            {
                if (DebitBalance < CreditBalance)
                {
                    CreditBalance = (CreditBalance - DebitBalance);
                    DebitBalance = 0;
                }
                else
                {
                    DebitBalance = (DebitBalance - CreditBalance);
                    CreditBalance = 0;
                }
            }
            dtShareHolderCollection.Rows.Add(drledger["LedgerID"].ToString(), drledger["LedgerName"].ToString(),DebitBalance,CreditBalance);
         }//end of foreach loop
            return dtShareHolderCollection;
        }
        
        //For LEdgers Only
        //public static void GetLedgerBalance(DateTime? FromDate, DateTime? ToDate, int LedgerID, ref double DebitBalance, ref double CreditBalance, ArrayList AccClassID, int ProjectID)
        //{
        //    #region BLOCK FOR COLLECTING THE OPENING BALANCE OF LIST OF LEDGERS OF CORRESPONDING GROUPID

        //    //BLOCK FOR COLLECTING THE OPENING BALANCE OF LIST OF LEDGERS
        //    double TotalDrBal, TotalCrBal;
        //    TotalDrBal = TotalCrBal = 0;
        //    int RootID = GetRootAccClassID(AccClassID);

        //    DataTable dtGetOpBalance = Global.m_db.SelectQry("SELECT * FROM Acc.tblOpeningBalance WHERE LedgerID IN (" + LedgerID + ")  AND AccClassID='" + RootID.ToString() + "'", "table");
        //    for (int k = 0; k < dtGetOpBalance.Rows.Count; k++)
        //    {
        //        DataRow drGetOpBalance = dtGetOpBalance.Rows[k];
        //        if (!drGetOpBalance.IsNull("OpenBal"))
        //        {
        //            if (drGetOpBalance["OpenBalDrCr"].ToString() == "DEBIT")
        //                TotalDrBal += Convert.ToDouble(drGetOpBalance["OpenBal"]);
        //            else if (drGetOpBalance["OpenBalDrCr"].ToString() == "CREDIT")
        //                TotalCrBal += Convert.ToDouble(drGetOpBalance["OpenBal"]);
        //        }
        //    }
        //    #endregion

        //    //Convert AccClassID ArrayList to TransactionID ArrayList and Prepare for the SQL Injection
        //    string TransactionID = AccountClass.GetTransactIDFromAccClassID(AccClassID);

        //    //Selecting the all contents of tblTransaction with help of LedgerID, Date and Transaction
        //    string SQLTransactionID = "";
        //    if (TransactionID != "")//There are transaction in the selected accounting class
        //    {
        //        SQLTransactionID = " AND TransactionID IN (" + TransactionID + ") order by Debit_Amount";
        //    }
        //    else if (AccClassID.Count > 0)//There are no transaction in the selected accounting class
        //    {
        //        SQLTransactionID = " AND TransactionID IS NULL order by Debit_Amount";
        //    }

        //    string strFinalQuery = "";

        //    string ProjectSQL = "";
        //    ArrayList arrchildProjectIds = new ArrayList();
        //    Project.GetChildProjects(Convert.ToInt32(ProjectID), ref arrchildProjectIds);
        //    ArrayList ProjectIDCollection = new ArrayList();
        //    foreach (object obj in arrchildProjectIds)
        //    {
        //        int p = (int)obj;
        //        ProjectIDCollection.Add(p.ToString());
        //    }

        //    if (ProjectID > 0) //A Project is selected
        //    {
        //        //Collect all Project  Which parent id is given projectid
        //        DataTable dtproject = Global.m_db.SelectQry("select * from Acc.tblProject WHERE ParentProjectID IN (" + (ProjectID) + ")", "GroupID");
        //        string ProjectIDS = "";

               
        //        ProjectIDS = "'" + ProjectID + "'";
             
        //        for (int iproject = 0; iproject < ProjectIDCollection.Count; iproject++)
        //        {
                   
        //            ProjectIDS += "," + "'" + (ProjectIDCollection[iproject].ToString()) + "'";
        //        }
        //        ProjectSQL = "AND ProjectID IN (" + (ProjectIDS) + ")";
               

        //    }
        //    else
        //    {
        //        ProjectSQL = "AND ProjectID = '" + ProjectID + "'";
        //    }
        //    string DateSQL = "";
        //    if (FromDate != null && ToDate != null)//There are both from and to dates
        //    {
        //        DateSQL = " AND TransactDate BETWEEN '" + Date.ToDB(Convert.ToDateTime(FromDate)) + "' AND '" + Date.ToDB(Convert.ToDateTime(ToDate)) + "'";
        //    }
        //    else if (FromDate != null)//FromDate is not given but ToDate is given, in this case show all transaction prior than ToDate
        //    {
        //        DateSQL = " AND TransactDate>='" + Date.ToDB(Convert.ToDateTime(FromDate)) + "'";
        //    }
        //    else if (ToDate != null)
        //    {
        //        DateSQL = " AND TransactDate<='" + Date.ToDB(Convert.ToDateTime(ToDate)) + "'";
        //    }
        //    strFinalQuery = "SELECT * FROM Acc.tblTransaction WHERE LedgerID IN (" + (LedgerID) + ")" + ProjectSQL + DateSQL + SQLTransactionID;

        //    DataTable dt2 = Global.m_db.SelectQry(strFinalQuery, "LedgerID");


        //    for (int j = 0; j < dt2.Rows.Count; j++)
        //    {
        //        DataRow dr1 = dt2.Rows[j];

        //        DebitBalance += Convert.ToDouble(dr1["Debit_Amount"]);

        //        CreditBalance += Convert.ToDouble(dr1["Credit_Amount"]);
        //    }
        //    if (TotalDrBal > TotalCrBal)
        //    {
        //        // DebitBalance += (TotalDrBal - TotalCrBal);
        //        DebitBalance += (TotalDrBal - CreditBalance);
        //        CreditBalance = 0;
        //    }
        //    else if (TotalDrBal < TotalCrBal)
        //    {
        //        // CreditBalance += (TotalCrBal - TotalDrBal);
        //        CreditBalance += (TotalCrBal - DebitBalance);
        //        DebitBalance = 0;
        //    }
        //    if (TotalDrBal == 0 && TotalCrBal == 0)
        //    {
        //        if (DebitBalance < CreditBalance)
        //        {
        //            CreditBalance = (CreditBalance - DebitBalance);
        //            DebitBalance = 0;
        //        }
        //        else
        //        {
        //            DebitBalance = (DebitBalance - CreditBalance);
        //            CreditBalance = 0;
        //        }
        //    }
        //}

        public static void GetOpeningLedgerBalance(DateTime? FromDate, DateTime? ToDate, int LedgerID, ref double DebitBalance, ref double CreditBalance, ArrayList AccClassID, int ProjectID)
        {

            #region BLOCK FOR COLLECTING THE OPENING BALANCE OF LIST OF LEDGERS OF CORRESPONDING GROUPID

            //BLOCK FOR COLLECTING THE OPENING BALANCE OF LIST OF LEDGERS
            double TotalDrBal, TotalCrBal;
            TotalDrBal = TotalCrBal = 0;
            int RootID = GetRootAccClassID(AccClassID);
            DataTable dtGetOpBalance = Global.m_db.SelectQry("SELECT * FROM Acc.tblOpeningBalance WHERE LedgerID IN (" + LedgerID + ") AND AccClassID='" + RootID.ToString() + "'", "table");
            for (int k = 0; k < dtGetOpBalance.Rows.Count; k++)
            {
                DataRow drGetOpBalance = dtGetOpBalance.Rows[k];
                if (!drGetOpBalance.IsNull("OpenBal"))
                {
                    if (drGetOpBalance["OpenBalDrCr"].ToString() == "DEBIT")
                        TotalDrBal += Convert.ToDouble(drGetOpBalance["OpenBal"]);
                    else if (drGetOpBalance["OpenBalDrCr"].ToString() == "CREDIT")
                        TotalCrBal += Convert.ToDouble(drGetOpBalance["OpenBal"]);
                }
            }
            DebitBalance = TotalDrBal;
            CreditBalance = TotalCrBal;
            #endregion
        }

        public static void GetOpeningGroupBalance(DateTime? FromDate, DateTime? ToDate, int m_GroupID, bool IncludeSubgroups, ref double DebitBalance, ref double CreditBalance, ArrayList AccClassID, int ProjectID)
        {
            DataTable dt;
            if (m_GroupID != 0)
            {
                ArrayList tmpGroupID = new ArrayList();//Dyanamically allocating array type of variable tmpGroupID   
                tmpGroupID.Add(m_GroupID);// it add itself too           

                if (IncludeSubgroups == true)
                    AccountGroup.GetAccountsUnder(m_GroupID, tmpGroupID);//Calling this function for collecting subGroupsID which fall under GroupID and storing on arrylist         

                string GroupID = "";
                int i = 0;
                foreach (object j in tmpGroupID)
                {
                    if (i == 0)// for first GroupID
                        GroupID = "'" + j.ToString() + "'";
                    else  //Separating Other GroupID by commas
                        GroupID += "," + "'" + j.ToString() + "'";
                    i++;
                }


                //Collect all ledgers that fall under these groups
                dt = Global.m_db.SelectQry("SELECT * FROM Acc.tblLedger WHERE GroupID IN (" + (GroupID) + ")", "GroupID");
            }
            else
            {
                //Collect all ledgers that fall under these groups
                dt = Global.m_db.SelectQry("SELECT * FROM Acc.tblLedger", "GroupID");
            }

            if (dt.Rows.Count <= 0)
            {
                DebitBalance = 0;
                CreditBalance = 0;
                return;
            }
            string LedgerID = "";

            for (int i1 = 0; i1 < dt.Rows.Count; i1++)
            {
                DataRow dr = dt.Rows[i1];
                if (i1 == 0)//for First LedgerID
                    LedgerID = "'" + (dr["LedgerID"].ToString()) + "'";
                else  //separating other LedgerID by comma

                    LedgerID += "," + "'" + (dr["LedgerID"].ToString()) + "'";
            }


            #region BLOCK FOR COLLECTING THE OPENING BALANCE OF LIST OF LEDGERS OF CORRESPONDING GROUPID

            //BLOCK FOR COLLECTING THE OPENING BALANCE OF LIST OF LEDGERS
            double TotalDrBal, TotalCrBal;
            TotalDrBal = TotalCrBal = 0;
            int RootID = GetRootAccClassID(AccClassID);

            DataTable dtGetOpBalance = Global.m_db.SelectQry("SELECT * FROM Acc.tblOpeningBalance WHERE LedgerID IN (" + LedgerID + ")  AND AccClassID='" + RootID.ToString() + "'", "table");
            for (int k = 0; k < dtGetOpBalance.Rows.Count; k++)
            {
                DataRow drGetOpBalance = dtGetOpBalance.Rows[k];
                if (!drGetOpBalance.IsNull("OpenBal"))
                {
                    if (drGetOpBalance["OpenBalDrCr"].ToString() == "DEBIT")
                        TotalDrBal += Convert.ToDouble(drGetOpBalance["OpenBal"]);
                    else if (drGetOpBalance["OpenBalDrCr"].ToString() == "CREDIT")
                        TotalCrBal += Convert.ToDouble(drGetOpBalance["OpenBal"]);
                }
            }
            DebitBalance = TotalDrBal;
            CreditBalance = TotalCrBal;
            #endregion
        }

        public static void GetOpeningLedgerBalancePY(DateTime? FromDate, DateTime? ToDate, int LedgerID, ref double DebitBalance, ref double CreditBalance, ArrayList AccClassID, int ProjectID)
        {

            #region BLOCK FOR COLLECTING THE OPENING BALANCE OF LIST OF LEDGERS OF CORRESPONDING GROUPID

            //BLOCK FOR COLLECTING THE OPENING BALANCE OF LIST OF LEDGERS
            double TotalDrBal, TotalCrBal;
            TotalDrBal = TotalCrBal = 0;
            int RootID = GetRootAccClassID(AccClassID);
            DataTable dtGetOpBalance = Global.m_dbPY.SelectQry("SELECT * FROM Acc.tblOpeningBalance WHERE LedgerID IN (" + LedgerID + ") AND AccClassID='" + RootID.ToString() + "'", "table");
            for (int k = 0; k < dtGetOpBalance.Rows.Count; k++)
            {
                DataRow drGetOpBalance = dtGetOpBalance.Rows[k];
                if (!drGetOpBalance.IsNull("OpenBal"))
                {
                    if (drGetOpBalance["OpenBalDrCr"].ToString() == "DEBIT")
                        TotalDrBal += Convert.ToDouble(drGetOpBalance["OpenBal"]);
                    else if (drGetOpBalance["OpenBalDrCr"].ToString() == "CREDIT")
                        TotalCrBal += Convert.ToDouble(drGetOpBalance["OpenBal"]);
                }
            }
            DebitBalance = TotalDrBal;
            CreditBalance = TotalCrBal;
            #endregion
        }

        public static void GetOpeningGroupBalancePY(DateTime? FromDate, DateTime? ToDate, int m_GroupID, bool IncludeSubgroups, ref double DebitBalance, ref double CreditBalance, ArrayList AccClassID, int ProjectID)
        {
            DataTable dt;
            if (m_GroupID != 0)
            {
                ArrayList tmpGroupID = new ArrayList();//Dyanamically allocating array type of variable tmpGroupID   
                tmpGroupID.Add(m_GroupID);// it add itself too           

                if (IncludeSubgroups == true)
                    AccountGroup.GetAccountsUnder(m_GroupID, tmpGroupID);//Calling this function for collecting subGroupsID which fall under GroupID and storing on arrylist         

                string GroupID = "";
                int i = 0;
                foreach (object j in tmpGroupID)
                {
                    if (i == 0)// for first GroupID
                        GroupID = "'" + j.ToString() + "'";
                    else  //Separating Other GroupID by commas
                        GroupID += "," + "'" + j.ToString() + "'";
                    i++;
                }


                //Collect all ledgers that fall under these groups
                dt = Global.m_dbPY.SelectQry("SELECT * FROM Acc.tblLedger WHERE GroupID IN (" + (GroupID) + ")", "GroupID");
            }
            else
            {
                //Collect all ledgers that fall under these groups
                dt = Global.m_dbPY.SelectQry("SELECT * FROM Acc.tblLedger", "GroupID");
            }

            if (dt.Rows.Count <= 0)
            {
                DebitBalance = 0;
                CreditBalance = 0;
                return;
            }
            string LedgerID = "";

            for (int i1 = 0; i1 < dt.Rows.Count; i1++)
            {
                DataRow dr = dt.Rows[i1];
                if (i1 == 0)//for First LedgerID
                    LedgerID = "'" + (dr["LedgerID"].ToString()) + "'";
                else  //separating other LedgerID by comma

                    LedgerID += "," + "'" + (dr["LedgerID"].ToString()) + "'";
            }

            #region BLOCK FOR COLLECTING THE OPENING BALANCE OF LIST OF LEDGERS OF CORRESPONDING GROUPID

            //BLOCK FOR COLLECTING THE OPENING BALANCE OF LIST OF LEDGERS
            double TotalDrBal, TotalCrBal;
            TotalDrBal = TotalCrBal = 0;
            int RootID = GetRootAccClassID(AccClassID);

            DataTable dtGetOpBalance = Global.m_dbPY.SelectQry("SELECT * FROM Acc.tblOpeningBalance WHERE LedgerID IN (" + LedgerID + ")  AND AccClassID='" + RootID.ToString() + "'", "table");
            for (int k = 0; k < dtGetOpBalance.Rows.Count; k++)
            {
                DataRow drGetOpBalance = dtGetOpBalance.Rows[k];
                if (!drGetOpBalance.IsNull("OpenBal"))
                {
                    if (drGetOpBalance["OpenBalDrCr"].ToString() == "DEBIT")
                        TotalDrBal += Convert.ToDouble(drGetOpBalance["OpenBal"]);
                    else if (drGetOpBalance["OpenBalDrCr"].ToString() == "CREDIT")
                        TotalCrBal += Convert.ToDouble(drGetOpBalance["OpenBal"]);
                }
            }
            DebitBalance = TotalDrBal;
            CreditBalance = TotalCrBal;
            #endregion
        }

        public static void GetLedgerBalancePY(DateTime? FromDate, DateTime? ToDate, int LedgerID, ref double DebitBalance, ref double CreditBalance, ArrayList AccClassID, int ProjectID)
        {

            #region BLOCK FOR COLLECTING THE OPENING BALANCE OF LIST OF LEDGERS OF CORRESPONDING GROUPID

            //BLOCK FOR COLLECTING THE OPENING BALANCE OF LIST OF LEDGERS
            double TotalDrBal, TotalCrBal;
            TotalDrBal = TotalCrBal = 0;
            int RootID = GetRootAccClassID(AccClassID);
            DataTable dtGetOpBalance = Global.m_dbPY.SelectQry("SELECT * FROM Acc.tblOpeningBalance WHERE LedgerID ='" + LedgerID + "' AND AccClassID='" + RootID.ToString() + "'", "table");
            for (int k = 0; k < dtGetOpBalance.Rows.Count; k++)
            {
                DataRow drGetOpBalance = dtGetOpBalance.Rows[k];
                if (!drGetOpBalance.IsNull("OpenBal"))
                {
                    if (drGetOpBalance["OpenBalDrCr"].ToString() == "DEBIT")
                        TotalDrBal += Convert.ToDouble(drGetOpBalance["OpenBal"]);
                    else if (drGetOpBalance["OpenBalDrCr"].ToString() == "CREDIT")
                        TotalCrBal += Convert.ToDouble(drGetOpBalance["OpenBal"]);
                }
            }
            #endregion

            //Convert AccClassID ArrayList to TransactionID ArrayList and Prepare for the SQL Injection
            string TransactionID = AccountClass.GetTransactIDFromAccClassID(AccClassID);

            //Selecting the all contents of tblTransaction with help of LedgerID
            // formate in this way SELECT * FROM Acc.tblTransaction WHERE LedgerID IN ('41','40','32','36','33')
            //string sql = "SELECT * FROM Acc.tblTransaction WHERE LedgerID IN (" + (LedgerID) + ") AND TransactDate BETWEEN '" + FromDate + "' AND '" + ToDate + "'";
            string SQLTransactionID = "";
            if (TransactionID != "")
            {
                SQLTransactionID = " AND TransactionID IN (" + TransactionID + ")";
            }
            else if (AccClassID.Count > 0)//There are no transaction in the selected accounting class
            {
                SQLTransactionID = " AND TransactionID IS NULL";
            }

            string strFinalQuery = "";


            string ProjectSQL = "";
            ArrayList arrchildProjectIds = new ArrayList();
            Project.GetChildProjects(Convert.ToInt32(ProjectID), ref arrchildProjectIds);
            ArrayList ProjectIDCollection = new ArrayList();
            foreach (object obj in arrchildProjectIds)
            {
                int p = (int)obj;
                ProjectIDCollection.Add(p.ToString());
            }

            if (ProjectID > 0) //A Project is selected
            {
                //Collect all Project  Which parent id is given projectid
                DataTable dtproject = Global.m_dbPY.SelectQry("select * from Acc.tblProject WHERE ParentProjectID IN (" + (ProjectID) + ")", "GroupID");
                string ProjectIDS = "";

                ProjectIDS = "'" + ProjectID + "'";

                for (int iproject = 0; iproject < ProjectIDCollection.Count; iproject++)
                {
                    ProjectIDS += "," + "'" + (ProjectIDCollection[iproject].ToString()) + "'";
                }
                ProjectSQL = "AND ProjectID IN (" + (ProjectIDS) + ")";
                // }

            }
            else
            {
                ProjectSQL = "AND ProjectID = '" + ProjectID + "'";
            }
            //if (ProjectID > 0) //A Project is selected
            //{
            //    //Collect all Project  Which parent id is given projectid
            //    DataTable dtproject = Global.m_db.SelectQry("select * from Acc.tblProject WHERE ParentProjectID IN (" + (ProjectID) + ")", "GroupID");
            //    string ProjectIDS = "";

            //    //if (dtproject.Rows.Count < 1)
            //    //{
            //    //    ProjectSQL = " AND ProjectID ='" + ProjectID + "'";
            //    //}
            //    //else
            //    //{
            //    // LedgerID = "'" + (dr["LedgerID"].ToString()) + "'";
            //    ProjectIDS = "'" + ProjectID + "'";
            //    for (int iproject = 0; iproject < dtproject.Rows.Count; iproject++)
            //    {
            //        DataRow dr = dtproject.Rows[iproject];
            //        //separating other LedgerID by comma
            //        ProjectIDS += "," + "'" + (dr["ProjectID"].ToString()) + "'";
            //    }
            //    ProjectSQL = "AND ProjectID IN (" + (ProjectIDS) + ")";
            //    // }

            //}

            string DateSQL = "";
            if (FromDate != null && ToDate != null)//There are both from and to dates
            {
                DateSQL = " AND TransactDate BETWEEN '" + Date.ToDB(Convert.ToDateTime(FromDate)) + "' AND '" + Date.ToDB(Convert.ToDateTime(ToDate)) + "'";
            }
            else if (FromDate != null)//FromDate is not given but ToDate is given, in this case show all transaction prior than ToDate
            {
                DateSQL = " AND TransactDate>='" + Date.ToDB(Convert.ToDateTime(FromDate)) + "'";
            }
            else if (ToDate != null)
            {
                DateSQL = " AND TransactDate<='" + Date.ToDB(Convert.ToDateTime(ToDate)) + "'";
            }

            strFinalQuery = "SELECT * FROM Acc.tblTransaction WHERE  LedgerID='" + LedgerID + "'" + ProjectSQL + DateSQL + SQLTransactionID;
            DataTable dt2 = Global.m_dbPY.SelectQry(strFinalQuery, "LedgerID");
            //DataTable dt1 = Global.m_db.SelectQry("SELECT * FROM Acc.tblTransaction WHERE LedgerID IN (" + (LedgerID) + ")", "LedgerID");

            for (int j = 0; j < dt2.Rows.Count; j++)
            {
                DataRow dr1 = dt2.Rows[j];
                DebitBalance += Convert.ToDouble(dr1["Debit_Amount"]);
                CreditBalance += Convert.ToDouble(dr1["Credit_Amount"]);
            }

            // here make comment
            if (TotalDrBal > TotalCrBal)
                DebitBalance += (TotalDrBal - TotalCrBal);
            else if (TotalDrBal < TotalCrBal)
                CreditBalance += (TotalCrBal - TotalDrBal);
        }

        public static void GetGroupBalancePY(DateTime? FromDate, DateTime? ToDate, int m_GroupID, bool IncludeSubgroups, ref double DebitBalance, ref double CreditBalance, ArrayList AccClassID, int ProjectID)
        {
            DataTable dt;
            if (m_GroupID != 0)
            {
                ArrayList tmpGroupID = new ArrayList();//Dyanamically allocating array type of variable tmpGroupID   
                tmpGroupID.Add(m_GroupID);// it add itself too           

                if (IncludeSubgroups == true)
                    AccountGroup.GetAccountsUnder(m_GroupID, tmpGroupID);//Calling this function for collecting subGroupsID which fall under GroupID and storing on arrylist         

                string GroupID = "";
                int i = 0;
                foreach (object j in tmpGroupID)
                {
                    if (i == 0)// for first GroupID
                        GroupID = "'" + j.ToString() + "'";
                    else  //Separating Other GroupID by commas
                        GroupID += "," + "'" + j.ToString() + "'";
                    i++;
                }

                //Collect all ledgers that fall under these groups
                dt = Global.m_dbPY.SelectQry("SELECT * FROM Acc.tblLedger WHERE GroupID IN (" + (GroupID) + ")", "GroupID");
            }
            else
            {
                //Collect all ledgers that fall under these groups
                dt = Global.m_dbPY.SelectQry("SELECT * FROM Acc.tblLedger", "GroupID");
            }

            if (dt.Rows.Count <= 0)
            {
                DebitBalance = 0;
                CreditBalance = 0;
                return;
            }
            string LedgerID = "";

            for (int i1 = 0; i1 < dt.Rows.Count; i1++)
            {
                DataRow dr = dt.Rows[i1];
                if (i1 == 0)//for First LedgerID
                    LedgerID = "'" + (dr["LedgerID"].ToString()) + "'";
                else  //separating other LedgerID by comma

                    LedgerID += "," + "'" + (dr["LedgerID"].ToString()) + "'";
            }


            #region BLOCK FOR COLLECTING THE OPENING BALANCE OF LIST OF LEDGERS OF CORRESPONDING GROUPID

            //BLOCK FOR COLLECTING THE OPENING BALANCE OF LIST OF LEDGERS
            double TotalDrBal, TotalCrBal;
            TotalDrBal = TotalCrBal = 0;
            int RootID = GetRootAccClassID(AccClassID);

            DataTable dtGetOpBalance = Global.m_dbPY.SelectQry("SELECT * FROM Acc.tblOpeningBalance WHERE LedgerID IN (" + LedgerID + ")  AND AccClassID='" + RootID.ToString() + "'", "table");
            for (int k = 0; k < dtGetOpBalance.Rows.Count; k++)
            {
                DataRow drGetOpBalance = dtGetOpBalance.Rows[k];
                if (!drGetOpBalance.IsNull("OpenBal"))
                {
                    if (drGetOpBalance["OpenBalDrCr"].ToString() == "DEBIT")
                        TotalDrBal += Convert.ToDouble(drGetOpBalance["OpenBal"]);
                    else if (drGetOpBalance["OpenBalDrCr"].ToString() == "CREDIT")
                        TotalCrBal += Convert.ToDouble(drGetOpBalance["OpenBal"]);
                }
            }
            #endregion

            //Convert AccClassID ArrayList to TransactionID ArrayList and Prepare for the SQL Injection
            string TransactionID = AccountClass.GetTransactIDFromAccClassID(AccClassID);

            //Selecting the all contents of tblTransaction with help of LedgerID, Date and Transaction
            string SQLTransactionID = "";
            if (TransactionID != "")//There are transaction in the selected accounting class
            {
                SQLTransactionID = " AND TransactionID IN (" + TransactionID + ") order by Debit_Amount";
            }
            else if (AccClassID.Count > 0)//There are no transaction in the selected accounting class
            {
                SQLTransactionID = " AND TransactionID IS NULL order by Debit_Amount";
            }

            string strFinalQuery = "";

            string ProjectSQL = "";
            ArrayList arrchildProjectIds = new ArrayList();
            Project.GetChildProjects(Convert.ToInt32(ProjectID), ref arrchildProjectIds);
            ArrayList ProjectIDCollection = new ArrayList();
            foreach (object obj in arrchildProjectIds)
            {
                int p = (int)obj;
                ProjectIDCollection.Add(p.ToString());
            }

            if (ProjectID > 0) //A Project is selected
            {
                //Collect all Project  Which parent id is given projectid
                DataTable dtproject = Global.m_dbPY.SelectQry("select * from Acc.tblProject WHERE ParentProjectID IN (" + (ProjectID) + ")", "GroupID");
                string ProjectIDS = "";

                //if (dtproject.Rows.Count < 1)
                //{
                //    ProjectSQL = " AND ProjectID ='" + ProjectID + "'";
                //}
                //else
                //{
                // LedgerID = "'" + (dr["LedgerID"].ToString()) + "'";
                ProjectIDS = "'" + ProjectID + "'";
                //for (int iproject = 0; iproject < dtproject.Rows.Count; iproject++)
                //{
                //    DataRow dr = dtproject.Rows[iproject];
                //    //separating other LedgerID by comma
                //    ProjectIDS += "," + "'" + (dr["ProjectID"].ToString()) + "'";
                //}
                for (int iproject = 0; iproject < ProjectIDCollection.Count; iproject++)
                {
                    //DataRow dr = dtproject.Rows[iproject];
                    //separating other LedgerID by comma
                    ProjectIDS += "," + "'" + (ProjectIDCollection[iproject].ToString()) + "'";
                }
                ProjectSQL = "AND ProjectID IN (" + (ProjectIDS) + ")";
                // }

            }
            else
            {
                ProjectSQL = "AND ProjectID = '" + ProjectID + "'";
            }
            string DateSQL = "";
            if (Global.PYFromDate != null && Global.PYToDate != null)//There are both from and to dates
            {
                DateSQL = " AND TransactDate BETWEEN '" + Date.ToDB(Convert.ToDateTime(Global.PYFromDate)) + "' AND '" + Date.ToDB(Convert.ToDateTime(Global.PYToDate)) + "'";
            }
            else if (Global.PYFromDate != null)//FromDate is not given but ToDate is given, in this case show all transaction prior than ToDate
            {
                DateSQL = " AND TransactDate>='" + Date.ToDB(Convert.ToDateTime(Global.PYFromDate)) + "'";
            }
            else if (Global.PYToDate != null)
            {
                DateSQL = " AND TransactDate<='" + Date.ToDB(Convert.ToDateTime(Global.PYToDate)) + "'";
            }
            strFinalQuery = "SELECT * FROM Acc.tblTransaction WHERE LedgerID IN (" + (LedgerID) + ")" + ProjectSQL + DateSQL + SQLTransactionID;

            DataTable dt2 = Global.m_dbPY.SelectQry(strFinalQuery, "LedgerID");


            for (int j = 0; j < dt2.Rows.Count; j++)
            {
                DataRow dr1 = dt2.Rows[j];

                DebitBalance += Convert.ToDouble(dr1["Debit_Amount"]);

                CreditBalance += Convert.ToDouble(dr1["Credit_Amount"]);
            }
            if (TotalDrBal > TotalCrBal)
            {
                // DebitBalance += (TotalDrBal - TotalCrBal);
                DebitBalance += (TotalDrBal - CreditBalance);
                CreditBalance = 0;
            }
            else if (TotalDrBal < TotalCrBal)
            {
                // CreditBalance += (TotalCrBal - TotalDrBal);
                CreditBalance += (TotalCrBal - DebitBalance);
                DebitBalance = 0;
            }
            if (TotalDrBal == 0 && TotalCrBal == 0)
            {
                if (DebitBalance < CreditBalance)
                {
                    CreditBalance = (CreditBalance - DebitBalance);
                    DebitBalance = 0;
                }
                else
                {
                    DebitBalance = (DebitBalance - CreditBalance);
                    CreditBalance = 0;
                }
            }
        }

        public static DataTable GetAccountLedgerReportData(DateTime? FromDate, DateTime? ToDate,int AccountID, string AccClassIDsXMLString, string ProjectIDsXMLString)
        {
            try
            {


                Global.m_db.ClearParameter();
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("Acc.spGetLedgerDetails");
                Global.m_db.AddParameter("@Transaction_Start_Date", SqlDbType.Date,FromDate);
                Global.m_db.AddParameter("@Transaction_End_Date", SqlDbType.Date,ToDate);
                Global.m_db.AddParameter("@LedgerID", SqlDbType.Int, AccountID);

                Global.m_db.AddParameter("@AccountClassIDsSettings", SqlDbType.Xml, AccClassIDsXMLString);
                Global.m_db.AddParameter("@ProjectIDsSettings", SqlDbType.Xml, ProjectIDsXMLString);
                Global.m_db.AddParameter("@Settings", SqlDbType.Xml,null);
                return Global.m_db.GetDataTable();

            }
            catch(Exception ex)
            {
                Global.Msg(ex.Message);
                return null;
            }

        }

       
    }
}
