--USE [BENT003]
--GO
/****** Object:  StoredProcedure [System].[spDeleteReference]    Script Date: 1/31/2017 11:30:42 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
create PROCEDURE [System].[spDeleteReference]
	(
		@VouID int = null,
		@VouType nvarchar(500) ,
		@ReturnResult nvarchar(100) output
		--@LedgerID int = null,
		--@AccountClassIDsSettings  xml,     -- It encludes AccountCLassIDs  Info
		--@ProjectIDsSettings xml           -- It includes ProjectIDs Info
	)
AS
BEGIN
	BEGIN TRANSACTION[T]
	BEGIN TRY
		Declare @RefID int = 0;
		Declare @RefVouID int = 0,@isAgainst bit,@doDelete bit = 1;
	
		Declare cur CURSOR
		Static FOR
		Select RVID, RefID,IsAgainst from System.tblReferenceVoucher where VoucherID=@VouID and VoucherType = @VouType;
		OPEN cur
		IF @@CURSOR_ROWS > 0
		BEGIN
			FETCH NEXT FROM cur INTO @RefVouID,@RefID,@isAgainst;
			While @@FETCH_STATUS = 0 AND @doDelete = 1
			BEGIN
				IF(@isAgainst = 0)
				BEGIN
					Declare @TempRefID int = 0;
					Select TOP 1  @TempRefID = RefID from System.tblReferenceVoucher where IsAgainst = 1 and RefID = @RefID;
					IF(@TempRefID > 0)
					BEGIN
						Set @doDelete = 0;
					END
				END
				FETCH NEXT FROM cur INTO @RefVouID,@RefID,@isAgainst;
			END
			IF @doDelete = 1
			BEGIN
				Declare @tblDeleteRef Table(
				refID int
				);
				Insert into @tblDeleteRef select RefID as refID from System.tblReferenceVoucher where VoucherID = @VouID and VoucherType = @VouType;
				--select * from @tblDeleteRef;

				Delete from System.tblReferenceVoucher where VoucherID  = @VouID and VoucherType = @VouType;
				Delete from System.tblReference where RefID in(select a.refID from @tblDeleteRef as a 
					where (select count(*) from System.tblReferenceVoucher as b where a.refID = b.RefID) = 0
					and (select count(RefID) from System.tblReferenceVoucher as c where a.refID = c.RefID and VoucherID = @VouID and VoucherType = @VouType and IsAgainst = 0) = 0);
				set @ReturnResult ='Success';
			END

			else if @doDelete = 0 
			Begin
				set @ReturnResult='Existing references !';
			end
		END
		else -- if the given voucher is not in reference table then set message so that it can be deleted
			set @ReturnResult = 'Success';

		CLOSE cur;
		DEALLOCATE cur;
	END TRY

	BEGIN CATCH
		ROLLBACK TRANSACTION[T];
		CLOSE cur;
		DEALLOCATE cur;
		RAISERROR('Problem while deleting reference',16,1)
	END CATCH
	COMMIT TRANSACTION[T]
END