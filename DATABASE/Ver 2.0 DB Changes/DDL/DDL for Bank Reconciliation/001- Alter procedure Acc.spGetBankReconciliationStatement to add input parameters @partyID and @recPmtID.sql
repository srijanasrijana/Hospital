--USE [BENT003]
--GO
/****** Object:  StoredProcedure [Acc].[spGetBankReconciliationStatement]    Script Date: 2/10/2017 10:34:24 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROC [Acc].[spGetBankReconciliationStatement]

--Get the transaction for the particular ledger

--product name | unit | net sales qty  | Net Amt
--Acc Class filter, Date filter, depot, single product, sales ledger, project
@LedgerID INT=NULL,
@FromDate nvarchar(100)=NULL,
@ToDate nvarchar(100) = NULL, 
@PartyID int = 0,
@RecPmtID int = 0,
@DefaultDate nvarchar(50) = 'Nepali',
@AccountClassIDsSettings  xml 
 --@CrSum decimal = 0 output,
 --@DrSum decimal = 0 output    -- It encludes AccountCLassIDs  Info
  AS
DECLARE @docHandle int;
EXEC sp_xml_preparedocument @docHandle OUTPUT, @AccountClassIDsSettings;

BEGIN

DECLARE @VchNRow Table
(
VoucherType NVARCHAR(100),
RowID INT
);

--To Store the final result
CREATE TABLE  #LedgerTransact 
(
Date DATETIME,
LedgerID INT,
Debit Decimal(20,5),
Credit Decimal(20,5),
VoucherType NVARCHAR(100),
VoucherNumber NVARCHAR(100),
RowID INT,
ChequeNumber Nvarchar(20),
ChequeDate NVARCHAR(20)
);
 
 --Read All Accounting Class
DECLARE @AccClassID TABLE
(
	AccClassID INT 
);

-- Write to temporary table @JAccClassID from xml
INSERT INTO @AccClassID(AccClassID)
    SELECT
        IdNode.value('(.)[1]', 'int')
    FROM
        @AccountClassIDsSettings.nodes('/LEDGERTRANSACT/ACCCLASSIDS/AccID') AS IdTbl(IdNode)


DECLARE @SqlSmt NVARCHAR(300);
--SET @SqlSmt = 'SELECT DISTINCT VoucherType,RowID FROM Acc.tblTransaction WHERE ledgerID = '+CONVERT(NVARCHAR,@LedgerID)+'AND TransactionID IN(SELECT TransactionID FROM Acc.tblTransactionClass WHERE AccClassID IN ('+@AccClassIDs+ '))';
--Get ledger transactions which falls on the accounting class
--INSERT INTO @VchNRow execute sp_executesql @SqlSmt;
 INSERT INTO @VchNRow SELECT DISTINCT VoucherType,RowID FROM Acc.tblTransaction WHERE ledgerID =@LedgerID AND TransactionID IN(SELECT TransactionID FROM Acc.tblTransactionClass WHERE AccClassID IN (SELECT AccClassID FROM @AccClassID))


DECLARE @RowID INT;
DECLARE @AnotherLedgerID INT;


----FOR PURCHASE-----

DECLARE db_cursor CURSOR FOR SELECT RowID FROM @VchNRow WHERE VoucherType='PURCH'

OPEN db_cursor FETCH NEXT FROM db_cursor INTO @RowID  

WHILE @@FETCH_STATUS = 0   
BEGIN

	--Get the ID of Another Ledger ID from the bunch of transaction.
	SELECT Top 1 @AnotherLedgerID=LedgerID from acc.tblTransaction WHERE VoucherType='PURCH' And RowID=@RowID AND LedgerID <> @LedgerID;
    --Note the swapping of Debit and Credit Amounts
    INSERT INTO #LedgerTransact SELECT a.TransactDate,@AnotherLedgerID,a.Credit_Amount, a.Debit_Amount,a.VoucherType,b.Voucher_No,a.RowID,' ' as ChequeNumber,' ' as ChequeDate from acc.tblTransaction a,Inv.tblPurchaseInvoiceMaster b WHERE a.VoucherType='PURCH' And a.RowID=@RowID AND a.LedgerID =@LedgerID AND a.RowID = b.PurchaseInvoiceID;

	FETCH NEXT FROM db_cursor INTO @RowID   
END   

CLOSE db_cursor   
DEALLOCATE db_cursor


----FOR PURCHASE RETURN-----

DECLARE db_cursor CURSOR FOR SELECT RowID FROM @VchNRow WHERE VoucherType='PURCH_RTN'



OPEN db_cursor FETCH NEXT FROM db_cursor INTO @RowID  


WHILE @@FETCH_STATUS = 0   
BEGIN

	--Get the ID of Another Ledger ID from the bunch of transaction.
	SELECT Top 1 @AnotherLedgerID=LedgerID from acc.tblTransaction WHERE VoucherType='PURCH_RTN' And RowID=@RowID AND LedgerID <> @LedgerID;
    --Note the swapping of Debit and Credit Amounts
    INSERT INTO #LedgerTransact SELECT a.TransactDate,@AnotherLedgerID,a.Credit_Amount, a.Debit_Amount,a.VoucherType,b.Voucher_No,a.RowID,' ' as ChequeNumber,' ' as ChequeDate from acc.tblTransaction a,Inv.tblPurchaseReturnMaster b WHERE a.VoucherType='PURCH_RTN' And a.RowID=@RowID AND a.LedgerID =@LedgerID AND a.RowID = b.PurchaseReturnID;

	FETCH NEXT FROM db_cursor INTO @RowID   
END   

CLOSE db_cursor   
DEALLOCATE db_cursor

--------------FOR SALES INVOICE--------------

DECLARE db_cursor CURSOR FOR SELECT RowID FROM @VchNRow WHERE VoucherType='SALES'

OPEN db_cursor FETCH NEXT FROM db_cursor INTO @RowID  


WHILE @@FETCH_STATUS = 0   
BEGIN

	--Get the ID of Another Ledger ID from the bunch of transaction.
	SELECT Top 1 @AnotherLedgerID=LedgerID from acc.tblTransaction WHERE VoucherType='SALES' And RowID=@RowID AND LedgerID <> @LedgerID;
    --Note the swapping of Debit and Credit Amounts
    INSERT INTO #LedgerTransact SELECT a.TransactDate,@AnotherLedgerID,a.Credit_Amount, a.Debit_Amount,a.VoucherType,b.Voucher_No,a.RowID,' ' as ChequeNumber,' ' as ChequeDate from acc.tblTransaction a,Inv.tblSalesInvoiceMaster b WHERE a.VoucherType='SALES' And a.RowID=@RowID AND a.LedgerID =@LedgerID AND a.RowID = b.SalesInvoiceID;

	FETCH NEXT FROM db_cursor INTO @RowID   
END   

CLOSE db_cursor   
DEALLOCATE db_cursor

-----------------------FOR SALES RETURN
DECLARE db_cursor CURSOR FOR SELECT RowID FROM @VchNRow WHERE VoucherType='SLS_RTN'

OPEN db_cursor FETCH NEXT FROM db_cursor INTO @RowID  

WHILE @@FETCH_STATUS = 0   
BEGIN

	--Get the ID of Another Ledger ID from the bunch of transaction.
	SELECT Top 1 @AnotherLedgerID=LedgerID from acc.tblTransaction WHERE VoucherType='SLS_RTN' And RowID=@RowID AND LedgerID <> @LedgerID;
    --Note the swapping of Debit and Credit Amounts
    INSERT INTO #LedgerTransact SELECT a.TransactDate,@AnotherLedgerID,a.Credit_Amount, a.Debit_Amount,a.VoucherType,b.Voucher_No,a.RowID,' ' as ChequeNumber,' ' as ChequeDate from acc.tblTransaction a,Inv.tblSalesReturnMaster b WHERE a.VoucherType='SLS_RTN' And a.RowID=@RowID AND a.LedgerID =@LedgerID AND a.RowID = b.SalesReturnID;

	FETCH NEXT FROM db_cursor INTO @RowID   
END   

CLOSE db_cursor   
DEALLOCATE db_cursor

----FOR Journal----

DECLARE db_cursor CURSOR FOR SELECT RowID FROM @VchNRow WHERE VoucherType='JRNL'

OPEN db_cursor FETCH NEXT FROM db_cursor INTO @RowID  

WHILE @@FETCH_STATUS = 0   
BEGIN

	--Get the ID of Another Ledger ID from the bunch of transaction.
	SELECT Top 1 @AnotherLedgerID=LedgerID from acc.tblTransaction WHERE VoucherType='JRNL' And RowID=@RowID AND LedgerID <> @LedgerID;
    --Note the swapping of Debit and Credit Amounts
    INSERT INTO #LedgerTransact SELECT a.TransactDate,@AnotherLedgerID,a.Credit_Amount, a.Debit_Amount,a.VoucherType,b.Voucher_No,a.RowID,' ' as ChequeNumber,' ' as ChequeDate from acc.tblTransaction a,Acc.tblJournalMaster b WHERE a.VoucherType='JRNL' And a.RowID=@RowID AND a.LedgerID =@LedgerID AND a.RowID = b.JournalID;

	FETCH NEXT FROM db_cursor INTO @RowID   
END   

CLOSE db_cursor   
DEALLOCATE db_cursor

----------------FOR CONTRA---------------

DECLARE db_cursor CURSOR FOR SELECT RowID FROM @VchNRow WHERE VoucherType='CNTR'

OPEN db_cursor FETCH NEXT FROM db_cursor INTO @RowID  

WHILE @@FETCH_STATUS = 0   
BEGIN

	--Get the ID of Another Ledger ID from the bunch of transaction.
	SELECT Top 1 @AnotherLedgerID=LedgerID from acc.tblTransaction WHERE VoucherType='CNTR' And RowID=@RowID AND LedgerID <> @LedgerID;
    --Note the swapping of Debit and Credit Amounts
    INSERT INTO #LedgerTransact SELECT a.TransactDate,@AnotherLedgerID,a.Credit_Amount, a.Debit_Amount,a.VoucherType,b.Voucher_No,a.RowID,' ' as ChequeNumber,' ' as ChequeDate from acc.tblTransaction a,Acc.tblContraMaster b WHERE a.VoucherType='CNTR' And a.RowID=@RowID AND a.LedgerID =@LedgerID AND a.RowID = b.ContraID;

	FETCH NEXT FROM db_cursor INTO @RowID   
END   

CLOSE db_cursor   
DEALLOCATE db_cursor

----FOR Cash Receipt----

DECLARE db_cursor CURSOR FOR SELECT RowID FROM @VchNRow WHERE VoucherType='CASH_RCPT'

OPEN db_cursor FETCH NEXT FROM db_cursor INTO @RowID  	
	
WHILE @@FETCH_STATUS = 0   
BEGIN

	--Get the ID of Another Ledger ID from the bunch of transaction.
	SELECT Top 1 @AnotherLedgerID=LedgerID from acc.tblTransaction WHERE VoucherType='CASH_RCPT' And RowID=@RowID AND LedgerID <> @LedgerID;
    --Note the swapping of Debit and Credit Amounts
    INSERT INTO #LedgerTransact SELECT a.TransactDate,@AnotherLedgerID,a.Credit_Amount, a.Debit_Amount,a.VoucherType,b.Voucher_No,a.RowID,' ' as ChequeNumber,' ' as ChequeDate from acc.tblTransaction a,Acc.tblCashReceiptMaster b WHERE a.VoucherType='CASH_RCPT' And a.RowID=@RowID AND a.LedgerID =@LedgerID AND a.RowID = b.CashReceiptID;

	FETCH NEXT FROM db_cursor INTO @RowID   
END   

CLOSE db_cursor   
DEALLOCATE db_cursor

--------------FOR CASH PAYMENT-------

DECLARE db_cursor CURSOR FOR SELECT RowID FROM @VchNRow WHERE VoucherType='CASH_PMNT'


OPEN db_cursor FETCH NEXT FROM db_cursor INTO @RowID  

	
	
	
WHILE @@FETCH_STATUS = 0   
BEGIN

	--Get the ID of Another Ledger ID from the bunch of transaction.
	SELECT Top 1 @AnotherLedgerID=LedgerID from acc.tblTransaction WHERE VoucherType='CASH_PMNT' And RowID=@RowID AND LedgerID <> @LedgerID;
    --Note the swapping of Debit and Credit Amounts
        INSERT INTO #LedgerTransact SELECT a.TransactDate,@AnotherLedgerID,a.Credit_Amount, a.Debit_Amount,a.VoucherType,b.Voucher_No,a.RowID,' ' as ChequeNumber,' ' as ChequeDate from acc.tblTransaction a,Acc.tblCashPaymentMaster b WHERE a.VoucherType='CASH_PMNT' And a.RowID=@RowID AND a.LedgerID =@LedgerID AND a.RowID = b.CashPaymentID;

	FETCH NEXT FROM db_cursor INTO @RowID   
END   

CLOSE db_cursor   
DEALLOCATE db_cursor

-----------------------FOR BANK PAYMENT------------------------

DECLARE db_cursor CURSOR FOR SELECT RowID FROM @VchNRow WHERE VoucherType='BANK_PMNT'

OPEN db_cursor FETCH NEXT FROM db_cursor INTO @RowID  

	
WHILE @@FETCH_STATUS = 0   
BEGIN

	--Get the ID of Another Ledger ID from the bunch of transaction.
	SELECT Top 1 @AnotherLedgerID=LedgerID from acc.tblTransaction WHERE VoucherType='BANK_PMNT' And RowID=@RowID AND LedgerID <> @LedgerID;
    --Note the swapping of Debit and Credit Amounts
    INSERT INTO #LedgerTransact SELECT a.TransactDate,@AnotherLedgerID,a.Credit_Amount, a.Debit_Amount,a.VoucherType,b.Voucher_No,a.RowID,c.ChequeNumber,c.ChequeDate from acc.tblTransaction a,Acc.tblBankPaymentMaster b,Acc.tblBankPaymentDetails c WHERE a.VoucherType='BANK_PMNT' And a.RowID=@RowID AND a.LedgerID =@LedgerID AND a.RowID = b.BankPaymentID and b.BankPaymentID=c.BankPaymentID;

	FETCH NEXT FROM db_cursor INTO @RowID   
END   

CLOSE db_cursor   
DEALLOCATE db_cursor

-----------------------------FOR BANK RECONCILIATION-------------------------------------------
DECLARE db_cursor CURSOR FOR SELECT RowID FROM @VchNRow WHERE VoucherType='BRECON'

OPEN db_cursor FETCH NEXT FROM db_cursor INTO @RowID  
WHILE @@FETCH_STATUS = 0   
BEGIN
	--Get the id of another ledger id from the bunch of transaction.
	SELECT Top 1 @AnotherLedgerID=LedgerID from acc.tblTransaction WHERE VoucherType='BRECON' And RowID=@RowID AND LedgerID <> @LedgerID;
	--Note the swapping of debit and credit amounts
	insert into #LedgerTransact select a.TransactDate,@AnotherLedgerID,a.Credit_Amount, a.Debit_Amount,a.VoucherType,b.Voucher_No,a.RowID,' ' as ChequeNumber,' ' as ChequeDate from acc.tblTransaction a,Acc.tblBankReconciliationMaster b,Acc.tblBankReconciliationDetails c WHERE a.VoucherType='BRECON' And a.RowID=@RowID AND a.LedgerID =@LedgerID AND a.RowID = b.BankReconciliationID and b.BankReconciliationID=c.BankReconciliationID;
	
	fetch next from db_cursor into @RowID
end

close db_cursor
deallocate db_cursor

-------------------------------FOR BANK RECEIPT -----------------------------------------
DECLARE db_cursor CURSOR FOR SELECT RowID FROM @VchNRow WHERE VoucherType='BANK_RCPT'

OPEN db_cursor FETCH NEXT FROM db_cursor INTO @RowID  
	
WHILE @@FETCH_STATUS = 0   
BEGIN

	--Get the ID of Another Ledger ID from the bunch of transaction.
	SELECT Top 1 @AnotherLedgerID=LedgerID from acc.tblTransaction WHERE VoucherType='BANK_RCPT' And RowID=@RowID AND LedgerID <> @LedgerID;
    --Note the swapping of Debit and Credit Amounts
     INSERT INTO #LedgerTransact SELECT a.TransactDate,@AnotherLedgerID,a.Credit_Amount, a.Debit_Amount,a.VoucherType,b.Voucher_No,a.RowID,c.ChequeNumber,c.ChequeDate from acc.tblTransaction a,Acc.tblBankReceiptMaster b,Acc.tblBankReceiptDetails c WHERE a.VoucherType='BANK_RCPT' And a.RowID=@RowID AND a.LedgerID =@LedgerID AND a.RowID = b.BankReceiptID and b.BankReceiptID=c.BankReceiptID;

	FETCH NEXT FROM db_cursor INTO @RowID   
END   

CLOSE db_cursor   
DEALLOCATE db_cursor





-------------------------------------------------------
--select * from #LedgerTransact;

declare @SqlSt nvarchar(max) = '';
declare @SqlSt1 nvarchar(max) = '';

declare @partySql nvarchar(max) = '';
declare @recpmtSql nvarchar(max) = '';
declare @dateSql nvarchar(max) = '';

if(@RecPmtID = 1)
	set @recpmtSql += ' and a.VoucherType =''BANK_PMNT''';

else if(@RecPmtID = 2)
	set @recpmtSql += ' and a.VoucherType =''BANK_RCPT''';

if(@PartyID > 0)
	set @partySql += ' and a.LedgerID ='+convert(nvarchar(20),@PartyID);

if(@DefaultDate = 'Nepali')
	set @dateSql = 'Date.fnEngToNep(a.Date) as LedgerDate';
else
	set @dateSql = ' convert(varchar, a.Date, 111) as LedgerDate';

	set @SqlSt = 'Select  '+@dateSql+' ,b.EngName Account, a.LedgerID,a.Debit,a.Credit,a.VoucherType,a.VoucherNumber,a.RowID,isnull(a.ChequeNumber, '' '') ChequeNumber,isnull (Date.fnEngToNep(a.ChequeDate),'' '') ChequeDate from #LedgerTransact a,Acc.tblLedger b WHERE a.LedgerID=b.LedgerID and convert(date,a.Date) >= '''+@FromDate+''' and convert(date,a.Date) <= '''+@ToDate+'''' + @recpmtSql + @partySql;


exec sp_executesql @SqlSt;

--else --if(@DefaultDate = 'Nepali')
--	Select a.Date as LedgerDate,b.EngName Account, a.LedgerID,a.Debit,a.Credit,a.VoucherType,a.VoucherNumber,a.RowID,a.ChequeNumber,isnull (a.ChequeDate,0) ChequeDate from #LedgerTransact a,Acc.tblLedger b WHERE a.LedgerID=b.LedgerID and convert(date,a.Date) >= @FromDate and convert(date,a.Date) <= @ToDate

--set @SqlSt = 'Select @DrSum = sum(a.Debit), a.VoucherType from #LedgerTransact a,Acc.tblLedger b WHERE a.LedgerID=b.LedgerID and convert(date,a.Date) >= '''+@FromDate+''' and convert(date,a.Date) <= '''+@ToDate+''''  + @recpmtSql + @partySql;

--exec sp_executesql 
-- @query = @SqlSt,
--		 @DrSum = N'@DrSum decimal OUTPUT',
--		 @DrSum = @DrSum output;

--set @SqlSt1 = 'Select @CrSum = sum(a.Credit), a.VoucherType from #LedgerTransact a,Acc.tblLedger b WHERE a.LedgerID=b.LedgerID and convert(date,a.Date) >= '''+@FromDate+''' and convert(date,a.Date) <= '''+@ToDate+''''  + @recpmtSql + @partySql;

--exec sp_executesql 
-- @query = @SqlSt1,
--		 @CrSum = N'@CrSum decimal OUTPUT',
--		 @CrSum = @CrSum output;

END
EXEC sp_xml_removedocument @docHandle 
drop table #LedgerTransact