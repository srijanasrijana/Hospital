using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessLogic.Accounting.Reports
{
  public class BudgetReportSetting
    {

      public string BudgetName = "";
      public DateTime FromDate=new DateTime();
          public DateTime ToDate=new DateTime();
          public int BudgetID = 0;
          public int ProjectID = 0;
          public ArrayList AccClassID = new ArrayList();
          public bool ShowSummary = true;
      
    }
}
