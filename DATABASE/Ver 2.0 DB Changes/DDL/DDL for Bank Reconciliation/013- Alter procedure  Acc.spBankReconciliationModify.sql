--USE [BENT003]
--GO
/****** Object:  StoredProcedure [Acc].[spBankReconciliationModify]    Script Date: 6/1/2017 11:55:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [Acc].[spBankReconciliationModify](
--@SeriesName NVARCHAR(50),
@BankReconciliationID int,
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
@ProjectID INT,
@Return NVARCHAR(20) OUTPUT
)
AS

SET NOCOUNT ON
	--DECLARE @SeriesID AS INT
--SERIES ID GETTING CODE GOES HERE
	--SET @SeriesID=0


--DO THE REAL MODIFICATION in the master table
UPDATE Acc.tblBankReconciliationMaster
	set SeriesID = @SeriesID,
		LedgerID = @LedgerID,
		BankReconciliation_Date = @BankReconciliation_Date,
		Voucher_No = @VoucherNo,
		Field1 = @First,
		Field2 = @Second,
		Field3 = @Third,
		Field4 = @Fourth,
		Field5 = @Fifth,
		Remarks = @Remarks,
		ProjectID = @ProjectID
		where BankReconciliationID = @BankReconciliationID;
		
		SET @Return='SUCCESS';

	if @@Error<>0
	begin
		raiserror('An error occured during modification of bank reconciliation. Please check the fields and try again!',15,1);
		SET @Return='FAILURE';
		RETURN -100
	end