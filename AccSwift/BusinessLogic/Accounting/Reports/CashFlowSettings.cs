using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace BusinessLogic
{
   public  class CashFlowSettings
    {
       public DateTime FromDate = new DateTime();
       public DateTime ToDate = new DateTime();
       public int CashAccID = 0;
       public bool Details = false;
       public bool Summary = false;
       public bool AccountWise = false;
       public bool GroupWise = false;
       public bool ShowLedger = false;
       public int GroupID = 0;
       public string LedgerID = "";
       public ArrayList AccClassID = new ArrayList();
       public int ProjectID = 0;

    }
}
