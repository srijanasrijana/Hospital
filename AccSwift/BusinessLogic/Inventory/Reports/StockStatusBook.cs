using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Windows.Forms;

namespace BusinessLogic
{
    public class StockStatusBook
    {
        public static DataTable GetStockStatusBook(int? ProducGroupID, int? ProductID, String Depot, DateTime? AtTheEndDate, bool ShowZeroQuantity, StockStatusType OpeningStock, string AccClassIDsXMLString)
        {
            try
            {
                Global.m_db.ClearParameter();
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("Inv.spGetStockStatusBook");
                Global.m_db.AddParameter("@ProductGroupID", SqlDbType.Int, ProducGroupID);
                Global.m_db.AddParameter("@ProductID", SqlDbType.Int, ProductID);
                Global.m_db.AddParameter("@Depot", SqlDbType.NVarChar, Depot);
                Global.m_db.AddParameter("@AtTheEndDate", SqlDbType.DateTime, AtTheEndDate);
                Global.m_db.AddParameter("@ShowZeroQuantity", SqlDbType.Bit,ShowZeroQuantity);
                Global.m_db.AddParameter("@StockStatusTypeIndex", SqlDbType.Int, (int)OpeningStock);
                Global.m_db.AddParameter("@AccountClassIDsSettings", SqlDbType.Xml, AccClassIDsXMLString);
                DataTable dtStockStatusDayBookDtl = Global.m_db.GetDataTable();
                return dtStockStatusDayBookDtl;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw ex;
            }

        }

        public static DataTable GetOpeningStockStatusBook(int? ProducGroupID, int? ProductID, String Depot, DateTime? AtTheEndDate, bool ShowZeroQuantity, StockStatusType OpeningStock, string AccClassIDsXMLString)
        {
            try
            {  

                int t = (int)OpeningStock;
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
                Global.m_db.AddParameter("@StockStatusTypeIndex", SqlDbType.Int, (int)OpeningStock);
                Global.m_db.AddParameter("@AccountClassIDsSettings", SqlDbType.Xml, AccClassIDsXMLString);
                //Global.m_db.AddParameter("@ProjectIDsSettings", SqlDbType.Xml, ProjectIDXMLString);
                DataTable dtStockStatusDayBookDtl = Global.m_db.GetDataTable();
                return dtStockStatusDayBookDtl;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw ex;
            }
        }

        public static DataTable GetStockStatusBook1(int? ProducGroupID, int? ProductID, String Depot, DateTime? AtTheEndDate, bool ShowZeroQuantity, StockStatusType OpeningStock, string AccClassIDsXMLString, string ProjectIDXMLString)
        {
            try
            {
                int t = (int)OpeningStock;
                Global.m_db.ClearParameter();
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("Inv.spGetStockStatusBook");
                Global.m_db.AddParameter("@ProductGroupID", SqlDbType.Int, ProducGroupID);
                Global.m_db.AddParameter("@ProductID", SqlDbType.Int, ProductID);
                Global.m_db.AddParameter("@Depot", SqlDbType.NVarChar, Depot);
                Global.m_db.AddParameter("@AtTheEndDate", SqlDbType.DateTime, AtTheEndDate);
                Global.m_db.AddParameter("@ShowZeroQuantity", SqlDbType.Bit, ShowZeroQuantity);
                Global.m_db.AddParameter("@StockStatusTypeIndex", SqlDbType.Int, (int)OpeningStock);
                Global.m_db.AddParameter("@AccountClassIDsSettings", SqlDbType.Xml, AccClassIDsXMLString);
                Global.m_db.AddParameter("@ProjectIDsSettings", SqlDbType.Xml, ProjectIDXMLString);
                DataTable dtStockStatusDayBookDtl = Global.m_db.GetDataTable();
                return dtStockStatusDayBookDtl;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw ex;
            }
        }
           
    }
}
