using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessLogic.HOS.Report
{
  public  class PatientReportSetting
    {
        public ArrayList AccClassID = new ArrayList();
        public int? patientID = 0;
       
        public string rptType = "";
        public DateTime? FromDate = new DateTime();
        public DateTime? ToDate = new DateTime();

        public string paramPatient = "";
        public string paramPatientType = "";
    }
}
