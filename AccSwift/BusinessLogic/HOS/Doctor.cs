using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using DateManager;
using System.Windows.Forms;
using System.Data.SqlClient;
using BusinessLogic.HRM;


namespace BusinessLogic.HOS
{
   public  class Doctor
    {
       int? employeeid=null;
       string designation;
       public string CreateDoctor(DoctorDetails doc, DataTable AcademicQualification, DataTable WorkExperience,DataTable Loan,DataTable Advance, out int DoctorID)
       {
           try
           {
               //Insert The Doctor Basic Information
               Global.m_db.BeginTransaction();
               Global.m_db.ClearParameter();
               Global.m_db.setCommandType(CommandType.StoredProcedure);
               Global.m_db.setCommandText("Hos.spDoctorCreate");
               Global.m_db.AddParameter("@FirstName", SqlDbType.NVarChar, 30, doc.FirstName);
               Global.m_db.AddParameter("@MiddleName", SqlDbType.NVarChar, 100, doc.MiddleName);
               Global.m_db.AddParameter("@LastName", SqlDbType.NVarChar, 30, doc.LastName);
               Global.m_db.AddParameter("@DoctorCode", SqlDbType.NVarChar, 30, doc.DoctorCode);
               Global.m_db.AddParameter("@BirthDate", SqlDbType.DateTime, doc.BirthDate);
               Global.m_db.AddParameter("@StartDate", SqlDbType.DateTime, doc.StartDate);
               Global.m_db.AddParameter("@Gender", SqlDbType.Int, doc.Gender);
               Global.m_db.AddParameter("@IsSingle", SqlDbType.Int, doc.IsSingle);
               Global.m_db.AddParameter("@DoctorType", SqlDbType.NVarChar, 50, doc.DoctorType);
               Global.m_db.AddParameter("@NationalityID", SqlDbType.Int, doc.NationalityID);
               Global.m_db.AddParameter("@Phone1", SqlDbType.NVarChar, 20, doc.Phone1);
               Global.m_db.AddParameter("@Phone2", SqlDbType.NVarChar, 20, doc.Phone2);
               Global.m_db.AddParameter("@Email", SqlDbType.NVarChar, 50, doc.Email);            
               Global.m_db.AddParameter("@CitizenshipNo", SqlDbType.NVarChar, 200, doc.CitizenshipNumber);
               Global.m_db.AddParameter("@EthnicityID", SqlDbType.Int, doc.EthnicityID);
               Global.m_db.AddParameter("@PermAddress", SqlDbType.NVarChar, 200, doc.PermAddress);
               Global.m_db.AddParameter("@TempAddress", SqlDbType.NVarChar, 200, doc.TdocAddress);
               Global.m_db.AddParameter("@PermDistID", SqlDbType.Int, doc.PermDist);
               Global.m_db.AddParameter("@PermZoneID", SqlDbType.Int, doc.PermZone);
               Global.m_db.AddParameter("@TempDistID", SqlDbType.Int, doc.TdocDist);
               Global.m_db.AddParameter("@TempZoneID", SqlDbType.Int, doc.TdocZone);
               Global.m_db.AddParameter("@Religion", SqlDbType.NVarChar, 50, doc.Religion);
               Global.m_db.AddParameter("@DoctorNote", SqlDbType.NVarChar, 500, doc.DoctorNote);
               Global.m_db.AddParameter("@FatherName", SqlDbType.NVarChar, 300, doc.FatherName);
               Global.m_db.AddParameter("@GrandfatherName", SqlDbType.NVarChar, 300, doc.GrandfatherName);
               System.Data.SqlClient.SqlParameter objReturn = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
               Global.m_db.ProcessParameter();
               int ReturnID = Convert.ToInt32(objReturn.Value);
               DoctorID = ReturnID;

               //Insert Employement Information
               Global.m_db.ClearParameter();
               Global.m_db.setCommandType(CommandType.StoredProcedure);
               Global.m_db.setCommandText("Hos.spEmploymentCreate");
               Global.m_db.AddParameter("@DoctorID", SqlDbType.Int, ReturnID.ToString());
               Global.m_db.AddParameter("@JoinDate", SqlDbType.DateTime, doc.JoinDate);
               Global.m_db.AddParameter("@RetirementDate", SqlDbType.DateTime, doc.EndDate);//Set same for both for time being
               Global.m_db.AddParameter("@DepartmentID", SqlDbType.Int, doc.DepartmentID);
               Global.m_db.AddParameter("@SpecilizationID", SqlDbType.Int, doc.SpecilizationID);
               Global.m_db.AddParameter("@StatusID", SqlDbType.Int, doc.Status);
               Global.m_db.AddParameter("@TypeID", SqlDbType.Int, doc.Type);
               Global.m_db.AddParameter("@FacultyID", SqlDbType.Int, doc.FacultyID);

               System.Data.SqlClient.SqlParameter paramReturn = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
               Global.m_db.ProcessParameter();
               if (paramReturn.Value.ToString() != "SUCCESS")
               {
                   Global.m_db.RollBackTransaction();
                   throw new Exception("Unable to create Doctor Information");
               }
               //For Academic Qualification
               for (int i = 0; i < AcademicQualification.Rows.Count; i++)
               {
                   DataRow drqualification = AcademicQualification.Rows[i];
                   Global.m_db.ClearParameter();
                   Global.m_db.setCommandType(CommandType.StoredProcedure);
                   Global.m_db.setCommandText("Hos.spQualificationCreate");

                   Global.m_db.AddParameter("@DoctorID", SqlDbType.Int, ReturnID.ToString());               
                   Global.m_db.AddParameter("@InstituteName", SqlDbType.NVarChar, 200, drqualification["InstituteName"].ToString());
                   Global.m_db.AddParameter("@Board", SqlDbType.NVarChar, 200, drqualification["Board"].ToString());//Set same for both for time being
                   Global.m_db.AddParameter("@Percentage", SqlDbType.NVarChar, 20, drqualification["Percentage"].ToString());
                   Global.m_db.AddParameter("@PassYear", SqlDbType.Int, drqualification["PassYear"].ToString());
                   System.Data.SqlClient.SqlParameter paramReturn1 = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                   Global.m_db.ProcessParameter();

                   if (paramReturn1.Value.ToString() != "SUCCESS")
                   {
                       Global.m_db.RollBackTransaction();
                       throw new Exception("Unable to create Doctor Qualification");
                   }
               }
               //For Work Experiences
               for (int i = 0; i < WorkExperience.Rows.Count; i++)
               {
                   DataRow drexperience = WorkExperience.Rows[i];
                   Global.m_db.ClearParameter();
                   Global.m_db.setCommandType(CommandType.StoredProcedure);
                   Global.m_db.setCommandText("HRM.spExperienceCreate");
                   Global.m_db.AddParameter("@EmployeeID", SqlDbType.Int, employeeid);
                   Global.m_db.AddParameter("@DoctorID", SqlDbType.Int, ReturnID.ToString());
                   Global.m_db.AddParameter("@CompanyName", SqlDbType.NVarChar, 100, drexperience["CompanyName"].ToString());
                   Global.m_db.AddParameter("@FromDate", SqlDbType.DateTime, (drexperience["FromDate"].ToString()));//Set same for both for time being
                   Global.m_db.AddParameter("@ToDate", SqlDbType.DateTime,(drexperience["ToDate"].ToString()));
                   Global.m_db.AddParameter("@Designation", SqlDbType.NVarChar, 20, designation);
                   Global.m_db.AddParameter("@Specilization", SqlDbType.NVarChar, 20, drexperience["Specilization"].ToString());
                   System.Data.SqlClient.SqlParameter paramReturn2 = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                   Global.m_db.ProcessParameter();
                   if (paramReturn2.Value.ToString() != "SUCCESS")
                   {
                       Global.m_db.RollBackTransaction();
                       throw new Exception("Unable to create Doctor Experience");
                   }
               }
               //For Doctor Photo
               Global.m_db.ClearParameter();
               Global.m_db.setCommandType(CommandType.StoredProcedure);
               Global.m_db.setCommandText("[Hos].[spDoctorPhotoCreate]");
               Global.m_db.AddParameter("@DoctorID", SqlDbType.Int, ReturnID.ToString());
               Global.m_db.AddParameter("@DoctorPhoto", SqlDbType.Image, doc.DoctorPhoto);
               System.Data.SqlClient.SqlParameter paramReturn3 = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
               Global.m_db.ProcessParameter();
               if (paramReturn3.Value.ToString() != "SUCCESS")
               {
                   Global.m_db.RollBackTransaction();
                   throw new Exception("Unable to create Doctor Photo");
               }


               //Insert DoctorSalary INformation
               Global.m_db.ClearParameter();
               Global.m_db.setCommandType(CommandType.StoredProcedure);
               Global.m_db.setCommandText("HRM.spEmployeeSalaryCreate");
               Global.m_db.AddParameter("@DoctorID", SqlDbType.Int, ReturnID.ToString());
               Global.m_db.AddParameter("@EmployeeID", SqlDbType.Int, employeeid);
               Global.m_db.AddParameter("@StartingSalary", SqlDbType.Decimal, doc.StartingSalary);
               Global.m_db.AddParameter("@Adjusted", SqlDbType.Decimal, doc.Adjusted);
               Global.m_db.AddParameter("@IsPF", SqlDbType.Bit, doc.IsPF);
               Global.m_db.AddParameter("@PFNumber", SqlDbType.Int, doc.PFNumber);
               Global.m_db.AddParameter("@CIFNumber", SqlDbType.Int, doc.CIFNumber);
               Global.m_db.AddParameter("@CITAmount", SqlDbType.Decimal, doc.CITAmount);
               Global.m_db.AddParameter("@BankID", SqlDbType.Int, doc.BankID);
               Global.m_db.AddParameter("@BankACNumber", SqlDbType.NVarChar, 20, doc.ACNumber);
               Global.m_db.AddParameter("@InsurancePremium", SqlDbType.Decimal, doc.InsurancePremium);
               Global.m_db.AddParameter("@BasicSalary", SqlDbType.Decimal, doc.BasicSalary);
               Global.m_db.AddParameter("@PAN", SqlDbType.NVarChar, 20, doc.PAN);

               Global.m_db.AddParameter("@TADA", SqlDbType.Decimal, doc.TADA);
               Global.m_db.AddParameter("@MiscAllowance", SqlDbType.Decimal, doc.MiscAllowance);
               Global.m_db.AddParameter("@NLKoshDeduct", SqlDbType.Decimal, doc.NLKoshDeduct);
               Global.m_db.AddParameter("@NLKoshNo", SqlDbType.NVarChar, doc.NLKoshNo);
               Global.m_db.AddParameter("@Remarks", SqlDbType.NVarChar, 200, doc.Remarks);

               Global.m_db.AddParameter("@Grade", SqlDbType.Int, doc.Grade);

               Global.m_db.AddParameter("@GradeIncrementDate", SqlDbType.DateTime, (doc.GradeIncrementDate));

               Global.m_db.AddParameter("@isPension", SqlDbType.Bit, doc.IsPension);
               Global.m_db.AddParameter("@PensionNumber", SqlDbType.NVarChar, 100, doc.PensionNumber);
               Global.m_db.AddParameter("@isInsurance", SqlDbType.Bit, doc.IsInsurance);
               Global.m_db.AddParameter("@InsuranceNumber", SqlDbType.NVarChar, 100, doc.InsuranceNumber);
               Global.m_db.AddParameter("@InsuranceAmount", SqlDbType.Decimal, doc.InsuranceAmt);
               Global.m_db.AddParameter("@PensionAdjusted", SqlDbType.Decimal, doc.PensionAdjust);
               Global.m_db.AddParameter("@PFAdjusted", SqlDbType.Decimal, doc.PFAdjust);

               Global.m_db.AddParameter("@InflationAlw", SqlDbType.Decimal, doc.inflationAlw);
               Global.m_db.AddParameter("@AdmistrativeAlw", SqlDbType.Decimal, doc.AdmAlw);
               Global.m_db.AddParameter("@AcademicAlw", SqlDbType.Decimal, doc.AcademicAlw);
               Global.m_db.AddParameter("@PostAlw", SqlDbType.Decimal, doc.PostAlw);

               Global.m_db.AddParameter("@Level", SqlDbType.Int, doc.Level);
               Global.m_db.AddParameter("@DepartmentID", SqlDbType.Int, doc.DepartmentID);
               Global.m_db.AddParameter("@DesignationID", SqlDbType.Int, doc.SpecilizationID);

               Global.m_db.AddParameter("@isQuarter", SqlDbType.Bit, doc.IsQuarter);
               Global.m_db.AddParameter("@Accommodation", SqlDbType.Decimal, doc.Accommodation);
               Global.m_db.AddParameter("@ElectricityCharge", SqlDbType.Decimal, doc.ElectricityCharge);


               Global.m_db.AddParameter("@KalyankariNo", SqlDbType.NVarChar, 50, doc.KalyankariNo);
               Global.m_db.AddParameter("@KalyankariAmt", SqlDbType.Decimal, doc.KalyankariAmt);
               Global.m_db.AddParameter("@FacultyID", SqlDbType.Int, doc.FacultyID);
               Global.m_db.AddParameter("@OverTimeAlw", SqlDbType.Decimal, doc.OverTimeAllow);
               System.Data.SqlClient.SqlParameter paramReturnsalary = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
               Global.m_db.ProcessParameter();
               if (paramReturnsalary.Value.ToString() != "SUCCESS")
               {
                   Global.m_db.RollBackTransaction();
                   throw new Exception("Unable to create Doctor Salary");
               }

               //For Loan
               for (int i = 0; i < Loan.Rows.Count; i++)
               {
                   DataRow dr = Loan.Rows[i];
                   Global.m_db.ClearParameter();
                   Global.m_db.setCommandType(CommandType.Text);
                   Global.m_db.setCommandText("Insert into  HRM.tblEmployeeLoan (DoctorID,LoanID,LoanPrincipal,LoanMthInstallment,LoanMthInterest,LoanTotalMth,LoanRemainingMth,LoanStartDate,LoanEndDate,LoanMthPremium,LoanMthDecreaseAmt,InitialInstallment) values(@DoctorID,@LoanID,@LoanPrincipal,@LoanMthInstallment,@LoanMthInterest,@LoanTotalMth,@LoanRemainingMth,@LoanStartDate,@LoanEndDate,@LoanMthPremium,@LoanMthDecreaseAmt,@LoanMthInstallment)");
                   Global.m_db.AddParameter("@DoctorID", SqlDbType.Int, ReturnID.ToString());
                //   Global.m_db.AddParameter("@EmpID", SqlDbType.Int,employeeid);
                   Global.m_db.AddParameter("@LoanID", SqlDbType.Int, Convert.ToInt32(dr["LoanID"].ToString()));
                   Global.m_db.AddParameter("@LoanPrincipal", SqlDbType.Decimal, Convert.ToDecimal(dr["LoanPrincipal"].ToString()));//Set same for both for time being
                   Global.m_db.AddParameter("@LoanMthInstallment", SqlDbType.Decimal, Convert.ToDecimal(dr["LoanMthInstallment"].ToString()));
                   Global.m_db.AddParameter("@LoanMthInterest", SqlDbType.Decimal, dr["LoanMthInterest"].ToString());
                   Global.m_db.AddParameter("@LoanTotalMth", SqlDbType.Int, Convert.ToInt32(dr["LoanTotalMth"].ToString()));
                   Global.m_db.AddParameter("@LoanRemainingMth", SqlDbType.Int, Convert.ToInt32(dr["LoanRemainingMth"].ToString()));
                   Global.m_db.AddParameter("@LoanStartDate", SqlDbType.DateTime, Date.ToDotNet(dr["LoanStartDate"].ToString()));
                   Global.m_db.AddParameter("@LoanEndDate", SqlDbType.DateTime, Date.ToDotNet(dr["LoanEndDate"].ToString()));
                   Global.m_db.AddParameter("@LoanMthPremium", SqlDbType.Decimal, Convert.ToDecimal(dr["LoanMthPremium"].ToString()));
                   Global.m_db.AddParameter("@LoanMthDecreaseAmt", SqlDbType.Decimal, Convert.ToDecimal(dr["LoanMthDecreaseAmt"].ToString()));
                   Global.m_db.ProcessParameter();
               }

               //For Advance
               for (int i = 0; i < Advance.Rows.Count; i++)
               {
                   DataRow dr = Advance.Rows[i];
                   Global.m_db.ClearParameter();
                   Global.m_db.setCommandType(CommandType.Text);
                   Global.m_db.setCommandText("Insert into HRM.tblEmployeeAdvance (DoctorID,AdvTitle,TotalAmt,Installment,TakenDate,ReturnDate,RemainingAmt) values(@DoctorID,@AdvTitle,@TotalAmt,@Installment,@TakenDate,@ReturnDate,@RemainingAmt)");
                   Global.m_db.AddParameter("@DoctorID", SqlDbType.Int, ReturnID.ToString());
                //   Global.m_db.AddParameter("@EmpID", SqlDbType.Int, employeeid);
                   Global.m_db.AddParameter("@AdvTitle", SqlDbType.NVarChar, dr["AdvTitle"].ToString());
                   Global.m_db.AddParameter("@TotalAmt", SqlDbType.Decimal, Convert.ToDecimal(dr["TotalAmt"].ToString()));
                   Global.m_db.AddParameter("@Installment", SqlDbType.Decimal, Convert.ToDecimal(dr["Installment"].ToString()));
                   Global.m_db.AddParameter("@TakenDate", SqlDbType.DateTime, Date.ToDotNet(dr["TakenDate"].ToString()));
                   Global.m_db.AddParameter("@ReturnDate", SqlDbType.DateTime, Date.ToDotNet(dr["ReturnDate"].ToString()));
                   Global.m_db.AddParameter("@RemainingAmt", SqlDbType.Decimal, Convert.ToDecimal(dr["RemainingAmt"].ToString()));
                   Global.m_db.ProcessParameter();
               }
         

               //Commit The Transaction
               Global.m_db.CommitTransaction();
               MessageBox.Show("Doctor Information Saved Successfully", "Doctor Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
               return "SUCCESS";
           }
           catch (Exception ex)
           {
               Global.m_db.RollBackTransaction();
               MessageBox.Show(ex.Message);
               DoctorID = -1;
               return "FAILURE";
           }
       }
       public string UpdateDoctor(DoctorDetails doc, DataTable AcademicQualification, DataTable WorkExperience, bool ispicupdate,DataTable Loan,DataTable Advance)
       {
           try
           {

               Global.m_db.BeginTransaction();
               Global.m_db.ClearParameter();
               Global.m_db.setCommandType(CommandType.StoredProcedure);
               Global.m_db.setCommandText("Hos.spDoctorModify");
               Global.m_db.AddParameter("@DoctorID", SqlDbType.Int, doc.DoctorID);
               Global.m_db.AddParameter("@FirstName", SqlDbType.NVarChar, 30, doc.FirstName);
               Global.m_db.AddParameter("@MiddleName", SqlDbType.NVarChar, 100, doc.MiddleName);
               Global.m_db.AddParameter("@LastName", SqlDbType.NVarChar, 30, doc.LastName);
               Global.m_db.AddParameter("@DoctorCode", SqlDbType.NVarChar, 30, doc.DoctorCode);
               Global.m_db.AddParameter("@BirthDate", SqlDbType.DateTime, doc.BirthDate);
               Global.m_db.AddParameter("@StartDate", SqlDbType.DateTime, doc.StartDate);
               Global.m_db.AddParameter("@Gender", SqlDbType.Int, doc.Gender);
               Global.m_db.AddParameter("@IsSingle", SqlDbType.Int, doc.IsSingle);
               Global.m_db.AddParameter("@DoctorType", SqlDbType.NVarChar, 50, doc.DoctorType);
               Global.m_db.AddParameter("@NationalityID", SqlDbType.Int, doc.NationalityID);
               Global.m_db.AddParameter("@Phone1", SqlDbType.NVarChar, 20, doc.Phone1);
               Global.m_db.AddParameter("@Phone2", SqlDbType.NVarChar, 20, doc.Phone2);
               Global.m_db.AddParameter("@Email", SqlDbType.NVarChar, 50, doc.Email);
               Global.m_db.AddParameter("@CitizenshipNo", SqlDbType.NVarChar, 200, doc.CitizenshipNumber);
               Global.m_db.AddParameter("@EthnicityID", SqlDbType.Int, doc.EthnicityID);
               Global.m_db.AddParameter("@PermAddress", SqlDbType.NVarChar, 200, doc.PermAddress);
               Global.m_db.AddParameter("@TempAddress", SqlDbType.NVarChar, 200, doc.TdocAddress);
               Global.m_db.AddParameter("@PermDistID", SqlDbType.Int, doc.PermDist);
               Global.m_db.AddParameter("@PermZoneID", SqlDbType.Int, doc.PermZone);
               Global.m_db.AddParameter("@TempDistID", SqlDbType.Int, doc.TdocDist);
               Global.m_db.AddParameter("@TempZoneID", SqlDbType.Int, doc.TdocZone);
               Global.m_db.AddParameter("@Religion", SqlDbType.NVarChar, 50, doc.Religion);
               Global.m_db.AddParameter("@DoctorNote", SqlDbType.NVarChar, 500, doc.DoctorNote);
               Global.m_db.AddParameter("@FatherName", SqlDbType.NVarChar, 300, doc.FatherName);
               Global.m_db.AddParameter("@GrandfatherName", SqlDbType.NVarChar, 300, doc.GrandfatherName);
               System.Data.SqlClient.SqlParameter objReturn = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
               Global.m_db.ProcessParameter();
               if (objReturn.Value.ToString() != "SUCCESS")
               {
                   Global.m_db.RollBackTransaction();
                   throw new Exception("Unable to Modify Doctor Information");
               }

               //Insert Employement Information
               if (doc.IsDocDetailsChanged)
               {
                   Global.m_db.ClearParameter();
                   Global.m_db.setCommandType(CommandType.StoredProcedure);
                   Global.m_db.setCommandText("Hos.spEmploymentCreate");
                   Global.m_db.AddParameter("@DoctorID", SqlDbType.Int, doc.DoctorID);
                   Global.m_db.AddParameter("@JoinDate", SqlDbType.DateTime, doc.JoinDate);
                   Global.m_db.AddParameter("@RetirementDate", SqlDbType.DateTime, doc.EndDate);//Set same for both for time being
                   Global.m_db.AddParameter("@DepartmentID", SqlDbType.Int, doc.DepartmentID);
                   Global.m_db.AddParameter("@SpecilizationID", SqlDbType.Int, doc.SpecilizationID);
                   Global.m_db.AddParameter("@StatusID", SqlDbType.Int, doc.Status);
                   Global.m_db.AddParameter("@TypeID", SqlDbType.Int, doc.Type);
                   Global.m_db.AddParameter("@FacultyID", SqlDbType.Int, doc.FacultyID);

                   System.Data.SqlClient.SqlParameter paramReturn = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                   Global.m_db.ProcessParameter();
                   if (paramReturn.Value.ToString() != "SUCCESS")
                   {
                       Global.m_db.RollBackTransaction();
                       throw new Exception("Unable to create Doctor Information");
                   }
               }
              // First delete the old Qualification Details
                       Global.m_db.InsertUpdateQry("DELETE FROM Hos.tblAcademicQualification WHERE DoctorID='" + doc.DoctorID + "'");

                       //For Academic Qualification
                       for (int i = 0; i < AcademicQualification.Rows.Count; i++)
                       {
                           DataRow drqualification = AcademicQualification.Rows[i];
                           Global.m_db.ClearParameter();
                           Global.m_db.setCommandType(CommandType.StoredProcedure);
                           Global.m_db.setCommandText("Hos.spQualificationCreate");              
                           Global.m_db.AddParameter("@DoctorID", SqlDbType.Int, doc.DoctorID);               
                           Global.m_db.AddParameter("@InstituteName", SqlDbType.NVarChar, 200, drqualification["InstituteName"].ToString());
                           Global.m_db.AddParameter("@Board", SqlDbType.NVarChar, 200, drqualification["Board"].ToString());//Set same for both for time being
                           Global.m_db.AddParameter("@Percentage", SqlDbType.NVarChar, 20, drqualification["Percentage"].ToString());
                           Global.m_db.AddParameter("@PassYear", SqlDbType.Int, drqualification["PassYear"].ToString());
                           System.Data.SqlClient.SqlParameter paramReturn1 = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                           Global.m_db.ProcessParameter();
                           if (paramReturn1.Value.ToString() != "SUCCESS")
                           {
                               Global.m_db.RollBackTransaction();
                               throw new Exception("Unable to Update Doctor Qualification");
                           }
                       }

                       //First delete the old Work Experiences Details

                       Global.m_db.InsertUpdateQry("DELETE FROM HRM.tblWorkExperiences WHERE DoctorID='" + doc.DoctorID + "'");
                       for (int i = 0; i < WorkExperience.Rows.Count; i++)
                       {
                           DataRow drexperience = WorkExperience.Rows[i];
                           Global.m_db.ClearParameter();
                           Global.m_db.setCommandType(CommandType.StoredProcedure);
                           Global.m_db.setCommandText("HRM.spExperienceCreate");
                           Global.m_db.AddParameter("@EmployeeID", SqlDbType.Int, employeeid);
                           Global.m_db.AddParameter("@DoctorID", SqlDbType.Int, doc.DoctorID);
                           Global.m_db.AddParameter("@CompanyName", SqlDbType.NVarChar, 100, drexperience["CompanyName"].ToString());
                           Global.m_db.AddParameter("@FromDate", SqlDbType.DateTime, (drexperience["FromDate"].ToString()));//Set same for both for time being
                           Global.m_db.AddParameter("@ToDate", SqlDbType.DateTime, (drexperience["ToDate"].ToString()));
                           Global.m_db.AddParameter("@Specilization", SqlDbType.NVarChar, 20, drexperience["Specilization"].ToString());
                           Global.m_db.AddParameter("@Designation", SqlDbType.NVarChar, 20, designation);
                           System.Data.SqlClient.SqlParameter paramReturn2 = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                           Global.m_db.ProcessParameter();
                           if (paramReturn2.Value.ToString() != "SUCCESS")
                           {
                               Global.m_db.RollBackTransaction();
                               throw new Exception("Unable to Update Doctor Experience");
                           }
                       }


                       if (ispicupdate == true)
                       {//For Doctor Photo
                           Global.m_db.ClearParameter();
                           Global.m_db.setCommandType(CommandType.StoredProcedure);
                           Global.m_db.setCommandText("Hos.spDoctorPhotoUpdate");
                           Global.m_db.AddParameter("@DoctorID", SqlDbType.Int, doc.DoctorID);
                           Global.m_db.AddParameter("@DoctorPhoto", SqlDbType.Image, doc.DoctorPhoto);
                           System.Data.SqlClient.SqlParameter paramReturn3 = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                           Global.m_db.ProcessParameter();
                           if (paramReturn3.Value.ToString() != "SUCCESS")
                           {
                               Global.m_db.RollBackTransaction();
                               throw new Exception("Unable to Update Doctor Photo");
                           }
                       }


                       //Insert EmployeeSalary INformation
                       Global.m_db.ClearParameter();
                       Global.m_db.setCommandType(CommandType.StoredProcedure);
                       Global.m_db.setCommandText("HRM.spEmployeeSalaryModify");
                       Global.m_db.AddParameter("@DoctorID", SqlDbType.Int, doc.DoctorID);
                       Global.m_db.AddParameter("@EmployeeID", SqlDbType.Int, employeeid);
                       Global.m_db.AddParameter("@StartingSalary", SqlDbType.Decimal, doc.StartingSalary);
                       Global.m_db.AddParameter("@Adjusted", SqlDbType.Decimal, doc.Adjusted);
                       Global.m_db.AddParameter("@IsPF", SqlDbType.Bit, doc.IsPF);
                       Global.m_db.AddParameter("@PFNumber", SqlDbType.Int, doc.PFNumber);
                       Global.m_db.AddParameter("@BankID", SqlDbType.Int, doc.BankID);
                       Global.m_db.AddParameter("@BankACNumber", SqlDbType.NVarChar, 20, doc.ACNumber);
                       Global.m_db.AddParameter("@InsurancePremium", SqlDbType.Decimal, doc.InsurancePremium);
                       Global.m_db.AddParameter("@BasicSalary", SqlDbType.Decimal, doc.BasicSalary);
                       Global.m_db.AddParameter("@PAN", SqlDbType.NVarChar, 20, doc.PAN);

                       Global.m_db.AddParameter("@NLKoshDeduct", SqlDbType.Decimal, doc.NLKoshDeduct);
                       Global.m_db.AddParameter("@NLKoshNo", SqlDbType.NVarChar, 50, doc.NLKoshNo);
                       Global.m_db.AddParameter("@Remarks", SqlDbType.NVarChar, 200, doc.Remarks);
                       Global.m_db.AddParameter("@Grade", SqlDbType.Int, doc.Grade);

                       Global.m_db.AddParameter("@GradeIncrementDate", SqlDbType.DateTime, doc.GradeIncrementDate);
                       Global.m_db.AddParameter("@PensionAdjusted", SqlDbType.Decimal, doc.PensionAdjust);
                       Global.m_db.AddParameter("@PFAdjusted", SqlDbType.Decimal, doc.PFAdjust);

                       Global.m_db.AddParameter("@isPension", SqlDbType.Bit, doc.IsPension);
                       Global.m_db.AddParameter("@PensionNumber", SqlDbType.NVarChar, 100, doc.PensionNumber);
                       Global.m_db.AddParameter("@isInsurance", SqlDbType.Bit, doc.IsInsurance);
                       Global.m_db.AddParameter("@InsuranceNumber", SqlDbType.NVarChar, 100, doc.InsuranceNumber);
                       Global.m_db.AddParameter("@InsuranceAmount", SqlDbType.Decimal, doc.InsuranceAmt);
                       Global.m_db.AddParameter("@InflationAlw", SqlDbType.Decimal, doc.inflationAlw);
                       Global.m_db.AddParameter("@AdmistrativeAlw", SqlDbType.Decimal, doc.AdmAlw);
                       Global.m_db.AddParameter("@AcademicAlw", SqlDbType.Decimal, doc.AcademicAlw);
                       Global.m_db.AddParameter("@PostAlw", SqlDbType.Decimal, doc.PostAlw);

                       Global.m_db.AddParameter("@Level", SqlDbType.Int, doc.Level);
                       Global.m_db.AddParameter("@DepartmentID", SqlDbType.Int, doc.DepartmentID);
                       Global.m_db.AddParameter("@DesignationID", SqlDbType.Int, doc.SpecilizationID);

                       Global.m_db.AddParameter("@isQuarter", SqlDbType.Bit, doc.IsQuarter);
                       Global.m_db.AddParameter("@Accommodation", SqlDbType.Decimal, doc.Accommodation);
                       Global.m_db.AddParameter("@ElectricityCharge", SqlDbType.Decimal, doc.ElectricityCharge);

                       Global.m_db.AddParameter("@KalyankariNo", SqlDbType.NVarChar, 50, doc.KalyankariNo);
                       Global.m_db.AddParameter("@KalyankariAmt", SqlDbType.Decimal, doc.KalyankariAmt);
                       Global.m_db.AddParameter("@FacultyID", SqlDbType.Int, doc.FacultyID);
                       Global.m_db.AddParameter("@OverTimeAlw", SqlDbType.Decimal, doc.OverTimeAllow);
                       System.Data.SqlClient.SqlParameter paramReturnsalary = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                       Global.m_db.ProcessParameter();
                       if (paramReturnsalary.Value.ToString() != "SUCCESS")
                       {
                           Global.m_db.RollBackTransaction();
                           throw new Exception("Unable to create Doctor Salary");
                       }

               ////For Loan
                       string ELIDs = ""; // to save all the ELIDs which have not been deleted in the row deletion process
                       List<DataRow> rowsToDelete = new List<DataRow>();

                       foreach (DataRow DR in Loan.Rows)
                       {

                           if (Convert.ToInt32(DR["ELID"]) > 0)
                           {
                               ELIDs += DR["ELID"].ToString() + ",";

                               rowsToDelete.Add(DR);

                           }
                       }

                       foreach (DataRow dr in rowsToDelete)
                       {
                           Loan.Rows.Remove(dr);
                       }

                       Loan.AcceptChanges();

                       ELIDs = ELIDs.Length > 0 ? ELIDs.Substring(0, ELIDs.Length - 1) : "";
                    //   Global.m_db.InsertUpdateQry("Delete from HRM.tblEmployeeLoan where EmpID = " + emp.EmployeeID + (ELIDs.Length > 0 ? " and ELID not in(" + ELIDs + ")" : ""));
                       Global.m_db.InsertUpdateQry("Delete from HRM.tblEmployeeLoan where DoctorID = " + doc.DoctorID + (ELIDs.Length > 0 ? " and ELID not in(" + ELIDs + ")" : ""));

                       //Global.m_db.InsertUpdateQry("DELETE FROM HRM.tblEmployeeLoan WHERE EmpID='" + emp.EmployeeID + "'");
                       for (int i = 0; i < Loan.Rows.Count; i++)
                       {
                           DataRow dr = Loan.Rows[i];

                           if (Convert.ToInt32(dr["ELID"]) == 0)
                           {
                               Global.m_db.ClearParameter();
                               Global.m_db.setCommandType(CommandType.Text);
                               Global.m_db.setCommandText("Insert into HRM.tblEmployeeLoan (DoctorID,LoanID,LoanPrincipal,LoanMthInstallment,LoanMthInterest,LoanTotalMth,LoanRemainingMth,LoanStartDate,LoanEndDate,LoanMthPremium,LoanMthDecreaseAmt,InitialInstallment) values (@DoctorID,@LoanID,@LoanPrincipal,@LoanMthInstallment,@LoanMthInterest,@LoanTotalMth,@LoanRemainingMth,@LoanStartDate,@LoanEndDate,@LoanMthPremium,@LoanMthDecreaseAmt,@InitialInstallment)");
                               Global.m_db.AddParameter("@DoctorID", SqlDbType.Int, doc.DoctorID);
                              // Global.m_db.AddParameter("@EmpID", SqlDbType.Int, employeeid);
                               Global.m_db.AddParameter("@LoanID", SqlDbType.Int, Convert.ToInt32(dr["LoanID"].ToString()));
                               Global.m_db.AddParameter("@LoanPrincipal", SqlDbType.Decimal, Convert.ToDecimal(dr["LoanPrincipal"].ToString()));//Set same for both for time being
                               Global.m_db.AddParameter("@LoanMthInstallment", SqlDbType.Decimal, Convert.ToDecimal(dr["LoanMthInstallment"].ToString()));
                               Global.m_db.AddParameter("@LoanMthInterest", SqlDbType.Decimal, dr["LoanMthInterest"].ToString());
                               Global.m_db.AddParameter("@LoanTotalMth", SqlDbType.Int, Convert.ToInt32(dr["LoanTotalMth"].ToString()));
                               Global.m_db.AddParameter("@LoanRemainingMth", SqlDbType.Int, Convert.ToInt32(dr["LoanRemainingMth"].ToString()));
                               Global.m_db.AddParameter("@LoanStartDate", SqlDbType.DateTime, Date.ToDotNet(dr["LoanStartDate"].ToString()));
                               Global.m_db.AddParameter("@LoanEndDate", SqlDbType.DateTime, Date.ToDotNet(dr["LoanEndDate"].ToString()));
                               Global.m_db.AddParameter("@LoanMthPremium", SqlDbType.Decimal, Convert.ToDecimal(dr["LoanMthPremium"].ToString()));
                               Global.m_db.AddParameter("@LoanMthDecreaseAmt", SqlDbType.Decimal, Convert.ToDecimal(dr["LoanMthDecreaseAmt"].ToString()));
                               Global.m_db.AddParameter("@InitialInstallment", SqlDbType.Decimal, Convert.ToDecimal(dr["InitialInstallment"].ToString()));

                               Global.m_db.ProcessParameter();
                           }
                       }

                    //   For Advance
                       Global.m_db.InsertUpdateQry("DELETE FROM  HRM.tblEmployeeAdvance WHERE DoctorID='" + doc.DoctorID + "'");
                               for (int i = 0; i < Advance.Rows.Count; i++)
                               {
                                   DataRow dr = Advance.Rows[i];
                                   Global.m_db.ClearParameter();
                                   Global.m_db.setCommandType(CommandType.Text);
                                   Global.m_db.setCommandText("Insert into  HRM.tblEmployeeAdvance (DoctorID,AdvTitle,TotalAmt,Installment,TakenDate,ReturnDate,RemainingAmt) values(@DoctorID,@AdvTitle,@TotalAmt,@Installment,@TakenDate,@ReturnDate,@RemainingAmt)");
                                   Global.m_db.AddParameter("@DoctorID", SqlDbType.Int, doc.DoctorID);
                               //    Global.m_db.AddParameter("@EmpID", SqlDbType.Int, employeeid);
                                   Global.m_db.AddParameter("@AdvTitle", SqlDbType.NVarChar, dr["AdvTitle"].ToString());
                                   Global.m_db.AddParameter("@TotalAmt", SqlDbType.Decimal, Convert.ToDecimal(dr["TotalAmt"].ToString()));
                                   Global.m_db.AddParameter("@Installment", SqlDbType.Decimal, Convert.ToDecimal(dr["Installment"].ToString()));
                                   Global.m_db.AddParameter("@TakenDate", SqlDbType.DateTime, Date.ToDotNet(dr["TakenDate"].ToString()));
                                   Global.m_db.AddParameter("@ReturnDate", SqlDbType.DateTime, Date.ToDotNet(dr["ReturnDate"].ToString()));
                                   Global.m_db.AddParameter("@RemainingAmt", SqlDbType.Decimal, Convert.ToDecimal(dr["RemainingAmt"].ToString()));
                                   Global.m_db.ProcessParameter();
                               }


               //Commit The Transaction
               Global.m_db.CommitTransaction();
               MessageBox.Show("Doctor Information Updated Successfully", "Doctor Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
               return "SUCCESS";
           }
           catch (SqlException ex)
           {
               Global.m_db.RollBackTransaction();
               MessageBox.Show(ex.Message + ex.LineNumber);
               return "FAILURE";
           }
       }
     
      
       public DataTable GetDoctorDetails(string filter)
       {
           string str = "select ID, DoctorCode, CONCAT( FirstName,' ',MiddleName,' ',LastName) as DoctorName from Hos.tblDoctor " + filter;
           return Global.m_db.SelectQry(str, "tblDoctor");
       }

       public static IEnumerable<DoctorInfo> GetDoctorDetailInListView()
       {
           try
           {

               string sql = "select  d.ID,d.DoctorCode,Concat(FirstName , '',MiddleName ,'', LastName) as DoctorName,d.BirthDate,d.Phone2 as ContactNo,d.Email, d.PermAddress as [Address],dis.DistrictName as City,w.Specilization,w.CompanyName ,Date.fnEngToNep(d.StartDate) StartDate from hos.tblDoctor d join System.tblDistrict dis on d.PermDistID=dis.DistrictID join  Hos.tblWorkExperiences w on w.DoctorID=d.ID  order by DoctorCode";
               DataTable dt = Global.m_db.SelectQry(sql, "tblDoctor");
               return dt.AsEnumerable().Select(r => new DoctorInfo
               {
                   doctorid=r.Field<int>("ID"),
                   Code = r.Field<string>("DoctorCode"),
                   DoctorName = r.Field<string>("DoctorName"),
                   BirthDate = r.Field<DateTime>("BirthDate"),
                   Phone = r.Field<string>("ContactNo"),
                   Email = r.Field<string>("Email"),
                   Address = r.Field<string>("Address"),
                   City = r.Field<string>("City"),
                   Specilization = r.Field<string>("Specilization"),
                   Company = r.Field<string>("CompanyName"),
                   StartDate = r.Field<string>("StartDate"),
 
               }).ToList();
           }
           catch (Exception ex)
           {

               throw ex;
           }
       }

       public DoctorInfo GetDoctorDetailInListViewByID(int id)
       {
           try
           {

               string sql = "select  d.ID,d.DoctorCode,Concat(FirstName , ' ',MiddleName ,' ', LastName) as DoctorName,d.BirthDate,d.Phone2 as ContactNo,d.Email, d.PermAddress as [Address],dis.DistrictName as City,w.Specilization,w.CompanyName ,Date.fnEngToNep(d.StartDate) StartDate from hos.tblDoctor d join System.tblDistrict dis on d.PermDistID=dis.DistrictID join  Hos.tblWorkExperiences w on w.DoctorID=d.ID  where ID='" + id + "'";
               DataTable dt = Global.m_db.SelectQry(sql, "tblDoctor");
               return dt.AsEnumerable().Select(r => new DoctorInfo
               {
                   doctorid = r.Field<int>("ID"),
                   Code = r.Field<string>("DoctorCode"),
                   DoctorName = r.Field<string>("DoctorName"),
                   BirthDate = r.Field<DateTime>("BirthDate"),
                   Phone = r.Field<string>("ContactNo"),
                   Email = r.Field<string>("Email"),
                   Address = r.Field<string>("Address"),
                   City = r.Field<string>("City"),
                   Specilization = r.Field<string>("Specilization"),
                   Company = r.Field<string>("CompanyName"),
                   StartDate = r.Field<string>("StartDate"),

               }).FirstOrDefault();
           }
           catch (Exception ex)
           {

               throw ex;
           }
       }
       public DataTable FillDoctorDetails(int DocID)
       {
           string str = "select D.*,ES.*,L.LevelName,P.DoctorPhoto,pdis.DistrictName PermanentDistrict,tdis.DistrictName TemporaryDistrict,pzon.ZoneName PermanentZone,tzon.ZoneName TemporaryZone,nat.NationalityName,eth.EthnicityName from Hos.tblDoctor D join  Hos.tblDoctorPhoto P on D.ID=P.DoctorID join System.tblDistrict pdis on D.PermDistID=pdis.DistrictID join System.tblDistrict tdis on D.TempDistID=tdis.DistrictID join system.tblZone pzon on D.PermZoneID=pzon.ZoneID join system.tblZone tzon on D.TempZoneID=tzon.ZoneID join System.tblNationality nat on D.NationalityID=nat.NationalityID join System.tblEthnicity eth on D.EthnicityID=eth.EthnicityID  join Hrm.tblEmployeeSalaryInfo ES on ES.DoctorID=D.ID  join Hos.tblDoctorLevel L on  ES.Level=L.LevelID where D.ID='" + DocID + "'";
       return Global.m_db.SelectQry(str, "tblDoctor");
       }
       public DataTable GetDoctorEmploymentDetails(int DocID)
       {
           
           string str = "select top 1 Ed.EmployeementDetailsID, ED.JoinDate,RetirementDate,D.DepartmentName,Ds.SpecilizationName,ef.FacultyName,Type,Status  from Hos.tblEmployeementDetails ED, Hos.tbldepartment D, Hos.tblSpecilization Ds, Hos.tblFaculty ef where Ed.DoctorID='" + DocID + "' and  ED.Department=D.DepartmentID and ED.Specilization=Ds.SpecilizationID and ED.FacultyID = ef.FID order by employeementdetailsid desc";
           return Global.m_db.SelectQry(str, "tblDoctor");

       }
       public DataTable DoctorQualification(int DocID)
       {
           string str = "select * from Hos.tblAcademicQualification where DoctorID='" + DocID + "'";
           return Global.m_db.SelectQry(str, "tblqualification");
       }

       public DataTable DoctorExperience(int DocID)
       {
           string str = "select * from HRM.tblWorkExperiences where DoctorID='" + DocID + "'";
           return Global.m_db.SelectQry(str, "tblExperience");
       }

       
       public DataTable JobHistory(int DocID)
       {
           string str = "select distinct ED.JoinDate,RetirementDate,D.DepartmentName,Ds.SpecilizationName,EF.FacultyName,Type,Status,ED.EmployeementDetailsID from Hos.tblEmployeementDetails ED,Hos.tbldepartment D, Hos.tblSpecilization Ds,Hos.tblFaculty EF where  ED.Department=D.DepartmentID and ED.Specilization=Ds.SpecilizationID and ED.FacultyID= EF.FID and ED.DoctorID='" + DocID + "' order by RetirementDate desc";
           return Global.m_db.SelectQry(str, "tblExperience");
       }

       public bool DeleteDoctor(int docId)
       {
           bool val = false;
           Global.m_db.ClearParameter();
           Global.m_db.setCommandType(CommandType.StoredProcedure);
           Global.m_db.setCommandText("Hos.spDoctorDelete");
           Global.m_db.AddParameter("@docId", SqlDbType.Int, docId);
           System.Data.SqlClient.SqlParameter paramReturn1 = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
           Global.m_db.ProcessParameter();
           if (paramReturn1.Value.ToString() == "SUCCESS")
               val = true;

           return val;
       }

       public bool CheckStaffCode(string code, int docID)
       {
           try
           {
               string str = "Select * from Hos.tblDoctor where DoctorCode='" + code + "' and ID <> " + docID;
               DataTable dtsc = Global.m_db.SelectQry(str, "Hos.tblDoctor");
               if (dtsc.Rows.Count > 0)
                   return false;
               else
                   return true;

           }
           catch (Exception ex)
           {
               MessageBox.Show(ex.Message);
               return false;
           }

       }

       public static DataTable getSpecilizationName()
       {

           string str = "select SpecilizationName  from Hos.tblSpecilization";
          return  Global.m_db.SelectQry(str, "tblSpecilization");
       }


       #region  Level
       public static DataTable GetDoctorLevel()
       {
           string str = "select * from Hos.tblDoctorLevel";
           return Global.m_db.SelectQry(str, "Hos.tblDoctorLevel");
       }

       public static DataTable GetDocLevelForCmb()
       {
           string str = "select LevelID as ID,LevelName as Value  from Hos.tblDoctorLevel";
           return Global.m_db.SelectQry(str, "Hos.tblDoctorLevel");
       }
       public static DataTable GetDocLevelByID(int id)
       {
           string str = "select * from Hos.tblDoctorLevel where LevelID = '" + id + "'";
           return Global.m_db.SelectQry(str, "Hos.tblDoctorLevel");
       }

       public static DataTable CreateLevel(string lvlCode, string lvlName, decimal lvlSal, int gradeNo, decimal gradeAmt, string remarks)
       {
           string str = "insert into Hos.tblDoctorLevel values('" + lvlCode + "','" + lvlName + "','" + lvlSal + "','" + remarks + "','" + gradeNo + "','" + gradeAmt + "')";
           return Global.m_db.SelectQry(str, "Hos.tblDoctorLevel");
       }

       public static DataTable UpdateLevel(int LvlID, string lvlCode, string lvlName, decimal lvlSal, int gradeNo, decimal gradeAmt, string remarks)
       {
           string str = "Update Hos.tblDoctorLevel set LevelCode = '" + lvlCode + "',LevelName = '" + lvlName + "',LevelBasicSalary = '" + lvlSal + "',Remarks = '" + remarks + "',MaxGradeNo = '" + gradeNo + "',PerGradeAmt = '" + gradeAmt + "'  where LevelID = '" + LvlID + "'";
           return Global.m_db.SelectQry(str, "Hos.tblDoctorLevel");
       }

       public static int DeleteLevel(int id)
       {
           string str = "delete from Hos.tblDoctorLevel where LevelID =" + id + "";
           return Global.m_db.InsertUpdateQry(str);
       }

       #endregion

       public static DataTable getBankName()
       {
           int BankID = AccountGroup.GetGroupIDFromGroupNumber(7);
           string str = "select LedgerID BankID,EngName BankName from Acc.tblLedger where GroupID='" + BankID + "'";
           return Global.m_db.SelectQry(str, "tblBank");
       }
       public DataTable GetDoctorAdvance(int DocID)
       {
           string str = "select * from HRM.tblEmployeeAdvance where DoctorID='" + DocID + "'";
           return Global.m_db.SelectQry(str, "tblDoctorAdvance");
       }
     

       public DataTable GetDoctorList(string filterString)
       {
           string str = "select E.ID,E.DoctorCode,CONCAT(FirstName,' ',MiddleName,' ',LastName) as DoctorName,a.*,e.DoctorType,el.LevelName,eth.EthnicityName,ES.BankACNumber from Hos.tblDoctor E,Hos.tblDoctorLevel el,System.tblEthnicity eth, Hrm.tblEmployeeSalaryInfo ES ,(select top 1 Ed.EmployeementDetailsID, ED.JoinDate,RetirementDate,ED.Department,D.DepartmentName,ED.Specilization,Ds.SpecilizationName,ED.FacultyID,ef.FacultyName,Type,Status from Hos.tblEmployeementDetails ED,Hos.tbldepartment D,Hos.tblSpecilization Ds,HRM.tblEmployeeFaculty ef where ED.Department=D.DepartmentID and ED.Specilization=Ds.SpecilizationID and ED.FacultyID = ef.FID order by employeementdetailsid desc )a where ES.DoctorID=E.ID and ES.Level = el.LevelID and e.EthnicityID = eth.EthnicityID " + filterString;
           return Global.m_db.SelectQry(str, "tblDoctor");
       }

       public static void SavePartTimeSalary(PartTimeSalaryDetails ps, DataTable dt)
       {
           try
           {
               Global.m_db.BeginTransaction();
               Global.m_db.ClearParameter();
               Global.m_db.setCommandType(CommandType.StoredProcedure);
               Global.m_db.setCommandText("HRM.spPartTimeSalaryMasterCreate");
               Global.m_db.AddParameter("@MasterID", SqlDbType.Int, ps.PartTimeSalaryMasterID);
               Global.m_db.AddParameter("@Quantity", SqlDbType.Decimal, ps.Quantity);
               Global.m_db.AddParameter("@Amount", SqlDbType.Decimal, ps.Amount);
               Global.m_db.AddParameter("@Tax", SqlDbType.Decimal, ps.Tax);
               Global.m_db.AddParameter("@NetAmount", SqlDbType.Decimal, ps.NetAmount);
               Global.m_db.AddParameter("@CreatedBy", SqlDbType.NVarChar, 30, ps.CreatedBy);
               Global.m_db.AddParameter("@Narration", SqlDbType.NVarChar, 500, ps.Narration);
               Global.m_db.AddParameter("@CreatedDate", SqlDbType.Date, ps.CreatedDate);
               Global.m_db.AddParameter("@Date", SqlDbType.Date, ps.Date);
               System.Data.SqlClient.SqlParameter objReturn = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
               Global.m_db.ProcessParameter();
               int ReturnID = ps.PartTimeSalaryMasterID > 0 ? ps.PartTimeSalaryMasterID : Convert.ToInt32(objReturn.Value);

               if (ps.PartTimeSalaryMasterID > 0)
               {
                   Global.m_db.InsertUpdateQry("Delete from HRM.tblPartTimeSalaryDetail where PTMasterID = " + ReturnID);
               }
               for (int i = 0; i < dt.Rows.Count; i++)
               {
                   int employeeid = 0;
                   DataRow dr = dt.Rows[i];
                   Global.m_db.ClearParameter();
                   Global.m_db.setCommandType(CommandType.StoredProcedure);
                   Global.m_db.setCommandText("HRM.spPartTimeSalaryDetailsCreate");
                   Global.m_db.AddParameter("@PTMasterID", SqlDbType.Int, ReturnID);
               
                   Global.m_db.AddParameter("@EmployeeID", SqlDbType.Int, employeeid.ToString());
                   Global.m_db.AddParameter("@DoctorID", SqlDbType.Int, Convert.ToInt32(dr["DoctorID"].ToString()));
                   Global.m_db.AddParameter("@EmpName", SqlDbType.NVarChar, 150, dr["Name"].ToString());
                   Global.m_db.AddParameter("@Designation", SqlDbType.NVarChar, 100, dr["Specilization"].ToString());
                   Global.m_db.AddParameter("@BankACNo", SqlDbType.NVarChar, 50, dr["BankAC"].ToString());
                   Global.m_db.AddParameter("@Remarks", SqlDbType.NVarChar, 500, dr["Remarks"].ToString());
                   Global.m_db.AddParameter("@ClassQty", SqlDbType.Decimal, Convert.ToDecimal(dr["QtyClass"].ToString()));
                   Global.m_db.AddParameter("@Rate", SqlDbType.Decimal, Convert.ToDecimal(dr["Rate"].ToString()));
                   Global.m_db.AddParameter("@Amount", SqlDbType.Decimal, Convert.ToDecimal(dr["Amount"].ToString()));
                   Global.m_db.AddParameter("@Tax", SqlDbType.Decimal, Convert.ToDecimal(dr["Tax"].ToString()));
                   Global.m_db.AddParameter("@NetAmount", SqlDbType.Decimal, Convert.ToDecimal(dr["NetAmount"].ToString()));
                   System.Data.SqlClient.SqlParameter paramReturn1 = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                   Global.m_db.ProcessParameter();
                   if (paramReturn1.Value.ToString() != "SUCCESS")
                   {
                       Global.m_db.RollBackTransaction();
                       throw new Exception("Unable to create part time salary.");
                   }
               }
               Global.m_db.CommitTransaction();
               MessageBox.Show("Doctor Salary Created Successfully ", "Part Time Salary Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
           }
           catch (Exception ex)
           {
               Global.m_db.RollBackTransaction();
               Global.MsgError(ex.Message);
           }
       }

       public static DataTable DoctorsalaryDetails(int departmentID, int facultyID, int paySlipID = 0)
       {
           string departmentSetting = "", facultySetting = "", paySlipSetting = "", selectPaySlipIDSetting = "";
           if (paySlipID > 0)
           {
               paySlipSetting = " and e.ID in(select DoctorID from HRM.tblPaySlipMasterDetails where paySlipID = " + paySlipID + " and EmpPresence = 0) ";
               selectPaySlipIDSetting = " ,(select top 1 ID from HRM.tblPaySlipMasterDetails ps where paySlipID = " + paySlipID + " and ps.DoctorID = e.ID) PrimaryID ";
           }
           string str = "select e.ID, e.DoctorCode, CONCAT( FirstName,' ',MiddleName,' ',LastName) as doctorname,d.DepartmentName,de.SpecilizationName,"
               + "si.*,el.LevelName,el.PerGradeAmt as GradeAmt,(Select SUM(EL.LoanMthPremium) from HRM.tblEmployeeLoan EL where EL.DoctorID = E.ID and " +
               "EL.LoanRemainingMth  > 0) as LoanPremium,(Select SUM(EA.Installment) from HRM.tblEmployeeAdvance EA where EA.DoctorID = E.ID and EA.RemainingAmt>0) " +
               "as AdvInsatallment,(Select SUM(EA1.RemainingAmt) from HRM.tblEmployeeAdvance EA1 where EA1.DoctorID = E.ID and EA1.RemainingAmt>0) as AdvRemaining " +
               selectPaySlipIDSetting + " from Hos.tblDoctor e,Hos.tbldepartment d,Hos.tblSpecilization de,HRM.tblEmployeeSalaryInfo si, Hos.tblDoctorLevel  el " +
               " where e.ID=si.DoctorID and si.departmentid=d.DepartmentID and si.designationid=de.SpecilizationID   and si.Level = el.LevelID and" +
               " e.IsPartTime = 0"
               // donot load employees whose status is break or retire
           + " and (select top 1 edet.Status from Hos.tblEmployeementDetails edet where e.ID = edet.DoctorID order by EmployeementDetailsID desc)<= 1 ";
           string orderBy = " order by si.BasicSalary desc, doctorname ";//" order by e.doctorname";
           if (departmentID > 0)
           {
               departmentSetting = " and si.DepartmentID = '" + departmentID + "'";
               //str = "select e.ID, e.StaffCode, CONCAT( FirstName,' ',MiddleName,' ',LastName) as staffname,d.DepartmentName,de.DesignationName,si.*,el.LevelName,el.PerGradeAmt as GradeAmt,(Select SUM(EL.LoanMthPremium) from HRM.tblEmployeeLoan EL where EL.EmpID = E.ID and EL.LoanRemainingMth  > 0) as LoanPremium,(Select SUM(EA.Installment) from HRM.tblEmployeeAdvance EA where EA.EmpID = E.ID and EA.RemainingAmt>0) as AdvInsatallment,(Select SUM(EA1.RemainingAmt) from HRM.tblEmployeeAdvance EA1 where EA1.EmpID = E.ID and EA1.RemainingAmt>0) as AdvRemaining  from HRM.tblEmployee e,HRM.tbldepartment d,HRM.tbldesignation de,HRM.tblEmployeeSalaryInfo si,HRM.tblEmployeeLevel el where e.ID=si.EmployeeID and si.departmentid=d.DepartmentID and si.designationid=de.DesignationID and si.EmpLevel = el.LevelID and e.IsPartTime = 0 and si.DepartmentID = '"+departmentID+"' order by e.StaffCode";
           }
           if (facultyID > 0)
           {
               facultySetting = " and  si.FacultyID = '" + facultyID + "'";
           }


           //string str = "select e.ID, e.StaffCode, CONCAT( FirstName,' ',MiddleName,' ',LastName) as staffname,d.DepartmentName,de.DesignationName,si.*,el.LevelName,el.PerGradeAmt as GradeAmt from HRM.tblEmployee e,HRM.tbldepartment d,HRM.tbldesignation de,HRM.tblEmployeeSalaryInfo si,HRM.tblEmployeeLevel el where e.ID=si.EmployeeID and si.departmentid=d.DepartmentID and si.designationid=de.DesignationID and si.EmpLevel = el.LevelID and e.IsPartTime = 0 order by e.StaffCode";
           str += departmentSetting + facultySetting + paySlipSetting + orderBy;

           return Global.m_db.SelectQry(str, "HRM.tblEmployeeSalaryInfo");
       }

       public static int UpdateDoctorGrade()
       {
           try
           {
               Global.m_db.ClearParameter();
               Global.m_db.setCommandText("[HOS].[spUpdateDoctorGrade]");
               Global.m_db.setCommandType(CommandType.StoredProcedure);

               return Global.m_db.ProcessParameter();
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }

           public static DataTable GetDoctorList(int FacultyID = 0)
           {
               try
               {
                   string FacultyStr = "";
                   if (FacultyID > 0)
                   {
                       FacultyStr = " where si.FacultyID = " + FacultyID;
                   }
                   return Global.m_db.SelectQry("select e.ID ID, CONCAT(FirstName, ' ', MiddleName, ' ', LastName) Name "
                                                  + "from  Hos.tblDoctor e  "
                                                  + "left join HRM.tblEmployeeSalaryInfo si on e.ID = si.DoctorID "
                                                  + FacultyStr, "Hos.tblDoctor");
               }
               catch (Exception ex)
               {

                   throw ex;
               }
           }

           public static DataTable GetDoctorLoan(int EmpID, bool isLoanForPaySlip = false)
           {
               string strForPaySlip = " ";
               if (isLoanForPaySlip)
               {
                   strForPaySlip = " and EL.LoanRemainingMth > 0";
               }
               string str = "select EL.*,L.LoanName,L.LoanType from HRM.tblEmployeeLoan EL left join HRM.tblLoan L on El.LoanID = L.LoanID where EL.DoctorID='" + EmpID + "' " + strForPaySlip;
               return Global.m_db.SelectQry(str, "tblEmployeeLoan");
           }

           public static DataTable GetDoctorForTax(int docID)
           {
               string str = "select e.IsSingle,e.DoctorType,coalesce(s.OverTimeAlw,0) OverTimeAlw,s.InsurancePremium,e.Gender from Hos.tblDoctor e,HRM.tblEmployeeSalaryInfo s where e.ID = s.DoctorID and e.ID = '" + docID + "'";
               return Global.m_db.SelectQry(str, "tblEmployee");
           }

           public static void SavePaySlip(int paySlipId, int id, string monthname, DateTime createddate, string createdby, string modifiedby, string year, int DepartmentID, int FacultyID, int isVoucherEntered, decimal tSalary, decimal tGrade, decimal tPF, decimal tPensionF, decimal tInflationAlw, decimal tAdmAlw, decimal tAcademicAlw, decimal tPostAlw, decimal tFestivalAlw, decimal tMiscAllow, decimal tGrossAmount, decimal tPFDeduct, decimal tPensionFDeduct, decimal tTaxDeduct, decimal tKK, decimal tNLK, decimal tAccommodation, decimal tElectricity, decimal tLoan, decimal tAdvance, decimal tMiscDeduct, decimal tTotalDeduct, decimal tNetSalary, bool isChkFestiveMonth, string Narration, DataTable dtpayslipmasterdetail, decimal tOverTimeAlw, decimal tOnePercentTax, decimal tPFAdjust, decimal tPensionAdjust, DataTable dtLoan, decimal tInsuranceAmt, DataTable dtSalaryAdjust = null)//,DataTable dtallowances,DataTable dtdeduct)
           {
               try
               {
                   Global.m_db.BeginTransaction();
                   Global.m_db.ClearParameter();
                   Global.m_db.setCommandType(CommandType.StoredProcedure);
                   Global.m_db.setCommandText("HRM.spPaySlipMasterCreate");
                   Global.m_db.AddParameter("@paySlipId", SqlDbType.Int, paySlipId);
                   Global.m_db.AddParameter("@monthID", SqlDbType.Int, id);
                   Global.m_db.AddParameter("@monthName", SqlDbType.NVarChar, 30, monthname);
                   Global.m_db.AddParameter("@date", SqlDbType.DateTime, createddate);
                   Global.m_db.AddParameter("@createdBy", SqlDbType.NVarChar, 30, createdby);
                   Global.m_db.AddParameter("@modifiedBy", SqlDbType.NVarChar, 30, modifiedby);
                   Global.m_db.AddParameter("@year", SqlDbType.NVarChar, 10, year);
                   Global.m_db.AddParameter("@isVoucherEntered", SqlDbType.Int, isVoucherEntered);
                   Global.m_db.AddParameter("@tSalary", SqlDbType.Decimal, tSalary);
                   Global.m_db.AddParameter("@tGrade", SqlDbType.Decimal, tGrade);
                   Global.m_db.AddParameter("@tPF", SqlDbType.Decimal, tPF);
                   Global.m_db.AddParameter("@tPension", SqlDbType.Decimal, tPensionF);
                   Global.m_db.AddParameter("@tInflationAlw", SqlDbType.Decimal, tInflationAlw);
                   Global.m_db.AddParameter("@tAdmAlw", SqlDbType.Decimal, tAdmAlw);
                   Global.m_db.AddParameter("@tAcademicAlw", SqlDbType.Decimal, tAcademicAlw);
                   Global.m_db.AddParameter("@tPostAlw", SqlDbType.Decimal, tPostAlw);
                   Global.m_db.AddParameter("@tFestivalAlw", SqlDbType.Decimal, tFestivalAlw);
                   Global.m_db.AddParameter("@tMiscAllow", SqlDbType.Decimal, tMiscAllow);
                   Global.m_db.AddParameter("@tOverTimeAlw", SqlDbType.Decimal, tOverTimeAlw);

                   Global.m_db.AddParameter("@tGrossAmount", SqlDbType.Decimal, tGrossAmount);
                   Global.m_db.AddParameter("@tPFDeduct", SqlDbType.Decimal, tPFDeduct);
                   Global.m_db.AddParameter("@tPensionDeduct", SqlDbType.Decimal, tPensionFDeduct);
                   Global.m_db.AddParameter("@tTaxDeduct", SqlDbType.Decimal, tTaxDeduct);
                   Global.m_db.AddParameter("@tKK", SqlDbType.Decimal, tKK);
                   Global.m_db.AddParameter("@tNLKosh", SqlDbType.Decimal, tNLK);
                   Global.m_db.AddParameter("@tAccommodation", SqlDbType.Decimal, tAccommodation);
                   Global.m_db.AddParameter("@tElectricity", SqlDbType.Decimal, tElectricity);
                   Global.m_db.AddParameter("@tLoan", SqlDbType.Decimal, tLoan);
                   Global.m_db.AddParameter("@tAdvance", SqlDbType.Decimal, tAdvance);
                   Global.m_db.AddParameter("@tMiscDeduct", SqlDbType.Decimal, tMiscDeduct);
                   Global.m_db.AddParameter("@tTotalDeduct", SqlDbType.Decimal, tTotalDeduct);
                   Global.m_db.AddParameter("@IsFestiveMonth", SqlDbType.Bit, isChkFestiveMonth);
                   Global.m_db.AddParameter("@tNetSalary", SqlDbType.Decimal, tNetSalary);
                   Global.m_db.AddParameter("@Narration", SqlDbType.NVarChar, 1000, Narration);
                   Global.m_db.AddParameter("@DepartmentID", SqlDbType.Int, DepartmentID);
                   Global.m_db.AddParameter("@FacultyID", SqlDbType.Int, FacultyID);
                   Global.m_db.AddParameter("@tOnePercentTax", SqlDbType.Decimal, tOnePercentTax);
                   Global.m_db.AddParameter("@tPFAdjust", SqlDbType.Decimal, tPFAdjust);
                   Global.m_db.AddParameter("@tPensionAdjust", SqlDbType.Decimal, tPensionAdjust);
                   Global.m_db.AddParameter("@tInsuranceAmt", SqlDbType.Decimal, tInsuranceAmt);

                   System.Data.SqlClient.SqlParameter objReturn = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                   Global.m_db.ProcessParameter();
                   int ReturnID = paySlipId > 0 ? paySlipId : Convert.ToInt32(objReturn.Value);

                   if (paySlipId > 0)
                   {
                       Global.m_db.InsertUpdateQry("delete from HRM.tblPaySlipMasterDetails where paySlipId = " + ReturnID);
                   }
                   //For MasterDetailSlip
                   for (int i = 0; i < dtpayslipmasterdetail.Rows.Count; i++)
                   {
                       int employeeid = 0;
                       DataRow drpayslipmasterdetail = dtpayslipmasterdetail.Rows[i];
                       Global.m_db.ClearParameter();
                       Global.m_db.setCommandType(CommandType.StoredProcedure);
                       Global.m_db.setCommandText("HRM.spPaySlipMasterDetailsCreate");
                       Global.m_db.AddParameter("@paySlipId", SqlDbType.Int, ReturnID);
                       Global.m_db.AddParameter("@employeeID", SqlDbType.Int,employeeid.ToString());
                       Global.m_db.AddParameter("@doctorID", SqlDbType.Int, Convert.ToInt32(drpayslipmasterdetail["DoctorID"].ToString()));
                     
                       Global.m_db.AddParameter("@employeeName", SqlDbType.NVarChar, 30, drpayslipmasterdetail["employeeName"].ToString());
                       Global.m_db.AddParameter("@employeeCode ", SqlDbType.NVarChar, 30, drpayslipmasterdetail["employeeCode"].ToString());
                       Global.m_db.AddParameter("@designationID", SqlDbType.Int, Convert.ToInt32(drpayslipmasterdetail["designationID"].ToString()));//Set same for both for time being
                       Global.m_db.AddParameter("@empLevel", SqlDbType.NVarChar, 50, drpayslipmasterdetail["empLevel"].ToString());
                       Global.m_db.AddParameter("@basicSalary", SqlDbType.Decimal, drpayslipmasterdetail["basicSalary"].ToString());
                       Global.m_db.AddParameter("@grade", SqlDbType.Int, Convert.ToInt32(drpayslipmasterdetail["grade"].ToString()));
                       Global.m_db.AddParameter("@gradeAmount", SqlDbType.Decimal, drpayslipmasterdetail["gradeAmount"].ToString());
                       Global.m_db.AddParameter("@pfAmount", SqlDbType.Decimal, drpayslipmasterdetail["pfAmount"].ToString());//Set same for both for time being 

                       Global.m_db.AddParameter("@miscAllowance", SqlDbType.Decimal, drpayslipmasterdetail["miscAllowance"].ToString());
                       Global.m_db.AddParameter("@overTimeAllowance", SqlDbType.Decimal, drpayslipmasterdetail["overTimeAlw"].ToString());

                       Global.m_db.AddParameter("@grossAmount", SqlDbType.Decimal, drpayslipmasterdetail["grossAmount"].ToString());
                       Global.m_db.AddParameter("@pfDeduct", SqlDbType.Decimal, drpayslipmasterdetail["pfDeduct"].ToString());//Set same for both for time being
                       Global.m_db.AddParameter("@taxDeduct", SqlDbType.Decimal, drpayslipmasterdetail["taxDeduct"].ToString());

                       Global.m_db.AddParameter("@NLKoshDeduct", SqlDbType.Decimal, drpayslipmasterdetail["NLKDeduct"].ToString());
                       Global.m_db.AddParameter("@MiscDeduct", SqlDbType.Decimal, drpayslipmasterdetail["MiscDeduct"].ToString());
                       Global.m_db.AddParameter("@totalDedcut", SqlDbType.Decimal, drpayslipmasterdetail["totalDedcut"].ToString());
                       Global.m_db.AddParameter("@netSalary", SqlDbType.Decimal, drpayslipmasterdetail["netSalary"].ToString());

                       Global.m_db.AddParameter("@Accommodation", SqlDbType.Decimal, drpayslipmasterdetail["accommodation"].ToString());
                       Global.m_db.AddParameter("@LoanMthPremium", SqlDbType.Decimal, drpayslipmasterdetail["loan"].ToString());
                       Global.m_db.AddParameter("@AdvMthInstallment", SqlDbType.Decimal, drpayslipmasterdetail["advance"].ToString());
                       Global.m_db.AddParameter("@KalyankariAmt", SqlDbType.Decimal, drpayslipmasterdetail["KKDeduct"].ToString());
                       Global.m_db.AddParameter("@InflationAlw", SqlDbType.Decimal, drpayslipmasterdetail["inflationAlw"].ToString());
                       Global.m_db.AddParameter("@AdmistrativeAlw", SqlDbType.Decimal, drpayslipmasterdetail["admAlw"].ToString());//Set same for both for time being
                       Global.m_db.AddParameter("@AcademicAlw", SqlDbType.Decimal, drpayslipmasterdetail["academicAlw"].ToString());
                       Global.m_db.AddParameter("@PostAlw", SqlDbType.Decimal, drpayslipmasterdetail["postAlw"].ToString());
                       Global.m_db.AddParameter("@FestivalAlw", SqlDbType.Decimal, drpayslipmasterdetail["festivalAlw"].ToString());
                       Global.m_db.AddParameter("@PensionFAmt", SqlDbType.Decimal, drpayslipmasterdetail["pensionfAmount"].ToString());
                       Global.m_db.AddParameter("@ElectricityDeduct", SqlDbType.Decimal, drpayslipmasterdetail["electricity"].ToString());
                       Global.m_db.AddParameter("@PensionFDeduct", SqlDbType.Decimal, drpayslipmasterdetail["pensionfDeduct"].ToString());
                       Global.m_db.AddParameter("@isAlreadySaved", SqlDbType.Int, paySlipId > 0 ? 1 : 0);
                       Global.m_db.AddParameter("@EmpPresence", SqlDbType.Bit, drpayslipmasterdetail["EmpPresence"].ToString());

                       Global.m_db.AddParameter("@OnePercentTax", SqlDbType.Decimal, drpayslipmasterdetail["OnePercentTax"].ToString());
                       Global.m_db.AddParameter("@PFAdjust", SqlDbType.Decimal, drpayslipmasterdetail["PFAdjust"].ToString());
                       Global.m_db.AddParameter("@PensionAdjust", SqlDbType.Decimal, drpayslipmasterdetail["PensionAdjust"].ToString());
                       Global.m_db.AddParameter("@InsuranceAmt", SqlDbType.Decimal, drpayslipmasterdetail["InsuranceAmt"].ToString());

                       System.Data.SqlClient.SqlParameter paramReturn1 = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                       Global.m_db.ProcessParameter();
                       if (paramReturn1.Value.ToString() != "SUCCESS")
                       {
                           Global.m_db.RollBackTransaction();
                           throw new Exception("Unable to create Salary PaySlip.");
                       }
                   }

                   // insert into loan table
                   Global.m_db.ClearParameter();
                   Global.m_db.AddParameter("@DocPaySlipLoanTable", SqlDbType.Structured, dtLoan);
                   Global.m_db.AddParameter("@PaySlipID", SqlDbType.Int, ReturnID);
                   Global.m_db.setCommandText("Hos.spDoctorPaySlipLoanCreate");
                   Global.m_db.setCommandType(CommandType.StoredProcedure);
                   Global.m_db.ProcessParameter();

                   // insert into salary adjustment table
                   dtSalaryAdjust.Columns.Add("PaySlipID", typeof(int), ReturnID.ToString());
                   

                   Global.m_db.ClearParameter();
                   Global.m_db.AddParameter("@SalaryAdjustmentTable", SqlDbType.Structured, dtSalaryAdjust);
                   Global.m_db.setCommandText("[Hos].[spSalaryAdjustmentCreateDoctor]");
                   Global.m_db.setCommandType(CommandType.StoredProcedure);
                   Global.m_db.ProcessParameter();
                

                   Global.m_db.CommitTransaction();
                   MessageBox.Show("Employee Salary PaySlip Created Successfully ", "PaySlip Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
               }
               catch (Exception ex)
               {
                   Global.m_db.RollBackTransaction();
                   throw ex;
                  
               }

           }

           public static DataTable getSpecilizationByName(string name)
           {
               string str = "select SpecilizationID from Hos.tblSpecilization where SpecilizationName='" + name + "'";
               return Global.m_db.SelectQry(str, "tbldesignation");
           }



           public static DataTable GetDoctorName()
           {
               string str = "select ID as DoctorID,CONCAT(FirstName,' ',MiddleName,' ',LastName) as DoctorName  from Hos.tblDoctor" ;
               return Global.m_db.SelectQry(str, "tblDoctor");
           }

    }
}
