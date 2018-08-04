using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DBLogic;
using System.Data.SqlClient;
using System.Security.Cryptography;

namespace BusinessLogic
{
    public class User
    {
        //Hold current user
        public static int CurrUserID =1;
        public static string CurrentUserName = "";

        public static string GetCurrUser()
        {
            DataTable dt = new DataTable();
            dt = GetUserInfo(CurrUserID);
            DataRow dr = dt.Rows[0];
            return dr["UserName"].ToString();
        }
        public static string GetCurrFullName()
        {
            DataTable dt = new DataTable();
            dt = GetUserInfo(CurrUserID);
            DataRow dr = dt.Rows[0];
            return dr["Name"].ToString();
        }
        public static DataTable GetUserInfo(int UserID)
        {
            try
            {


                if (UserID == 0)
                    return Global.m_db.SelectQry("SELECT * FROM System.tblUser where UserStatus='" + 1 + "'", "tblUser");
                else if (UserID == -1)
                    return Global.m_db.SelectQry("SELECT * FROM System.tblUser", "tblUser");
                else
                    return Global.m_db.SelectQry("SELECT * FROM System.tblUser WHERE UserID='" + UserID + "'", "tblUser");

                // return Global.m_db.SelectQry("SELECT * FROM System.tblUser WHERE UserID='" + UserID + "'", "tblUser");
            }
            catch
            {
                throw new Exception("Unable to get user information");
            }
        }




        public static DataTable CheckUserStatus(int UserID)
        {
                return Global.m_db.SelectQry("SELECT * FROM System.tblUser WHERE UserID='" + UserID + "'", "tblUser");
        }
        /// <summary>
        /// Gets the information of given parent ID in table format
        /// </summary>
        /// <param name="ParentID"></param>
        /// <returns></returns>
        public static DataTable GetAccessInfo(int ParentID)
        {
            if (ParentID == 0)
                return Global.m_db.SelectQry("SELECT * FROM System.tblAccess WHERE ParentID is null", "tblAccess");
            else
                return Global.m_db.SelectQry("SELECT * FROM System.tblAccess WHERE ParentID=" + ParentID.ToString(), "tblAccess");
        }

        public static bool HasRight(string SecurityCode)
        {
            try
            {
                //Read code from tblAccess

                //Look at tblUserAccess if the current user has the access previlage

                int RowCount = (int)Global.m_db.GetScalarValue("SELECT Count(*) FROM System.tblUserAccess WHERE UserID='" + User.CurrUserID.ToString() + "' AND AccessID=(SELECT AccessID FROM System.tblAccess WHERE Code='" + SecurityCode.ToString() + "')");
                if (RowCount > 0)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                throw new Exception("Unknown Error");
                return false;
            }
        }
        public void changepassword(int userid, string password)
        {
            string EncryptedPassword = "";
            if (!String.IsNullOrEmpty(password))
                EncryptedPassword = Cryptography.Crypto.Encrypt(password.Trim(), "Ac104");
            Global.m_db.ClearParameter();
            Global.m_db.setCommandType(CommandType.StoredProcedure);
            Global.m_db.setCommandText("System.spchangepassword");
            Global.m_db.AddParameter("@UserID", SqlDbType.Int, userid);
            Global.m_db.AddParameter("@Password", SqlDbType.NVarChar, 150, EncryptedPassword);
            object objReturn = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
            Global.m_db.ProcessParameter();
            // Global.m_db.InsertUpdateQry("UPDATE System.tblAccessRole SET " + LangField + " = '" + NewRoleName + "' WHERE RoleID='" + AccessRoleID + "'");
            //Global.m_db.InsertUpdateQry("update System.tblUser SET password = '" + EncryptedPassword + "' where UserID='" + userid + "'");

        }



        public void save(string username,string password,string Name,string Address,string Contact,string Email,string Department,int AccessRoleID,int AccClassID,string createdby)
        {
            string EncryptedPassword = Cryptography.Crypto.Encrypt(password.Trim(), "Ac104");
            Global.m_db.ClearParameter();
            Global.m_db.setCommandType(CommandType.StoredProcedure);
            Global.m_db.setCommandText("System.spUserCreate");
            Global.m_db.AddParameter("@UserName",SqlDbType.NVarChar,150,username);
            Global.m_db.AddParameter("@Password", SqlDbType.NVarChar, 150, EncryptedPassword);
            Global.m_db.AddParameter("@Name",SqlDbType.NVarChar,150,Name);
            Global.m_db.AddParameter("@Address",SqlDbType.NVarChar,150,Address);
            Global.m_db.AddParameter("@Contact",SqlDbType.NVarChar,150,Contact);
            Global.m_db.AddParameter("@Email",SqlDbType.NVarChar,150,Email);
            Global.m_db.AddParameter("@Department",SqlDbType.NVarChar,150,Department);
            Global.m_db.AddParameter("@AccessRoleID",SqlDbType.Int,AccessRoleID);
            Global.m_db.AddParameter("@AccClassID", SqlDbType.Int, AccClassID);
            Global.m_db.AddParameter("@Created_By",SqlDbType.NVarChar,20,createdby);
            object objReturn = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
            Global.m_db.ProcessParameter();
        }

        public void edit(string Username,string password,int userid,string name,string address,string contact,string email,string department,int accessroleid,int AccClassID,string modifiedby)
        {
            string EncryptedPassword="";
            if(!String.IsNullOrEmpty(password))
                EncryptedPassword = Cryptography.Crypto.Encrypt(password.Trim(), "Ac104");

            Global.m_db.ClearParameter();
            Global.m_db.setCommandType(CommandType.StoredProcedure);
            Global.m_db.setCommandText("System.spUserModify");
            Global.m_db.AddParameter("@UserID",SqlDbType.Int,userid);
            Global.m_db.AddParameter("@UserName",SqlDbType.NVarChar,150,Username);
            Global.m_db.AddParameter("@Password", SqlDbType.NVarChar, 150, EncryptedPassword);
            Global.m_db.AddParameter("@Name",SqlDbType.NVarChar,150,name);
            Global.m_db.AddParameter("@Address",SqlDbType.NVarChar,150,address);
            Global.m_db.AddParameter("@Contact",SqlDbType.NVarChar,150,contact);
            Global.m_db.AddParameter("@Email",SqlDbType.NVarChar,150,email);
            Global.m_db.AddParameter("@Department",SqlDbType.NVarChar,150,department);
            Global.m_db.AddParameter("@AccessRoleID",SqlDbType.Int,accessroleid);
            Global.m_db.AddParameter("@AccClassID", SqlDbType.Int, AccClassID);
            Global.m_db.AddParameter("@ModifiedBy",SqlDbType.NVarChar,20,modifiedby);
            object objReturn = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
            Global.m_db.ProcessParameter();
        }

        public void delete(int userid)
        {
            Global.m_db.InsertUpdateQry("Delete from System.tblUser where UserID='" + userid + "'");
        }

        public void UserStatusEnable(int userid)
        {
            Global.m_db.InsertUpdateQry("Update System.tblUser  set UserStatus='"+1+"' where UserID='" + userid + "'");
        }

        public void UserStatusDisable(int userid)
        {
            Global.m_db.InsertUpdateQry("Update System.tblUser  set UserStatus='" + 0 + "' where UserID='" + userid + "'");
        }
        public static void AddAccessRole(string AccessRoleName, int[] AccessID)
        {
            try
            {
                Global.m_db.BeginTransaction();
                Global.m_db.ClearParameter();
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("Acc.spAccessRoleCreate");
                if (LangMgr.DefaultLanguage == Lang.English)
                    Global.m_db.AddParameter("@EngName", SqlDbType.NVarChar, 50, AccessRoleName);
                else if (LangMgr.DefaultLanguage == Lang.Nepali)
                    Global.m_db.AddParameter("@NepName", SqlDbType.NVarChar, 50, AccessRoleName);
                Global.m_db.AddParameter("@Created_By", SqlDbType.NVarChar, 20, User.CurrUserID.ToString());
                System.Data.SqlClient.SqlParameter paramReturn = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                Global.m_db.ProcessParameter();

                //Now insert Details
                foreach (int _AccessID in AccessID)
                {
                    Global.m_db.InsertUpdateQry("INSERT INTO System.tblAccessRoleDtl(RoleID,AccessID) VALUES('" + paramReturn.Value.ToString() + "','" + _AccessID + "')");
                }

                //if everthing is OK, commit the transaction
                Global.m_db.CommitTransaction();
            }
            catch (Exception ex)
            {
                //if error occured, rollback the transaction
                Global.m_db.RollBackTransaction();
                throw ex;
            }
        }

        public static void ModifyAccessRole(string NewRoleName, int AccessRoleID, int[] AccessID)
        {

            #region Language Mgmt
            string LangField = "";
            switch (LangMgr.DefaultLanguage)
            {
                case Lang.English:
                    LangField = "EngName";
                    break;
                case Lang.Nepali:
                    LangField = "NepName";
                    break;
            }

            #endregion



            try
            {
                Global.m_db.BeginTransaction();

                //Update the AccessRole Name
                Global.m_db.InsertUpdateQry("UPDATE System.tblAccessRole SET " + LangField + " = '" + NewRoleName + "' WHERE RoleID='" + AccessRoleID + "'");

                //Delete all the previous records from tblAccessRoleDtl
                Global.m_db.InsertUpdateQry("DELETE FROM System.tblAccessRoleDtl WHERE RoleID='" + AccessRoleID + "'");

                //Now insert the new ones
                foreach (int ID in AccessID)
                {
                    Global.m_db.InsertUpdateQry("INSERT INTO System.tblAccessRoleDtl(RoleID, AccessID) VALUES('" + AccessRoleID + "','" + ID + "')");
                }
                Global.m_db.CommitTransaction();


            }
            catch (Exception ex)
            {
                Global.m_db.RollBackTransaction();

                throw ex;
            }
        }



        public static DataTable GetAccessRoleID(string RoleName)
        {
            return Global.m_db.SelectQry("SELECT * from System.tblAccessRole where EngName='" + RoleName + "'","tblAccessRole");
        }

        public static DataTable GetAcessRoleInfo(int AccessRoleID)
        {

            if (AccessRoleID == 0)
                return Global.m_db.SelectQry("SELECT * FROM System.tblAccessRole", "Table");
            else
                return Global.m_db.SelectQry("SELECT * FROM System.tblAccessRole WHERE RoleID='" + AccessRoleID.ToString() + "'", "Table");
        }


        public static  DataTable CheckUserAvailability(string  userName, string Password)
        {
            Global.m_db.ClearParameter();
            Global.m_db.setCommandType(CommandType.StoredProcedure);
            Global.m_db.setCommandText("System.spCheckUserAvailability");
            Global.m_db.AddParameter("@username", SqlDbType.NVarChar,200, userName);
            Global.m_db.AddParameter("@password", SqlDbType.NVarChar, 200, Password);
            DataTable dtUserInfo = Global.m_db.GetDataTable();
            return dtUserInfo;
        }


        public static DataTable GetAccessRoleDetails(int AccessRoleID)
        {
            return Global.m_db.SelectQry("SELECT RoleID, AccessID FROM System.tblAccessRoleDtl  WHERE RoleID='" + AccessRoleID.ToString() + "'", "tblAccessRoles");
        }

        
        public static DataTable GetAccessInfo(string accessId)
        {
            DataTable dt = new DataTable();
            if (accessId == "")
            {
                string strQuery = "SELECT * FROM System.tblAccess";
                dt = Global.m_db.SelectQry(strQuery, "tbl");
                return dt;

            }
            else
            {
                string strQuery1 = "SELECT * FROM System.tblAccess WHERE AccessID ='" + accessId + "'";
                dt = Global.m_db.SelectQry(strQuery1, "dt");
                return dt;

            }
            return dt;
        }


        /// <summary>
        /// Checks whether the access role has been used by the user or not
        /// </summary>
        /// <param name="accessRoleId"></param>
        /// <returns></returns>
        public static DataTable CheckAccessRoleUsedByUser(string accessRoleId)
        {
            string strQuery = "SELECT * FROM System.tblUser WHERE AccessRoleID ='" + accessRoleId + "'";
            DataTable dt = Global.m_db.SelectQry(strQuery, "dt");
            return dt;
        }


        public void DeleteAccessRole(string accessRoleId)
        {
            string strQuery1 = "DELETE FROM System.tblAccessRoleDtl WHERE RoleID ='" + accessRoleId + "'";
            Global.m_db.InsertUpdateQry(strQuery1);
            //Delete the record of user if this user is accessing this AccessRole
            //First check 
            string strQuery = "DELETE FROM System.tblAccessRole WHERE RoleID ='" + accessRoleId + "' ";
            Global.m_db.InsertUpdateQry(strQuery);
        }

        public static string Encrypt(string password)
        {
            MD5CryptoServiceProvider hasher = new MD5CryptoServiceProvider();
            byte[] tBytes = Encoding.ASCII.GetBytes(password);
            byte[] hBytes = hasher.ComputeHash(tBytes);

            StringBuilder sb = new StringBuilder();
            for (int c = 0; c < hBytes.Length; c++)
                sb.AppendFormat("{0:x2}", hBytes[c]);

            return (sb.ToString());
        }



    }
}
