using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;
using DateManager;

namespace BusinessLogic
{
    public class PurchaseOrder
    {
        public void Create(int CashPartyID, int OrderNo, DateTime PurchaseOrder_Date, string Remarks, DataTable PurchaseOrderDetails, int[] AccClassID, int ProjectID, string oldgrid, string newgrid, bool isnew, DataTable dtVoucherRecurring)
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
            //Check if the PurchaseInvoiceDetails has fields
            if (PurchaseOrderDetails.Rows.Count == 0)
            {
                throw new Exception("Please fill the ledger details");
                return;
            }
            ////This loop is to check whether ledger names are correct and properly implemented
            foreach (DataRow row in PurchaseOrderDetails.Rows)
            {
                //Check whether the ledger name are correct
                Global.m_db.ClearParameter();
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("Inv.spGetProductIDFromName");
                Global.m_db.AddParameter("@ProductName", SqlDbType.NVarChar, 50, row["Product"].ToString());
                Global.m_db.AddParameter("@Language", SqlDbType.NVarChar, 20, Language);
                System.Data.SqlClient.SqlParameter objReturn = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                Global.m_db.ProcessParameter();

                //if (objReturn.Value.ToString() == "-100")//It return -100 in case of failure
                //    throw new Exception("Product Name - " + row["Product"].ToString() + " not found!");
            }
            try
            {
                // Global.m_db.InsertUpdateQry("INSERT INTO Account.tblAccountHead(AccountHeadName,Under_Account_ID,Description,BuiltIn) VALUES ('" + AccountHeadName + "','1','" + Description + "','0')");
                Global.m_db.BeginTransaction();
                Global.m_db.ClearParameter();
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("Inv.spPurchaseOrderCreate");
                Global.m_db.AddParameter("@CashPartyID", SqlDbType.Int, CashPartyID);
                Global.m_db.AddParameter("@PurchaseOrder_Date", SqlDbType.DateTime, PurchaseOrder_Date);
                Global.m_db.AddParameter("@OrderNo", SqlDbType.Int, OrderNo);
                Global.m_db.AddParameter("@Remarks", SqlDbType.NVarChar, 200, Remarks);
                Global.m_db.AddParameter("@ProjectID", SqlDbType.Int, ProjectID);
                Global.m_db.AddParameter("@Created_By", SqlDbType.NVarChar, 50, User.CurrUserID.ToString());
                System.Data.SqlClient.SqlParameter objReturn = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                Global.m_db.ProcessParameter();
                int ReturnID = Convert.ToInt32(objReturn.Value);
                int? UpdatedQty = null;
                int? PendingQty = null;
                for (int i = 0; i < PurchaseOrderDetails.Rows.Count; i++)
                {
                    DataRow dr = PurchaseOrderDetails.Rows[i];
                    //Now go for the Detail Inserts
                    Global.m_db.ClearParameter();
                    Global.m_db.setCommandType(CommandType.StoredProcedure);
                    Global.m_db.setCommandText("Inv.spPurchaseOrderDetailCreate");
                    Global.m_db.AddParameter("@PurchaseOrderID", SqlDbType.Int, ReturnID.ToString());
                    Global.m_db.AddParameter("@ProductCode", SqlDbType.NVarChar, 30, dr["ProductCode"].ToString());
                    Global.m_db.AddParameter("@ProductID", SqlDbType.NVarChar, 30, dr["ProductID"].ToString());//Set same for both for time being
                    Global.m_db.AddParameter("@Language", SqlDbType.NVarChar, 30, Language);
                    Global.m_db.AddParameter("@Quantity", SqlDbType.Float, Convert.ToDouble(dr["Quantity"]));
                    Global.m_db.AddParameter("@PurchaseRate", SqlDbType.Money, Convert.ToDouble(dr["PurchaseRate"]));
                    Global.m_db.AddParameter("@Amount", SqlDbType.Money, Convert.ToDouble(dr["Amount"]));
                    if (dr["UpdatedQty"].ToString() != "")
                        Global.m_db.AddParameter("@UpdatedQty", SqlDbType.Int, Convert.ToDouble(dr["UpdatedQty"]));
                    else
                        Global.m_db.AddParameter("@UpdatedQty", SqlDbType.Int, UpdatedQty);
                    if (dr["PendingQty"].ToString() != "")
                        Global.m_db.AddParameter("@PendingQty", SqlDbType.Int, Convert.ToDouble(dr["PendingQty"]));
                    else
                        Global.m_db.AddParameter("@PendingQty", SqlDbType.Int, PendingQty);
                    System.Data.SqlClient.SqlParameter objReturn1 = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                    Global.m_db.ProcessParameter();


                }

                foreach (int _AccClassID in AccClassID)
                {
                    Global.m_db.ClearParameter();
                    Global.m_db.setCommandType(CommandType.StoredProcedure);
                    Global.m_db.setCommandText("Inv.spPurchaseOrderAccClassCreate");
                    Global.m_db.AddParameter("@PurchaseOrderID", SqlDbType.Int, ReturnID.ToString());
                    Global.m_db.AddParameter("@AccClassID", SqlDbType.Int, _AccClassID);//Set same for both for time being
                    System.Data.SqlClient.SqlParameter paramTransactClassID = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                    Global.m_db.ProcessParameter();

                    if (paramTransactClassID.Value.ToString() == "FAILURE")
                    {
                        Global.m_db.RollBackTransaction();
                        throw new Exception("Unable to create Purchase Order");
                    }
                }

                try
                {
                    if (isnew == true)
                    {
                        string username = User.CurrentUserName;
                        string voucherdate = Date.ToDB(DateTime.Now).ToString();
                        string VoucherType = "PORD";
                        string action = "INSERT";
                        int rowid = 0;
                        string ComputerName = Global.ComputerName;
                        string MacAddress = Global.MacAddess;
                        string IpAddress = Global.IpAddress;
                        string desc = oldgrid + newgrid;

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
                        string VoucherType = "PORD";
                        string action = "UPDATE";
                        int rowid = 0;
                        string ComputerName = Global.ComputerName;
                        string MacAddress = Global.MacAddess;
                        string IpAddress = Global.IpAddress;
                        string desc = oldgrid + newgrid;

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

                #region Save voucher recurring settings
                if (dtVoucherRecurring.Rows.Count > 0)
                {
                    dtVoucherRecurring.Rows[0]["VoucherID"] = ReturnID;
                    dtVoucherRecurring.Rows[0]["VoucherType"] = "PURCHASE_ORDER";
                    string res = RecurringVoucher.CreateRecurringVoucherSetting(dtVoucherRecurring);
                    if (res == "FAILURE")
                    {
                        Global.m_db.RollBackTransaction();
                        throw new Exception("Unable to create Purchase Order due to recurring settings.");
                    }
                }
                #endregion

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
                ////}
                #endregion
            }
        }

        public void Modify(int PurchaseOrderID, int CashPartyID, string OrderNo, DateTime PurchaseOrder_Date, string Remarks, DataTable PurchaseOrderDetails, int[] AccClassID, int ProjectID, string oldgrid, string newgrid, bool isnew, DataTable dtVoucherRecurring)
        {

            try
            {
                Global.m_db.BeginTransaction();
                Global.m_db.ClearParameter();
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("Inv.spPurchaseOrderModify");
                Global.m_db.AddParameter("@PurchaseOrderID", SqlDbType.Int, PurchaseOrderID);
                Global.m_db.AddParameter("@CashPartyID", SqlDbType.Int, CashPartyID);
                Global.m_db.AddParameter("@PurchaseOrder_Date", SqlDbType.DateTime, PurchaseOrder_Date);
                Global.m_db.AddParameter("@OrderNo", SqlDbType.NVarChar, OrderNo);
                Global.m_db.AddParameter("@Remarks", SqlDbType.NVarChar, 200, Remarks);
                Global.m_db.AddParameter("@ProjectID", SqlDbType.Int, ProjectID);
                Global.m_db.AddParameter("@Modified_By", SqlDbType.NVarChar, 50, User.CurrUserID.ToString());
                System.Data.SqlClient.SqlParameter objReturn = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                Global.m_db.ProcessParameter();
                //int ReturnID = Convert.ToInt32(objReturn.Value);

                //First delete the old record
                Global.m_db.InsertUpdateQry("DELETE FROM Inv.tblPurchaseOrderDetails WHERE PurchaseOrderID='" + PurchaseOrderID.ToString() + "'");

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
                int? UpdatedQty = null;
                int? PendingQty = null;
                for (int i = 0; i < PurchaseOrderDetails.Rows.Count; i++)
                {
                    DataRow dr = PurchaseOrderDetails.Rows[i];
                    //Now go for the Detail Inserts
                    Global.m_db.ClearParameter();
                    Global.m_db.setCommandType(CommandType.StoredProcedure);
                    Global.m_db.setCommandText("Inv.spPurchaseOrderDetailCreate");
                    Global.m_db.AddParameter("@PurchaseOrderID", SqlDbType.Int, PurchaseOrderID.ToString());
                    Global.m_db.AddParameter("@ProductCode", SqlDbType.NVarChar, 30, dr["ProductCode"].ToString());
                    Global.m_db.AddParameter("@ProductID", SqlDbType.NVarChar, 30, dr["ProductID"].ToString());//Set same for both for time being
                    Global.m_db.AddParameter("@Language", SqlDbType.NVarChar, 30, Language);
                    Global.m_db.AddParameter("@Quantity", SqlDbType.Float, Convert.ToDouble(dr["Quantity"]));
                    Global.m_db.AddParameter("@PurchaseRate", SqlDbType.Money, Convert.ToDouble(dr["PurchaseRate"]));
                    Global.m_db.AddParameter("@Amount", SqlDbType.Money, Convert.ToDouble(dr["Amount"]));
                    if (dr["UpdatedQty"].ToString() != "")
                    {
                        Global.m_db.AddParameter("@UpdatedQty", SqlDbType.Money, Convert.ToDouble(dr["UpdatedQty"]));
                    }
                    else
                        Global.m_db.AddParameter("@UpdatedQty", SqlDbType.Money, UpdatedQty);
                    if (dr["PendingQty"].ToString() != "")
                    {
                        Global.m_db.AddParameter("@PendingQty", SqlDbType.Money, Convert.ToDouble(dr["PendingQty"]));
                    }
                    else
                    {
                        Global.m_db.AddParameter("@PendingQty", SqlDbType.Money, PendingQty);
                    }
                    System.Data.SqlClient.SqlParameter paramReturn = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                    Global.m_db.ProcessParameter();
                    if (paramReturn.Value.ToString() != "SUCCESS")
                    {
                        Global.m_db.RollBackTransaction();
                        throw new Exception("Unable to create Purchase Order");
                    }
                    if (paramReturn.Value.ToString() != "SUCCESS")
                    {
                        Global.m_db.RollBackTransaction();
                        throw new Exception("Unable to create Purchase Order");
                    }

                }

                foreach (int _AccClassID in AccClassID)
                {
                    Global.m_db.ClearParameter();
                    Global.m_db.setCommandType(CommandType.StoredProcedure);
                    Global.m_db.setCommandText("Inv.spPurchaseOrderAccClassCreate");
                    Global.m_db.AddParameter("@PurchaseOrderID", SqlDbType.Int, PurchaseOrderID.ToString());
                    Global.m_db.AddParameter("@AccClassID", SqlDbType.Int, _AccClassID);//Set same for both for time being
                    System.Data.SqlClient.SqlParameter paramTransactClassID = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                    Global.m_db.ProcessParameter();

                    if (paramTransactClassID.Value.ToString() == "FAILURE")
                    {
                        Global.m_db.RollBackTransaction();
                        throw new Exception("Unable to create Purchase Order");
                    }
                }

                try
                {
                    if (isnew == true)
                    {
                        string username = User.CurrentUserName;
                        string voucherdate = Date.ToDB(DateTime.Now).ToString();
                        string VoucherType = "PORD";
                        string action = "INSERT";
                        int rowid = 0;
                        string ComputerName = Global.ComputerName;
                        string MacAddress = Global.MacAddess;
                        string IpAddress = Global.IpAddress;
                        string desc = oldgrid + newgrid;

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
                        string VoucherType = "PORD";
                        string action = "UPDATE";
                        int rowid = 0;
                        string ComputerName = Global.ComputerName;
                        string MacAddress = Global.MacAddess;
                        string IpAddress = Global.IpAddress;
                        string desc = oldgrid + newgrid;

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


                #region Save voucher recurring settings
                if (dtVoucherRecurring.Rows.Count > 0)
                {
                    dtVoucherRecurring.Rows[0]["VoucherType"] = "PURCHASE_ORDER";
                    dtVoucherRecurring.Rows[0]["VoucherID"] = PurchaseOrderID;
                    string result = RecurringVoucher.ModifyRecurringVoucherSetting(dtVoucherRecurring);

                    if (result == "FAILURE")
                    {
                        Global.m_db.RollBackTransaction();
                        throw new Exception("Unable to modify Purchase Order due to recurring settings.");
                    }
                }
                #endregion

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
                ////}
                #endregion
            }
        }

        public DataTable GetPurchaseOrderMasterInfo(string RowID)
        {
            string strQuery = "";
            strQuery = "SELECT * FROM Inv.tblPurchaseOrderMaster WHERE PurchaseOrderID ='" + RowID + "'";
            return Global.m_db.SelectQry(strQuery, "table");
        }

        public static DataTable GetPurchaseOrderMasterInfoByOrderNo(int OrderNo)
        {
            string strQuery = "";
            //Getting Maximum value of OrderNumber
            if (OrderNo == -1)//Getting maximum value of OrderNumber form tblPurchaseOrderMaster
            {
                strQuery = "SELECT MAX (OrderNo) AS OrderNo From Inv.tblPurchaseOrderMaster";

            }
            else//Getting PurchaseOrderMasterInfo with help of Order Number
            {
                strQuery = "SELECT * FROM Inv.tblPurchaseOrderMaster WHERE OrderNo ='" + OrderNo + "'";
            }

            return Global.m_db.SelectQry(strQuery, "dt");

        }

        public static DataTable GetPurchaseOrderDetailsByDetailsID(int PurchaseOrderDetailsID)
        {
            return Global.m_db.SelectQry("SELECT * FROM Inv.tblPurchaseOrderDetails WHERE PurchaseOrderDetailsID ='" + PurchaseOrderDetailsID + "'", "tbl");
        }

        public DataTable GetPurchaseOrderDetails(int MasterID)
        {
            Global.m_db.ClearParameter();
            Global.m_db.setCommandType(CommandType.StoredProcedure);
            Global.m_db.setCommandText("Inv.spGetPurchaseOrderDetail");
            Global.m_db.AddParameter("@MasterID", SqlDbType.Int, MasterID);
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
            DataTable dtPurchaseOrderDtl = Global.m_db.GetDataTable();
            return dtPurchaseOrderDtl;
        }

        public DataTable NavigatePurchaseOrderMaster(int CurrentID, Navigate NavTo)
        {
            Global.m_db.ClearParameter();
            Global.m_db.setCommandType(CommandType.StoredProcedure);
            Global.m_db.setCommandText("Inv.spPurchaseOrderNavigate");
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

            DataTable dtPurchaseOrderMst = Global.m_db.GetDataTable();
            return dtPurchaseOrderMst;
        }
    }
}
