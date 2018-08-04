using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Collections;
using DateManager;


namespace BusinessLogic
{

   public interface IDebitNote
    {
        void Create(int SeriesID,int LedgerID, string VoucherNo, DateTime DebitNoteDate, string Remarks, DataTable DebitNoteDetails,int[] AccClassID,int ProjeactID,OptionalField of);
        void Modify(int DebitNoteID, int SeriesID,int LedgerID, string VoucherNo, DateTime DebitNoteDate, string Remarks, DataTable DebitNoteDetails,int[] AccClassID,int ProjectID,OptionalField of);
        ////void Delete(string AccountHeadName);
        DataTable NavigateDebitNoteMaster(int CurrentID, Navigate NavTo);
        DataTable GetDebitNoteDetail(int DebitNoteID);
    }
   public  class DebitNote : IDebitNote
    {
       public void Create(int SeriesID,int LedgerID,string VoucherNo,DateTime DebitNoteDate, string Remarks, DataTable DebitNoteDetails, int[] AccClassID,int ProjectID,OptionalField of)
       {
           #region Language Mgmt
           string Language = "";
           switch (LangMgr.DefaultLanguage)
           {
               case Lang.English:
                   Language = "ENGLISH";
                   break;
               case Lang.Nepali:
                   Language = "NEPALI";
                   break;
           }

           #endregion
           //Check if the DebitNoteDetails has fields
           if (DebitNoteDetails.Rows.Count == 0)
           {
               throw new Exception("Please fill the ledger details");
               return;
           }
           double TotalAmount = 0;
           ArrayList Debit = new ArrayList();
           ArrayList Credit = new ArrayList();
           //This loop is to check whether ledger names are correct and properly implemented
           foreach (DataRow row in DebitNoteDetails.Rows)
           {
               //Check whether the ledger name are correct
               Global.m_db.ClearParameter();
               Global.m_db.setCommandType(CommandType.StoredProcedure);
               Global.m_db.setCommandText("Acc.spGetLedgerIDFromName");
               Global.m_db.AddParameter("@LedgerName", SqlDbType.NVarChar, 50, row["Ledger"].ToString());
               Global.m_db.AddParameter("@Language", SqlDbType.NVarChar, 20, Language);
               System.Data.SqlClient.SqlParameter objReturn = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
               Global.m_db.ProcessParameter();

               if (objReturn.Value.ToString() == "-100")//It return -100 in case of failure
                   throw new Exception("Ledger Name - " + row["Ledger"].ToString() + " not found!");
           }
           double Amount = 0;
           try
           {
               // Global.m_db.InsertUpdateQry("INSERT INTO Account.tblAccountHead(AccountHeadName,Under_Account_ID,Description,BuiltIn) VALUES ('" + AccountHeadName + "','1','" + Description + "','0')");
               Global.m_db.BeginTransaction();
               Global.m_db.ClearParameter();
               Global.m_db.setCommandType(CommandType.StoredProcedure);
               Global.m_db.setCommandText("Acc.spDebitNoteCreate");
               Global.m_db.AddParameter("@SeriesID", SqlDbType.Int, SeriesID);
               Global.m_db.AddParameter("@LedgerID", SqlDbType.Int, LedgerID);
               Global.m_db.AddParameter("@Voucher_No", SqlDbType.NVarChar, 30, VoucherNo);//Set same for both for time being
               Global.m_db.AddParameter("@DebitNote_Date", SqlDbType.DateTime, DebitNoteDate);
               Global.m_db.AddParameter("@Remarks", SqlDbType.NVarChar, 200, Remarks);
               Global.m_db.AddParameter("@ProjectID", SqlDbType.NVarChar, 200, ProjectID);
               Global.m_db.AddParameter("@Created_By", SqlDbType.NVarChar, 50, User.CurrUserID.ToString());
               Global.m_db.AddParameter("@First", SqlDbType.NVarChar, 50, of.First);
               Global.m_db.AddParameter("@Second", SqlDbType.NVarChar, 50, of.Second);
               Global.m_db.AddParameter("@Third", SqlDbType.NVarChar, 50, of.Third);
               Global.m_db.AddParameter("@Fourth", SqlDbType.NVarChar, 50, of.Fourth);
               Global.m_db.AddParameter("@Fifth", SqlDbType.NVarChar, 50, of.Fifth);
               System.Data.SqlClient.SqlParameter objReturn = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
               Global.m_db.ProcessParameter();
               int ReturnID = Convert.ToInt32(objReturn.Value);
               for (int i = 0; i < DebitNoteDetails.Rows.Count; i++)
               {
                   DataRow dr = DebitNoteDetails.Rows[i];
                   //Now go for the Detail Inserts
                   Global.m_db.ClearParameter();
                   Global.m_db.setCommandType(CommandType.StoredProcedure);
                   Global.m_db.setCommandText("Acc.spDebitNoteDetailCreate");
                   Global.m_db.AddParameter("@DebitNoteID", SqlDbType.Int, ReturnID.ToString());
                   Global.m_db.AddParameter("@LedgerName", SqlDbType.NVarChar, 30, dr["Ledger"].ToString());//Set same for both for time being
                   Global.m_db.AddParameter("@Language", SqlDbType.NVarChar, 30, Language);
                   Global.m_db.AddParameter("@Amount", SqlDbType.Money, Convert.ToDouble(dr["Amount"]));
                   Amount += Convert.ToDouble(dr["Amount"]);
                   Global.m_db.AddParameter("@Remarks", SqlDbType.NVarChar, 200, dr["Remarks"].ToString());
                   System.Data.SqlClient.SqlParameter paramReturn = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                   Global.m_db.ProcessParameter();
                   if (paramReturn.Value.ToString() != "SUCCESS")
                   {
                       Global.m_db.RollBackTransaction();
                       throw new Exception("Unable to create Debit Note");
                   }
                   //Also insert the transaction in tbltransaction
                   Global.m_db.ClearParameter();
                   Global.m_db.setCommandType(CommandType.StoredProcedure);
                   Global.m_db.setCommandText("Acc.spTransactCreate");
                   Global.m_db.AddParameter("@Date", SqlDbType.DateTime, DebitNoteDate);
                   Global.m_db.AddParameter("@LedgerName", SqlDbType.NVarChar, 30, dr["Ledger"].ToString());//Set same for both for time being
                   Global.m_db.AddParameter("@Language", SqlDbType.NVarChar, 30, Language);
                   Global.m_db.AddParameter("@Amount", SqlDbType.Money, Convert.ToDouble(dr["Amount"]));
                   TotalAmount += Convert.ToDouble(dr["Amount"]);
                   Global.m_db.AddParameter("@DrCr", SqlDbType.NVarChar, 10, "CREDIT");
                   Global.m_db.AddParameter("@VoucherType", SqlDbType.NVarChar, 20, "DR_NOT");
                   Global.m_db.AddParameter("@RowID", SqlDbType.Int, ReturnID);
                   Global.m_db.AddParameter("@ProjectID", SqlDbType.Int, ProjectID);
                   paramReturn = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                   Global.m_db.ProcessParameter();
                   int ReturnTransactID = Convert.ToInt32(paramReturn.Value);
                   if (paramReturn.Value.ToString() == "FAILURE")
                   {
                       throw new Exception("Unable to create Debit Note");
                   }
                   ////Add the information  in Acc.tblTransactionClass
                   foreach (int _AccClassID in AccClassID)
                   {
                       Global.m_db.ClearParameter();
                       Global.m_db.setCommandType(CommandType.StoredProcedure);
                       Global.m_db.setCommandText("Acc.spTransactClassCreate");
                       Global.m_db.AddParameter("@TransactID", SqlDbType.Int, ReturnTransactID.ToString());
                       Global.m_db.AddParameter("@AccClassID", SqlDbType.Int, _AccClassID);//Set same for both for time being
                       Global.m_db.AddParameter("@RowID", SqlDbType.Int, ReturnID.ToString());
                       Global.m_db.AddParameter("VoucherType", SqlDbType.NVarChar, 20, "DR_NOT");
                       System.Data.SqlClient.SqlParameter paramTransactClassID = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                       Global.m_db.ProcessParameter();

                       if (paramTransactClassID.Value.ToString() == "FAILURE")
                       {
                           Global.m_db.RollBackTransaction();
                           throw new Exception("Unable to create Debit Note");
                       }
                   }
               }
               string LedgerName = Ledger.GetLedgerNameFromID(LedgerID);
               //Also insert the transaction in tbltransaction
               Global.m_db.ClearParameter();
               Global.m_db.setCommandType(CommandType.StoredProcedure);
               Global.m_db.setCommandText("Acc.spTransactCreate");
               Global.m_db.AddParameter("@Date", SqlDbType.DateTime, DebitNoteDate);
               //Global.m_db.AddParameter("@LedgerID", SqlDbType.Int, LedgerID);//Set same for both for time being
               Global.m_db.AddParameter("@LedgerName", SqlDbType.NVarChar, 30, LedgerName);
               Global.m_db.AddParameter("@Language", SqlDbType.NVarChar, 30, Language);
               Global.m_db.AddParameter("@Amount", SqlDbType.Money, TotalAmount);
               Global.m_db.AddParameter("@DrCr", SqlDbType.NVarChar, 10, "DEBIT");
               Global.m_db.AddParameter("@VoucherType", SqlDbType.NVarChar, 20, "DR_NOT");
               Global.m_db.AddParameter("@RowID", SqlDbType.Int, ReturnID);
               Global.m_db.AddParameter("@ProjectID", SqlDbType.Int, ProjectID);
               System.Data.SqlClient.SqlParameter paramReturn1 = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
               Global.m_db.ProcessParameter();

               int ReturnTransactID1 = Convert.ToInt32(paramReturn1.Value);

               if (paramReturn1.Value.ToString() == "FAILURE")
               {
                   throw new Exception("Unable to create Debit Note");
               }

               //Add the information  in Acc.tblTransactionClass
               foreach (int _AccClassID in AccClassID)
               {
                   Global.m_db.ClearParameter();
                   Global.m_db.setCommandType(CommandType.StoredProcedure);
                   Global.m_db.setCommandText("Acc.spTransactClassCreate");
                   Global.m_db.AddParameter("@TransactID", SqlDbType.Int, ReturnTransactID1.ToString());
                   Global.m_db.AddParameter("@AccClassID", SqlDbType.Int, _AccClassID);//Set same for both for time being
                   Global.m_db.AddParameter("@RowID", SqlDbType.Int, ReturnID.ToString());
                   Global.m_db.AddParameter("VoucherType", SqlDbType.NVarChar, 20, "DR_NOT");
                   System.Data.SqlClient.SqlParameter paramTransactClassID = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                   Global.m_db.ProcessParameter();

                   if (paramTransactClassID.Value.ToString() == "FAILURE")
                   {
                       Global.m_db.RollBackTransaction();
                       throw new Exception("Unable to create Debit Note");
                   }
               }

               Global.m_db.CommitTransaction();
           }
           catch (Exception ex)
           {
               Global.m_db.RollBackTransaction();

               throw ex;

               #region SQLException
               //switch (ex.Number)
               //{
               //    case 4060: // Invalid Database 
               //        Global.Msg("Invalid Database", MBType.Error, "Error");
               //        break;

               //    case 18456: // Login Failed 
               //        Global.Msg("Login Failed!", MBType.Error, "Error");
               //        break;

               //    case 547: // ForeignKey Violation , Check Constraint
               //        Global.Msg("Invalid parent group! Check the parent group and try again!", MBType.Warning, "Error");
               //        break;

               //    case 2627: // Unique Index/ Primary key Violation/ Constriant Violation 
               //        Global.Msg("The group name already exists! Please choose another group names!", MBType.Warning, "Error");
               //        break;

               //    case 2601: // Unique Index/Constriant Violation 
               //        Global.Msg("Unique index violation!", MBType.Warning, "Error");
               //        break;

               //    case 5000: //Trigger violation
               //        Global.Msg("Trigger violation!", MBType.Warning, "Error");
               //        break;

               //    default:
               //        break;
               //}
               #endregion
           }
       }

       public void Modify(int DebitNoteID, int SeriesID,int LedgerID, string VoucherNo, DateTime DebitNoteDate, string Remarks, DataTable DebitNoteDetails,int[] AccClassID,int ProjectID,OptionalField of)
       {
           try
           {
               // Global.m_db.InsertUpdateQry("INSERT INTO Account.tblAccountHead(AccountHeadName,Under_Account_ID,Description,BuiltIn) VALUES ('" + AccountHeadName + "','1','" + Description + "','0')");
               Global.m_db.BeginTransaction();
               Global.m_db.ClearParameter();
               Global.m_db.setCommandType(CommandType.StoredProcedure);
               Global.m_db.setCommandText("Acc.spDebitNoteModify");
               Global.m_db.AddParameter("@DebitNoteID", SqlDbType.Int, DebitNoteID);
               Global.m_db.AddParameter("@SeriesID", SqlDbType.Int, SeriesID);
               Global.m_db.AddParameter("@LedgerID", SqlDbType.Int, LedgerID);
               Global.m_db.AddParameter("@Voucher_No", SqlDbType.NVarChar, 30, VoucherNo);//Set same for both for time being
               Global.m_db.AddParameter("@DebitNote_Date", SqlDbType.DateTime, DebitNoteDate);
               Global.m_db.AddParameter("@Remarks", SqlDbType.NVarChar, 200, Remarks);
               Global.m_db.AddParameter("@ProjectID", SqlDbType.NVarChar, 200, ProjectID);
               Global.m_db.AddParameter("@Modified_By", SqlDbType.NVarChar, 50, User.CurrUserID.ToString());
               Global.m_db.AddParameter("@First", SqlDbType.NVarChar, 50, of.First);
               Global.m_db.AddParameter("@Second", SqlDbType.NVarChar, 50, of.Second);
               Global.m_db.AddParameter("@Third", SqlDbType.NVarChar, 50, of.Third);
               Global.m_db.AddParameter("@Fourth", SqlDbType.NVarChar, 50, of.Fourth);
               Global.m_db.AddParameter("@Fifth", SqlDbType.NVarChar, 50, of.Fifth);
               System.Data.SqlClient.SqlParameter objReturn = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
               Global.m_db.ProcessParameter();

               //First delete the old record
               Global.m_db.InsertUpdateQry("DELETE FROM Acc.tblDebitNoteDetail WHERE DebitNoteID='" + DebitNoteID.ToString() + "'");

               //Now insert the new ones
               string Language = "";
               switch (LangMgr.DefaultLanguage)
               {
                   case Lang.English:
                       Language = "ENGLISH";
                       break;
                   case Lang.Nepali:
                       Language = "NEPALI";
                       break;
               }
               //First delete the old transaction
               Global.m_db.InsertUpdateQry("DELETE FROM Acc.tblTransaction WHERE VoucherType='DR_NOT' AND RowID='" + DebitNoteID.ToString() + "'");

               //First delete the previous records of Acc.tblTransactionClass according to VoucherType and RowID
               Global.m_db.InsertUpdateQry("DELETE FROM Acc.tblTransactionClass WHERE VoucherType='DR_NOT' AND RowID='" + DebitNoteID.ToString() + "'");

               double Amount = 0;
               for (int i = 0; i < DebitNoteDetails.Rows.Count; i++)
               {
                   DataRow dr = DebitNoteDetails.Rows[i];
                   //Now go for the Detail Inserts
                   Global.m_db.ClearParameter();
                   Global.m_db.setCommandType(CommandType.StoredProcedure);
                   Global.m_db.setCommandText("Acc.spDebitNoteDetailCreate");
                   Global.m_db.AddParameter("@DebitNoteID", SqlDbType.Int, DebitNoteID.ToString());
                   Global.m_db.AddParameter("@LedgerName", SqlDbType.NVarChar, 30, dr["Ledger"].ToString());//Set same for both for time being
                   Global.m_db.AddParameter("@Language", SqlDbType.NVarChar, 30, Language);
                   Global.m_db.AddParameter("@Amount", SqlDbType.Money, Convert.ToDouble(dr["Amount"]));
                   Amount += Convert.ToDouble(dr["Amount"]);
                   Global.m_db.AddParameter("@Remarks", SqlDbType.NVarChar, 200, dr["Remarks"].ToString());
                   System.Data.SqlClient.SqlParameter paramReturn = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                   Global.m_db.ProcessParameter();
                   if (paramReturn.Value.ToString() != "SUCCESS")
                   {
                       Global.m_db.RollBackTransaction();
                       throw new Exception("Unable to create Debit Note");
                   }
                   //Also insert the transaction in tbltransaction
                   Global.m_db.ClearParameter();
                   Global.m_db.setCommandType(CommandType.StoredProcedure);
                   Global.m_db.setCommandText("Acc.spTransactCreate");
                   Global.m_db.AddParameter("@Date", SqlDbType.DateTime, DebitNoteDate);
                   Global.m_db.AddParameter("@LedgerName", SqlDbType.NVarChar, 30, dr["Ledger"].ToString());//Set same for both for time being
                   Global.m_db.AddParameter("@Language", SqlDbType.NVarChar, 30, Language);
                   Global.m_db.AddParameter("@Amount", SqlDbType.Money, Convert.ToDouble(dr["Amount"]));
                   //Amount += Convert.ToDouble(dr["Amount"]);
                   Global.m_db.AddParameter("@DrCr", SqlDbType.NVarChar, 10, "CREDIT");
                   Global.m_db.AddParameter("@VoucherType", SqlDbType.NVarChar, 20, "DR_NOT");
                   Global.m_db.AddParameter("@RowID", SqlDbType.Int, DebitNoteID);
                   Global.m_db.AddParameter("@ProjectID", SqlDbType.Int, ProjectID);
                   paramReturn = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                   Global.m_db.ProcessParameter();

                   int ReturnTransactID = Convert.ToInt32(paramReturn.Value);

                   if (paramReturn.Value.ToString() == "FAILURE")
                   {
                       throw new Exception("Unable to create Debit Note");
                   }
                   //Now add the New editable records for Acc.tblTransactionClass
                   foreach (int _AccClassID in AccClassID)
                   {
                       Global.m_db.ClearParameter();
                       Global.m_db.setCommandType(CommandType.StoredProcedure);
                       Global.m_db.setCommandText("Acc.spTransactClassCreate");
                       Global.m_db.AddParameter("@TransactID", SqlDbType.Int, ReturnTransactID.ToString());
                       Global.m_db.AddParameter("@AccClassID", SqlDbType.Int, _AccClassID);//Set same for both for time being
                       Global.m_db.AddParameter("@RowID", SqlDbType.Int, DebitNoteID.ToString());
                       Global.m_db.AddParameter("VoucherType", SqlDbType.NVarChar, 20, "DR_NOT");
                       System.Data.SqlClient.SqlParameter paramTransactClassID = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                       Global.m_db.ProcessParameter();

                       if (paramTransactClassID.Value.ToString() == "FAILURE")
                       {
                           Global.m_db.RollBackTransaction();
                           throw new Exception("Unable to create Debit Note");
                       }
                   }
               }
               string LedgerName = Ledger.GetLedgerNameFromID(LedgerID);
              // Also insert the transaction in tbltransaction
               Global.m_db.ClearParameter();
               Global.m_db.setCommandType(CommandType.StoredProcedure);
               Global.m_db.setCommandText("Acc.spTransactCreate");
               Global.m_db.AddParameter("@Date", SqlDbType.DateTime, DebitNoteDate);
               // Global.m_db.AddParameter("@LedgerID", SqlDbType.Int, LedgerID);//Set same for both for time beingLedgerName
               Global.m_db.AddParameter("@LedgerName", SqlDbType.NVarChar, 30, LedgerName);
               Global.m_db.AddParameter("@Language", SqlDbType.NVarChar, 30, Language);
               Global.m_db.AddParameter("@Amount", SqlDbType.Money, Amount);
               Global.m_db.AddParameter("@DrCr", SqlDbType.NVarChar, 10, "DEBIT");
               Global.m_db.AddParameter("@VoucherType", SqlDbType.NVarChar, 20, "DR_NOT");
               Global.m_db.AddParameter("@RowID", SqlDbType.Int, DebitNoteID);
               Global.m_db.AddParameter("@ProjectID", SqlDbType.Int, ProjectID);
               System.Data.SqlClient.SqlParameter paramReturn1 = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
               Global.m_db.ProcessParameter();

               int ReturnTransactID1 = Convert.ToInt32(paramReturn1.Value);

               if (paramReturn1.Value.ToString() == "FAILURE")
               {
                   throw new Exception("Unable to create Debit Note");
               }
               //Add the information  in Acc.tblTransactionClass
               foreach (int _AccClassID in AccClassID)
               {
                   Global.m_db.ClearParameter();
                   Global.m_db.setCommandType(CommandType.StoredProcedure);
                   Global.m_db.setCommandText("Acc.spTransactClassCreate");
                   Global.m_db.AddParameter("@TransactID", SqlDbType.Int, ReturnTransactID1.ToString());
                   Global.m_db.AddParameter("@AccClassID", SqlDbType.Int, _AccClassID);//Set same for both for time being
                   Global.m_db.AddParameter("@RowID", SqlDbType.Int, DebitNoteID.ToString());
                   Global.m_db.AddParameter("VoucherType", SqlDbType.NVarChar, 20, "DR_NOT");
                   System.Data.SqlClient.SqlParameter paramTransactClassID = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                   Global.m_db.ProcessParameter();
                   if (paramTransactClassID.Value.ToString() == "FAILURE")
                   {
                       Global.m_db.RollBackTransaction();
                       throw new Exception("Unable to create Debit Note");
                   }
               }
               Global.m_db.CommitTransaction();
           }
           catch (Exception ex)
           {
               Global.m_db.RollBackTransaction();

               throw ex;

               #region SQLException
               //switch (ex.Number)
               //{
               //    case 4060: // Invalid Database 
               //        Global.Msg("Invalid Database", MBType.Error, "Error");
               //        break;

               //    case 18456: // Login Failed 
               //        Global.Msg("Login Failed!", MBType.Error, "Error");
               //        break;

               //    case 547: // ForeignKey Violation , Check Constraint
               //        Global.Msg("Invalid parent group! Check the parent group and try again!", MBType.Warning, "Error");
               //        break;

               //    case 2627: // Unique Index/ Primary key Violation/ Constriant Violation 
               //        Global.Msg("The group name already exists! Please choose another group names!", MBType.Warning, "Error");
               //        break;

               //    case 2601: // Unique Index/Constriant Violation 
               //        Global.Msg("Unique index violation!", MBType.Warning, "Error");
               //        break;

               //    case 5000: //Trigger violation
               //        Global.Msg("Trigger violation!", MBType.Warning, "Error");
               //        break;

               //    default:
               //        break;
               //}
               #endregion
           }
       }

       /// <summary>
       /// Outputs the previous DebitNote Voucher Information. It sends blank if no previous record is available
       /// </summary>
       /// <param name="CurrentID"></param>
       /// <returns></returns>
       public DataTable NavigateDebitNoteMaster(int CurrentID, Navigate NavTo)
       {
           Global.m_db.ClearParameter();
           Global.m_db.setCommandType(CommandType.StoredProcedure);
           Global.m_db.setCommandText("Acc.spDebitNoteNavigate");
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

           DataTable dtDebitNoteMst = Global.m_db.GetDataTable();
           return dtDebitNoteMst;


       }

       public static DataTable GetDebNoteMasterInfo(DateTime FromDate, DateTime ToDate)
       {

           string SQL = "SELECT * FROM Acc.tblDebitNoteMaster WHERE DebitNote_Date BETWEEN '" + Date.ToDB(FromDate) + "' AND '" + Date.ToDB(ToDate) + "'";
        

          return Global.m_db.SelectQry(SQL, "table");




       }

       public static DataTable GetDebitNoteMasterInfo(DateTime FromDate, DateTime ToDate, ArrayList AccClassID,string VoucherType)
       { 
           string AccClassID1 = "";
           int i = 0;

           foreach (object j in AccClassID)
           {
               if (i == 0)// for first GroupID
                   AccClassID1 = "'" + j.ToString() + "'";
               else  //Separating Other GroupID by commas
                   AccClassID1 += "," + "'" + j.ToString() + "'";
               i++;
           }

           string SQLAccClassID = "";
           if (AccClassID1 != "")
           {
               SQLAccClassID = " AND AccClassID IN (" + AccClassID1+ ")";
           }

           return Global.m_db.SelectQry("SELECT * FROM Acc.tblDebitNoteMaster WHERE DebitNote_Date BETWEEN '" + Date.ToDB(FromDate) + "' AND '" + Date.ToDB(ToDate) + "' AND DebitNoteID IN (SELECT RowID FROM Acc.tblTransactionClass WHERE VoucherType = '" + VoucherType + "'" + SQLAccClassID + ")", "table");

       
       }

       public  static DataTable GetDebNoteDtlInfo(int DebitNoteID)
       {

           //   return Global.m_db.SelectQry("SELECT * FROM Acc.tblDebitNoteDetail WHERE DebitNoteID ='" + DebitNoteID + "' AND DrCr ='Debit'", "table");
           return Global.m_db.SelectQry("SELECT * FROM Acc.tblDebitNoteDetail WHERE DebitNoteID ='" + DebitNoteID + "' ", "table");
       
       }

       /// <summary>
       /// Outputs the Journal Detail Information. 
       /// </summary>
       /// <param name="CurrentID"></param>
       /// <returns></returns>
       public DataTable GetDebitNoteDetail(int DebitNoteID)
       {
           Global.m_db.ClearParameter();
           Global.m_db.setCommandType(CommandType.StoredProcedure);
           Global.m_db.setCommandText("Acc.spGetDebitNoteDetail");
           Global.m_db.AddParameter("@DebitNoteID", SqlDbType.Int, DebitNoteID);

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
           DataTable dtDebitNoteDtl = Global.m_db.GetDataTable();
           return dtDebitNoteDtl;


       }

       /// <summary>
       /// Simply Deletes the debit note from given Debit Note ID
       /// </summary>
       /// <param name="JournalID"></param>
       public bool Delete(int DebitNoteID)
       {
           //If he's sure he has intentionally pressed delete button,

           try
           {

               //Flush data from Transaction table
               Global.m_db.InsertUpdateQry("DELETE Acc.tblTransaction WHERE RowID='" + DebitNoteID.ToString() + "' AND VoucherType='DR_NOT'");

               //Delete data from Journal Master, Journal Details gets automatically flushed away
               Global.m_db.InsertUpdateQry("DELETE Acc.tblDebitNoteMaster WHERE DebitNoteID='" + DebitNoteID.ToString() + "'");
               return true;
           }
           catch (Exception ex)
           {
               return false;
           }

       }

       public DataTable GetDebitNoteMaster(int DebitNoteID)
       {

           return Global.m_db.SelectQry("SELECT * FROM Acc.tblDebitNoteMaster WHERE DebitNoteID ='" + DebitNoteID + "'", "DebitNoteMaster");
     
       }

       public int GetSeriesIDFromMasterID(int MasterID)
       {

           object returnID;
           returnID = Global.m_db.GetScalarValue("SELECT SeriesID FROM Acc.tblDebitNoteMaster WHERE DebitNoteID ='" + MasterID + "'");

           return Convert.ToInt32(returnID);


       }


    }
}
