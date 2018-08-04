using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;
using System.Windows.Forms;
using BusinessLogic;

namespace BusinessLogic.Accounting
{
    public class DebtorsDueSettings
    {
        public DateTime? FromDate = null;
        public DateTime? ToDate = null;
        public ArrayList AccClassID = new ArrayList();
        public bool DueBills = false;
        public bool OverDueBills = false;
        public int ProjectID = 0;
        public bool isAllDebtors = false;
        public int DebtorsID = 0;
    }
}
