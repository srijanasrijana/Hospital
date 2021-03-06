USE [BISH001]
GO
/****** Object:  StoredProcedure [HRM].[spPaySlipMasterCreate]    Script Date: 15-Jun-16 10:50:03 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER procedure [HRM].[spPaySlipMasterCreate]
(
@monthID int=null,
@monthName nvarchar(25)=null,
@date datetime=null,
@createdBy nvarchar(30)=null,
@modifiedBy nvarchar(30)=null,
@Return nvarchar(20) Output
)
AS
insert into HRM.tblSalaryPayslipMaster(monthID,monthName,date,createdBy)
values(@monthID,@monthName,@date,@createdBy)

SET @Return=scope_identity();

	if @@Error<>0
	begin
		raiserror('An error occured during Creation of Salary PaySlip Master . Please check the fields and try again!',15,1);
		SET @Return='FAILURE';
		RETURN -100
	end