using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;
using System.Windows.Forms;

namespace BusinessLogic
{
    public class UnitMaintenance
    {

        public void Save(string UnitName, string Symbol, string Remarks)
        {

          
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("System.spUnitMaintenanceCreate");
                Global.m_db.AddParameter("@UnitName", SqlDbType.NVarChar,150, UnitName);
                Global.m_db.AddParameter("@Symbol", SqlDbType.NVarChar, 30, Symbol);         
                Global.m_db.AddParameter("@Remarks", SqlDbType.NVarChar, 200, Remarks);
                Global.m_db.AddParameter("@Created_By", SqlDbType.NVarChar, 50, User.CurrUserID.ToString());
                System.Data.SqlClient.SqlParameter objReturn = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                Global.m_db.ProcessParameter();

  
        
        }

        public void Modify(int UnitMaintenanceID, string UnitName, string Symbol, string Remarks)
        {

            try
            {

                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("System.spUnitMaintenanceModify");
                Global.m_db.AddParameter("@UnitMaintenanceID", SqlDbType.Int, UnitMaintenanceID);
                Global.m_db.AddParameter("@UnitName", SqlDbType.NVarChar, 150, UnitName);//Set same for both for time being
                Global.m_db.AddParameter("@Symbol", SqlDbType.NVarChar, 50, Symbol);
                Global.m_db.AddParameter("@Remarks", SqlDbType.NVarChar, 200, Remarks);
                Global.m_db.AddParameter("@Modified_By", SqlDbType.NVarChar, 50, User.CurrUserID.ToString());
                System.Data.SqlClient.SqlParameter objReturn = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                Global.m_db.ProcessParameter();


            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        
       
        }

        public void Delete(int UnitMaintenanceID)
        {

            Global.m_db.InsertUpdateQry("DELETE FROM System.tblUnitMaintenance WHERE UnitMaintenanceID ='" + UnitMaintenanceID + "'");
        
        }

        public static  DataTable GetUnitMaintenaceInfo(int UnitMaintenanceID)
        {

            if (UnitMaintenanceID == -1)
            {
                return Global.m_db.SelectQry("SELECT * FROM System.tblUnitMaintenance ", "tbl");
            }
            else
            {

                return Global.m_db.SelectQry("SELECT * FROM System.tblUnitMaintenance WHERE UnitMaintenanceID = '" + UnitMaintenanceID + "'", "tbl");
            
            }
        
        
        
        }

        public static int CompoundUnitCreate(int UnitID, int ParentUnitID, decimal RelationValue, string Remarks, int CreatedBy, DateTime Date)
        {
            try
            {
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("System.spCompoundUnitSave");
                Global.m_db.AddParameter("@UnitID", SqlDbType.Int, 150, UnitID);
                Global.m_db.AddParameter("@ParentUnitID", SqlDbType.Int, 30, ParentUnitID);
                Global.m_db.AddParameter("@RelationValue",SqlDbType.Decimal, 200, RelationValue);
                Global.m_db.AddParameter("@Remarks", SqlDbType.NVarChar, 50, Remarks);
                Global.m_db.AddParameter("@CreatedBy", SqlDbType.Int, 50, User.CurrUserID.ToString());
                Global.m_db.AddParameter("@CreatedDate", SqlDbType.DateTime, 50, DateManager.Date.ToDB(DateTime.Today));

                //System.Data.SqlClient.SqlParameter objReturn = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                return Global.m_db.ProcessParameter();
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }

        public static DataTable GetCompoundUnit(int CompoundUnitID = 0)
        {
            try
            {
                string sql = "select CompoundUnitID, (select UnitName from System.tblUnitMaintenance um where um.UnitMaintenanceID = cu.UnitID) ChildUnit, "+
                        "(select UnitName from System.tblUnitMaintenance um where um.UnitMaintenanceID = cu.ParentUnitID) ParentUnit, RelationValue, Remarks FROM System.tblCompoundUnit cu ";

                if (CompoundUnitID > 0)
                {
                    sql += "  where cu.CompoundUnitID = " + CompoundUnitID;
                }

                return Global.m_db.SelectQry(sql, "System.tblCompoundUnit");

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static int DeleteCompoundUnit(int CompoundUnitID)
        {
            try
            {
                return Global.m_db.InsertUpdateQry("delete from System.tblCompoundUnit where CompoundUnitID =" + CompoundUnitID + "");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static int CompoundUnitModify(int CompoundUnitID, int UnitID, int ParentUnitID, decimal RelationValue, string Remarks, int ModifiedBy, DateTime Date)
        {
            try
            {
                int res = 0;
                Global.m_db.BeginTransaction();
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("System.spCompoundUnitSave");
                Global.m_db.AddParameter("@CompoundUnitID", SqlDbType.Int, 150, CompoundUnitID);
                Global.m_db.AddParameter("@UnitID", SqlDbType.Int, 150, UnitID);
                Global.m_db.AddParameter("@ParentUnitID", SqlDbType.Int, 30, ParentUnitID);
                Global.m_db.AddParameter("@RelationValue", SqlDbType.Decimal, 200, RelationValue);
                Global.m_db.AddParameter("@Remarks", SqlDbType.NVarChar, 500, Remarks);
                Global.m_db.AddParameter("@MidifiedBy", SqlDbType.Int, 50, User.CurrUserID.ToString());
                Global.m_db.AddParameter("@MdifiedDate", SqlDbType.DateTime, 50, DateManager.Date.ToDB(DateTime.Today));
                Global.m_db.AddParameter("@IsNew", SqlDbType.Bit, 1, 0);
                res= Global.m_db.ProcessParameter();
                Global.m_db.CommitTransaction();

                return res;
            }
            catch (Exception ex)
            {
                Global.m_db.RollBackTransaction();
                throw ex;
            }
        }

        /// <summary>
        /// check if the relation between two units already exists in the database
        /// </summary>
        /// <param name="FirstUnitID"></param>
        /// <param name="SecondUnitID"></param>
        /// <returns></returns>
        public static bool CheckIfCompoundUnitExists(int FirstUnitID, int SecondUnitID)
        {
            try
            {
               DataTable dt = Global.m_db.SelectQry("select CompoundUnitID from System.tblCompoundUnit where UnitID in ("+FirstUnitID+","+SecondUnitID+") and ParentUnitID in ("+FirstUnitID+","+SecondUnitID+")", "tblCompoundUnit");
               if (dt.Rows.Count > 0)
                   return true;
               else
                   return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// gets all related units for the default unit . Eg: if kg is default unit then gm, mg, ton... are related units
        /// </summary>
        /// <param name="UnitID"></param>
        /// <param name="ProductID"></param>

        /// <returns></returns>
        public static DataTable GetAllRelatedUnit(int ProductID, int UnitID=0)
        {
            try
            {
                Global.m_db.ClearParameter();
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("System.spGetAllRelatedUnits");
                Global.m_db.AddParameter("@UnitID", SqlDbType.Int, 150, UnitID);
                Global.m_db.AddParameter("@ProductID", SqlDbType.Int, 150, ProductID);

                return Global.m_db.GetDataTable();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static Decimal ConvertCompoundUnit(int defaultUnitID, int curUnitID, decimal actualValue)
        {
            try
            {
                Global.m_db.ClearParameter();
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.AddParameter("@defaultUnitID", SqlDbType.Int, 150, defaultUnitID);
                Global.m_db.AddParameter("@curUnit", SqlDbType.Int, 150, curUnitID);
                Global.m_db.AddParameter("@actualValue", SqlDbType.Decimal, 150, actualValue);

                string sql = "select System.fnConvertCompoundUnit(@defaultUnitID, @curUnit, @actualValue)";

                return Convert.ToDecimal(Global.m_db.GetScalarValue(sql));
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }

    }
}
