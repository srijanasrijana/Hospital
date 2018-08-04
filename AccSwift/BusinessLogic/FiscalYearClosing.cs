using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Data;

namespace BusinessLogic
{
    public class FiscalYearClosing
    {
        public DataTable GetAccountClasses()
        {
            string strQuery = "select * from Acc.tblAccClass where ParentID is null";
            DataTable dt = Global.m_db.SelectQry(strQuery, "tblAccountClass");
            return dt;
        }
        public DataTable GetTransactDateInfo()
        {
            string strQuery = "select * from Acc.tblTransaction";
            DataTable dt = Global.m_db.SelectQry(strQuery, "tblAccountClass");
            return dt;
        }
        public static DataTable GetCompanyDetails()
        {
            string strQuery = "select * from System.tblCompanyInfo";
            DataTable dt = Global.m_db.SelectQry(strQuery, "tblCompanyDetails");
            return dt;
        }


        #region old code to add ledgers, products and user details into new database
        /// <summary>
        /// when closing the fiscal year only the builtin ledgers are readily available in new DB, so other ledgers are added from Previous DB to the new one 
        /// </summary>
        /// <param name="prevDBName"></param>
        /// <param name="newDBName"></param>
        /// <returns></returns>
        public static int AddLedgerToNewDB(string prevDBName, string newDBName)
        {
            try
            {
                string sql = "DELETE FROM [" + newDBName + "].[Acc].[tblLedger];" +
                                "SET IDENTITY_INSERT [" + newDBName + "].[Acc].[tblLedger] ON " +
                                "INSERT INTO  " + newDBName + ".Acc.tblLedger(Address1,Address2, BuiltIn, Calculate_Rate, Calculated, City, Company,Created_By, Created_Date, CreditLimit, DrCr, Email, EngName, GroupID, IsActive, IsBillReference, LedgerCode, LedgerID, LedgerNumber, LF, Modified_By, Modified_Date,NepName,OpCCR,OpCCRDate,OpCCYID, PersonName, Phone, PreYrBal, PreYrBalDrCr, Remarks, VatPanNo, Website)" +
                                "SELECT Address1,Address2, BuiltIn, Calculate_Rate, Calculated, City, Company,Created_By, Created_Date, CreditLimit, DrCr, Email, EngName, GroupID, IsActive, IsBillReference, LedgerCode, LedgerID, LedgerNumber, LF, Modified_By, Modified_Date,NepName,OpCCR,OpCCRDate,OpCCYID, PersonName, Phone, PreYrBal, PreYrBalDrCr, Remarks, VatPanNo, Website FROM  " + prevDBName + ".Acc.tblLedger " +//--where  BuiltIn = 0" +
                                "SET IDENTITY_INSERT [" + newDBName + "].[Acc].[tblLedger] OFF";
                return Global.m_db.InsertUpdateQry(sql);
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// when closing the fiscal year only the builtin products are readily available in new DB, so other products are added from Previous DB to the new one 
        /// </summary>
        /// <param name="prevDBName"></param>
        /// <param name="newDBName"></param>
        /// <returns></returns>
        public static int AddProductToNewDB(string prevDBName, string newDBName)
        {
            try
            {
                Global.m_db.InsertUpdateQry("DELETE FROM [" + newDBName + "].[Inv].[tblOpeningQuantity]");
                Global.m_db.InsertUpdateQry("DELETE FROM [" + newDBName + "].[Inv].[tblProduct]");
                string sql =
                            "SET IDENTITY_INSERT [" + newDBName + "].[Inv].[tblProduct] ON " +
                            "INSERT INTO  " + newDBName + ".Inv.tblProduct (BackColor, BuiltIn, Created_By, Created_Date, DebtorsID, DepotID, EngName, GroupID, Image, IsActive, IsInventoryApplicable, IsVatApplicable, Modified_By, Modified_Date, NepName, ProductCode, ProductColor, ProductID, PurchaseDiscount, PurchaseRate, Quantity, Remarks, RentDate, SalesRate , TotalValue, UnitMaintenanceID) " +
                            "SELECT BackColor, BuiltIn, Created_By, Created_Date, DebtorsID, DepotID, EngName, GroupID, Image, IsActive, IsInventoryApplicable, IsVatApplicable, Modified_By, Modified_Date, NepName, ProductCode, ProductColor, ProductID, PurchaseDiscount, PurchaseRate, Quantity, Remarks, RentDate, SalesRate , TotalValue, UnitMaintenanceID FROM  " + prevDBName + ".Inv.tblProduct  " +//--where BuiltIn = 0 " +
                            "SET IDENTITY_INSERT [" + newDBName + "].[Inv].[tblProduct] OFF";
                return Global.m_db.InsertUpdateQry(sql);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// when closing the fiscal year only the builtin user i.e root is readily available in new DB, so other users are added from Previous DB to the new one 
        /// </summary>
        /// <param name="prevDBName"></param>
        /// <param name="newDBName"></param>
        /// <returns></returns>
        public static int AddUsersToNewDB(string prevDBName, string newDBName)
        {
            try
            {
                Global.m_db.InsertUpdateQry("delete from " + newDBName + ".System.tblUser ");
                Global.m_db.InsertUpdateQry("delete from " + newDBName + ".System.tblAccessRoleDtl ");
                Global.m_db.InsertUpdateQry("delete from " + newDBName + ".System.tblAccess ");
                Global.m_db.InsertUpdateQry("delete from " + newDBName + ".System.tblAccessRole ");

                string sql =
                             " SET IDENTITY_INSERT [" + newDBName + "].[System].[tblAccessRole] ON " +
                             " INSERT INTO  BENT002.System.tblAccessRole(BuiltIn, Created_By, Created_Date, Description, EngName, NepName, RoleID) " +
                             " SELECT BuiltIn, Created_By, Created_Date, Description, EngName, NepName, RoleID FROM  " + prevDBName + ".System.tblAccessRole " +//--where BuiltIn = 1" +
                             " SET IDENTITY_INSERT [" + newDBName + "].[System].[tblAccessRole] OFF ";
                Global.m_db.InsertUpdateQry(sql);

                sql = " SET IDENTITY_INSERT [" + newDBName + "].[System].[tblAccess] ON " +
                        " INSERT INTO  " + newDBName + ".System.tblAccess(AccessID, Code, Description, EngName, NepName, ParentID)" +
                        " SELECT AccessID, Code, Description, EngName, NepName, ParentID FROM  " + prevDBName + ".System.tblAccess" +// --where BuiltIn = 0" +
                        " SET IDENTITY_INSERT [" + newDBName + "].[System].[tblAccess] OFF";
                Global.m_db.InsertUpdateQry(sql);

                sql = " SET IDENTITY_INSERT [" + newDBName + "].[System].[tblAccessRoleDtl] ON " +
                        " INSERT INTO  " + newDBName + ".System.tblAccessRoleDtl(AccessID, RoleDtlID, RoleID)" +
                        " SELECT AccessID, RoleDtlID, RoleID FROM  " + prevDBName + ".System.tblAccessRoleDtl " +//--where BuiltIn = 0" +
                        " SET IDENTITY_INSERT [" + newDBName + "].[System].[tblAccessRoleDtl] OFF";
                Global.m_db.InsertUpdateQry(sql);

                sql = " SET IDENTITY_INSERT [" + newDBName + "].[System].[tblUser] ON " +
                " INSERT INTO  " + newDBName + ".System.tblUser(AccClassID, AccessRoleID, Address, Contact, CreatedBy, CreatedDate, Department, Email, ModifiedBy, ModifiedDate, Name, Password, UserID, UserName, UserStatus) " +
                " SELECT AccClassID, AccessRoleID, Address, Contact, CreatedBy, CreatedDate, Department, Email, ModifiedBy, ModifiedDate, Name, Password, UserID, UserName, UserStatus FROM  " + prevDBName + ".System.tblUser" +// -- where UserID != 1 " +
                " SET IDENTITY_INSERT [" + newDBName + "].[System].[tblUser] OFF ";

                return Global.m_db.InsertUpdateQry(sql);
            }
            catch (Exception)
            {

                throw;
            }
        }
        
        #endregion
        public static int FiscalYearClose(string prevDBName, string newDBName)
        {
            try
            {
                Global.m_db.ClearParameter();
                Global.m_db.AddParameter("@prevYrDBName", SqlDbType.NVarChar, prevDBName);
                Global.m_db.AddParameter("@newYrDBName", SqlDbType.NVarChar, newDBName);
                Global.m_db.setCommandText("System.spFiscalYearClosing");
                Global.m_db.setCommandType(CommandType.StoredProcedure);

                return Global.m_db.ProcessParameter();
            }
            catch (Exception)
            {
                
                throw;
            }
        }
    }
}
