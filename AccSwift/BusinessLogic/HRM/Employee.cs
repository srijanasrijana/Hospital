using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;
using System.Data.SqlClient;
using System.Windows.Forms;
using DateManager;
using BusinessLogic.HRM.Report;

namespace BusinessLogic.HRM
{
    public class Employee
    {
        int? doctorid=null;
        string specilization;
        public string CreateEmployee(EmployeeDetails emp, DataTable AcademicQualification, DataTable WorkExperience,DataTable Loan,DataTable Advance,out int EmployeeID)
        {
            try
            {
                //Insert The Employee Basic Information
                Global.m_db.BeginTransaction();
                Global.m_db.ClearParameter();
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("HRM.spEmployeeCreate");
                Global.m_db.AddParameter("@FirstName", SqlDbType.NVarChar, 30, emp.FirstName);
                Global.m_db.AddParameter("@LastName", SqlDbType.NVarChar, 30, emp.LastName);
                Global.m_db.AddParameter("@EmployeeCode", SqlDbType.NVarChar, 30, emp.EmployeeCode);
                Global.m_db.AddParameter("@BirthDate", SqlDbType.DateTime, emp.BirthDate);
                Global.m_db.AddParameter("@StartDate", SqlDbType.DateTime, emp.StartDate);
                Global.m_db.AddParameter("@Gender", SqlDbType.Int, emp.Gender);
                Global.m_db.AddParameter("@IsSingle", SqlDbType.Int, emp.IsSingle);
                Global.m_db.AddParameter("@EmpType", SqlDbType.NVarChar,50, emp.EmpType);
                Global.m_db.AddParameter("@IsCoupleWorking", SqlDbType.Bit, emp.IsCoupleWorking);
                Global.m_db.AddParameter("@NationalityID", SqlDbType.Int, emp.NationalityID);
                Global.m_db.AddParameter("@Phone1", SqlDbType.NVarChar, 20, emp.Phone1);
                Global.m_db.AddParameter("@Phone2", SqlDbType.NVarChar, 20, emp.Phone2);
                Global.m_db.AddParameter("@Email", SqlDbType.NVarChar, 50, emp.Email);

                Global.m_db.AddParameter("@MiddleName", SqlDbType.NVarChar, 100, emp.MiddleName);
                Global.m_db.AddParameter("@CitizenshipNo", SqlDbType.NVarChar, 200, emp.CitizenshipNumber);
                Global.m_db.AddParameter("@EthnicityID", SqlDbType.Int, emp.EthnicityID);
                Global.m_db.AddParameter("@PermAddress", SqlDbType.NVarChar, 200, emp.PermAddress);
                Global.m_db.AddParameter("@TempAddress", SqlDbType.NVarChar, 200, emp.TempAddress);
                Global.m_db.AddParameter("@PermDistID", SqlDbType.Int, emp.PermDist);
                Global.m_db.AddParameter("@PermZoneID", SqlDbType.Int, emp.PermZone);
                Global.m_db.AddParameter("@TempDistID", SqlDbType.Int, emp.TempDist);
                Global.m_db.AddParameter("@TempZoneID", SqlDbType.Int, emp.TempZone);
                Global.m_db.AddParameter("@Religion", SqlDbType.NVarChar, 50, emp.Religion);
                Global.m_db.AddParameter("@EmployeeNote", SqlDbType.NVarChar, 500, emp.EmployeeNote);
                Global.m_db.AddParameter("@FatherName", SqlDbType.NVarChar, 300, emp.FatherName);
                Global.m_db.AddParameter("@GrandfatherName", SqlDbType.NVarChar, 300, emp.GrandfatherName);
                System.Data.SqlClient.SqlParameter objReturn = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                Global.m_db.ProcessParameter();
                int ReturnID = Convert.ToInt32(objReturn.Value);
                EmployeeID = ReturnID;

                //Insert Employement Information
                Global.m_db.ClearParameter();
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("HRM.spEmploymentCreate");
                Global.m_db.AddParameter("@EmployeeID", SqlDbType.Int, ReturnID.ToString());
                Global.m_db.AddParameter("@JoinDate", SqlDbType.DateTime, emp.JoinDate);
                Global.m_db.AddParameter("@RetirementDate", SqlDbType.DateTime, emp.EndDate);//Set same for both for time being
                Global.m_db.AddParameter("@DepartmentID", SqlDbType.Int, emp.DepartmentID);
                Global.m_db.AddParameter("@DesignationID", SqlDbType.Int, emp.DesignationID);
                Global.m_db.AddParameter("@StatusID", SqlDbType.Int, emp.Status);
                Global.m_db.AddParameter("@TypeID", SqlDbType.Int, emp.Type);
                Global.m_db.AddParameter("@EmpLevel", SqlDbType.NVarChar, 50, emp.Level);
                Global.m_db.AddParameter("@Grade", SqlDbType.Int, emp.Grade);
                Global.m_db.AddParameter("@FacultyID", SqlDbType.Int, emp.FacultyID);

                System.Data.SqlClient.SqlParameter paramReturn = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                Global.m_db.ProcessParameter();
                if (paramReturn.Value.ToString() != "SUCCESS")
                {
                    Global.m_db.RollBackTransaction();
                    throw new Exception("Unable to create Employee Information");
                }
                //For Academic Qualification
                for (int i = 0; i < AcademicQualification.Rows.Count; i++)
                {
                    DataRow drqualification = AcademicQualification.Rows[i];
                    Global.m_db.ClearParameter();
                    Global.m_db.setCommandType(CommandType.StoredProcedure);
                    Global.m_db.setCommandText("HRM.spQualificationCreate");
                    Global.m_db.AddParameter("@EmployeeID", SqlDbType.Int, ReturnID.ToString());
                    Global.m_db.AddParameter("@InstituteName", SqlDbType.NVarChar, 200, drqualification["InstituteName"].ToString());
                    Global.m_db.AddParameter("@Board", SqlDbType.NVarChar, 200, drqualification["Board"].ToString());//Set same for both for time being
                    Global.m_db.AddParameter("@Course", SqlDbType.NVarChar, 200, drqualification["Course"].ToString());
                    Global.m_db.AddParameter("@Percentage", SqlDbType.NVarChar, 20, drqualification["Percentage"].ToString());
                    Global.m_db.AddParameter("@PassYear", SqlDbType.Int, drqualification["PassYear"].ToString());
                    System.Data.SqlClient.SqlParameter paramReturn1 = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                    Global.m_db.ProcessParameter();
                    if (paramReturn1.Value.ToString() != "SUCCESS")
                    {
                        Global.m_db.RollBackTransaction();
                        throw new Exception("Unable to create Employee Qualification");
                    }
                }
                //For Work Experiences
                for (int i = 0; i < WorkExperience.Rows.Count; i++)
                {
                    DataRow drexperience = WorkExperience.Rows[i];
                    Global.m_db.ClearParameter();
                    Global.m_db.setCommandType(CommandType.StoredProcedure);
                    Global.m_db.setCommandText("HRM.spExperienceCreate");
                    Global.m_db.AddParameter("@EmployeeID", SqlDbType.Int, ReturnID.ToString());
                    Global.m_db.AddParameter("@DoctorID", SqlDbType.Int, doctorid);
                    Global.m_db.AddParameter("@CompanyName", SqlDbType.NVarChar, 100, drexperience["CompanyName"].ToString());
                    Global.m_db.AddParameter("@FromDate", SqlDbType.DateTime, drexperience["FromDate"].ToString());//Set same for both for time being
                    Global.m_db.AddParameter("@ToDate", SqlDbType.DateTime,drexperience["ToDate"].ToString());
                    Global.m_db.AddParameter("@Designation", SqlDbType.NVarChar, 20, drexperience["Designation"].ToString());
                    Global.m_db.AddParameter("@Specilization", SqlDbType.NVarChar, 20,specilization);
                    System.Data.SqlClient.SqlParameter paramReturn2 = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                    
                    Global.m_db.ProcessParameter();
                    if (paramReturn2.Value.ToString() != "SUCCESS")
                    {
                        Global.m_db.RollBackTransaction();
                        throw new Exception("Unable to create Employee Experience");
                    }
                 
                }
               
                //For Employee Photo
                Global.m_db.ClearParameter();
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("HRM.spEmployeePhotoCreate");
                Global.m_db.AddParameter("@EmployeeID", SqlDbType.Int, ReturnID.ToString());
                Global.m_db.AddParameter("@EmployeePhoto", SqlDbType.Image, emp.EmployeePhoto);
                System.Data.SqlClient.SqlParameter paramReturn3 = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                Global.m_db.ProcessParameter();
                if (paramReturn3.Value.ToString() != "SUCCESS")
                {
                    Global.m_db.RollBackTransaction();
                    throw new Exception("Unable to create Employee Photo");
                }
                //Insert EmployeeSalary INformation
                Global.m_db.ClearParameter();
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("HRM.spEmployeeSalaryCreate");
                Global.m_db.AddParameter("@EmployeeID", SqlDbType.Int, ReturnID.ToString());
                Global.m_db.AddParameter("@DoctorID", SqlDbType.Int, doctorid);

                Global.m_db.AddParameter("@StartingSalary", SqlDbType.Decimal, emp.StartingSalary);
                Global.m_db.AddParameter("@Adjusted", SqlDbType.Decimal, emp.Adjusted);
                Global.m_db.AddParameter("@IsPF", SqlDbType.Bit, emp.IsPF);
                Global.m_db.AddParameter("@PFNumber", SqlDbType.Int, emp.PFNumber);
                Global.m_db.AddParameter("@CIFNumber", SqlDbType.Int, emp.CIFNumber);
                Global.m_db.AddParameter("@CITAmount", SqlDbType.Decimal, emp.CITAmount);
                Global.m_db.AddParameter("@BankID", SqlDbType.Int, emp.BankID);
                Global.m_db.AddParameter("@BankACNumber", SqlDbType.NVarChar, 20, emp.ACNumber);
                Global.m_db.AddParameter("@InsurancePremium", SqlDbType.Decimal, emp.InsurancePremium);
                Global.m_db.AddParameter("@BasicSalary", SqlDbType.Decimal, emp.BasicSalary);
                Global.m_db.AddParameter("@PAN", SqlDbType.NVarChar, 20, emp.PAN);
                
                Global.m_db.AddParameter("@TADA", SqlDbType.Decimal, emp.TADA);
                Global.m_db.AddParameter("@MiscAllowance", SqlDbType.Decimal, emp.MiscAllowance);
                Global.m_db.AddParameter("@NLKoshDeduct", SqlDbType.Decimal, emp.NLKoshDeduct);
                Global.m_db.AddParameter("@NLKoshNo", SqlDbType.NVarChar, emp.NLKoshNo);
                Global.m_db.AddParameter("@Remarks", SqlDbType.NVarChar, 200, emp.Remarks);
                
                Global.m_db.AddParameter("@Grade", SqlDbType.Int, emp.Grade);

                Global.m_db.AddParameter("@GradeIncrementDate", SqlDbType.DateTime, emp.GradeIncrementDate);

                Global.m_db.AddParameter("@isPension", SqlDbType.Bit, emp.IsPension);
                Global.m_db.AddParameter("@PensionNumber", SqlDbType.NVarChar, 100, emp.PensionNumber);
                Global.m_db.AddParameter("@isInsurance", SqlDbType.Bit, emp.IsInsurance);
                Global.m_db.AddParameter("@InsuranceNumber", SqlDbType.NVarChar, 100, emp.InsuranceNumber);
                Global.m_db.AddParameter("@InsuranceAmount", SqlDbType.Decimal, emp.InsuranceAmt);
                Global.m_db.AddParameter("@PensionAdjusted", SqlDbType.Decimal, emp.PensionAdjust);
                Global.m_db.AddParameter("@PFAdjusted", SqlDbType.Decimal, emp.PFAdjust);

                Global.m_db.AddParameter("@InflationAlw", SqlDbType.Decimal, emp.inflationAlw);
                Global.m_db.AddParameter("@AdmistrativeAlw", SqlDbType.Decimal, emp.AdmAlw);
                Global.m_db.AddParameter("@AcademicAlw", SqlDbType.Decimal, emp.AcademicAlw);
                Global.m_db.AddParameter("@PostAlw", SqlDbType.Decimal, emp.PostAlw);
                
                Global.m_db.AddParameter("@Level", SqlDbType.Int, emp.Level);
                Global.m_db.AddParameter("@DepartmentID", SqlDbType.Int, emp.DepartmentID);
                Global.m_db.AddParameter("@DesignationID", SqlDbType.Int, emp.DesignationID);

                Global.m_db.AddParameter("@isQuarter", SqlDbType.Bit, emp.IsQuarter);
                Global.m_db.AddParameter("@Accommodation", SqlDbType.Decimal, emp.Accommodation);
                Global.m_db.AddParameter("@ElectricityCharge", SqlDbType.Decimal, emp.ElectricityCharge);

             
                Global.m_db.AddParameter("@KalyankariNo", SqlDbType.NVarChar, 50, emp.KalyankariNo);
                Global.m_db.AddParameter("@KalyankariAmt", SqlDbType.Decimal, emp.KalyankariAmt);
                Global.m_db.AddParameter("@FacultyID", SqlDbType.Int, emp.FacultyID);
                Global.m_db.AddParameter("@OverTimeAlw", SqlDbType.Decimal, emp.OverTimeAllow);
                System.Data.SqlClient.SqlParameter paramReturnsalary = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                Global.m_db.ProcessParameter();
                if (paramReturnsalary.Value.ToString() != "SUCCESS")
                {
                    Global.m_db.RollBackTransaction();
                    throw new Exception("Unable to create Employee Salary");
                }

                //For Loan
                for (int i = 0; i < Loan.Rows.Count; i++)
                {
                    DataRow dr = Loan.Rows[i];
                    Global.m_db.ClearParameter();
                    Global.m_db.setCommandType(CommandType.Text);
                    Global.m_db.setCommandText("Insert into HRM.tblEmployeeLoan(EmpID,LoanID,LoanPrincipal,LoanMthInstallment,LoanMthInterest,LoanTotalMth,LoanRemainingMth,LoanStartDate,LoanEndDate,LoanMthPremium,LoanMthDecreaseAmt,InitialInstallment) values(@EmpID,@LoanID,@LoanPrincipal,@LoanMthInstallment,@LoanMthInterest,@LoanTotalMth,@LoanRemainingMth,@LoanStartDate,@LoanEndDate,@LoanMthPremium,@LoanMthDecreaseAmt,@LoanMthInstallment)");
                    Global.m_db.AddParameter("@EmpID", SqlDbType.Int, ReturnID.ToString());
                 //   Global.m_db.AddParameter("@DoctorID", SqlDbType.Int,doctorid.ToString());
                    
                    Global.m_db.AddParameter("@LoanID", SqlDbType.Int, Convert.ToInt32(dr["LoanID"].ToString()));
                    Global.m_db.AddParameter("@LoanPrincipal", SqlDbType.Decimal, Convert.ToDecimal(dr["LoanPrincipal"].ToString()));//Set same for both for time being
                    Global.m_db.AddParameter("@LoanMthInstallment", SqlDbType.Decimal, Convert.ToDecimal(dr["LoanMthInstallment"].ToString()));
                    Global.m_db.AddParameter("@LoanMthInterest", SqlDbType.Decimal, dr["LoanMthInterest"].ToString());
                    Global.m_db.AddParameter("@LoanTotalMth", SqlDbType.Int,Convert.ToInt32(dr["LoanTotalMth"].ToString()));
                    Global.m_db.AddParameter("@LoanRemainingMth", SqlDbType.Int,Convert.ToInt32(dr["LoanRemainingMth"].ToString()));
                    Global.m_db.AddParameter("@LoanStartDate", SqlDbType.DateTime, Date.ToDotNet(dr["LoanStartDate"].ToString()));
                    Global.m_db.AddParameter("@LoanEndDate", SqlDbType.DateTime, Date.ToDotNet(dr["LoanEndDate"].ToString()));
                    Global.m_db.AddParameter("@LoanMthPremium", SqlDbType.Decimal,Convert.ToDecimal(dr["LoanMthPremium"].ToString()));
                    Global.m_db.AddParameter("@LoanMthDecreaseAmt", SqlDbType.Decimal,Convert.ToDecimal(dr["LoanMthDecreaseAmt"].ToString()));
                    Global.m_db.ProcessParameter();
                }

                //For Advance
                for (int i = 0; i < Advance.Rows.Count; i++)
                {
                    DataRow dr = Advance.Rows[i];
                    Global.m_db.ClearParameter();
                    Global.m_db.setCommandType(CommandType.Text);
                    Global.m_db.setCommandText("Insert into HRM.tblEmployeeAdvance (EmpID,AdvTitle,TotalAmt,Installment,TakenDate,ReturnDate,RemainingAmt) values(@EmpID,@AdvTitle,@TotalAmt,@Installment,@TakenDate,@ReturnDate,@RemainingAmt)");
                    Global.m_db.AddParameter("@EmpID", SqlDbType.Int, ReturnID.ToString());
                    //Global.m_db.AddParameter("@DoctorID", SqlDbType.Int, doctorid);
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
                MessageBox.Show("Employee Information Saved Successfully", "Employee Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return "SUCCESS";
            }
            catch (Exception ex)
            {
                Global.m_db.RollBackTransaction();
                MessageBox.Show(ex.Message);
                EmployeeID = -1;
                
                return "FAILURE";
            }
        }

        public string UpdateEmployee(EmployeeDetails emp, DataTable AcademicQualification, DataTable WorkExperience, bool ispicupdate,DataTable Loan,DataTable Advance)
        {
            try
            {

                Global.m_db.BeginTransaction();
                Global.m_db.ClearParameter();
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("HRM.spEmployeeModify");
                Global.m_db.AddParameter("@EmployeeID", SqlDbType.Int, emp.EmployeeID);
                Global.m_db.AddParameter("@FirstName", SqlDbType.NVarChar, 30, emp.FirstName);
                Global.m_db.AddParameter("@LastName", SqlDbType.NVarChar, 30, emp.LastName);
                Global.m_db.AddParameter("@EmployeeCode", SqlDbType.NVarChar, 30, emp.EmployeeCode);
                Global.m_db.AddParameter("@BirthDate", SqlDbType.DateTime, emp.BirthDate);
                Global.m_db.AddParameter("@StartDate", SqlDbType.DateTime, emp.StartDate);
                Global.m_db.AddParameter("@Gender", SqlDbType.Int, emp.Gender);
                Global.m_db.AddParameter("@IsSingle", SqlDbType.Int, emp.IsSingle);
                Global.m_db.AddParameter("@EmpType", SqlDbType.NVarChar, 50, emp.EmpType);
                Global.m_db.AddParameter("@IsCoupleWorking", SqlDbType.Bit, emp.IsCoupleWorking);
                Global.m_db.AddParameter("@NationalityID", SqlDbType.Int, emp.NationalityID);
                Global.m_db.AddParameter("@Phone1", SqlDbType.NVarChar, 20, emp.Phone1);
                Global.m_db.AddParameter("@Phone2", SqlDbType.NVarChar, 20, emp.Phone2);
                Global.m_db.AddParameter("@Email", SqlDbType.NVarChar, 50, emp.Email);
                Global.m_db.AddParameter("@MiddleName", SqlDbType.NVarChar, 100, emp.MiddleName);
                Global.m_db.AddParameter("@CitizenshipNo", SqlDbType.NVarChar, 200, emp.CitizenshipNumber);
                Global.m_db.AddParameter("@EthnicityID", SqlDbType.Int, emp.EthnicityID);
                Global.m_db.AddParameter("@PermAddress", SqlDbType.NVarChar, 200, emp.PermAddress);
                Global.m_db.AddParameter("@TempAddress", SqlDbType.NVarChar, 200, emp.TempAddress);
                Global.m_db.AddParameter("@PermDistID", SqlDbType.Int, emp.PermDist);
                Global.m_db.AddParameter("@PermZoneID", SqlDbType.Int, emp.PermZone);
                Global.m_db.AddParameter("@TempDistID", SqlDbType.Int, emp.TempDist);
                Global.m_db.AddParameter("@TempZoneID", SqlDbType.Int, emp.TempZone);
                Global.m_db.AddParameter("@Religion", SqlDbType.NVarChar, 50, emp.Religion);
                Global.m_db.AddParameter("@EmployeeNote", SqlDbType.NVarChar, 500, emp.EmployeeNote);
                Global.m_db.AddParameter("@FatherName", SqlDbType.NVarChar, 300, emp.FatherName);
                Global.m_db.AddParameter("@GrandfatherName", SqlDbType.NVarChar, 300, emp.GrandfatherName);
                System.Data.SqlClient.SqlParameter objReturn = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                Global.m_db.ProcessParameter();
                if (objReturn.Value.ToString() != "SUCCESS")
                {
                    Global.m_db.RollBackTransaction();
                    throw new Exception("Unable to Modify Employee Information");
                }

                //Insert Employement Information
                if (emp.IsEmpDetailsChanged)
                {
                    Global.m_db.ClearParameter();
                    Global.m_db.setCommandType(CommandType.StoredProcedure);
                    Global.m_db.setCommandText("HRM.spEmploymentCreate");
                    Global.m_db.AddParameter("@EmployeeID", SqlDbType.Int, emp.EmployeeID);
                    Global.m_db.AddParameter("@JoinDate", SqlDbType.DateTime, emp.JoinDate);
                    Global.m_db.AddParameter("@RetirementDate", SqlDbType.DateTime, emp.EndDate);//Set same for both for time being
                    Global.m_db.AddParameter("@DepartmentID", SqlDbType.Int, emp.DepartmentID);
                    Global.m_db.AddParameter("@DesignationID", SqlDbType.Int, emp.DesignationID);
                    Global.m_db.AddParameter("@StatusID", SqlDbType.Int, emp.Status);
                    Global.m_db.AddParameter("@TypeID", SqlDbType.Int, emp.Type);
                    Global.m_db.AddParameter("@EmpLevel", SqlDbType.NVarChar, 50, emp.Level);
                    Global.m_db.AddParameter("@Grade", SqlDbType.Int, emp.Grade);
                    Global.m_db.AddParameter("@FacultyID", SqlDbType.Int, emp.FacultyID);

                    System.Data.SqlClient.SqlParameter paramReturn = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                    Global.m_db.ProcessParameter();
                    if (paramReturn.Value.ToString() != "SUCCESS")
                    {
                        Global.m_db.RollBackTransaction();
                        throw new Exception("Unable to create Employee Information");
                    } 
                }

                //First delete the old Qualification Details
                Global.m_db.InsertUpdateQry("DELETE FROM HRM.tblAcademicQualification WHERE EmployeeID='" + emp.EmployeeID + "'");

                //For Academic Qualification
                for (int i = 0; i < AcademicQualification.Rows.Count; i++)
                {
                    DataRow drqualification = AcademicQualification.Rows[i];
                    Global.m_db.ClearParameter();
                    Global.m_db.setCommandType(CommandType.StoredProcedure);
                    Global.m_db.setCommandText("HRM.spQualificationCreate");
                    Global.m_db.AddParameter("@EmployeeID", SqlDbType.Int, emp.EmployeeID);
                    Global.m_db.AddParameter("@InstituteName", SqlDbType.NVarChar, 200, drqualification["InstituteName"].ToString());
                    Global.m_db.AddParameter("@Board", SqlDbType.NVarChar, 200, drqualification["Board"].ToString());//Set same for both for time being
                    Global.m_db.AddParameter("@Course", SqlDbType.NVarChar, 200, drqualification["Course"].ToString());
                    Global.m_db.AddParameter("@Percentage", SqlDbType.NVarChar, 20, drqualification["Percentage"].ToString());
                    Global.m_db.AddParameter("@PassYear", SqlDbType.Int, drqualification["PassYear"].ToString());
                    System.Data.SqlClient.SqlParameter paramReturn1 = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                    Global.m_db.ProcessParameter();
                    if (paramReturn1.Value.ToString() != "SUCCESS")
                    {
                        Global.m_db.RollBackTransaction();
                        throw new Exception("Unable to Update Employee Qualification");
                    }
                }

                //First delete the old Work Experiences Details
                Global.m_db.InsertUpdateQry("DELETE FROM HRM.tblWorkExperiences WHERE EmployeeID='" + emp.EmployeeID + "'");
                for (int i = 0; i < WorkExperience.Rows.Count; i++)
                {
                    DataRow drexperience = WorkExperience.Rows[i];
                    Global.m_db.ClearParameter();
                    Global.m_db.setCommandType(CommandType.StoredProcedure);
                    Global.m_db.setCommandText("HRM.spExperienceCreate");
                    Global.m_db.AddParameter("@EmployeeID", SqlDbType.Int, emp.EmployeeID);
                    Global.m_db.AddParameter("@DoctorID", SqlDbType.Int, doctorid);
                    Global.m_db.AddParameter("@CompanyName", SqlDbType.NVarChar, 100, drexperience["CompanyName"].ToString());
                    Global.m_db.AddParameter("@FromDate", SqlDbType.DateTime, drexperience["FromDate"].ToString());//Set same for both for time being
                    Global.m_db.AddParameter("@ToDate", SqlDbType.DateTime, drexperience["ToDate"].ToString());
                    Global.m_db.AddParameter("@Designation", SqlDbType.NVarChar, 20, drexperience["Designation"].ToString());
                    System.Data.SqlClient.SqlParameter paramReturn2 = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                 
                    Global.m_db.ProcessParameter();
                    if (paramReturn2.Value.ToString() != "SUCCESS")
                    {
                        Global.m_db.RollBackTransaction();
                        throw new Exception("Unable to Update Employee Experience");
                    }
                }


                if (ispicupdate == true)
                {//For Employee Photo
                    Global.m_db.ClearParameter();
                    Global.m_db.setCommandType(CommandType.StoredProcedure);
                    Global.m_db.setCommandText("HRM.spEmployeePhotoUpdate");
                    Global.m_db.AddParameter("@EmployeeID", SqlDbType.Int, emp.EmployeeID);
                    Global.m_db.AddParameter("@EmployeePhoto", SqlDbType.Image, emp.EmployeePhoto);
                    System.Data.SqlClient.SqlParameter paramReturn3 = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                    Global.m_db.ProcessParameter();
                    if (paramReturn3.Value.ToString() != "SUCCESS")
                    {
                        Global.m_db.RollBackTransaction();
                        throw new Exception("Unable to Update Employee Photo");
                    }
                }

                //Insert EmployeeSalary INformation
                Global.m_db.ClearParameter();
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("HRM.spEmployeeSalaryModify");
                Global.m_db.AddParameter("@EmployeeID", SqlDbType.Int, emp.EmployeeID);
                Global.m_db.AddParameter("@DoctorID", SqlDbType.Int, doctorid);
                Global.m_db.AddParameter("@StartingSalary", SqlDbType.Decimal, emp.StartingSalary);
                Global.m_db.AddParameter("@Adjusted", SqlDbType.Decimal, emp.Adjusted);
                Global.m_db.AddParameter("@IsPF", SqlDbType.Bit, emp.IsPF);
                Global.m_db.AddParameter("@PFNumber", SqlDbType.Int, emp.PFNumber);
                Global.m_db.AddParameter("@BankID", SqlDbType.Int, emp.BankID);
                Global.m_db.AddParameter("@BankACNumber", SqlDbType.NVarChar, 20, emp.ACNumber);
                Global.m_db.AddParameter("@InsurancePremium", SqlDbType.Decimal, emp.InsurancePremium);
                Global.m_db.AddParameter("@BasicSalary", SqlDbType.Decimal, emp.BasicSalary);
                Global.m_db.AddParameter("@PAN", SqlDbType.NVarChar, 20, emp.PAN);
                
                Global.m_db.AddParameter("@NLKoshDeduct", SqlDbType.Decimal, emp.NLKoshDeduct);
                Global.m_db.AddParameter("@NLKoshNo", SqlDbType.NVarChar, 50, emp.NLKoshNo);
                Global.m_db.AddParameter("@Remarks", SqlDbType.NVarChar, 200, emp.Remarks);
                Global.m_db.AddParameter("@Grade", SqlDbType.Int, emp.Grade);

                Global.m_db.AddParameter("@GradeIncrementDate", SqlDbType.DateTime, emp.GradeIncrementDate);
                Global.m_db.AddParameter("@PensionAdjusted", SqlDbType.Decimal, emp.PensionAdjust);
                Global.m_db.AddParameter("@PFAdjusted", SqlDbType.Decimal, emp.PFAdjust);

                Global.m_db.AddParameter("@isPension", SqlDbType.Bit, emp.IsPension);
                Global.m_db.AddParameter("@PensionNumber", SqlDbType.NVarChar, 100, emp.PensionNumber);
                Global.m_db.AddParameter("@isInsurance", SqlDbType.Bit, emp.IsInsurance);
                Global.m_db.AddParameter("@InsuranceNumber", SqlDbType.NVarChar, 100, emp.InsuranceNumber);
                Global.m_db.AddParameter("@InsuranceAmount", SqlDbType.Decimal, emp.InsuranceAmt);
                Global.m_db.AddParameter("@InflationAlw", SqlDbType.Decimal, emp.inflationAlw);
                Global.m_db.AddParameter("@AdmistrativeAlw", SqlDbType.Decimal, emp.AdmAlw);
                Global.m_db.AddParameter("@AcademicAlw", SqlDbType.Decimal, emp.AcademicAlw);
                Global.m_db.AddParameter("@PostAlw", SqlDbType.Decimal, emp.PostAlw);
               
                Global.m_db.AddParameter("@Level", SqlDbType.Int, emp.Level);
                Global.m_db.AddParameter("@DepartmentID", SqlDbType.Int, emp.DepartmentID);
                Global.m_db.AddParameter("@DesignationID", SqlDbType.Int, emp.DesignationID);

                Global.m_db.AddParameter("@isQuarter", SqlDbType.Bit, emp.IsQuarter);
                Global.m_db.AddParameter("@Accommodation", SqlDbType.Decimal, emp.Accommodation);
                Global.m_db.AddParameter("@ElectricityCharge", SqlDbType.Decimal, emp.ElectricityCharge);

                //Global.m_db.AddParameter("@isLoan", SqlDbType.Bit, emp.IsLoan);
                //Global.m_db.AddParameter("@isAdvance", SqlDbType.Bit, emp.IsAdvance);
                //Global.m_db.AddParameter("@LoanID", SqlDbType.Int, emp.LoanID);
                //Global.m_db.AddParameter("@LoanPrincipal", SqlDbType.Decimal, emp.LoanPrincipal);
                //Global.m_db.AddParameter("@LoanMthPremium", SqlDbType.Decimal, emp.LoanPremium);
                //Global.m_db.AddParameter("@LoanMthInstallment", SqlDbType.Decimal, emp.LoanMthInstallment);
                //Global.m_db.AddParameter("@LoanMthInterest", SqlDbType.Decimal, emp.LoanMthInterest);
                //Global.m_db.AddParameter("@LoanMthDecreaseAmt", SqlDbType.Decimal, emp.LoanMthDecrease);
                //Global.m_db.AddParameter("@AdvAmt", SqlDbType.Decimal, emp.AdvAmt);
                //Global.m_db.AddParameter("@AdvMthInstallment", SqlDbType.Decimal, emp.AdvMthInstallment);
                //Global.m_db.AddParameter("@AdvRemainingAmt", SqlDbType.Decimal, emp.AdvRemainingAmt);
                //Global.m_db.AddParameter("@LoanTotalMth", SqlDbType.Int, emp.LoanDuration);
                //Global.m_db.AddParameter("@LoanRemainingMth", SqlDbType.Int, emp.LoanRemainingMth);
                //Global.m_db.AddParameter("@LoanEndDate", SqlDbType.DateTime, emp.LoanEndDate);
                //Global.m_db.AddParameter("@LoanStartDate", SqlDbType.DateTime, emp.LoanStartDate);
                //Global.m_db.AddParameter("@AdvTakenDate", SqlDbType.DateTime, emp.AdvStartDate);
                //Global.m_db.AddParameter("@AdvReturnDate", SqlDbType.DateTime, emp.AdvEndDate);
                Global.m_db.AddParameter("@KalyankariNo", SqlDbType.NVarChar, 50, emp.KalyankariNo);
                Global.m_db.AddParameter("@KalyankariAmt", SqlDbType.Decimal, emp.KalyankariAmt);
                Global.m_db.AddParameter("@FacultyID", SqlDbType.Int, emp.FacultyID);
                Global.m_db.AddParameter("@OverTimeAlw", SqlDbType.Decimal, emp.OverTimeAllow);
                System.Data.SqlClient.SqlParameter paramReturnsalary = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                Global.m_db.ProcessParameter();
                if (paramReturnsalary.Value.ToString() != "SUCCESS")
                {
                    Global.m_db.RollBackTransaction();
                    throw new Exception("Unable to create Employee Salary");
                }

                //For Loan
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
                Global.m_db.InsertUpdateQry("Delete from HRM.tblEmployeeLoan where EmpID = " + emp.EmployeeID + (ELIDs.Length > 0 ? " and ELID not in(" + ELIDs + ")" : ""));


                //Global.m_db.InsertUpdateQry("DELETE FROM HRM.tblEmployeeLoan WHERE EmpID='" + emp.EmployeeID + "'");
                for (int i = 0; i < Loan.Rows.Count; i++)
                {
                    DataRow dr = Loan.Rows[i];

                    if (Convert.ToInt32(dr["ELID"]) == 0)
                    {
                        Global.m_db.ClearParameter();
                        Global.m_db.setCommandType(CommandType.Text);
                        Global.m_db.setCommandText("Insert into HRM.tblEmployeeLoan (EmpID,LoanID,LoanPrincipal,LoanMthInstallment,LoanMthInterest,LoanTotalMth,LoanRemainingMth,LoanStartDate,LoanEndDate,LoanMthPremium,LoanMthDecreaseAmt,InitialInstallment) values (@EmpID,@LoanID,@LoanPrincipal,@LoanMthInstallment,@LoanMthInterest,@LoanTotalMth,@LoanRemainingMth,@LoanStartDate,@LoanEndDate,@LoanMthPremium,@LoanMthDecreaseAmt,@InitialInstallment)");
                        Global.m_db.AddParameter("@EmpID", SqlDbType.Int, emp.EmployeeID);
                   //     Global.m_db.AddParameter("@DoctorID", SqlDbType.Int, doctorid.ToString());
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

                //For Advance
                Global.m_db.InsertUpdateQry("DELETE FROM HRM.tblEmployeeAdvance WHERE EmpID='" + emp.EmployeeID + "'");
                for (int i = 0; i < Advance.Rows.Count; i++)
                {
                    DataRow dr = Advance.Rows[i];
                    Global.m_db.ClearParameter();
                    Global.m_db.setCommandType(CommandType.Text);
                    Global.m_db.setCommandText("Insert into HRM.tblEmployeeAdvance (EmpID,AdvTitle,TotalAmt,Installment,TakenDate,ReturnDate,RemainingAmt) values (@EmpID,@AdvTitle,@TotalAmt,@Installment,@TakenDate,@ReturnDate,@RemainingAmt)");
                    Global.m_db.AddParameter("@EmpID", SqlDbType.Int, emp.EmployeeID);
                 // Global.m_db.AddParameter("@DoctorID", SqlDbType.Int, doctorid.ToString());
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
                MessageBox.Show("Employee Information Updated Successfully", "Employee Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return "SUCCESS";
            }
            catch (SqlException ex)
            {
                Global.m_db.RollBackTransaction();
                MessageBox.Show(ex.Message + ex.LineNumber);
                return "FAILURE";
            }
        }

        public void ClearImage(EmployeeDetails emp)
        {
            try
            {
                Global.m_db.InsertUpdateQry("DELETE FROM HRM.tblEmployeePhoto WHERE EmployeeID='" + emp.EmployeeID + "'");
                MessageBox.Show("Photo Deleted Successfully");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public int UpdateEmployeeLedgerID(int ledgerID,int employeeID)
        {
            return Global.m_db.InsertUpdateQry("Update HRM.tblEmployee set LedgerID = '" + ledgerID + "' where ID = '" + employeeID + "'");
        }

        public int GetEmployeeLedgerID(int employeeID)
        {
            object result = Global.m_db.GetScalarValue("select LedgerID from HRM.tblEmployee where ID = '" + employeeID + "'");
            return Convert.ToInt32(result);
        }
        public DataTable EmployeeDetails(string filter)
        {
            string str = "select ID, StaffCode, CONCAT( FirstName,' ',MiddleName,' ',LastName) as EmployeeName from Hrm.tblEmployee "+filter;
            return Global.m_db.SelectQry(str, "tblEmployee");
        }

        public DataTable FillEmployeeDetails(int EmpID)
        {
            //string str = "select E.*,c.NationalityName,P.EmployeePhoto,a.*,ES.*,pd.DistrictName as PermDistName,td.DistrictName as TempDistName,pz.ZoneName as PermZoneName,tz.ZoneName as TempZoneName,el.LevelName,eth.EthnicityName ,loan.LoanName,loan.LoanType from Hrm.tblEmployee E,System.tblNationality c,hrm.tblemployeephoto P,System.tblDistrict pd,System.tblDistrict td,System.tblZone pz,System.tblZone tz,HRM.tblEmployeeLevel el,System.tblEthnicity eth, (select top 1 Ed.EmployeementDetailsID, ED.JoinDate,RetirementDate,D.DepartmentName,Ds.DesignationName,ef.FacultyName,Type,Status,EmpLevel,Grade from HRM.tblEmployeementDetails ED,HRM.tbldepartment D,HRM.tbldesignation Ds,HRM.tblEmployeeFaculty ef where employeeID='" + EmpID + "' and  ED.Department=D.DepartmentID and ED.Designation=Ds.DesignationID and ED.FacultyID = ef.FID order by employeementdetailsid desc)a,Hrm.tblEmployeeSalaryInfo ES,HRM.tblLoan loan where  E.ID='" + EmpID + "' and E.NationalityID=C.NationalityID and E.ID=P.EmployeeID and ES.EmployeeID=E.ID and e.PermDistID = pd.DistrictID and e.TempDistID = td.DistrictID and e.PermZoneID = pz.ZoneID and e.TempZoneID = tz.ZoneID and ES.EmpLevel = el.LevelID and e.EthnicityID = eth.EthnicityID and es.LoanID = loan.LoanID";
            string str = "select E.*,c.NationalityName,P.EmployeePhoto,ES.*,pd.DistrictName as PermDistName,td.DistrictName as TempDistName,pz.ZoneName as PermZoneName,tz.ZoneName as TempZoneName,el.LevelName,eth.EthnicityName from Hrm.tblEmployee E,System.tblNationality c,hrm.tblemployeephoto P,System.tblDistrict pd,System.tblDistrict td,System.tblZone pz,System.tblZone tz,HRM.tblEmployeeLevel el,System.tblEthnicity eth,Hrm.tblEmployeeSalaryInfo ES where  E.ID='" + EmpID + "' and E.NationalityID=C.NationalityID and E.ID=P.EmployeeID and ES.EmployeeID=E.ID and e.PermDistID = pd.DistrictID and e.TempDistID = td.DistrictID and e.PermZoneID = pz.ZoneID and e.TempZoneID = tz.ZoneID and ES.Level = el.LevelID and e.EthnicityID = eth.EthnicityID";
            return Global.m_db.SelectQry(str, "tblEmployee");
        }

        public DataTable GetEmploymentDetails(int EmpID)
        {
            string str = " select top 1 Ed.EmployeementDetailsID, ED.JoinDate,RetirementDate,D.DepartmentName,Ds.DesignationName,ef.FacultyName,Type,Status,EmpLevel,Grade from HRM.tblEmployeementDetails ED,HRM.tbldepartment D,HRM.tbldesignation Ds,HRM.tblEmployeeFaculty ef where Ed.EmployeeID='" + EmpID + "' and  ED.Department=D.DepartmentID and ED.Designation=Ds.DesignationID and ED.FacultyID = ef.FID order by employeementdetailsid desc";
            return Global.m_db.SelectQry(str, "tblEmployee");

        }
        public DataTable GetListOfEmployee(string filterString)
        {
            string str = "select E.ID,E.StaffCode,CONCAT(FirstName,' ',MiddleName,' ',LastName) as EmployeeName,a.*,e.EmpType,el.LevelName,eth.EthnicityName,ES.BankACNumber from Hrm.tblEmployee E,HRM.tblEmployeeLevel el,System.tblEthnicity eth, (select top 1 Ed.EmployeementDetailsID, ED.JoinDate,RetirementDate,ED.Department,D.DepartmentName,ED.Designation,Ds.DesignationName,ED.FacultyID,ef.FacultyName,Type,Status,EmpLevel,Grade from HRM.tblEmployeementDetails ED,HRM.tbldepartment D,HRM.tbldesignation Ds,HRM.tblEmployeeFaculty ef where ED.Department=D.DepartmentID and ED.Designation=Ds.DesignationID and ED.FacultyID = ef.FID order by employeementdetailsid desc)a,Hrm.tblEmployeeSalaryInfo ES where ES.EmployeeID=E.ID and ES.Level = el.LevelID and e.EthnicityID = eth.EthnicityID" + filterString;
            return Global.m_db.SelectQry(str, "tblEmployee");
        }

        public DataTable EmployeeQualification(int EmpID)
        {
            string str = "select * from HRM.tblAcademicQualification where EmployeeID='" + EmpID + "'";
            return Global.m_db.SelectQry(str, "tblqualification");
        }

        public DataTable EmployeeExperience(int EmpID)
        {
            string str = "select * from HRM.tblWorkExperiences where EmployeeID='" + EmpID + "'";
            return Global.m_db.SelectQry(str, "tblExperience");
        }

        public DataTable GetEmployeeLoan(int EmpID, bool isLoanForPaySlip = false)
        {
            string strForPaySlip = " ";
            if (isLoanForPaySlip)
            {
                strForPaySlip = " and EL.LoanRemainingMth > 0";
            }
            string str = "select EL.*,L.LoanName,L.LoanType from HRM.tblEmployeeLoan EL left join HRM.tblLoan L on El.LoanID = L.LoanID where EL.EmpID='" + EmpID + "' "+strForPaySlip;
            return Global.m_db.SelectQry(str, "tblEmployeeLoan");
        }

        public DataTable GetEmployeeAdvance(int EmpID)
        {
            string str = "select * from HRM.tblEmployeeAdvance where EmpID='" + EmpID + "'";
            return Global.m_db.SelectQry(str, "tblEmployeeAdvance");
        }

        public DataTable JobHistory(int EmpID)
        {
            string str = "select distinct ED.JoinDate,RetirementDate,D.DepartmentName,Ds.DesignationName,EF.FacultyName, Type,Status,ED.EmployeementDetailsID from HRM.tblEmployeementDetails ED,HRM.tbldepartment D,HRM.tbldesignation Ds,HRM.tblEmployeeFaculty EF where  ED.Department=D.DepartmentID and ED.Designation=Ds.DesignationID and ED.FacultyID = EF.FID and ED.EmployeeID='" + EmpID + "' order by RetirementDate desc";
            return Global.m_db.SelectQry(str, "tblExperience");
        }

        public int CurrentJobHistoryID(int EmpID)
        {
            string str = "select Top 1 EmployeementDetailsID from  HRM.tblEmployeementDetails where EmployeeID = '" + EmpID + "' order by EmployeementDetailsID desc";
            return Convert.ToInt32(Global.m_db.GetScalarValue(str));
        }

        public int DeleteJobHistory(int jhID)
        {
            string str = "Delete from HRM.tblEmployeementDetails where EmployeementDetailsID = '"+jhID+"'";
            return Global.m_db.InsertUpdateQry(str);
        }
        public DataTable searchEmployeeDetails()
        {
            string str = "select E.*,c.NationalityName,P.EmployeePhoto,a.*  from Hrm.tblEmployee E,HRM.Nationality c,hrm.tblemployeephoto P,(select top 1 Ed.EmployeementDetailsID, ED.JoinDate,RetirementDate,D.DepartmentName,Ds.DesignationName,Type,Status from HRM.tblEmployeementDetails ED,HRM.tbldepartment D,HRM.tbldesignation Ds where ED.Department=D.DepartmentID and ED.Designation=Ds.DesignationID order by employeementdetailsid desc)a  where E.Country=C.NationalityID and E.ID=P.EmployeeID";
            return Global.m_db.SelectQry(str, "tblEmployee");
        }

        public DataTable salaryDetails(int departmentID, int facultyID, int paySlipID = 0)
        {
            string departmentSetting = "", facultySetting = "", paySlipSetting = "", selectPaySlipIDSetting = "";
            if (paySlipID > 0)
            {
                paySlipSetting = " and e.ID in(select EmployeeID from HRM.tblPaySlipMasterDetails where paySlipID = " + paySlipID + " and EmpPresence = 0) ";
                selectPaySlipIDSetting = " ,(select top 1 ID from HRM.tblPaySlipMasterDetails ps where paySlipID = " + paySlipID + " and ps.EmployeeID = e.ID) PrimaryID ";
            }
            string str = "select e.ID, e.StaffCode, CONCAT( FirstName,' ',MiddleName,' ',LastName) as staffname,d.DepartmentName,de.DesignationName,"
                + "si.*,el.LevelName,el.PerGradeAmt as GradeAmt,(Select SUM(EL.LoanMthPremium) from HRM.tblEmployeeLoan EL where EL.EmpID = E.ID and " +
                "EL.LoanRemainingMth  > 0) as LoanPremium,(Select SUM(EA.Installment) from HRM.tblEmployeeAdvance EA where EA.EmpID = E.ID and EA.RemainingAmt>0) " +
                "as AdvInsatallment,(Select SUM(EA1.RemainingAmt) from HRM.tblEmployeeAdvance EA1 where EA1.EmpID = E.ID and EA1.RemainingAmt>0) as AdvRemaining " +
                selectPaySlipIDSetting + " from HRM.tblEmployee e,HRM.tbldepartment d,HRM.tbldesignation de,HRM.tblEmployeeSalaryInfo si,HRM.tblEmployeeLevel el " +
                " where e.ID=si.EmployeeID and si.departmentid=d.DepartmentID and si.designationid=de.DesignationID and si.Level = el.LevelID and" +
                " e.IsPartTime = 0"
                // donot load employees whose status is break or retire
            + " and (select top 1 edet.Status from HRM.tblEmployeementDetails edet where e.ID = edet.EmployeeID order by EmployeementDetailsID desc)<= 1 ";
            string orderBy = " order by si.BasicSalary desc, staffname ";//" order by e.StaffCode";
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

        /// <summary>
        /// This method is to get salary details of employee whose EmpPresence is false
        /// </summary>
        /// <param name="paySlipID"></param>
        /// <returns></returns>
        public DataTable savedSalaryDetails(int paySlipID)
        {
            string str = "select d.*,de.DesignationName, (select StartingSalary from HRM.tblEmployeeSalaryInfo si where si.EmployeeID = d.EmployeeID) StartingSalary  from HRM.tblPaySlipMasterDetails d,HRM.tbldesignation de where d.designationID = de.DesignationID and d.EmpPresence = 0 and d.paySlipId = " + paySlipID;
            return Global.m_db.SelectQry(str, "HRM.tblPaySlipMasterDetails");
        }

        public static string ConvertIDsToString(int[] paySlipIDs)
        {
            string payslips = "";

            foreach (int payslipId in paySlipIDs)
            {
                payslips += payslipId.ToString() + ",";
            }
            payslips = payslips.Substring(0, payslips.Length - 1); // remove the last comma ","
            return payslips;
        }

        /// <summary>
        /// This method is to get salary details of employee whose EmpPresence is true
        /// </summary>
        /// <param name="paySlipIDs"></param>
        /// <returns></returns>
        public DataTable savedSalaryDetailsPresent(EmployeeReportSettings settings)//(int[] paySlipIDs, bool isRemaining, string date)
        {
            string payslips = ConvertIDsToString(settings.paySlipIds);
            string dateStr = "";
            if (settings.paySlipDate != null)
            {
                dateStr = " and convert(date, d.PaySlipDate) = '" + settings.paySlipDate + "'";
            }
            if (settings.fromDate != null && settings.toDate != null)
            {
                dateStr = "and d.PaySlipDate between '" + Convert.ToDateTime(settings.fromDate).ToShortDateString() + "' and '" + Convert.ToDateTime(settings.toDate).ToShortDateString() + " 23:59:59:999' ";
            }
            string str = "select d.*,de.DesignationName from HRM.tblPaySlipMasterDetails d,HRM.tbldesignation de where d.designationID = de.DesignationID and d.EmpPresence = " + Convert.ToInt32(!settings.isRemaining) + " and d.paySlipId in ( " + payslips + ") "+dateStr+" order by d.BasicSalary desc, d.StaffName";
            return Global.m_db.SelectQry(str, "HRM.tblPaySlipMasterDetails");
        }

        public DataTable getMonth()
        {
            string str = "select * from System.tblMonth";
            return Global.m_db.SelectQry(str, "tblMonth");
        }

        public int createAdditionDeduction(string name, string code, int IsAddition)
        {
            string str = "insert into HRm.tblAdditionDeduction values('" + name + "','" + code + "'," + IsAddition + ")";
            return Global.m_db.InsertUpdateQry(str);
        }

        public DataTable getAdditionDeduction()
        {
            string str = "select * from HRm.tblAdditionDeduction";
            return Global.m_db.SelectQry(str, "tblAdditionDeduction");
        }
        public DataTable getAdditionOnly()
        {
            string str = "select * from HRm.tblAdditionDeduction where IsAddition=1";
            return Global.m_db.SelectQry(str, "tblAdditionDeduction");
        }
        public DataTable getDeductionOnly()
        {
            string str = "select * from HRm.tblAdditionDeduction where IsAddition=0";
            return Global.m_db.SelectQry(str, "tblAdditionDeduction");
        }
        public DataTable getAdditionDeductionID(int ID)
        {
            string str = "select * from Hrm.tblAdditionDeduction  where AdditionDeductionID=" + ID + "";
            return Global.m_db.SelectQry(str, "tblAdditionDeduction");
        }
        public DataTable deleteFromAdditionDeduction(int id)
        {
            string str = "delete from HRM.tblAdditionDeduction where AdditionDeductionID=" + id + "";
            return Global.m_db.SelectQry(str, "tblAdditionDeduction");
        }
        public int updateAdditionDeduction(int ID, string name, string code, int isaddition)
        {
            string str = "update Hrm.tblAdditionDeduction set Name='" + name + "',Code='" + code + "',IsAddition=" + isaddition + " where AdditionDeductionID=" + ID + "";
            return Global.m_db.InsertUpdateQry(str);
        }
        public DataTable getEmployee()
        {
            string str = "select e.ID,e.StaffCode,e.FirstName+' '+e.LastName Name from HRM.tblEmployee e";
            return Global.m_db.SelectQry(str, "tblEmployee");

        }

        public DataTable GetEmployeeForTax(int empID)
        {
            string str = "select e.IsSingle,e.EmpType,e.IsCoupleWorking,coalesce(s.OverTimeAlw,0) OverTimeAlw,s.InsurancePremium,e.Gender from HRM.tblEmployee e,HRM.tblEmployeeSalaryInfo s where e.ID = s.EmployeeID and e.ID = '" + empID + "'";
            return Global.m_db.SelectQry(str, "tblEmployee");
        }

        public DataTable insertIntoMasterPaySlip(int monthId, string monthName, DateTime datetime, string createdby, string modifiedby)
        {
            string str = "insert into Hrm.tblSalaryPayslipMaster(monthID,monthName,date,createdBy,modifiedBy) values(" + monthId + ",'" + monthName + "'," + datetime + ",'" + createdby + "','" + modifiedby + "')";
            return Global.m_db.SelectQry(str, "tblSalaryPayslipMaster");
        }

        public DataTable insertIntoPaySlipAllowances(int slipID, int empid, string empName, string allowanceName, double allowanceAmount)
        {
            string str = "insert into Hrm.tblPaySlipDetailAllowances values(" + slipID + ",'" + empid + "','" + empName + "','" + allowanceName + "'," + allowanceAmount + ")";
            return Global.m_db.SelectQry(str, "tblPaySlipDetailAllowances");
        }
        //public DataTable insertIntoPaySlipMasterDetails(paySlipDetails ps)
        //{
        //    string str = "insert into Hrm.tblPaySlipMasterDetails values(" + ps.employeeID + ",'" + ps.employeeCode + "','" + ps.employeeName + "'," + ps.designationID + "," + ps.basicSalary + "," + ps.absentDays + "," + ps.payableSalary + "," + ps.miscDeduction + "," + ps.pfAmount + "," + ps.basicAllowance + "," + ps.bonus + "," + ps.tada + "," + ps.otherAllowances + "," + ps.totalAllowances + "," + ps.pfDeduction + "," + ps.cifNo + "," + ps.citamount + "," + ps.advance + "," + ps.TDS + "," + ps.netPayableAmount + "," + ps.paySlipid + ")";
        //    return Global.m_db.SelectQry(str, "tblPaySlipMasterDetails");
        //}

        public DataTable getDesigIdByName(string name)
        {
            string str = "select DesignationID from HRM.tbldesignation where DesignationName='" + name + "'";
            return Global.m_db.SelectQry(str, "tbldesignation");
        }
        public DataTable insertIntoPaySlipDeduction(int slipID, int empid, string empName, string deductionName, double deductAmount)
        {
            string str = "insert into Hrm.tblPaySlipDetailDeduction values(" + empid + ",'" + slipID + "','" + empName + "','" + deductionName + "'," + deductAmount + ")";
            return Global.m_db.SelectQry(str, "tblPaySlipDetailDeduction");
        }
        public DataTable getSalaryPayInfo(int empID, int monthid)
        {
            string str = "select ad.Name as allowanceName ,A.allowanceAmount from HRM.tblPaySlipDetailAllowances A,HRM.tblSalaryPayslipMaster M,HRM.tblAdditionDeduction ad where  M.salaryPaySlipID=A.salaryPaySlipID and a.allowanceID=ad.AdditionDeductionID and M.monthID=" + monthid + "  and A.employeeID=" + empID + "";
            return Global.m_db.SelectQry(str, "tblPaySlipMasterDetails");
        }
        public DataTable getBasicSalary(int empID, int monthid)
        {
            string str = "select D.basicSalary from HRM.tblPaySlipMasterDetails D,HRM.tblSalaryPayslipMaster M where  M.salaryPaySlipID=D.paySlipId and M.monthID=" + monthid + "  and D.employeeID=" + empID + "";
            return Global.m_db.SelectQry(str, "tblPaySlipMasterDetails");
        }
        public DataTable getPfCfAmount(int empID, int monthid)
        {
            string str = "select D.pfAmount,D.citamount,D.TDS from HRM.tblPaySlipMasterDetails D,HRM.tblSalaryPayslipMaster M where  M.salaryPaySlipID=D.paySlipId and M.monthID=" + monthid + "  and D.employeeID=" + empID + "";
            return Global.m_db.SelectQry(str, "tblPaySlipMasterDetails");
        }
        public DataTable getdeduction(int empID, int monthid)
        {
            string str = "select D.deductionName,D.deductionAmount from HRM.tblPaySlipDetailDeduction D,HRM.tblSalaryPayslipMaster M where  M.salaryPaySlipID=D.salaryPaySlipID and M.monthID=" + monthid + "  and D.employeeID=" + empID + "";
            return Global.m_db.SelectQry(str, "tblPaySlipMasterDetails");
        }

        /// <summary>
        /// Returns Id as string according to name from tblAdditionDeduction
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string GetAddDedIdByName(string name)
        {

            DataTable dtadibn = new DataTable();
            string str = "select AdditionDeductionID from HRM.tblAdditionDeduction where Name = '" + name + "'";
            dtadibn = Global.m_db.SelectQry(str, "HRM.tblAdditionDeduction");
            if (dtadibn.Rows.Count > 0)
            {
                str = dtadibn.Rows[0][0].ToString();
                return str;
            }
            else
            {
                str = "";
                return str;
            }

        }

        /// <summary>
        /// Returns Id as string according to name from tblAdditionDeduction for update
        /// </summary>
        /// <param name="name"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetAddDedIdByName(string name, int id)
        {

            DataTable dtadibn = new DataTable();
            string str = "select AdditionDeductionID from HRM.tblAdditionDeduction where Name = '" + name + "'  and AdditionDeductionID <> " + id;
            dtadibn = Global.m_db.SelectQry(str, "HRM.tblAdditionDeduction");
            if (dtadibn.Rows.Count > 0)
            {
                str = dtadibn.Rows[0][0].ToString();
                return str;
            }
            else
            {
                str = "";
                return str;
            }

        }

        /// <summary>
        /// Returns false if employee has pay slip records.
        /// </summary>
        /// <param name="empId"></param>
        /// <returns></returns>
        public bool CheckEmpPaySlip(int empId)
        {
            string str = "Select * from HRM.tblPaySlipMasterDetails where employeeID=" + empId;
            DataTable dt = Global.m_db.SelectQry(str, "HRM.tblPaySlipMasterDetails");
            if (dt.Rows.Count > 0)
                return false;
            else
                return true;
        }

        public bool CheckDepartment(int departID)
        {
            string str = "Select * from HRM.tblEmployeementDetails where Department=" + departID;
            DataTable dt = Global.m_db.SelectQry(str, "HRM.tblEmployeementDetails");
            if (dt.Rows.Count > 0)
                return false;
            else
                return true;
        }

        public bool CheckDesignation(int desigID)
        {
            string str = "Select * from HRM.tblEmployeementDetails where Designation=" + desigID;
            DataTable dt = Global.m_db.SelectQry(str, "HRM.tblEmployeementDetails");
            if (dt.Rows.Count > 0)
                return false;
            else
                return true;
        }

        public bool CheckCountry(int id)
        {
            string str = "Select * from HRM.tblEmployee where Country=" + id;
            DataTable dt = Global.m_db.SelectQry(str, "HRM.tblEmployee");
            if (dt.Rows.Count > 0)
                return false;
            else
                return true;
        }

        /// <summary>
        /// Returns false if Employee code already in use
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public bool CheckStaffCode(string code)
        {
            try
            {
                string str = "Select * from HRM.tblEmployee where StaffCode='" + code + "'";
                DataTable dtsc = Global.m_db.SelectQry(str, "HRM.tblEmployee");
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

        public DataTable GetSalaryMaster(int paySlipId)
        {
            string str = "Select * from HRM.tblSalaryPayslipMaster where salaryPaySlipID=" + paySlipId;
            return Global.m_db.SelectQry(str, "HRM.tblSalaryPayslipMaster");
        }

        /// <summary>
        /// Returns false if Employee code already in use except for the provided employee id.
        /// </summary>
        /// <param name="code"></param>
        /// <param name="empID"></param>
        /// <returns></returns>
        public bool CheckStaffCode(string code, int empID)
        {
            try
            {
                string str = "Select * from HRM.tblEmployee where StaffCode='" + code + "' and ID <> " + empID;
                DataTable dtsc = Global.m_db.SelectQry(str, "HRM.tblEmployee");
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

        /// <summary>
        /// Checks if pay slip for an employee for a month of the fiscal year has been created.
        /// </summary>
        /// <param name="monthId"></param>
        /// <param name="employeeId"></param>
        /// <param name="FYStart"></param>
        /// <returns></returns>
        public static bool CheckPayMonth(int monthId, int employeeId, DateTime FYStart)
        {
            bool val = false;
            Global.m_db.ClearParameter();
            Global.m_db.setCommandType(CommandType.StoredProcedure);
            Global.m_db.setCommandText("HRM.spGetPaySlipByMonthEmp");
            Global.m_db.AddParameter("@monthID", SqlDbType.Int, monthId);
            Global.m_db.AddParameter("@empId", SqlDbType.Int, employeeId);
            DataTable dtPS = Global.m_db.GetDataTable();
            if (dtPS.Rows.Count > 0)
            {
                for (int i = 0; i < dtPS.Rows.Count; i++)
                {
                    DateTime psDate = Convert.ToDateTime(dtPS.Rows[i]["date"].ToString());
                    if (psDate >= FYStart && psDate <= FYStart.AddYears(1))
                        val = true;
                }
                return val;
            }
            else
            {
                return val;
            }

        }

        /// <summary>
        /// returns the row of a particular month and year
        /// </summary>
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public static DataTable GetPayslipMaster(int month, string year,int DepartID,int FacID, bool isReport)
        {
            Global.m_db.ClearParameter();
            Global.m_db.setCommandType(CommandType.Text);
           
            string sqlStmt = "Select * from HRM.tblSalaryPayslipMaster where monthID =@monthID and year = @year and DepartmentID = @DepartID and FacultyID = @FacID";
            Global.m_db.setCommandText(sqlStmt);
            Global.m_db.AddParameter("@monthID", SqlDbType.Int, month);
            Global.m_db.AddParameter("@year", SqlDbType.NVarChar, 10, year);
            Global.m_db.AddParameter("@DepartID", SqlDbType.Int, DepartID);
            Global.m_db.AddParameter("@FacID", SqlDbType.Int, FacID);
            DataTable dt = Global.m_db.GetDataTable();

            if (dt.Rows.Count == 0 && FacID == 0 && isReport) // if facultyID is i.e. all faculty is selected and no record available, then select data of all saved faculties
            {
                Global.m_db.ClearParameter();
                Global.m_db.setCommandType(CommandType.Text);

                sqlStmt = "Select * from HRM.tblSalaryPayslipMaster where monthID =@monthID and year = @year and DepartmentID = @DepartID and FacultyID in (select FID from HRM.tblEmployeeFaculty)";
                Global.m_db.setCommandText(sqlStmt);
                Global.m_db.AddParameter("@monthID", SqlDbType.Int, month);
                Global.m_db.AddParameter("@year", SqlDbType.NVarChar, 10, year);
                Global.m_db.AddParameter("@DepartID", SqlDbType.Int, DepartID);
                Global.m_db.AddParameter("@FacID", SqlDbType.Int, FacID);
                dt = Global.m_db.GetDataTable();

            }

            return dt;
        }

        /// <summary>
        /// Deletes all the records of a payslip entry of an employee on a particular month of the current fisal year
        /// </summary>
        /// <param name="monthId"></param>
        /// <param name="employeeId"></param>
        /// <param name="FYStart"></param>
        /// <returns></returns>
        public static bool DeletePaySlip(int monthId, int employeeId, DateTime FYStart)
        {
            bool val = false;
            Global.m_db.ClearParameter();
            Global.m_db.setCommandType(CommandType.StoredProcedure);
            Global.m_db.setCommandText("HRM.spPaySlipDelete");
            Global.m_db.AddParameter("@monthID", SqlDbType.Int, monthId);
            Global.m_db.AddParameter("@empId", SqlDbType.Int, employeeId);
            Global.m_db.AddParameter("@FYStart", SqlDbType.Date, FYStart);
            System.Data.SqlClient.SqlParameter paramReturn1 = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
            Global.m_db.ProcessParameter();
            if (paramReturn1.Value.ToString() == "SUCCESS")
                val = true;

            return val;

        }

        /// <summary>
        /// deletes the transation of the payslipID and changes isVocherEntered to 0 from HRM.tblSalaryPayslipMaster
        /// </summary>
        /// <param name="paySlipID"></param>
        public void RemoveSalaryJournalEntry(int paySlipID)
        {
                Global.m_db.ClearParameter();
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("Acc.spRemoveSalaryEntry");
                Global.m_db.AddParameter("@paySlipId", SqlDbType.Int, paySlipID);
                //system.Data.SqlClient.SqlParameter objReturn = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);

                System.Data.SqlClient.SqlParameter paramReturn1 = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20); 
                Global.m_db.ProcessParameter();
                if (paramReturn1.Value.ToString() == "FAILURE")
                {
                    throw new Exception("There was a problem while deleting the data.");
                }
        }

        /// <summary>
        /// Returns true if employee records are deleted.
        /// </summary>
        /// <param name="empId"></param>
        /// <returns></returns>
        public bool DeleteEmployee(int empId)
        {
            bool val = false;
            Global.m_db.ClearParameter();
            Global.m_db.setCommandType(CommandType.StoredProcedure);
            Global.m_db.setCommandText("HRM.spEmployeeDelete");
            Global.m_db.AddParameter("@empId", SqlDbType.Int, empId);
            System.Data.SqlClient.SqlParameter paramReturn1 = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
            Global.m_db.ProcessParameter();
            if (paramReturn1.Value.ToString() == "SUCCESS")
                val = true;

            return val;
        }

        /// <summary>
        /// Saves Part Time Salary; Inserts new if the partTimeMasterID is 0 else Updates the existing one
        /// </summary>
        /// <param name="partTimeMasterID"></param>
        /// <param name="ps"></param>
        /// <param name="dt"></param>
        
        public static void SavePartTimeSalary(PartTimeSalaryDetails ps,DataTable dt)
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
                    int DocIDD = 0;
                    DataRow dr = dt.Rows[i];
                    Global.m_db.ClearParameter();
                    Global.m_db.setCommandType(CommandType.StoredProcedure);
                    Global.m_db.setCommandText("HRM.spPartTimeSalaryDetailsCreate");
                    Global.m_db.AddParameter("@PTMasterID", SqlDbType.Int, ReturnID);
                    Global.m_db.AddParameter("@EmployeeID", SqlDbType.Int, Convert.ToInt32(dr["EmployeeID"].ToString()));
                    Global.m_db.AddParameter("@DoctorID", SqlDbType.Int, DocIDD.ToString());
                   
                    Global.m_db.AddParameter("@EmpName", SqlDbType.NVarChar, 150, dr["Name"].ToString());
                    Global.m_db.AddParameter("@Designation", SqlDbType.NVarChar, 100, dr["Designation"].ToString());
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
                MessageBox.Show("Employee Salary Created Successfully ", "Part Time Salary Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                Global.m_db.RollBackTransaction();
                Global.MsgError(ex.Message);
            }
        }

        public static DataTable NavigatePartTimeSalary(int CurrentID,Navigate NavTo)
        {
            Global.m_db.ClearParameter();
            Global.m_db.setCommandType(CommandType.StoredProcedure);
            Global.m_db.setCommandText("HRM.spPartTimeSalaryNavigate");
            Global.m_db.AddParameter("@CurrentID", SqlDbType.Int, CurrentID);
            string strNavigate = "FIRST";
            switch (NavTo)
            {
                case Navigate.First:
                    strNavigate = "FIRST";
                    break;
                case Navigate.Last:
                    strNavigate = "LAST";
                    break;
                case Navigate.Next:
                    strNavigate = "NEXT";
                    break;
                case Navigate.Prev:
                    strNavigate = "PREV";
                    break;
                case Navigate.ID:
                    strNavigate = "ID";
                    break;
            }
            Global.m_db.AddParameter("@NavigateTo", SqlDbType.NVarChar, 20, strNavigate);
            DataTable dt = Global.m_db.GetDataTable();
            return dt;
        }

        public static DataTable GetPartTimeSalaryDetail(int masterID)
        {
            Global.m_db.ClearParameter();
            Global.m_db.setCommandType(CommandType.Text);
            Global.m_db.setCommandText("Select PT.* from HRM.tblPartTimeSalaryDetail PT where PT.PTMasterID = @a");
            Global.m_db.AddParameter("@a", SqlDbType.Int, masterID);
            return Global.m_db.GetDataTable();
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
                    int doctorid = 0;
                    DataRow drpayslipmasterdetail = dtpayslipmasterdetail.Rows[i];
                    Global.m_db.ClearParameter();
                    Global.m_db.setCommandType(CommandType.StoredProcedure);
                    Global.m_db.setCommandText("HRM.spPaySlipMasterDetailsCreate");
                    Global.m_db.AddParameter("@paySlipId", SqlDbType.Int, ReturnID);
                    Global.m_db.AddParameter("@employeeID", SqlDbType.Int, Convert.ToInt32(drpayslipmasterdetail["employeeID"].ToString()));
                    Global.m_db.AddParameter("@doctorID", SqlDbType.Int, doctorid.ToString());
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
                    Global.m_db.AddParameter("@isAlreadySaved", SqlDbType.Int, paySlipId >0?1:0);
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
                Global.m_db.AddParameter("@EmpPaySlipLoanTable", SqlDbType.Structured, dtLoan);
                Global.m_db.AddParameter("@PaySlipID", SqlDbType.Int, ReturnID);
                Global.m_db.setCommandText("HRM.spEmployeePaySlipLoanCreate");
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.ProcessParameter();

                // insert into salary adjustment table
                dtSalaryAdjust.Columns.Add("PaySlipID", typeof(int), ReturnID.ToString());
                //dtSalaryAdjust.Columns["ExamMasterID"].SetOrdinal(0);

                Global.m_db.ClearParameter();
                Global.m_db.AddParameter("@SalaryAdjustmentTable", SqlDbType.Structured, dtSalaryAdjust);
                Global.m_db.setCommandText("HRM.spSalaryAdjustmentCreate");
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.ProcessParameter();
                //For Allowances
                //for (int i = 0; i < dtallowances.Rows.Count; i++)
                //{
                //    DataRow drallowances = dtallowances.Rows[i];
                //    Global.m_db.ClearParameter();
                //    Global.m_db.setCommandType(CommandType.StoredProcedure);
                //    Global.m_db.setCommandText("HRM.spSalaryAllowanceCreate");
                //    Global.m_db.AddParameter("@salaryPaySlipID", SqlDbType.Int, ReturnID);
                //    Global.m_db.AddParameter("@employeeID", SqlDbType.Int, drallowances["empId"].ToString());
                //    Global.m_db.AddParameter("@employeeName", SqlDbType.NVarChar, 30, drallowances["empname"].ToString());
                //    Global.m_db.AddParameter("@allowanceID", SqlDbType.Int, drallowances["allowanceID"].ToString());//Set same for both for time being
                //    Global.m_db.AddParameter("@allowanceAmount", SqlDbType.Float, drallowances["amount"].ToString());

                //    System.Data.SqlClient.SqlParameter paramReturn1 = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                //    Global.m_db.ProcessParameter();
                //    if (paramReturn1.Value.ToString() != "SUCCESS")
                //    {
                //        Global.m_db.RollBackTransaction();
                //        throw new Exception("Unable to create Salary PaySlip Allowance");
                //    }
                //}

                ////For Deduction
                //for (int i = 0; i < dtdeduct.Rows.Count; i++)
                //{
                //    DataRow drdeduct = dtdeduct.Rows[i];
                //    Global.m_db.ClearParameter();
                //    Global.m_db.setCommandType(CommandType.StoredProcedure);
                //    Global.m_db.setCommandText("HRM.spSalaryDeductionCreate");
                //    Global.m_db.AddParameter("@salaryPaySlipID", SqlDbType.Int, ReturnID);
                //    Global.m_db.AddParameter("@employeeID", SqlDbType.Int, drdeduct["empID"].ToString());
                //    Global.m_db.AddParameter("@employeeName", SqlDbType.NVarChar, 30, drdeduct["empname"].ToString());
                //    Global.m_db.AddParameter("@deductionID", SqlDbType.Int, drdeduct["deductionID"].ToString());//Set same for both for time being
                //    Global.m_db.AddParameter("@deductionAmount", SqlDbType.Float, drdeduct["amount"].ToString());
                //    System.Data.SqlClient.SqlParameter paramReturn1 = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                //    Global.m_db.ProcessParameter();
                //    if (paramReturn1.Value.ToString() != "SUCCESS")
                //    {
                //        Global.m_db.RollBackTransaction();
                //        throw new Exception("Unable to create Salary PaySlip Deduct");
                //    }
                //}

                Global.m_db.CommitTransaction();
                MessageBox.Show("Employee Salary PaySlip Created Successfully ", "PaySlip Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                Global.m_db.RollBackTransaction();
                throw ex;
                //MessageBox.Show(ex.Message);
            }

        }

        public static void UpdateSavedPaySlip(int paySlipId, int id, string monthname, DateTime createddate, string createdby, string modifiedby, string year, int DepartmentID, int FacultyID, int isVoucherEntered, decimal tSalary, decimal tGrade, decimal tPF, decimal tPensionF, decimal tInflationAlw, decimal tAdmAlw, decimal tAcademicAlw, decimal tPostAlw, decimal tFestivalAlw, decimal tMiscAllow, decimal tGrossAmount, decimal tPFDeduct, decimal tPensionFDeduct, decimal tTaxDeduct, decimal tKK, decimal tNLK, decimal tAccommodation, decimal tElectricity, decimal tLoan, decimal tAdvance, decimal tMiscDeduct, decimal tTotalDeduct, decimal tNetSalary, bool isChkFestiveMonth, string Narration, DataTable dtpayslipmasterdetail, decimal tOverTimeAlw, decimal tOnePercentTax, decimal tPFAdjust, decimal tPensionAdjust, DataTable dtLoan, decimal tInsuranceAmt,DataTable dtSalaryAdjust = null)//,DataTable dtallowances,DataTable dtdeduct)
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

                //For MasterDetailSlip
                for (int i = 0; i < dtpayslipmasterdetail.Rows.Count; i++)
                {
                    DataRow drpayslipmasterdetail = dtpayslipmasterdetail.Rows[i];
                    Global.m_db.ClearParameter();
                    Global.m_db.setCommandType(CommandType.StoredProcedure);
                    Global.m_db.setCommandText("HRM.spPaySlipMasterDetailsUpdate");
                    Global.m_db.AddParameter("@paySlipId", SqlDbType.Int, ReturnID);
                    Global.m_db.AddParameter("@employeeID", SqlDbType.Int, Convert.ToInt32(drpayslipmasterdetail["employeeID"].ToString()));
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
                    Global.m_db.AddParameter("@PrimaryID", SqlDbType.Int, Convert.ToInt32(drpayslipmasterdetail["PrimaryID"].ToString()));
                    Global.m_db.AddParameter("@OnePercentTax", SqlDbType.Decimal, drpayslipmasterdetail["OnePercentTax"].ToString());
                    Global.m_db.AddParameter("@PFAdjust", SqlDbType.Decimal, drpayslipmasterdetail["PFAdjust"].ToString());
                    Global.m_db.AddParameter("@PensionAdjust", SqlDbType.Decimal, drpayslipmasterdetail["PensionAdjust"].ToString());
                    Global.m_db.AddParameter("@InsuranceAmt", SqlDbType.Decimal, drpayslipmasterdetail["InsuranceAmt"].ToString());

                    System.Data.SqlClient.SqlParameter paramReturn1 = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                    Global.m_db.ProcessParameter();
                    if (paramReturn1.Value.ToString() != "SUCCESS")
                    {
                        Global.m_db.RollBackTransaction();
                        throw new Exception("Unable to update Salary PaySlip.");
                    }
                }

                // insert into loan table
                Global.m_db.ClearParameter();
                Global.m_db.AddParameter("@EmpPaySlipLoanTable", SqlDbType.Structured, dtLoan);
                Global.m_db.AddParameter("@PaySlipID", SqlDbType.Int, ReturnID);
                Global.m_db.setCommandText("HRM.spEmployeePaySlipLoanCreate");
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.ProcessParameter();

                // insert into salary adjustment table
                dtSalaryAdjust.Columns.Add("PaySlipID", typeof(int), ReturnID.ToString());
                //dtSalaryAdjust.Columns["ExamMasterID"].SetOrdinal(0);

                Global.m_db.ClearParameter();
                Global.m_db.AddParameter("@SalaryAdjustmentTable", SqlDbType.Structured, dtSalaryAdjust);
                Global.m_db.setCommandText("HRM.spSalaryAdjustmentCreate");
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.ProcessParameter();
                //For Allowances
                //for (int i = 0; i < dtallowances.Rows.Count; i++)
                //{
                //    DataRow drallowances = dtallowances.Rows[i];
                //    Global.m_db.ClearParameter();
                //    Global.m_db.setCommandType(CommandType.StoredProcedure);
                //    Global.m_db.setCommandText("HRM.spSalaryAllowanceCreate");
                //    Global.m_db.AddParameter("@salaryPaySlipID", SqlDbType.Int, ReturnID);
                //    Global.m_db.AddParameter("@employeeID", SqlDbType.Int, drallowances["empId"].ToString());
                //    Global.m_db.AddParameter("@employeeName", SqlDbType.NVarChar, 30, drallowances["empname"].ToString());
                //    Global.m_db.AddParameter("@allowanceID", SqlDbType.Int, drallowances["allowanceID"].ToString());//Set same for both for time being
                //    Global.m_db.AddParameter("@allowanceAmount", SqlDbType.Float, drallowances["amount"].ToString());

                //    System.Data.SqlClient.SqlParameter paramReturn1 = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                //    Global.m_db.ProcessParameter();
                //    if (paramReturn1.Value.ToString() != "SUCCESS")
                //    {
                //        Global.m_db.RollBackTransaction();
                //        throw new Exception("Unable to create Salary PaySlip Allowance");
                //    }
                //}

                ////For Deduction
                //for (int i = 0; i < dtdeduct.Rows.Count; i++)
                //{
                //    DataRow drdeduct = dtdeduct.Rows[i];
                //    Global.m_db.ClearParameter();
                //    Global.m_db.setCommandType(CommandType.StoredProcedure);
                //    Global.m_db.setCommandText("HRM.spSalaryDeductionCreate");
                //    Global.m_db.AddParameter("@salaryPaySlipID", SqlDbType.Int, ReturnID);
                //    Global.m_db.AddParameter("@employeeID", SqlDbType.Int, drdeduct["empID"].ToString());
                //    Global.m_db.AddParameter("@employeeName", SqlDbType.NVarChar, 30, drdeduct["empname"].ToString());
                //    Global.m_db.AddParameter("@deductionID", SqlDbType.Int, drdeduct["deductionID"].ToString());//Set same for both for time being
                //    Global.m_db.AddParameter("@deductionAmount", SqlDbType.Float, drdeduct["amount"].ToString());
                //    System.Data.SqlClient.SqlParameter paramReturn1 = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                //    Global.m_db.ProcessParameter();
                //    if (paramReturn1.Value.ToString() != "SUCCESS")
                //    {
                //        Global.m_db.RollBackTransaction();
                //        throw new Exception("Unable to create Salary PaySlip Deduct");
                //    }
                //}

                Global.m_db.CommitTransaction();
                MessageBox.Show("Employee Salary Sheet Updated Successfully ", "Salary Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                Global.m_db.RollBackTransaction();
                throw ex;
                //MessageBox.Show(ex.Message + " "+ ex.Data);
            }

        }
        public static string GetSavedFacultyInfo(int month, int year)
        {
            try
            {
                string sql = "DECLARE @result varchar(1000); " +

                " SET @result = ''; "+

                " SELECT @result = @result + convert(varchar(10),FacultyID) + ',' FROM HRM.tblSalaryPayslipMaster where monthID = " + month + " and year = " + year + " and FacultyID != 0 ;" +

                " select substring(@result, 0, len(@result)) ;";

                string res = Global.m_db.GetScalarValue(sql).ToString();

                return res;
            }
            catch (Exception)
            {
                return "";
            }
        }
    
        /// <summary>
        /// Journal entry for salary sheet
        /// </summary>
        /// <param name="paySlipID"></param>
        /// <param name="date"></param>
        /// <param name="salaryAmt"></param>
        /// <param name="basicAllowAmt"></param>
        /// <param name="PFadditionAmt"></param>
        /// <param name="PFDedcutAmt"></param>
        /// <param name="incomeTaxAmt"></param>
        /// <param name="KalyankariAmt"></param>
        /// <param name="StaffLoanAmt"></param>
        /// <param name="NLFundAmt"></param>
        /// <param name="incomeIntAmt"></param>
        /// <param name="bankAmt"></param>
        /// <param name="bankID"></param>
        /// <param name="AccClassID"></param>
        public void SalaryJournalEntry(int paySlipID, DateTime date, decimal salaryAmt,decimal basicAllowAmt,decimal PFadditionAmt,decimal PFDedcutAmt,decimal incomeTaxAmt,decimal KalyankariAmt,decimal StaffLoanAmt,decimal NLFundAmt,decimal incomeIntAmt, decimal bankAmt,int bankID, int[] AccClassID)
        {
            try
            {

                string ledgerName = "";
                #region Transaction for Salary
                if (salaryAmt > 0)
                {
                    ledgerName = Ledger.GetLedgerNameFromID(Global.SalaryAcID);
                    Global.m_db.BeginTransaction();
                    Global.m_db.ClearParameter();
                    Global.m_db.setCommandType(CommandType.StoredProcedure);
                    Global.m_db.setCommandText("Acc.spTransactCreate");
                    Global.m_db.AddParameter("@Date", SqlDbType.DateTime, date);
                    Global.m_db.AddParameter("@LedgerID", SqlDbType.Int, Global.SalaryAcID);
                    Global.m_db.AddParameter("@LedgerName", SqlDbType.NVarChar, 300, ledgerName);
                    Global.m_db.AddParameter("@Language", SqlDbType.NVarChar, 20, "ENGLISH");
                    Global.m_db.AddParameter("@Amount", SqlDbType.Money, salaryAmt);
                    Global.m_db.AddParameter("@DrCr", SqlDbType.NVarChar, 20, "DEBIT");
                    Global.m_db.AddParameter("@VoucherType", SqlDbType.NVarChar, 20, "SALARY");
                    Global.m_db.AddParameter("@RowID", SqlDbType.Int, paySlipID);
                    Global.m_db.AddParameter("@ProjectID", SqlDbType.Int, 1);
                    System.Data.SqlClient.SqlParameter paramReturn1 = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                    Global.m_db.ProcessParameter();

                    int ReturnTransactID1 = Convert.ToInt32(paramReturn1.Value);
                    if (paramReturn1.Value.ToString() == "FAILURE")
                    {
                        Global.m_db.RollBackTransaction();
                        throw new Exception("Unable to create journal entry");
                    }
                    //Now add the New editable records for Acc.tblTransactionClass

                    foreach (int _AccClassID in AccClassID)
                    {
                        Global.m_db.ClearParameter();
                        Global.m_db.setCommandType(CommandType.StoredProcedure);
                        Global.m_db.setCommandText("Acc.spTransactClassCreate");
                        Global.m_db.AddParameter("@TransactID", SqlDbType.Int, ReturnTransactID1.ToString());
                        Global.m_db.AddParameter("@AccClassID", SqlDbType.Int, _AccClassID);//Set same for both for time being
                        Global.m_db.AddParameter("@RowID", SqlDbType.Int, paySlipID.ToString());
                        Global.m_db.AddParameter("VoucherType", SqlDbType.NVarChar, 20, "SALARY");
                        System.Data.SqlClient.SqlParameter paramTransactClassID = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                        Global.m_db.ProcessParameter();

                        if (paramTransactClassID.Value.ToString() == "FAILURE")
                        {
                            Global.m_db.RollBackTransaction();
                            throw new Exception("Unable to create journal entry");
                        }
                    }
                }
                #endregion

                #region Transaction for basic allowance
                if (basicAllowAmt > 0)
                {
                    ledgerName = Ledger.GetLedgerNameFromID(Global.BasicAllowanceID);
                    Global.m_db.ClearParameter();
                    Global.m_db.setCommandType(CommandType.StoredProcedure);
                    Global.m_db.setCommandText("Acc.spTransactCreate");
                    Global.m_db.AddParameter("@Date", SqlDbType.DateTime, date);
                    Global.m_db.AddParameter("@LedgerID", SqlDbType.Int, Global.BasicAllowanceID);
                    Global.m_db.AddParameter("@LedgerName", SqlDbType.NVarChar, 300, ledgerName);
                    Global.m_db.AddParameter("@Language", SqlDbType.NVarChar, 20, "ENGLISH");
                    Global.m_db.AddParameter("@Amount", SqlDbType.Money, basicAllowAmt);
                    Global.m_db.AddParameter("@DrCr", SqlDbType.NVarChar, 20, "DEBIT");
                    Global.m_db.AddParameter("@VoucherType", SqlDbType.NVarChar, 20, "SALARY");
                    Global.m_db.AddParameter("@RowID", SqlDbType.Int, paySlipID);
                    Global.m_db.AddParameter("@ProjectID", SqlDbType.Int, 1);
                    System.Data.SqlClient.SqlParameter paramReturn2 = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                    Global.m_db.ProcessParameter();

                    int ReturnTransactID2 = Convert.ToInt32(paramReturn2.Value);
                    if (paramReturn2.Value.ToString() == "FAILURE")
                    {
                        Global.m_db.RollBackTransaction();
                        throw new Exception("Unable to create journal entry");
                    }
                    //Now add the New editable records for Acc.tblTransactionClass

                    foreach (int _AccClassID in AccClassID)
                    {
                        Global.m_db.ClearParameter();
                        Global.m_db.setCommandType(CommandType.StoredProcedure);
                        Global.m_db.setCommandText("Acc.spTransactClassCreate");
                        Global.m_db.AddParameter("@TransactID", SqlDbType.Int, ReturnTransactID2.ToString());
                        Global.m_db.AddParameter("@AccClassID", SqlDbType.Int, _AccClassID);//Set same for both for time being
                        Global.m_db.AddParameter("@RowID", SqlDbType.Int, paySlipID);
                        Global.m_db.AddParameter("VoucherType", SqlDbType.NVarChar, 20, "SALARY");
                        System.Data.SqlClient.SqlParameter paramTransactClassID = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                        Global.m_db.ProcessParameter();

                        if (paramTransactClassID.Value.ToString() == "FAILURE")
                        {
                            Global.m_db.RollBackTransaction();
                            throw new Exception("Unable to create journal entry");
                        }
                    }
                }
                #endregion
                
                #region Transaction for PF Contribution
                if (PFadditionAmt > 0)
                {
                    ledgerName = Ledger.GetLedgerNameFromID(Global.PFContributionID);
                    Global.m_db.ClearParameter();
                    Global.m_db.setCommandType(CommandType.StoredProcedure);
                    Global.m_db.setCommandText("Acc.spTransactCreate");
                    Global.m_db.AddParameter("@Date", SqlDbType.DateTime, date);
                    Global.m_db.AddParameter("@LedgerID", SqlDbType.Int, Global.PFContributionID);
                    Global.m_db.AddParameter("@LedgerName", SqlDbType.NVarChar, 300, ledgerName);
                    Global.m_db.AddParameter("@Language", SqlDbType.NVarChar, 20, "ENGLISH");
                    Global.m_db.AddParameter("@Amount", SqlDbType.Money, PFadditionAmt);
                    Global.m_db.AddParameter("@DrCr", SqlDbType.NVarChar, 20, "DEBIT");
                    Global.m_db.AddParameter("@VoucherType", SqlDbType.NVarChar, 20, "SALARY");
                    Global.m_db.AddParameter("@RowID", SqlDbType.Int, paySlipID);
                    Global.m_db.AddParameter("@ProjectID", SqlDbType.Int, 1);
                    System.Data.SqlClient.SqlParameter paramReturn3 = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                    Global.m_db.ProcessParameter();

                    int ReturnTransactID3 = Convert.ToInt32(paramReturn3.Value);
                    if (paramReturn3.Value.ToString() == "FAILURE")
                    {
                        Global.m_db.RollBackTransaction();
                        throw new Exception("Unable to create journal entry");
                    }
                    //Now add the New editable records for Acc.tblTransactionClass

                    foreach (int _AccClassID in AccClassID)
                    {
                        Global.m_db.ClearParameter();
                        Global.m_db.setCommandType(CommandType.StoredProcedure);
                        Global.m_db.setCommandText("Acc.spTransactClassCreate");
                        Global.m_db.AddParameter("@TransactID", SqlDbType.Int, ReturnTransactID3.ToString());
                        Global.m_db.AddParameter("@AccClassID", SqlDbType.Int, _AccClassID);//Set same for both for time being
                        Global.m_db.AddParameter("@RowID", SqlDbType.Int, paySlipID);
                        Global.m_db.AddParameter("VoucherType", SqlDbType.NVarChar, 20, "SALARY");
                        System.Data.SqlClient.SqlParameter paramTransactClassID = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                        Global.m_db.ProcessParameter();

                        if (paramTransactClassID.Value.ToString() == "FAILURE")
                        {
                            Global.m_db.RollBackTransaction();
                            throw new Exception("Unable to create journal entry");
                        }
                    }
                }
                #endregion

                #region Transaction for PF Deduction
                if (PFDedcutAmt > 0)
                {
                    ledgerName = Ledger.GetLedgerNameFromID(Global.PFPayableID);
                    Global.m_db.ClearParameter();
                    Global.m_db.setCommandType(CommandType.StoredProcedure);
                    Global.m_db.setCommandText("Acc.spTransactCreate");
                    Global.m_db.AddParameter("@Date", SqlDbType.DateTime, date);
                    Global.m_db.AddParameter("@LedgerID", SqlDbType.Int, Global.PFPayableID);
                    Global.m_db.AddParameter("@LedgerName", SqlDbType.NVarChar, 300, ledgerName);
                    Global.m_db.AddParameter("@Language", SqlDbType.NVarChar, 20, "ENGLISH");
                    Global.m_db.AddParameter("@Amount", SqlDbType.Money, PFDedcutAmt);
                    Global.m_db.AddParameter("@DrCr", SqlDbType.NVarChar, 20, "CREDIT");
                    Global.m_db.AddParameter("@VoucherType", SqlDbType.NVarChar, 20, "SALARY");
                    Global.m_db.AddParameter("@RowID", SqlDbType.Int, paySlipID);
                    Global.m_db.AddParameter("@ProjectID", SqlDbType.Int, 1);
                    System.Data.SqlClient.SqlParameter paramReturn3 = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                    Global.m_db.ProcessParameter();

                    int ReturnTransactID3 = Convert.ToInt32(paramReturn3.Value);
                    if (paramReturn3.Value.ToString() == "FAILURE")
                    {
                        Global.m_db.RollBackTransaction();
                        throw new Exception("Unable to create journal entry");
                    }
                    //Now add the New editable records for Acc.tblTransactionClass

                    foreach (int _AccClassID in AccClassID)
                    {
                        Global.m_db.ClearParameter();
                        Global.m_db.setCommandType(CommandType.StoredProcedure);
                        Global.m_db.setCommandText("Acc.spTransactClassCreate");
                        Global.m_db.AddParameter("@TransactID", SqlDbType.Int, ReturnTransactID3.ToString());
                        Global.m_db.AddParameter("@AccClassID", SqlDbType.Int, _AccClassID);//Set same for both for time being
                        Global.m_db.AddParameter("@RowID", SqlDbType.Int, paySlipID);
                        Global.m_db.AddParameter("VoucherType", SqlDbType.NVarChar, 20, "SALARY");
                        System.Data.SqlClient.SqlParameter paramTransactClassID = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                        Global.m_db.ProcessParameter();

                        if (paramTransactClassID.Value.ToString() == "FAILURE")
                        {
                            Global.m_db.RollBackTransaction();
                            throw new Exception("Unable to create journal entry");
                        }
                    }
                }
                #endregion

                #region Transaction for Income Tax/TDS on Salary
                if (incomeTaxAmt > 0)
                {
                    ledgerName = Ledger.GetLedgerNameFromID(Global.TDSonSalaryID);
                    Global.m_db.ClearParameter();
                    Global.m_db.setCommandType(CommandType.StoredProcedure);
                    Global.m_db.setCommandText("Acc.spTransactCreate");
                    Global.m_db.AddParameter("@Date", SqlDbType.DateTime, date);
                    Global.m_db.AddParameter("@LedgerID", SqlDbType.Int, Global.TDSonSalaryID);
                    Global.m_db.AddParameter("@LedgerName", SqlDbType.NVarChar, 300,ledgerName);
                    Global.m_db.AddParameter("@Language", SqlDbType.NVarChar, 20, "ENGLISH");
                    Global.m_db.AddParameter("@Amount", SqlDbType.Money, incomeTaxAmt);
                    Global.m_db.AddParameter("@DrCr", SqlDbType.NVarChar, 20, "CREDIT");
                    Global.m_db.AddParameter("@VoucherType", SqlDbType.NVarChar, 20, "SALARY");
                    Global.m_db.AddParameter("@RowID", SqlDbType.Int, paySlipID);
                    Global.m_db.AddParameter("@ProjectID", SqlDbType.Int, 1);
                    System.Data.SqlClient.SqlParameter paramReturn3 = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                    Global.m_db.ProcessParameter();

                    int ReturnTransactID3 = Convert.ToInt32(paramReturn3.Value);
                    if (paramReturn3.Value.ToString() == "FAILURE")
                    {
                        Global.m_db.RollBackTransaction();
                        throw new Exception("Unable to create journal entry");
                    }
                    //Now add the New editable records for Acc.tblTransactionClass

                    foreach (int _AccClassID in AccClassID)
                    {
                        Global.m_db.ClearParameter();
                        Global.m_db.setCommandType(CommandType.StoredProcedure);
                        Global.m_db.setCommandText("Acc.spTransactClassCreate");
                        Global.m_db.AddParameter("@TransactID", SqlDbType.Int, ReturnTransactID3.ToString());
                        Global.m_db.AddParameter("@AccClassID", SqlDbType.Int, _AccClassID);//Set same for both for time being
                        Global.m_db.AddParameter("@RowID", SqlDbType.Int, paySlipID);
                        Global.m_db.AddParameter("VoucherType", SqlDbType.NVarChar, 20, "SALARY");
                        System.Data.SqlClient.SqlParameter paramTransactClassID = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                        Global.m_db.ProcessParameter();

                        if (paramTransactClassID.Value.ToString() == "FAILURE")
                        {
                            Global.m_db.RollBackTransaction();
                            throw new Exception("Unable to create journal entry");
                        }
                    }
                }
                #endregion

                #region Transaction for Kalyankari Fund
                if (KalyankariAmt > 0)
                {
                    ledgerName = Ledger.GetLedgerNameFromID(Global.KalyankariFundID);
                    Global.m_db.ClearParameter();
                    Global.m_db.setCommandType(CommandType.StoredProcedure);
                    Global.m_db.setCommandText("Acc.spTransactCreate");
                    Global.m_db.AddParameter("@Date", SqlDbType.DateTime, date);
                    Global.m_db.AddParameter("@LedgerID", SqlDbType.Int, Global.KalyankariFundID);
                    Global.m_db.AddParameter("@LedgerName", SqlDbType.NVarChar, 300,ledgerName);
                    Global.m_db.AddParameter("@Language", SqlDbType.NVarChar, 20, "ENGLISH");
                    Global.m_db.AddParameter("@Amount", SqlDbType.Money, KalyankariAmt);
                    Global.m_db.AddParameter("@DrCr", SqlDbType.NVarChar, 20, "CREDIT");
                    Global.m_db.AddParameter("@VoucherType", SqlDbType.NVarChar, 20, "SALARY");
                    Global.m_db.AddParameter("@RowID", SqlDbType.Int, paySlipID);
                    Global.m_db.AddParameter("@ProjectID", SqlDbType.Int, 1);
                    System.Data.SqlClient.SqlParameter paramReturn3 = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                    Global.m_db.ProcessParameter();

                    int ReturnTransactID3 = Convert.ToInt32(paramReturn3.Value);
                    if (paramReturn3.Value.ToString() == "FAILURE")
                    {
                        Global.m_db.RollBackTransaction();
                        throw new Exception("Unable to create journal entry");
                    }
                    //Now add the New editable records for Acc.tblTransactionClass

                    foreach (int _AccClassID in AccClassID)
                    {
                        Global.m_db.ClearParameter();
                        Global.m_db.setCommandType(CommandType.StoredProcedure);
                        Global.m_db.setCommandText("Acc.spTransactClassCreate");
                        Global.m_db.AddParameter("@TransactID", SqlDbType.Int, ReturnTransactID3.ToString());
                        Global.m_db.AddParameter("@AccClassID", SqlDbType.Int, _AccClassID);//Set same for both for time being
                        Global.m_db.AddParameter("@RowID", SqlDbType.Int, paySlipID);
                        Global.m_db.AddParameter("VoucherType", SqlDbType.NVarChar, 20, "SALARY");
                        System.Data.SqlClient.SqlParameter paramTransactClassID = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                        Global.m_db.ProcessParameter();

                        if (paramTransactClassID.Value.ToString() == "FAILURE")
                        {
                            Global.m_db.RollBackTransaction();
                            throw new Exception("Unable to create journal entry");
                        }
                    }
                }
                #endregion

                #region Transaction for Staff Loan Account
                if (StaffLoanAmt > 0)
                {
                    ledgerName = Ledger.GetLedgerNameFromID(Global.StaffLoanAcID);
                    Global.m_db.ClearParameter();
                    Global.m_db.setCommandType(CommandType.StoredProcedure);
                    Global.m_db.setCommandText("Acc.spTransactCreate");
                    Global.m_db.AddParameter("@Date", SqlDbType.DateTime, date);
                    Global.m_db.AddParameter("@LedgerID", SqlDbType.Int, Global.StaffLoanAcID);
                    Global.m_db.AddParameter("@LedgerName", SqlDbType.NVarChar, 300, ledgerName);
                    Global.m_db.AddParameter("@Language", SqlDbType.NVarChar, 20, "ENGLISH");
                    Global.m_db.AddParameter("@Amount", SqlDbType.Money, StaffLoanAmt);
                    Global.m_db.AddParameter("@DrCr", SqlDbType.NVarChar, 20, "CREDIT");
                    Global.m_db.AddParameter("@VoucherType", SqlDbType.NVarChar, 20, "SALARY");
                    Global.m_db.AddParameter("@RowID", SqlDbType.Int, paySlipID);
                    Global.m_db.AddParameter("@ProjectID", SqlDbType.Int, 1);
                    System.Data.SqlClient.SqlParameter paramReturn3 = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                    Global.m_db.ProcessParameter();

                    int ReturnTransactID3 = Convert.ToInt32(paramReturn3.Value);
                    if (paramReturn3.Value.ToString() == "FAILURE")
                    {
                        Global.m_db.RollBackTransaction();
                        throw new Exception("Unable to create journal entry");
                    }
                    //Now add the New editable records for Acc.tblTransactionClass

                    foreach (int _AccClassID in AccClassID)
                    {
                        Global.m_db.ClearParameter();
                        Global.m_db.setCommandType(CommandType.StoredProcedure);
                        Global.m_db.setCommandText("Acc.spTransactClassCreate");
                        Global.m_db.AddParameter("@TransactID", SqlDbType.Int, ReturnTransactID3.ToString());
                        Global.m_db.AddParameter("@AccClassID", SqlDbType.Int, _AccClassID);//Set same for both for time being
                        Global.m_db.AddParameter("@RowID", SqlDbType.Int, paySlipID);
                        Global.m_db.AddParameter("VoucherType", SqlDbType.NVarChar, 20, "SALARY");
                        System.Data.SqlClient.SqlParameter paramTransactClassID = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                        Global.m_db.ProcessParameter();

                        if (paramTransactClassID.Value.ToString() == "FAILURE")
                        {
                            Global.m_db.RollBackTransaction();
                            throw new Exception("Unable to create journal entry");
                        }
                    }
                }
                #endregion

                #region Transaction for Nagarik Lagani Fund
                if (NLFundAmt > 0)
                {
                    ledgerName = Ledger.GetLedgerNameFromID(Global.NagarikLaganiFundID);
                    Global.m_db.ClearParameter();
                    Global.m_db.setCommandType(CommandType.StoredProcedure);
                    Global.m_db.setCommandText("Acc.spTransactCreate");
                    Global.m_db.AddParameter("@Date", SqlDbType.DateTime, date);
                    Global.m_db.AddParameter("@LedgerID", SqlDbType.Int, Global.NagarikLaganiFundID);
                    Global.m_db.AddParameter("@LedgerName", SqlDbType.NVarChar, 300, ledgerName);
                    Global.m_db.AddParameter("@Language", SqlDbType.NVarChar, 20, "ENGLISH");
                    Global.m_db.AddParameter("@Amount", SqlDbType.Money, NLFundAmt);
                    Global.m_db.AddParameter("@DrCr", SqlDbType.NVarChar, 20, "CREDIT");
                    Global.m_db.AddParameter("@VoucherType", SqlDbType.NVarChar, 20, "SALARY");
                    Global.m_db.AddParameter("@RowID", SqlDbType.Int, paySlipID);
                    Global.m_db.AddParameter("@ProjectID", SqlDbType.Int, 1);
                    System.Data.SqlClient.SqlParameter paramReturn3 = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                    Global.m_db.ProcessParameter();

                    int ReturnTransactID3 = Convert.ToInt32(paramReturn3.Value);
                    if (paramReturn3.Value.ToString() == "FAILURE")
                    {
                        Global.m_db.RollBackTransaction();
                        throw new Exception("Unable to create journal entry");
                    }
                    //Now add the New editable records for Acc.tblTransactionClass

                    foreach (int _AccClassID in AccClassID)
                    {
                        Global.m_db.ClearParameter();
                        Global.m_db.setCommandType(CommandType.StoredProcedure);
                        Global.m_db.setCommandText("Acc.spTransactClassCreate");
                        Global.m_db.AddParameter("@TransactID", SqlDbType.Int, ReturnTransactID3.ToString());
                        Global.m_db.AddParameter("@AccClassID", SqlDbType.Int, _AccClassID);//Set same for both for time being
                        Global.m_db.AddParameter("@RowID", SqlDbType.Int, paySlipID);
                        Global.m_db.AddParameter("VoucherType", SqlDbType.NVarChar, 20, "SALARY");
                        System.Data.SqlClient.SqlParameter paramTransactClassID = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                        Global.m_db.ProcessParameter();

                        if (paramTransactClassID.Value.ToString() == "FAILURE")
                        {
                            Global.m_db.RollBackTransaction();
                            throw new Exception("Unable to create journal entry");
                        }
                    }
                }
                #endregion

                //#region Transaction for Income From Interest
                //if (incomeIntAmt > 0)
                //{
                //    ledgerName = Ledger.GetLedgerNameFromID(Global.StaffLoanInterestID);
                //    Global.m_db.ClearParameter();
                //    Global.m_db.setCommandType(CommandType.StoredProcedure);
                //    Global.m_db.setCommandText("Acc.spTransactCreate");
                //    Global.m_db.AddParameter("@Date", SqlDbType.DateTime, date);
                //    Global.m_db.AddParameter("@LedgerID", SqlDbType.Int, Global.StaffLoanInterestID);
                //    Global.m_db.AddParameter("@LedgerName", SqlDbType.NVarChar, 300,ledgerName );
                //    Global.m_db.AddParameter("@Language", SqlDbType.NVarChar, 20, "ENGLISH");
                //    Global.m_db.AddParameter("@Amount", SqlDbType.Money, incomeIntAmt);
                //    Global.m_db.AddParameter("@DrCr", SqlDbType.NVarChar, 20, "CREDIT");
                //    Global.m_db.AddParameter("@VoucherType", SqlDbType.NVarChar, 20, "SALARY");
                //    Global.m_db.AddParameter("@RowID", SqlDbType.Int, paySlipID);
                //    Global.m_db.AddParameter("@ProjectID", SqlDbType.Int, 1);
                //    System.Data.SqlClient.SqlParameter paramReturn3 = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                //    Global.m_db.ProcessParameter();

                //    int ReturnTransactID3 = Convert.ToInt32(paramReturn3.Value);
                //    if (paramReturn3.Value.ToString() == "FAILURE")
                //    {
                //        Global.m_db.RollBackTransaction();
                //        throw new Exception("Unable to create journal entry");
                //    }
                //    //Now add the New editable records for Acc.tblTransactionClass

                //    foreach (int _AccClassID in AccClassID)
                //    {
                //        Global.m_db.ClearParameter();
                //        Global.m_db.setCommandType(CommandType.StoredProcedure);
                //        Global.m_db.setCommandText("Acc.spTransactClassCreate");
                //        Global.m_db.AddParameter("@TransactID", SqlDbType.Int, ReturnTransactID3.ToString());
                //        Global.m_db.AddParameter("@AccClassID", SqlDbType.Int, _AccClassID);//Set same for both for time being
                //        Global.m_db.AddParameter("@RowID", SqlDbType.Int, paySlipID);
                //        Global.m_db.AddParameter("VoucherType", SqlDbType.NVarChar, 20, "SALARY");
                //        System.Data.SqlClient.SqlParameter paramTransactClassID = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                //        Global.m_db.ProcessParameter();

                //        if (paramTransactClassID.Value.ToString() == "FAILURE")
                //        {
                //            Global.m_db.RollBackTransaction();
                //            throw new Exception("Unable to create journal entry");
                //        }
                //    }
                //}
                //#endregion

                #region Transaction for Net Salary/Bank Amount
                if (bankAmt > 0)
                {
                    ledgerName =Ledger.GetLedgerNameFromID(bankID);
                    Global.m_db.ClearParameter();
                    Global.m_db.setCommandType(CommandType.StoredProcedure);
                    Global.m_db.setCommandText("Acc.spTransactCreate");
                    Global.m_db.AddParameter("@Date", SqlDbType.DateTime, date);
                    Global.m_db.AddParameter("@LedgerID", SqlDbType.Int, bankID);
                    Global.m_db.AddParameter("@LedgerName", SqlDbType.NVarChar, 300, ledgerName );
                    Global.m_db.AddParameter("@Language", SqlDbType.NVarChar, 20, "ENGLISH");
                    Global.m_db.AddParameter("@Amount", SqlDbType.Money, bankAmt);
                    Global.m_db.AddParameter("@DrCr", SqlDbType.NVarChar, 20, "CREDIT");
                    Global.m_db.AddParameter("@VoucherType", SqlDbType.NVarChar, 20, "SALARY");
                    Global.m_db.AddParameter("@RowID", SqlDbType.Int, paySlipID);
                    Global.m_db.AddParameter("@ProjectID", SqlDbType.Int, 1);
                    System.Data.SqlClient.SqlParameter paramReturn3 = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                    Global.m_db.ProcessParameter();

                    int ReturnTransactID3 = Convert.ToInt32(paramReturn3.Value);
                    if (paramReturn3.Value.ToString() == "FAILURE")
                    {
                        Global.m_db.RollBackTransaction();
                        throw new Exception("Unable to create journal entry");
                    }
                    //Now add the New editable records for Acc.tblTransactionClass

                    foreach (int _AccClassID in AccClassID)
                    {
                        Global.m_db.ClearParameter();
                        Global.m_db.setCommandType(CommandType.StoredProcedure);
                        Global.m_db.setCommandText("Acc.spTransactClassCreate");
                        Global.m_db.AddParameter("@TransactID", SqlDbType.Int, ReturnTransactID3.ToString());
                        Global.m_db.AddParameter("@AccClassID", SqlDbType.Int, _AccClassID);//Set same for both for time being
                        Global.m_db.AddParameter("@RowID", SqlDbType.Int, paySlipID);
                        Global.m_db.AddParameter("VoucherType", SqlDbType.NVarChar, 20, "SALARY");
                        System.Data.SqlClient.SqlParameter paramTransactClassID = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                        Global.m_db.ProcessParameter();

                        if (paramTransactClassID.Value.ToString() == "FAILURE")
                        {
                            Global.m_db.RollBackTransaction();
                            throw new Exception("Unable to create journal entry");
                        }
                    }
                }
                #endregion

                Global.m_db.CommitTransaction();
            }
            catch (Exception ex)
            {
                Global.m_db.RollBackTransaction();
                throw new Exception("Unable to create journal entry");
            }
        }

        public int VoucherEntered(int paySlipID,int journalId)
        {
            string str = "update HRM.tblSalaryPayslipMaster set isVoucherEntered=1,JournalID =" + journalId + " where salaryPaySlipID=" + paySlipID;
            return Global.m_db.InsertUpdateQry(str);
        }

        public int VoucherDeleted(int paySlipID, int journalId)
        {
            string str = "update HRM.tblSalaryPayslipMaster set isVoucherEntered=0,JournalID =0 where salaryPaySlipID=" + paySlipID  + " and JournalID = "+journalId;
            return Global.m_db.InsertUpdateQry(str);
        }

        public static int PTVoucherEntered(int MasterID, int journalId)
        {
            string str = "update HRM.tblPartTimeSalaryMaster set JournalID =" + journalId + " where ID=" + MasterID;
            return Global.m_db.InsertUpdateQry(str);
        }

        public static int PTVoucherDeleted(int MasterID, int journalId)
        {
            string str = "update HRM.tblPartTimeSalaryMaster set JournalID =0 where ID=" + MasterID + " and JournalID = " + journalId;
            return Global.m_db.InsertUpdateQry(str);
        }

        public static int PTGetIDbySN(int sn)
        {
            string str = "Select ID from HRM.tblPartTimeSalaryMaster where SN = '"+sn+"'";
            return Convert.ToInt32(Global.m_db.GetScalarValue(str));
        }

        public static DataTable GetLoanReport(int loanID, int month, int year, ref bool isFixedInstallment)
        {
            string str = "HRM.spGetLoanReport";

            #region old code
            //if (!isFixedInstallment)
            //{
            //    str = " Select distinct PSMD.EmployeeID,PSMD.StaffCode,PSMD.StaffName,ed.DesignationName, "
            //            + " (EL.LoanMthPremium + EL.LoanMthDecreaseAmt) LoanMthPremium, es.PFNumber, EL.LoanPrincipal, EL.LoanMthInstallment,EL.LoanMthInterest LoanMthInterest, "
            //            + " (EL.LoanMthPremium + EL.LoanMthDecreaseAmt + EL.LoanMthInterest) Total, '' Remarks "
            //            + " From HRM.tblPaySlipMasterDetails PSMD "
            //            + " left join HRM.tblSalaryPayslipMaster PSM on PSMD.paySlipId = PSM.salaryPaySlipID	"
            //            + " left join  HRM.tblEmployeeLoan EL on PSMD.EmployeeID = EL.EmpID "
            //            + " left join HRM.tbldesignation ed on PSMD.designationID = ed.DesignationID "
            //            + " left join HRM.tblEmployeeSalaryInfo es on PSMD.EmployeeID = es.EmployeeID "
            //            + " Where EL.LoanRemainingMth > -1 and PSMD.EmpPresence = 1 and PSM.monthID = @month and PSM.year = @year and EL.LoanID =@loanID "
            //            + " and EL.LoanTotalMth > EL.LoanRemainingMth "
            //            ;
            //}
            //else
            //{

            //    str = "     Select distinct PSMD.EmployeeID,PSMD.StaffCode,PSMD.StaffName,ed.DesignationName,EL.LoanMthPremium,es.PFNumber"
            //            + " From HRM.tblPaySlipMasterDetails PSMD "
            //            + " left join HRM.tblSalaryPayslipMaster PSM on PSMD.paySlipId = PSM.salaryPaySlipID"
            //            + " left join  HRM.tblEmployeeLoan EL on PSMD.EmployeeID = EL.EmpID "
            //            + " left join HRM.tbldesignation ed on PSMD.designationID = ed.DesignationID "
            //            + " left join HRM.tblEmployeeSalaryInfo es on PSMD.EmployeeID= es.EmployeeID"
            //            + " Where EL.LoanRemainingMth > -1 and PSMD.EmpPresence = 1 and PSM.monthID = @month and PSM.year = @year and EL.LoanID =@loanID";

            //} 
            #endregion
            
            Global.m_db.ClearParameter();
            Global.m_db.setCommandType(CommandType.StoredProcedure);
            Global.m_db.setCommandText(str);
            Global.m_db.AddParameter("@month", SqlDbType.Int, month);
            Global.m_db.AddParameter("@year", SqlDbType.Int, year);
            Global.m_db.AddParameter("@loanID", SqlDbType.Int, loanID);
            SqlParameter objReturnID =  (SqlParameter)Global.m_db.AddOutputParameter("@isFixedInsallment", SqlDbType.Bit);

            DataTable dt = Global.m_db.GetDataTable();
            isFixedInstallment = Convert.ToBoolean(Convert.ToInt32(objReturnID.Value));

            return dt;
        }
        //public static DataTable GetLoanReport(int loanID,int month,int year)
        //{
        //    string str = "Select PSMD.EmployeeID,PSMD.StaffCode,PSMD.StaffName,EL.LoanMthPremium From HRM.tblPaySlipMasterDetails PSMD left join HRM.tblSalaryPayslipMaster PSM on PSMD.paySlipId = PSM.salaryPaySlipID	left join  HRM.tblEmployeeLoan EL on PSMD.EmployeeID = EL.EmpID Where EL.LoanRemainingMth > -1 and PSM.monthID = @month and PSM.year = @year and EL.LoanID =@loanID";
        //    Global.m_db.ClearParameter();
        //    Global.m_db.setCommandType(CommandType.Text);
        //    Global.m_db.setCommandText(str);
        //    Global.m_db.AddParameter("@month", SqlDbType.Int, month);
        //    Global.m_db.AddParameter("@year", SqlDbType.Int, year);
        //    Global.m_db.AddParameter("@loanID", SqlDbType.Int, loanID);
        //    return Global.m_db.GetDataTable();
        //}

        public static DataTable GetAdvanceReport(int month, int year)
        {
            string str = "";
            string dateStr = "";
            if (Global.Default_Date == Date.DateType.English)
            {
                dateStr = " ea.TakenDate ";
            }
            else
            {
                dateStr = " Date.fnEngToNep(ea.TakenDate) TakenDate ";
            }
            str = "select distinct PSMD.EmployeeID EmployeeID, DENSE_RANK() OVER (ORDER BY  EmployeeID) AS SN, PSMD.StaffCode, PSMD.StaffName, ea.AdvTitle, "+dateStr+", Installment, TotalAmt  from HRM.tblPaySlipMasterDetails PSMD left join HRM.tblSalaryPayslipMaster PSM on PSMD.paySlipId = PSM.salaryPaySlipID left join HRM.tblEmployeeAdvance ea on PSMD.EmployeeID = ea.EmpID where ea.RemainingAmt > 0 and PSM.monthID = @month and PSM.year = @year";
            Global.m_db.ClearParameter();
            Global.m_db.setCommandType(CommandType.Text);
            Global.m_db.setCommandText(str);
            Global.m_db.AddParameter("@month", SqlDbType.Int, month);
            Global.m_db.AddParameter("@year", SqlDbType.Int, year);
            return Global.m_db.GetDataTable();
        }

        public static DataTable GetDetailedEmployeeReport(DetailEmployeeReportSettings m_settings)
        {
            try
            {
                Global.m_db.ClearParameter();
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("HRM.spEmployeeReport");
                Global.m_db.AddParameter("@DepartmentID", SqlDbType.Int, m_settings.DepartmentID);
                Global.m_db.AddParameter("@DesignationID", SqlDbType.Int, m_settings.DesignationID);
                Global.m_db.AddParameter("@FacultyID", SqlDbType.Int, m_settings.FacultyID);
                Global.m_db.AddParameter("@LevelID", SqlDbType.Int, m_settings.JobTypeID);
                Global.m_db.AddParameter("@Status", SqlDbType.Int, m_settings.StatusID);
                Global.m_db.AddParameter("@JobType", SqlDbType.Int, m_settings.LevelID);
                Global.m_db.AddParameter("@DefaultDate", SqlDbType.NVarChar, Global.Default_Date);

                return Global.m_db.GetDataTable();
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        public static DataTable GetPensionReport(EmployeeReportSettings settings)//(int[] paySlipIDs, bool isRemaining, string date)
        {
            string paySlipids = ConvertIDsToString(settings.paySlipIds);
            string dateStr = "";
            if (settings.paySlipDate != null)
            {
                dateStr = " and convert(date,md.PaySlipDate) = '" + settings.paySlipDate + "'";
            }

            if (settings.fromDate != null && settings.toDate != null)
            {
                dateStr = "and md.PaySlipDate between '" + Convert.ToDateTime(settings.fromDate).ToShortDateString() + "' and '" + Convert.ToDateTime(settings.toDate).ToShortDateString() + " 23:59:59:999' ";
            }
            string qry = "	select d.DesignationName,md.StaffName,si.PensionNumber,md.PensionFAmt as PensionAmt,md.PensionFDeduct as pfTotal "
                + "from HRM.tblPaySlipMasterDetails md join HRM.tbldesignation d  on md.designationID=d.DesignationID"
                +" join HRM.tblEmployeeSalaryInfo si on md.EmployeeID = si.EmployeeID"
                +" where si.IsPension = 'True' and md.EmpPresence = " + Convert.ToInt32(!settings.isRemaining) 
                + " and paySlipId in ( " + paySlipids + " ) " + dateStr + " order by md.StaffName;";
            Global.m_db.ClearParameter();
            Global.m_db.setCommandText(qry);
            Global.m_db.setCommandType(CommandType.Text);
            //Global.m_db.AddParameter("@paySlipID", SqlDbType.NVarChar, paySlipids);
            return Global.m_db.GetDataTable();
        }

        public static DataTable GetCITReport(EmployeeReportSettings settings)//(int[] paySlipIDs, bool isRemaining, string date)
        {
            string paySlipids = ConvertIDsToString(settings.paySlipIds);
            string dateStr = "";
            if (settings.paySlipDate != null)
            {
                dateStr = " and convert(date,md.PaySlipDate) = '" + settings.paySlipDate + "'";
            }
            if (settings.fromDate != null && settings.toDate != null)
            {
                dateStr = "and md.PaySlipDate between '" + Convert.ToDateTime(settings.fromDate).ToShortDateString() + "' and '" + Convert.ToDateTime(settings.toDate).ToShortDateString() + " 23:59:59:999' ";
            }
            string qry = "select d.DesignationName,md.StaffName,si.NLKoshNo CITNumber,md.NLKoshDeduct as CITAmt,md.NLKoshDeduct as CITTotal "
                + " from HRM.tblPaySlipMasterDetails md join HRM.tbldesignation d on  md.designationID=d.DesignationID "
                + " join HRM.tblEmployeeSalaryInfo si on md.EmployeeID = si.EmployeeID where md.EmpPresence = " + Convert.ToInt32(!settings.isRemaining)
                + " and si.NLKoshNo != '' and si.NLKoshNo is not null "
                + " and paySlipId in ( " + paySlipids + " )" + dateStr + "  and md.NLKoshDeduct >0 order by md.StaffName;";
            Global.m_db.ClearParameter();
            Global.m_db.setCommandText(qry);
            Global.m_db.setCommandType(CommandType.Text);
            //Global.m_db.AddParameter("@paySlipID", SqlDbType.NVarChar, paySlipids);
            return Global.m_db.GetDataTable();
        }

        public static DataTable GetTaxReport(EmployeeReportSettings settings)//(int[] paySlipIDs, bool isRemaining, string date)
        {
            string paySlipids = ConvertIDsToString(settings.paySlipIds);
            string dateStr = "";
            if (settings.paySlipDate != null)
            {
                dateStr = " and convert(date,md.PaySlipDate) = '" + settings.paySlipDate + "'";
            }

            if (settings.fromDate != null && settings.toDate != null)
            {
                dateStr = "and md.PaySlipDate between '" + Convert.ToDateTime(settings.fromDate).ToShortDateString() + "' and '" + Convert.ToDateTime(settings.toDate).ToShortDateString() + " 23:59:59:999' ";
            }
            string qry = "select md.EmployeeID, d.DesignationName, md.StaffName,  si.PAN TaxNumber,md.grossAmount GrossAmt,md.taxDeduct as TaxAmt, isnull(md.OnePercentTax,0) OnePercentTax,"
                +" Date.fnEngtoNep(md.PaySlipDate)  Date , (md.taxDeduct -isnull(md.OnePercentTax, 0)) 'RemainingTax', md.NLKoshDeduct as TaxTotal "
                + " from HRM.tblPaySlipMasterDetails md join HRM.tbldesignation d on md.designationID=d.DesignationID join HRM.tblEmployeeSalaryInfo si  "
                + " on md.EmployeeID = si.EmployeeID where "
                //+" si.PAN != '' and si.PAN is not null and "
                +" md.EmpPresence = " + Convert.ToInt32(!settings.isRemaining) + " and paySlipId in ( " + paySlipids + " ) " + dateStr + " order by md.StaffName;";
            Global.m_db.ClearParameter();
            Global.m_db.setCommandText(qry);
            Global.m_db.setCommandType(CommandType.Text);
            //Global.m_db.AddParameter("@paySlipID", SqlDbType.NVarChar, paySlipids);
            return Global.m_db.GetDataTable();
        }

        public static DataTable GetWelfareReport(EmployeeReportSettings settings)//(int[] paySlipIDs, bool isRemaining, string date)
        {
            string paySlipids = ConvertIDsToString(settings.paySlipIds);
            string dateStr = "";
            if (settings.paySlipDate != null)
            {
                dateStr = " and convert(date,md.PaySlipDate) = '" + settings.paySlipDate + "'";
            }

            if (settings.fromDate != null && settings.toDate != null)
            {
                dateStr = "and md.PaySlipDate between '" + Convert.ToDateTime(settings.fromDate).ToShortDateString() + "' and '" 
                    + Convert.ToDateTime(settings.toDate).ToShortDateString() + " 23:59:59:999' ";
            }
            string qry = "select d.DesignationName,md.StaffName, si.KalyankariNo KalyankariNo,md.KalyankariAmt from HRM.tblPaySlipMasterDetails md join"
                + " HRM.tbldesignation d on md.designationID=d.DesignationID join HRM.tblEmployeeSalaryInfo si on md.EmployeeID = si.EmployeeID where "
                + "  md.EmpPresence = " + Convert.ToInt32(!settings.isRemaining)
                + " and (si.KalyankariNo != '' AND SI.KalyankariNo IS NOT NULL) "
                + " and paySlipId in ( " + paySlipids + " ) and " + dateStr
                +"  md.KalyankariAmt >0 order by md.StaffName;";
            Global.m_db.ClearParameter();
            Global.m_db.setCommandText(qry);
            Global.m_db.setCommandType(CommandType.Text);
            //Global.m_db.AddParameter("@paySlipID", SqlDbType.NVarChar, paySlipids);
            return Global.m_db.GetDataTable();
        }

        public static DataTable GetSalarySheetDate(int month, int year)
        {
            string sql = "select distinct Date.fnEngtoNep(PaySlipDate) PaySlipDate from HRM.tblPaySlipMasterDetails where paySlipId in (select salaryPaySlipID from HRM.tblSalaryPayslipMaster where monthID = " + month + " and year = " + year + ")";
            return Global.m_db.SelectQry(sql, "tblPaySlipMasterDetails");
        }

        public static int UpdateEmployeeGrade()
        {
            try
            {
                Global.m_db.ClearParameter();
                Global.m_db.setCommandText("[HRM].[spUpdateGrade]");
                Global.m_db.setCommandType(CommandType.StoredProcedure);

                return Global.m_db.ProcessParameter();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable GetEmployeeList(int FacultyID = 0)
        {
            try
            {
                string FacultyStr = "";
                if (FacultyID > 0)
                {
                    FacultyStr = " where si.FacultyID = "+FacultyID;
                }
                return Global.m_db.SelectQry("select e.ID ID, CONCAT(FirstName, ' ', MiddleName, ' ', LastName) Name "
                                               + "from HRM.tblEmployee e "
                                               + "left join HRM.tblEmployeeSalaryInfo si on e.ID = si.EmployeeID "
                                               +  FacultyStr , "HRM.tblEmployee");
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }
        public static DataTable GetAnnualEmpReport(int EmployeeID, string Year)
        {
            try
            {
                //string sqlStmt = "select  d.BasicSalary, (d.AcademicAlw + d.AdmistrativeAlw + d.OverTimeAlw) Allowance,"
                //+" d.PFAmount , d.PensionFAmt ,d.FestivalAlw , d.grossAmount, d.pfDeduct, d.PensionFDeduct, d.NLKoshDeduct, d.LoanMthPremium,  "
                //+" d.AdvMthInstallment, d.KalyankariAmt, d.taxDeduct, d.ElectricityDeduct,0 'Rent',m.monthID, m.monthName, m.year"
                //+"  from HRM.tblPaySlipMasterDetails d "
                //+" join HRM.tblSalaryPayslipMaster m on d.paySlipId = m.salaryPaySlipID "
                //+ " where EmployeeID = " + EmployeeID + " and d.EmpPresence = 1 and ((year = " + year + " and monthID >= 4 and monthID <=12) or (year = " + year + 1 + " and monthID >= 1 and monthID <=3) ) order by m.monthID, m.year";

                Global.m_db.ClearParameter();

                Global.m_db.setCommandText("HRM.spGetEmployeeAnnualSalaryReport ");
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.AddParameter("@EmployeeID", SqlDbType.Int, EmployeeID);
                Global.m_db.AddParameter("@Year", SqlDbType.NVarChar, Year);

                return Global.m_db.GetDataTable();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable GetAnnualTaxAdjustReport(string Year, int FromMonth, int ToMonth, int FacultyID)
        {
            try
            {
                Global.m_db.ClearParameter();

                Global.m_db.setCommandText("HRM.spAnnualTaxAdjustment");
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.AddParameter("@Year", SqlDbType.NVarChar, Year);
                Global.m_db.AddParameter("@fromMonth", SqlDbType.Int, FromMonth);
                Global.m_db.AddParameter("@toMonth", SqlDbType.Int, ToMonth);
                Global.m_db.AddParameter("@facultyID", SqlDbType.Int, FacultyID);

                return Global.m_db.GetDataTable();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
    