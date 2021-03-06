
/****** Object:  StoredProcedure [Acc].[spBankReconciliationDetailStatus]    Script Date: 19-Jul-16 5:26:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


ALTER PROCEDURE [Acc].[spBankReconciliationDetailStatus](
@sn int,
@date Datetime,
@partyname nvarchar(20),
@debitamount float,
@creditamount float,
@chequenumber float,
@chequebank nvarchar(20),
@chequedate datetime=null,
@matched char(1),
@vouchertype nvarchar(20),
@rowid int,
@userid int,
@ledgerid int,
@fromdate datetime,
@todate datetime,
@Return NVARCHAR(20) OUTPUT
)
AS

SET NOCOUNT ON
	--DECLARE @SeriesID AS INT
--SERIES ID GETTING CODE GOES HERE
	--SET @SeriesID=0


--DO THE REAL INSERT
	INSERT INTO Acc.tblBankReconciliationSaveState(sn,date,Debit_amount,credit_amount,chequenumber,chequebank,matched,vouchertype,rowid,chequedate,userid,ledgerid,partyname,fromdate,todate) VALUES
	(@sn,@date,@debitamount,@creditamount,@chequenumber,@chequebank,@matched,@vouchertype,@rowid,
	@chequedate,@userid,@ledgerid,@partyname,@fromdate,@todate
										
										)

	SET @Return=scope_identity();

	if @@Error<>0
	begin
		raiserror('An error occured during addition of accounnting group. Please check the fields and try again!',15,1);
		SET @Return='FAILURE';
		RETURN -100
	end