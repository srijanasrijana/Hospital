using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;
using DBLogic;
using System.Windows.Forms;
using DateManager;


namespace BusinessLogic
{

    public interface IAccountClass
    {
        void Create(string Name, int ParentID, string Remarks,string oldaccdetails,string newaccdetails,bool isnew);
        void Create(string Name, string ParentName, string Remarks);
        void Modify(int OldAccClassID, string NewGroupName, int? ParentID, string Remarks,string oldaccdetails,string newaccdetails,bool isnew);
        void Modify(int OldAccClassID, string NewGroupName, string ParentName, string Remarks);
        void Delete(int AccClassID);
    }

    public class AccountClass : IAccountClass
    {


        public void Create(string Name, int ParentID, string Remarks,string oldaccdetails,string newaccdetails,bool isnew)
        {
            try
            {
                // Global.m_db.InsertUpdateQry("INSERT INTO Account.tblAccountHead(AccountHeadName,Under_Account_ID,Description,BuiltIn) VALUES ('" + AccountHeadName + "','1','" + Description + "','0')");
                string Language="ENGLISH";
                if(LangMgr.DefaultLanguage==Lang.English)
                    Language="ENGLISH";
                else if(LangMgr.DefaultLanguage==Lang.Nepali)
                    Language="NEPALI";


                Global.m_db.ClearParameter();
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("Acc.spAccClassCreate");
                Global.m_db.AddParameter("@Name", SqlDbType.NVarChar, 50, Name);
                Global.m_db.AddParameter("@Language", SqlDbType.NVarChar, 50, Language);
                Global.m_db.AddParameter("@ParentID", SqlDbType.Int, ParentID);
                Global.m_db.AddParameter("@Remarks", SqlDbType.NVarChar, 200, Remarks);
               // Global.m_db.AddParameter("@Compulsory", SqlDbType.Bit, Compulson);
                object objReturn = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                Global.m_db.ProcessParameter();
                try
                {
                if (isnew == true)
                {
                    string username = User.CurrentUserName;
                    string voucherdate = Date.ToDB(DateTime.Now).ToString();
                    string VoucherType = "ACLASS";
                    string action = "INSERT";
                    int rowid = 0;
                    string ComputerName = Global.ComputerName;
                    string MacAddress = Global.MacAddess;
                    string IpAddress = Global.IpAddress;
                    string desc = oldaccdetails+newaccdetails;

                    Global.m_db.ClearParameter();
                    Global.m_db.setCommandType(CommandType.StoredProcedure);
                    Global.m_db.setCommandText("system.spAddAuditLog");
                    Global.m_db.AddParameter("@ComputerName", SqlDbType.NVarChar, 50, ComputerName);
                    Global.m_db.AddParameter("@UserName", SqlDbType.NVarChar, 50, username);//Set same for both for time being
                    Global.m_db.AddParameter("@Action", SqlDbType.NVarChar, 50, action);
                    Global.m_db.AddParameter("@Description", SqlDbType.NVarChar, 4000, desc);
                    Global.m_db.AddParameter("@RowID", SqlDbType.Int, rowid);
                    Global.m_db.AddParameter("@MAC_Address", SqlDbType.NVarChar, 50, MacAddress);
                    Global.m_db.AddParameter("@IP_Address", SqlDbType.NVarChar, 50, IpAddress);
                    Global.m_db.AddParameter("@Voucher_Type", SqlDbType.NVarChar, 50, VoucherType);
                    Global.m_db.AddParameter("@VoucherDate", SqlDbType.NVarChar, 200, voucherdate);
                    object objReturn1 = Global.m_db.AddOutputParameter("@Result", SqlDbType.NVarChar, 20);
                    Global.m_db.ProcessParameter();
                  
                }
                else if (isnew == false)
                {
                    string username = User.CurrentUserName;
                    string voucherdate = Date.ToDB(DateTime.Now).ToString();
                    string VoucherType = "ACLASS";
                    string action = "UPDATE";
                    int rowid = 0;
                    string ComputerName = Global.ComputerName;
                    string MacAddress = Global.MacAddess;
                    string IpAddress = Global.IpAddress;
                    string desc = oldaccdetails+newaccdetails;

                    Global.m_db.ClearParameter();
                    Global.m_db.setCommandType(CommandType.StoredProcedure);
                    Global.m_db.setCommandText("system.spAddAuditLog");
                    Global.m_db.AddParameter("@ComputerName", SqlDbType.NVarChar, 50, ComputerName);
                    Global.m_db.AddParameter("@UserName", SqlDbType.NVarChar, 50, username);//Set same for both for time being
                    Global.m_db.AddParameter("@Action", SqlDbType.NVarChar, 50, action);
                    Global.m_db.AddParameter("@Description", SqlDbType.NVarChar, 4000, desc);
                    Global.m_db.AddParameter("@RowID", SqlDbType.Int, rowid);
                    Global.m_db.AddParameter("@MAC_Address", SqlDbType.NVarChar, 50, MacAddress);
                    Global.m_db.AddParameter("@IP_Address", SqlDbType.NVarChar, 50, IpAddress);
                    Global.m_db.AddParameter("@Voucher_Type", SqlDbType.NVarChar, 50, VoucherType);
                    Global.m_db.AddParameter("@VoucherDate", SqlDbType.NVarChar, 200, voucherdate);
                    object objReturn1 = Global.m_db.AddOutputParameter("@Result", SqlDbType.NVarChar, 20);
                    Global.m_db.ProcessParameter();
                }
            }
            catch(Exception ex)
            {
                 Global.MsgError(ex.Message);
            }
            }
            catch (Exception)
            {

                throw;

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
        public void Create(string Name, string ParentName, string Remarks)
        {

            string Language = "ENGLISH";
            if (LangMgr.DefaultLanguage == Lang.English)
                Language = "ENGLISH";
            else if (LangMgr.DefaultLanguage == Lang.Nepali)
                Language = "NEPALI";
            try
            {
                // Global.m_db.InsertUpdateQry("INSERT INTO Account.tblAccountHead(AccountHeadName,Under_Account_ID,Description,BuiltIn) VALUES ('" + AccountHeadName + "','1','" + Description + "','0')");

                Global.m_db.ClearParameter();
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("Acc.spAccClassCreate");
                Global.m_db.AddParameter("@Name", SqlDbType.NVarChar, 50, Name);
                Global.m_db.AddParameter("@Language", SqlDbType.NVarChar, 50, Language);
                Global.m_db.AddParameter("@ParentName", SqlDbType.NVarChar, 50, ParentName);
                Global.m_db.AddParameter("@Remarks", SqlDbType.NVarChar, 200, Remarks);
               // Global.m_db.AddParameter("@Compulsory", SqlDbType.Bit, Compulson);
                object objReturn = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                Global.m_db.ProcessParameter();

            }
            catch (Exception)
            {
                throw;

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
        /// Returns the root class ID of the given child ID
        /// </summary>
        /// <param name="ChildAccClassID"></param>
        /// <returns></returns>
        public static DataTable GetRootAccClass(int ChildAccClassID)//Cnanged for only to filter according to accclass
        {
            try
            {
                // Global.m_db.InsertUpdateQry("INSERT INTO Account.tblAccountHead(AccountHeadName,Under_Account_ID,Description,BuiltIn) VALUES ('" + AccountHeadName + "','1','" + Description + "','0')");
                if (ChildAccClassID == -1)//List all Root Acc Classes
                {
                    string strQuery = "SELECT * FROM " + "Acc.tblAccClass ";//Changed for to filter according to accclass----new one
                   // string strQuery = "SELECT * FROM " + "Acc.tblAccClass WHERE ParentID is null";--old one
                    return Global.m_db.SelectQry(strQuery, "tblAccClass");
                }

                //Else list specific
                Global.m_db.ClearParameter();
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("Acc.spAccClassFindRoot");
                Global.m_db.AddParameter("@AccClassID", SqlDbType.Int, ChildAccClassID);
                return Global.m_db.GetDataTable();

            }
            catch (Exception)
            {
                throw;

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
        /// Gets all the Account Class informations under the given parent ID. If parent ID is 0 it returns only parent group information.
        /// If you want to get all classes, introduce ParentGroupID as -1
        /// </summary>
        /// <param name="ParentGroupID"></param>
        /// <returns></returns>
        public static DataTable GetAccClassTable(int ParentClassID)
        {
            if (ParentClassID == 0)//Only Parent AccClass
            {
                return Global.m_db.SelectQry("SELECT * FROM Acc.tblAccClass WHERE ParentID is null", "tblAccClass");
            }
            else if (ParentClassID == -1)//All AccClass
            {
                return Global.m_db.SelectQry("SELECT * FROM Acc.tblAccClass", "tblAccClass");
            }
            else
            {
                return Global.m_db.SelectQry("SELECT * FROM Acc.tblAccClass WHERE ParentID =" + ParentClassID.ToString(), "tblAccClass");
            }
        }

        public static DataTable GetAccClassTable1(int ParentClassID)
        {
           
           return Global.m_db.SelectQry("SELECT * FROM Acc.tblAccClass WHERE AccClassID =" + ParentClassID.ToString(), "tblAccClass");
           
        }

        public static DataTable GetAccClassTable(string AccClassName)
        {
            if (LangMgr.DefaultLanguage == Lang.English)
                return Global.m_db.SelectQry("SELECT * FROM Acc.tblAccClass WHERE EngName ='" + AccClassName.ToString()+ "'", "tblGroup");
            else if (LangMgr.DefaultLanguage == Lang.Nepali)
                return Global.m_db.SelectQry("SELECT * FROM Acc.tblAccClass WHERE NepName ='" + AccClassName.ToString() + "'", "tblGroup");

            return null;
        }

        /// <summary>
        /// Returns a row of accounting class by its id
        /// </summary>
        /// <param name="accClassID"></param>
        /// <returns></returns>
        public static DataTable GetAccClassTableByID(int accClassID)
        {
            return Global.m_db.SelectQry("SELECT * FROM Acc.tblAccClass WHERE AccClassID ='" + accClassID.ToString() + "'", "tblAccClass");
            
        }

        public static string GetNameFromID(int AccClassID)
        {
            string LangField = "EngName";
            if (LangMgr.DefaultLanguage == Lang.English)
                LangField = "EngName";
            else if (LangMgr.DefaultLanguage == Lang.Nepali)
                LangField = "NepName";

            return Global.m_db.GetScalarValue("SELECT " + LangField + " FROM Acc.tblAccClass WHERE AccClassID=" + AccClassID.ToString()).ToString();
        }

        public void Modify(int OldAccClassID, string NewGroupName, string ParentName, string Remarks)
        {
            //Find which language is there
            string CurrLang;
            switch (LangMgr.DefaultLanguage)
            {
                case Lang.English:
                    CurrLang = "ENGLISH";
                    break;
                case Lang.Nepali:
                    CurrLang = "NEPALI";
                    break;
                default:
                    CurrLang = "ENGLISH";
                    break;
            }

            try
            {
                Global.m_db.ClearParameter();
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("Acc.spAccClassModify");
                Global.m_db.AddParameter("@OldID", SqlDbType.Int, OldAccClassID);
                Global.m_db.AddParameter("@Name", SqlDbType.NVarChar, 50, NewGroupName);
                Global.m_db.AddParameter("@Language", SqlDbType.NVarChar, 50, CurrLang);
                Global.m_db.AddParameter("@ParentGroup", SqlDbType.NVarChar, 50, ParentName);
                Global.m_db.AddParameter("@Modified_By", SqlDbType.NVarChar, 20, User.CurrUserID.ToString());
                Global.m_db.AddParameter("@Remarks", SqlDbType.NVarChar, 200, Remarks);
                //Global.m_db.AddParameter("@Compulsory", SqlDbType.Bit, Compulson);
                object objReturn = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                Global.m_db.ProcessParameter();
            }
            catch (Exception ex)
            {
                throw ex;
            }
 
        }

        public void Modify(int OldAccClassID, string NewGroupName, int? ParentID, string Remarks,string oldaccdetails,string newaccdetails,bool isnew)
        {
            //Find which language is there
            string CurrLang;
            switch (LangMgr.DefaultLanguage)
            {
                case Lang.English:
                    CurrLang = "ENGLISH";
                    break;
                case Lang.Nepali:
                    CurrLang = "NEPALI";
                    break;
                default:
                    CurrLang = "ENGLISH";
                    break;
            }
            try
            {
                Global.m_db.ClearParameter();
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("Acc.spAccClassModify");
                Global.m_db.AddParameter("@OldID", SqlDbType.Int, OldAccClassID);
                Global.m_db.AddParameter("@Name", SqlDbType.NVarChar, 50, NewGroupName);
                Global.m_db.AddParameter("@Language", SqlDbType.NVarChar, 50, CurrLang);
                Global.m_db.AddParameter("@ParentID", SqlDbType.Int, ParentID.Value >0 ? ParentID.Value : (object)DBNull.Value);//IF ---No Parent--- is selected, than null is inserted to ParentID
                Global.m_db.AddParameter("@Modified_By", SqlDbType.NVarChar, 20, User.CurrUserID.ToString());
                Global.m_db.AddParameter("@Remarks", SqlDbType.NVarChar, 200, Remarks);
               // Global.m_db.AddParameter("@Compulsory", SqlDbType.Bit, Compulson);
                object objReturn = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                Global.m_db.ProcessParameter();
                 try
                {
                if (isnew == true)
                {
                    string username = User.CurrentUserName;
                    string voucherdate = Date.ToDB(DateTime.Now).ToString();
                    string VoucherType = "ACLASS";
                    string action = "INSERT";
                    int rowid = 0;
                    string ComputerName = Global.ComputerName;
                    string MacAddress = Global.MacAddess;
                    string IpAddress = Global.IpAddress;
                    string desc = oldaccdetails+newaccdetails;
                      
                    Global.m_db.ClearParameter();
                    Global.m_db.setCommandType(CommandType.StoredProcedure);
                    Global.m_db.setCommandText("system.spAddAuditLog");
                    Global.m_db.AddParameter("@ComputerName", SqlDbType.NVarChar, 50, ComputerName);
                    Global.m_db.AddParameter("@UserName", SqlDbType.NVarChar, 50, username);//Set same for both for time being
                    Global.m_db.AddParameter("@Action", SqlDbType.NVarChar, 50, action);
                    Global.m_db.AddParameter("@Description", SqlDbType.NVarChar, 4000, desc);
                    Global.m_db.AddParameter("@RowID", SqlDbType.Int, rowid);
                    Global.m_db.AddParameter("@MAC_Address", SqlDbType.NVarChar, 50, MacAddress);
                    Global.m_db.AddParameter("@IP_Address", SqlDbType.NVarChar, 50, IpAddress);
                    Global.m_db.AddParameter("@Voucher_Type", SqlDbType.NVarChar, 50, VoucherType);
                    Global.m_db.AddParameter("@VoucherDate", SqlDbType.NVarChar, 200, voucherdate);
                    object objReturn1 = Global.m_db.AddOutputParameter("@Result", SqlDbType.NVarChar, 20);
                    Global.m_db.ProcessParameter();
                  
                }
                else if (isnew == false)
                {
                    string username = User.CurrentUserName;
                    string voucherdate = Date.ToDB(DateTime.Now).ToString();
                    string VoucherType = "ACLASS";
                    string action = "UPDATE";
                    int rowid = 0;
                    string ComputerName = Global.ComputerName;
                    string MacAddress = Global.MacAddess;
                    string IpAddress = Global.IpAddress;
                    string desc = oldaccdetails+newaccdetails;

                    Global.m_db.ClearParameter();
                    Global.m_db.setCommandType(CommandType.StoredProcedure);
                    Global.m_db.setCommandText("system.spAddAuditLog");
                    Global.m_db.AddParameter("@ComputerName", SqlDbType.NVarChar, 50, ComputerName);
                    Global.m_db.AddParameter("@UserName", SqlDbType.NVarChar, 50, username);//Set same for both for time being
                    Global.m_db.AddParameter("@Action", SqlDbType.NVarChar, 50, action);
                    Global.m_db.AddParameter("@Description", SqlDbType.NVarChar, 4000, desc);
                    Global.m_db.AddParameter("@RowID", SqlDbType.Int, rowid);
                    Global.m_db.AddParameter("@MAC_Address", SqlDbType.NVarChar, 50, MacAddress);
                    Global.m_db.AddParameter("@IP_Address", SqlDbType.NVarChar, 50, IpAddress);
                    Global.m_db.AddParameter("@Voucher_Type", SqlDbType.NVarChar, 50, VoucherType);
                    Global.m_db.AddParameter("@VoucherDate", SqlDbType.NVarChar, 200, voucherdate);
                    object objReturn1 = Global.m_db.AddOutputParameter("@Result", SqlDbType.NVarChar, 20);
                    Global.m_db.ProcessParameter();
                }
            }
            catch(Exception ex)
            {
                 Global.MsgError(ex.Message);
            }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Delete(int AccClassID)
        {
            Global.m_db.SelectQry("DELETE FROM Acc.tblAccClass WHERE AccClassID='" + AccClassID + "'","Acc.tblGroup");
        }

        /// <summary>
        /// Returns the table of accounts which falls under given GroupID and fills the result in ReturnTable. Recursive function
        /// </summary>
        /// <param name="GroupID"></param>
        /// <param name="ReturnTable"></param>
        public static void GetChildIDs(int AccClassID, ref ArrayList ReturnChildIDs)
        {
            DataTable dt;
           // int PreviousAccClass = 0;
            //ArrayList ReturnChildIDs = new ArrayList();
            dt = AccountClass.GetAccClassTable(AccClassID);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];
                ReturnChildIDs.Add(dr["AccClassID"]);
                //PreviousAccClass = Convert.ToInt32(dr["AccClassID"]);
                GetChildIDs(Convert.ToInt32(dr["AccClassID"].ToString()),ref ReturnChildIDs);
            }
            //return ReturnChildIDs;
        }

        public static DataTable GetTransactClassInfo(int RowID, string VoucherType)
        {
            return Global.m_db.SelectQry("SELECT DISTINCT(AccClassID) FROM Acc.tblTransactionClass WHERE RowID ='" + RowID + "' AND VoucherType='" + VoucherType + "'", "table");
        }
        /// <summary>
        /// Returns the Transaction IDs from Accounting Class IDs
        /// </summary>
        /// <param name="AccClassID"></param>
        /// <returns></returns>
        public static string GetTransactIDFromAccClassID(ArrayList AccClassID)
        {
            string AccClassID1 = "";
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
            string strQuery = "SELECT * FROM Acc.tblTransactionClass WHERE AccClassID IN (" + (AccClassID1) + ")";
            DataTable dtTransactIDsByAccClass = Global.m_db.SelectQry(strQuery, "AccClassID");

            string TransactionIDsByAccClass = "";
            int i1 = 0;
            foreach (DataRow dr in dtTransactIDsByAccClass.Rows)
            {
                if (i1 == 0)// for first TransactionID
                    TransactionIDsByAccClass = "'" + dr["TransactionID"].ToString() + "'";
                else  //Separating Other TransactionID by commas
                    TransactionIDsByAccClass += "," + "'" + dr["TransactionID"].ToString() + "'";
                i1++;
            }

            return TransactionIDsByAccClass;
        }

        public static string GetTransactIDFromAccClassIDAndProjectID(ArrayList AccClassID,ArrayList ProjectID)
        {
            string AccClassID1 = "";
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

            string ProjectID1 = "";
            int p = 0;
            if (ProjectID != null)
            {
                foreach (object r in ProjectID)
                {
                    if (p == 0)// for first GroupID
                        ProjectID1 = "'" + r.ToString() + "'";
                    else  //Separating Other GroupID by commas
                        ProjectID1 += "," + "'" + r.ToString() + "'";
                    p++;
                }
            }

            if (ProjectID1=="")
            {
                ProjectID1 = "1";
            }
            if (AccClassID1 == "")
                AccClassID1 = "1";
            string strQuery = "SELECT TC.* FROM Acc.tblTransactionClass TC,Acc.tblTransaction T WHERE TC.AccClassID IN (" + (AccClassID1) + ") and T.ProjectID IN (" + (ProjectID1) + ") and T.TransactionID=TC.TransactionID";
            DataTable dtTransactIDsByAccClass = Global.m_db.SelectQry(strQuery, "AccClassID");

            string TransactionIDsByAccClass = "";
            int i1 = 0;
            foreach (DataRow dr in dtTransactIDsByAccClass.Rows)
            {
                if (i1 == 0)// for first TransactionID
                    TransactionIDsByAccClass = "'" + dr["TransactionID"].ToString() + "'";
                else  //Separating Other TransactionID by commas
                    TransactionIDsByAccClass += "," + "'" + dr["TransactionID"].ToString() + "'";
                i1++;
            }

            return TransactionIDsByAccClass;
        }


        public static DataTable GetAccClassInfo(int MasterID, string VocherType)
        {
            string strQuery = "SELECT DISTINCT (AccClassID) FROM " +
                             "Acc.tblTransactionClass WHERE RowID='" + MasterID + "' AND " +
                             "VoucherType='" + VocherType + "'";
            return Global.m_db.SelectQry(strQuery, "tblTransactionClass");

        }

        public static DataTable GetPOAccClassInfo(int MasterID)
        {
            string strQuery = "SELECT DISTINCT (AccClassID) FROM " +
                             "Inv.tblPurchaseOrderAccClass WHERE PurchaseOrderID='" + MasterID + "' ";

            return Global.m_db.SelectQry(strQuery, "tblPurchaseOrderAccClass");

        }

        public static DataTable GetAllAccClass()
        {
            string strQuery = "select * from Acc.tblAccClass";
            return Global.m_db.SelectQry(strQuery, "tblAccClass");

        }

        //public static bool IsParent(string ParentClassID)
        //{
        //    DataTable dt = new DataTable();
        //    //if (ParentClassID == "")
        //    //    return true;

        //    //DataTable dt = Global.m_db.SelectQry("SELECT * FROM Acc.tblAccClass WHERE AccClassID =" + ParentClassID + " and ParentID is null", "tblAccClass");           
        //    //if (dt.Rows.Count == 1)
        //    //    return true;                       
        //    //else
        //        //dt = Global.m_db.SelectQry("SELECT * FROM Acc.tblAccClass WHERE AccClassID =" + ParentClassID , "tblAccClass");
        //        //foreach (DataRow dr in dt.Rows)
        //        //{
        //        //    if(dr["ParentID"] = DBNull.Value)
        //        //}
        //        //return true;
        //}
    }
}

