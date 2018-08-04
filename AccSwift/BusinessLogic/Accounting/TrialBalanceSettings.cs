using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;
using System.Windows.Forms;
using BusinessLogic;

namespace BusinessLogic
{
   public  class TrialBalanceSettings
    {
         public DateTime? FromDate = null;
         public DateTime? ToDate = null;
         public int GroupID = 0;
         public bool HasDateRange = false;
         public ArrayList AccClassID = new ArrayList();
         public bool ShowZeroBalance = false;
         public bool ShowSecondLevelGroupDtl = false;
         public bool Details = false;
         public bool AllGroups = false;
         public bool OnlyPrimaryGroups = false;
         public int ProjectID = 0;
         public bool LedgerOnly = false;
         public bool ShowPreviousYear = false;
    }
}
