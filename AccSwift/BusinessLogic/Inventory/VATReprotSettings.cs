using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace BusinessLogic
{
  public  class VATReprotSettings
    {



      public bool Summary = false;

      public bool Detail = false;

      public DateTime FromDate = new DateTime();

      public DateTime ToDate = new DateTime();

      public bool Collected = false;

      public bool Paid = false;

      public int ProjectID = 0;

      public ArrayList AccClassID = new ArrayList();

        
    }
}
