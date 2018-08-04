using DateManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessLogic.Accounting
{
   public class DebtorDueDate
    {
       public static string GetDueDate(int RowID,int LedgerID,string VoucherType)
       {
           try
           {
               object objResult = Global.m_db.GetScalarValue("select DueDate from System.tblDueDate where RowID='" + RowID + "' and LedgerID='" + LedgerID + "' and VoucherType='" + VoucherType + "'");
               if (objResult == null)
                   return null;
               //  return  objResult.ToString();
               return Date.ToSystem(Convert.ToDateTime(objResult.ToString())).ToString();
               
           }
           catch (Exception ex)
           {             
               throw;
           }
       }

       public static string GetDueDate( int LedgerID, string VoucherType)
       {
           try
           {
               object objResult = Global.m_db.GetScalarValue("select DueDate from System.tblDueDate where LedgerID='" + LedgerID + "' and VoucherType='" + VoucherType + "'");
               if (objResult == null)
                   return null;
               //  return  objResult.ToString();
               return Date.ToSystem(Convert.ToDateTime(objResult.ToString())).ToString();

           }
           catch (Exception ex)
           {
               throw;
           }
       }
    }
}
