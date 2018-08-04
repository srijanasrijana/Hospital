using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Windows.Forms;
using DBLogic;
using System.Data;
using System.IO;
using DateManager;
using RegistryManager;
using System.Drawing;

namespace BusinessLogic
{
   public  class SalesInvoiceModel
    {
       DataTable dt;
       //protected SqlConnection con;
       //protected SqlCommand cmd;
        public  DataTable GetProductCategory()
        {
            try
            {
                //con = new SqlConnection();
                string strQuery = "";
                strQuery = "select * from Inv.tblProductGroup where EngName <>'Root'";
                dt=Global.m_db.SelectQry(strQuery, "tblProductGroup");
                return dt;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                return dt;
            }
            // return Global.m_db.SelectQry(strQuery, "tblProduct");
        }
        //public static DataTable GetAllProduct()
        //{
        //    string strquery = "";
        //    strquery = "select * from Inv.tblProduct";
        //    return Global.m_db.SelectQry(strquery, "tblProduct");
        //}


       /// <summary>
       /// Get all products filtering through ProductID, ProductCode and AccClassID
       /// </summary>
       /// <param name="ProductID"></param>
       /// <param name="ProductCode"></param>
       /// <param name="AccClassID"></param>
       /// <returns></returns>
        public static DataTable GetProducts(int ProductID=0, string ProductCode=null, int AccClassID=0)
        {
            string strquery = "", productIDSQL="", productCodeSQL="", AccClassSQL="";
            //No Parameter given, show all
            //if (ProductID == 0 && ProductCode == null && AccClassID == 0)
            //    strquery = "select * from Inv.tblProduct";
            if (ProductID > 0)
                productIDSQL = " AND p.ProductID='" + ProductID + "'";
            if (ProductCode != null)
                productCodeSQL = " AND p.ProductCode='" + ProductCode + "'";
            if (AccClassID > 0)
                AccClassSQL = " AND oq.AccClassID='" + AccClassID + "'";

            strquery = "select p.productid,p.ProductCode ProductCode,p.EngName,p.UnitMaintenanceID UnitID,oq.OpenPurchaseRate PurchaseRate,oq.OpenSalesRate salesrate, oq.AccClassID AccClassID, pg.EngName GroupType from Inv.tblProduct p left join Inv.tblProductGroup pg on p.GroupID=pg.GroupID left join inv.tblOpeningQuantity oq on p.ProductID=oq.ProductID WHERE 1=1 " + AccClassSQL + productCodeSQL + productIDSQL;
            return Global.m_db.SelectQry(strquery, "tblProduct");
        }
        
        public static DataTable GetAllProductByIDAndAccClassID(int ProductID,int AccClassID)
        {
            string strquery = "";
            strquery = "select p.productid,p.ProductCode ProductCode,p.EngName,oq.OpenPurchaseRate PurchaseRate,oq.OpenSalesRate salesrate, oq.AccClassID AccClassID, pg.EngName GroupType from Inv.tblProduct p left join Inv.tblProductGroup pg on p.GroupID=pg.GroupID left join inv.tblOpeningQuantity oq on p.ProductID=oq.ProductID WHERE oq.AccClassID='" + AccClassID + "' and p.ProductID='" + ProductID + "'";
            return Global.m_db.SelectQry(strquery, "tblProduct");
        }

        public static DataTable GetAllProductbycategoryid(string catid)
        {
            string strquery = "";
            strquery = "select * from Inv.tblProduct where groupid='"+catid+"'";
            return Global.m_db.SelectQry(strquery, "tblProduct");
        }
        public byte[] GetProductImgFromID(string productid)
        {
            long imageLen;
            byte[] ImageData = new byte[1];

            try
            {
                string strquery = "";
                strquery = "select image from Inv.tblProduct where ProductID='" + productid + "'";
                DataTable dtimage = Global.m_db.SelectQry(strquery, "tblProduct");
                imageLen = Convert.ToInt64(dtimage.Rows[0]["image"].ToString().Count());
                if (imageLen > 0)
                {
                     ImageData = (byte[])dtimage.Rows[0]["image"];
                }
                else
                {
                    ImageData = null;
                }
                return ImageData;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }

        }
      
    }
}
