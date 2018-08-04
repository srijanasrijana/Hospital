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
   public  class Services
    {
       public void Create(string ServiceCode, string ServiceName,string ParentServiceName,string Description,double SalesRate,double PurchaseRate,string oldservice,string newservice,bool isnew)
       {
           try
           {
               // Global.m_db.InsertUpdateQry("INSERT INTO Account.tblAccountHead(AccountHeadName,Under_Account_ID,Description,BuiltIn) VALUES ('" + AccountHeadName + "','1','" + Description + "','0')");
               Global.m_db.ClearParameter();
               Global.m_db.setCommandType(CommandType.StoredProcedure);
               Global.m_db.setCommandText("Acc.spServicesCreate");
               Global.m_db.AddParameter("@Code", SqlDbType.NVarChar, 50, ServiceCode);
               Global.m_db.AddParameter("@EngName", SqlDbType.NVarChar, 50, ServiceName);
               Global.m_db.AddParameter("@NepName", SqlDbType.NVarChar, 50, ServiceName);//Set same for both for time being
               if(ParentServiceName!="")
               {
                   Global.m_db.AddParameter("@Under_EngName", SqlDbType.NVarChar, 50, ParentServiceName);
               }
               Global.m_db.AddParameter("@Description", SqlDbType.NVarChar, 200, Description);
               Global.m_db.AddParameter("@SalesRate", SqlDbType.Money, 50, SalesRate);
               Global.m_db.AddParameter("@PurchaseRate", SqlDbType.Money, 50, PurchaseRate);
               object objReturn = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
               Global.m_db.ProcessParameter();

               try
               {
                   if (isnew == true)
                   {
                       string username = User.CurrentUserName;
                       string voucherdate = Date.ToDB(DateTime.Now).ToString();
                       string VoucherType = "SERVICE";
                       string action = "INSERT";
                       int rowid = 0;
                       string ComputerName = Global.ComputerName;
                       string MacAddress = Global.MacAddess;
                       string IpAddress = Global.IpAddress;
                       string desc = oldservice + newservice;

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
                   else if (isnew == false)
                   {
                       string username = User.CurrentUserName;
                       string voucherdate = Date.ToDB(DateTime.Now).ToString();
                       string VoucherType = "SERVICE";
                       string action = "UPDATE";
                       int rowid = 0;
                       string ComputerName = Global.ComputerName;
                       string MacAddress = Global.MacAddess;
                       string IpAddress = Global.IpAddress;
                       string desc = oldservice + newservice;

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
               }
               catch (Exception ex)
               {
                   Global.MsgError(ex.Message);
               }
              
               //Also insert into tblLedger
           }
           catch (Exception)
           {
               throw;

               #region SQLException
               //switch (ex.Number)
               //{
               //    case 4060: // Invalid Database 
               //        Global.Msg("Invalid Database", MBType.Error, "Error");
               //        break;

               //    case 18456: // Login Failed 
               //        Global.Msg("Login Failed!", MBType.Error, "Error");
               //        break;

               //    case 547: // ForeignKey Violation , Check Constraint
               //        Global.Msg("Invalid parent group! Check the parent group and try again!", MBType.Warning, "Error");
               //        break;

               //    case 2627: // Unique Index/ Primary key Violation/ Constriant Violation 
               //        Global.Msg("The group name already exists! Please choose another group names!", MBType.Warning, "Error");
               //        break;

               //    case 2601: // Unique Index/Constriant Violation 
               //        Global.Msg("Unique index violation!", MBType.Warning, "Error");
               //        break;

               //    case 5000: //Trigger violation
               //        Global.Msg("Trigger violation!", MBType.Warning, "Error");
               //        break;

               //    default:
               //        break;
               //}
               #endregion
           }
       }

       public void Modify(int ServicesID, string Code, string NewServicesName, string ParentServicesName, string Description, double SalesRate, double PurchaseRate, string oldservice, string newservice, bool isnew)
       {
           //Find which language is there
           string CurrLang;
           switch (LangMgr.DefaultLanguage)
           {
               case Lang.English:
                   CurrLang = "ENGLISH";
                   break;
               case Lang.Nepali:
                   CurrLang = "NEPALI";
                   break;
               default:
                   CurrLang = "ENGLISH";
                   break;
           }
           try
           {
               Global.m_db.ClearParameter();
               Global.m_db.setCommandType(CommandType.StoredProcedure);
               Global.m_db.setCommandText("Acc.spServicesModify");
               Global.m_db.AddParameter("@OldID", SqlDbType.Int, ServicesID);
               Global.m_db.AddParameter("@Code", SqlDbType.NVarChar, 50, Code);
               Global.m_db.AddParameter("@NewServicesName", SqlDbType.NVarChar, 50, NewServicesName);
               Global.m_db.AddParameter("@ParentServices", SqlDbType.NVarChar, 50, ParentServicesName);
               Global.m_db.AddParameter("@Lang", SqlDbType.NVarChar, 20, "ENGLISH");
               Global.m_db.AddParameter("@Description", SqlDbType.NVarChar, 200, Description);
               Global.m_db.AddParameter("@SalesRate", SqlDbType.Money, 20, SalesRate);
               Global.m_db.AddParameter("@PurchaseRate", SqlDbType.Money, 20, PurchaseRate);
               object objReturn = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
               Global.m_db.ProcessParameter();

               try
               {
                   if (isnew == true)
                   {
                       string username = User.CurrentUserName;
                       string voucherdate = Date.ToDB(DateTime.Now).ToString();
                       string VoucherType = "SERVICE";
                       string action = "INSERT";
                       int rowid = 0;
                       string ComputerName = Global.ComputerName;
                       string MacAddress = Global.MacAddess;
                       string IpAddress = Global.IpAddress;
                       string desc = oldservice + newservice;

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
                   else if (isnew == false)
                   {
                       string username = User.CurrentUserName;
                       string voucherdate = Date.ToDB(DateTime.Now).ToString();
                       string VoucherType = "SERVICE";
                       string action = "UPDATE";
                       int rowid = 0;
                       string ComputerName = Global.ComputerName;
                       string MacAddress = Global.MacAddess;
                       string IpAddress = Global.IpAddress;
                       string desc = oldservice + newservice;

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
               }
               catch (Exception ex)
               {
                   Global.MsgError(ex.Message);
               }
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }

       public void Delete(string ServicesName)
       {
           //Language Management
           string LangField = "EngName";
           if (LangMgr.DefaultLanguage == Lang.English)
               LangField = "EngName";
           else if (LangMgr.DefaultLanguage == Lang.Nepali)
               LangField = "NepName";

           Global.m_db.SelectQry("DELETE FROM Acc.tblServices WHERE " + LangField + "='" + ServicesName + "'", "Acc.tblServices");
           Global.m_db.SelectQry("DELETE FROM Acc.tblLedger WHERE " + LangField + " ='"+ ServicesName +"'","Acc.tblLedger");
       }

       /// <summary>
       /// Gets all the Group informations under the given parent ID. If parent ID is 0 it returns only parent group information.
       /// If you want to get all groups, introduce ParentGroupID as -1
       /// </summary>
       /// <param name="ParentGroupID"></param>
       /// <returns></returns>
       public static DataTable GetServicesTable(int ParentServicesID)
       {
           if (ParentServicesID == 0)//Only Parent group
           {
               return Global.m_db.SelectQry("SELECT * FROM Acc.tblServices WHERE ParentServicesID is null", "tblServices");
           }
           else if (ParentServicesID == -1)//All Groups
           {
               return Global.m_db.SelectQry("SELECT * FROM Acc.tblServices", "tblServices");
           }
           else
           {
               return Global.m_db.SelectQry("SELECT * FROM Acc.tblServices WHERE ParentServicesID =" + ParentServicesID.ToString(), "tblServices");
           }
       }

       public static int GetIDFromName(string Name, Lang Language)
       {
           string LangField = "EngName";
           switch (Language)
           {
               case Lang.English:
                   LangField = "EngName";
                   break;
               case Lang.Nepali:
                   LangField = "NepName";
                   break;
           }
           object objResult = Global.m_db.GetScalarValue("SELECT ServicesID FROM Acc.tblServices WHERE " + LangField + "='" + Name + "'");
           return Convert.ToInt32(objResult);
       }

       public static DataTable GetServicesByID(int ServicesID, Lang Language)
       {
           string LangField = "EngName";
           if (Language == Lang.English)
               LangField = "EngName";
           else if (Language == Lang.Nepali)
               LangField = "NepName";

           return Global.m_db.SelectQry("SELECT a.ServicesID ID,a.Code Code, a." + LangField + " Name, b." + LangField + " Parent, a.SalesRate SalesRate,a.PurchaseRate,a.Description Description FROM Acc.tblServices a LEFT OUTER JOIN Acc.tblServices b ON a.ParentServicesID=b.ServicesID WHERE a.ServicesID='" + ServicesID.ToString() + "'", "Search");
       }

       /// <summary>
       /// Returns the table of accounts which falls under given GroupID and fills the result in ReturnTable. Recursive function
       /// </summary>
       /// <param name="GroupID"></param>
       /// <param name="ReturnTable"></param>
       public static void GetServicesUnder(int ServicesID, ArrayList ReturnIDs)
       {
           #region Language Management

           //tv.Font = LangMgr.GetFont();

           string LangField = "EngName";
           switch (LangMgr.DefaultLanguage)
           {
               case Lang.English:
                   LangField = "EngName";
                   break;
               case Lang.Nepali:
                   LangField = "NepName";
                   break;

           }
           #endregion
           DataTable dt;
           dt = Services.GetServicesTable(ServicesID);
           for (int i = 0; i < dt.Rows.Count; i++)
           {
               DataRow dr = dt.Rows[i];
               ReturnIDs.Add(dr["ServicesID"]);
               GetServicesUnder(Convert.ToInt32(dr["ServicesID"].ToString()), ReturnIDs);
           }
       }

       public static DataTable GetServicesByName(string ProductName)
       {
           return Global.m_db.SelectQry("SELECT * FROM Acc.tblServices WHERE EngName = '" + ProductName + "'","tblServices");
       }

       public static DataTable GetServicesList(int ParentServiceID)
       {
           DataTable dt;
           string strQuery ="";
           if (ParentServiceID == 0)
           {
               strQuery = "SELECT ServicesID,ParentServicesID,Code,EngName,NepName,Description,SalesRate,PurchaseRate FROM Acc.tblServices";
               dt = Global.m_db.SelectQry(strQuery, "tbl");
           }
           else
           {
               strQuery = "SELECT ServicesID,ParentServicesID,Code,EngName,NepName,Description,SalesRate,PurchaseRate FROM Acc.tblServices WHERE ParentServicesID ='" + ParentServiceID + "'";
               dt = Global.m_db.SelectQry(strQuery, "tbl");
           }
           return dt;
       }
    }
}
