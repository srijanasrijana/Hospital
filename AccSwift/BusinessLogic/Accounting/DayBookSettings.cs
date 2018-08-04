using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace BusinessLogic
{
   public  class DayBookSettings
    {
        public DateTime FromDate = new DateTime();
        public DateTime ToDate = new DateTime();
        public bool CashBalanceWise = false;
        public bool TransactionWise = false;
        public bool HasDateRange = false;
        public int ProjectID = 0;
       public  ArrayList AccClassID = new ArrayList();
    }
}
