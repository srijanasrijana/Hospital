using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using System.Windows.Forms;
using DateManager;
using System.Data.SqlClient;

namespace BusinessLogic.Accounting
{
   public class Budget
    {
       //functions for budget allocation
       public void saveBudgetAllocation(int budgetID,int accountID,string accType, decimal totalAllocationForAccount, DataTable dt )
       {
           try
           {
               Global.m_db.BeginTransaction();
               Global.m_db.ClearParameter();
               Global.m_db.setCommandType(CommandType.StoredProcedure);
               Global.m_db.setCommandText("Acc.spInsertMasterBudgetAllocation");
               Global.m_db.AddParameter("@BudgetID", SqlDbType.Int, budgetID);
               Global.m_db.AddParameter("@AccountID", SqlDbType.Int, accountID);
               Global.m_db.AddParameter("@AccountType", SqlDbType.NVarChar, 50, accType);

               Global.m_db.AddParameter("@TotalAllocationForAccount", SqlDbType.Decimal, totalAllocationForAccount);
               
               System.Data.SqlClient.SqlParameter objReturn = Global.m_db.AddOutputParameter("@Return", SqlDbType.Int);
               Global.m_db.ProcessParameter();

               int ReturnId = Convert.ToInt32(objReturn.Value);
               for (int i = 0; i < dt.Rows.Count;i++)
               {
                   DataRow dr = dt.Rows[i];
                   
                   Global.m_db.ClearParameter();
                   Global.m_db.setCommandType(CommandType.StoredProcedure);
                   Global.m_db.setCommandText("Acc.spInsertDetailBudgetAllocation");
                   Global.m_db.AddParameter("@MasterbudgetID", SqlDbType.Int, ReturnId);
                   Global.m_db.AddParameter("@ClassID", SqlDbType.Int, Convert.ToInt32(dr[0]));
                   Global.m_db.AddParameter("@Amount", SqlDbType.Decimal, Convert.ToDecimal(dr[1]));
                   System.Data.SqlClient.SqlParameter result = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                   Global.m_db.ProcessParameter();

                   if (result.Value.ToString() != "SUCCESS")
                   {
                       Global.m_db.RollBackTransaction();
                       throw new Exception("Unable to create budget...");
                   }                                                         
               }
               Global.m_db.CommitTransaction();
               MessageBox.Show("Budget successfully inserted...");

           }
           catch(Exception ex)
           {
               Global.m_db.RollBackTransaction();

               throw ex;
           }          
       }

       public DataTable GetAllocationDataFromBudgetID(int budgetID)
       {
           try
           {
               Global.m_db.ClearParameter();
               Global.m_db.setCommandType(CommandType.StoredProcedure);
               Global.m_db.setCommandText("Acc.spGetBudgetAllocationByBudgetID");
               Global.m_db.AddParameter("@budgetID", SqlDbType.Int, budgetID);
               return Global.m_db.GetDataTable();
           }
           catch (Exception ex)
           {
             //  throw ex;
               Global.MsgError(ex.Message);
               return null;
           }                     
       }
       //public int getMasterBudgetIDByBgtID(int budgetId)
       //{
       //    int i = 0;
       //    object obj= Global.m_db.GetScalarValue("SELECT budgetMasterID from Acc.tblBudgetAllocationMaster where budgetID='"+budgetId+"'");
       //     i = Convert.ToInt32(obj);
       //    return i;
       //}
      public void UpdateBudgetAllocation(int mid, decimal totalAllocationForAccount, DataTable dt)
      {
          try
          {
              Global.m_db.BeginTransaction();
              //first update master budget by the master budget id
              Global.m_db.InsertUpdateQry("UPDATE Acc.tblBudgetAllocationMaster SET TotalAllocationForAccount ='" + totalAllocationForAccount + "'  WHERE BudgetMasterID='" + mid + "'");
              //delete from budgetdetail table of master budget id
              Global.m_db.InsertUpdateQry("DELETE FROM Acc.tblBudgetAllocationDetail WHERE BudgetMasterID='" + mid + "'");
              //again insert data into budgetdetail table of master budget id 
              for (int i = 0; i < dt.Rows.Count; i++)
              {
                  DataRow dr = dt.Rows[i];

                  Global.m_db.ClearParameter();
                  Global.m_db.setCommandType(CommandType.StoredProcedure);
                  Global.m_db.setCommandText("Acc.spInsertDetailBudgetAllocation");
                  Global.m_db.AddParameter("@MasterbudgetID", SqlDbType.Int, mid);
                  Global.m_db.AddParameter("@ClassID", SqlDbType.Int, Convert.ToInt32(dr[0]));
                  Global.m_db.AddParameter("@Amount", SqlDbType.Decimal, Convert.ToDecimal(dr[1]));
                  System.Data.SqlClient.SqlParameter result = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                  Global.m_db.ProcessParameter();


                  if (result.Value.ToString() != "SUCCESS")
                  {
                      Global.m_db.RollBackTransaction();
                      throw new Exception("Unable to update budget allocation...");
                  }

              }
              Global.m_db.CommitTransaction();
              MessageBox.Show("Budget successfully updated...");
          }
          catch(Exception ex)
          {
              Global.m_db.RollBackTransaction();
             
              throw ex;
          }
      }
       public void DeleteAccountFromAllocation(int masterBudgetID)
      {
          try
          {
              Global.m_db.BeginTransaction();
              Global.m_db.InsertUpdateQry("DELETE FROM Acc.tblBudgetAllocationDetail WHERE BudgetMasterID='" + masterBudgetID + "'");

              Global.m_db.InsertUpdateQry("DELETE FROM Acc.tblBudgetAllocationMaster WHERE BudgetMasterID='" + masterBudgetID + "'");

              Global.m_db.CommitTransaction();
              MessageBox.Show(" Account deleted successfully...", "MESSAGE");
          }
          catch (Exception ex)
          {
              Global.m_db.RollBackTransaction();
              throw ex;
          }
      }

      //unuseable after updation
      //public void DeleteBudget(int budgetID,string budgetName)
      //{
      //    int mid=0;
      //    mid= getMasterBudgetIDByBgtID(budgetID);
      //    if(mid<=0)
      //    {
      //        MessageBox.Show("No allocation are made for Budget"+" "+budgetName+"","MESSAGE");
      //        return;
      //    }

      // DialogResult result= MessageBox.Show("Are you sure You want to delete  all allocation for the budget" + " " + budgetName ,"CONFIRMATION",MessageBoxButtons.YesNo);
      // if (result == DialogResult.Yes)
      // {
      //     try
      //     {
      //         Global.m_db.BeginTransaction();
      //         Global.m_db.InsertUpdateQry("DELETE FROM Acc.tblBudgetAllocationDetail WHERE budgetMasterID='" + mid + "'");

      //         Global.m_db.InsertUpdateQry("DELETE FROM Acc.tblBudgetAllocationMaster WHERE budgetMasterID='" + mid + "'");

      //         Global.m_db.CommitTransaction();
      //         MessageBox.Show(" All allocations for Budget" + " " + budgetName + " are deleted successfully...", "MESSAGE");
               
      //     }
      //     catch (Exception ex)
      //     {
      //         Global.m_db.RollBackTransaction();
      //         throw ex;
      //     }
      // }

      //}
  
       public DataTable GetClassIDnAmount(int MasterBudgetID)
      {
          return Global.m_db.SelectQry("Select ClassID, Amount from Acc.tblBudgetAllocationDetail where BudgetMasterID='" + MasterBudgetID + "'", "ClassData");

      }

       
      //folloing functions are for new budget name addition,updation or deletion

       public void AddBudget(string budgetName,DateTime startDate,DateTime endDate,string description)
      {
          try
          {
              int chk = 0;
          object obj=  Global.m_db.GetScalarValue("Select budgetID from Acc.tblBudget where budgetName='"+budgetName+"'");
           chk = Convert.ToInt32(obj);
              if(chk>0)
              {
                  MessageBox.Show("Budget name '"+budgetName+"' already exists.");
                  return;
              }
              chk = 0;
              object ob = Global.m_db.GetScalarValue("Select budgetID from Acc.tblBudget where startDate='" + startDate + "'");
              chk = Convert.ToInt32(ob);
              if(chk>0)
              {
                  MessageBox.Show("Budget for date '"+startDate+"' had already created");
                  return;
              }                
              Global.m_db.ClearParameter();
              Global.m_db.setCommandType(CommandType.StoredProcedure);
              Global.m_db.setCommandText("Acc.spAddBudgetName");
              Global.m_db.AddParameter("@budgetName", SqlDbType.NVarChar, 50, budgetName);
              Global.m_db.AddParameter("@startDate", SqlDbType.Date, startDate);
              Global.m_db.AddParameter("@endDate", SqlDbType.Date, endDate);
              Global.m_db.AddParameter("@description", SqlDbType.NVarChar, 100, description);
              int i = Global.m_db.ProcessParameter();
              if (i > 0)
              {
                  MessageBox.Show(" Budget name added.");
              }
              else
              {
                  MessageBox.Show("Unable to add budget name.");
              }

          }
          catch (Exception ex)
          {

              MessageBox.Show(ex.Message); 
          }
      }      
       public DataTable GetCurrentNUpcommingBudgetData()
       {          
           DateTime currentdate = Date.ToDotNet(Date.ToSystem(Date.GetServerDate()));
           return Global.m_db.SelectQry("Select budgetID, budgetName,startDate,endDate,[description] from Acc.tblBudget where endDate>='"+currentdate+"'","BudgetData");
       }
       public void EditBudgetName(int budgetID,string budgetName,DateTime startDate,DateTime endDate,string description)
       {
           try
           {
               int chk = 0;
               object obj = Global.m_db.GetScalarValue("Select budgetID from Acc.tblBudget where budgetName='" + budgetName + "' and budgetID!='"+budgetID+"'");
               chk = Convert.ToInt32(obj);
               if (chk > 0)
               {
                   MessageBox.Show("Budget name '" + budgetName + "' already exists.");
                   return;
               }
               chk = 0;
               object ob = Global.m_db.GetScalarValue("Select budgetID from Acc.tblBudget where startDate='" + startDate + "' and budgetID!='"+budgetID+"'");
               chk = Convert.ToInt32(ob);
               if (chk > 0)
               {
                   MessageBox.Show("Budget for date '" + startDate + "' had already created");
                   return;
               }
               DialogResult res = MessageBox.Show("Are you sure you want to edit the budget"+" "+budgetName,"CONFIRMATION",MessageBoxButtons.YesNo);
               if(res==DialogResult.No)
               {
                   return;
               }
                 Global.m_db.ClearParameter();
              Global.m_db.setCommandType(CommandType.StoredProcedure);
              Global.m_db.setCommandText("Acc.spEditBudgetName");
              Global.m_db.AddParameter("@budgetID", SqlDbType.Int,budgetID);
              Global.m_db.AddParameter("@budgetName", SqlDbType.NVarChar, 50, budgetName);
              Global.m_db.AddParameter("@startDate", SqlDbType.Date, startDate);
              Global.m_db.AddParameter("@endDate", SqlDbType.Date, endDate);
              Global.m_db.AddParameter("@description", SqlDbType.NVarChar, 100, description);

              int i = Global.m_db.ProcessParameter();
              if (i > 0)
              {
                  MessageBox.Show("Budget successfully updated.");
              }
              else
              {
                  MessageBox.Show("Unable to update budget.");
              }
           }
           catch(Exception ex)
           {
               MessageBox.Show(ex.Message);
           }
       }

       public void DeleteBudget(int budgetID)
       {
           DataTable dt = Global.m_db.SelectQry("Select budgetMasterID from Acc.tblBudgetAllocationMaster where budgetID='" + budgetID + "'","MasterBudgetID");
           if(dt.Rows.Count>0)
           {
               MessageBox.Show("You don't have permission to delete this budget...");
               return;
           }
        DialogResult res=  MessageBox.Show("Are you sure you want to delete the budget...","Confirmation",MessageBoxButtons.YesNo);
           if(res==DialogResult.No)
           {
               return;
           }
           int i = Global.m_db.InsertUpdateQry("Delete from Acc.tblBudget where budgetID='" + budgetID + "'");
           if(i>0)
           {
               MessageBox.Show("Budget successfully deleted.");
           }
           else
           {
               MessageBox.Show("Unable to delete.");
           }
       }

       public void DeleteBudgetAllAllocation(int budgetID)
       {
           try
           {
               Global.m_db.BeginTransaction();
               Global.m_db.ClearParameter();
               Global.m_db.setCommandType(CommandType.StoredProcedure);
               Global.m_db.setCommandText("Acc.spDeleteBudgetAllocation");
               Global.m_db.AddParameter("@BudgetID", SqlDbType.Int, budgetID);
               Global.m_db.ProcessParameter();
               Global.m_db.CommitTransaction();
           }
           catch (Exception ex)
           {
               Global.m_db.RollBackTransaction();
               throw ex;
           }

       }
    
       public DataTable  getStartNEndDate(int budgetID)
       {
           return Global.m_db.SelectQry("Select startDate,endDate from Acc.tblBudget where budgetID='" + budgetID + "'","budgetData");

       }
       public DataTable CheckRedundency(DateTime startDate,DateTime endDate,bool isNew,int id)
       {
           if(isNew)
               return Global.m_db.SelectQry("Select *from Acc.tblBudget where '" + startDate + "' between startDate and endDate or '" + endDate + "' between startDate and endDate or startDate between '" + startDate + "' and '" + endDate + "' or endDate between '" + startDate + "' and '" + endDate + "'", "Budget");
           else
               return Global.m_db.SelectQry("Select *from Acc.tblBudget where  budgetID!='" + id + "' and ('" + startDate + "' between startDate and endDate or '" + endDate + "' between startDate and endDate or startDate between '" + startDate + "' and '" + endDate + "' or endDate between '" + startDate + "' and '" + endDate + "') ", "Budget");

       }

       public DataTable GetBudgetIDNName()
       {
           return Global.m_db.SelectQry("Select budgetID, budgetName from Acc.tblBudget ", "Budget");
       }
       public DataTable GetBudgetIDnNameFromAllocation()
       {
           return Global.m_db.SelectQry("Select budgetID, budgetName from Acc.tblBudget as B where B.budgetID in(select distinct BudgetID from Acc.tblBudgetAllocationMaster ) ", "Budget");
       }

       public static bool CheckBudget(int ledgerID, decimal amount,string strAccClassIDs)
       {
           try
           {
               DateTime currentdate = Date.ToDotNet(Date.ToSystem(Date.GetServerDate()));

               Global.m_db.ClearParameter();
               Global.m_db.setCommandType(CommandType.StoredProcedure);
               Global.m_db.setCommandText("Acc.spCheckBudget");
               Global.m_db.AddParameter("@CurrentDate", SqlDbType.DateTime, currentdate);
               Global.m_db.AddParameter("@LedgerID", SqlDbType.Int, ledgerID);
               Global.m_db.AddParameter("@Amount", SqlDbType.Decimal, amount);
               Global.m_db.AddParameter("@AccountClassIDsSettings", SqlDbType.Xml, strAccClassIDs);

              DataTable BudgetInfo=  Global.m_db.GetDataTable();
              if (BudgetInfo.Rows.Count > 0)
              {
                  if (Convert.ToInt32(BudgetInfo.Rows[0][0]) == 0)
                  {
                      switch (Global.Default_BudgetLimit)
                      {
                          case BudgetLimit.Deny:
                              Global.Msg("Provided amount exceeds the budget limit of Account " + BudgetInfo.Rows[0][1].ToString() + " By " + (Convert.ToDecimal(BudgetInfo.Rows[0][3]) - Convert.ToDecimal(BudgetInfo.Rows[0][2])).ToString());
                              //   grdJournal[Convert.ToInt32(CurRow), (int)GridColumn.Amount].Value = 0;
                              return false;
                              break;
                          case BudgetLimit.Warn:
                              //  DialogResult result=Global.MsgQuest("Provided amount exceeds the budget limit of Account " + BudgetInfo.Rows[0][1].ToString() + " By " + (Convert.ToInt32(BudgetInfo.Rows[0][3]) - Convert.ToInt32(BudgetInfo.Rows[0][2])).ToString()," ",MessageBoxButtons.YesNo);
                              DialogResult result = Global.MsgQuest("Provided amount exceeds the budget limit of Account " + BudgetInfo.Rows[0][1].ToString() + " By " + (Convert.ToInt32(BudgetInfo.Rows[0][3]) - Convert.ToInt32(BudgetInfo.Rows[0][2])).ToString() + "\n Do you want to continue ?");
                              if (result == DialogResult.No)
                              {
                                  // grdJournal[Convert.ToInt32(CurRow), (int)GridColumn.Amount].Value = 0;
                                  return false;
                              }
                              break;
                          default:
                              break;
                      }
                  }
              }
                  return true;
                          
           }
           catch (Exception ex)
           {
               Global.MsgError(ex.Message);
               return false;
           }


       }

       public static bool CopyBudget(int budgetID,int newBudgetID)
       {
           try
           {
               Global.m_db.BeginTransaction();
               Global.m_db.ClearParameter();
               Global.m_db.setCommandType(CommandType.StoredProcedure);
               Global.m_db.setCommandText("Acc.spCopyBudget");
               Global.m_db.AddParameter("@BudgetID", SqlDbType.Int, budgetID);
               Global.m_db.AddParameter("@NewBudgetID", SqlDbType.Int, newBudgetID);
               Global.m_db.ProcessParameter();
               Global.m_db.CommitTransaction();
               return true;
           }
           catch (Exception ex)
           {
               Global.m_db.RollBackTransaction();
               throw ex;
               
           }
          
       }
       
    }
}
