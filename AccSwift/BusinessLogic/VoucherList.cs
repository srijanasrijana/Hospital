using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace BusinessLogic
{
    public class VoucherList
    {
        static int recordCount = 0;
        static DateTime MinDate;
        public static DataTable GetVoucherList(string MasterID, string[] VoucherValues, String NavigateTo,string SearchStr, string FromDate , string ToDate,int createdBy=0)
        {
            try
            {
                int VouchCode = 0;
               
                if (SearchStr != "" && SearchStr != null)
                {
                    if (VoucherValues[0] == "JOURNAL" || VoucherValues[0] == "CONTRA")
                    {
                      
                        VouchCode = 1;
                    }
                    else if (VoucherValues[0] == "BANK_RECEIPT" || VoucherValues[0] == "BANK_PAYMENT" || VoucherValues[0] == "CASH_RECEIPT" || VoucherValues[0] == "CASH_PAYMENT")
                    {
                        
                        VouchCode = 2;
                    }
                    else if (VoucherValues[0] == "PURCHASE_ORDER" || VoucherValues[0] == "SALES_ORDER")
                    {
                       
                        VouchCode = 3;
                    }
                    else if (VoucherValues[0] == "BANK_RECONCILIATION")
                    {
                        
                        VouchCode = 5;
                    }
                    else
                        VouchCode = 4;

                }
                else
                    VouchCode = 0;

                Global.m_db.ClearParameter();
                Global.m_db.AddParameter("@MasterID", SqlDbType.NVarChar, MasterID);
                Global.m_db.AddParameter("@VoucherType", SqlDbType.NVarChar, VoucherValues[0]);
                Global.m_db.AddParameter("@NavigateTo", SqlDbType.NVarChar, NavigateTo);
                Global.m_db.AddParameter("@MasterTableName", SqlDbType.NVarChar, VoucherValues[1]);
                Global.m_db.AddParameter("@DetailsTableName", SqlDbType.NVarChar, VoucherValues[2]);
                Global.m_db.AddParameter("@FieldName", SqlDbType.NVarChar, VoucherValues[3]);
                Global.m_db.AddParameter("@FilterStr", SqlDbType.NVarChar, SearchStr);
                Global.m_db.AddParameter("@VouchCode", SqlDbType.Int, VouchCode);
                Global.m_db.AddParameter("@FromDate", SqlDbType.NVarChar, FromDate);
                Global.m_db.AddParameter("@ToDate", SqlDbType.NVarChar, ToDate);
                Global.m_db.AddParameter("@DateField", SqlDbType.NVarChar, VoucherValues[4]);
                Global.m_db.AddParameter("@CreatedBy", SqlDbType.NVarChar, createdBy);

                SqlParameter outputParam1 = Global.m_db.AddOutputParameter("@RecordCount", SqlDbType.Int);
                SqlParameter outputParam2 = Global.m_db.AddOutputParameter("@MinDate", SqlDbType.Date);

                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("System.spGetVoucherList");
                
                DataTable dt = Global.m_db.GetDataTable();
                try
                {
                    recordCount = Convert.ToInt32(outputParam1.Value);
                    MinDate = Convert.ToDateTime(outputParam2.Value);                    
                }
                catch (Exception)
                {
                    recordCount = 0;
                }


                dt.DefaultView.Sort = "ID";  // sort the datatable according to MasterID in ascending order
                dt = dt.DefaultView.ToTable();
                return dt;
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
                return null;
            }          
        }

        public static int GetRecordCount()
        {
            return recordCount;
        }
        public static DateTime GetFromDate()
        {
            return MinDate;
        }

        //public static void SetFromDate(String MasterTableName)
        //{
        //    String sql = "select min(Created_Date) from "+MasterTableName+"";
        //    DataTable dt = Global.m_db.SelectQry(sql,MasterTableName);

        //    fromDate = Convert.ToDateTime(dt.Rows[0][0].ToString());
        // //   return fromDate;
        //}
    }
}
