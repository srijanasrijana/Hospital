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
    public interface ILedger
    {
        //bool Create(string LedgerCode, string LedgerName, int GroupID, double PreYrBal, string PreYrBalDrCr, int OpCCYID, double OpCCYRate, DateTime OpCCRateDate, string Remarks, string Person_Name, string Address1, string Address2, string City, string Phone, string Email, string Company, string Website, string VatPanNo, double CreditLimit, bool CalculateChecked, float Rate,string oldledgerdesc,string newledgerdesc,bool isnew, bool isBillReference);
        //bool Modify(int LedgerID, string LedgerCode, string LedgerName, int GroupID, double PreYrBal, string PreYrBalDrCr, int OpCCYID, double OpCCYRate, DateTime OpCCRateDate, string Remarks, string Person_Name, string Address1, string Address2, string City, string Phone, string Email, string Company, string Website, string VatPanNo, double CreditLimit, bool CalculateChecked, float Rate, string oldledgerdesc, string newledgerdesc, bool isnew, bool isBillReference);
        void Delete(string AccountHeadName);

        bool Create(string LedgerCode, string LedgerName, int GroupID, double PreYrBal, string PreYrBalDrCr, int OpCCYID, double OpCCYRate, DateTime OpCCRateDate, string Remarks, string Person_Name, string Address1, string Address2, string City, string Phone, string Email, string Company, string Website, string VatPanNo, double CreditLimit, bool CalculateChecked, float Rate, string oldledgerdesc, string newledgerdesc, bool isnew, out int ledgerID);
        bool Modify(int LedgerID, string LedgerCode, string LedgerName, int GroupID, double PreYrBal, string PreYrBalDrCr, int OpCCYID, double OpCCYRate, DateTime OpCCRateDate, string Remarks, string Person_Name, string Address1, string Address2, string City, string Phone, string Email, string Company, string Website, string VatPanNo, double CreditLimit, bool CalculateChecked, float Rate, string oldledgerdesc, string newledgerdesc, bool isnew);
    }

    /// <summary>
    /// Handles the ledger in Account
    /// Version: 1.0
    /// Dependensies: Global.cs, SqlDB.cs, LangMgr.cs
    /// Developer: Shamit Shrestha
    /// </summary>
    public class Ledger : ILedger
    {

        /// <summary>
        /// Create a ledger with only 3 parameters
        /// </summary>
        /// <param name="LedgerName"></param>
        /// <param name="GroupID"></param>
        /// <param name="Remarks"></param>
        /// <returns></returns>

        public bool Create(string LedgerCode, string LedgerName, int GroupID, double PreYrBal, string PreYrBalDrCr, int OpCCYID, double OpCCYRate, DateTime OpCCRateDate, string Remarks, string Person_Name, string Address1, string Address2, string City, string Phone, string Email, string Company, string Website, string VatPanNo, double CreditLimit, bool CalculateChecked, float Rate, string oldledgerdesc, string newledgerdesc, bool isnew, out int ledgerID)
        {
            try
            {

                string str1 = (string)Global.MakeNull(Person_Name);
                Global.m_db.ClearParameter();
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("Acc.spLedgerCreate");
                Global.m_db.AddParameter("@LedgerCode", SqlDbType.NVarChar, 50, LedgerCode);
                Global.m_db.AddParameter("@Name", SqlDbType.NVarChar, 50, LedgerName);
                Global.m_db.AddParameter("@GroupID", SqlDbType.Int, GroupID);
                //Global.m_db.AddParameter("@OpBalance", SqlDbType.Money,OpeningBalance);
                Global.m_db.AddParameter("@PreYrBal", SqlDbType.Money, PreYrBal);
                Global.m_db.AddParameter("@PreYrBalDrCr", SqlDbType.NVarChar, 50, PreYrBalDrCr);
                Global.m_db.AddParameter("@OpCCYID", SqlDbType.Int, OpCCYID);
                Global.m_db.AddParameter("@OpCCR", SqlDbType.Money, OpCCYRate);
                //Global.m_db.AddParameter("@OpCCRDate", SqlDbType.DateTime, Date.ToDB(OpCCRateDate));
                //Global.m_db.AddParameter("@OpDrCr", SqlDbType.NVarChar,10, OpBalance_DrCr);
                Global.m_db.AddParameter("@PerName", SqlDbType.NVarChar, 50, Person_Name);
                Global.m_db.AddParameter("@Address1", SqlDbType.NVarChar, 50, Address1);
                Global.m_db.AddParameter("@Address2", SqlDbType.NVarChar, 50, Address2);
                Global.m_db.AddParameter("@City", SqlDbType.NVarChar, 50, City);
                Global.m_db.AddParameter("@Phone", SqlDbType.NVarChar, 50, Phone);
                Global.m_db.AddParameter("@Email", SqlDbType.NVarChar, 50, Email);
                Global.m_db.AddParameter("@Company", SqlDbType.NVarChar, 50, Company);
                Global.m_db.AddParameter("@Website", SqlDbType.NVarChar, 50, Website);
                Global.m_db.AddParameter("@VatPanNo", SqlDbType.NVarChar, 50, VatPanNo);
                Global.m_db.AddParameter("@CreditLimit", SqlDbType.Money, CreditLimit);
                Global.m_db.AddParameter("@CalculateChecked", SqlDbType.Bit, CalculateChecked);
                Global.m_db.AddParameter("@Rate", SqlDbType.Float, Rate);
                Global.m_db.AddParameter("@Remarks", SqlDbType.NVarChar, 200, Remarks);
                Global.m_db.AddParameter("@Created_By", SqlDbType.NVarChar, 50, User.CurrUserID.ToString());
                SqlParameter objReturn = (SqlParameter)Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                SqlParameter objReturnID = (SqlParameter)Global.m_db.AddOutputParameter("@ReturnID", SqlDbType.NVarChar, 20);

                Global.m_db.ProcessParameter();
                int returnID = Convert.ToInt32(objReturnID.Value);
                ledgerID = returnID;

                try
                {
                    if (isnew == true)
                    {
                        string username = User.CurrentUserName;
                        string voucherdate = Date.ToDB(DateTime.Now).ToString();
                        string VoucherType = "LEDGER";
                        string action = "INSERT";
                        int rowid = 0;
                        string ComputerName = Global.ComputerName;
                        string MacAddress = Global.MacAddess;
                        string IpAddress = Global.IpAddress;
                        string desc = oldledgerdesc + newledgerdesc;

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
                        string VoucherType = "LEDGER";
                        string action = "UPDATE";
                        int rowid = 0;
                        string ComputerName = Global.ComputerName;
                        string MacAddress = Global.MacAddess;
                        string IpAddress = Global.IpAddress;
                        string desc = oldledgerdesc + newledgerdesc;

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

                return (objReturn.Value.ToString() == "SUCCESS" ? true : false);


            }
            catch (SqlException ex)
            {
                #region SQLException
                switch (ex.Number)
                {
                    case 4060: // Invalid Database 
                        throw new Exception("Invalid Database");
                        break;

                    case 18456: // Login Failed 
                        throw new Exception("Login Failed!");
                        break;

                    case 547: // ForeignKey Violation , Check Constraint
                        throw new Exception("Invalid parent group! Check the parent group and try again!");
                        break;

                    case 2627: // Unique Index/ Primary key Violation/ Constriant Violation 
                        throw new Exception("The ledger name already exists! Please choose another group names!");
                        break;

                    case 2601: // Unique Index/Constriant Violation 
                        throw new Exception("Unique index violation!");
                        break;

                    case 5000: //Trigger violation
                        throw new Exception("Trigger violation!");
                        break;

                    default:
                        throw new Exception("Problem with the SQL-" + ex.Message);
                        break;
                }
                #endregion
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
                return false;
            }

            //try
            //{
            //    if (isnew == true)
            //    {
            //        string username = User.CurrentUserName;
            //        string voucherdate = Convert.ToString(DateTime.Now);
            //        string VoucherType = "LEDGER";
            //        string action = "INSERT";
            //        int rowid = 0;
            //        string ComputerName = Global.ConcatAllCompInfo;
            //        string desc = oldledgerdesc + newledgerdesc;
            //        string SQL = "INSERT INTO System.tblAuditLog(computername,username,voucher_type,action,rowid,description,voucherdate) VALUES ('" + ComputerName + "','" + username + "','" + VoucherType + "','" + action + "','" + rowid + "','" + desc + "','" + Date.ToDB(DateTime.Now).ToString() + "')";
            //        Global.m_db.InsertUpdateQry(SQL);
            //    }
            //    else if (isnew == false)
            //    {
            //        string username = User.CurrentUserName;
            //        string voucherdate = Convert.ToString(DateTime.Now);
            //        string VoucherType = "LEDGER";
            //        string action = "UPDATE";
            //        int rowid = 0;
            //        string ComputerName = Global.ConcatAllCompInfo;
            //        string desc = oldledgerdesc + newledgerdesc;
            //        string SQL = "INSERT INTO System.tblAuditLog(computername,username,voucher_type,action,rowid,description,voucherdate) VALUES ('" + ComputerName + "','" + username + "','" + VoucherType + "','" + action + "','" + rowid + "','" + desc + "','" + Date.ToDB(DateTime.Now).ToString() + "')";
            //        Global.m_db.InsertUpdateQry(SQL);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Global.MsgError(ex.Message);
            //}


        }
        //public bool Create(string LedgerCode, string LedgerName, int GroupID, double PreYrBal, string PreYrBalDrCr, int OpCCYID, double OpCCYRate, DateTime OpCCRateDate, string Remarks, string Person_Name, string Address1, string Address2, string City, string Phone, string Email, string Company, string Website, string VatPanNo, double CreditLimit, bool CalculateChecked, float Rate, string oldledgerdesc, string newledgerdesc, bool isnew, bool isBillReference)
        //{
        //    try
        //    {

        //        string str1 = (string)Global.MakeNull(Person_Name);
        //        Global.m_db.ClearParameter();
        //        Global.m_db.setCommandType(CommandType.StoredProcedure);
        //        Global.m_db.setCommandText("Acc.spLedgerCreate");
        //        Global.m_db.AddParameter("@LedgerCode", SqlDbType.NVarChar, 50, LedgerCode);
        //        Global.m_db.AddParameter("@Name", SqlDbType.NVarChar, 50, LedgerName);
        //        Global.m_db.AddParameter("@GroupID", SqlDbType.Int, GroupID);
        //        //Global.m_db.AddParameter("@OpBalance", SqlDbType.Money,OpeningBalance);
        //        Global.m_db.AddParameter("@PreYrBal", SqlDbType.Money, PreYrBal);
        //        Global.m_db.AddParameter("@PreYrBalDrCr", SqlDbType.NVarChar, 50, PreYrBalDrCr);
        //        Global.m_db.AddParameter("@OpCCYID", SqlDbType.Int, OpCCYID);
        //        Global.m_db.AddParameter("@OpCCR", SqlDbType.Money, OpCCYRate);
        //        //Global.m_db.AddParameter("@OpCCRDate", SqlDbType.DateTime, Date.ToDB(OpCCRateDate));
        //        //Global.m_db.AddParameter("@OpDrCr", SqlDbType.NVarChar,10, OpBalance_DrCr);
        //        Global.m_db.AddParameter("@PerName", SqlDbType.NVarChar, 50, Person_Name);
        //        Global.m_db.AddParameter("@Address1", SqlDbType.NVarChar, 50, Address1);
        //        Global.m_db.AddParameter("@Address2", SqlDbType.NVarChar, 50, Address2);
        //        Global.m_db.AddParameter("@City", SqlDbType.NVarChar, 50, City);
        //        Global.m_db.AddParameter("@Phone", SqlDbType.NVarChar, 50, Phone);
        //        Global.m_db.AddParameter("@Email", SqlDbType.NVarChar, 50, Email);
        //        Global.m_db.AddParameter("@Company", SqlDbType.NVarChar, 50, Company);
        //        Global.m_db.AddParameter("@Website", SqlDbType.NVarChar, 50, Website);
        //        Global.m_db.AddParameter("@VatPanNo", SqlDbType.NVarChar, 50, VatPanNo);
        //        Global.m_db.AddParameter("@CreditLimit", SqlDbType.Money, CreditLimit);
        //        Global.m_db.AddParameter("@CalculateChecked", SqlDbType.Bit, CalculateChecked);
        //        Global.m_db.AddParameter("@Rate", SqlDbType.Float, Rate);
        //        Global.m_db.AddParameter("@Remarks", SqlDbType.NVarChar, 200, Remarks);
        //        Global.m_db.AddParameter("@Created_By", SqlDbType.NVarChar, 50, User.CurrUserID.ToString());
        //        Global.m_db.AddParameter("@IsBillReference", SqlDbType.Bit, 200, isBillReference);
        //        SqlParameter objReturn = (SqlParameter)Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
        //        SqlParameter objReturnID = Global.m_db.AddOutputParameter("@ReturnID", SqlDbType.Int);
        //        Global.m_db.ProcessParameter();

        //        try
        //        {
        //            if (isnew == true)
        //            {
        //                string username = User.CurrentUserName;
        //                string voucherdate = Date.ToDB(DateTime.Now).ToString();
        //                string VoucherType = "LEDGER";
        //                string action = "INSERT";
        //                int rowid = 0;
        //                string ComputerName = Global.ComputerName;
        //                string MacAddress = Global.MacAddess;
        //                string IpAddress = Global.IpAddress;
        //                string desc = oldledgerdesc + newledgerdesc;

        //                Global.m_db.ClearParameter();
        //                Global.m_db.setCommandType(CommandType.StoredProcedure);
        //                Global.m_db.setCommandText("system.spAddAuditLog");
        //                Global.m_db.AddParameter("@ComputerName", SqlDbType.NVarChar, 50, ComputerName);
        //                Global.m_db.AddParameter("@UserName", SqlDbType.NVarChar, 50, username);//Set same for both for time being
        //                Global.m_db.AddParameter("@Action", SqlDbType.NVarChar, 50, action);
        //                Global.m_db.AddParameter("@Description", SqlDbType.NVarChar, 4000, desc);
        //                Global.m_db.AddParameter("@RowID", SqlDbType.Int, rowid);
        //                Global.m_db.AddParameter("@MAC_Address", SqlDbType.NVarChar, 50, MacAddress);
        //                Global.m_db.AddParameter("@IP_Address", SqlDbType.NVarChar, 50, IpAddress);
        //                Global.m_db.AddParameter("@Voucher_Type", SqlDbType.NVarChar, 50, VoucherType);
        //                Global.m_db.AddParameter("@VoucherDate", SqlDbType.NVarChar, 200, voucherdate);
        //                object objReturn1 = Global.m_db.AddOutputParameter("@Result", SqlDbType.NVarChar, 20);
        //                Global.m_db.ProcessParameter();
        //                //string SQL = "INSERT INTO System.tblAuditLog(computername,username,voucher_type,action,rowid,description,voucherdate) VALUES ('" + ComputerName + "','" + username + "','" + VoucherType + "','" + action + "','" + rowid + "','" + desc + "','" + Date.ToDB(DateTime.Now).ToString() + "')";
        //                //Global.m_db.InsertUpdateQry(SQL);
        //            }
        //            else if (isnew == false)
        //            {
        //                string username = User.CurrentUserName;
        //                string voucherdate = Date.ToDB(DateTime.Now).ToString();
        //                string VoucherType = "LEDGER";
        //                string action = "UPDATE";
        //                int rowid = 0;
        //                string ComputerName = Global.ComputerName;
        //                string MacAddress = Global.MacAddess;
        //                string IpAddress = Global.IpAddress;
        //                string desc = oldledgerdesc + newledgerdesc;

        //                Global.m_db.ClearParameter();
        //                Global.m_db.setCommandType(CommandType.StoredProcedure);
        //                Global.m_db.setCommandText("system.spAddAuditLog");
        //                Global.m_db.AddParameter("@ComputerName", SqlDbType.NVarChar, 50, ComputerName);
        //                Global.m_db.AddParameter("@UserName", SqlDbType.NVarChar, 50, username);//Set same for both for time being
        //                Global.m_db.AddParameter("@Action", SqlDbType.NVarChar, 50, action);
        //                Global.m_db.AddParameter("@Description", SqlDbType.NVarChar, 4000, desc);
        //                Global.m_db.AddParameter("@RowID", SqlDbType.Int, rowid);
        //                Global.m_db.AddParameter("@MAC_Address", SqlDbType.NVarChar, 50, MacAddress);
        //                Global.m_db.AddParameter("@IP_Address", SqlDbType.NVarChar, 50, IpAddress);
        //                Global.m_db.AddParameter("@Voucher_Type", SqlDbType.NVarChar, 50, VoucherType);
        //                Global.m_db.AddParameter("@VoucherDate", SqlDbType.NVarChar, 200, voucherdate);
        //                object objReturn1 = Global.m_db.AddOutputParameter("@Result", SqlDbType.NVarChar, 20);
        //                Global.m_db.ProcessParameter();
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            Global.MsgError(ex.Message);
        //        }

        //        return (objReturn.Value.ToString() == "SUCCESS" ? true : false);


        //    }
        //    catch (SqlException ex)
        //    {
        //        #region SQLException
        //        switch (ex.Number)
        //        {
        //            case 4060: // Invalid Database 
        //                throw new Exception("Invalid Database");
        //                break;

        //            case 18456: // Login Failed 
        //                throw new Exception("Login Failed!");
        //                break;

        //            case 547: // ForeignKey Violation , Check Constraint
        //                throw new Exception("Invalid parent group! Check the parent group and try again!");
        //                break;

        //            case 2627: // Unique Index/ Primary key Violation/ Constriant Violation 
        //                throw new Exception("The ledger name already exists! Please choose another group names!");
        //                break;

        //            case 2601: // Unique Index/Constriant Violation 
        //                throw new Exception("Unique index violation!");
        //                break;

        //            case 5000: //Trigger violation
        //                throw new Exception("Trigger violation!");
        //                break;

        //            default:
        //                throw new Exception("Problem with the SQL-" + ex.Message);
        //                break;
        //        }
        //        #endregion
        //        return false;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //        return false;
        //    }
           
        //    //try
        //    //{
        //    //    if (isnew == true)
        //    //    {
        //    //        string username = User.CurrentUserName;
        //    //        string voucherdate = Convert.ToString(DateTime.Now);
        //    //        string VoucherType = "LEDGER";
        //    //        string action = "INSERT";
        //    //        int rowid = 0;
        //    //        string ComputerName = Global.ConcatAllCompInfo;
        //    //        string desc = oldledgerdesc + newledgerdesc;
        //    //        string SQL = "INSERT INTO System.tblAuditLog(computername,username,voucher_type,action,rowid,description,voucherdate) VALUES ('" + ComputerName + "','" + username + "','" + VoucherType + "','" + action + "','" + rowid + "','" + desc + "','" + Date.ToDB(DateTime.Now).ToString() + "')";
        //    //        Global.m_db.InsertUpdateQry(SQL);
        //    //    }
        //    //    else if (isnew == false)
        //    //    {
        //    //        string username = User.CurrentUserName;
        //    //        string voucherdate = Convert.ToString(DateTime.Now);
        //    //        string VoucherType = "LEDGER";
        //    //        string action = "UPDATE";
        //    //        int rowid = 0;
        //    //        string ComputerName = Global.ConcatAllCompInfo;
        //    //        string desc = oldledgerdesc + newledgerdesc;
        //    //        string SQL = "INSERT INTO System.tblAuditLog(computername,username,voucher_type,action,rowid,description,voucherdate) VALUES ('" + ComputerName + "','" + username + "','" + VoucherType + "','" + action + "','" + rowid + "','" + desc + "','" + Date.ToDB(DateTime.Now).ToString() + "')";
        //    //        Global.m_db.InsertUpdateQry(SQL);
        //    //    }
        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    Global.MsgError(ex.Message);
        //    //}


        //}
        public bool Modify(int LedgerID, string LedgerCode, string LedgerName, int GroupID, double PreYrBal, string PreYrBalDrCr, int OpCCYID, double OpCCYRate, DateTime OpCCRateDate, string Remarks, string Person_Name, string Address1, string Address2, string City, string Phone, string Email, string Company, string Website, string VatPanNo, double CreditLimit, bool CalculateChecked, float Rate, string oldledgerdesc, string newledgerdesc, bool isnew)
        {
            try
            {
                string LangField = "ENGLISH";
                string str1 = (string)Global.MakeNull(Person_Name);
                Global.m_db.ClearParameter();
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("Acc.spLedgerModify");
                Global.m_db.AddParameter("@LedgerID", SqlDbType.Int, LedgerID);
                Global.m_db.AddParameter("@LedgerCode", SqlDbType.NVarChar, 50, LedgerCode);
                Global.m_db.AddParameter("@Name", SqlDbType.NVarChar, 50, LedgerName);
                if (LangMgr.DefaultLanguage == Lang.English)
                    LangField = "ENGLISH";
                else if (LangMgr.DefaultLanguage == Lang.Nepali)
                    LangField = "NEPALI";

                Global.m_db.AddParameter("@Lang", SqlDbType.NVarChar, 50, LangField);
                Global.m_db.AddParameter("@GroupID", SqlDbType.Int, GroupID);
                Global.m_db.AddParameter("@PreYrBal", SqlDbType.Money, PreYrBal);
                Global.m_db.AddParameter("@PreYrBalDrCr", SqlDbType.NVarChar, 50, PreYrBalDrCr);
                Global.m_db.AddParameter("@OpCCYID", SqlDbType.Int, OpCCYID);
                Global.m_db.AddParameter("@OpCCR", SqlDbType.Money, OpCCYRate);
                // Global.m_db.AddParameter("@OpCCRDate", SqlDbType.DateTime, Date.ToDB(OpCCRateDate));
                Global.m_db.AddParameter("@PerName", SqlDbType.NVarChar, 50, Person_Name);
                Global.m_db.AddParameter("@Address1", SqlDbType.NVarChar, 50, Address1);
                Global.m_db.AddParameter("@Address2", SqlDbType.NVarChar, 50, Address2);
                Global.m_db.AddParameter("@City", SqlDbType.NVarChar, 50, City);
                Global.m_db.AddParameter("@Phone", SqlDbType.NVarChar, 50, Phone);
                Global.m_db.AddParameter("@Email", SqlDbType.NVarChar, 50, Email);
                Global.m_db.AddParameter("@Company", SqlDbType.NVarChar, 50, Company);
                Global.m_db.AddParameter("@Website", SqlDbType.NVarChar, 50, Website);
                Global.m_db.AddParameter("@VatPanNo", SqlDbType.NVarChar, 50, VatPanNo);
                Global.m_db.AddParameter("@CreditLimit", SqlDbType.Money, CreditLimit);
                Global.m_db.AddParameter("@CalculateChecked", SqlDbType.Bit, CalculateChecked);
                Global.m_db.AddParameter("@Rate", SqlDbType.Float, Rate);
                Global.m_db.AddParameter("@Remarks", SqlDbType.NVarChar, 200, Remarks);
                Global.m_db.AddParameter("@Modified_By", SqlDbType.NVarChar, 50, User.CurrUserID.ToString());
                SqlParameter objReturn = (SqlParameter)Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                SqlParameter objReturnID = Global.m_db.AddOutputParameter("@ReturnID", SqlDbType.Int);
                Global.m_db.ProcessParameter();
                try
                {
                    if (isnew == true)
                    {
                        string username = User.CurrentUserName;
                        string voucherdate = Date.ToDB(DateTime.Now).ToString();
                        string VoucherType = "LEDGER";
                        string action = "INSERT";
                        int rowid = 0;
                        string ComputerName = Global.ComputerName;
                        string MacAddress = Global.MacAddess;
                        string IpAddress = Global.IpAddress;
                        string desc = oldledgerdesc + newledgerdesc;

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
                        string VoucherType = "LEDGER";
                        string action = "UPDATE";
                        int rowid = 0;
                        string ComputerName = Global.ComputerName;
                        string MacAddress = Global.MacAddess;
                        string IpAddress = Global.IpAddress;
                        string desc = oldledgerdesc + newledgerdesc;

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

                return (objReturn.Value.ToString() == "SUCCESS" ? true : false);


            }
            catch (SqlException ex)
            {
                #region SQLException
                switch (ex.Number)
                {
                    case 4060: // Invalid Database 
                        throw new Exception("Invalid Database");
                        break;

                    case 18456: // Login Failed 
                        throw new Exception("Login Failed!");
                        break;

                    case 547: // ForeignKey Violation , Check Constraint
                        throw new Exception("Invalid parent group! Check the parent group and try again!");
                        break;

                    case 2627: // Unique Index/ Primary key Violation/ Constriant Violation 
                        throw new Exception("The ledger name already exists! Please choose another group names!");
                        break;

                    case 2601: // Unique Index/Constriant Violation 
                        throw new Exception("Unique index violation!");
                        break;

                    case 5000: //Trigger violation
                        throw new Exception("Trigger violation!");
                        break;

                    default:
                        throw new Exception("Problem with the SQL-" + ex.Message);
                        break;
                }
                #endregion
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
                return false;
            }


            //try
            //{
            //    if (isnew == true)
            //    {
            //        string username = User.CurrentUserName;
            //        string voucherdate = Convert.ToString(DateTime.Now);
            //        string VoucherType = "LEDGER";
            //        string action = "INSERT";
            //        int rowid = 0;
            //        string ComputerName = Global.ConcatAllCompInfo;
            //        string desc = oldledgerdesc + newledgerdesc;
            //        string SQL = "INSERT INTO System.tblAuditLog(computername,username,voucher_type,action,rowid,description,voucherdate) VALUES ('" + ComputerName + "','" + username + "','" + VoucherType + "','" + action + "','" + rowid + "','" + desc + "','" + Date.ToDB(DateTime.Now).ToString() + "')";
            //        Global.m_db.InsertUpdateQry(SQL);
            //    }
            //    else if (isnew == false)
            //    {
            //        string username = User.CurrentUserName;
            //        string voucherdate = Convert.ToString(DateTime.Now);
            //        string VoucherType = "LEDGER";
            //        string action = "UPDATE";
            //        int rowid = 0;
            //        string ComputerName = Global.ConcatAllCompInfo;
            //        string desc = oldledgerdesc + newledgerdesc;
            //        string SQL = "INSERT INTO System.tblAuditLog(computername,username,voucher_type,action,rowid,description,voucherdate) VALUES ('" + ComputerName + "','" + username + "','" + VoucherType + "','" + action + "','" + rowid + "','" + desc + "','" + Date.ToDB(DateTime.Now).ToString() + "')";
            //        Global.m_db.InsertUpdateQry(SQL);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Global.MsgError(ex.Message);
            //}


        }
        ////public bool Modify(int LedgerID, string LedgerCode, string LedgerName, int GroupID, double PreYrBal, string PreYrBalDrCr, int OpCCYID, double OpCCYRate, DateTime OpCCRateDate, string Remarks, string Person_Name, string Address1, string Address2, string City, string Phone, string Email, string Company, string Website, string VatPanNo, double CreditLimit, bool CalculateChecked, float Rate, string oldledgerdesc, string newledgerdesc, bool isnew, bool isBillReference)
        ////{
        ////    try
        ////    {
        ////        string LangField = "ENGLISH";
        ////        string str1 = (string)Global.MakeNull(Person_Name);
        ////        Global.m_db.ClearParameter();
        ////        Global.m_db.setCommandType(CommandType.StoredProcedure);
        ////        Global.m_db.setCommandText("Acc.spLedgerModify");
        ////        Global.m_db.AddParameter("@LedgerID", SqlDbType.Int, LedgerID);
        ////        Global.m_db.AddParameter("@LedgerCode", SqlDbType.NVarChar, 50, LedgerCode);
        ////        Global.m_db.AddParameter("@Name", SqlDbType.NVarChar, 50, LedgerName);
        ////        if (LangMgr.DefaultLanguage == Lang.English)
        ////            LangField = "ENGLISH";
        ////        else if (LangMgr.DefaultLanguage == Lang.Nepali)
        ////            LangField = "NEPALI";

        ////        Global.m_db.AddParameter("@Lang", SqlDbType.NVarChar, 50, LangField);
        ////        Global.m_db.AddParameter("@GroupID", SqlDbType.Int, GroupID);
        ////        Global.m_db.AddParameter("@PreYrBal", SqlDbType.Money, PreYrBal);
        ////        Global.m_db.AddParameter("@PreYrBalDrCr", SqlDbType.NVarChar, 50, PreYrBalDrCr);
        ////        Global.m_db.AddParameter("@OpCCYID", SqlDbType.Int, OpCCYID);
        ////        Global.m_db.AddParameter("@OpCCR", SqlDbType.Money, OpCCYRate);
        ////       // Global.m_db.AddParameter("@OpCCRDate", SqlDbType.DateTime, Date.ToDB(OpCCRateDate));
        ////        Global.m_db.AddParameter("@PerName", SqlDbType.NVarChar, 50, Person_Name);
        ////        Global.m_db.AddParameter("@Address1", SqlDbType.NVarChar, 50, Address1);
        ////        Global.m_db.AddParameter("@Address2", SqlDbType.NVarChar, 50, Address2);
        ////        Global.m_db.AddParameter("@City", SqlDbType.NVarChar, 50, City);
        ////        Global.m_db.AddParameter("@Phone", SqlDbType.NVarChar, 50, Phone);
        ////        Global.m_db.AddParameter("@Email", SqlDbType.NVarChar, 50, Email);
        ////        Global.m_db.AddParameter("@Company", SqlDbType.NVarChar, 50, Company);
        ////        Global.m_db.AddParameter("@Website", SqlDbType.NVarChar, 50, Website);
        ////        Global.m_db.AddParameter("@VatPanNo", SqlDbType.NVarChar, 50, VatPanNo);
        ////        Global.m_db.AddParameter("@CreditLimit", SqlDbType.Money, CreditLimit);
        ////        Global.m_db.AddParameter("@CalculateChecked", SqlDbType.Bit, CalculateChecked);
        ////        Global.m_db.AddParameter("@Rate", SqlDbType.Float, Rate);
        ////        Global.m_db.AddParameter("@Remarks", SqlDbType.NVarChar, 200, Remarks);
        ////        Global.m_db.AddParameter("@Modified_By", SqlDbType.NVarChar, 50, User.CurrUserID.ToString());
        ////        Global.m_db.AddParameter("@IsBillReference", SqlDbType.Bit, 200, isBillReference);
        ////        SqlParameter objReturn = (SqlParameter)Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
        ////        SqlParameter objReturnID = Global.m_db.AddOutputParameter("@ReturnID", SqlDbType.Int);
        ////        Global.m_db.ProcessParameter();
        ////        try
        ////        {
        ////            if (isnew == true)
        ////            {
        ////                string username = User.CurrentUserName;
        ////                string voucherdate = Date.ToDB(DateTime.Now).ToString();
        ////                string VoucherType = "LEDGER";
        ////                string action = "INSERT";
        ////                int rowid = 0;
        ////                string ComputerName = Global.ComputerName;
        ////                string MacAddress = Global.MacAddess;
        ////                string IpAddress = Global.IpAddress;
        ////                string desc = oldledgerdesc + newledgerdesc;

        ////                Global.m_db.ClearParameter();
        ////                Global.m_db.setCommandType(CommandType.StoredProcedure);
        ////                Global.m_db.setCommandText("system.spAddAuditLog");
        ////                Global.m_db.AddParameter("@ComputerName", SqlDbType.NVarChar, 50, ComputerName);
        ////                Global.m_db.AddParameter("@UserName", SqlDbType.NVarChar, 50, username);//Set same for both for time being
        ////                Global.m_db.AddParameter("@Action", SqlDbType.NVarChar, 50, action);
        ////                Global.m_db.AddParameter("@Description", SqlDbType.NVarChar, 4000, desc);
        ////                Global.m_db.AddParameter("@RowID", SqlDbType.Int, rowid);
        ////                Global.m_db.AddParameter("@MAC_Address", SqlDbType.NVarChar, 50, MacAddress);
        ////                Global.m_db.AddParameter("@IP_Address", SqlDbType.NVarChar, 50, IpAddress);
        ////                Global.m_db.AddParameter("@Voucher_Type", SqlDbType.NVarChar, 50, VoucherType);
        ////                Global.m_db.AddParameter("@VoucherDate", SqlDbType.NVarChar, 200, voucherdate);
        ////                object objReturn1 = Global.m_db.AddOutputParameter("@Result", SqlDbType.NVarChar, 20);
        ////                Global.m_db.ProcessParameter();
        ////                //string SQL = "INSERT INTO System.tblAuditLog(computername,username,voucher_type,action,rowid,description,voucherdate) VALUES ('" + ComputerName + "','" + username + "','" + VoucherType + "','" + action + "','" + rowid + "','" + desc + "','" + Date.ToDB(DateTime.Now).ToString() + "')";
        ////                //Global.m_db.InsertUpdateQry(SQL);
        ////            }
        ////            else if (isnew == false)
        ////            {
        ////                string username = User.CurrentUserName;
        ////                string voucherdate = Date.ToDB(DateTime.Now).ToString();
        ////                string VoucherType = "LEDGER";
        ////                string action = "UPDATE";
        ////                int rowid = 0;
        ////                string ComputerName = Global.ComputerName;
        ////                string MacAddress = Global.MacAddess;
        ////                string IpAddress = Global.IpAddress;
        ////                string desc = oldledgerdesc + newledgerdesc;

        ////                Global.m_db.ClearParameter();
        ////                Global.m_db.setCommandType(CommandType.StoredProcedure);
        ////                Global.m_db.setCommandText("system.spAddAuditLog");
        ////                Global.m_db.AddParameter("@ComputerName", SqlDbType.NVarChar, 50, ComputerName);
        ////                Global.m_db.AddParameter("@UserName", SqlDbType.NVarChar, 50, username);//Set same for both for time being
        ////                Global.m_db.AddParameter("@Action", SqlDbType.NVarChar, 50, action);
        ////                Global.m_db.AddParameter("@Description", SqlDbType.NVarChar, 4000, desc);
        ////                Global.m_db.AddParameter("@RowID", SqlDbType.Int, rowid);
        ////                Global.m_db.AddParameter("@MAC_Address", SqlDbType.NVarChar, 50, MacAddress);
        ////                Global.m_db.AddParameter("@IP_Address", SqlDbType.NVarChar, 50, IpAddress);
        ////                Global.m_db.AddParameter("@Voucher_Type", SqlDbType.NVarChar, 50, VoucherType);
        ////                Global.m_db.AddParameter("@VoucherDate", SqlDbType.NVarChar, 200, voucherdate);
        ////                object objReturn1 = Global.m_db.AddOutputParameter("@Result", SqlDbType.NVarChar, 20);
        ////                Global.m_db.ProcessParameter();
        ////            }
        ////        }
        ////        catch (Exception ex)
        ////        {
        ////            Global.MsgError(ex.Message);
        ////        }

        ////        return (objReturn.Value.ToString() == "SUCCESS" ? true : false);


        ////    }
        ////    catch (SqlException ex)
        ////    {
        ////        #region SQLException
        ////        switch (ex.Number)
        ////        {
        ////            case 4060: // Invalid Database 
        ////                throw new Exception("Invalid Database");
        ////                break;

        ////            case 18456: // Login Failed 
        ////                throw new Exception("Login Failed!");
        ////                break;

        ////            case 547: // ForeignKey Violation , Check Constraint
        ////                throw new Exception("Invalid parent group! Check the parent group and try again!");
        ////                break;

        ////            case 2627: // Unique Index/ Primary key Violation/ Constriant Violation 
        ////                throw new Exception("The ledger name already exists! Please choose another group names!");
        ////                break;

        ////            case 2601: // Unique Index/Constriant Violation 
        ////                throw new Exception("Unique index violation!");
        ////                break;

        ////            case 5000: //Trigger violation
        ////                throw new Exception("Trigger violation!");
        ////                break;

        ////            default:
        ////                throw new Exception("Problem with the SQL-" + ex.Message);
        ////                break;
        ////        }
        ////        #endregion
        ////        return false;
        ////    }
        ////    catch (Exception ex)
        ////    {
        ////        throw ex;
        ////        return false;
        ////    }

           
        ////    //try
        ////    //{
        ////    //    if (isnew == true)
        ////    //    {
        ////    //        string username = User.CurrentUserName;
        ////    //        string voucherdate = Convert.ToString(DateTime.Now);
        ////    //        string VoucherType = "LEDGER";
        ////    //        string action = "INSERT";
        ////    //        int rowid = 0;
        ////    //        string ComputerName = Global.ConcatAllCompInfo;
        ////    //        string desc = oldledgerdesc + newledgerdesc;
        ////    //        string SQL = "INSERT INTO System.tblAuditLog(computername,username,voucher_type,action,rowid,description,voucherdate) VALUES ('" + ComputerName + "','" + username + "','" + VoucherType + "','" + action + "','" + rowid + "','" + desc + "','" + Date.ToDB(DateTime.Now).ToString() + "')";
        ////    //        Global.m_db.InsertUpdateQry(SQL);
        ////    //    }
        ////    //    else if (isnew == false)
        ////    //    {
        ////    //        string username = User.CurrentUserName;
        ////    //        string voucherdate = Convert.ToString(DateTime.Now);
        ////    //        string VoucherType = "LEDGER";
        ////    //        string action = "UPDATE";
        ////    //        int rowid = 0;
        ////    //        string ComputerName = Global.ConcatAllCompInfo;
        ////    //        string desc = oldledgerdesc + newledgerdesc;
        ////    //        string SQL = "INSERT INTO System.tblAuditLog(computername,username,voucher_type,action,rowid,description,voucherdate) VALUES ('" + ComputerName + "','" + username + "','" + VoucherType + "','" + action + "','" + rowid + "','" + desc + "','" + Date.ToDB(DateTime.Now).ToString() + "')";
        ////    //        Global.m_db.InsertUpdateQry(SQL);
        ////    //    }
        ////    //}
        ////    //catch (Exception ex)
        ////    //{
        ////    //    Global.MsgError(ex.Message);
        ////    //}


        ////}


        public void DeleteLedger(int ledgerId)
        {
            string strQuery = "DELETE FROM Acc.tblLedger WHERE LedgerID ='" + ledgerId + "'";
            Global.m_db.SelectQry(strQuery, "Acc.tblLedger");
        }

        public void Delete(string AccountHeadName)
        {
            Global.m_db.SelectQry("DELETE FROM Account.tblAccountHead WHERE AccountHeadName='" + AccountHeadName + "'", "Account.tblAccountHead");

        }

        /// <summary>
        /// Gets the LedgerID of the given ledger name
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="LangField"></param>
        /// <returns></returns>
        public static int GetLedgerIdFromName(string Name, Lang Language)
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
           
            Name = Name.Replace("'", "''");
            object objResult = Global.m_db.GetScalarValue("SELECT LedgerID FROM Acc.tblLedger WHERE " + LangField + "='" + Name + "'");
            return Convert.ToInt32(objResult);
        }

        /// <summary>
        /// If LedgerID is  -1 gives out all ledger iin ledger table, else gives out specific ledger's information
        /// </summary>
        /// <param name="LedgerID"></param>
        /// <param name="Language"></param>
        /// <returns></returns>
        public static DataTable GetLedgerInfo(int? LedgerID, Lang Language)
        {

            DataTable dt = new DataTable();
            string strQuery = "";
            string LangField = "EngName";
            if (Language == Lang.English)
                LangField = "EngName";
            else if (Language == Lang.Nepali)
                LangField = "NepName";
            if (LedgerID == -1)
            {
                strQuery = "SELECT * FROM Acc.tblLedger";
            }
            else
            {

                strQuery = "SELECT led.LedgerID ID,led.LedgerCode LedgerCode, led." + LangField + " LedName, grp." + LangField + " GroupName, " +
                                                "led.PreYrBal PreYrBal,led.PreYrBalDrCr PreYrBalDrCr, led.OpCCRDate OpCCRDate, " +
                                                "led.OpCCR OpCCR, led.OpCCYID opCCYID,  led.Remarks Remarks, " +
                                                "led.PersonName PersonName, led.Address1 Address1, led.Address2 Address2, " +
                                                "led.City City, led.Phone Phone, led.Email Email, led.Company Company, " +
                                                "led.Website Website, led.VatPanNo VatPanNo, led.CreditLimit CreditLimit,led.Calculated Calculated,led.Calculate_Rate Calculate_Rate,led.IsBillReference FROM Acc.tblLedger led  " +
                                                "LEFT OUTER JOIN Acc.tblGroup grp ON led.GroupID=grp.GroupID " +
                                                "WHERE led.LedgerID='" + LedgerID + "'";
            }
            dt = Global.m_db.SelectQry(strQuery, "Search");

            return dt;

        }

        /// <summary>
        /// Gets the information of ledger under the given group_id
        /// </summary>
        /// <param name="Group_ID"></param>
        /// <returns></returns>
        public static DataTable GetLedgerTable(int Group_ID)
        {
            return Global.m_db.SelectQry("SELECT * FROM Acc.tblLedger WHERE GroupID ='" + Group_ID.ToString() + "'", "tblGroup");
        }

        /// <summary>
        /// Fills available ledgers in ComboBox Control
        /// </summary>
        /// <param name="cbo"></param>
        public static void FillLedgerCombo(ComboBox ComboBoxControl)
        {
            #region Language Management

            ComboBoxControl.Font = LangMgr.GetFont();

            string LangField = "English";
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
            ComboBoxControl.Items.Clear();
            DataTable dt = Global.m_db.SelectQry("SELECT GroupID,EngName,NepName FROM Acc.tblGroup", "AccountGroup");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];
                ComboBoxControl.Items.Add(new ListItem((int)dr["GroupID"], dr[LangField].ToString()));
            }
            ComboBoxControl.DisplayMember = "value";
            ComboBoxControl.ValueMember = "id";
        }
        /// <summary>
        /// Returns the string array list of ledgers with the provided group ID, if 0 is provided as group id, it returns all ledgers
        /// </summary>
        /// <param name="GroupID"></param>
        /// <returns></returns>
        public static string[] GetLedgerList(int GroupID)
        {
            #region Language Management



            string LangField = "English";
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
            if (GroupID == 0)//If 0 is the group ID, select all Ledgers
                dt = Global.m_db.SelectQry("SELECT GroupID,EngName,NepName,LedgerCode FROM Acc.tblLedger", "AccountLedger");
            else
                dt = Global.m_db.SelectQry("SELECT GroupID,EngName,NepName,LedgerCode FROM Acc.tblLedger WHERE GroupID='" + GroupID + "'", "AccountLedger");

            List<string> lstLedger = new List<string>();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];
                // lstLedger.Add(dr[LangField].ToString() + "[" + dr["LedgerCode"].ToString() +"]");
                lstLedger.Add(dr[LangField].ToString());

                //ComboBoxControl.Items.Add(new ListItem((int)dr["GroupID"], dr[LangField].ToString()));

            }
            return (string[])lstLedger.ToArray<string>();
            //ComboBoxControl.DisplayMember = "value";
            //ComboBoxControl.ValueMember = "id";
        }

        //budgetalloc is set true for budgetallocation only
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
                #region Ledger Search
                case SearchIn.Ledgers:
                    if (budgetalloc)
                    {
                        string[] str = AccountGroup.GetAllGroupsUnderExpenditure();
                        dtSearch1 = Global.m_db.SelectQry("SELECT led.LedgerID ID,led.LedgerCode LedgerCode, led." + LangField + " LedName, grp." + LangField + " GroupName, led.DrCr DrCr FROM Acc.tblLedger led LEFT OUTER JOIN Acc.tblGroup grp ON led.GroupID=grp.GroupID WHERE led." + LangField + " " + Operator + " '" + Op1 + SearchParam1 + Op2 + "' and  grp.GroupID IN(" + string.Join(",", (string[])str) + ")", "Search");
                    }
                    else
                    {
                        dtSearch1 = Global.m_db.SelectQry("SELECT led.LedgerID ID,led.LedgerCode LedgerCode, led." + LangField + " LedName, grp." + LangField + " GroupName, led.DrCr DrCr FROM Acc.tblLedger led LEFT OUTER JOIN Acc.tblGroup grp ON led.GroupID=grp.GroupID WHERE led." + LangField + " " + Operator + " '" + Op1 + SearchParam1 + Op2 + "'", "Search");
                    }
                    return dtSearch1;
                    break;
                #endregion

                #region Ledger Under Search
                case SearchIn.Ledgers_Under:
                    try
                    {
                        dtSearch1 = Global.m_db.SelectQry("SELECT GroupID FROM Acc.tblGroup WHERE " + LangField + " " + Operator + " '" + Op1 + SearchParam1 + Op2 + "'", "Search");
                        List<string> lstGroupID = new List<string>();
                        for (int i = 0; i < dtSearch1.Rows.Count; i++)
                        {
                            //Also add itself
                            lstGroupID.Add(dtSearch1.Rows[i]["GroupID"].ToString());
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
                            Global.Msg("No data found");
                           // return null;
                            return new DataTable();
                        }
                        DataTable dtSrchLedger;
                        if (budgetalloc)
                        {
                            string[] str = AccountGroup.GetAllGroupsUnderExpenditure();

                            dtSrchLedger = Global.m_db.SelectQry("SELECT led.LedgerID ID, led.LedgerCode LedgerCode, led." + LangField + " LedName, grp." + LangField + " GroupName, led.DrCr DrCr FROM Acc.tblLedger led LEFT OUTER JOIN Acc.tblGroup grp ON led.GroupID=grp.GroupID WHERE grp.GroupID IN(" + string.Join(",", (string[])strResult) + ") and  grp.GroupID IN(" + string.Join(",", (string[])str) + ")", "Search");

                        }
                        else
                        {
                            dtSrchLedger = Global.m_db.SelectQry("SELECT led.LedgerID ID, led.LedgerCode LedgerCode, led." + LangField + " LedName, grp." + LangField + " GroupName, led.DrCr DrCr FROM Acc.tblLedger led LEFT OUTER JOIN Acc.tblGroup grp ON led.GroupID=grp.GroupID WHERE grp.GroupID IN(" + string.Join(",", (string[])strResult) + ")", "Search");
                        }
                        return dtSrchLedger;

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


        public static DataTable  GetLedgerDetail(String AccountingClassIDs, DateTime? TranSactionStartDate, DateTime? TransactionEndDate, int? LedgerID)
        {
            try
            {
                DataTable dt = new DataTable();
                Global.m_db.ClearParameter();
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("Acc.spLedgerDetail");
                Global.m_db.AddParameter("@AccountingClassIDsCSV", SqlDbType.NVarChar, AccountingClassIDs);
                if (TranSactionStartDate != null)
                    Global.m_db.AddParameter("@Transaction_Start_Date", SqlDbType.DateTime, Date.ToDB(Convert.ToDateTime(TranSactionStartDate)));
                else
                    Global.m_db.AddParameter("@Transaction_Start_Date", SqlDbType.DateTime, DBNull.Value);
                if (TransactionEndDate != null)
                    Global.m_db.AddParameter("@Transaction_End_Date", SqlDbType.DateTime, Date.ToDB(Convert.ToDateTime(TransactionEndDate)));
                else
                    Global.m_db.AddParameter("@Transaction_End_Date", SqlDbType.DateTime, DBNull.Value);
                if (LedgerID != null)
                    Global.m_db.AddParameter("@LedgerID", SqlDbType.Int, LedgerID);
                else
                    Global.m_db.AddParameter("@LedgerID", SqlDbType.Int, DBNull.Value);

                dt = Global.m_db.GetDataTable();

                return dt;
            }
            catch (Exception ex)
            {

                throw;
            }

        }



        ///To return according with new procedure spGetLedgerDetails
        public static DataTable GetLedgerDetails(string xmlAccountingClassIDs, string xmlProjectIDs, DateTime? TranSactionStartDate, DateTime? TransactionEndDate, int? LedgerID, string xmlSettings)
        {
            try
            {
                DataTable dt = new DataTable();
                Global.m_db.ClearParameter();
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("Acc.spGetLedgerDetails");
                Global.m_db.AddParameter("@AccountClassIDsSettings", SqlDbType.NVarChar, xmlAccountingClassIDs);
                Global.m_db.AddParameter("@ProjectIDsSettings", SqlDbType.NVarChar, xmlProjectIDs);
                if (TranSactionStartDate != null)
                    Global.m_db.AddParameter("@Transaction_Start_Date", SqlDbType.DateTime, Convert.ToDateTime(TranSactionStartDate));
                else
                    Global.m_db.AddParameter("@Transaction_Start_Date", SqlDbType.DateTime,null); //DBNull.Value);
                if (TransactionEndDate != null)
                    Global.m_db.AddParameter("@Transaction_End_Date", SqlDbType.DateTime, Convert.ToDateTime(TransactionEndDate));
                else
                    Global.m_db.AddParameter("@Transaction_End_Date", SqlDbType.DateTime, null);//DBNull.Value);
                if (LedgerID != null)
                    Global.m_db.AddParameter("@LedgerID", SqlDbType.Int, LedgerID);
                else
                    Global.m_db.AddParameter("@LedgerID", SqlDbType.Int, null);//DBNull.Value);

                //if (GroupID != null)
                //    Global.m_db.AddParameter("@GroupID", SqlDbType.Int, GroupID);
                //else
                //    Global.m_db.AddParameter("@GroupID", SqlDbType.Int, null);//DBNull.Value);

                Global.m_db.AddParameter("@Settings", SqlDbType.Xml, xmlSettings);
             
                dt = Global.m_db.GetDataTable();

                return dt;
            }
            catch (Exception ex)
            {

                throw;
            }

        }


        public static DataTable GetLedgerDetails1(string xmlAccountingClassIDs, string xmlProjectIDs, DateTime? TranSactionStartDate, DateTime? TransactionEndDate, int? LedgerID, int? GroupID, string xmlSettings)
        {
            try
            {
                DataTable dt = new DataTable();
                Global.m_db.ClearParameter();
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("Acc.spGetLedgerDetailsByGroupOrLedgerID");
                Global.m_db.AddParameter("@AccountClassIDsSettings", SqlDbType.NVarChar, xmlAccountingClassIDs);
                Global.m_db.AddParameter("@ProjectIDsSettings", SqlDbType.NVarChar, xmlProjectIDs);
                if (TranSactionStartDate != null)
                    Global.m_db.AddParameter("@Transaction_Start_Date", SqlDbType.DateTime, Convert.ToDateTime(TranSactionStartDate));
                else
                    Global.m_db.AddParameter("@Transaction_Start_Date", SqlDbType.DateTime, null); //DBNull.Value);
                if (TransactionEndDate != null)
                    Global.m_db.AddParameter("@Transaction_End_Date", SqlDbType.DateTime, Convert.ToDateTime(TransactionEndDate));
                else
                    Global.m_db.AddParameter("@Transaction_End_Date", SqlDbType.DateTime, null);//DBNull.Value);
                if (LedgerID != null)
                    Global.m_db.AddParameter("@LedgerID", SqlDbType.Int, LedgerID);
                else
                    Global.m_db.AddParameter("@LedgerID", SqlDbType.Int, null);//DBNull.Value);

                if (GroupID != null)
                    Global.m_db.AddParameter("@GroupID", SqlDbType.Int, GroupID);
                else
                    Global.m_db.AddParameter("@GroupID", SqlDbType.Int, null);//DBNull.Value);

                Global.m_db.AddParameter("@Settings", SqlDbType.Xml, xmlSettings);

                dt = Global.m_db.GetDataTable();

                return dt;
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        /// <summary>
        /// Returns all ledgers in the given group. Also gets its subgroups.
        /// </summary>
        /// <param name="Group_ID"></param>
        /// <returns></returns>
        public static DataTable GetAllLedger(int Group_ID)
        {
            if (Group_ID == 0)
            {
                DataTable dtAllLedger = Global.m_db.SelectQry("Select L.LedgerID, L.LedgerCode, L.EngName as LedgerName, L.GroupID, G.EngName as GroupName FROM Acc.tblLedger as L, Acc.tblGroup as G WHERE L.GroupID = G.GroupID", "Acc.tblLedger");
                return dtAllLedger;
            }

            ArrayList tmpGroupID = new ArrayList();//Dyanamically allocating array type of variable tmpGroupID
            AccountGroup.GetAccountsUnder(Group_ID, tmpGroupID);//Calling this function for collecting subGroupsID which fall under GroupID and storing on arrylist
            tmpGroupID.Add(Group_ID);
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

            string LedgerID = "";

            int i1 = 0;
            foreach (DataRow dr in dt.Rows)
            {
                if (i1 == 0)//for First LedgerID
                    LedgerID = "'" + (dr["LedgerID"].ToString()) + "'";
                else  //separating other LedgerID by comma

                    LedgerID += "," + "'" + (dr["LedgerID"].ToString()) + "'";

                i1++;
            }
            DataTable dt1;


            if (LedgerID.Length > 0)
                dt1 = Global.m_db.SelectQry("SELECT * FROM Acc.tblLedger WHERE LedgerID IN (" + (LedgerID) + ")", "LedgerID");
            else
                dt1 = new DataTable();
            return dt1;
        }

        public static bool IsCashAccount(string ledgerName)
        {
            bool returnValue = false;
            int cashInHand = AccountGroup.GetGroupIDFromGroupNumber(102); //Get Cash Account Group with help of GroupNumber
            DataTable dtCashLedgers = Ledger.GetAllLedger(cashInHand); //Get All ledgers Corresponding to Cash in Hand Group
            foreach (DataRow drCashLedgers in dtCashLedgers.Rows)
            {
                if (ledgerName == drCashLedgers["EngName"].ToString())
                {
                    returnValue = true;
                }
            }
            return returnValue;
        }

        public static bool IsBankAccount(string ledgerName)
        {
            bool returnValue = false;
            int bankId = AccountGroup.GetGroupIDFromGroupNumber(7);//Get Bank Account Group with help of GroupNumber
            DataTable dtBankLedgers = Ledger.GetAllLedger(bankId);//Get All ledgers Corresponding to Cash in Hand Group
            foreach (DataRow drBankLedgers in dtBankLedgers.Rows)
            {
                if (ledgerName == drBankLedgers["EngName"].ToString())
                {
                    returnValue = true;
                }
            }
            return returnValue;
        }

        public static DataTable GetLedgerConfig()
        {
            DataTable dt = Global.m_db.SelectQry("select NumberingType,SeriesID from acc.tblLedgerCodeConfig", "Config");
            return dt;
        }

        public static DataTable GetFormatParameter()
        {
            DataTable dt = Global.m_db.SelectQry("select Type, parameter from acc.tblLedgerCodeFormat  ", "Parameter");
            return dt;
        }

        public static DataTable GetGroupConfig()
        {
            DataTable dt = Global.m_db.SelectQry("select NumberingType,SeriesID from acc.tblGroupCodeConfig", "Config");
            return dt;
        }

        public static DataTable GetGrpFormatParameter()
        {
            DataTable dt = Global.m_db.SelectQry("select Type, parameter from acc.tblGroupCodeFormat  ", "Parameter");
            return dt;
        }

        public static DataTable GetAllLedger1(int Group_ID)
        {
            ArrayList temGroupID = new ArrayList();
            AccountGroup.GetAccountsUnder(Group_ID, temGroupID);
            temGroupID.Add(Group_ID);
            string GroupID1 = "";
            int i = 0;
            foreach (object j in temGroupID)
            {
                if (i == 0)// for first GroupID
                    GroupID1 = "'" + j.ToString() + "'";
                else  //Separating Other GroupID by commas
                    GroupID1 += "," + "'" + j.ToString() + "'";
                i++;
            }
            DataTable dt = Global.m_db.SelectQry("SELECT * FROM Acc.tblLedger WHERE GroupID IN (" + (GroupID1) + ")", "GroupID");
            string LedgerID = "";
            int i1 = 0;
            foreach (DataRow dr in dt.Rows)
            {
                if (i1 == 0)//for First LedgerID
                    LedgerID = "'" + (dr["LedgerID"].ToString()) + "'";
                else  //separating other LedgerID by comma

                    LedgerID += "," + "'" + (dr["LedgerID"].ToString()) + "'";

                i1++;
            }
            DataTable dt1;
            if (LedgerID.Length > 0)
            {
                dt1 = Global.m_db.SelectQry("SELECT l.LedgerID,l.EngName,isnull(d.DepreciationPercentage,0) Depreciatition FROM Acc.tblLedger l left OUTER JOIN System.tblDepreciation d on L.LedgerID=D.LedgerID where l.LedgerID IN (" + (LedgerID) + ")", "LedgerID");
            }
            else
            {
                dt1 = new DataTable();
            }

            return dt1;
            //if (LedgerID.Length > 0)
            //    dt1 = Global.m_db.SelectQry("SELECT * FROM Acc.tblLedger WHERE LedgerID IN (" + (LedgerID) + ")", "LedgerID");
            //else
            //    dt1 = new DataTable();
            //return dt1;

            //DataTable dt1 = Global.m_db.SelectQry("SELECT l.LedgerID,l.EngName,isnull(d.DepreciationPercentage,0) Depreciatition FROM Acc.tblLedger l left OUTER JOIN System.tblDepreciation d on L.LedgerID=D.LedgerID WHERE groupid='"+Group_ID+"'", "LedgerID");
            //return dt1;
        }

        public static DataTable GetPreviousYearPL(int accid,int ledgerid)
        {
            DataTable dt = Global.m_db.SelectQry("select OpenBal Amount,OpenBalDrCr DrCr from Acc.tblOpeningBalance where LedgerID='"+ledgerid+"' and  AccClassID='"+accid+"'", "GetPreviousYearPL");
            return dt;
        }

        public static DataTable GetAllLedgerValues()
        {
            return Global.m_db.SelectQry("SELECT * FROM Acc.tblLedger ", "tblGroup");
        }

        public static string GetLedgerNameFromID(int id)
        {
            object objResult = Global.m_db.GetScalarValue("SELECT EngName FROM Acc.tblLedger WHERE LedgerID='" + id + "'");
            if (objResult != null)
                return objResult.ToString();

            else return null;
        }

        public static DataTable GetAllLedgerDetailsByIDForAgeing(int ledgerid)
        {
            return Global.m_db.SelectQry("SELECT * FROM Acc.tblLedger where ledgerid='"+ledgerid+"' ", "tblledger");
        }

        public static DataTable GetLedgerIDnName(int LedgerID)
        {
            if(LedgerID>0)
                return Global.m_db.SelectQry("SELECT LedgerID,EngName FROM Acc.tblLedger where LedgerID='" + LedgerID + "' ", "tblledger");
            else
                return Global.m_db.SelectQry("SELECT LedgerID,EngName FROM Acc.tblLedger","tblledger");


        }

        public static DataTable GetLedgerOpenBal(string xmlAccountingClassIDs, int? LedgerID, int? GroupID,DateTime? fromDate)
        {
            try
            {

                DataTable dt = new DataTable();
                Global.m_db.ClearParameter();
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("Acc.spGetLedgerNameNOpBal");
                Global.m_db.AddParameter("@AccountClassIDsSettings", SqlDbType.NVarChar, xmlAccountingClassIDs);
                if (LedgerID != null)
                    Global.m_db.AddParameter("@LedgerID", SqlDbType.Int, LedgerID);
                else
                    Global.m_db.AddParameter("@LedgerID", SqlDbType.Int, null);//DBNull.Value);
                if (GroupID != null)
                    Global.m_db.AddParameter("@GroupID", SqlDbType.Int, GroupID);
                else
                    Global.m_db.AddParameter("@GroupID", SqlDbType.Int, null);//DBNull.Value);

                if (fromDate != null)
                {
                    // fromDate = ((DateTime)fromDate).AddDays(-1);
                    Global.m_db.AddParameter("@fromDate", SqlDbType.Date, fromDate);
                }
                else
                    Global.m_db.AddParameter("@fromDate", SqlDbType.Date, null);//DBNull.Value);
                dt = Global.m_db.GetDataTable();

                return dt;

            }
                    catch(Exception ex)
            {
                throw;
            }
        }
        public static  DataTable GetPrimaryGroupIDnName()
        {
            return Global.m_db.SelectQry("select GroupID,EngName,LedgerCode from Acc.tblGroup where Parent_GrpID is NULL", "Group");
        }
        public static DataTable GetGroupIDnNameFromtblLedger()
        {
            return Global.m_db.SelectQry("select distinct g.GroupID,g.EngName,g.LedgerCode from Acc.tblGroup as g ,Acc.tblLedger as l  where g.GroupID=l.GroupID", "Group");
        }

        public static DataTable GetAllGroupNLedgerBal(string xmlAccountingClassIDs, string xmlProjectIDs, DateTime? TranSactionStartDate, DateTime? TransactionEndDate,int GroupID, string xmlSettings)
        {
            try
            {
                DataTable dt = new DataTable();
                Global.m_db.ClearParameter();
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("[Acc].[spGetGroupNldgBal]");
                Global.m_db.AddParameter("@AccountClassIDsSettings", SqlDbType.NVarChar, xmlAccountingClassIDs);
                Global.m_db.AddParameter("@ProjectIDsSettings", SqlDbType.NVarChar, xmlProjectIDs);
                if (TranSactionStartDate != null)
                    Global.m_db.AddParameter("@Transaction_Start_Date", SqlDbType.DateTime, Convert.ToDateTime(TranSactionStartDate));
                else
                    Global.m_db.AddParameter("@Transaction_Start_Date", SqlDbType.DateTime, null); //DBNull.Value);
                if (TransactionEndDate != null)
                    Global.m_db.AddParameter("@Transaction_End_Date", SqlDbType.DateTime, Convert.ToDateTime(TransactionEndDate));
                else
                    Global.m_db.AddParameter("@Transaction_End_Date", SqlDbType.DateTime, null);//DBNull.Value);      
                Global.m_db.AddParameter("@GroupID", SqlDbType.Int,GroupID);
                Global.m_db.AddParameter("@Settings", SqlDbType.Xml, xmlSettings);

                dt = Global.m_db.GetDataTable();

                return dt;
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        public static DataTable FilterLedgersByGroup(string ledgerIDs,int childGroupID,int groupID)
        {
              try
            {
                DataTable dt = new DataTable();
                Global.m_db.ClearParameter();
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("[ACC].[spFilterLedgersOrGroupByGroupID]");
                if (ledgerIDs != null || ledgerIDs != "")
                Global.m_db.AddParameter("@LedgersID", SqlDbType.NVarChar, ledgerIDs);
                  else
                 Global.m_db.AddParameter("@LedgersID", SqlDbType.NVarChar, null);

                Global.m_db.AddParameter("@ChildGroupID", SqlDbType.Int, childGroupID);
                Global.m_db.AddParameter("@GroupID", SqlDbType.Int, groupID);
                dt = Global.m_db.GetDataTable();

                return dt;
            }
              catch (Exception ex)
              {
                  throw;
              }
        }
    }
}
