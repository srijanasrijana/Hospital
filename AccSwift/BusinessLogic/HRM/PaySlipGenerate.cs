using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace BusinessLogic.HRM
{
    public interface IPaySlip
    {
        void InsertJournalId(int id);

        void DeleteJournalId(int journalId);
    }

    public interface IEmployeeList
    {
        void AddEmployee(int EmpID, string Code, string Name, string Designation, string BankAC, bool IsSelected);
    }

    public interface IDoctorList
    {
        void AddDoctor(int DocID, string Code, string Name, string Specilization, string BankAC, bool IsSelected);
    }

    public interface IDoctorInfo
    {
        void AddDoctorInfo(int docID, string code, string Name, string Specilization, bool IsSelected);
    }
  
}
