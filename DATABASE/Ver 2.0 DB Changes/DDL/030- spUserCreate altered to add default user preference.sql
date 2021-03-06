
/****** Object:  StoredProcedure [System].[spUserCreate]    Script Date: 10-May-16 4:52:23 PM ******/
--ALtered by Sunil Shrestha for default user preference of new user at 10th-May-2016
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


ALTER PROCEDURE [System].[spUserCreate](
@UserName NVARCHAR(150)=NULL,
@Password NVARCHAR(150),
@Name NVARCHAR(150),
@Address NVARCHAR(150),
@Contact NVARCHAR(150),
@Email NVARCHAR(150),
@Department NVARCHAR(150),
@AccessRoleID INT,
@AccClassID INT,
@Created_By NVARCHAR(20) = NULL,
@Return NVARCHAR(20) OUTPUT
)
AS

--DO THE REAL INSERT
	INSERT INTO System.tblUser VALUES(
										@UserName,@Password,@Name,@Address,@Contact,@Email,@Department,@AccessRoleID,@AccClassID,@Created_By,GetDate(),null,null,1
										)

	SET @Return=scope_identity();

	if @@Error<>0
	begin
		raiserror('An error occured during addition of User. Please check the fields and try again!',15,1);
		SET @Return='FAILURE';
		RETURN -100
	end
	else --Insert default user preference(taking user preference of 'root' as default) for the new user if user create successfull.
	begin
		insert into System.tblUser_Preference (UserID,PreferenceID,Value) select @Return,PreferenceID,Value from System.tblUser_Preference where UserID = 1
	end
