using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace BusinessLogic
{
    public class CreditNoteRegisterSettings
    {

        public DateTime FromDate = new DateTime();
        public DateTime ToDate = new DateTime();
        public ArrayList AccClassID = new ArrayList();
        public bool HasDateRange = false;
        public int ProjectID = 0;

    }
}
