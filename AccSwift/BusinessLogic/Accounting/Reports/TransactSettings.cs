using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Data;
using System.Windows.Forms;
using BusinessLogic;


namespace BusinessLogic
{
    public class TransactSettings
    {
        public ArrayList AccClassID=new ArrayList();
        public DateTime? FromDate = null;
        public DateTime? ToDate = null;
        public int LedgerID = 0;
        public int AccountGroupID = 0;
        public bool HasDateRange = false;
        public bool ShowZeroBalance = false;
        public string VoucherType = "";
        public int ProjectID = 0;
        public bool ShowRemarks = false;
    }

    
  
}
