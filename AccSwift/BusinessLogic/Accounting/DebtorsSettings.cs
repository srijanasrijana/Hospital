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
    public class DebtorsSettings
    {
        public DateTime? FromDate = null;
        public DateTime? ToDate = null;
        public ArrayList AccClassID = new ArrayList();
        public bool Ageing = false;
        public bool BillwiseAgeing = false;
        public bool ShowAllDebtores = false;
        public int DebtorsID = 0;
        public bool ShowVoucherBalance = false;
        public int FirstPeriod = 0;
        public int SecondPeriod = 0;
        public int ThirdPeriod = 0;
        public int FourthPeriod = 0;
        public int ProjectID = 0;
    }
}
