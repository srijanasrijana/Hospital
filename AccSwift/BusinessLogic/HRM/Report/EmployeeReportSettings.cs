using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessLogic.HRM.Report
{
    public class EmployeeReportSettings
    {
        public int[] paySlipIds { get; set; }

        public string faculty { get; set; }

        public bool isRemaining { get; set; }

        public DateTime? paySlipDate { get; set; }

        public DateTime? fromDate{get; set;}

        public DateTime? toDate{get; set;}
    }
}
