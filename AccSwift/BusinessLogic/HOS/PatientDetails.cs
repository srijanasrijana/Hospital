using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessLogic.HOS
{
   public  class PatientDetails
    {
        public int PatientID { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string RegistrationNo { get; set; }
        public DateTime RegistartionDate { get; set; }
        public DateTime BirthDate { get; set; }
        public int Age { get; set; }
        public int Gender { get; set; }
        public int IsSingle { get; set; }     
        public string FatherName { get; set; }
        public string GuardianName { get; set; }
        public string PatientType { get; set; }
        public int NationalityID { get; set; }

        public int DoctorID { get; set; }
        public string Religion { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
        public string Email { get; set; }
        public string Reason { get; set; }
        public bool IsPatientDetailsChanged { get; set; }

        public string sex { get; set; }

        public int PNID { get; set; }

        
      
    }
}
