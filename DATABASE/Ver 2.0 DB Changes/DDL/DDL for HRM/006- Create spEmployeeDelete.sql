-- =============================================
-- Author:		<Sunil Shrestha>
-- Create date: <16-June-2016>
-- Description:	<Deletes all the records of an employee>
-- =============================================
Create Proc [HRM].[spEmployeeDelete]
(
@empId int=null,
@Return nvarchar(20) output
)
as
BEGIN TRY
	
		delete from HRM.tblEmployeeSalaryInfo where EmployeeID = @empId;
		delete from HRM.tblEmployeePhoto where EmployeeID = @empId;
		delete from HRM.tblWorkExperiences where EmployeeID = @empId;
		delete from HRM.tblAcademicQualification where EmployeeID = @empId;
		delete from HRM.tblEmployeementDetails where EmployeeID = @empId;
		delete from HRM.tblEmployee where ID = @empId;
		SET @Return='SUCCESS';
	
END TRY
BEGIN CATCH
	SET @Return='FAILURE';
END CATCH