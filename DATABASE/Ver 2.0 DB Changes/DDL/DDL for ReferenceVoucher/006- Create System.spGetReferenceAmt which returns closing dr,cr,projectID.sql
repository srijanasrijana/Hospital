--USE [BENT003]
--GO
/****** Object:  StoredProcedure [System].[spGetReferenceAmt]    Script Date: 1/31/2017 3:16:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
create PROCEDURE [System].[spGetReferenceAmt]
	(
		@RefID nvarchar = null,
		@AccClassIDs nvarchar(500) output
		--@LedgerID int = null,
		--@AccountClassIDsSettings  xml,     -- It encludes AccountCLassIDs  Info
		--@ProjectIDsSettings xml           -- It includes ProjectIDs Info
	)
AS
BEGIN
	Declare @SqlStmt nvarchar(max) = null;
	Declare @tblLedgerVoucher table(
		ledgerID int not null,
		vouID int not null,
		vouType varchar(20),
		isAgainst bit
		);

	--Set @SqlStmt = '
	INSERT INTO @tblLedgerVoucher
		Select R.LedgerID,RV.VoucherID,RV.VoucherType,RV.IsAgainst from System.tblReference R inner join System.tblReferenceVoucher RV on R.RefID = RV.RefID where R.RefID = @RefID;

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
	 Insert	into @tblFinal
		Select LV.isAgainst,T.Debit_Amount,T.Credit_Amount,T.ProjectID from @tblLedgerVoucher LV inner join Acc.tblTransaction T on LV.vouID = T.RowID and LV.vouType = T.VoucherType and T.LedgerID = LV.ledgerID;

		--select * from @tblFinal;
	-- execute sp_executesql @SqlStmt;

	--This code is used if reference against can not be done on same drcr side.
	--Declare @DrNotAgainst money,
	--		@CrNotAgainst money;

	--Select @DrNotAgainst = DrAmt,@CrNotAgainst = CrAmt from @tblFinal where isAgainst = 0;

	--Select (@DrNotAgainst - SUM(DrAmt)) as DrAmt,(@CrNotAgainst - SUM(CrAmt)) as CrAmt,(Select projectID from @tblFinal where isAgainst = 0) as projectID from @tblFinal where isAgainst = 1;

	Select (select DrAmt from @tblFinal where isAgainst = 0)  as RefDrAmt, (select CrAmt from @tblFinal where isAgainst = 0)  as RefCrAmt, COALESCE(SUM(DrAmt),0) as DrAmt,COALESCE(SUM(CrAmt),0) as CrAmt,(Select projectID from @tblFinal where isAgainst = 0) as projectID from @tblFinal where isAgainst = 1;
END