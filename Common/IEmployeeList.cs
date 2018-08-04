using System;


namespace Common
{
   public interface IEmployeeList
    {
       void AddEmployee(int EmpID, string Code, string Name, string Designation, string BankAC, bool IsSelected);
    }
}
