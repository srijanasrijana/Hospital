using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;
using System.Windows.Forms;

namespace BusinessLogic
{
    public class InventoryTrans
    {

        public static DataTable GetDistinctDepotID(DateTime Date)
        {
            DataTable dtResult=new DataTable();

            dtResult= Global.m_db.SelectQry("SELECT DISTINCT DepotID from Inv.tblInventoryTrans WHERE TransactDate<='" + Date + "'", "tbl");

            return dtResult;

        }

        public static DataTable GetDistinctDepotID()
        {
            DataTable dtResult = new DataTable();

            dtResult = Global.m_db.SelectQry("SELECT DISTINCT DepotID from Inv.tblInventoryTrans", "tbl");
            
            return dtResult;

        }

        public static DataTable GetDistinctProductID(int DepotID,int GroupID)
        {
            if (GroupID == -1)
            {
                return Global.m_db.SelectQry("SELECT DISTINCT ProductID from Inv.tblInventoryTrans WHERE DepotID ='" + DepotID.ToString() + "'", "tbl");
            }
            else
            {

                return Global.m_db.SelectQry("SELECT * FROM Inv.tblInventoryTrans WHERE ProductID IN (SELECT ProductID from Inv.tblProduct WHERE GroupID ='" + GroupID + "') AND DepotID ='"+DepotID+"'", "tbl");
            }
        
        }

        public static int GetProductQuantity(int ProductID, DateTime Date, int DepotID, int GroupID)
        {
            

            string SQLGroup = "' AND b.GroupID = '" + GroupID;
            if (GroupID == -1)
                SQLGroup = "";

            string SQLDepot = "' AND a.DepotID ='" + DepotID;
            if (DepotID == -1)
                SQLDepot = "";

            DataTable dt = new DataTable();
   
            if(Date == null)
            {
                 dt = Global.m_db.SelectQry("SELECT a.TransactDate,a.DepotID,a.ProductID,a.Incoming,a.Outgoing,b.GroupID,b.Quantity FROM Inv.tblInventoryTrans a, Inv.tblProduct b WHERE a.ProductID = b.ProductID AND  b.ProductID ='" + ProductID + SQLDepot + SQLGroup + "'", "tbl");
            }
            else
            {

               dt = Global.m_db.SelectQry("SELECT a.TransactDate,a.DepotID,a.ProductID,a.Incoming,a.Outgoing,b.GroupID,b.Quantity FROM Inv.tblInventoryTrans a, Inv.tblProduct b WHERE a.ProductID = b.ProductID AND  b.ProductID ='" + ProductID + "' AND a.TransactDate<='" + Date + SQLDepot+ SQLGroup + "'", "tbl");

            }


            int NetQuantity=0;
         
            foreach (DataRow dr in dt.Rows)
            {
                int IncomingQuantity, OutgoingQuantity;
               
                IncomingQuantity = OutgoingQuantity= NetQuantity= 0;

                IncomingQuantity += Convert.ToInt32(dr["Incoming"]);

                OutgoingQuantity += Convert.ToInt32(dr["Outgoing"]);

                NetQuantity = NetQuantity+(IncomingQuantity - OutgoingQuantity);
            
            
            }

            int OpeningQuantity = 0;
            foreach (DataRow drProductInfo in dt.Rows)

            {
               
                OpeningQuantity = Convert.ToInt32(drProductInfo["Quantity"]);
              
                      
            }

            NetQuantity = (NetQuantity + OpeningQuantity);

            return NetQuantity;
 
        }
    }
}
