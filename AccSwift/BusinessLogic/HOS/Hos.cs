using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace BusinessLogic.HOS
{
   public class Hos
    {
       public static DataTable insertIntoDepartment(string code, string name)
       {
           string str = "insert into Hos.tbldepartment values('" + code + "','" + name + "')";
           return Global.m_db.SelectQry(str, "tbldepartment");
       }
       public static DataTable editdepartment(int ID, string dcode, string dname)
       {
           string str = "update Hos.tbldepartment set DepartmentCode='" + dcode + "',DepartmentName='" + dname + "' where DepartmentID=" + ID + "";
           return Global.m_db.SelectQry(str, "tbldepartment");
       }
       public static DataTable getDepartmentByID(int ID)
       {
           string str = "select DepartmentID,DepartmentCode,DepartmentName from Hos.tbldepartment  where DepartmentID=" + ID + "";
           return Global.m_db.SelectQry(str, "tbldepartment");
       }

       public static DataTable deleteFromDepartment(int departID)
       {
           string str = "delete from Hos.tbldepartment where DepartmentID=" + departID + "";
           return Global.m_db.SelectQry(str, "tbldepartment");
       }
       public static DataTable getdepartment()
       {
           
           string str = "select * from Hos.tbldepartment";
           return Global.m_db.SelectQry(str, "tbldepartment");
       }
       public static DataTable getDepartmentCmb ()
       {
           string str = "Select DepartmentID as ID,DepartmentName as Value from Hos.tblDepartment order by DepartmentName";
          
           return Global.m_db.SelectQry(str, "tbldepartment");
       }

       public static DataTable insertIntoSpecilization(string code, string name)
       {

           string str = "insert into Hos.tblSpecilization values('" + code + "','" + name + "')";
           return Global.m_db.SelectQry(str, "tblSpecilization");
           
       }
       public static DataTable editSpecilization(int ID, string dcode, string dname)
       {
           string str = "update Hos.tblSpecilization set SpecilizationCode='" + dcode + "',SpecilizationName='" + dname + "' where SpecilizationID=" + ID + "";
           return Global.m_db.SelectQry(str, "tblSpecilization");
       }
       public static DataTable deleteFromSpecilization(int departID)
       {
           string str = "delete from Hos.tblSpecilization where SpecilizationID=" + departID + "";
           return Global.m_db.SelectQry(str, "tblSpecilization");
       }
       public static DataTable getSpecilization()
       {

           string str = "select * from Hos.tblSpecilization";
           return Global.m_db.SelectQry(str, "tblSpecilization");
       }
       public static DataTable gettblSpecilizationByID(int ID)
       {
           string str = "select SpecilizationID,SpecilizationCode,SpecilizationName from Hos.tblSpecilization  where SpecilizationID=" + ID + "";
           return Global.m_db.SelectQry(str, "tblSpecilization");
       }

       #region Doctor Faculty
       public static DataTable GetFaculty()
       {
           string str = "select * from Hos.tblFaculty";
           return Global.m_db.SelectQry(str, "Hos.tblFaculty");
       }

       public static DataTable GetFaculty(int id)
       {
         
           string str = "select * from Hos.tblFaculty where FID = '" + id + "'";
           return Global.m_db.SelectQry(str, "Hos.tblFaculty");
       }

       public static DataTable GetFacultyByCmb()
       {
           string str = "select FID as ID,FacultyName as Value  from Hos.tblFaculty";        
           return Global.m_db.SelectQry(str, "Hos.tblFaculty");
       }

       public static int DeleteFaculty(int id)
       {
           string str = "delete from Hos.tblFaculty where FID =" + id + "";
           return Global.m_db.InsertUpdateQry(str);
       }

       public static int CreateFaculty(string code, string name, string remarks)
       {
           int i = 0;
           DataTable dt;
           dt = Global.m_db.SelectQry("select * from Hos.tblFaculty where FacultyCode = '" + code + "'", "Hos.tblFaculty");
           if (dt.Rows.Count > 0)
           {
               i = -100;
           }
           else
           {
               string str = "insert into Hos.tblFaculty values('" + name + "','" + code + "','" + remarks + "')";
               i = Global.m_db.InsertUpdateQry(str);
           }
           return i;
       }

       public static int UpdateFaculty(int Id, string code, string name, string remarks)
       {
           int i = 0;
           DataTable dt;
           dt = Global.m_db.SelectQry("select * from Hos.tblFaculty where FacultyCode = '" + code + "' and FID <> '" + Id + "'", "Hos.tblFaculty");
           if (dt.Rows.Count > 0)
           {
               i = -100;
           }
           else
           {
               string str = "Update Hos.tblFaculty set FacultyCode = '" + code + "',FacultyName = '" + name + "',Remarks = '" + remarks + "' where FID = '" + Id + "'";
               i = Global.m_db.InsertUpdateQry(str);
           }
           return i;
       }

       public static DataTable GetFacultyForCmb()
       {
           string str = "select FID as ID,FacultyName as Value  from Hos.tblFaculty";
           return Global.m_db.SelectQry(str, "Hos.tblFaculty");
       }
       #endregion
       public static DataTable insertintoDiseases(string name, int type)
       {
           string str = "insert into Hos.tblDiseases values('" + name + "','" + type + "')";
           return Global.m_db.SelectQry(str, "tblDiseases");
       }

       public static DataTable GetAllDiseasesTypeData()
       {

           string str = "select DiseasesID,DisesasesName,Dt.DiseasesType from Hos.tblDiseases D join Hos.tblDiseasesType Dt on D.DiseasesType=Dt.DiseasesTypeId";

           return Global.m_db.SelectQry(str, "tblDiseasesType");
       }


       public static DataTable getDisease()
       {

           string str = "select DiseasesID,DisesasesName,Dt.DiseasesType from Hos.tblDiseases D join Hos.tblDiseasesType Dt on D.DiseasesType=Dt.DiseasesTypeId";
           return Global.m_db.SelectQry(str, "tblDiseases");
       }
       public static DataTable editDiseases(int ID, string dname, int distype)
       {
           string str = "update Hos.tblDiseases set DisesasesName='" + dname + "',DiseasesType='" + distype + "' where DiseasesID=" + ID + "";
           return Global.m_db.SelectQry(str, "tblDisease");
       }
       public static DataTable GetDiseseByID(int ID)
       {
           string str = "select DiseasesID,DisesasesName,Dt.DiseasesType from Hos.tblDiseases D join Hos.tblDiseasesType Dt on D.DiseasesType=Dt.DiseasesTypeId where DiseasesID=" + ID + "";
           return Global.m_db.SelectQry(str, "tblDisease");
       }

    }
}
