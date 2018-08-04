using BusinessLogic.HOS.Report;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BusinessLogic.HOS
{
 public  class Patient
    {

     public string CreatePatient(PatientDetails pat, out int PatientID)
     {
         try
         {
             //Insert The Patinet Basic Information
             Global.m_db.BeginTransaction();
             Global.m_db.ClearParameter();
             Global.m_db.setCommandType(CommandType.StoredProcedure);
             Global.m_db.setCommandText("Hos.spPatientCreate");
             Global.m_db.AddParameter("@FirstName", SqlDbType.NVarChar, 30, pat.FirstName);
             Global.m_db.AddParameter("@MiddleName", SqlDbType.NVarChar, 100, pat.MiddleName);
             Global.m_db.AddParameter("@LastName", SqlDbType.NVarChar, 30, pat.LastName);
             Global.m_db.AddParameter("@RegistrationNo", SqlDbType.NVarChar, 30, pat.RegistrationNo);
             Global.m_db.AddParameter("@RegistrationDate", SqlDbType.DateTime, pat.RegistartionDate);
             Global.m_db.AddParameter("@BirthDate", SqlDbType.DateTime, pat.BirthDate);
             Global.m_db.AddParameter("@Age", SqlDbType.Int, pat.Age);
             Global.m_db.AddParameter("@Gender", SqlDbType.Int, pat.Gender);
             Global.m_db.AddParameter("@IsSingle", SqlDbType.Int, pat.IsSingle);                 
             Global.m_db.AddParameter("@FatherName", SqlDbType.NVarChar, 300, pat.FatherName);
             Global.m_db.AddParameter("@GuardianName", SqlDbType.NVarChar, 300, pat.GuardianName);
             Global.m_db.AddParameter("@PatientType", SqlDbType.NVarChar, 50, pat.PatientType);
             Global.m_db.AddParameter("@NationalityID", SqlDbType.Int, pat.NationalityID);
             Global.m_db.AddParameter("@DoctorID", SqlDbType.Int, pat.DoctorID);
             Global.m_db.AddParameter("@Religion", SqlDbType.NVarChar, 50, pat.Religion);
             Global.m_db.AddParameter("@Address", SqlDbType.NVarChar, 200, pat.Address);
             Global.m_db.AddParameter("@City", SqlDbType.NVarChar, 200, pat.City);
             Global.m_db.AddParameter("@Phone1", SqlDbType.NVarChar, 20, pat.Phone1);
             Global.m_db.AddParameter("@Phone2", SqlDbType.NVarChar, 20, pat.Phone2);
             Global.m_db.AddParameter("@Email", SqlDbType.NVarChar, 50, pat.Email);
             Global.m_db.AddParameter("@Reason", SqlDbType.NVarChar, 200, pat.Reason);
             System.Data.SqlClient.SqlParameter objReturn = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
             Global.m_db.ProcessParameter();
             //if (objReturn.Value.ToString() != "SUCCESS")
             //{
                

             //   // Global.m_db.RollBackTransaction();
             //   // throw new Exception("Unable to create ");
             //}

             int ReturnID = Convert.ToInt32(objReturn.Value.ToString());
             PatientID = ReturnID;
         
            
             //Commit The Transaction
             Global.m_db.CommitTransaction();
             MessageBox.Show("Patient Information Saved Successfully", "Patient Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
             return "SUCCESS";
         }
         catch (Exception ex)
         {
             Global.m_db.RollBackTransaction();
             MessageBox.Show(ex.Message);
             PatientID = -1;
             return "FAILURE";
         }
     }
     public DataTable GetMaxRegistrationNo()
     {
           string str = "select Max(RegistrationNo) from Hos.tblPatient";
           return Global.m_db.SelectQry(str, "tblPAtient");
       

     }

     public string UpdatePatient(PatientDetails pat)
     {
         try
         {
             //Insert The Patinet Basic Information
             Global.m_db.BeginTransaction();
             Global.m_db.ClearParameter();
             Global.m_db.setCommandType(CommandType.StoredProcedure);
             Global.m_db.setCommandText("Hos.spPatientModify");
             Global.m_db.AddParameter("@PatientID", SqlDbType.Int, pat.PatientID);
             Global.m_db.AddParameter("@FirstName", SqlDbType.NVarChar, 30, pat.FirstName);
             Global.m_db.AddParameter("@MiddleName", SqlDbType.NVarChar, 100, pat.MiddleName);
             Global.m_db.AddParameter("@LastName", SqlDbType.NVarChar, 30, pat.LastName);
             Global.m_db.AddParameter("@RegistrationNo", SqlDbType.NVarChar, 30, pat.RegistrationNo);
             Global.m_db.AddParameter("@RegistrationDate", SqlDbType.DateTime, pat.RegistartionDate);
             Global.m_db.AddParameter("@BirthDate", SqlDbType.DateTime, pat.BirthDate);
             Global.m_db.AddParameter("@Age", SqlDbType.Int, pat.Age);
             Global.m_db.AddParameter("@Gender", SqlDbType.Int, pat.Gender);
             Global.m_db.AddParameter("@IsSingle", SqlDbType.Int, pat.IsSingle);
             Global.m_db.AddParameter("@FatherName", SqlDbType.NVarChar, 300, pat.FatherName);
             Global.m_db.AddParameter("@GuardianName", SqlDbType.NVarChar, 300, pat.GuardianName);
             Global.m_db.AddParameter("@PatientType", SqlDbType.NVarChar, 50, pat.PatientType);
             Global.m_db.AddParameter("@NationalityID", SqlDbType.Int, pat.NationalityID);
             Global.m_db.AddParameter("@DoctorID", SqlDbType.Int, pat.DoctorID);
             Global.m_db.AddParameter("@Religion", SqlDbType.NVarChar, 50, pat.Religion);
             Global.m_db.AddParameter("@Address", SqlDbType.NVarChar, 200, pat.Address);
             Global.m_db.AddParameter("@City", SqlDbType.NVarChar, 200, pat.City);
             Global.m_db.AddParameter("@Phone1", SqlDbType.NVarChar, 20, pat.Phone1);
             Global.m_db.AddParameter("@Phone2", SqlDbType.NVarChar, 20, pat.Phone2);
             Global.m_db.AddParameter("@Email", SqlDbType.NVarChar, 50, pat.Email);
             Global.m_db.AddParameter("@Reason", SqlDbType.NVarChar, 200, pat.Reason);
             System.Data.SqlClient.SqlParameter objReturn = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
             Global.m_db.ProcessParameter();
           
             if (objReturn.Value.ToString() != "SUCCESS")
             {
                 Global.m_db.RollBackTransaction();
                 throw new Exception("Unable to Modify Student Information");
             }

            
             //Commit The Transaction
             Global.m_db.CommitTransaction();
             MessageBox.Show("Patient Information Modified Successfully", "Patient Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
             return "SUCCESS";
         }
         catch (Exception ex)
         {
             Global.m_db.RollBackTransaction();
             MessageBox.Show(ex.Message);
            
             return "FAILURE";
         }
     }

     public DataTable GetPAtientDetails(string filter)
     {
         string str = "select PatientID, RegistrationNo, CONCAT( FirstName,' ',MiddleName,' ',LastName) as PatientName from Hos.tblPatient " + filter;
         return Global.m_db.SelectQry(str, "tblDoctor");
     }

     public DataTable FillPatientDetails(int PatID)
     {
         string str = "select p.*,n.NationalityName, CONCAT(d.FirstName,' ',d.MiddleName,' ',d.LastName) as DoctorName from Hos.tblPatient as p join System.tblNationality n on p.NationalityID =n.NationalityID  join Hos.tblDoctor d on d.ID=p.DoctorID where PatientID='" + PatID + "'";
         return Global.m_db.SelectQry(str, "tblPatient");
     }

     public bool  DeletePatient(int patID)
     {
         string str = "delete from Hos.tblPatient where PatientID = '" + patID + "'";   
           DataTable dtsc = Global.m_db.SelectQry(str, "tblPatient");
               if (dtsc.Rows.Count > 0)
                   return false;
               else
                   return true;
     }

     public static DataTable GetPatientNameForCmb()
     {
         string str = "select PatientID as ID, CONCAT(FirstName,' ',MiddleName,' ',LastName) as Value from Hos.tblPatient";
         return Global.m_db.SelectQry(str, "Hos.tblPatient");
      
     }

     public static DataTable GetPatientByID(int id)
     {
         string str = "select PatientID as ID, CONCAT(FirstName,' ',MiddleName,' ',LastName) as Name,RegistrationNo,Gender,RegistrationDate,Age,Address,City,Phone1 as Telephone,Phone2 as Mobile,Reason from Hos.tblPatient where PatientID= '" + id + "'";
         return Global.m_db.SelectQry(str, "Hos.tblPatient");
     }

     public  static DataTable GetPatientList(string filterString)
     {
         string str = "select PatientID,p.RegistrationNo,concat(p.FirstName,' ',p.MiddleName,' ',p.LastName) as PatientName,p.Age,p.PatientType,p.Gender as Sex,p.Phone1 as Telephone,p.Phone2 as Mobile,Date.fnEngToNep( p.RegistrationDate) Date,LedgerID,CONCAT(d.FirstName,' ',d.MiddleName,' ',d.LastName) as DoctorName from HOS.tblPatient as p join Hos.tblDoctor d on d.ID=p.DoctorID" + filterString;
         return Global.m_db.SelectQry(str, "tblPatient");
     }
     public static DataTable GetPatientDetail(int filterString)
     {
         string str ="select p.PatientID,p.RegistrationNo,concat(p.FirstName,' ',p.MiddleName,' ',p.LastName) as PatientName,p.Age,p.PatientType,p.Gender as Sex,p.Phone1 as Telephone,p.Phone2 as Mobile,Date.fnEngToNep( p.RegistrationDate) Date,p.Address,p.City,CONCAT(d.FirstName,' ',d.MiddleName,' ',d.LastName) as DoctorName from HOS.tblPatient as p join Hos.tblDoctor d on d.ID=p.DoctorID where LedgerID='" + filterString + "'";
         return Global.m_db.SelectQry(str, "tblPatient");
     }
     public static string CreateReceipt(PatientDetails pat, DataTable DocotorDetail,  out int ReceiptID)
     {
         try
         {
             //Insert The PAtinet Basic Information
             Global.m_db.BeginTransaction();
             Global.m_db.ClearParameter();
             Global.m_db.setCommandType(CommandType.StoredProcedure);
             Global.m_db.setCommandText("Hos.spReceiptMaster");
             Global.m_db.AddParameter("@PatientID", SqlDbType.Int,  pat.PNID);
             Global.m_db.AddParameter("@PatientRegNo", SqlDbType.NVarChar, 50, pat.RegistrationNo);
             Global.m_db.AddParameter("@Address", SqlDbType.NVarChar, 50, pat.Address);
             Global.m_db.AddParameter("@City", SqlDbType.NVarChar, 50, pat.City);
             Global.m_db.AddParameter("@TelephoneNo", SqlDbType.NVarChar, pat.Phone1);
             Global.m_db.AddParameter("@MobileNo", SqlDbType.NVarChar, pat.Phone2);
             Global.m_db.AddParameter("@Sex", SqlDbType.NVarChar, pat.sex);
             Global.m_db.AddParameter("@Age", SqlDbType.Int, pat.Age);
             Global.m_db.AddParameter("@RegistrationDate", SqlDbType.DateTime,  pat.RegistartionDate);
             Global.m_db.AddParameter("@Reasons", SqlDbType.NVarChar, 500, pat.Reason);
           
             System.Data.SqlClient.SqlParameter objReturn = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
             Global.m_db.ProcessParameter();
             int ReturnID = Convert.ToInt32(objReturn.Value);
             ReceiptID = ReturnID;

         
             //For Doctor Information
             for (int i = 0; i < DocotorDetail.Rows.Count; i++)
             {
                 DataRow drDoctorInfo = DocotorDetail.Rows[i];
                 Global.m_db.ClearParameter();
                 Global.m_db.setCommandType(CommandType.StoredProcedure);
                 Global.m_db.setCommandText("Hos.spReceiptDetail");
                 Global.m_db.AddParameter("@ReceiptMasterID", SqlDbType.Int, ReturnID.ToString());

                 Global.m_db.AddParameter("@DoctorName", SqlDbType.NVarChar, 50, drDoctorInfo["DoctorName"].ToString());
                 Global.m_db.AddParameter("@Specilization", SqlDbType.NVarChar,50, (drDoctorInfo["Specilization"].ToString()));//Set same for both for time being
                 Global.m_db.AddParameter("@Amount", SqlDbType.Decimal, (drDoctorInfo["Amount"].ToString()));
                 Global.m_db.AddParameter("@Remarks", SqlDbType.NVarChar, 500, (drDoctorInfo["Remarks"].ToString()));

                 System.Data.SqlClient.SqlParameter paramReturn2 = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                 Global.m_db.ProcessParameter();
                 if (paramReturn2.Value.ToString() != "SUCCESS")
                 {
                     Global.m_db.RollBackTransaction();
                     throw new Exception("Unable to create Doctor Information");
                 }
             }
            
             //Commit The Transaction
             Global.m_db.CommitTransaction();
             MessageBox.Show("Patinet Information Saved Successfully", "Doctor Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
             return "SUCCESS";
         }
         catch (Exception ex)
         {
             Global.m_db.RollBackTransaction();
             MessageBox.Show(ex.Message);
             ReceiptID = -1;
             return "FAILURE";
         }
     }

     public static int GetPatientLedgerId(int PatientID)
     {
         string str = "select LedgerID from Hos.tblPatient where PatientID ='" + PatientID + "'";
         int returnID = Convert.ToInt32(Global.m_db.GetScalarValue(str));
         return returnID;
     }

     public static DataTable GetPatientDetailByLedgerID(int LedgerID)
     {
         //string filterString = " and SM.LedgerID ='" + LedgerID + "'";
         //Global.m_db.ClearParameter();
         //Global.m_db.setCommandType(CommandType.StoredProcedure);
         //Global.m_db.setCommandText("Sch.spGetStudentDetailFilter");
         //Global.m_db.AddParameter("@FilterString", SqlDbType.NVarChar, 500, filterString);
         
         
         DataTable dt = Global.m_db.GetDataTable();
         return dt;
     }
     public static DataTable GetPatientTypeIdValue()
     {
         String str = "SELECT PatientID as ID,PatientType as Value FROM Hos.tblPatient";
         return Global.m_db.SelectQry(str, "Hos.tblPatient");
     }

     public static DataTable GetAllPatient()
     {
         string str = "select PatientID as ID, CONCAT(FirstName,' ',MiddleName,' ',LastName) as Value from Hos.tblPatient ";
         return Global.m_db.SelectQry(str, "Hos.tblPatient");
     }

     public static DataTable GetPatientReport()
     {

         string str = "select PatientID,p.RegistrationNo,concat(p.FirstName,' ',p.MiddleName,' ',p.LastName) as PatientName,p.Age,p.PatientType,p.Gender as Sex,p.Phone1 as Telephone,p.Phone2 as Mobile,Date.fnEngToNep( p.RegistrationDate) Date, LedgerID from HOS.tblPatient as p";
         return Global.m_db.SelectQry(str, "Hos.tblPatient");
     }



     public static DataTable GetPatientData(int filterString)
     {
         string str = "select p.PatientID,p.RegistrationNo,concat(p.FirstName,' ',p.MiddleName,' ',p.LastName) as PatientName,p.Age,p.PatientType,p.Gender as Sex,p.Phone1 as Telephone,p.Phone2 as Mobile,Date.fnEngToNep( p.RegistrationDate) Date,p.Address,p.City,CONCAT(d.FirstName,' ',d.MiddleName,' ',d.LastName) as DoctorName from HOS.tblPatient as p join Hos.tblDoctor d on d.ID=p.DoctorID where PatientID='" + filterString + "'";
         return Global.m_db.SelectQry(str, "tblPatient");
     }

     

     public static DataTable GetPatientInfoReport(int? PatientID, DateTime? FromDate, DateTime? ToDate)
     {

         string str = "select distinct ROW_NUMBER() over(order by Concat(p.FirstName,' ',p.MiddleName,' ',p.LastName)) as SN,Date.fnEngToNep(p.RegistrationDate) Date,Concat(p.FirstName,' ',p.MiddleName,' ',p.LastName) as Name,Concat(p.Address,' ','',' ',p.City) as Address,p.Gender,p.Age,Concat(p.Phone1,'','  ','',p.Phone2) as ContactNo,Concat(d.FirstName,' ',d.MiddleName,' ',d.LastName) as DoctorConsultant,p.PatientID from Hos.tblPatient p inner join Hos.tblDoctor d on p.DoctorID=d.ID ";
         return Global.m_db.SelectQry(str, "tblPatient");
     }
    }



}
