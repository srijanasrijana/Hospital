-- =============================================
-- Author:		<Sunil Shrestha>
-- Create date: <15-June-2016>
-- Description:	<Deletes all the records of a payslip entry of an employee on a particular month of the current fisal year>
-- =============================================
Create Proc [HRM].[spPaySlipDelete]
(
@monthID int=null,
@empId int=null,
@FYStart DateTime = null,
@Return nvarchar(20) output
)
as
BEGIN TRY
	Declare @psId int = 0;
	SELECT @psId = d.paySlipId FROM HRM.tblPaySlipMasterDetails d INNER JOIN HRM.tblSalaryPayslipMaster m ON d.paySlipId = m.salaryPaySlipID where m.monthID=@monthID and d.employeeID=@empId and m.date >= @FYStart;
	if(@psId <> 0)
	begin
		delete from HRM.tblPaySlipDetailAllowances where salaryPaySlipID = @psId and employeeID = @empId;
		delete from HRM.tblPaySlipDetailDeduction where salaryPaySlipID = @psId and employeeID = @empId;
		delete from HRM.tblPaySlipMasterDetails where paySlipId = @psId and employeeID = @empId;
		declare @psIdTemp int = 0;
		SELECT top 1 @psIdTemp = d.paySlipId FROM HRM.tblPaySlipMasterDetails d INNER JOIN HRM.tblSalaryPayslipMaster m ON d.paySlipId = m.salaryPaySlipID where m.monthID=@monthID and d.paySlipId = @psId;
		if(@psIdTemp = 0)
		begin
			delete from HRM.tblSalaryPayslipMaster where salaryPaySlipID = @psId;
		end
		SET @Return='SUCCESS';
	end
END TRY
BEGIN CATCH
	SET @Return='FAILURE';
END CATCH