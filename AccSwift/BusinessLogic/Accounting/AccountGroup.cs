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
    public interface IAccountGroup
    {
        void Create(string LedgerCode, string GroupName, string ParentGroup, string Remarks,string oldacgroup,string newacgroup,bool isnew);
        void Modify(string LedgerCode, int GroupID, string NewGroupName, string ParentGroupName, string Remarks,string oldacgroup,string newacgroup,bool isnew);
        void Delete(string AccountHeadName);
    }

  

    public class AccountGroup:IAccountGroup
    {


        public void Create(string LedgerCode, string GroupName, string ParentGroup, string Remarks, string oldacgroup, string newacgroup, bool isnew)
        {
            try
            {
               // Global.m_db.InsertUpdateQry("INSERT INTO Account.tblAccountHead(AccountHeadName,Under_Account_ID,Description,BuiltIn) VALUES ('" + AccountHeadName + "','1','" + Description + "','0')");
                Global.m_db.ClearParameter();
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("Acc.spGroupCreate");
                Global.m_db.AddParameter("@LedgerCode", SqlDbType.NVarChar, 50, LedgerCode);
                Global.m_db.AddParameter("@EngName", SqlDbType.NVarChar, 50, GroupName);
                Global.m_db.AddParameter("@NepName", SqlDbType.NVarChar, 50, GroupName);//Set same for both for time being
                Global.m_db.AddParameter("@Under_EngName", SqlDbType.NVarChar, 50, ParentGroup);
                Global.m_db.AddParameter("@Remarks", SqlDbType.NVarChar, 200, Remarks);
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
            try
            {
                if (isnew == true)
                {
                    string username = User.CurrentUserName;
                    string voucherdate = Date.ToDB(DateTime.Now).ToString();
                    string VoucherType = "AGROUP";
                    string action = "INSERT";
                    int rowid = 0;
                    string ComputerName = Global.ComputerName;
                    string MacAddress = Global.MacAddess;
                    string IpAddress = Global.IpAddress;
                    string desc = oldacgroup + newacgroup;

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
                    //string SQL = "INSERT INTO System.tblAuditLog(computername,username,voucher_type,action,rowid,description,voucherdate) VALUES ('" + ComputerName + "','" + username + "','" + VoucherType + "','" + action + "','" + rowid + "','" + desc + "','" + Date.ToDB(DateTime.Now).ToString() + "')";
                    //Global.m_db.InsertUpdateQry(SQL);
                }
                else if (isnew == false)
                {
                    string username = User.CurrentUserName;
                    string voucherdate = Date.ToDB(DateTime.Now).ToString();
                    string VoucherType = "AGROUP";
                    string action = "UPDATE";
                    int rowid = 0;
                    string ComputerName = Global.ComputerName;
                    string MacAddress = Global.MacAddess;
                    string IpAddress = Global.IpAddress;
                    string desc = oldacgroup + newacgroup;

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
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }
            //try
            //{
            //    if (isnew == true)
            //    {
            //        string username = User.CurrentUserName;
            //        string voucherdate = Convert.ToString(DateTime.Now);
            //        string VoucherType = "AGROUP";
            //        string action = "INSERT";
            //        int rowid = 0;
            //        string ComputerName = Global.ConcatAllCompInfo;
            //        string desc = oldacgroup + newacgroup;
            //        string SQL = "INSERT INTO System.tblAuditLog(computername,username,voucher_type,action,rowid,description,voucherdate) VALUES ('" + ComputerName + "','" + username + "','" + VoucherType + "','" + action + "','" + rowid + "','" + desc + "','" + Date.ToDB(DateTime.Now).ToString() +"')";
            //        Global.m_db.InsertUpdateQry(SQL);
            //    }
            //    else if (isnew == false)
            //    {
            //        string username = User.CurrentUserName;
            //        string voucherdate = Convert.ToString(DateTime.Now);
            //        string VoucherType = "AGROUP";
            //        string action = "UPDATE";
            //        int rowid = 0;
            //        string ComputerName = Global.ConcatAllCompInfo;
            //        string desc = oldacgroup + newacgroup;
            //        string SQL = "INSERT INTO System.tblAuditLog(computername,username,voucher_type,action,rowid,description,voucherdate) VALUES ('" + ComputerName + "','" + username + "','" + VoucherType + "','" + action + "','" + rowid + "','" + desc + "','" + Date.ToDB(DateTime.Now).ToString() + "')";
            //        Global.m_db.InsertUpdateQry(SQL);
            //    }
            //}
            //catch(Exception ex)
            //{
            //    Global.MsgError(ex.Message);
            //}
        }

        public void Modify(string LedgerCode, int GroupID, string NewGroupName, string ParentGroupName, string Remarks,string oldacgroup,string newacgroup,bool isnew)
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
                Global.m_db.setCommandText("Acc.spGroupModify");
                Global.m_db.AddParameter("@LedgerCode", SqlDbType.NVarChar, 50, LedgerCode);
                Global.m_db.AddParameter("@OldID", SqlDbType.Int, GroupID);
                Global.m_db.AddParameter("@NewGroupName", SqlDbType.NVarChar, 50, NewGroupName);
                Global.m_db.AddParameter("@ParentGroup", SqlDbType.NVarChar, 50, ParentGroupName);
                Global.m_db.AddParameter("@Lang", SqlDbType.NVarChar, 20, "ENGLISH");
                Global.m_db.AddParameter("@User", SqlDbType.NVarChar, 20, User.CurrUserID.ToString());
                Global.m_db.AddParameter("@Remarks", SqlDbType.NVarChar, 200, Remarks);
                object objReturn = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                Global.m_db.ProcessParameter();

              
            }
            catch (Exception ex)
            {
                throw ex;
            }
            try
            {
                if (isnew == true)
                {
                    string username = User.CurrentUserName;
                    string voucherdate = Date.ToDB(DateTime.Now).ToString();
                    string VoucherType = "AGROUP";
                    string action = "INSERT";
                    int rowid = 0;
                    string ComputerName = Global.ComputerName;
                    string MacAddress = Global.MacAddess;
                    string IpAddress = Global.IpAddress;
                    string desc = oldacgroup + newacgroup;

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
                    //string SQL = "INSERT INTO System.tblAuditLog(computername,username,voucher_type,action,rowid,description,voucherdate) VALUES ('" + ComputerName + "','" + username + "','" + VoucherType + "','" + action + "','" + rowid + "','" + desc + "','" + Date.ToDB(DateTime.Now).ToString() + "')";
                    //Global.m_db.InsertUpdateQry(SQL);
                }
                else if (isnew == false)
                {
                    string username = User.CurrentUserName;
                    string voucherdate = Date.ToDB(DateTime.Now).ToString();
                    string VoucherType = "AGROUP";
                    string action = "UPDATE";
                    int rowid = 0;
                    string ComputerName = Global.ComputerName;
                    string MacAddress = Global.MacAddess;
                    string IpAddress = Global.IpAddress;
                    string desc = oldacgroup + newacgroup;

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
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }
            //try
            //{
            //    if (isnew == true)
            //    {
            //        string username = User.CurrentUserName;
            //        string voucherdate = Convert.ToString(DateTime.Now);
            //        string VoucherType = "AGROUP";
            //        string action = "INSERT";
            //        int rowid = 0;
            //        string ComputerName = Global.ConcatAllCompInfo;
            //        string desc = oldacgroup + newacgroup;
            //        string SQL = "INSERT INTO System.tblAuditLog(computername,username,voucher_type,action,rowid,description,voucherdate) VALUES ('" + ComputerName + "','" + username + "','" + VoucherType + "','" + action + "','" + rowid + "','" + desc + "','" + Date.ToDB(DateTime.Now).ToString() + "')";
            //        Global.m_db.InsertUpdateQry(SQL);
            //    }
            //    else if (isnew == false)
            //    {
            //        string username = User.CurrentUserName;
            //        string voucherdate = Convert.ToString(DateTime.Now);
            //        string VoucherType = "AGROUP";
            //        string action = "UPDATE";
            //        int rowid = 0;
            //        string ComputerName = Global.ConcatAllCompInfo;
            //        string desc = oldacgroup + newacgroup;
            //        string SQL = "INSERT INTO System.tblAuditLog(computername,username,voucher_type,action,rowid,description,voucherdate) VALUES ('" + ComputerName + "','" + username + "','" + VoucherType + "','" + action + "','" + rowid + "','" + desc + "','" + Date.ToDB(DateTime.Now).ToString() + "')";
            //        Global.m_db.InsertUpdateQry(SQL);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Global.MsgError(ex.Message);
            //}
 
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
            object objResult = Global.m_db.GetScalarValue("SELECT BuiltIn FROM Acc.tblGroup WHERE " + LangField + "='" + GroupName + "'");
            return Convert.ToInt32(objResult);
        }

        public void Delete(string GroupName)
        {
            //Language Management
            string LangField = "EngName";
            if (LangMgr.DefaultLanguage == Lang.English)
                LangField = "EngName";
            else if (LangMgr.DefaultLanguage == Lang.Nepali)
                LangField = "NepName";
            GroupName = GroupName.Replace("'", "''");
            Global.m_db.SelectQry("DELETE FROM Acc.tblGroup WHERE "+ LangField + "='" + GroupName + "'","Acc.tblGroup");

        }

        public static int GetIDFromName(string Name, Lang Language)
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

            Name = Name.Replace("'","''");
            object objResult = Global.m_db.GetScalarValue("SELECT GroupID FROM Acc.tblGroup WHERE " + LangField + "='" + Name + "'");
            return Convert.ToInt32(objResult);
        }


        /// <summary>
        /// Returns the ID of the Group with a given Accounting Type like Assets, Liabilities, Income and Expenditure
        /// </summary>
        /// <param name="AccType"></param>
        /// <returns></returns>
        public static int GetIDFromType(AccountType AccType)
        {
            object objResult = Global.m_db.GetScalarValue("SELECT GroupID FROM Acc.tblGroup WHERE AccountType='" + AccType.ToString() + "'");
            int value = Convert.ToInt32(objResult);
            return Convert.ToInt32(objResult);
        }

        /// <summary>
        /// Gets all the Group informations under the given parent ID. If parent ID is 0 it returns only parent group information.
        /// If you want to get all groups, introduce ParentGroupID as -1
        /// </summary>
        /// <param name="ParentGroupID"></param>
        /// <returns></returns>
        public static DataTable GetGroupTable(int ParentGroupID)
        {
            DataTable dt = new DataTable();
            string strQuery = "";
            if (ParentGroupID == 0)//Only Parent group
            {
                strQuery = "SELECT * FROM Acc.tblGroup WHERE Parent_GrpID is null"; 
            }
            else if (ParentGroupID == -1)//All Groups
            {
                strQuery = "SELECT * FROM Acc.tblGroup";
            }
            else
            {
                // here i made change to the Parnt_GrpID to GroupID////////// 
                strQuery = "SELECT * FROM Acc.tblGroup WHERE Parent_GrpID =" + ParentGroupID.ToString();
            }
            return Global.m_db.SelectQry(strQuery, "tblGroup");

        }

        public static DataSet GetDataSetForAccountTreeView()
        {
            DataSet ds = new DataSet();
            DataTable dtGroup = new DataTable("tblGroup");
            dtGroup = Global.m_db.SelectQry("select GroupID,EngName as GroupName,Parent_GrpID as ParentID from acc.tblGroup ", "tblGroup");
            ds.Tables.Add(dtGroup);
         
            DataTable dtLedger = new DataTable("tblLedger");
            dtLedger = Global.m_db.SelectQry("select LedgerID,EngName as LedgerName,GroupID as ParentID from acc.tblLedger", "tblLedger");
            ds.Tables.Add(dtLedger);

            ds.Relations.Add("ChildGroup", ds.Tables[0].Columns["GroupID"], ds.Tables[0].Columns["ParentID"],false);
            ds.Relations.Add("ChildLedger", ds.Tables[0].Columns["GroupID"], ds.Tables[1].Columns["ParentID"],false);

            return ds;


        }
        /// <summary>
        /// Returns the table of accounts which falls under given GroupID and fills the result in ReturnTable. Recursive function
        /// </summary>
        /// <param name="GroupID"></param>
        /// <param name="ReturnTable"></param>
        public static void GetAccountsUnder(int GroupID, ArrayList ReturnIDs)
        {
            #region Language Management

            //tv.Font = LangMgr.GetFont();

            string LangField = "EngName";
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
            
            DataTable dt;

            dt = AccountGroup.GetGroupTable(GroupID);


            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];

                ReturnIDs.Add(dr["GroupID"]);
                GetAccountsUnder(Convert.ToInt32(dr["GroupID"].ToString()), ReturnIDs);
            }
        }

        //for budgetallocation only expenditure
        public static string[] GetAllGroupsUnderExpenditure()
        {
            try
            {
                List<string> lstGroupID = new List<string>();
                lstGroupID.Add("4");
                ArrayList tmpGroupID = new ArrayList();
                GetAccountsUnder(4, tmpGroupID);

                foreach (object j in tmpGroupID)
                {
                    lstGroupID.Add(j.ToString());
                }

                string[] strResult = (string[])lstGroupID.ToArray();
                if (strResult.Count<string>() <= 0)
                {
                    Global.Msg("No data found");
                    // return null;
                   string[] arr = new string[] { };
                   return arr;
                }
                return strResult;
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
                return null;
            }


        }
        public static DataTable GetGroupByID(int GroupID, Lang Language)
        {
            string LangField = "EngName";
            if (Language == Lang.English)
                LangField = "EngName";
            else if (Language == Lang.Nepali)
                LangField = "NepName";

            return Global.m_db.SelectQry("SELECT a.LedgerCode, a.GroupID ID, a." + LangField + " Name, b." + LangField + " Parent, a.DrCr Type, a.Remarks Remarks FROM Acc.tblGroup a LEFT OUTER JOIN Acc.tblGroup b ON a.Parent_GrpID=b.GroupID WHERE a.GroupID='" + GroupID.ToString() + "'", "Search");

        }

        public static DataTable GetGroupByID(string GroupID)
        {
            return Global.m_db.SelectQry("SELECT * FROM Acc.tblGroup WHERE GroupID IN(" + (GroupID) + ")", "table");
        }


        /// <summary>
        /// Finds GroupID of the given ledger ID
        /// </summary>
        /// <param name="LedgerID"></param>
        /// <returns></returns>
        public static int GetGroupFromLedgerID(int LedgerID)
        {
            try
            {
                object GroupID = Global.m_db.GetScalarValue("SELECT GroupID FROM Acc.tblLedger WHERE LedgerID='" + LedgerID.ToString() + "'");
                return Convert.ToInt32(GroupID);
            }
            catch
            {
                throw new Exception("GetGroupFromLedgerID - Invalid Ledger ID");
            }

        }

        /// <summary>
        /// Gets Root group information from child group ID
        /// </summary>
        /// <param name="GroupID"></param>
        /// <returns></returns>
        public static DataTable GetRootGroup(int GroupID)
        {
            try
            {
                // Global.m_db.InsertUpdateQry("INSERT INTO Account.tblAccountHead(AccountHeadName,Under_Account_ID,Description,BuiltIn) VALUES ('" + AccountHeadName + "','1','" + Description + "','0')");

                Global.m_db.ClearParameter();
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("Acc.spFindRootGroup");
                Global.m_db.AddParameter("@GroupID", SqlDbType.Int, GroupID);
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

        //bugetalloc is set true only for budget allocation form search
        public static DataTable Search(SearchIn m_SearchIn, SearchOp SrchOP1, string SearchParam1, SearchOp SrchOP2, string SearchParam2, Lang Language, bool budgetalloc)
        {
            string Operator = "LIKE", Op1 = "", Op2 = "";
            switch (SrchOP1)
            {
                case SearchOp.Equals:
                    Operator = "=";
                    break;
                case SearchOp.Begins_With:
                    Operator = "LIKE";
                    Op2 = "%";
                    break;
                case SearchOp.Greater_Or_Equals:
                    Operator = ">=";
                    break;
                case SearchOp.Smaller_Or_Equals:
                    Operator = "<=";
                    break;
                case SearchOp.Contains:
                    Operator = "LIKE";
                    Op1 = "%";
                    Op2 = "%";
                    break;
            }

            string Conj = "", Operator2 = "<=";
            switch (SrchOP2)
            {
                case SearchOp.Equals:
                    Operator2 = "='";
                    Conj = "' AND ";
                    break;
                case SearchOp.Smaller_Or_Equals:
                    Operator2 = "<='";
                    Conj = "' AND ";
                    break;
            }
            //Deactivate SrchOP2 if SearchParam2 is not available
            if (SearchParam2 == "")
            {
                Conj = "";
                Operator2 = "";
            }
            string LangField = "EngName";
            if (Language == Lang.English)
                LangField = "EngName";
            else if (Language == Lang.Nepali)
                LangField = "NepName";
            DataTable dtSearch1;
            string FilterString = "";
            switch (m_SearchIn)
            {
                #region Account Groups Search
                case SearchIn.Account_Groups:


                    //change

                    if (budgetalloc)
                    {
                        string[] str = AccountGroup.GetAllGroupsUnderExpenditure();

                        dtSearch1 = Global.m_db.SelectQry("SELECT  a.LedgerCode, a.GroupID, a." + LangField + " Name, b." + LangField + " Parent, a.DrCr Type FROM Acc.tblGroup a LEFT OUTER JOIN Acc.tblGroup b ON a.Parent_GrpID=b.GroupID WHERE a." + LangField + " " + Operator + " '" + Op1 + SearchParam1 + Op2 + "' and  a.GroupID IN(" + string.Join(",", (string[])str) + ")", "Search");
                    }
                    else
                    {
                        dtSearch1 = Global.m_db.SelectQry("SELECT  a.LedgerCode, a.GroupID, a." + LangField + " Name, b." + LangField + " Parent, a.DrCr Type FROM Acc.tblGroup a LEFT OUTER JOIN Acc.tblGroup b ON a.Parent_GrpID=b.GroupID WHERE a." + LangField + " " + Operator + " '" + Op1 + SearchParam1 + Op2 + "'", "Search");

                    }
                    return dtSearch1;
                    break;
                #endregion

                #region Accounts Under Search
                case SearchIn.Accounts_Under:
                    try
                    {
                        //Get total Accounts which matches the parameter
                        if (LangMgr.DefaultLanguage == Lang.English)
                            LangField = "EngName";
                        else if (LangMgr.DefaultLanguage == Lang.Nepali)
                            LangField = "NepName";

                        dtSearch1 = Global.m_db.SelectQry("SELECT a.LedgerCode, a.GroupID, a." + LangField + " Name, b." + LangField + " Parent, a.DrCr Type FROM Acc.tblGroup a LEFT OUTER JOIN Acc.tblGroup b ON a.Parent_GrpID=b.GroupID WHERE a." + LangField + " " + Operator + " '" + Op1 + SearchParam1 + Op2 + "'", "Search");

                        List<string> lstGroupID = new List<string>();
                        for (int i = 0; i < dtSearch1.Rows.Count; i++)
                        {
                            ArrayList tmpGroupID = new ArrayList();
                            AccountGroup.GetAccountsUnder(Convert.ToInt32(dtSearch1.Rows[i]["GroupID"]), tmpGroupID);
                            foreach (object j in tmpGroupID)
                            {
                                lstGroupID.Add(j.ToString());
                            }
                        }
                        //finally we have set of IDs in arrGroupID which falls under the Searched GroupID

                        //Now list all those in listview with the IDs in arrGroupID
                        DataTable dtFound = new DataTable();

                        string[] strResult = (string[])lstGroupID.ToArray();

                        if (strResult.Count<string>() <= 0)
                        {
                            //Global.Msg("No data found");
                            return new DataTable();
                        }

                        if (budgetalloc)
                        {
                            string[] str = AccountGroup.GetAllGroupsUnderExpenditure();
                            dtFound = Global.m_db.SelectQry("SELECT  a.LedgerCode, a.GroupID, a." + LangField + " Name, b." + LangField + " Parent, a.DrCr Type FROM Acc.tblGroup a LEFT OUTER JOIN Acc.tblGroup b ON a.Parent_GrpID=b.GroupID WHERE a.GroupID IN (" + string.Join(",", (string[])strResult) + ")  and  a.GroupID IN(" + string.Join(",", (string[])str) + ")", "Search");

                        }
                        else
                        {
                            dtFound = Global.m_db.SelectQry("SELECT  a.LedgerCode, a.GroupID, a." + LangField + " Name, b." + LangField + " Parent, a.DrCr Type FROM Acc.tblGroup a LEFT OUTER JOIN Acc.tblGroup b ON a.Parent_GrpID=b.GroupID WHERE a.GroupID IN (" + string.Join(",", (string[])strResult) + ")", "Search");
                        }

                        return dtFound;

                    }
                    catch (Exception ex)
                    {
                        Global.MsgError(ex.Message);
                    }
                    break;
                #endregion

                    
            }
            return null;
        }

        //public void GetBalance(ArrayList tmpGroupID, ref double DebitBalance, ref double CreditBalance)

        //Another MethodZZ

        ////public string GetTransaction(ArrayList tmpGroupID)
        ////{

        ////    string GroupID = "";
        ////    int i = 0;
        ////    foreach (object j in tmpGroupID)
        ////    {
        ////        if (i == 0)// for first GroupID
        ////            GroupID = "'" + j.ToString() + "'";
        ////        else  //Separating Other GroupID by commas
        ////            GroupID += "," + "'" + j.ToString() + "'";
        ////        i++;

        ////    }

        ////    DataTable dt = Global.m_db.SelectQry("SELECT * FROM Acc.tblLedger WHERE GroupID IN (" + (GroupID) + ")", "GroupID");
        ////    string LedgerID = "";

        ////    for (int i1 = 0; i1 < dt.Rows.Count; i1++)
        ////    {
        ////        DataRow dr = dt.Rows[i1];
        ////        if (i1 == 0)//for First LedgerID
        ////            LedgerID = "'" + (dr["LedgerID"].ToString()) + "'";
        ////        else  //separating other LedgerID by comma

        ////            LedgerID += "," + "'" + (dr["LedgerID"].ToString()) + "'";

        ////    }
        ////   //Selecting the all contents of tblTransaction with help of LedgerID
        ////    // formate in this way SELECT * FROM Acc.tblTransaction WHERE LedgerID IN ('41','40','32','36','33')
        ////    DataTable dt1 = Global.m_db.SelectQry("SELECT * FROM Acc.tblTransaction WHERE LedgerID IN (" +(LedgerID) + ")","LedgerID");
        ////        int Debit_Sum = 0;
        ////        int Credit_Sum = 0;
        ////        for (int j = 0; j < dt1.Rows.Count; j++)
        ////        {
        ////            DataRow dr1 = dt1.Rows[j];
        ////            int Debit_Amount = Convert.ToInt32(dr1["Debit_Amount"]);
        ////            int Credit_Amount = Convert.ToInt32(dr1["Credit_Amount"]);
        ////            Debit_Sum+=Debit_Amount;
        ////            Credit_Sum += Credit_Amount;
        ////        }
        ////        if (Debit_Sum > Credit_Sum)
        ////        {
        ////            int Debit = Debit_Sum - Credit_Sum;
        ////            return "D"+","+Debit.ToString();

        ////        }

        ////        else
        ////        {
        ////            int Credit = Credit_Sum - Debit_Sum;
        ////            return "C" + "," + Credit.ToString();
        ////        }
        ////}

        // Getting EngName of corresponding GroupID

        public static string GetEngName(string GroupID)
        {
            DataTable dt1 = Global.m_db.SelectQry("SELECT EngName FROM Acc.tblGroup WHERE GroupID='" + GroupID + "'", "EngName");
            string EngName = "";
            for (int i = 0; i < dt1.Rows.Count; i++)
            {
                DataRow dr = dt1.Rows[i];
                EngName = (dr["EngName"].ToString());
            }
            return EngName;
        }

        public static DataTable GetDetailLedgerID(int GrpID,bool IncludeSubgroups)
        {
            #region BLOCK FOR GETTING THE INFORMATION OF DETAIL LEDGERS OF CORESPONDING GROUP AND SUB-GROUPS
                ArrayList tmpGroupID = new ArrayList();//Dyanamically allocating array type of variable tmpGroupID
                tmpGroupID.Add(GrpID);
                if (IncludeSubgroups == true)//Including the ledgers of subgroups too
                AccountGroup.GetAccountsUnder(GrpID, tmpGroupID);//Calling this function for collecting subGroupsID which fall under GroupID and storing on arrylist
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

                // If datatable doesnot have any value then return empty datatable
                DataTable dt1;
                if (LedgerID.Length > 0)
                    dt1 = Global.m_db.SelectQry("SELECT * FROM Acc.tblLedger WHERE LedgerID IN (" + (LedgerID) + ")", "LedgerID");
                else
                    dt1 = new DataTable();

                return dt1;
            #endregion
        }
           

        public static bool IsLedgerUnderGroup(int LedgerID, int GroupID)
        {
            DataTable dt = AccountGroup.GetDetailLedgerID(GroupID,true);
            for (int i = 1; i <= dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i-1];
                if (Convert.ToInt32(dr["LedgerID"]) == LedgerID)
                    return true;
            }
            return false;
        }

        public DataTable GetDistinctRowID()
        {
            return Global.m_db.SelectQry("SELECT DISTINCT (RowID), TransactDate FROM Acc.tblTransaction ORDER BY TransactDate Asc","RowID");       
        }

        public DataTable GetDistinctLedgerID()
        {
            DataTable dt = new DataTable();
            dt = Global.m_db.SelectQry("SELECT DISTINCT(LedgerID) FROM Acc.tblLedger ORDER BY ledgerID ASC", "LedgerID");
            return dt;
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
            Global.Msg("SELECT DISTINCT(LedgerID) FROM Acc.tblTransaction WHERE LedgerID IN (" + (LedgerID) + ")");

            if (LedgerID.Length > 0)
                dt1 = Global.m_db.SelectQry("SELECT DISTINCT(LedgerID) FROM Acc.tblTransaction WHERE LedgerID IN (" + (LedgerID) + ")", "LedgerID");
            else
                dt1 = new DataTable();

            return dt1;
        }
        public DataTable GetDistinctParentGrpID()
        {
            return Global.m_db.SelectQry("SELECT DISTINCT (Parent_GrpID) FROM Acc.tblGroup", "Group");   
        }

        public static int GetGroupIDFromGroupNumber(int GroupNumber)
        {
            object objReturn = Global.m_db.GetScalarValue("SELECT GroupID FROM Acc.tblGroup WHERE GroupNumber = '" + GroupNumber + "'");
            int intReturn = Convert.ToInt32(objReturn);
            return intReturn;
        }

        public static int GetLedgerIDFromLedgerNumber(int LedgerNumber)
        {
            object objReturn = Global.m_db.GetScalarValue("SELECT LedgerID FROM Acc.tblLedger WHERE LedgerNumber = '" + LedgerNumber + "'");
            int intReturn = Convert.ToInt32(objReturn);
            return intReturn;
        }

        public static int GetGroupIDFromAccountType(string accounttype)
        {
            object objReturn = Global.m_db.GetScalarValue("SELECT GroupID FROM Acc.tblGroup WHERE AccountType = '" + accounttype + "'");
            int intReturn = Convert.ToInt32(objReturn);
            return intReturn;
        }


        public DataTable GetLedgerIDFromGroupID(int GroupID)
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

            DataTable dt = Global.m_db.SelectQry("SELECT LedgerID,EngName LedgerName FROM Acc.tblLedger WHERE GroupID IN (" + (GroupID1) + ")", "GroupID");
            return dt;
        }

        public static DataTable GetChildGroupsFromParentID(string ParentGroupID)
        {
            DataTable dt1 = Global.m_db.SelectQry("SELECT * FROM Acc.tblGroup WHERE Parent_GrpID='" + ParentGroupID + "'", "GroupInfo");

            return dt1;
        }
        public DataTable getPartyByID()
        {
            string str = "select * from Acc.tblLedger where GroupID=29";
            return Global.m_db.SelectQry(str, "tblLedger");
        }

        public static int GetGroupIDByLedgerID(int LedgerID)
        {
            object objReturn = Global.m_db.GetScalarValue("select GroupID from Acc.tblLedger WHERE LedgerID = '" + LedgerID + "'");
            int intReturn = Convert.ToInt32(objReturn);
            return intReturn; 
        }
        //public DataTable getPartyByID()
        //{
        //    string str = "select * from Acc.tblLedger where GroupID=29";
        //    return Global.m_db.SelectQry(str, "tblLedger");
        //}
    }
}
