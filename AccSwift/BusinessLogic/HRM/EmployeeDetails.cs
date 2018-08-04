using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessLogic.HRM
{
    public class EmployeeDetails
    {
        public int EmployeeID { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string EmployeeCode { get; set; }
        public DateTime BirthDate { get; set; }
        public DateTime StartDate { get; set; }
        public int Gender { get; set; }
        public int IsSingle {get;set; }
        public bool IsCoupleWorking { get; set; }
        public string PermAddress { get; set; }
        public string  TempAddress { get; set; }
        public int PermDist { get; set; }
        public int PermZone { get; set; }
        public int TempDist { get; set; }
        public int TempZone { get; set; }
        public int NationalityID { get; set; }
        public string CitizenshipNumber { get; set; }
        public string FatherName { get; set; }
        public string GrandfatherName { get; set; }
        public string Religion { get; set; }
        public int EthnicityID { get; set; }
        public string EmpType { get; set; }//For Normal or Disable Type
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
        public string Email { get; set; }
        public string EmployeeNote { get; set; }
        public byte[] EmployeePhoto { get; set; }
        public DateTime JoinDate { get; set; }
        public DateTime EndDate { get; set; }
        public int DepartmentID { get; set; }
        public int DesignationID { get; set; }
        public int FacultyID { get; set; }
        public int Status { get; set; }
        public int Level { get; set; }
        public int Grade { get; set; }
        public DateTime GradeIncrementDate { get; set; }
        public int Type { get; set; }
        public double StartingSalary { get; set; }
        public double Adjusted { get; set; }
        public bool IsPF { get; set; }
        public int PFNumber { get; set; }
        public decimal PFAdjust { get; set; }
        public bool IsPension { get; set; }
        public string PensionNumber  {get; set; }
        public decimal PensionAdjust { get; set; }

        public bool IsInsurance { get; set; }
        public string InsuranceNumber { get; set; }
        public double InsuranceAmt { get; set; }
        public double InsurancePremium { get; set; }
        public int CIFNumber { get; set; }
        public double CITAmount { get; set; }
        public int BankID { get; set; }
        public string ACNumber { get; set; }
        public double AcademicAlw { get; set; }
        public double BasicSalary { get; set; }
        public string PAN { get; set; }
        public double inflationAlw { get; set; }
        public double AdmAlw { get; set; }

        public double PostAlw { get; set; }
        public double TADA { get; set; }
        public double MiscAllowance { get; set; }
        public double NLKoshDeduct { get; set; }
        public string NLKoshNo { get; set; }

        public double KalyankariAmt { get; set; }
        public string KalyankariNo { get; set; }
        public decimal OverTimeAllow { get; set; }
        public decimal ElectricityCharge { get; set; }

        public string Remarks { get; set; }

        //public bool IsLoan { get; set; }
        //public int LoanID { get; set; }
        //public double LoanPrincipal { get; set; }
        //public double LoanMthInstallment { get; set; }
        //public double LoanMthInterest { get; set; }
        //public double LoanMthDecrease { get; set; }
        //public int LoanRemainingMth { get; set; }
        //public DateTime LoanStartDate { get; set; }
        //public DateTime LoanEndDate { get; set; }
        //public int LoanDuration { get; set; }
        //public double LoanPremium { get; set; }

        //public bool IsAdvance { get; set; }
        //public double AdvAmt { get; set; }
        //public double AdvMthInstallment { get; set; }
        //public double AdvRemainingAmt { get; set; }
        //public DateTime AdvStartDate { get; set; }
        //public DateTime AdvEndDate { get; set; }
        public bool IsQuarter { get; set; }
        public double Accommodation { get; set; }
        public bool IsEmpDetailsChanged { get; set; }
    }
}
