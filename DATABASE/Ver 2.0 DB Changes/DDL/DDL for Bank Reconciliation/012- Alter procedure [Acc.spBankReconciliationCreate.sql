--USE [BENT003]
--GO
/****** Object:  StoredProcedure [Acc].[spBankReconciliationCreate]    Script Date: 6/1/2017 11:51:38 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [Acc].[spBankReconciliationCreate](
--@SeriesName NVARCHAR(50),
@SeriesID int,
@LedgerID int,
@BankReconciliation_Date Datetime,
@VoucherNo NVARCHAR(20),
@First nvarchar(50),
@Second nvarchar(50),
@Third nvarchar(50),
@Fourth nvarchar(50),
@Fifth nvarchar(50),
@Remarks nvarchar(500),
@ProjectID int,
@Return NVARCHAR(20) OUTPUT
)
AS

SET NOCOUNT ON
	--DECLARE @SeriesID AS INT
--SERIES ID GETTING CODE GOES HERE
	--SET @SeriesID=0


--DO THE REAL INSERT
	INSERT INTO Acc.tblBankReconciliationMaster VALUES(
										@SeriesID, @LedgerID, @BankReconciliation_Date, @VoucherNo, @First, @Second, @Third, @Fourth, @Fifth, @Remarks, @ProjectID
										)

	SET @Return=scope_identity();

	if @@Error<>0
	begin
		raiserror('An error occured during addition of bank reconciliation. Please check the fields and try again!',15,1);
		SET @Return='FAILURE';
		RETURN -100
	end