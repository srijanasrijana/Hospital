using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessLogic.HRM
{
    public class PartTimeSalaryDetails
    {
        public int PartTimeSalaryMasterID { get; set; }
        public int SN { get; set; }
        public int CashBankLedgerID { get; set; }

        public DateTime Date { get; set; }
        public string Narration { get; set; }
        public decimal Quantity { get; set; }

        public decimal Amount { get; set; }
        public decimal Tax { get; set; }
        public decimal NetAmount { get; set; }
        public int JournalID { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
