--USE [BENT003]
--GO
/****** Object:  StoredProcedure [System].[spVoucherRecurringCreate]    Script Date: 10/5/2016 1:31:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [System].[spVoucherRecurringCreate](
@VoucherID NVARCHAR(50),
@VoucherType NVARCHAR(50),
@Description NVARCHAR(500),
@RecurringType NVARCHAR(20),
@Unit1 NVARCHAR(50),
@Unit2 NVARCHAR(50),
@Date date,
@Return NVARCHAR(20) OUTPUT
)
AS
 INSERT INTO System.tblRecurringVoucher VALUES( @VoucherID , @VoucherType , @Description , @RecurringType, @Unit1 , @Unit2, @Date )
 if @@Error<>0
	begin
		raiserror('An error occured during addition of recurring settings. Please check the fields and try again!',15,1);
		SET @Return='FAILURE';
		RETURN -100
	end