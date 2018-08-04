using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Windows.Forms;

namespace BusinessLogic
{
    public class InventoryBook
    {

        /// <summary>
        /// Returns the Opening Quantity of the Product or Group from given RootAccClassID and ProductID or GroupID. To show all, provide none
        /// </summary>
        /// <param name="RootAccClassID"></param>
        /// <param name="ProductID"></param>
        /// <param name="GroupID"></param>
        /// <returns></returns>
        public static int GetOpeningQty(int RootAccClassID, int ProductID=-1, List<int> GroupIDList=null)
        {


            //Check if RootAccClassID is valid
            if (!(RootAccClassID > 0))
                throw new Exception("Invalid RootAccClassID");

            try
            {

                DataTable dtRootAccClass = AccountClass.GetRootAccClass(RootAccClassID);
                RootAccClassID = Convert.ToInt32(dtRootAccClass.Rows[0]["AccClassID"]);
            }
            catch
            {
                throw new Exception("Invalid Accounting Class selected");
            }

            try
            {
                if (ProductID > 0) //If ProductID is provided
                {

                    DataTable dtQty = Global.m_db.SelectQry("Select OpenPurchaseQty FROM Inv.tblOpeningQuantity WHERE ProductID='" + ProductID.ToString() + "' AND AccClassID='" + RootAccClassID.ToString() + "'", "tblOpeningBalance");
                    return Convert.ToInt32(dtQty.Rows[0]["OpenPurchaseQty"]);
                }
                else if (GroupIDList!=null) //If GroupID is provided
                {
                    DataTable dtQty = Global.m_db.SelectQry("Select SUM(OpenPurchaseQty) as OpenPurchaseQty FROM Inv.tblOpeningQuantity WHERE ProductID IN (SELECT ProductID FROM Inv.tblProduct WHERE GroupID IN (" + string.Join(",", GroupIDList.ToArray()) + ")) AND AccClassID='" + RootAccClassID.ToString() + "'", " Inv.tblOpeningQuantity ");
                    return Convert.ToInt32(dtQty.Rows[0]["OpenPurchaseQty"]);
                }
                else //If both are not provided show all
                {
                    DataTable dtQty = Global.m_db.SelectQry("Select SUM(OpenPurchaseQty) as OpenPurchaseQty FROM Inv.tblOpeningQuantity WHERE AccClassID='" + RootAccClassID.ToString() + "'", " Inv.tblOpeningQuantity ");
                    return Convert.ToInt32(dtQty.Rows[0]["OpenPurchaseQty"]);
                }
            }
            catch (Exception ex)
            {
                
                
                return 0;//Because when there is nothing entered on the opening balance for root accounting class ID it generates error
            }
        }


        public static DataTable GetInventoryBook(int? PartyGroupID, int? PartyID, int? ProducGroupID, int? ProductID, int? DepotID,
            int? ProjectID, DateTime? FromDate, DateTime? ToDate, InvenotryBookType InventoryBookType, string AccClassIDsXMLString,
            string ProjectIDsXMLString, ref decimal openingQuantity)
        {
            try
            {
                Global.m_db.ClearParameter();
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("Inv.spGetInventoryBook");
                Global.m_db.AddParameter("@ProductGroupID", SqlDbType.Int, ProducGroupID);
                Global.m_db.AddParameter("@ProductID", SqlDbType.Int, ProductID);
                Global.m_db.AddParameter("@PartyGroupID", SqlDbType.Int, PartyGroupID);
                Global.m_db.AddParameter("@PartyID", SqlDbType.Int, PartyID);
                Global.m_db.AddParameter("@DepotID", SqlDbType.Int, DepotID);
                Global.m_db.AddParameter("@ProjectID", SqlDbType.Int, ProjectID);
                Global.m_db.AddParameter("@FromDate", SqlDbType.DateTime, FromDate);
                Global.m_db.AddParameter("@ToDate", SqlDbType.DateTime, ToDate);
                Global.m_db.AddParameter("@InventoryBookTypeIndex", SqlDbType.Int, (int)InventoryBookType);
                Global.m_db.AddParameter("@AccountClassIDsSettings", SqlDbType.Xml, AccClassIDsXMLString);
                Global.m_db.AddParameter("@ProjectIDsSettings", SqlDbType.Xml, ProjectIDsXMLString);

                System.Data.SqlClient.SqlParameter objReturn = Global.m_db.AddOutputParameter("@OpeningQuantity", SqlDbType.NVarChar, 100);

                DataTable dtInventoryDayBookDtl = Global.m_db.GetDataTable();

                openingQuantity = Convert.ToDecimal(objReturn.Value);

                return dtInventoryDayBookDtl;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                // throw ex;
                return null;
            }

        }

        public static DataTable GetInvPartyLedgerReport(int? PartyGroupID, int? PartyID, DateTime? FromDate, DateTime? ToDate, string AccClassIDsXMLString, string ProjectIDsXMLString,string VoucherType)
        {
            try
            {
                Global.m_db.ClearParameter();
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("Inv.spGetProductPartyDetails");
                Global.m_db.AddParameter("@PartyID", SqlDbType.Int, PartyID);
                Global.m_db.AddParameter("@PartyGroupID", SqlDbType.Int, PartyGroupID);
                Global.m_db.AddParameter("@FromDate", SqlDbType.DateTime, FromDate);
                Global.m_db.AddParameter("@ToDate", SqlDbType.DateTime, ToDate);
                Global.m_db.AddParameter("@AccountClassIDsSettings", SqlDbType.Xml, AccClassIDsXMLString);
                Global.m_db.AddParameter("@ProjectIDsSettings", SqlDbType.Xml, ProjectIDsXMLString);
                Global.m_db.AddParameter("@VoucherType", SqlDbType.NVarChar,50, VoucherType);
                return Global.m_db.GetDataTable();
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        }
}
