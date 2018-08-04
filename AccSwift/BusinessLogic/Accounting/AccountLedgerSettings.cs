using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;
using System.Windows.Forms;
using BusinessLogic;
using BusinessLogic.Accounting.Reports;

namespace BusinessLogic
{
   public  class AccountLedgerSettings
    {
       public int LedgerID = 0;
       public int AccountGroupID = 0;
       public bool ChooseLedger = false;
       public bool ChooseAccountGrp = false;
       public DateTime FromDate = new DateTime();
       public DateTime ToDate = new DateTime();
       public bool HasDateRange = false;
       public ArrayList AccClassID = new ArrayList();
       public int ProjectID = 0;
    }
}
