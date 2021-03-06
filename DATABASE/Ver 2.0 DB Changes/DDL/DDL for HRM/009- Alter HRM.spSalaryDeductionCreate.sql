USE [BISH001]
GO
/****** Object:  StoredProcedure [HRM].[spSalaryDeductionCreate]    Script Date: 16-Jun-16 4:32:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER procedure [HRM].[spSalaryDeductionCreate]
(
@employeeID int=null,
@salaryPaySlipID int=null,
@employeeName nvarchar(30)=null,
@deductionID nvarchar(30)=null,
@deductionAmount float=null,
@Return nvarchar(20) output
)
AS
insert into HRM.tblPaySlipDetailDeduction(employeeID,salaryPaySlipID,employeeName,deductionID,deductionAmount)
values(@employeeID,@salaryPaySlipID,@employeeName,@deductionID,@deductionAmount)

SET @Return='SUCCESS';

	if @@Error<>0
	begin
		raiserror('An error occured during Creation of Payslip Deduction . Please check the fields and try again!',15,1);
		SET @Return='FAILURE';
		RETURN -100
	end