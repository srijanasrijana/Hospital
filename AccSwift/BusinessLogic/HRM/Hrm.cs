using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace BusinessLogic
{
  public class Hrm
    {
      
      public static DataTable getdep()
      {
         
          string str = "select * from Hrm.tbldepartment";
          return Global.m_db.SelectQry(str, "tbldepartment");
      }

      public static DataTable GetDepartmentForCmb()
      {
          string str = "Select DepartmentID as ID,DepartmentName as Value from HRM.tblDepartment order by DepartmentName";
          return Global.m_db.SelectQry(str, "HRM.tblDepartment");
      }

      public static DataTable getDepartmentByID(int ID)
      {
          string str = "select DepartmentID,DepartmentCode,DepartmentName from Hrm.tbldepartment  where DepartmentID=" + ID + "";
          return Global.m_db.SelectQry(str, "tbldepartment");
      }


      public static DataTable insertIntoDepartment(string code,string name)
      {
          string str = "insert into Hrm.tbldepartment values('" + code + "','" + name + "')";
          return Global.m_db.SelectQry(str, "tbldepartment");
      }


      public static DataTable editdepartment(int ID,string dcode,string dname)
      {
          string str = "update Hrm.tbldepartment set DepartmentCode='" + dcode + "',DepartmentName='" + dname + "' where DepartmentID=" + ID + "";
          return Global.m_db.SelectQry(str, "tbldepartment");
      }


      public static DataTable deleteFromDepartment(int departID)
      {
          string str = "delete from HRM.tbldepartment where DepartmentID=" + departID + "";
          return Global.m_db.SelectQry(str, "tbldepartment");
      }

      public static DataTable getDesignation()
      {

          string str = "select * from HRM.tbldesignation";
          return Global.m_db.SelectQry(str, "tbldesignation");
      }

      public static DataTable getDesignationByID(int ID)
      {
          string str = "select * from HRM.tbldesignation  where DesignationID=" + ID + "";
          return Global.m_db.SelectQry(str, "tbldesignation");
      }

      public static DataTable insertIntoDesignation(string descode,string desname)
      {
          string str = "insert into Hrm.tbldesignation values('" + descode + "','" + desname + "')";
          return Global.m_db.SelectQry(str, "tbldesignation");
      }

      public static DataTable updateDesignation(int desigID,string desigcode,string designame)
      {
          string str = "update Hrm.tbldesignation set DesignationCode='" + desigcode + "',DesignationName ='" + designame + "' where DesignationID =" + desigID + "";
          return Global.m_db.SelectQry(str, "tbldesignation");
      }

      public static DataTable deleteFromDesignation(int designationID)
      {
          string str = "delete from Hrm.tbldesignation where DesignationID =" + designationID + "";
          return Global.m_db.SelectQry(str, "tbldesignation");
      }

      #region Nationality
      //public static DataTable getNationality()
      //{

      //    string str = "select * from Hrm.Nationality";
      //    return Global.m_db.SelectQry(str, "Nationality");
      //}


      //public static DataTable getNationalityByID(int ID)
      //{
      //    string str = "select NationalityId,NationalityName from HRM.Nationality  where NationalityId=" + ID + "";
      //    return Global.m_db.SelectQry(str, "Nationality");
      //}

      //public static DataTable insertIntoNationality(string name)
      //{
      //    string str = "insert into Hrm.Nationality values('" + name + "')";
      //    return Global.m_db.SelectQry(str, "Nationality");
      //}

      //public static DataTable updateNationality(int ID,string name)
      //{
      //    string str = "update Hrm.Nationality set NationalityName='" + name + "' where NationalityId =" + ID + "";
      //    return Global.m_db.SelectQry(str, "Nationality");
      //}


      //public static DataTable deletefromNationality(int naId)
      //{
      //    string str = "delete from Hrm.Nationality where NationalityId  =" + naId + "";
      //    return Global.m_db.SelectQry(str, "Nationality");
      //}
      #endregion
      public static DataTable getBankName()
      {
          int BankID = AccountGroup.GetGroupIDFromGroupNumber(7);
          string str = "select LedgerID BankID,EngName BankName from Acc.tblLedger where GroupID='" + BankID + "'";
          return Global.m_db.SelectQry(str, "tblBank");
      }


    

      public static bool CheckPF(int EmployeeID)
      {
          object objResult = Global.m_db.GetScalarValue("SELECT IsPF FROM HRM.tblEmployeeSalaryInfo WHERE EmployeeID='" + EmployeeID + "'");
          return Convert.ToBoolean (objResult);
      }

    #region Employee Level
      public static DataTable GetEmpLevel()
      {
          string str = "select * from HRM.tblEmployeeLevel";
          return Global.m_db.SelectQry(str, "HRM.tblEmployeeLevel");
      }

      public static DataTable GetEmpLevelForCmb()
      {
          string str = "select LevelID as ID,LevelName as Value  from HRM.tblEmployeeLevel";
          return Global.m_db.SelectQry(str, "HRM.tblEmployeeLevel");
      }
      public static DataTable GetEmpLevelByID(int id)
      {
          string str = "select * from HRM.tblEmployeeLevel where LevelID = '"+id+"'";
          return Global.m_db.SelectQry(str, "HRM.tblEmployeeLevel");
      }

      public static DataTable CreateLevel(string lvlCode, string lvlName,decimal lvlSal,int gradeNo,decimal gradeAmt,string remarks)
      {
          string str = "insert into Hrm.tblEmployeeLevel values('" + lvlCode + "','" + lvlName + "','" + lvlSal + "','" + remarks + "','" + gradeNo + "','" + gradeAmt + "')";
          return Global.m_db.SelectQry(str, "Hrm.tblEmployeeLevel");
      }

      public static DataTable UpdateLevel(int LvlID, string lvlCode, string lvlName, decimal lvlSal, int gradeNo, decimal gradeAmt, string remarks)
      {
          string str = "Update Hrm.tblEmployeeLevel set LevelCode = '" + lvlCode + "',LevelName = '" + lvlName + "',LevelBasicSalary = '" + lvlSal + "',Remarks = '" + remarks + "',MaxGradeNo = '" + gradeNo + "',PerGradeAmt = '" + gradeAmt + "'  where LevelID = '" + LvlID + "'";
          return Global.m_db.SelectQry(str, "Hrm.tblEmployeeLevel");
      }

      public static int DeleteLevel(int id)
      {
          string str = "delete from HRM.tblEmployeeLevel where LevelID =" + id + "";
          return Global.m_db.InsertUpdateQry(str);
      }

    #endregion

      #region Employee Faculty
      public static DataTable GetFaculty()
      {
          string str = "select * from HRM.tblEmployeeFaculty";
          return Global.m_db.SelectQry(str, "HRM.tblEmployeeFaculty");
      }

      public static DataTable GetFaculty(int id)
      {
          string str = "select * from HRM.tblEmployeeFaculty where FID = '"+id+"'";
          return Global.m_db.SelectQry(str, "HRM.tblEmployeeFaculty");
      }

      public static int DeleteFaculty(int id)
      {
          string str = "delete from HRM.tblEmployeeFaculty where FID =" + id + "";
          return Global.m_db.InsertUpdateQry(str);
      }

      public static int CreateFaculty(string code, string name, string remarks)
      {
          int i = 0;
          DataTable dt;
          dt = Global.m_db.SelectQry("select * from HRM.tblEmployeeFaculty where FacultyCode = '" + code + "'", "HRM.tblEmployeeFaculty");
          if (dt.Rows.Count > 0)
          {
              i = -100;
          }
          else
          {
              string str = "insert into Hrm.tblEmployeeFaculty values('" + name + "','" + code + "','" + remarks + "')";
              i = Global.m_db.InsertUpdateQry(str);
          }
          return i;
      }

      public static int UpdateFaculty(int Id, string code, string name, string remarks)
      {
          int i = 0;
          DataTable dt;
          dt = Global.m_db.SelectQry("select * from HRM.tblEmployeeFaculty where FacultyCode = '" + code + "' and FID <> '" + Id + "'", "HRM.tblEmployeeFaculty");
          if (dt.Rows.Count > 0)
          {
              i = -100;
          }
          else
          {
              string str = "Update Hrm.tblEmployeeFaculty set FacultyCode = '" + code + "',FacultyName = '" + name + "',Remarks = '" + remarks + "' where FID = '" + Id + "'";
              i = Global.m_db.InsertUpdateQry(str);
          }
          return i;
      }

      public static DataTable GetEmpFacultyForCmb()
      {
          string str = "select FID as ID,FacultyName as Value  from HRM.tblEmployeeFaculty";
          return Global.m_db.SelectQry(str, "HRM.tblEmployeeFaculty");
      }
      #endregion

      #region Employee Loan
      public static DataTable GetLoan()
      {
          string str = "select * from HRM.tblLoan";
          return Global.m_db.SelectQry(str, "HRM.tblLoan");
      }

      public static DataTable GetLoanForCmb()
      {
          string str = "select LoanID as ID,LoanName as Value  from HRM.tblLoan";
          return Global.m_db.SelectQry(str, "HRM.tblLoan");
      }

      public static string GetLoanType(int loanID)
      {
          string str = "select LoanType from HRM.tblLoan where LoanID = "+loanID;
          DataTable dt = Global.m_db.SelectQry(str, "HRM.tblLoan");
          if (dt.Rows.Count > 0)
              return dt.Rows[0][0].ToString();

          else
              return null;
      }
      public static DataTable GetLoanByID(int id)
      {
          string str = "select * from HRM.tblLoan where LoanID = '" + id + "'";
          return Global.m_db.SelectQry(str, "HRM.tblLoan");
      }

      public static int DeleteLoan(int id)
      {
          string str = "delete from HRM.tblLoan where LoanID =" + id + "";
          return Global.m_db.InsertUpdateQry(str);
      }

      public static int CreateLoan(string loanName, string loantype,string remarks)
      {
          int i = 0;
          DataTable dt;
          dt = Global.m_db.SelectQry("select * from HRM.tblLoan where LoanName = '" + loanName + "'", "HRM.tblLoan");
          if (dt.Rows.Count > 0)
          {
              i = -100;
          }
          else
          {
              string str = "insert into Hrm.tblLoan values('" + loanName + "','" + loantype + "','" + remarks + "')";
              i = Global.m_db.InsertUpdateQry(str);
          }
          return i;
      }

      public static int UpdateLoan(int loanId,string loanName, string loantype,string remarks)
      {
           int i = 0;
          DataTable dt;
          dt = Global.m_db.SelectQry("select * from HRM.tblLoan where LoanName = '" + loanName + "' and LoanID <> '"+loanId+"'", "HRM.tblLoan");
          if (dt.Rows.Count > 0)
          {
              i = -100;
          }
          else
          {
              string str = "Update Hrm.tblLoan set LoanName = '" + loanName + "',LoanType = '" + loantype +"',Remarks = '" + remarks + "' where LoanID = '" + loanId + "'";
              i = Global.m_db.InsertUpdateQry(str);
          }
          return i;
      }
      #endregion
    }
}
