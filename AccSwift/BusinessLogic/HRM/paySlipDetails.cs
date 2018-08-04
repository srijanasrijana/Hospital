using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessLogic.HRM
{
  public class paySlipDetails
    {
        public int employeeID  { get; set; }

        public int doctorID { get; set; }
        public string employeeCode  { get; set; }
        public string employeeName  { get; set; }
        public int designationID  { get; set; }
        public string empLevel { get; set; }
        public decimal basicSalary { get; set; }
        public int grade { get; set; }
        public decimal gradeAmount { get; set; }
        public decimal pfAmount { get; set; }
        public decimal pensionfAmount { get; set; }
        public decimal inflationAlw { get; set; }
        public decimal admAlw { get; set; }
        public decimal academicAlw { get; set; }
        public decimal postAlw { get; set; }
        public decimal festivalAlw { get; set; }
        public decimal miscAllowance { get; set; }

        public decimal overTimeAlw { get; set; }
        public decimal grossAmount { get; set; }
        public decimal pfDeduct { get; set; }
        public decimal pensionfDeduct { get; set; }
        public decimal taxDeduct { get; set; }
        public decimal KKDeduct { get; set; }
        public decimal NLKoshDeduct { get; set; }
        public decimal accommodation { get; set; }
        public decimal electricity { get; set; }
        public decimal loan { get; set; }
        public decimal advanceDeduct { get; set; }
        
        public decimal MiscDeduct { get; set; }
        public decimal totalDeduct { get; set; }
        public decimal netSalary { get; set; }
        public int absentDays  { get; set; }
        public double payableSalary  { get; set; }
        public double miscDeduction  { get; set; }
        //public double pfAmount  { get; set; }
        //public double basicAllowance  { get; set; }
        public double bonus { get; set; }
        public double tada { get; set; }
        public double otherAllowances { get; set; }
        public double totalAllowances { get; set; }
        public double pfDeduction { get; set; }
        public double cifNo { get; set; }
        public double citamount { get; set; }
        public double advance { get; set; }
        public double TDS { get; set; }
        public double netPayableAmount { get; set; }
        public int paySlipid { get; set; }
        public bool EmpPresence { get; set; }
        public int PrimaryID { get; set; }

        public decimal OnePercentTax { get; set; }
        public decimal PFAdjust { get; set; }
        public decimal PensionAdjust { get; set; }
        public decimal InsuranceAmt { get; set; }

    }
}
