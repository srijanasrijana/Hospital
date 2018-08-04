using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;
using System.Windows.Forms;
using DateManager;

namespace BusinessLogic
{

    public enum SlabType
    {
        None,
        TAX1,
        TAX2,
        TAX3,
        VAT,
        CUSTOMDUTY,
        PURCHTAX

    }

    public enum SalesPurchaseType
    {
        SALES,
        PURCHASE
    }

    public class Slabs
    {
        public static DataTable GetSlabInfo(SlabType SlabCode)
        {
            string SlabString = "";
            switch (SlabCode)
            {
                case SlabType.VAT:
                    SlabString = "VAT";
                    break;
                case SlabType.TAX1:
                    SlabString = "TAX1";
                    break;
                case SlabType.TAX2:
                    SlabString = "TAX2";
                    break;
                case SlabType.TAX3:
                    SlabString = "TAX3";
                    break;
                case SlabType.CUSTOMDUTY:
                    SlabString = "CUSTOMDUTY";
                    break;
                case SlabType.PURCHTAX:
                    SlabString = "PURCHTAX";
                    break;
            }


            if (SlabCode == SlabType.None)
            {
                return Global.m_db.SelectQry("SELECT * FROM System.tblSlabs", "table");
            }
            else
            {
                return Global.m_db.SelectQry("SELECT * FROM System.tblSlabs WHERE Code ='" + SlabString + "'", "table");
            }
        }


        /// <summary>
        /// Converts SlabType into SlabCode to be queried with Database SlabCode
        /// </summary>
        /// <param name="m_SlabType"></param>
        /// <returns></returns>
        public string SlabTypeToSlabCode(SlabType m_SlabType)
        {
            switch (m_SlabType)
            {
                case SlabType.TAX1:
                    return "TAX1";
                case SlabType.TAX2:
                    return "TAX2";
                case SlabType.TAX3:
                    return "TAX3";
                case SlabType.VAT:
                    return "VAT";
                case SlabType.CUSTOMDUTY:
                    return "CUSTOMDUTY";

                case SlabType.PURCHTAX:
                    return "PURCHTAX";
                case SlabType.None:
                    return "";
            }
            return "";
        }

        public void Modify( SlabType SlabCode,string SlabName, double Rate, string Description,string oldslab,string newslab)
        {
            try
            {
                Global.m_db.ClearParameter();
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("System.spSlabsModify");


                Global.m_db.AddParameter("@SlabCode", SqlDbType.NVarChar, 50, SlabTypeToSlabCode(SlabCode));
                Global.m_db.AddParameter("@SlabName", SqlDbType.NVarChar, 50, SlabName);
                Global.m_db.AddParameter("@Rate", SqlDbType.Money, Rate);//Set same for both for time being           
                Global.m_db.AddParameter("@Description", SqlDbType.NVarChar, 200, Description);
                Global.m_db.AddParameter("@Modified_By", SqlDbType.NVarChar, 50, User.CurrUserID.ToString());
                System.Data.SqlClient.SqlParameter objReturn = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                Global.m_db.ProcessParameter();

                try
                {
                  
                        string username = User.CurrentUserName;
                        string voucherdate = Date.ToDB(DateTime.Now).ToString();
                        string VoucherType = "SLAB";
                        string action = "INSERT/UPDATE";
                        int rowid = 0;
                        string ComputerName = Global.ComputerName;
                        string MacAddress = Global.MacAddess;
                        string IpAddress = Global.IpAddress;
                        string desc = oldslab + newslab;

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
                catch (Exception ex)
                {
                    Global.MsgError(ex.Message);
                }
            }
            catch (Exception ex)
            {
                
                throw;
            }
        }

        public  static double CalculateSlab(double SlabRate,double Amount)
        {
            return((SlabRate*Amount)/100);
                                                    
        }

        public static double CalculateSlabsDetails(SlabType Slab,SalesPurchaseType SALES_PURCH, double NetAmount, double GrossAmount)
        {
            //read the dr[code]

            double rtnSlab = 0;
            switch (Slab)
            {
                case SlabType.TAX1:
                    //for SALES
                    if (SALES_PURCH == SalesPurchaseType.SALES)
                    {
                        if (Settings.GetSettings("DEFAULT_SALES_TAX1") == "Nt Amt")
                        {
                            rtnSlab = CalculateSlab(Global.Default_Tax1, NetAmount);
                            return rtnSlab;
                        }
                        else if (Settings.GetSettings("DEFAULT_SALES_TAX1") == "Gross")
                        {
                            rtnSlab = CalculateSlab(Global.Default_Tax1, GrossAmount);
                            return rtnSlab;
                        }
                    }                                           
                 //For purchase
                    if (SALES_PURCH == SalesPurchaseType.PURCHASE)
                    {                      
                            if (Settings.GetSettings("DEFAULT_PURCHASE_TAX1")=="Nt Amt")
                            {
                                rtnSlab = CalculateSlab(Global.Default_Tax1, NetAmount);
                                return rtnSlab;                                                              
                            }
                            else if (Settings.GetSettings("DEFAULT_PURCHASE_TAX1") == "Gross")
                            {
                                rtnSlab = CalculateSlab(Global.Default_Tax1, GrossAmount);
                                return rtnSlab;
                            }                     
                    }
                    break;

                case SlabType.TAX2:

                    //for sales
                    if (SALES_PURCH == SalesPurchaseType.SALES)
                    {

                        if (Settings.GetSettings("DEFAULT_SALES_TAX2") == "Nt Amt")
                        {
                            rtnSlab = CalculateSlab(Global.Default_Tax2, NetAmount);
                            return rtnSlab;
                        }
                        else if (Settings.GetSettings("DEFAULT_SALES_TAX2") == "Gross")
                        {
                            rtnSlab = CalculateSlab(Global.Default_Tax2, GrossAmount);
                            return rtnSlab;
                        }
                        else if (Settings.GetSettings("DEFAULT_SALES_TAX2") == "Tax 1")
                        {
                            rtnSlab = CalculateSlab((Global.Default_Tax2), CalculateSlabsDetails(SlabType.TAX1, SalesPurchaseType.SALES, NetAmount, GrossAmount));
                        }
                    }

                    if (SALES_PURCH == SalesPurchaseType.PURCHASE)
                    {
                       
                            if (Settings.GetSettings("DEFAULT_PURCHASE_TAX2")=="Nt Amt")
                            {
                                 rtnSlab = CalculateSlab(Global.Default_Tax2, NetAmount);
                                    return rtnSlab;                          
                            }
                            else if (Settings.GetSettings("DEFAULT_PURCHASE_TAX2") == "Gross")
                            {
                                rtnSlab = CalculateSlab(Global.Default_Tax2, GrossAmount);
                                return rtnSlab;
                            }
                            else if (Settings.GetSettings("DEFAULT_PURCHASE_TAX2") == "Tax 1")
                            {
                                rtnSlab = CalculateSlab((Global.Default_Tax2), CalculateSlabsDetails(SlabType.TAX1, SalesPurchaseType.PURCHASE, NetAmount, GrossAmount));
                            }
                        
                    }
                    break;
                case SlabType.TAX3:
                    //for sales
                    if (SALES_PURCH == SalesPurchaseType.SALES)
                    {
                      
                            if (Settings.GetSettings("DEFAULT_SALES_TAX3")=="Nt Amt")
                            {
                                 rtnSlab = CalculateSlab(Global.Default_Tax3, NetAmount);
                                    return rtnSlab;                                                    
                            }
                            else if (Settings.GetSettings("DEFAULT_SALES_TAX3") == "Gross")
                            {
                                rtnSlab = CalculateSlab(Global.Default_Tax3, GrossAmount);
                                return rtnSlab;
                            }
                            else if (Settings.GetSettings("DEFAULT_SALES_TAX3") == "Tax 1")
                            {
                                rtnSlab = CalculateSlab((Global.Default_Tax3), CalculateSlabsDetails(SlabType.TAX1, SalesPurchaseType.SALES, NetAmount, GrossAmount));
                            }
                            else if (Settings.GetSettings("DEFAULT_SALES_TAX3") == "Tax 2")
                            {
                                rtnSlab = CalculateSlab((Global.Default_Tax3), CalculateSlabsDetails(SlabType.TAX2, SalesPurchaseType.SALES, NetAmount, GrossAmount));

                            }
                    }
                    //for purchase
                    if (SALES_PURCH == SalesPurchaseType.PURCHASE)
                    {

                            if(Settings.GetSettings("DEFAULT_PURCHASE_TAX3")=="Nt Amt")
                            {
                                rtnSlab = CalculateSlab(Global.Default_Tax3, NetAmount);
                                return rtnSlab;                          
                            }
                            else if (Settings.GetSettings("DEFAULT_PURCHASE_TAX3") == "Gross")
                            {
                                rtnSlab = CalculateSlab(Global.Default_Tax3, GrossAmount);
                                return rtnSlab;
                            }
                            else if (Settings.GetSettings("DEFAULT_PURCHASE_TAX3") == "Tax 1")
                            {
                                rtnSlab = CalculateSlab((Global.Default_Tax3), CalculateSlabsDetails(SlabType.TAX1, SalesPurchaseType.PURCHASE, NetAmount, GrossAmount));
                            }
                            else if (Settings.GetSettings("DEFAULT_PURCHASE_TAX3") == "Tax 2")
                            {
                                rtnSlab = CalculateSlab((Global.Default_Tax3), CalculateSlabsDetails(SlabType.TAX2, SalesPurchaseType.PURCHASE, NetAmount, GrossAmount));

                            }
                    }
                    break;

           }
            return rtnSlab;
            
        }
    }
}
