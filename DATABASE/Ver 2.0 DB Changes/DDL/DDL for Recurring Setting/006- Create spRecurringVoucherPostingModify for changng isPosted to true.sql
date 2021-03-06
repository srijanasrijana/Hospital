
/****** Object:  StoredProcedure [System].[spRecurringVoucherPostingModify]    Script Date: 10/5/2016 12:34:21 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
create PROCEDURE [System].[spRecurringVoucherPostingModify](

--@VoucherType NVARCHAR(50),
--@VoucherID NVARCHAR(20),
@RVPID int ,
@isPosted bit,
@Return NVARCHAR(20) OUTPUT
)
AS
update System.tblRecurringVoucherPosting set isPosted = @isPosted where RVPID = @RVPID; --VoucherID=@VoucherID and VoucherType= @VoucherType
 if @@Error<>0
	begin
		raiserror('An error occured during modification of recurring posting. Please check the fields and try again!',15,1);
		SET @Return='FAILURE';
		RETURN -100
	end