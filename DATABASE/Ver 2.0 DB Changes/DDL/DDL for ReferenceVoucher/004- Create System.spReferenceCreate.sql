
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE System.spReferenceCreate
	(
		@ref nvarchar(500) = null,
		@ledgerID int = null,
		@voucherID int = null,
		@voucherType nvarchar(50) = null,
		@isAgainst bit = null,
		@Return nvarchar(20) output
	)
AS
BEGIN
	BEGIN TRANSACTION[T]
	BEGIN TRY
		Declare @refID int = 0;
		Insert into System.tblReference values (@ref,@ledgerID);
		set @refID = SCOPE_IDENTITY();

		IF(@refID > 0)
		BEGIN
			Insert into System.tblReferenceVoucher values (@refID,@voucherID,@voucherType,@isAgainst);
		END
		ELSE
		BEGIN
			ROLLBACK TRANSACTION[T];
			RAISERROR('Problem while inserting reference',16,1);
			SET @Return = 'Failure';
		END
	COMMIT TRANSACTION[T]
	SET @Return = @refID;
	END TRY

	BEGIN CATCH

		ROLLBACK TRANSACTION[T]
		RAISERROR('Problem while inserting reference',16,1)
		SET @Return = 'Failure';
	END CATCH
END
GO
