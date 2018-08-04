using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessLogic.HOS
{
 public   class DoctorDetails
    {
       
        public int DoctorID { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string DoctorCode { get; set; }
        public DateTime BirthDate { get; set; }
        public DateTime StartDate { get; set; }
        public int Gender { get; set; }
        public int IsSingle { get; set; }     
        public string PermAddress { get; set; }
        public string TdocAddress { get; set; }
        public int PermDist { get; set; }
        public int PermZone { get; set; }
        public int TdocDist { get; set; }
        public int TdocZone { get; set; }
        public int NationalityID { get; set; }
        public string CitizenshipNumber { get; set; }
        public string FatherName { get; set; }
        public string GrandfatherName { get; set; }
        public string Religion { get; set; }
        public int EthnicityID { get; set; }
        public string DoctorType { get; set; }
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
        public string Email { get; set; }
        public string DoctorNote { get; set; }
        public byte[] DoctorPhoto { get; set; }
        public DateTime JoinDate { get; set; }
        public DateTime EndDate { get; set; }
        public int DepartmentID { get; set; }
        public int SpecilizationID { get; set; }
        public int FacultyID { get; set; }
        public int Status { get; set; }
        public int Type { get; set; }
        public DateTime GradeIncrementDate { get; set; }
      
        public double StartingSalary { get; set; }
        public double Adjusted { get; set; }
        public bool IsPF { get; set; }
        public int PFNumber { get; set; }
        public decimal PFAdjust { get; set; }
        public bool IsPension { get; set; }
        public string PensionNumber { get; set; }
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

        public bool IsQuarter { get; set; }
        public double Accommodation { get; set; }
      
        public bool IsDocDetailsChanged { get; set; }


        public int Level { get; set; }
        public int Grade { get; set; }

      
    }
         public class DoctorInfo
         {
             public int doctorid { get; set; }
             public string Code { get; set; }
             public string DoctorName { get; set; }
             public DateTime BirthDate { get; set; }
             public string Phone { get; set; }
             public string Email { get; set; }
             public string Address { get; set; }
             public string City { get; set; }
             public string Specilization { get; set; }
             public string Company { get; set; }
             public string StartDate { get; set; }
             public Boolean isDefault { get; set; }

         }
}
