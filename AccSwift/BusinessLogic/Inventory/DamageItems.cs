using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;
using System.Windows.Forms;
using DateManager;

namespace BusinessLogic.Inventory
{
    public class DamageItems
    {
        public void Create(DataTable DamageInvoiceDetail,int seriesid,int depotid,int projectid,string voucher_No,DateTime invoicedate,int[] AccClassID,OptionalField of)
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

            try
            {
                Global.m_db.BeginTransaction();
                Global.m_db.ClearParameter();
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("Inv.spDamageInvoiceCreate");
                Global.m_db.AddParameter("@SeriesID", SqlDbType.Int, seriesid);
                Global.m_db.AddParameter("@ProjectID", SqlDbType.Int, projectid);
                Global.m_db.AddParameter("@DepotID", SqlDbType.Int, depotid);
                Global.m_db.AddParameter("@Voucher_No", SqlDbType.NVarChar, 30, voucher_No);//Set same for both for time being
                Global.m_db.AddParameter("@DamageInvoice_Date", SqlDbType.DateTime, invoicedate);
                Global.m_db.AddParameter("@Created_By", SqlDbType.Int, User.CurrUserID);
                Global.m_db.AddParameter("@First", SqlDbType.NVarChar, 50, of.First);
                Global.m_db.AddParameter("@Second", SqlDbType.NVarChar, 50, of.Second);
                Global.m_db.AddParameter("@Third", SqlDbType.NVarChar, 50, of.Third);
                Global.m_db.AddParameter("@Fourth", SqlDbType.NVarChar, 50, of.Fourth);
                Global.m_db.AddParameter("@Fifth", SqlDbType.NVarChar, 50, of.Fifth);
                System.Data.SqlClient.SqlParameter objReturn = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                Global.m_db.ProcessParameter();
                int ReturnID = Convert.ToInt32(objReturn.Value);


                for (int i = 0; i < DamageInvoiceDetail.Rows.Count; i++)
                {
                    DataRow dr = DamageInvoiceDetail.Rows[i];
                    //Now go for the Detail Inserts
                    Global.m_db.ClearParameter();
                    Global.m_db.setCommandType(CommandType.StoredProcedure);
                    Global.m_db.setCommandText("Inv.spDamageInvoiceDetailCreate");
                    Global.m_db.AddParameter("@DamageInvoiceID", SqlDbType.Int, ReturnID.ToString());
                    Global.m_db.AddParameter("@Code", SqlDbType.NVarChar, 30, dr["ProductCode"].ToString());
                    Global.m_db.AddParameter("@ProductName", SqlDbType.NVarChar, 30, dr["ProductName"].ToString());//Set same for both for time being
                    Global.m_db.AddParameter("@Language", SqlDbType.NVarChar, 30, Language);
                    Global.m_db.AddParameter("@Quantity", SqlDbType.Float, Convert.ToDouble(dr["Quantity"].ToString()));
                    Global.m_db.AddParameter("@PurchaseRate", SqlDbType.Money, Convert.ToDouble(dr["PurchaseRate"]));
                    Global.m_db.AddParameter("@Amount", SqlDbType.Money, Convert.ToDouble(dr["Amount"]));
                    Global.m_db.AddParameter("@ProductID", SqlDbType.Int, Convert.ToInt32(dr["ProductID"].ToString()));

                    System.Data.SqlClient.SqlParameter paramReturn = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                    Global.m_db.ProcessParameter();
                    if (paramReturn.Value.ToString() != "SUCCESS")
                    {
                        Global.m_db.RollBackTransaction();
                        throw new Exception("Unable to create Damage Product Invoice");
                    }


                    #region This code will use when inventory system is implemented
                    ////Addition in Inv.tblInventoryTrans 

                    //Also insert the transaction in Inv.tblInventoryTrnas  
                    Global.m_db.ClearParameter();
                    Global.m_db.setCommandType(CommandType.StoredProcedure);
                    Global.m_db.setCommandText("Inv.spInventoryTransCreate");
                    Global.m_db.AddParameter("@Date", SqlDbType.DateTime, invoicedate);
                    Global.m_db.AddParameter("@ProductID", SqlDbType.NVarChar, 30, dr["ProductID"].ToString());//Set same for both for time being
                    //Global.m_db.AddParameter("@ProductName", SqlDbType.NVarChar, 30, dr["ProductName"].ToString());//Set same for both for time being
                    Global.m_db.AddParameter("@DepotID", SqlDbType.Int, depotid);
                    Global.m_db.AddParameter("@Language", SqlDbType.NVarChar, 30, Language);
                    Global.m_db.AddParameter("@Quantity", SqlDbType.Float, Convert.ToDouble(dr["Quantity"]));
                    Global.m_db.AddParameter("@InOut", SqlDbType.NVarChar, 10, "DAMAGE");
                    Global.m_db.AddParameter("@VoucherType", SqlDbType.NVarChar, 20, "DAMAGE");
                    Global.m_db.AddParameter("@RowID", SqlDbType.Int, ReturnID);
                    System.Data.SqlClient.SqlParameter ParamReturnInventory = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                    Global.m_db.ProcessParameter();
                    int id = Convert.ToInt32(ParamReturnInventory.Value);
                    #endregion

                    foreach (int _AccClassID in AccClassID)
                    {
                        Global.m_db.ClearParameter();
                        Global.m_db.setCommandType(CommandType.StoredProcedure);
                        Global.m_db.setCommandText("Inv.spTransactClassCreate");
                        Global.m_db.AddParameter("@InvTransactID", SqlDbType.Int, id.ToString());
                        Global.m_db.AddParameter("@AccClassID", SqlDbType.Int, _AccClassID);//Set same for both for time being
                        System.Data.SqlClient.SqlParameter paramTransactClassID = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                        Global.m_db.ProcessParameter();

                        if (paramTransactClassID.Value.ToString() == "FAILURE")
                        {
                            Global.m_db.RollBackTransaction();
                            throw new Exception("Unable to create Purchase Invoice");
                        }
                    }
                    Global.m_db.CommitTransaction();
                }
            }
            catch (Exception ex)
            {
                Global.m_db.RollBackTransaction();
                MessageBox.Show(ex.Message); ;
            }
        
        }

        public void EditDefectITems(int DamageProductID, DataTable DamageInvoiceDetail, int seriesid, int depotid, int projectid, string voucher_No, DateTime invoicedate, int[] AccClassID,OptionalField of)
        {
            try
            {
                Global.m_db.BeginTransaction();
                Global.m_db.ClearParameter();
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("Inv.spDamageInvoiceModify");
                Global.m_db.AddParameter("@DamageProductID", SqlDbType.Int, DamageProductID);
                Global.m_db.AddParameter("@SeriesID", SqlDbType.Int, seriesid);
                Global.m_db.AddParameter("@ProjectID", SqlDbType.Int, projectid);
                Global.m_db.AddParameter("@DepotID", SqlDbType.Int, depotid);
                Global.m_db.AddParameter("@Voucher_No", SqlDbType.NVarChar, 30, voucher_No);//Set same for both for time being
                Global.m_db.AddParameter("@DamageInvoiceDate", SqlDbType.DateTime, invoicedate);
                Global.m_db.AddParameter("@Modified_By", SqlDbType.NVarChar, 50, User.CurrUserID.ToString());
                Global.m_db.AddParameter("@First", SqlDbType.NVarChar, 50, of.First);
                Global.m_db.AddParameter("@Second", SqlDbType.NVarChar, 50, of.Second);
                Global.m_db.AddParameter("@Third", SqlDbType.NVarChar, 50, of.Third);
                Global.m_db.AddParameter("@Fourth", SqlDbType.NVarChar, 50, of.Fourth);
                Global.m_db.AddParameter("@Fifth", SqlDbType.NVarChar, 50, of.Fifth);
                System.Data.SqlClient.SqlParameter objReturn = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                Global.m_db.ProcessParameter();

                Global.m_db.InsertUpdateQry("DELETE FROM Inv.tbldamageproductinvoicedetail WHERE DamageProductID='" + DamageProductID.ToString() + "'");
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

                for (int i = 0; i < DamageInvoiceDetail.Rows.Count; i++)
                {
                    DataRow dr = DamageInvoiceDetail.Rows[i];
                    //Now go for the Detail Inserts
                    Global.m_db.ClearParameter();
                    Global.m_db.setCommandType(CommandType.StoredProcedure);
                    Global.m_db.setCommandText("Inv.spDamageInvoiceDetailCreate");
                    Global.m_db.AddParameter("@DamageInvoiceID", SqlDbType.Int, DamageProductID.ToString());
                    Global.m_db.AddParameter("@Code", SqlDbType.NVarChar, 30, dr["ProductCode"].ToString());
                    Global.m_db.AddParameter("@ProductName", SqlDbType.NVarChar, 30, dr["ProductName"].ToString());//Set same for both for time being
                    Global.m_db.AddParameter("@Language", SqlDbType.NVarChar, 30, Language);
                    Global.m_db.AddParameter("@Quantity", SqlDbType.Float, Convert.ToDouble(dr["Quantity"].ToString()));
                    Global.m_db.AddParameter("@PurchaseRate", SqlDbType.Money, Convert.ToDouble(dr["PurchaseRate"]));
                    Global.m_db.AddParameter("@Amount", SqlDbType.Money, Convert.ToDouble(dr["Amount"]));
                    //Global.m_db.AddParameter("@ProductID", SqlDbType.Int, Convert.ToInt32(dr["ProductID"].ToString()));

                    System.Data.SqlClient.SqlParameter paramReturn = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                    Global.m_db.ProcessParameter();

                    if (paramReturn.Value.ToString() != "SUCCESS")
                    {
                        Global.m_db.RollBackTransaction();
                        throw new Exception("Unable to create Damage Product Invoice");
                    }
                    #region This code will use when inventory system is implemented
                    ////Addition in Inv.tblInventoryTrans 

                    //Also insert the transaction in Inv.tblInventoryTrnas  
                    Global.m_db.ClearParameter();
                    Global.m_db.setCommandType(CommandType.StoredProcedure);
                    Global.m_db.setCommandText("Inv.spInventoryTransCreate");
                    Global.m_db.AddParameter("@Date", SqlDbType.DateTime, invoicedate);
                   // Global.m_db.AddParameter("@ProductName", SqlDbType.NVarChar, 30, dr["ProductName"].ToString());//Set same for both for time being
                    Global.m_db.AddParameter("@ProductID", SqlDbType.NVarChar, 30, dr["ProductID"].ToString());//Set same for both for time being
                    Global.m_db.AddParameter("@DepotID", SqlDbType.Int, depotid);
                    Global.m_db.AddParameter("@Language", SqlDbType.NVarChar, 30, Language);
                    Global.m_db.AddParameter("@Quantity", SqlDbType.Float, Convert.ToDouble(dr["Quantity"]));
                    Global.m_db.AddParameter("@InOut", SqlDbType.NVarChar, 10, "DAMAGE");
                    Global.m_db.AddParameter("@VoucherType", SqlDbType.NVarChar, 20, "DAMAGE");
                    Global.m_db.AddParameter("@RowID", SqlDbType.Int, DamageProductID);
                    System.Data.SqlClient.SqlParameter ParamReturnInventory = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                    Global.m_db.ProcessParameter();
                    int id = Convert.ToInt32(ParamReturnInventory.Value);
                    #endregion

                    foreach (int _AccClassID in AccClassID)
                    {
                        Global.m_db.ClearParameter();
                        Global.m_db.setCommandType(CommandType.StoredProcedure);
                        Global.m_db.setCommandText("Inv.spTransactClassCreate");
                        Global.m_db.AddParameter("@InvTransactID", SqlDbType.Int, id.ToString());
                        Global.m_db.AddParameter("@AccClassID", SqlDbType.Int, _AccClassID);//Set same for both for time being
                        System.Data.SqlClient.SqlParameter paramTransactClassID = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                        Global.m_db.ProcessParameter();

                        if (paramTransactClassID.Value.ToString() == "FAILURE")
                        {
                            Global.m_db.RollBackTransaction();
                            throw new Exception("Unable to create Purchase Invoice");
                        }
                    }
                    Global.m_db.CommitTransaction();
                }

               
            }
            catch (Exception ex)
            {
                Global.m_db.RollBackTransaction();
                MessageBox.Show(ex.Message);
            }
        }
        public DataTable GetDamageItemsDetails(int MasterID)
        {
            Global.m_db.ClearParameter();
            Global.m_db.setCommandType(CommandType.StoredProcedure);
            Global.m_db.setCommandText("Inv.spGetDamageItemDetail");
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
            DataTable dtContraDtl = Global.m_db.GetDataTable();
            return dtContraDtl;
        }
        public int GetSeriesIDFromMasterID(int MasterID)
        {
            object returnID;
            returnID = Global.m_db.GetScalarValue("SELECT SeriesID FROM Inv.tblDamageProductInvoiceMaster WHERE DamageProductID ='" + MasterID + "'");
            return Convert.ToInt32(returnID);
        }
        public DataTable GetDamageItemMasterInfo(string RowID)
        {
            return Global.m_db.SelectQry("SELECT * FROM Inv.tblDamageProductInvoiceMaster WHERE DamageProductID ='" + RowID + "'", "table");
        }

        public DataTable NavigateDamageInvoiceMaster(int CurrentID, Navigate NavTo)
        {
            Global.m_db.ClearParameter();
            Global.m_db.setCommandType(CommandType.StoredProcedure);
            Global.m_db.setCommandText("Inv.spDamageInvoiceNavigate");
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

            DataTable dtDamageInvoiceMst = Global.m_db.GetDataTable();
            return dtDamageInvoiceMst;
        }

        public DataTable GetDamageInvoiceDetails(int MasterID)
        {
            Global.m_db.ClearParameter();
            Global.m_db.setCommandType(CommandType.StoredProcedure);
            Global.m_db.setCommandText("Inv.spGetDamageInvoiceDetail");
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
            DataTable dtDamageinvoicedetail = Global.m_db.GetDataTable();
            return dtDamageinvoicedetail;
        }


        public bool RemoveDamageItemsEntry(int DamageProductID)
        {
            try
            {
                Global.m_db.ClearParameter();
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("Inv.spRemoveDamageProdcutInvoiceEntry");
                Global.m_db.AddParameter("@DamageProductInvoiceID", SqlDbType.Int, DamageProductID);
                //system.Data.SqlClient.SqlParameter objReturn = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                Global.m_db.ProcessParameter();
                return true;
            }
            catch
            {
                return false;
            }
        }
        
    }
}
