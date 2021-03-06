
/****** Object:  StoredProcedure [Acc].[spAccessRoleCreate]    Script Date: 11/2/2015 7:24:10 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [Acc].[spAccessRoleCreate](
@EngName NVARCHAR(50)=NULL,
@NepName NVARCHAR(50)=NULL,
@Created_By NVARCHAR(20) = NULL,
@Return NVARCHAR(20) OUTPUT
)
AS

SET NOCOUNT ON

	if(@EngName=NULL AND @NepName=NULL)
	begin
		raiserror('Invalid Role Name!',15,1);
		SET @Return='FAILURE';
		RETURN -100
	end

	if(@EngName is not null)
	begin
		INSERT INTO System.tblAccessRole(EngName,Created_By,Created_Date,BuiltIn) VALUES(@EngName, @Created_By, GetDate(),0);
	end
	else if(@NepName is not null)
	begin
		INSERT INTO System.tblAccessRole(NepName,Created_By,Created_Date,BuiltIn) VALUES(@NepName, @Created_By, GetDate(),0);
	end

	SET @Return=scope_identity();

	if @@Error<>0
	begin
		raiserror('An error occured during addition of accounnting group. Please check the fields and try again!',15,1);
		SET @Return='FAILURE';
		RETURN -100
	end