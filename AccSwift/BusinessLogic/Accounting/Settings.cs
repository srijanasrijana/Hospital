using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data;
using DBLogic;

namespace BusinessLogic
{
   public class Settings
    {
       public static string GetSettings(string Code)
       {
           try
           {
               object Value;
               Value = Global.m_db.GetScalarValue("SELECT Value from System.tblSettings WHERE Code='" + Code + "'");
               //if (Value == null)
               //{
               //    throw new Exception("Invalid settings code");
               //    return null;
               //}
               return Value.ToString();
           }
           catch (Exception ex)
           {
               throw ex;
           }

       }

       public void SetSettings(string Code, string Value)
       {
           try
           {
               string strQuery = "UPDATE System.tblSettings SET Value ='" + Value + "' WHERE Code ='" + Code + "'";
               Global.m_db.InsertUpdateQry(strQuery);
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }

       public static DataTable GetSettingTable()
       {
           DataTable dt = new DataTable();
           string strQuery = "SELECT Name,Code,Value FROM System.tblSettings";
           dt = Global.m_db.SelectQry(strQuery, "tblSettings");
           //if (dt.Rows.Count <= 0)
           //{
           //    throw new Exception("Invalid");
           //}
           return dt;
       }

       public static NegativeCash NegativeCashToNegativeCashType(string DefalultFormate)
       {
           NegativeCash ReturnNegativeCashType = NegativeCash.Warn;
           switch (DefalultFormate)
           {
               case "Allow":
                   ReturnNegativeCashType = NegativeCash.Allow; ;
                   break;
               case "Warn":
                   ReturnNegativeCashType = NegativeCash.Warn; ;
                   break;
               case "Deny":
                   ReturnNegativeCashType = NegativeCash.Deny; ;
                   break;
           }
           return ReturnNegativeCashType;
       }

       public static BudgetLimit BudgetLimitAction(string DefalultFormate)
       {
           BudgetLimit ReturnBudgetLimitType = BudgetLimit.Warn;
           switch (DefalultFormate)
           {
               case "Allow":
                   ReturnBudgetLimitType = BudgetLimit.Allow; ;
                   break;
               case "Warn":
                   ReturnBudgetLimitType = BudgetLimit.Warn; ;
                   break;
               case "Deny":
                   ReturnBudgetLimitType = BudgetLimit.Deny; ;
                   break;
           }
           return ReturnBudgetLimitType;
       }
       public static NegativeBank NegativeBankToNegativeBankType(string DefalultFormate)
       {
           NegativeBank ReturnNegativeBankType = NegativeBank.Warn;
           switch (DefalultFormate)
           {
               case "Allow":
                   ReturnNegativeBankType = NegativeBank.Allow; ;
                   break;
               case "Warn":
                   ReturnNegativeBankType = NegativeBank.Warn; ;
                   break;
               case "Deny":
                   ReturnNegativeBankType = NegativeBank.Deny; ;
                   break;
           }
           return ReturnNegativeBankType;
       }

       public static CreditLimit CreditLimitFunction(string DefalultFormate)
       {
           CreditLimit ReturnCreditLimitType = CreditLimit.Warn;
           switch (DefalultFormate)
           {              
               case "Warn":
                   ReturnCreditLimitType = CreditLimit.Warn; 
                   break;
               case "Deny":
                   ReturnCreditLimitType = CreditLimit.Deny; 
                   break;
           }
           return ReturnCreditLimitType;
       }


       /// <summary>
       /// Check if the transaction date false in the freeze date
       /// </summary>
       /// <param name="transactDate"></param>
       /// <returns></returns>
       public static bool isFrozen(DateTime transactDate)
       {
           string value = GetSettings("FREEZE_STATUS");
           string startdate = GetSettings("FREEZE_START_DATE");
           string enddate = GetSettings("FREEZE_END_DATE");
           DateTime dstart = Convert.ToDateTime(startdate);
           DateTime dend = Convert.ToDateTime(enddate);



           if (value == "1" && transactDate >= dstart && transactDate <= dend)
           {
               return true;

           }
           else
           {
               return false;
           }
       }

    }
}
