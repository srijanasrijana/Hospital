--USE [BENT003]
--GO
/****** Object:  StoredProcedure [Acc].[spRetrieveBankReconciliation]    Script Date: 2/22/2017 3:33:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [Acc].[spRetrieveBankReconciliation]
@BankID int = 0,
@AccountClassIDsSettings  xml,
@DefaultDate nvarchar(50) = 'Nepali' 
as 
begin 

declare @LedgerID INT=NULL;
declare @FromDate nvarchar(100)=NULL;
declare @ToDate nvarchar(100) = NULL;
declare @PartyID int = 0;
declare @RecPmtID int = 0;
--declare @DefaultDate nvarchar(50) = 'Nepali',
--declare @AccountClassIDsSettings  xml 

select @LedgerID = BankID,
	   @FromDate = FromDate,
	   @ToDate = ToDate,
	   @PartyID = PartyID,
	   @RecPmtID = PmtRcpTypeID
 from Acc.tblBankReconSaveStateMaster where BankID = @BankID;

declare  @tblBankSavedStateDetails table
(
	 SaveStateID int,
	 VoucherType nvarchar(50),
	 RowID int,
	 Matched bit--,
	 --FromDate Date,
	 --ToDate Date
);

declare @tblBankStatement table
(
	 LedgerDate Date,
	 Account nvarchar(500),
	 LedgerID int,
	 Debit decimal(20,5),
	 Credit decimal(20,5),
	 VoucherType nvarchar(500),
	 VoucherNumber nvarchar(500),
	 RowID int,
	 ChequeNumber nvarchar(500), 
	 ChequeDate  nvarchar(500) 
);

insert into @tblBankSavedStateDetails 
	select SaveStateID, VoucherType, RowID, Matched from Acc.tblBankReconSaveStateDetails

insert into @tblBankStatement
		exec [Acc].[spGetBankReconciliationStatement] 
		@LedgerID,
		@FromDate,
		@ToDate, 
		@PartyID,
		@RecPmtID,
		@DefaultDate,
		@AccountClassIDsSettings



select convert(varchar,a.LedgerDate, 111) LedgerDate,
	 a.Account ,
	 a.LedgerID ,
	 a.Debit ,
	 a.Credit ,
	 a.VoucherType,
	 a.VoucherNumber,
	 a.RowID ,
	 ChequeNumber, 	 
	 ChequeDate,
	 isnull(b.Matched, 0) Matched
				  from @tblBankStatement a left outer join @tblBankSavedStateDetails b on a.VoucherType = b.VoucherType and a.RowID = b.RowID;
 
end