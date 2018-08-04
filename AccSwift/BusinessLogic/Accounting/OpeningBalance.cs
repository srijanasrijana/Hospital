using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DBLogic;
using System.Windows.Forms;
using System.Data;
using DateManager;

namespace BusinessLogic
{
    public class OpeningBalance
    {

        /// <summary>
        /// Gets the opening balances of the given or all ledger ID
        /// </summary>
        /// <param name="AccountClassID"></param>
        /// <param name="LedgerID"></param>
        /// <returns></returns>
        public static DataTable GetAccClassOpeningBalance(int AccountClassID, int? LedgerID)
        {
            if (LedgerID == -1)//Dont care ledger, list all Opening Balances
            {
                return Global.m_db.SelectQry("SELECT * FROM Acc.tblOpeningBalance WHERE AccClassID =" + AccountClassID.ToString(), "tblOpeningBalance");
            }
            return Global.m_db.SelectQry("SELECT * FROM Acc.tblOpeningBalance WHERE AccClassID =" + AccountClassID.ToString() + " and LedgerID =" + LedgerID.ToString(), "tblOpeningBalance");
        }

        public static DataTable GetAccClassOpeningQuantity(int AccountClassID, int? ProductID)
        {
            if (ProductID == -1)//Dont care ledger, list all Opening Balances
            {
                return Global.m_db.SelectQry("SELECT * FROM Inv.tblOpeningQuantity WHERE AccClassID =" + AccountClassID.ToString(), "tblOpeningQuantity");
            }
            return Global.m_db.SelectQry("SELECT * FROM Inv.tblOpeningQuantity WHERE AccClassID =" + AccountClassID.ToString() + " and ProductID =" + ProductID.ToString(), "tblOpeningQuantity");
        }

        public static DataTable GetAccClassPreYearBalance(int AccountClassID, int? LedgerID)
        {
            if (LedgerID == -1)//Dont care ledger, list all Opening Balances
            {
                return Global.m_db.SelectQry("SELECT * FROM Acc.tblPreYearBalance WHERE AccClassID =" + AccountClassID.ToString(), "tblPreYearBalance");
            }
            return Global.m_db.SelectQry("SELECT * FROM Acc.tblPreYearBalance WHERE AccClassID =" + AccountClassID.ToString() + " and LedgerID =" + LedgerID.ToString(), "tblPreYearBalance");
        }


        public static int GetAccClassIDFromName(string AccountClass, Lang Language)
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
            object objResult = Global.m_db.GetScalarValue("SELECT AccClassID FROM Acc.tblAccClass WHERE " + LangField + "='" + AccountClass + "'");
            return Convert.ToInt32(objResult);
        }

        //public static string InsertAccountOpeningBalance(int LedgerID, int AccountClassID, string OpeningAmount)
        //{
        //    try
        //    {
        //        Global.m_db.ClearParameter();
        //        Global.m_db.setCommandType(CommandType.StoredProcedure);
        //        Global.m_db.setCommandText("Acc.spOpeningBalanceCreate");              
        //        Global.m_db.AddParameter("@LedgerID", SqlDbType.Int, 80,LedgerID);
        //        Global.m_db.AddParameter("@AccClassID", SqlDbType.Int, 80, AccountClassID);
        //        Global.m_db.AddParameter("@OpeningBalance", SqlDbType.NVarChar, 80, OpeningAmount);              

        //        System.Data.SqlClient.SqlParameter paramReturn = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
        //        Global.m_db.ProcessParameter();
        //        if (paramReturn.Value.ToString() == "SUCCESS")
        //            return "SUCCESS";
        //        else
        //            return "FAILURE";
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public static string InsertAccountOpeningBalance(int LedgerID, DataTable dtAllOpeningBalance,DateTime? dueDate=null)
        {

            try
            {
               //  Global.m_db.BeginTransaction();

                //Clear all Opening Balance
                Global.m_db.SelectQry("DELETE FROM Acc.tblOpeningBalance WHERE LedgerID='" + LedgerID.ToString() + "'", "op bal");
                for (int i = 0; i < dtAllOpeningBalance.Rows.Count; i++)
                {
                    DataRow dr = dtAllOpeningBalance.Rows[i];

                    Global.m_db.ClearParameter();
                    Global.m_db.setCommandType(CommandType.StoredProcedure);
                    Global.m_db.setCommandText("Acc.spOpeningBalanceCreate");
                    Global.m_db.AddParameter("@LedgerID", SqlDbType.Int, LedgerID);
                    Global.m_db.AddParameter("@AccClassID", SqlDbType.Int, dr["AccClassID"]);
                    Global.m_db.AddParameter("@DrCr", SqlDbType.NVarChar, 10, dr["DrCr"]);
                   // Global.m_db.AddParameter("@OpenBalDate", SqlDbType.DateTime, );
                    Global.m_db.AddParameter("@OpBalCCY", SqlDbType.NVarChar, 10, 1);
                    Global.m_db.AddParameter("@OpeningBalance", SqlDbType.NVarChar, 80, dr["OpeningBalance"]);

                    System.Data.SqlClient.SqlParameter paramReturn = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                    Global.m_db.ProcessParameter();

                }
                   if (dueDate != null)
                   {
                	Global.m_db.InsertUpdateQry("DELETE FROM System.tblDueDate WHERE LedgerID='"+LedgerID+"' AND VoucherType='OPBAL'");
                    Global.m_db.InsertUpdateQry("INSERT INTO System.tblDueDate(DueDate,LedgerID,VoucherType) VALUES('" + dueDate + "','" + LedgerID + "','OPBAL')");
                   }
               // Global.m_db.CommitTransaction();
                //if (paramReturn.Value.ToString() == "SUCCESS")
                return "SUCCESS";
                //else
                //    return "FAILURE";
            }
            catch (Exception ex)
            {
              //  Global.m_db.RollBackTransaction();
                throw ex;
            }
        }

        public static string InsertProductOpeningQuantity(int ProductID, DataTable dtProductQuantity)
        {

            try
            {
                //Clear all Opening Balance
                Global.m_db.SelectQry("DELETE FROM Inv.tblOpeningQuantity WHERE ProductID='" + ProductID.ToString() + "'", "op Qty");
                for (int i = 0; i < dtProductQuantity.Rows.Count; i++)
                {
                    DataRow dr = dtProductQuantity.Rows[i];

                    Global.m_db.ClearParameter();
                    Global.m_db.setCommandType(CommandType.StoredProcedure);
                    Global.m_db.setCommandText("Inv.spOpeningQuantityCreate");
                    Global.m_db.AddParameter("@ProductID", SqlDbType.Int, ProductID);
                    Global.m_db.AddParameter("@AccClassID", SqlDbType.Int, dr["AccClassID"]);
                    Global.m_db.AddParameter("@OpenPurchaseQuantity", SqlDbType.Int, dr["PurchaseQuantity"]);
                    Global.m_db.AddParameter("@OpenPurchaseRate", SqlDbType.Float,  dr["PurchaseRate"]);
                    Global.m_db.AddParameter("@OpenSaleRate", SqlDbType.Float,  dr["SaleRate"]);
                   
                    System.Data.SqlClient.SqlParameter paramReturn = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                    Global.m_db.ProcessParameter();
                }
                //if (paramReturn.Value.ToString() == "SUCCESS")
                return "SUCCESS";
                //else
                //    return "FAILURE";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string InsertAccountPreYearBalance(int LedgerID, DataTable dtAllPreYearBalance)
        {

            try
            {
                //Clear all Opening Balance
                Global.m_db.SelectQry("DELETE FROM Acc.tblPreYearBalance WHERE LedgerID='" + LedgerID.ToString() + "'", "op bal");
                for (int i = 0; i < dtAllPreYearBalance.Rows.Count; i++)
                {
                    DataRow dr = dtAllPreYearBalance.Rows[i];

                    Global.m_db.ClearParameter();
                    Global.m_db.setCommandType(CommandType.StoredProcedure);
                    Global.m_db.setCommandText("Acc.spPreYearBalanceCreate");
                    Global.m_db.AddParameter("@LedgerID", SqlDbType.Int, LedgerID);
                    Global.m_db.AddParameter("@AccClassID", SqlDbType.Int, dr["AccClassID"]);
                    Global.m_db.AddParameter("@DrCr", SqlDbType.NVarChar, 10, dr["DrCr"]);

                    Global.m_db.AddParameter("@PreYearBalCCY", SqlDbType.NVarChar, 10, 1);
                    Global.m_db.AddParameter("@PreYearBalance", SqlDbType.NVarChar, 80, dr["PreYearBalance"]);

                    System.Data.SqlClient.SqlParameter paramReturn = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                    Global.m_db.ProcessParameter();
                }
                //if (paramReturn.Value.ToString() == "SUCCESS")
                return "SUCCESS";
                //else
                //    return "FAILURE";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
