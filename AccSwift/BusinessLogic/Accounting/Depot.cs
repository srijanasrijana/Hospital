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
    public class Depot
    {

        public void Save(string DepotName, string City, string Telephone, string ContactPerson,string LicenceNo,string DepotAddress,string PostalCode,string Mobile,string RegNo,string Remarks,string olddepot,string newdepot,bool isnew)
        {


            Global.m_db.setCommandType(CommandType.StoredProcedure);
            Global.m_db.setCommandText("Inv.spDepotCreate");
            Global.m_db.AddParameter("@DepotName", SqlDbType.NVarChar, 150, DepotName);
            Global.m_db.AddParameter("@City", SqlDbType.NVarChar, 150, City);
            Global.m_db.AddParameter("@Telephone", SqlDbType.NVarChar, 150, Telephone);
            Global.m_db.AddParameter("@ContactPerson", SqlDbType.NVarChar, 150, ContactPerson);
            Global.m_db.AddParameter("@LicenceNo", SqlDbType.NVarChar, 150, LicenceNo);
            Global.m_db.AddParameter("@DepotAddress", SqlDbType.NVarChar, 150, DepotAddress);
            Global.m_db.AddParameter("@PostalCode", SqlDbType.NVarChar, 150, PostalCode);
            Global.m_db.AddParameter("@Mobile", SqlDbType.NVarChar, 150, Mobile);
            Global.m_db.AddParameter("@RegNo", SqlDbType.NVarChar, 150, RegNo);
            Global.m_db.AddParameter("@Remarks", SqlDbType.NVarChar, 200, Remarks);
            Global.m_db.AddParameter("@Created_By", SqlDbType.NVarChar, 50, User.CurrUserID.ToString());
            System.Data.SqlClient.SqlParameter objReturn = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
            Global.m_db.ProcessParameter();

            try
            {
                if (isnew == true)
                {
                    string username = User.CurrentUserName;
                    string voucherdate = Date.ToDB(DateTime.Now).ToString();
                    string VoucherType = "DEPOT";
                    string action = "INSERT";
                    int rowid = 0;
                    string ComputerName = Global.ComputerName;
                    string MacAddress = Global.MacAddess;
                    string IpAddress = Global.IpAddress;
                    string desc = olddepot + newdepot;

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
                    string VoucherType = "DEPOT";
                    string action = "UPDATE";
                    int rowid = 0;
                    string ComputerName = Global.ComputerName;
                    string MacAddress = Global.MacAddess;
                    string IpAddress = Global.IpAddress;
                    string desc = olddepot + newdepot;

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

        }

        public void Modify(int DepotID, string DepotName, string City, string Telephone, string ContactPerson, string LicenceNo, string DepotAddress, string PostalCode, string Mobile, string RegNo, string Remarks, string olddepot, string newdepot, bool isnew)
        {

            try
            {

                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("Inv.spDepotModify");
                Global.m_db.AddParameter("@DepotID", SqlDbType.Int, DepotID);
                Global.m_db.AddParameter("@DepotName", SqlDbType.NVarChar, 150, DepotName);
                Global.m_db.AddParameter("@City", SqlDbType.NVarChar, 150, City);
                Global.m_db.AddParameter("@Telephone", SqlDbType.NVarChar, 150, Telephone);
                Global.m_db.AddParameter("@ContactPerson", SqlDbType.NVarChar, 150, ContactPerson);
                Global.m_db.AddParameter("@LicenceNo", SqlDbType.NVarChar, 150, LicenceNo);
                Global.m_db.AddParameter("@DepotAddress", SqlDbType.NVarChar, 150, DepotAddress);
                Global.m_db.AddParameter("@PostalCode", SqlDbType.NVarChar, 150, PostalCode);
                Global.m_db.AddParameter("@Mobile", SqlDbType.NVarChar, 150, Mobile);
                Global.m_db.AddParameter("@RegNo", SqlDbType.NVarChar, 150, RegNo);
                Global.m_db.AddParameter("@Remarks", SqlDbType.NVarChar, 200, Remarks);
                Global.m_db.AddParameter("@Modified_By", SqlDbType.NVarChar, 50, User.CurrUserID.ToString());
                System.Data.SqlClient.SqlParameter objReturn = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                Global.m_db.ProcessParameter();

                try
                {
                    if (isnew == true)
                    {
                        string username = User.CurrentUserName;
                        string voucherdate = Date.ToDB(DateTime.Now).ToString();
                        string VoucherType = "DEPOT";
                        string action = "INSERT";
                        int rowid = 0;
                        string ComputerName = Global.ComputerName;
                        string MacAddress = Global.MacAddess;
                        string IpAddress = Global.IpAddress;
                        string desc = olddepot + newdepot;

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
                        string VoucherType = "DEPOT";
                        string action = "UPDATE";
                        int rowid = 0;
                        string ComputerName = Global.ComputerName;
                        string MacAddress = Global.MacAddess;
                        string IpAddress = Global.IpAddress;
                        string desc = olddepot + newdepot;

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
            }
            catch (Exception ex)
            {

                throw ex;
            }


        }

        public void Delete(int DepotID)
        {

            Global.m_db.InsertUpdateQry("DELETE FROM Inv.tblDepot WHERE DepotID ='" + DepotID + "'");

        }    

        public static DataTable GetDepotInfo(int DepotID)
        {

            if (DepotID == -1)
            {
                return Global.m_db.SelectQry("SELECT * FROM Inv.tblDepot ", "tbl");
            }
            else
            {

                return Global.m_db.SelectQry("SELECT * FROM Inv.tblDepot WHERE DepotID = '" + DepotID + "'", "tbl");

            }



        }

        public static DataTable FindTransactPresent(int DepotID)
        {
            return Global.m_db.SelectQry("SELECT top 1 * FROM inv.tblInventoryTrans WHERE DepotID = '" + DepotID + "'", "tbl");
        }

    }
}
