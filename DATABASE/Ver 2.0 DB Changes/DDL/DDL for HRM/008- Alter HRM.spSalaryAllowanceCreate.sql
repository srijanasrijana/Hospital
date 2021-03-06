USE [BISH001]
GO
/****** Object:  StoredProcedure [HRM].[spSalaryAllowanceCreate]    Script Date: 16-Jun-16 4:28:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER procedure [HRM].[spSalaryAllowanceCreate]
(
@salaryPaySlipID int=null,
@employeeID int=null ,
@employeeName nvarchar(30)=null,
@allowanceID int=null,
@allowanceAmount float=null,
@Return nvarchar(20) output
)
AS
insert into HRM.tblPaySlipDetailAllowances(salaryPaySlipID,employeeID,employeeName,allowanceID,allowanceAmount)
values(@salaryPaySlipID,@employeeID,@employeeName,@allowanceID,@allowanceAmount)

SET @Return='SUCCESS';

	if @@Error<>0
	begin
		raiserror('An error occured during Creation of Payslip Allowances . Please check the fields and try again!',15,1);
		SET @Return='FAILURE';
		RETURN -100
	end