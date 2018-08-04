using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;
using DBLogic;
using DateManager;

namespace BusinessLogic
{
    public class Reminder
    {
        public static DataTable SelectReminder(int ReminderID)
        {
            DataTable dt = new DataTable(); 
             string strQuery = "";
             if (ReminderID == 0)
             {
                 strQuery = "SELECT * FROM System.tblReminder";
             }
             else
             {
                 strQuery = "SELECT * FROM System.tblReminder where ReminderID= '" + ReminderID + "'";
             }
            dt = Global.m_db.SelectQry(strQuery, "tblReminder");
            return dt;            
        }

        public static DataTable GetRecurrenceDetail(int ReminderID)
        {
            DataTable dt = new DataTable();
            string strQuery = "SELECT * FROM System.tblRecurrence where ReminderID= '" + ReminderID + "'";
            dt = Global.m_db.SelectQry(strQuery, "tblRecurrence");
            return dt;
        }


        public static ArrayList GetReminderUserDetail(int ReminderID)
        {
            ArrayList userList = new ArrayList();
            DataTable dt = new DataTable();
            string strQuery = "SELECT * FROM System.tblReminderUser where ReminderID= '" + ReminderID + "'";
            dt = Global.m_db.SelectQry(strQuery, "tblReminderUser");
            foreach (DataRow dr in dt.Rows)
            {
                userList.Add(dr["UserID"]);
            }
            return userList;
        }

        public static bool IsRecurrenceExist(int ReminderID)
        {
            DataTable dt = new DataTable();
            string strQuery = "SELECT * FROM System.tblRecurrence where ReminderID= '" + ReminderID + "'";
            dt = Global.m_db.SelectQry(strQuery, "tblRecurrence");
            if (dt.Rows.Count != 0)
                return true;
            else
                return false;           
        }

        public static int CreateReminder(string Subject, int Status, int Priority, DateTime? StartDate, string Description)
        {
            try
            {
                if (StartDate == null)
                    StartDate = DateManager.Date.GetServerDate();

                Global.m_db.ClearParameter();
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("System.spReminderCreate");
                Global.m_db.AddParameter("@Subject", SqlDbType.NVarChar, 200, Subject);
                Global.m_db.AddParameter("@Status", SqlDbType.Int,Status);
                Global.m_db.AddParameter("@Priority", SqlDbType.Int, Priority);               
                //Global.m_db.AddParameter("@Date", SqlDbType.DateTime, DateManager.Date.ToDB(Convert.ToDateTime(StartDate)));                
                Global.m_db.AddParameter("@Date", SqlDbType.DateTime,StartDate);                
                Global.m_db.AddParameter("@Description", SqlDbType.NVarChar, 500, Description);                
                Global.m_db.AddParameter("@CreatedBy", SqlDbType.NVarChar, 50, User.CurrUserID.ToString());           
                System.Data.SqlClient.SqlParameter paramReturn = Global.m_db.AddOutputParameter("@ReturnID", SqlDbType.Int);
                Global.m_db.ProcessParameter();

                return Convert.ToInt32(paramReturn.Value);

            }
            catch (Exception ex)
            {              
                throw ex;
            }
        }


        public static int ModifyReminder(int ReminderID,string Subject, int Status, int Priority, DateTime StartDate, string Description)
        {
            try
            {
                Global.m_db.ClearParameter();
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("System.spReminderModify");
                Global.m_db.AddParameter("@ReminderID", SqlDbType.Int, ReminderID);
                Global.m_db.AddParameter("@Subject", SqlDbType.NVarChar, 200, Subject);
                Global.m_db.AddParameter("@Status", SqlDbType.Int, Status);
                Global.m_db.AddParameter("@Priority", SqlDbType.Int, Priority);
                Global.m_db.AddParameter("@Date", SqlDbType.DateTime, StartDate);                
                Global.m_db.AddParameter("@Description", SqlDbType.NVarChar, 500, Description);
                Global.m_db.AddParameter("@ModifiedBy", SqlDbType.NVarChar, 50, User.CurrUserID.ToString());
                System.Data.SqlClient.SqlParameter paramReturn = Global.m_db.AddOutputParameter("@ReturnID", SqlDbType.Int);
                Global.m_db.ProcessParameter();
                return Convert.ToInt32(@ReminderID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void DeleteRecurrence(int ReminderID)
        {
            DataTable dt = new DataTable();
            string strQuery = "Delete FROM System.tblRecurrence where ReminderID= '" + ReminderID + "'";
            Global.m_db.SelectQry(strQuery, "tblRecurrence");             
        }

         public static void DeleteReminderUser(int ReminderID)
        {
            DataTable dt = new DataTable();
            string strQuery = "Delete FROM System.tblReminderUser where ReminderID= '" + ReminderID + "'";
            Global.m_db.SelectQry(strQuery, "tblReminderUser");            
        }
        public static void DeleteReminder(int ReminderID)
        {
            DataTable dt = new DataTable();
            string strQuery = "Delete FROM System.tblReminder where ReminderID= '" + ReminderID + "'";
            Global.m_db.SelectQry(strQuery, "tblReminder");
        }

        public static string CreateRecurrence( DataTable dtRecurrence, int ReminderID)
        {
            try
            {
                if (dtRecurrence.Rows.Count == 0)
                    return "SUCCESS";

                DataRow dr = dtRecurrence.Rows[0];
                Global.m_db.ClearParameter();
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("System.spRecurrenceCreate");
                //Global.m_db.AddParameter("@OccurrenceDateStart", SqlDbType.DateTime,DateManager.Date.ToDB(Convert.ToDateTime(dr["OccurenceDateStart"])));
                //Global.m_db.AddParameter("@OccurrenceDateEnd", SqlDbType.DateTime, DateManager.Date.ToDB(Convert.ToDateTime(dr["OccurenceDateEnd"])));
                Global.m_db.AddParameter("@RecurrencePattern", SqlDbType.Int, dr["OccurencePattern"]);
                Global.m_db.AddParameter("@ReminderID", SqlDbType.Int, ReminderID);
                System.Data.SqlClient.SqlParameter paramReturn = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                Global.m_db.ProcessParameter();

                if (paramReturn.Value.ToString() == "SUCCESS")
                    return "SUCCESS";
                else
                    return "FAILURE";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void CreateRecurrenceUser(ArrayList UserList, int ReminderID)
        {
            try
            {                         
                foreach (int UserID in UserList)
                {
                    Global.m_db.ClearParameter();
                    Global.m_db.setCommandType(CommandType.StoredProcedure);
                    Global.m_db.setCommandText("System.spReminderUserCreate");
                    Global.m_db.AddParameter("@ReminderID", SqlDbType.Int, ReminderID);
                    Global.m_db.AddParameter("@UserID", SqlDbType.NVarChar, 50, UserID);
                    System.Data.SqlClient.SqlParameter paramReturn = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                    Global.m_db.ProcessParameter();
                }      
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public static DataTable GetActiveReminderByUser(int UserID)
        {
            DataTable dt = new DataTable();
            string strQuery = "Select * FROM System.tblReminder rem,System.tblReminderUser remUsr where rem.ReminderID= remUsr.ReminderID AND remUsr.UserID ='" + UserID + "' AND rem.Status = '" +  1 + "' Order By Priority Asc" ;
            dt = Global.m_db.SelectQry(strQuery, "tblReminderUser");
            return dt;
        }

        public static DataTable GetReminderIfExistToday(int UserID, int Status)
        {           
            //first get the reminder of the current user           
            DataTable dtReminder = new DataTable();
            DataTable dtFinal = new DataTable();
            string strQuery = "Select * FROM System.tblReminder rem,System.tblReminderUser remUsr where rem.ReminderID= remUsr.ReminderID AND remUsr.UserID =" + UserID + " AND rem.Status =" + Status + " Order By Priority Desc";
            dtReminder = Global.m_db.SelectQry(strQuery, "tblReminderUser");
            if (dtReminder.Rows.Count == 0)
            {
                return dtReminder;
            }
            dtFinal = dtReminder.Clone();
            dtFinal.Clear();
            foreach (DataRow drReminder in dtReminder.Rows)
            {
                //second get the recurrence of the current reminder
                DataTable dtRecurrence = new DataTable();
                DateTime ReminderDate = new DateTime();
                strQuery = "Select * FROM System.tblReminder rem,System.tblRecurrence rec where rem.ReminderID= rec.ReminderID AND rec.ReminderID = " + Convert.ToInt32(drReminder["ReminderID"]);
                dtRecurrence = Global.m_db.SelectQry(strQuery, "tblReminderUser");
                if (dtRecurrence.Rows.Count == 0)
                {
                    //Do nothing
                    if(Convert.ToDateTime(drReminder["Date"]) == DateTime.Today)
                        dtFinal.ImportRow(drReminder);
                    //return dtRecurrence;
                }
                foreach (DataRow drRecurrence in dtRecurrence.Rows)
                {
                    int multiplierDigit = 1;
                    // int difference = DateTime.Now - Convert.ToDateTime(drReminder["Date"]);
                    int Recurrence = Convert.ToInt32(drRecurrence["OccurencePattern"]);
                    switch (Recurrence)
                    {
                        case 0: if (Convert.ToDateTime(drReminder["Date"]) <= DateTime.Today)
                            dtFinal.ImportRow(drReminder); //if daily
                            break;
                        case 1: ReminderDate = Convert.ToDateTime(drReminder["Date"]); //if weekly
                                multiplierDigit =Convert.ToInt32((DateTime.Today - ReminderDate).TotalDays);
                                multiplierDigit = multiplierDigit / 7;
                                if(ReminderDate.AddDays(multiplierDigit * 7) == DateTime.Today)
                                    dtFinal.ImportRow(drReminder);
                                    // difference = DateTime.Now - Convert.ToDateTime(drReminder["Date"]);
                                break;
                        case 2: ReminderDate = Convert.ToDateTime(drReminder["Date"]);   //if Monthly
                              multiplierDigit =Convert.ToInt32((DateTime.Today - ReminderDate).TotalDays);
                                multiplierDigit = multiplierDigit / 30;
                                if (ReminderDate.AddMonths(multiplierDigit) == DateTime.Today)
                                    dtFinal.ImportRow(drReminder);     
                                break;
                        case 3: ReminderDate = Convert.ToDateTime(drReminder["Date"]);   //if Yearly
                                 multiplierDigit =Convert.ToInt32((DateTime.Today - ReminderDate).TotalDays);
                                multiplierDigit = multiplierDigit / 365;
                                if (ReminderDate.AddYears(multiplierDigit) == DateTime.Today)
                                    dtFinal.ImportRow(drReminder);
                                break;
                    }                    
                }               
            }
            //if recurrence is not set then no reminder message
            return dtFinal;
        }
    }
}
