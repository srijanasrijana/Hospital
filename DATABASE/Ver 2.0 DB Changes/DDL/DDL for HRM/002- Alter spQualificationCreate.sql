ALTER procedure [HRM].[spQualificationCreate]
(
@EmployeeID int=null,
@InstituteName nvarchar(100)=null,
@Board nvarchar(200)=null,
@Course nvarchar(200)=null,
@Percentage nvarchar(20)=null,
@PassYear int=null,
@Return nvarchar(20) output
)
AS
insert into Hrm.tblAcademicQualification(InstituteName,Board,Course,Percentage,PassYear,EmployeeID)
 values(@InstituteName,@Board,@Course,@Percentage,@PassYear,@EmployeeID)

SET @Return='SUCCESS';

	if @@Error<>0
	begin
		raiserror('An error occured during addition of Employee Qualification. Please check the fields and try again!',15,1);
		SET @Return='FAILURE';
		RETURN -100
	end