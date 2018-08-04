using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;
using System.Data.SqlClient;
using System.Windows.Forms;
using DateManager;
using Language;
using DBLogic;


namespace BusinessLogic
{


   
    public interface IProduct
    {

        void Create(string GroupName, string ParentGroup, string Remarks,string oldpgroupdesc,string newgroupdesc,bool isnew,long textcolor);
        void Modify(int GroupID, string NewGroupName, string ParentGroupName, string Remarks,string oldpgroupdesc,string newgroupdesc,bool isnew,long backcolor);
        void Delete(string ProductHeadName);
        bool CreateProduct(string ProductName, int GroupID, string ProductCode, string ProductColor, int? DepotID, string Remarks, int? UnitMaintenanceID, double? SalesRate, int? Quantity, double? PurchaseRate, double? PurchaseDiscount, double? TotalValue, byte[] Image,string oldproductdesc,string newproductdesc,bool isnew,long ProductTxtColor,int isVatApplicable,int isInventoryApplicable, int isDecimalApplicable);
        bool ModifyProduct(int ProductID, string ProductName, int GroupID, string ProductCode, string ProductColor, int? DepotID, string Remarks, int? UnitMaintenanceID, double? SalesRate, int? Quantity, double? PurchaseRate, double? PurchaseDiscount, double? TotalValue, byte[] Image, string oldproductdesc, string newproductdesc, bool isnew, long ProductTextColor, int isVatApplicable, int isInventoryApplicable, int isDecimalApplicable);
    }

    public class Product:IProduct
    {
        
        public void Create(string GroupName, string ParentGroup, string Remarks,string oldpgroupdesc,string newgroupdesc,bool isnew,long textcolor)
        {
            try
            {
                // Global.m_db.InsertUpdateQry("INSERT INTO Account.tblAccountHead(AccountHeadName,Under_Account_ID,Description,BuiltIn) VALUES ('" + AccountHeadName + "','1','" + Description + "','0')");

                Global.m_db.ClearParameter();
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("Inv.spProductGroupCreate");
                Global.m_db.AddParameter("@EngName", SqlDbType.NVarChar, 50, GroupName);
                Global.m_db.AddParameter("@NepName", SqlDbType.NVarChar, 50, GroupName);//Set same for both for time being
                Global.m_db.AddParameter("@Under_EngName", SqlDbType.NVarChar, 50, ParentGroup);
                Global.m_db.AddParameter("@BackColor",SqlDbType.BigInt,textcolor);
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
                    string VoucherType = "PGROUP";
                    string action = "INSERT";
                    int rowid = 0;
                    string ComputerName = Global.ComputerName;
                    string MacAddress = Global.MacAddess;
                    string IpAddress = Global.IpAddress;
                    string desc = oldpgroupdesc+newgroupdesc;

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
                    object objReturn = Global.m_db.AddOutputParameter("@Result", SqlDbType.NVarChar, 20);
                    Global.m_db.ProcessParameter();
                    //string SQL = "INSERT INTO System.tblAuditLog(computername,username,voucher_type,action,rowid,description,voucherdate) VALUES ('" + ComputerName + "','" + username + "','" + VoucherType + "','" + action + "','" + rowid + "','" + desc + "','" + Date.ToDB(DateTime.Now).ToString() + "')";
                    //Global.m_db.InsertUpdateQry(SQL);
                }
                else if (isnew == false)
                {
                    string username = User.CurrentUserName;
                    string voucherdate = Date.ToDB(DateTime.Now).ToString();
                    string VoucherType = "PGROUP";
                    string action = "UPDATE";
                    int rowid = 0;
                    string ComputerName = Global.ComputerName;
                    string MacAddress = Global.MacAddess;
                    string IpAddress = Global.IpAddress;
                    string desc = oldpgroupdesc + newgroupdesc;

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
                    object objReturn = Global.m_db.AddOutputParameter("@Result", SqlDbType.NVarChar, 20);
                    Global.m_db.ProcessParameter();
                    //string username = User.CurrentUserName;
                    //string voucherdate = Convert.ToString(DateTime.Now);
                    //string VoucherType = "PGROUP";
                    //string action = "UPDATE";
                    //int rowid = 0;
                    //string ComputerName = Global.ComputerName;
                    //string MacAddress = Global.MacAddess;
                    //string IpAddress = Global.IpAddress;
                    //string desc = oldpgroupdesc+newgroupdesc;
                    //string SQL = "INSERT INTO System.tblAuditLog(computername,username,voucher_type,action,rowid,description,voucherdate) VALUES ('" + ComputerName + "','" + username + "','" + VoucherType + "','" + action + "','" + rowid + "','" + desc + "','" + Date.ToDB(DateTime.Now).ToString() + "')";
                    //Global.m_db.InsertUpdateQry(SQL);
                }
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }
        }

        public void Modify(int GroupID, string NewGroupName, string ParentGroupName, string Remarks,string oldpgroupdesc,string newgroupdesc,bool isnew,long backcolor)
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

            long checkbackcolor = backcolor;
            try
            {
                string ModifiedBy = User.CurrentUserName;
                Global.m_db.ClearParameter();
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("Inv.spProductGroupModify");
                Global.m_db.AddParameter("@OldID", SqlDbType.Int, GroupID);
                Global.m_db.AddParameter("@NewGroupName", SqlDbType.NVarChar, 50, NewGroupName);
                Global.m_db.AddParameter("@ParentGroup", SqlDbType.NVarChar, 50, ParentGroupName);
                Global.m_db.AddParameter("@Lang", SqlDbType.NVarChar, 20, "ENGLISH");
                Global.m_db.AddParameter("@User", SqlDbType.NVarChar, 20, ModifiedBy);
                Global.m_db.AddParameter("@Remarks", SqlDbType.NVarChar, 200, Remarks);
                Global.m_db.AddParameter("@BackColor", SqlDbType.BigInt, backcolor);

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
                    string VoucherType = "PGROUP";
                    string action = "INSERT";
                    int rowid = 0;
                    string ComputerName = Global.ComputerName;
                    string MacAddress = Global.MacAddess;
                    string IpAddress = Global.IpAddress;
                    string desc = oldpgroupdesc + newgroupdesc;

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
                    Global.m_db.AddParameter("@VoucherDate", SqlDbType.NVarChar, 50, Remarks);
                    object objReturn = Global.m_db.AddOutputParameter("@Result", SqlDbType.NVarChar, 20);
                    Global.m_db.ProcessParameter();
                    //string username = User.CurrentUserName;
                    //string voucherdate = Convert.ToString(DateTime.Now);
                    //string VoucherType = "PGROUP";
                    //string action = "INSERT";
                    //int rowid = 0;
                    //string ComputerName = Global.ComputerName;
                    //string MacAddress = Global.MacAddess;
                    //string IpAddress = Global.IpAddress;
                    //string desc = oldpgroupdesc+newgroupdesc;
                    //string SQL = "INSERT INTO System.tblAuditLog(computername,username,voucher_type,action,rowid,description,voucherdate) VALUES ('" + ComputerName + "','" + username + "','" + VoucherType + "','" + action + "','" + rowid + "','" + desc + "','" + Date.ToDB(DateTime.Now).ToString() + "')";
                    //Global.m_db.InsertUpdateQry(SQL);
                }
                else if (isnew == false)
                {
                    string username = User.CurrentUserName;
                    string voucherdate = Date.ToDB(DateTime.Now).ToString();
                    string VoucherType = "PGROUP";
                    string action = "UPDATE";
                    int rowid = 0;
                    string ComputerName = Global.ComputerName;
                    string MacAddress = Global.MacAddess;
                    string IpAddress = Global.IpAddress;
                    string desc = oldpgroupdesc + newgroupdesc;

                    Global.m_db.ClearParameter();
                    Global.m_db.setCommandType(CommandType.StoredProcedure);
                    Global.m_db.setCommandText("system.spAddAuditLog");
                    Global.m_db.AddParameter("@ComputerName", SqlDbType.NVarChar, 50, ComputerName);
                    Global.m_db.AddParameter("@UserName", SqlDbType.NVarChar, 50, username);//Set same for both for time being
                    Global.m_db.AddParameter("@Action", SqlDbType.NVarChar, 50, action);
                    Global.m_db.AddParameter("@Description", SqlDbType.NVarChar, 50, desc);
                    Global.m_db.AddParameter("@RowID", SqlDbType.Int, rowid);
                    Global.m_db.AddParameter("@MAC_Address", SqlDbType.NVarChar, 50, MacAddress);
                    Global.m_db.AddParameter("@IP_Address", SqlDbType.NVarChar, 50, IpAddress);
                    Global.m_db.AddParameter("@Voucher_Type", SqlDbType.NVarChar, 50, VoucherType);
                    Global.m_db.AddParameter("@VoucherDate", SqlDbType.NVarChar, 50, voucherdate);
                    object objReturn = Global.m_db.AddOutputParameter("@Result", SqlDbType.NVarChar, 20);
                    Global.m_db.ProcessParameter();
                    //string username = User.CurrentUserName;
                    //string voucherdate = Convert.ToString(DateTime.Now);
                    //string VoucherType = "PGROUP";
                    //string action = "UPDATE";
                    //int rowid = 0;
                    //string ComputerName = Global.ComputerName;
                    //string MacAddress = Global.MacAddess;
                    //string IpAddress = Global.IpAddress;
                    //string desc = oldpgroupdesc+newgroupdesc;
                    //string SQL = "INSERT INTO System.tblAuditLog(computername,username,voucher_type,action,rowid,description,voucherdate) VALUES ('" + ComputerName + "','" + username + "','" + VoucherType + "','" + action + "','" + rowid + "','" + desc + "','" + Date.ToDB(DateTime.Now).ToString() + "')";
                    //Global.m_db.InsertUpdateQry(SQL);
                }
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }

        }

        public void Delete(string GroupName)
        {
            //Language Management
            string LangField = "EngName";
            if (LangMgr.DefaultLanguage == Lang.English)
                LangField = "EngName";
            else if (LangMgr.DefaultLanguage == Lang.Nepali)
                LangField = "NepName";

            Global.m_db.SelectQry("DELETE FROM Inv.tblProductGroup WHERE " + LangField + "='" + GroupName + "'", "Inv.tblProductGroup");

        }

        public void DeleteProduct(int ProductID)
        {
            Global.m_db.BeginTransaction();
            try
            {
                Global.m_db.InsertUpdateQry("DELETE FROM Inv.tblOpeningQuantity WHERE ProductID ='" + ProductID + "'");
            }
            catch
            {
                Global.m_db.RollBackTransaction();
                throw new Exception("Unable to delete the product");
            }

            try
            {
                Global.m_db.InsertUpdateQry("DELETE FROM Inv.tblProduct WHERE ProductID ='" + ProductID + "'");
            }
            catch
            {
                Global.m_db.RollBackTransaction();
                throw new Exception("Unable to delete the product");
            }
            Global.m_db.CommitTransaction();

        }

        //FOR PRODUCT


        /// <summary>
        /// Create product
        /// </summary>
        /// <param name="LedgerName"></param>
        /// <param name="GroupID"></param>
        /// <param name="Remarks"></param>
        /// <returns></returns>

        public bool CreateProduct(string ProductName, int GroupID, string ProductCode, string ProductColor, int? DepotID, string Remarks, int? UnitMaintenaceID, double? SalesRate, int? Quantity, double? PurchaseRate, double? PurchaseDiscount, double? TotalValue, byte[] Image, string oldproductdesc, string newproductdesc, bool isnew, long ProductTextColor, int isVatApplicable, int IsInventoryApplicable, int isDecimalApplicable)
        {
            try
            {
                    
                //string str1 = (string)Global.MakeNull(Person_Name);
                Global.m_db.ClearParameter();
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("Inv.spProductCreate");
                Global.m_db.AddParameter("@Name", SqlDbType.NVarChar, 50, ProductName);
                Global.m_db.AddParameter("@GroupID", SqlDbType.Int, GroupID);
                Global.m_db.AddParameter("@ProductCode", SqlDbType.NVarChar,50, ProductCode);
                Global.m_db.AddParameter("@ProductColor", SqlDbType.NVarChar,100, ProductColor);
                Global.m_db.AddParameter("@ProductTextColor", SqlDbType.NVarChar, 100, ProductTextColor);
                if (DepotID == null)
                {
                    Global.m_db.AddParameter("@DepotID", SqlDbType.Int, 0);
                }
                else
                {
                    Global.m_db.AddParameter("@DepotID", SqlDbType.Int, DepotID);
                }
                Global.m_db.AddParameter("@Remarks", SqlDbType.NVarChar, 200, Remarks);
                Global.m_db.AddParameter("@UnitMaintenanceID", SqlDbType.Int, UnitMaintenaceID);
                if (SalesRate==null)
                {
                    Global.m_db.AddParameter("@SalesRate", SqlDbType.Money, 0);
                }
                else
                {
                Global.m_db.AddParameter("@SalesRate", SqlDbType.Money, SalesRate);
                }
                if(Quantity==null)
                {
                    Global.m_db.AddParameter("@Quantity", SqlDbType.Float, 0.00);
                }
                else
                {
                    Global.m_db.AddParameter("@Quantity", SqlDbType.Float, Quantity);
                }
                if (PurchaseRate==null)
                {
                    Global.m_db.AddParameter("@PurchaseRate", SqlDbType.Money, 0);
                }
                else
                {
                    Global.m_db.AddParameter("@PurchaseRate", SqlDbType.Money, PurchaseRate);
                }
                if (PurchaseDiscount==null)
                {
                    Global.m_db.AddParameter("@PurchaseDiscount", SqlDbType.Money, 0);
                }
                else
                {
                    Global.m_db.AddParameter("@PurchaseDiscount", SqlDbType.Money, PurchaseDiscount);
                }
                if (TotalValue==null)
                {
                    Global.m_db.AddParameter("@TotalValue", SqlDbType.Money, 0);
                }
                else
                {
                    Global.m_db.AddParameter("@TotalValue", SqlDbType.Money, TotalValue);
                }
                Global.m_db.AddParameter("@IsVatApplicable", SqlDbType.Int, isVatApplicable);
                Global.m_db.AddParameter("@IsInventoryApplicable", SqlDbType.Int, IsInventoryApplicable);
                Global.m_db.AddParameter("@IsDecimalApplicable", SqlDbType.Int, isDecimalApplicable);
                Global.m_db.AddParameter("@Image", SqlDbType.Binary, Image);
                Global.m_db.AddParameter("@Created_By", SqlDbType.NVarChar, 50, User.CurrUserID.ToString());
                SqlParameter objReturn = (SqlParameter)Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                SqlParameter objReturnID = Global.m_db.AddOutputParameter("@ReturnID", SqlDbType.Int);
                Global.m_db.ProcessParameter();
                try
                {
                    if (isnew == true)
                    {
                        string username = User.CurrentUserName;
                        string voucherdate = Date.ToDB(DateTime.Now).ToString();
                        string VoucherType = "PRODUCT";
                        string action = "INSERT";
                        int rowid = 0;
                        string ComputerName = Global.ComputerName;
                        string MacAddress = Global.MacAddess;
                        string IpAddress = Global.IpAddress;
                        string desc = oldproductdesc + newproductdesc;

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
                        string VoucherType = "PRODUCT";
                        string action = "UPDATE";
                        int rowid = 0;
                        string ComputerName = Global.ComputerName;
                        string MacAddress = Global.MacAddess;
                        string IpAddress = Global.IpAddress;
                        string desc = oldproductdesc + newproductdesc;

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
                //        string voucherdate = Date.ToDB(DateTime.Now).ToString();
                //        string VoucherType = "PRODUCT";
                //        string action = "INSERT";
                //        int rowid = 0;
                //        string ComputerName = Global.ComputerName;
                //        string MacAddress = Global.MacAddess;
                //        string IpAddress = Global.IpAddress;
                //        string desc = oldproductdesc + newproductdesc;
                //        string SQL = "INSERT INTO System.tblAuditLog(computername,username,voucher_type,action,rowid,description,voucherdate) VALUES ('" + ComputerName + "','" + username + "','" + VoucherType + "','" + action + "','" + rowid + "','" + desc + "','" + Date.ToDB(DateTime.Now).ToString() + "')";
                //        Global.m_db.InsertUpdateQry(SQL);
                //    }
                //    else if (isnew == false)
                //    {
                //        string username = User.CurrentUserName;
                //        string voucherdate = Date.ToDB(DateTime.Now).ToString();
                //        string VoucherType = "PRODUCT";
                //        string action = "UPDATE";
                //        int rowid = 0;
                //        string ComputerName = Global.ComputerName;
                //        string MacAddress = Global.MacAddess;
                //        string IpAddress = Global.IpAddress;
                //        string desc = oldproductdesc + newproductdesc;
                //        string SQL = "INSERT INTO System.tblAuditLog(computername,username,voucher_type,action,rowid,description,voucherdate) VALUES ('" + ComputerName + "','" + username + "','" + VoucherType + "','" + action + "','" + rowid + "','" + desc + "','" + Date.ToDB(DateTime.Now).ToString() + "')";
                //        Global.m_db.InsertUpdateQry(SQL);
                //    }
                //}
                //catch (Exception ex)
                //{
                //    Global.MsgError(ex.Message);
                //}
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
                        throw new Exception("The Product name already exists! Please choose another Product names!");
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

           


        }

        public bool ModifyProduct(int ProductID, string ProductName, int GroupID, string ProductCode, string ProductColor, int? DepotID, string Remarks, int? UnitMaintenanceID, double? SalesRate, int? Quantity, double? PurchaseRate, double? PurchaseDiscount, double? TotalValue, byte[] Image, string oldproductdesc, string newproductdesc, bool isnew, long ProductTextColor, int isVatApplicable, int IsInventoryApplicable, int isDecimalApplicable)
        {
            try
            {
                string LangField = "ENGLISH";
                //string str1 = (string)Global.MakeNull(Person_Name);
                Global.m_db.ClearParameter();
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("Inv.spProductModify");
                Global.m_db.AddParameter("@ProductID", SqlDbType.Int, ProductID);
                Global.m_db.AddParameter("@Name", SqlDbType.NVarChar, 50, ProductName);
                if (LangMgr.DefaultLanguage == Lang.English)
                    LangField = "ENGLISH";
                else if (LangMgr.DefaultLanguage == Lang.Nepali)
                    LangField = "NEPALI";
                Global.m_db.AddParameter("@Lang", SqlDbType.NVarChar, 50, LangField);
                Global.m_db.AddParameter("@GroupID", SqlDbType.Int, GroupID);
                Global.m_db.AddParameter("@ProductCode", SqlDbType.NVarChar, 50, ProductCode);
                Global.m_db.AddParameter("@ProductColor", SqlDbType.NVarChar, 50, ProductColor);
                Global.m_db.AddParameter("@ProductTextColor", SqlDbType.NVarChar, 50, ProductTextColor);
                if(DepotID==null)
                {
                    Global.m_db.AddParameter("@DepotID", SqlDbType.Int, 0);
                }
                else
                {
                    Global.m_db.AddParameter("@DepotID", SqlDbType.Int, DepotID);
                }
                
                Global.m_db.AddParameter("@Remarks", SqlDbType.NVarChar, 50, Remarks);//@UnitMaintenanceID
                if(UnitMaintenanceID==null)
                {
                    Global.m_db.AddParameter("@UnitMaintenanceID", SqlDbType.NVarChar, 50, 0);
                }
                else
                {
                    Global.m_db.AddParameter("@UnitMaintenanceID", SqlDbType.NVarChar, 50, UnitMaintenanceID);
                }
                
                if(SalesRate==null)
                {
                    Global.m_db.AddParameter("@SalesRate", SqlDbType.Money, 0);
                }
                else
                {
                    Global.m_db.AddParameter("@SalesRate", SqlDbType.Money, SalesRate);
                }
                
                if(Quantity==null)
                {
                    Global.m_db.AddParameter("@Quantity", SqlDbType.Float, 0);
                }
                else
                {
                    Global.m_db.AddParameter("@Quantity", SqlDbType.Float, Quantity);
                }
                
                if(PurchaseRate==null)
                {
                    Global.m_db.AddParameter("@PurchaseRate", SqlDbType.Money, 0);
                }
                else
                {
                    Global.m_db.AddParameter("@PurchaseRate", SqlDbType.Money, PurchaseRate);
                }
                
                if(PurchaseDiscount==null)
                {
                     Global.m_db.AddParameter("@PurchaseDiscount", SqlDbType.Money, 0);
                }
                else
                {
                     Global.m_db.AddParameter("@PurchaseDiscount", SqlDbType.Money, PurchaseDiscount);
                }

                if (TotalValue==null)
                {
                    Global.m_db.AddParameter("@TotalValue", SqlDbType.Money, 0);
                }
                else
                {
                    Global.m_db.AddParameter("@TotalValue", SqlDbType.Money, TotalValue);
                }
                Global.m_db.AddParameter("@IsVatApplicable", SqlDbType.Int, isVatApplicable);
                Global.m_db.AddParameter("@IsInventoryApplicable", SqlDbType.Int, IsInventoryApplicable);
                Global.m_db.AddParameter("@IsDecimalApplicable", SqlDbType.Int, isDecimalApplicable);
                Global.m_db.AddParameter("@Image", SqlDbType.Binary, Image);
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
                        string VoucherType = "PRODUCT";
                        string action = "INSERT";
                        int rowid = 0;
                        string ComputerName = Global.ComputerName;
                        string MacAddress = Global.MacAddess;
                        string IpAddress = Global.IpAddress;
                        string desc = oldproductdesc + newproductdesc;

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
                        string VoucherType = "PRODUCT";
                        string action = "UPDATE";
                        int rowid = 0;
                        string ComputerName = Global.ComputerName;
                        string MacAddress = Global.MacAddess;
                        string IpAddress = Global.IpAddress;
                        string desc = oldproductdesc + newproductdesc;

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
                //        string VoucherType = "PRODUCT";
                //        string action = "INSERT";
                //        int rowid = 0;
                //        string ComputerName = Global.ComputerName;
                //        string MacAddress = Global.MacAddess;
                //        string IpAddress = Global.IpAddress;
                //        string desc = oldproductdesc + newproductdesc;
                //        string SQL = "INSERT INTO System.tblAuditLog(computername,username,voucher_type,action,rowid,description,voucherdate) VALUES ('" + ComputerName + "','" + username + "','" + VoucherType + "','" + action + "','" + rowid + "','" + desc + "','" + Date.ToDB(DateTime.Now).ToString() + "')";
                //        Global.m_db.InsertUpdateQry(SQL);
                //    }
                //    else if (isnew == false)
                //    {
                //        string username = User.CurrentUserName;
                //        string voucherdate = Convert.ToString(DateTime.Now);
                //        string VoucherType = "PRODUCT";
                //        string action = "UPDATE";
                //        int rowid = 0;
                //        string ComputerName = Global.ComputerName;
                //        string MacAddress = Global.MacAddess;
                //        string IpAddress = Global.IpAddress;
                //        string desc = oldproductdesc + newproductdesc;
                //        string SQL = "INSERT INTO System.tblAuditLog(computername,username,voucher_type,action,rowid,description,voucherdate) VALUES ('" + ComputerName + "','" + username + "','" + VoucherType + "','" + action + "','" + rowid + "','" + desc + "','" + Date.ToDB(DateTime.Now).ToString() + "')";
                //        Global.m_db.InsertUpdateQry(SQL);
                //    }
                //}
                //catch (Exception ex)
                //{
                //    Global.MsgError(ex.Message);
                //}

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
                        throw new Exception("The Product name already exists! Please choose another Product names!");
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
           

        }

        public static int GetGroupIDFromName(string Name, Lang Language)
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


            object objResult = Global.m_db.GetScalarValue("SELECT GroupID FROM Inv.tblProductGroup WHERE " + LangField + "='" + Name + "'");
            return Convert.ToInt32(objResult);
        }

        public static int GetProductIDFromName(string Name, Lang Language)
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


            object objResult = Global.m_db.GetScalarValue("SELECT ProductID FROM Inv.tblProduct WHERE " + LangField + "='" + Name + "'");
            return Convert.ToInt32(objResult);
        }


        /// <summary>
        /// Returns the Opening Quantity of a product dependent to Root Accounting Class by either ProductID or GroupID
        /// Example: GetOpeningQty(2,ProductID:141) or GetOpeningQty(2,GroupID:14)
        /// </summary>
        /// <param name="RootAccClassID"></param>
        /// <param name="ProductID"></param>
        /// <param name="GroupID"></param>
        /// <returns></returns>
        public static int GetOpeningQty( int RootAccClassID, int ProductID=-1, int GroupID=-1)
        {
            
            
            

            int OpeningQty=-1; 
            //Write Opening Quantity
            if(ProductID>0)
            {
                OpeningQty= InventoryBook.GetOpeningQty(RootAccClassID, ProductID:ProductID);

            }
            else if(GroupID>0) // If GroupID is selected
            {

                //Get all child Group ID from the given group ID
                DataTable dtChildProductGroupIDs = GetChildProductGroupIDs(GroupID);
                List<int> arrChildProductGroupIDs = new List<int>();
                foreach (DataRow dr in dtChildProductGroupIDs.Rows)
                {
                    arrChildProductGroupIDs.Add(Convert.ToInt32(dr[0]));
                }

                OpeningQty= InventoryBook.GetOpeningQty(RootAccClassID, GroupIDList:arrChildProductGroupIDs);

            }
            else //Both not given means all product is selected
            {
                OpeningQty = InventoryBook.GetOpeningQty(RootAccClassID);
            }

            //Write on the label
            return OpeningQty;
        }


        /// <summary>
        /// Returns the DataTable of Child Product GroupIDs from the given GroupID
        /// </summary>
        /// <param name="GroupID"></param>
        /// <returns></returns>
        public static DataTable GetChildProductGroupIDs(int GroupID)
        {
            //Check for valid groupID
            if (!(GroupID > 0))
            {
                throw new Exception("Invalid GroupID. Please provide a proper groupID");
            }

            //Try getting Child Product GroupID from the stored procedure
            try
            {
                
                Global.m_db.ClearParameter();
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("Inv.spGetChildProductGroupIDs");
                Global.m_db.AddParameter("@ParentID", SqlDbType.Int, GroupID);
                
                return Global.m_db.GetDataTable();

            }
            catch (Exception ex)
            {
                throw new Exception ("Error occurred while trying to get child product group ID. Message: "  + ex.Message);

                
            }
        }

        /// <summary>
        /// Gets all the Group informations under the given parent ID. If parent ID is 0 it returns only parent group information.
        /// If you want to get all groups, introduce ParentGroupID as -1
        /// </summary>
        /// <param name="ParentGroupID"></param>
        /// <returns></returns>
        public static DataTable GetGroupTable(int ParentGroupID)
        {
            if (ParentGroupID == 0)//Only Parent group
            {
                return Global.m_db.SelectQry("SELECT * FROM Inv.tblProductGroup WHERE Parent_GrpID is null", "tblGroup");
            }
            else if (ParentGroupID == -1)//All Groups
            {
                return Global.m_db.SelectQry("SELECT * FROM Inv.tblProductGroup", "tblGroup");
            }
            else
            {
                return Global.m_db.SelectQry("SELECT * FROM Inv.tblProductGroup WHERE Parent_GrpID =" + ParentGroupID.ToString(), "tblGroup");
            }
        }

        /// <summary>
        /// Returns the table of Product which falls under given GroupID and fills the result in ReturnTable. Recursive function
        /// </summary>
        /// <param name="GroupID"></param>
        /// <param name="ReturnTable"></param>
        public static void GetProductsUnder(int GroupID, ArrayList ReturnIDs)
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


            dt = Product.GetGroupTable(GroupID);


            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];

                ReturnIDs.Add(dr["GroupID"]);


                GetProductsUnder(Convert.ToInt32(dr["GroupID"].ToString()), ReturnIDs);



            }


        }

        public static DataTable GetGroupByID(int GroupID, Lang Language)
        {
            string LangField = "EngName";
            if (Language == Lang.English)
                LangField = "EngName";
            else if (Language == Lang.Nepali)
                LangField = "NepName";

            return Global.m_db.SelectQry("SELECT a.GroupID ID, a." + LangField + " Name, b." + LangField + " Parent, a.Remarks Remarks,a.BackColor BackColor FROM Inv.tblProductGroup a LEFT OUTER JOIN Inv.tblProductGroup b ON a.Parent_GrpID=b.GroupID WHERE a.GroupID='" + GroupID.ToString() + "'", "Search");

        }

        /// <summary>
        /// Gets the information of Product under the given group_id
        /// </summary>
        /// <param name="Group_ID"></param>
        /// <returns></returns>
        public static DataTable GetProductTable(int Group_ID)
        {
            return Global.m_db.SelectQry("SELECT * FROM Inv.tblProduct WHERE GroupID ='" + Group_ID.ToString() + "'", "tblGroup");
        }

        public static DataTable GetProductInfo(int ProductID, Lang Language)
        {
            #region Language Management

            string LangField = "EngName";
            if (Language == Lang.English)
                LangField = "EngName";
            else if (Language == Lang.Nepali)
                LangField = "NepName";

            #endregion
            string strQuery = "";
            if (ProductID == -1)
            {
                strQuery = "SELECT Prod.ProductID ID, Prod." + LangField + " ProdName, grp." + LangField + " GroupName,Prod.BackColor BackColor, Prod.GroupID,Prod.ProductCode Code,Prod.ProductColor Color,Prod.DepotID DepotID,Prod.Remarks Remarks,Prod.UnitMaintenanceID UnitMaintenanceID,Prod.SalesRate SalesRate,Prod.Quantity Quantity,Prod.PurchaseRate PurchaseRate,Prod.PurchaseDiscount PurchaseDiscount,Prod.TotalValue,prod.Image,Prod.IsVatApplicable,Prod.IsInventoryApplicable,isnull(Prod.IsDecimalApplicable, 0) IsDecimalApplicable FROM Inv.tblProduct Prod LEFT OUTER JOIN Inv.tblProductGroup grp ON Prod.GroupID=grp.GroupID ";
                return Global.m_db.SelectQry(strQuery, "Search");
            }
           
            else
            {
                strQuery = "SELECT Prod.ProductID ID, Prod." + LangField + " ProdName, grp." + LangField + " GroupName,Prod.BackColor BackColor,Prod.GroupID,Prod.ProductCode Code,Prod.ProductColor Color,Prod.DepotID DepotID,Prod.Remarks Remarks,Prod.UnitMaintenanceID UnitMaintenanceID,Prod.SalesRate SalesRate,Prod.Quantity Quantity,Prod.PurchaseRate PurchaseRate,Prod.PurchaseDiscount PurchaseDiscount,Prod.TotalValue,Prod.Image,Prod.IsVatApplicable,Prod.IsInventoryApplicable,isnull(Prod.IsDecimalApplicable, 0) IsDecimalApplicable FROM Inv.tblProduct Prod LEFT OUTER JOIN Inv.tblProductGroup grp ON Prod.GroupID=grp.GroupID WHERE Prod.ProductID='" + ProductID + "'";
                return Global.m_db.SelectQry(strQuery, "Search");
            }
        }

        public static DataTable GetProductInfoFromDepotID(int DepotID, Lang Language)
        {

            #region Language Management

            string LangField = "EngName";
            if (Language == Lang.English)
                LangField = "EngName";
            else if (Language == Lang.Nepali)
                LangField = "NepName";

            #endregion
            return Global.m_db.SelectQry("SELECT Prod.ProductID ID, Prod." + LangField + " ProdName, grp." + LangField + " GroupName,Prod.GroupID,Prod.ProductCode Code,Prod.ProductColor Color,Prod.DepotID DepotID,Prod.Remarks Remarks,Prod.Base_Unit BaseUnit,Prod.SalesRate SalesRate,Prod.Quantity Quantity,Prod.PurchaseRate PurchaseRate,Prod.PurchaseDiscount PurchaseDiscount,Prod.TotalValue FROM Inv.tblProduct Prod LEFT OUTER JOIN Inv.tblProductGroup grp ON Prod.GroupID=grp.GroupID WHERE Prod.DepotID='" + DepotID + "'", "Search");
              
        
        }

        /// <summary>
        /// Returns the string array list of ledgers with the provided group ID, if 0 is provided as group id, it returns all Products
        /// </summary>
        /// <param name="GroupID"></param>
        /// <returns></returns>
        public static DataTable  GetProductList(int GroupID)
        {
            string strQuery = "";
            DataTable dt;
            if (GroupID == 0)//If 0 is the group ID, select all Product
            {
                strQuery = "SELECT GroupID,ProductID,ProductCode,EngName,NepName FROM Inv.tblProduct order by EngName";
                dt = Global.m_db.SelectQry(strQuery, "Product");
            }
            else
            {
                strQuery = "SELECT GroupID,ProductID,ProductCode,EngName,NepName FROM Inv.tblProduct WHERE GroupID='" + GroupID + "'";
                dt = Global.m_db.SelectQry(strQuery, "Product");
            }
            return dt;
        }

        public static DataTable GetProductList1(int GroupID)
        {
            string strQuery = "";
            DataTable dt;
            if (GroupID == 0)//If 0 is the group ID, select all Product
            {
                strQuery = "SELECT GroupID,ProductID,ProductCode,EngName,NepName FROM Inv.tblProduct where IsInventoryApplicable=1 order by EngName";
                dt = Global.m_db.SelectQry(strQuery, "Product");
            }
            else
            {
                strQuery = "SELECT GroupID,ProductID,ProductCode,EngName,NepName FROM Inv.tblProduct WHERE GroupID='" + GroupID + "' and IsInventoryApplicable=1";
                dt = Global.m_db.SelectQry(strQuery, "Product");
            }
            return dt;
        }

        public  int GetProductIdFromName(string Name, Lang Language)
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


            object objResult = Global.m_db.GetScalarValue("SELECT ProductID FROM Inv.tblProduct WHERE " + LangField + "='" + Name + "'");
            return Convert.ToInt32(objResult);
        }

        public static DataTable GetProductByName(string ProductName)
        {
            return Global.m_db.SelectQry("SELECT * FROM Inv.tblProduct WHERE EngName = '" + ProductName + "'", "tblProduct");
        }

        public static DataTable GetProductByCode(string ProductCode)
        {
            string strQuery = "";
            strQuery = "SELECT * FROM Inv.tblProduct WHERE ProductCode = '" + ProductCode + "'";
            return Global.m_db.SelectQry(strQuery, "tblProduct");
        }


        /// <summary>
        /// Returns list of all products where accClassID is optional
        /// </summary>
        /// <param name="accClassID"></param>
        /// <returns></returns>
        public static DataTable GetAllProduct(int accClassID = 0)
        {
            string strquery = "";
            //double closingQuantity = 0;
            //string AccClassIDsXMLString = "";
            //string ProjectIDsXMLString = "";
            //if (drcheckvinventoryapplicable["IsInventoryApplicable"].ToString() == "1")
            //               {
            //DataTable dtOpeningStockStatusInfo = StockStatusBook.GetOpeningStockStatusBook(null, Convert.ToInt32(0), " ", DateTime.Today.AddDays(1), true, StockStatusType.OpeningStock, AccClassIDsXMLString);
            //DataTable dtTransactionStockStatusInfo = StockStatusBook.GetStockStatusBook1(null, Convert.ToInt32(0), "", DateTime.Today.AddDays(1), true, StockStatusType.ClosingStock, AccClassIDsXMLString, ProjectIDsXMLString);
            //// DataTable dtTransactionStockStatusInfo = StockStatusBook.GetStockStatusBook1(m_StockStatus.ProductGroupID, m_StockStatus.ProductID, m_StockStatus.Depot, m_StockStatus.AtTheEndDate, m_StockStatus.ShowZeroQunatity, StockStatusType.ClosingStock, AccClassIDsXMLString, ProjectIDsXMLString);//use InventoyBookType becuase its index is zero soo it looks for all VoucherType and its difference than InventoryBook becuase it is filtered by Product
            //if (dtTransactionStockStatusInfo.Rows.Count != 0)
            //{
            //    foreach (DataRow drOpeningStockStatusInfo in dtOpeningStockStatusInfo.Rows)
            //    {
            //        // if (dtTransactionStockStatusInfo.Rows.Count != 0)
            //        //{
            //        foreach (DataRow drTransactionStockStatusInfo in dtTransactionStockStatusInfo.Rows)
            //        {
            //            if (Convert.ToInt32(drTransactionStockStatusInfo["ProductID"]) == Convert.ToInt32(drOpeningStockStatusInfo["ProductID"]))
            //            {
            //                //isTrans = true;
            //                closingQuantity = Convert.ToDouble(drTransactionStockStatusInfo["Quantity"]) + Convert.ToInt32(drOpeningStockStatusInfo["Quantity"]);
            //            }
            //        }
            //    }
            //}
                if (accClassID > 0) //If accClassID is given
                    strquery = "select p.productid,p.ProductCode Code,p.EngName Name,oq.OpenPurchaseRate PurchaseRate,oq.OpenSalesRate salesrate,pg.EngName GroupType from Inv.tblProduct p Left Join Inv.tblProductGroup pg on p.GroupID=pg.GroupID Left Join inv.tblOpeningQuantity oq on p.ProductID=oq.ProductID where oq.AccClassID='" + accClassID + "'";
                else
                    strquery = "select p.productid,p.ProductCode Code,p.EngName Name,p.PurchaseRate PurchaseRate,p.salesrate,pg.EngName GroupType from Inv.tblProduct p,Inv.tblProductGroup pg where p.GroupID=pg.GroupID";
                return Global.m_db.SelectQry(strquery, "tblProduct");
        }

        public static DataTable GetProductData(int accClassID = 0, bool isInventory = false)
        {
            string strquery = "";
            string strinvCheck = " and p.GroupID in(1096,1097) ";

            if (isInventory)
            {
                strinvCheck = " and p.IsInventoryApplicable = 1";
            }
            if (accClassID > 0) //If accClassID is given
                strquery = "select p.productid ProductID,p.ProductCode Code,p.EngName ProductName,oq.OpenPurchaseRate AveragePurchaseRate,oq.OpenSalesRate SalesRate,pg.EngName GroupName, 0 as 'ClosingQty' from Inv.tblProduct p Left Join Inv.tblProductGroup pg on p.GroupID=pg.GroupID Left Join inv.tblOpeningQuantity oq on p.ProductID=oq.ProductID where oq.AccClassID='" + accClassID + "'" + strinvCheck;

            else
                strquery = "select p.productid ProductID,p.ProductCode Code,p.EngName ProductName,p.PurchaseRate AveragePurchaseRate,p.salesrate SalesRate,pg.EngName GroupName, 0 as 'ClosingQty' from Inv.tblProduct p join Inv.tblProductGroup pg on p.GroupID=pg.GroupID" + (!isInventory ? " where pg.GroupID in(1096,1097)" : " where p.IsInventoryApplicable = 1");
            return Global.m_db.SelectQry(strquery, "tblProduct");
        }
        public static DataTable GetAllProduct(int? ProducGroupID, int? ProductID, String Depot, DateTime? AtTheEndDate, bool ShowZeroQuantity, StockStatusType DiffInStock, bool isInventory = true)
        {
            try
            {
                string AccClassIDsXMLString = "<?xml version=\"1.0\" encoding=\"utf-16\"?><STOCKSTATUS><AccClassIDSettings><AccClassID>"+Global.m_db.GetScalarValue("select AccClassID from System.tblUser where UserID = "+User.CurrUserID+"")+"</AccClassID></AccClassIDSettings></STOCKSTATUS>";
                string ProjectIDXMLString = "﻿<?xml version=\"1.0\" encoding=\"utf-16\"?><STOCKSTATUS><ProjectIDSettings><ProjectID>1</ProjectID></ProjectIDSettings></STOCKSTATUS>";
                int t = (int)DiffInStock;
                string EndDate = AtTheEndDate.ToString();
                DateTime? EndDateTime = new DateTime();
                if (AtTheEndDate != null)
                {
                    EndDateTime = Convert.ToDateTime(EndDate).AddDays(1);
                }
                else
                {
                    EndDateTime = null;
                }
                Global.m_db.ClearParameter();
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("Inv.spGetStockStatusBook");
                Global.m_db.AddParameter("@ProductGroupID", SqlDbType.Int, ProducGroupID);
                Global.m_db.AddParameter("@ProductID", SqlDbType.Int, ProductID);
                Global.m_db.AddParameter("@Depot", SqlDbType.NVarChar, Depot);
                Global.m_db.AddParameter("@AtTheEndDate", SqlDbType.DateTime, EndDateTime);
                Global.m_db.AddParameter("@ShowZeroQuantity", SqlDbType.Bit, ShowZeroQuantity);
                Global.m_db.AddParameter("@StockStatusTypeIndex", SqlDbType.Int, (int)DiffInStock);
                Global.m_db.AddParameter("@AccountClassIDsSettings", SqlDbType.Xml, AccClassIDsXMLString);
                Global.m_db.AddParameter("@ProjectIDsSettings", SqlDbType.Xml, ProjectIDXMLString);
                Global.m_db.AddParameter("@IsInventryApplicable", SqlDbType.Bit, isInventory);

                DataTable dtProduct = Global.m_db.GetDataTable();
                return dtProduct;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw ex;
            }
        }
        public static DataTable filterproduct(string a, string b, string c)
        {
            string strquery = "";
                                                                                                                                            // "and p.ProductCode LIKE '%" + txtproductCode.Text + "%' AND p.EngName LIKE '%" + txtProductName.Text + "%";
            // strquery = "select p.productid,p.ProductCode Code,p.EngName Name,p.PurchaseRate PurchaseRate,pg.EngName GroupType from Inv.tblProduct p,Inv.tblProductGroup pg where p.GroupID=pg.GroupID  AND p.EngName LIKE '%" + a + "%'";
           // strquery = "select p.productid,p.ProductCode Code,p.EngName Name,p.PurchaseRate PurchaseRate,p.salesrate,pg.EngName GroupType from Inv.tblProduct p,Inv.tblProductGroup pg where p.GroupID=pg.GroupID  AND  p.ProductCode LIKE '%" + b + "%' AND p.EngName LIKE '%" + a + "%' AND pg.EngName LIKE '" + c + "%'";
            strquery = "  select p.productid,p.ProductCode Code,p.EngName Name,pg.EngName GroupType,oq.OpenPurchaseRate PurchaseRate,OpenSalesRate SalesRate"+" "+
                        "from Inv.tblProduct p,Inv.tblProductGroup pg,inv.tblopeningquantity oq where p.GroupID=pg.GroupID  and p.productid=oq.productid and oq.AccClassID='1'"+" "+
                        "AND  p.ProductCode LIKE '%" + b + "%' AND p.EngName LIKE '%" + a + "%' AND pg.EngName LIKE '" + c + "%'";
            return Global.m_db.SelectQry(strquery, "tblProduct");
        }


        public static DataTable FindProductTransactPresent(int ProductID)
        {
            return Global.m_db.SelectQry("SELECT top 1 * FROM inv.tblInventoryTrans WHERE ProductID = '" + ProductID + "'", "tbl");
        }

        public static DataTable FindProductGroupTransactPresent(int GroupID)
        {
            return Global.m_db.SelectQry("SELECT top 1 * FROM inv.tblInventoryTrans WHERE ProductID  in (select productid from inv.tblProduct where GroupID = '" + GroupID + "')", "tbl");
        }

        public static DataTable GetProductCategory()
        {
            string strQuery = "";
            strQuery = "select * from Inv.tblProductGroup where productgroupid=1";
            DataTable dt = Global.m_db.SelectQry(strQuery, "tblProductGroup");
            return dt;
            // return Global.m_db.SelectQry(strQuery, "tblProduct");
        }

        public static DataTable GetProductByID(int ProductID)
        {
            return Global.m_db.SelectQry("SELECT * FROM Inv.tblProduct WHERE ProductID ='" + ProductID.ToString() + "'", "tblProduct");
        }

        public DataTable getProductByGroupID()
        {
            string str = "select * from Inv.tblProduct";
            return Global.m_db.SelectQry(str, "tblProduct");
        }

        public DataTable getGroup()
        {
            string str = "select * from Inv.tblProductGroup where Parent_grpID=13";
            return Global.m_db.SelectQry(str, "tblProductGroup");
        }

        public static double GetProductPurchasePrice(int ProductID, int AccClassID)
        {
            object objResult = Global.m_db.GetScalarValue("select OpenPurchaseRate from Inv.tblOpeningQuantity where ProductID='"+ProductID+"' and AccClassID='"+AccClassID+"'");
            return Convert.ToDouble(objResult);
        }

        public static double GetFreightandCustomDuty(int PurchaseMasterID)
        {
            object objResult = Global.m_db.GetScalarValue("select CustomDuty+freight AdditionalCost from Inv.tblPurchaseInvoiceMaster where PurchaseInvoiceID='"+PurchaseMasterID+"'");
            return Convert.ToDouble(objResult);
        }

        public static DataTable getProduct()
        {
            return Global.m_db.SelectQry("select pg.EngName ProductGroup,p.EngName ProductName from Inv.tblProduct p,Inv.tblProductGroup pg where p.GroupID=pg.GroupID ", "Inv.tblProduct");

        }

        /// <summary>
        /// Check if Product code is already in use
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static bool ValidProductCode(string code)
        {
            string str = "select ProductCode from Inv.tblProduct where ProductCode = '" + code + "'";
            DataTable dt = Global.m_db.SelectQry(str, "Inv.tblProduct");
            if (dt.Rows.Count > 0)
                return false;
            else
                return true;
        }
        /// <summary>
        /// Check if Product code is already in use for edit
        /// </summary>
        /// <param name="code"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool ValidProductCode(string code, int id)
        {
            string str = "select ProductCode from Inv.tblProduct where ProductCode = '" + code + "' and ProductID <> '" + id + "'";
            DataTable dt = Global.m_db.SelectQry(str, "Inv.tblProduct");
            if (dt.Rows.Count > 0)
                return false;
            else
                return true;
        }

        public static bool IsGroupBuiltIn(int groupId)
        {
            object objResult = Global.m_db.GetScalarValue("SELECT BuiltIn FROM Inv.tblProductGroup WHERE GroupID = '" + groupId + "'");
            return Convert.ToBoolean(objResult);
        }

        public static bool IsProductBuiltIn(int productId)
        {
            object objResult = Global.m_db.GetScalarValue("SELECT BuiltIn FROM Inv.tblProduct WHERE ProductID = '" + productId + "'");
            return Convert.ToBoolean(objResult);
        }

        public static int GetTotalProductQuantity(int productID)
        {
            object objResult = Global.m_db.GetScalarValue("select sum(OpenPurchaseQty) from inv.tblOpeningQuantity where ProductID = '" + productID + "'");
            if(objResult == DBNull.Value)
            {
                objResult = 0;
            }
            return Convert.ToInt32(objResult);
        }

        //NEW for DataGrid
        public static   DataTable GetAllProductOpnNCloStock(string xmlAccountingClassIDs, string xmlProjectIDs, DateTime ? fromDate,DateTime? EndDate,int? productID,int? groupID, ref decimal openingStock,ref decimal closingStock )
        {
            DataTable dtprd;

            try
            {
                Global.m_db.ClearParameter();
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("[Inv].[spGetAllProductOpenNColStock]");

                if (productID != null)
                    Global.m_db.AddParameter("@InputProductID", SqlDbType.Int, productID);
                else
                    Global.m_db.AddParameter("@InputProductID", SqlDbType.Int, null);

                if (groupID != null)
                    Global.m_db.AddParameter("@ProductGrpID", SqlDbType.Int, groupID);
                else
                    Global.m_db.AddParameter("@ProductGrpID", SqlDbType.Int, null);


                if (fromDate != null)
                    Global.m_db.AddParameter("@FromDate", SqlDbType.DateTime, fromDate);
                else
                    Global.m_db.AddParameter("@FromDate", SqlDbType.DateTime, null);
                
                if (EndDate != null)
                    Global.m_db.AddParameter("@AtTheEndDate", SqlDbType.DateTime, EndDate);
                else
                    Global.m_db.AddParameter("@AtTheEndDate", SqlDbType.DateTime, null);
                Global.m_db.AddParameter("@AccountClassIDsSettings", SqlDbType.NVarChar, xmlAccountingClassIDs);
                Global.m_db.AddParameter("@ProjectIDsSettings", SqlDbType.NVarChar, xmlProjectIDs);
                System.Data.SqlClient.SqlParameter objReturn = Global.m_db.AddOutputParameter("@OpeningStock", SqlDbType.NVarChar, 100);
                System.Data.SqlClient.SqlParameter objReturn1 = Global.m_db.AddOutputParameter("@ClosingStock", SqlDbType.NVarChar,100);

                dtprd = Global.m_db.GetDataTable();
             // Global.m_db.ProcessParameter();
                openingStock=Convert.ToDecimal(objReturn.Value);
                closingStock=Convert.ToDecimal(objReturn1.Value);
                return dtprd;

            }
            catch (Exception ex)
            {            
                throw ex;
            }
        }

        public static bool IsDecimalApplicable(int productID)
        {
            try
            {
                object res = Global.m_db.GetScalarValue("select isnull(IsDecimalApplicable ,0) from Inv.tblProduct where ProductID = " + productID);
                return Convert.ToBoolean(res);
            
            }
            catch (Exception)
            {
                
                throw;
            }
        }


        public static string GetProductUnit(int productID)
        {
            try
            {
                if (productID > 0)
                {
                    Global.m_db.ClearParameter();
                    Global.m_db.setCommandType(CommandType.Text);
                    Global.m_db.AddParameter("@productID", SqlDbType.Int, 50, productID);

                    return Global.m_db.GetScalarValue("select u.UnitName from Inv.tblProduct p join System.tblUnitMaintenance u on p.UnitMaintenanceID = u.UnitMaintenanceID where p.ProductID = @productID ").ToString();
                    
                }

                return "";
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static DataTable GetStockLedgerReport(string xmlAccountingClassIDs, string xmlProjectIDs, DateTime? fromDate, DateTime? EndDate, int? productID,ref decimal openingStock)
        {
            DataTable dtprd;

            try
            {
                Global.m_db.ClearParameter();
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("[Inv].[spGetProductTransactionDetail]");

                if (productID != null)
                    Global.m_db.AddParameter("@ProductID", SqlDbType.Int, productID);
                else
                    Global.m_db.AddParameter("@ProductID", SqlDbType.Int, null);


                if (fromDate != null)
                    Global.m_db.AddParameter("@FromDate", SqlDbType.DateTime, fromDate);
                else
                    Global.m_db.AddParameter("@FromDate", SqlDbType.DateTime, null);

                if (EndDate != null)
                    Global.m_db.AddParameter("@ToDate", SqlDbType.DateTime, EndDate);
                else
                    Global.m_db.AddParameter("@ToDate", SqlDbType.DateTime, null);

                Global.m_db.AddParameter("@AccountClassIDsSettings", SqlDbType.NVarChar, xmlAccountingClassIDs);
                Global.m_db.AddParameter("@ProjectIDsSettings", SqlDbType.NVarChar, xmlProjectIDs);
                System.Data.SqlClient.SqlParameter objReturn = Global.m_db.AddOutputParameter("@OpeningStockQty", SqlDbType.Decimal);


                dtprd = Global.m_db.GetDataTable();
                if (objReturn.Value == DBNull.Value)
                    openingStock = 0;
                else
                openingStock = Convert.ToDecimal(objReturn.Value);

                return dtprd;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
