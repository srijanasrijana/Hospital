using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BusinessLogic;
using System.Data;
using System.Data.SqlClient;

namespace BusinessLogic
{
    public class UserPreference
    {
        public enum UserPreferences { DefaultLanguage, DefaultDate, DateFormat };
        public bool UpdateUserPreference(int userid, int preferenceid, string value)
        {
            try
            {
                Global.m_db.InsertUpdateQry("UPDATE System.tbluser_Preference SET Value='" + value + "' where userid='"+userid+"' and preferenceid='"+preferenceid+"' ");
                return true;

            }
            catch (Exception)
            {
                throw;
            }

        }

        public bool UpdateUserPreference(int userID, string preferenceCode, string value)
        {
            try
            {
                Global.m_db.InsertUpdateQry("UPDATE up set Value = '"+value+"'   from System.tbluser_Preference as up where (up.UserID = "+userID+" and up.preferenceid =(select PreferenceID from System.tblPreference p where p.PreferenceID = up.PreferenceID and p.Code = '"+preferenceCode+"'))");
                return true;

            }
            catch(Exception)
            {
                throw;
            }

        }

        public bool SetUserPreference(int userID, string preferenceCode, string value)
        {
            try
            {
                Global.m_db.InsertUpdateQry("insert into  System.tblUser_Preference  values("+userID+", (select top 1 PreferenceID from System.tblPreference where Code = '"+preferenceCode+"'), '"+value+"')");
                return true;

            }
            catch (Exception)
            {
                throw;
            }

        }

        public  bool SetUserPreference(int userid,int preferenceid,string value)
        {
            try
            {
                Global.m_db.InsertUpdateQry("insert into System.tbluser_Preference values('"+userid+"','"+preferenceid+"','"+value+"')");
                return true;

            }
            catch (Exception)
            {
                throw;
            }

        }

        public string GetUserPreference(string Code)
        {
            try
            {
                object Result = Global.m_db.GetScalarValue("SELECT PreferenceID FROM System.tblpreference WHERE Code='" + Code + "'");
                return Result.ToString();

            }
            catch (Exception)
            {

                throw;
            }
        }
        public DataTable GetPreferenceInfo(int userid)
        {
            DataTable dt = new DataTable();
            string strQuery = "select p.Name,p.Code,up.Value from System.tblPreference p,System.tbluser_Preference up where p.PreferenceID=up.PreferenceID and up.UserID='"+userid+"'";
            dt = Global.m_db.SelectQry(strQuery, "tbluserpreference");
            //if (dt.Rows.Count <= 0)
            //{
            //    throw new Exception("Invalid");
            //}
            return dt;
        }
        public DataTable GetPreferenceCount(int userid)
        {
            DataTable dt = new DataTable();
            string strQuery = "select p.Name,p.Code,up.Value from System.tblPreference p,System.tbluser_Preference up where p.PreferenceID=up.PreferenceID and up.UserID='" + userid + "'";
            dt = Global.m_db.SelectQry(strQuery, "tbluserpreference");
            //if (dt.Rows.Count <= 0)
            //{
            //    throw new Exception("Invalid");
            // }

            return dt;
        }
        public static string GetValue(string Code,int UserID)
        {
            try
            {
                object Value;

                Value = Global.m_db.GetScalarValue("SELECT up.Value from System.tbluser_Preference up,System.tblPreference p WHERE p.Code='" + Code + "' and up.UserID='" + UserID + "' and p.PreferenceID=up.PreferenceID");
                if (Value == null)
                {
                    throw new Exception("Invalid settings code");
                    //return null;
                }
                return Value.ToString();
            }
            catch (Exception)
            {
                
                throw;
            }
        }
        /// <summary>
        /// Converts Settings code(.NET level) to Database Level Code to be inserted in the database table
        /// </summary>
        /// <param name="Code"></param>
        /// <returns></returns>
        public static string CodeToDB(UserPreferences Code)
        {
            switch (Code)
            {
                case UserPreferences.DateFormat:
                    return "DATE_FORMAT";
                    //break;
                case UserPreferences.DefaultDate:
                    return "DEFAULT_DATE";
                    //break;
                case UserPreferences.DefaultLanguage:
                    return "DEFAULT_LANGUAGE";
                    //break;
                default:
                    throw new Exception("Invalid Settings Code - " + Code);
                    //return null;


            }
        }
        

    }
}
