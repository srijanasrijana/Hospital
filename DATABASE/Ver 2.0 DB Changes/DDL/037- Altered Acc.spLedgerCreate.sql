USE [PNCA001]
GO
/****** Object:  StoredProcedure [Acc].[spLedgerCreate]    Script Date: 11-Aug-16 2:00:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


ALTER PROCEDURE [Acc].[spLedgerCreate]
(
@Name NVARCHAR(300),
@LedgerCode NVARCHAR(50),
@GroupID INT,
@PreYrBal MONEY = NULL,
@PreYrBalDrCr NVARCHAR(20)=NULL,
@OpCCYID INT=NULL,
@OpCCR MONEY=NULL,
@OpCCRDate DateTime=NULL,
@Remarks NVARCHAR(200)=NULL,
@PerName NVARCHAR(50)=NULL,
@Address1 NVARCHAR(50)=NULL,
@Address2 NVARCHAR(50)=NULL,
@City NVARCHAR(50)=NULL,
@Phone NVARCHAR(50)=NULL,
@Email NVARCHAR(50)=NULL,
@Company NVARCHAR(50)=NULL,
@Website NVARCHAR(50)=NULL,
@VatPanNo	NVARCHAR(50)= NULL,
@CreditLimit MONEY = 0,
@CalculateChecked Bit=0,
@Rate float=0.00, 
@Created_By NVARCHAR(20),
@ReturnID  NVARCHAR(20) OUTPUT,
@Return NVARCHAR(20) OUTPUT
)
AS
SET NOCOUNT ON

	DECLARE @DrCr NVARCHAR(3)

	--VALIDATE INPUTS
	IF @GroupID IS NULL
	BEGIN
		RAISERROR('Invalid Group Name',15,1)
		RETURN -100
	END
	

	--GET DR/CR type from its group behaviour
	SET @DrCr=Acc.fnGetDrCrTypeFromGroup(@GroupID)

	--Finally insert into tblledger
	INSERT INTO Acc.tblLedger VALUES(   @LedgerCode,
										0,
										@Name,
										@Name,
                                        @PreYrBal,
                                        @PreYrBalDrCr,
										@OpCCYID,
										@OpCCR,
										@OpCCRDate,
										@DrCr,
										@GroupID,
										@Remarks,
										@PerName,
										@Address1,
										@Address2,
										@City,
										@Phone,
										@Email,
										@Company,
										@Website,
										@VatPanNo,
										@CreditLimit,
										0,
										1,
										@Created_By,
										GetDate(),
										NULL,
										NULL,
										@CalculateChecked,
										@Rate,0
										
											)
	SET @ReturnID=Scope_Identity();


	if(@@Error<>0)
	begin
		Set @Return='FAILURE';
	end
	else
	begin
		SET @Return='SUCCESS';
	end


