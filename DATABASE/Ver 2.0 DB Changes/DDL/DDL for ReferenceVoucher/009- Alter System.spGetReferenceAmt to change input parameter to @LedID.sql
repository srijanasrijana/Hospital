--USE [BENT003]
--GO
/****** Object:  StoredProcedure [System].[spGetReferenceAmt]    Script Date: 4/11/2017 3:27:05 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [System].[spGetReferenceAmt]
	(
		--@RefID nvarchar(50) = null,
		@LedID nvarchar(50) = null,
		@VoucherID nvarchar(50) = 0,
		@VoucherType nvarchar(50) = null,
		@AccClassIDs nvarchar(500) output
		--@AccountClassIDsSettings  xml,     -- It encludes AccountCLassIDs  Info
		--@ProjectIDsSettings xml           -- It includes ProjectIDs Info
	)
AS
BEGIN
	Declare @SqlStmt nvarchar(max) = null;
	declare @DrAmt decimal(20, 5) = 0;
	declare @CrAmt decimal(20, 5) = 0;

	Declare @tblLedgerVoucher table(
		ID int not null identity(1,1),
		RefID int not null,
		RefName nvarchar(500) not null,
		ledgerID int not null,
		vouID int not null,
		vouType varchar(20),
		isAgainst bit,
		DrAmt decimal(20, 5),
		CrAmt decimal(20, 5)
		);

	Declare @ledgerID int = 0;
	Declare @vouID int = 0;
	Declare @vouType varchar(20) =0;
	declare @ID int = 0;
	--Set @SqlStmt = '
	
		INSERT INTO @tblLedgerVoucher
			Select R.RefID,R.Ref as RefName, R.LedgerID,RV.VoucherID,RV.VoucherType,RV.IsAgainst, 0 asDRAmt, 0 as CRAmt from System.tblReference R inner join System.tblReferenceVoucher RV on R.RefID = RV.RefID where R.LedgerID = @LedID;-- and RV.VoucherID not in (select VoucherID from System.tblReferenceVoucher where VoucherID = @VoucherID and VoucherType = @VoucherType);

-- execute sp_executesql @SqlStmt;
	--select * from @tblLedgerVoucher;

	--Set @AccClassIDs with Accounting Class ID of the voucher 
	Select @AccClassIDs = @AccClassIDs + convert(nvarchar, TC.AccClassID) + N',' from Acc.tblTransactionClass TC inner join @tblLedgerVoucher LV on TC.RowID = LV.vouID and TC.VoucherType = LV.vouType where LV.isAgainst = 0; 
		If(LEN(@AccClassIDs) > 0)
		BEGIN
			SET @AccClassIDs= left(@AccClassIDs,len(@AccClassIDs)-1); --Trim one last trailing comma
		END

	Declare @tblFinal table(
	isAgainst bit,
	DrAmt money,
	CrAmt money,
	projectID int
	);

	 --Set @SqlStmt = '

	 	DECLARE db_cursor CURSOR FOR SELECT ledgerID, vouID, vouType FROM @tblLedgerVoucher 

		OPEN db_cursor FETCH NEXT FROM db_cursor INTO @ledgerID, @vouID, @vouType 
		WHILE @@FETCH_STATUS = 0   
		BEGIN
			set @ID = @ID+1;
			if(@vouID = @VoucherID and @vouType = @VoucherType)
				goto Exiting;
			else
				select @DrAmt = Debit_Amount, @CrAmt = Credit_Amount from Acc.tblTransaction where LedgerID = @ledgerID and VoucherType = @vouType and RowID = @vouID;

			Updating:
				update @tblLedgerVoucher set DrAmt = @DrAmt, CrAmt = @CrAmt where ID =@ID;

			Exiting:
				FETCH NEXT FROM db_cursor INTO @ledgerID, @vouID, @vouType 
		END
		CLOSE db_cursor   
		DEALLOCATE db_cursor

	 --Insert	into @tblFinal
		--Select LV.isAgainst,T.Debit_Amount,T.Credit_Amount,T.ProjectID from @tblLedgerVoucher LV inner join Acc.tblTransaction T on LV.vouID = T.RowID and T.LedgerID = LV.ledgerID; --and LV.vouType = T.VoucherType 

		--select * from @tblFinal;
	-- execute sp_executesql @SqlStmt;

	--This code is used if reference against can not be done on same drcr side.
	--Declare @DrNotAgainst money,
	--		@CrNotAgainst money;

	--Select @DrNotAgainst = DrAmt,@CrNotAgainst = CrAmt from @tblFinal where isAgainst = 0;

	--Select (@DrNotAgainst - SUM(DrAmt)) as DrAmt,(@CrNotAgainst - SUM(CrAmt)) as CrAmt,(Select projectID from @tblFinal where isAgainst = 0) as projectID from @tblFinal where isAgainst = 1;

	--Select (select DrAmt from @tblFinal where isAgainst = 0)  as RefDrAmt, (select CrAmt from @tblFinal where isAgainst = 0)  as RefCrAmt, COALESCE(SUM(DrAmt),0) as DrAmt,COALESCE(SUM(CrAmt),0) as CrAmt,(Select projectID from @tblFinal where isAgainst = 0) as projectID from @tblFinal where isAgainst = 1;
	--select * from @tblLedgerVoucher;
	select RefID, RefName, sum(DrAmt) as DrAmt , sum(CrAmt) CrAmt from @tblLedgerVoucher group by(RefID), RefName
END