using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace BusinessLogic.Inventory
{
  public  class StockTransfer
    {
        public void Create(int SeriesID, string VoucherNo,DateTime StockTransferDate, int FromDepotID,int ToDepotID, string Remarks, DataTable StockTransferDetails, int[] AccClassID,OptionalField of)
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
            if (StockTransferDetails.Rows.Count == 0)
            {
                throw new Exception("Please fill the ledger details");
                return;
            }
           
            ////This loop is to check whether ledger names are correct and properly implemented
            foreach (DataRow row in StockTransferDetails.Rows)
            {
                //Check whether the ledger name are correct
                Global.m_db.ClearParameter();
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("Inv.spGetProductIDFromName");
                Global.m_db.AddParameter("@ProductName", SqlDbType.NVarChar, 50, row["Product"].ToString());
                Global.m_db.AddParameter("@Language", SqlDbType.NVarChar, 20, Language);
                System.Data.SqlClient.SqlParameter objReturn = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                Global.m_db.ProcessParameter();

                if (objReturn.Value.ToString() == "-100")//It return -100 in case of failure
                    throw new Exception("Product Name - " + row["Product"].ToString() + " not found!");
            }
          
            try
            {
                // Global.m_db.InsertUpdateQry("INSERT INTO Account.tblAccountHead(AccountHeadName,Under_Account_ID,Description,BuiltIn) VALUES ('" + AccountHeadName + "','1','" + Description + "','0')");
                Global.m_db.BeginTransaction();
                Global.m_db.ClearParameter();
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("Inv.spStockTransferMasterCreate");
                Global.m_db.AddParameter("@SeriesID", SqlDbType.Int, SeriesID);
                Global.m_db.AddParameter("@FromDepotID", SqlDbType.Int, FromDepotID);
                Global.m_db.AddParameter("@ToDepotID", SqlDbType.Int, ToDepotID);
                Global.m_db.AddParameter("@Voucher_No", SqlDbType.NVarChar, 30, VoucherNo);//Set same for both for time being
                Global.m_db.AddParameter("@StockTransfer_Date", SqlDbType.DateTime, StockTransferDate);
                Global.m_db.AddParameter("@Remarks", SqlDbType.NVarChar, 200, Remarks);
                Global.m_db.AddParameter("@Created_By", SqlDbType.NVarChar, 50, User.CurrUserID.ToString());
                Global.m_db.AddParameter("@First", SqlDbType.NVarChar, 50, of.First);
                Global.m_db.AddParameter("@Second", SqlDbType.NVarChar, 50, of.Second);
                Global.m_db.AddParameter("@Third", SqlDbType.NVarChar, 50, of.Third);
                Global.m_db.AddParameter("@Fourth", SqlDbType.NVarChar, 50, of.Fourth);
                Global.m_db.AddParameter("@Fifth", SqlDbType.NVarChar, 50, of.Fifth);
                System.Data.SqlClient.SqlParameter objReturn = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                Global.m_db.ProcessParameter();
                int ReturnID = Convert.ToInt32(objReturn.Value);
               
                

                for (int i = 0; i < StockTransferDetails.Rows.Count; i++)
                {
                    DataRow dr = StockTransferDetails.Rows[i];
                    //Now go for the Detail Inserts
                    Global.m_db.ClearParameter();
                    Global.m_db.setCommandType(CommandType.StoredProcedure);
                    Global.m_db.setCommandText("Inv.spStockTransferDetailCreate");
                    Global.m_db.AddParameter("@StockTransferID", SqlDbType.Int, ReturnID.ToString());
                    Global.m_db.AddParameter("@Code", SqlDbType.NVarChar, 30, dr["Code"].ToString());
                    Global.m_db.AddParameter("@ProductName", SqlDbType.NVarChar, 30, dr["Product"].ToString());//Set same for both for time being
                    Global.m_db.AddParameter("@Language", SqlDbType.NVarChar, 30, Language);
                    Global.m_db.AddParameter("@Quantity", SqlDbType.Int, Convert.ToInt32(dr["Quantity"]));
                    Global.m_db.AddParameter("@PurchaseRate", SqlDbType.Money, Convert.ToDouble(dr["PurchaseRate"]));
                    Global.m_db.AddParameter("@Amount", SqlDbType.Money, Convert.ToDouble(dr["Amount"]));
                    System.Data.SqlClient.SqlParameter paramReturn = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                    Global.m_db.ProcessParameter();
                    if (paramReturn.Value.ToString() != "SUCCESS")
                    {
                        Global.m_db.RollBackTransaction();
                        throw new Exception("Unable to perform Stock Transfer");
                    }

                    #region This code will use when inventory system is implemented
                    ////Addition in Inv.tblInventoryTrans 

                    //Also insert the transaction in Inv.tblInventoryTrnas  
                    Global.m_db.ClearParameter();
                    Global.m_db.setCommandType(CommandType.StoredProcedure);
                    Global.m_db.setCommandText("Inv.spInventoryTransCreate");
                    Global.m_db.AddParameter("@Date", SqlDbType.DateTime, StockTransferDate);
                    Global.m_db.AddParameter("@ProductName", SqlDbType.NVarChar, 30, dr["Product"].ToString());//Set same for both for time being
                    Global.m_db.AddParameter("@DepotID", SqlDbType.Int, ToDepotID);
                    Global.m_db.AddParameter("@Language", SqlDbType.NVarChar, 30, Language);
                    Global.m_db.AddParameter("@Quantity", SqlDbType.Int, Convert.ToInt32(dr["Quantity"]));
                    Global.m_db.AddParameter("@InOut", SqlDbType.NVarChar, 10, "INCOMING");
                    Global.m_db.AddParameter("@VoucherType", SqlDbType.NVarChar, 20, "STOCK_TRANS");
                    Global.m_db.AddParameter("@RowID", SqlDbType.Int, ReturnID);
                    System.Data.SqlClient.SqlParameter ParamStockInInventory = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                    Global.m_db.ProcessParameter();
                    #endregion     
  
                    #region This code will use when inventory system is implemented
                    ////Addition in Inv.tblInventoryTrans 

                    //Also insert the transaction in Inv.tblInventoryTrnas  
                    Global.m_db.ClearParameter();
                    Global.m_db.setCommandType(CommandType.StoredProcedure);
                    Global.m_db.setCommandText("Inv.spInventoryTransCreate");
                    Global.m_db.AddParameter("@Date", SqlDbType.DateTime, StockTransferDate);
                    Global.m_db.AddParameter("@ProductName", SqlDbType.NVarChar, 30, dr["Product"].ToString());//Set same for both for time being
                    Global.m_db.AddParameter("@DepotID", SqlDbType.Int, FromDepotID);
                    Global.m_db.AddParameter("@Language", SqlDbType.NVarChar, 30, Language);
                    Global.m_db.AddParameter("@Quantity", SqlDbType.Int, Convert.ToInt32(dr["Quantity"]));
                    Global.m_db.AddParameter("@InOut", SqlDbType.NVarChar, 10, "OUTGOING");
                    Global.m_db.AddParameter("@VoucherType", SqlDbType.NVarChar, 20, "STOCK_TRANS");
                    Global.m_db.AddParameter("@RowID", SqlDbType.Int, ReturnID);
                    System.Data.SqlClient.SqlParameter ParamStockOutInventory = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                    Global.m_db.ProcessParameter();
                    #endregion      
                }


                foreach (int _AccClassID in AccClassID)
                {
                    Global.m_db.ClearParameter();
                    Global.m_db.setCommandType(CommandType.StoredProcedure);
                    Global.m_db.setCommandText("Inv.spStockTransferAccClassCreate");
                    Global.m_db.AddParameter("@StockTransferID", SqlDbType.Int, ReturnID.ToString());
                    Global.m_db.AddParameter("@AccClassID", SqlDbType.Int, _AccClassID);//Set same for both for time being
                    System.Data.SqlClient.SqlParameter paramTransactClassID = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                    Global.m_db.ProcessParameter();

                    if (paramTransactClassID.Value.ToString() == "FAILURE")
                    {
                        Global.m_db.RollBackTransaction();
                        throw new Exception("Unable to create Purchase Order");
                    }
                }

               

                Global.m_db.CommitTransaction();
            }
            catch (Exception ex)
            {
                Global.m_db.RollBackTransaction();

                throw ex;

               
            }
        }

        public void Modify(int StockTransferID,int SeriesID, string VoucherNo, DateTime StockTransferDate, int FromDepotID, int ToDepotID, string Remarks, DataTable StockTransferDetails, int[] AccClassID,OptionalField of)
        {
           
          

            try
            {
                // Global.m_db.InsertUpdateQry("INSERT INTO Account.tblAccountHead(AccountHeadName,Under_Account_ID,Description,BuiltIn) VALUES ('" + AccountHeadName + "','1','" + Description + "','0')");
                Global.m_db.BeginTransaction();
                Global.m_db.ClearParameter();
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("Inv.spStockTransferMasterModify");
                Global.m_db.AddParameter("StockTransferID", SqlDbType.Int, StockTransferID);
                Global.m_db.AddParameter("@SeriesID", SqlDbType.Int, SeriesID);
                Global.m_db.AddParameter("@FromDepotID", SqlDbType.Int, FromDepotID);
                Global.m_db.AddParameter("@ToDepotID", SqlDbType.Int, ToDepotID);
                Global.m_db.AddParameter("@Voucher_No", SqlDbType.NVarChar, 30, VoucherNo);//Set same for both for time being
                Global.m_db.AddParameter("@StockTransfer_Date", SqlDbType.DateTime, StockTransferDate);
                Global.m_db.AddParameter("@Remarks", SqlDbType.NVarChar, 200, Remarks);
                Global.m_db.AddParameter("@Modified_By", SqlDbType.NVarChar, 50, User.CurrUserID.ToString());
                Global.m_db.AddParameter("@First", SqlDbType.NVarChar, 50, of.First);
                Global.m_db.AddParameter("@Second", SqlDbType.NVarChar, 50, of.Second);
                Global.m_db.AddParameter("@Third", SqlDbType.NVarChar, 50, of.Third);
                Global.m_db.AddParameter("@Fourth", SqlDbType.NVarChar, 50, of.Fourth);
                Global.m_db.AddParameter("@Fifth", SqlDbType.NVarChar, 50, of.Fifth);
                System.Data.SqlClient.SqlParameter objReturn = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                Global.m_db.ProcessParameter();

                Global.m_db.InsertUpdateQry("DELETE FROM Inv.tblStockTransferDetails WHERE StockTransferID='" + StockTransferID.ToString() + "'");
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

                //First delete the old transaction
                Global.m_db.InsertUpdateQry("DELETE FROM inv.tblInventoryTrans WHERE VoucherType='STOCK_TRANS' AND RowID='" + StockTransferID.ToString() + "'");

                //First delete the previous records of Acc.tblTransactionClass according to VoucherType and RowID
                Global.m_db.InsertUpdateQry("DELETE FROM inv.tblStockTransferAccClass WHERE  AND RowID='" + StockTransferID.ToString() + "'");



                for (int i = 0; i < StockTransferDetails.Rows.Count; i++)
                {
                    DataRow dr = StockTransferDetails.Rows[i];
                    //Now go for the Detail Inserts
                    Global.m_db.ClearParameter();
                    Global.m_db.setCommandType(CommandType.StoredProcedure);
                    Global.m_db.setCommandText("Inv.spStockTransferDetailCreate");
                    Global.m_db.AddParameter("@StockTransferID", SqlDbType.Int, StockTransferID.ToString());
                    Global.m_db.AddParameter("@Code", SqlDbType.NVarChar, 30, dr["Code"].ToString());
                    Global.m_db.AddParameter("@ProductName", SqlDbType.NVarChar, 30, dr["Product"].ToString());//Set same for both for time being
                    Global.m_db.AddParameter("@Language", SqlDbType.NVarChar, 30, Language);
                    Global.m_db.AddParameter("@Quantity", SqlDbType.Int, Convert.ToInt32(dr["Quantity"]));
                    Global.m_db.AddParameter("@PurchaseRate", SqlDbType.Money, Convert.ToDouble(dr["PurchaseRate"]));
                    Global.m_db.AddParameter("@Amount", SqlDbType.Money, Convert.ToDouble(dr["Amount"]));
                    System.Data.SqlClient.SqlParameter paramReturn = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                    Global.m_db.ProcessParameter();
                    if (paramReturn.Value.ToString() != "SUCCESS")
                    {
                        Global.m_db.RollBackTransaction();
                        throw new Exception("Unable to  perform Stock Transfer");
                    }
                    if (paramReturn.Value.ToString() != "SUCCESS")
                    {
                        Global.m_db.RollBackTransaction();
                        throw new Exception("Unable to perform Stock Transfer");
                    }

                    #region This code will use when inventory system is implemented
                    ////Addition in Inv.tblInventoryTrans 

                    //Also insert the transaction in Inv.tblInventoryTrnas  
                    Global.m_db.ClearParameter();
                    Global.m_db.setCommandType(CommandType.StoredProcedure);
                    Global.m_db.setCommandText("Inv.spInventoryTransCreate");
                    Global.m_db.AddParameter("@Date", SqlDbType.DateTime, StockTransferDate);
                    Global.m_db.AddParameter("@ProductName", SqlDbType.NVarChar, 30, dr["Product"].ToString());//Set same for both for time being
                    Global.m_db.AddParameter("@DepotID", SqlDbType.Int, ToDepotID);
                    Global.m_db.AddParameter("@Language", SqlDbType.NVarChar, 30, Language);
                    Global.m_db.AddParameter("@Quantity", SqlDbType.Int, Convert.ToInt32(dr["Quantity"]));
                    Global.m_db.AddParameter("@InOut", SqlDbType.NVarChar, 10, "INCOMING");
                    Global.m_db.AddParameter("@VoucherType", SqlDbType.NVarChar, 20, "STOCK_TRANS");
                    Global.m_db.AddParameter("@RowID", SqlDbType.Int, StockTransferID);
                    System.Data.SqlClient.SqlParameter ParamStockInInventory = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                    Global.m_db.ProcessParameter();
                    #endregion

                    #region This code will use when inventory system is implemented
                    ////Addition in Inv.tblInventoryTrans 

                    //Also insert the transaction in Inv.tblInventoryTrnas  
                    Global.m_db.ClearParameter();
                    Global.m_db.setCommandType(CommandType.StoredProcedure);
                    Global.m_db.setCommandText("Inv.spInventoryTransCreate");
                    Global.m_db.AddParameter("@Date", SqlDbType.DateTime, StockTransferDate);
                    Global.m_db.AddParameter("@ProductName", SqlDbType.NVarChar, 30, dr["Product"].ToString());//Set same for both for time being
                    Global.m_db.AddParameter("@DepotID", SqlDbType.Int, FromDepotID);
                    Global.m_db.AddParameter("@Language", SqlDbType.NVarChar, 30, Language);
                    Global.m_db.AddParameter("@Quantity", SqlDbType.Int, Convert.ToInt32(dr["Quantity"]));
                    Global.m_db.AddParameter("@InOut", SqlDbType.NVarChar, 10, "OUTGOING");
                    Global.m_db.AddParameter("@VoucherType", SqlDbType.NVarChar, 20, "STOCK_TRANS");
                    Global.m_db.AddParameter("@RowID", SqlDbType.Int, StockTransferID);
                    System.Data.SqlClient.SqlParameter ParamStockOutInventory = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                    Global.m_db.ProcessParameter();
                    #endregion      
                }


                foreach (int _AccClassID in AccClassID)
                {
                    Global.m_db.ClearParameter();
                    Global.m_db.setCommandType(CommandType.StoredProcedure);
                    Global.m_db.setCommandText("Inv.spStockTransferAccClassCreate");
                    Global.m_db.AddParameter("@StockTransferID", SqlDbType.Int, StockTransferID.ToString());
                    Global.m_db.AddParameter("@AccClassID", SqlDbType.Int, _AccClassID);//Set same for both for time being
                    System.Data.SqlClient.SqlParameter paramTransactClassID = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                    Global.m_db.ProcessParameter();

                    if (paramTransactClassID.Value.ToString() == "FAILURE")
                    {
                        Global.m_db.RollBackTransaction();
                        throw new Exception("Unable to create Purchase Order");
                    }
                }



                Global.m_db.CommitTransaction();
            }
            catch (Exception ex)
            {
                Global.m_db.RollBackTransaction();

                throw ex;


            }
        }

        public void Delete(int StockTransferID)
        {

            //First delete the previous records of Acc.tblStockTransferAccClass according to VoucherType and RowID
            Global.m_db.InsertUpdateQry("DELETE FROM inv.tblStockTransferAccClass WHERE   StockTransferID=" + Convert.ToInt32(StockTransferID.ToString()));

            //delete from tblstocktransfermaster
            Global.m_db.InsertUpdateQry("DELETE FROM Inv.tblStockTransferMaster WHERE StockTransferID=" +Convert.ToInt32(StockTransferID.ToString()) );
            
               
            //Delete from tblstocktransferdetails
            Global.m_db.InsertUpdateQry("DELETE FROM Inv.tblStockTransferDetails WHERE StockTransferID='" + StockTransferID.ToString() + "'");

            
               
            //First delete the old transaction
            Global.m_db.InsertUpdateQry("DELETE FROM inv.tblInventoryTrans WHERE VoucherType='STOCK_TRANS' AND RowID='" + StockTransferID.ToString() + "'");

           

            //delete from tblstocktransfermaster
            Global.m_db.InsertUpdateQry("DELETE FROM Inv.tblStockTransferMaster WHERE StockTransferID=" + Convert.ToInt32(StockTransferID.ToString()));
            

        }

        public DataTable NavigateStockTransferMaster(int CurrentID, Navigate NavTo)
        {
            Global.m_db.ClearParameter();
            Global.m_db.setCommandType(CommandType.StoredProcedure);
            Global.m_db.setCommandText("Inv.spStockTransferNavigate");
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

            DataTable dtStockTransferMst = Global.m_db.GetDataTable();
            return dtStockTransferMst;
        }

        public DataTable GetStockTransferDetails(int MasterID)
        {
            Global.m_db.ClearParameter();
            Global.m_db.setCommandType(CommandType.StoredProcedure);
            Global.m_db.setCommandText("Inv.spGetStockTransferDetail");
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
            DataTable dtStockTransferDtl = Global.m_db.GetDataTable();
            return dtStockTransferDtl;
        }

        public DataTable GetStockTransferMasterInfo(int StockTransferID)
        {

            return Global.m_db.SelectQry("SELECT * FROM Acc.tblStockTransferMaster WHERE StockTransferID ='" + StockTransferID + "'", "StockTransfer");

        }

        public int GetSeriesIDFromMasterID(int MasterID)
        {

            object returnID;
            returnID = Global.m_db.GetScalarValue("SELECT SeriesID FROM Acc.tblStockTransferMaster WHERE ContraID ='" + MasterID + "'");

            return Convert.ToInt32(returnID);


        }

    }
}
