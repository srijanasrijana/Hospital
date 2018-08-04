using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessLogic.HRM.Report
{
    public class DetailEmployeeReportSettings
    {
        public int DepartmentID = 0;
        public int DesignationID = 0;
        public int FacultyID = 0;
        public int LevelID = 0;
        public int StatusID = 0;
        public int JobTypeID = 0;

        public string Department = "All";
        public string Designation = "All";
        public string Faculty = "All";
        public string Level = "All";
        public string Status = "All";
        public string JobType = "All";
        public string RptType = "Details";
        // for now set to false
        public bool IsDepartment = false;
        public bool IsDesignation = false;
        public bool IsFaculty = false;
        public bool IsLevel = false;
        public bool IsStatus = false;
        public bool IsJobType = false;
    }
}
