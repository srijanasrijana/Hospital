--USE [BENT003]
--GO
/****** Object:  StoredProcedure [Acc].[spLedgerModify]    Script Date: 1/26/2017 3:13:24 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [Acc].[spLedgerModify]
(
@LedgerID INT,
@LedgerCode NVARCHAR(50),
@Lang NVARCHAR(50),
@Name NVARCHAR(300),
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
@VatPanNo NVARCHAR(50) = NULL,
@CreditLimit MONEY=0,
@CalculateChecked Bit=0,
@Rate float=0.00, 
@Modified_By NVARCHAR(20),
@IsBillReference bit = null,
@ReturnID INT OUTPUT,
@Return NVARCHAR(20) OUTPUT
)
AS
SET NOCOUNT ON

	DECLARE @DrCr NVARCHAR(3), @BuiltIn BIT

	--CHECK IF IT IS BUILT IN 
	--SELECT @BuiltIn=BuiltIn FROM Acc.tblLedger WHERE LedgerID=@LedgerID;
	--if(@BuiltIn=1)
	--begin
	--	raiserror('Built in ledger cannot be modified',15,1);
	--	return -100
	--end

	--VALIDATE INPUTS
	IF @GroupID IS NULL
	BEGIN
		RAISERROR('Invalid Group Name',15,1)
		RETURN -100
	END
	--IF @OpDrCr<>'DEBIT' and @OpDrCr<>'CREDIT' and @OpDrCr is not null
	--BEGIN	
	--	RAISERROR('Invalid Opening Balance type. You can only input either Debit or Credit',15,1);
	--	RETURN -100
	--END

	--GET DR/CR type from its group behaviour
	SET @DrCr=Acc.fnGetDrCrTypeFromGroup(@GroupID)

	DECLARE @NameField AS NVARCHAR(20)
	if(@Lang='ENGLISH')
		UPDATE Acc.tblLedger SET LedgerCode=@LedgerCode,
										EngName=@Name,
                                        PreYrBal =@PreYrBal,
                                        PreYrBalDrCr = @PreYrBalDrCr,
										OpCCYID=@OpCCYID,
										OpCCR=@OpCCR,
										OpCCRDate=@OpCCRDate,
										DrCr=@DrCr,
										GroupID=@GroupID,
										Remarks=@Remarks,
										PersonName=@PerName,
										Address1=@Address1,
										Address2=@Address2,
										City=@City,
										Phone=@Phone,
										Email=@Email,
										Company=@Company,
										Website=@Website,
										VatPanNo = @VatPanNo,
										CreditLimit = @CreditLimit,
										Modified_By=@Modified_By,
										Modified_Date=GetDate(),
										Calculated=@CalculateChecked,
										Calculate_Rate=@Rate,
										IsBillReference = @IsBillReference
		WHERE LedgerID=@LedgerID;

	ELSE IF(@Lang='NEPALI')
		UPDATE Acc.tblLedger SET LedgerCode=@LedgerCode,
										NepName=@Name,
										OpCCYID=@OpCCYID,
										OpCCR=@OpCCR,
										OpCCRDate=@OpCCRDate,
										DrCr=@DrCr,
										GroupID=@GroupID,
										Remarks=@Remarks,
										PersonName=@PerName,
										Address1=@Address1,
										Address2=@Address2,
										City=@City,
										Phone=@Phone,
										Email=@Email,
										Company=@Company,
										Website=@Website,
										VatPanNo = @VatPanNo,
										CreditLimit = @CreditLimit,
										Modified_By=@Modified_By,
										Modified_Date=GetDate(),
										Calculated=@CalculateChecked,
										Calculate_Rate=@Rate,
										IsBillReference = @IsBillReference
		WHERE LedgerID=@LedgerID;
											
	SET @ReturnID=Scope_Identity();


	if(@@Error<>0)
	begin
		Set @Return='FAILURE';
	end
	else
	begin
		SET @Return='SUCCESS';
	end

