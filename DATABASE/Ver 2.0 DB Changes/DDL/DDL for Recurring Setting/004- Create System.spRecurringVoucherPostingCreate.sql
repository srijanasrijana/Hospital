
/****** Object:  StoredProcedure [System].[spRecurringVoucherPostingCreate]    Script Date: 8/26/2016 4:23:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
create PROCEDURE [System].[spRecurringVoucherPostingCreate](

@VoucherType NVARCHAR(50),
@Date date,
@VoucherID NVARCHAR(20),
@isPosted bit,
@Return NVARCHAR(20) OUTPUT
)
AS
 INSERT INTO System.tblRecurringVoucherPosting VALUES(  @VoucherType , @Date , @VoucherID, @isPosted )
 if @@Error<>0
	begin
		raiserror('An error occured during addition of recurring settings. Please check the fields and try again!',15,1);
		SET @Return='FAILURE';
		RETURN -100
	end