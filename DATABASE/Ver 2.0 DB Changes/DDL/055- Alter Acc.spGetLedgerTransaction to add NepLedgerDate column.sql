
/****** Object:  StoredProcedure [Acc].[spGetLedgerTransaction]    Script Date: 12/22/2016 3:57:28 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

--Acc.spGetLedgerTransact
ALTER PROC [Acc].[spGetLedgerTransaction]

--Get the transaction for the particular ledger
--SELECT DATEDIFF(DAY,  DATEADD(day, -1, @CreatedDate), GETDATE())
--product name | unit | net sales qty  | Net Amt
--Acc Class filter, Date filter, depot, single product, sales ledger, project
@LedgerID INT=NULL,
@FromDate DATETIME=NULL,

@ToDate DATETIME = NULL, 
@AccountClassIDsSettings  xml,    -- It encludes AccountCLassIDs  Info
@ProjectIDsSettings xml
  AS

DECLARE @docHandle int;
DECLARE @docHandle1 int;
EXEC sp_xml_preparedocument @docHandle OUTPUT, @AccountClassIDsSettings;
EXEC sp_xml_preparedocument @docHandle1 OUTPUT, @ProjectIDsSettings;

BEGIN
--SELECT DATEDIFF(DAY,  DATEADD(day, -1, @FromDate), GETDATE())
DECLARE @VchNRow Table
(
VoucherType NVARCHAR(100),
RowID INT
);

--To Store the final result
DECLARE @LedgerTransact Table
(
Date DATETIME,
LedgerID INT,
Debit Decimal,
Credit Decimal,
VoucherType NVARCHAR(100),
VoucherNumber NVARCHAR(100),
RowID INT
);

 --Read All Accounting Class
DECLARE @AccClassID TABLE
(
	AccClassID INT 
);

-- Write to temporary table @JAccClassID from xml
INSERT INTO @AccClassID(AccClassID)
    SELECT AccClassID FROM  Acc.fnGetAllAccClass (@AccountClassIDsSettings);



--Read All Accounting Class
DECLARE @ProjectID TABLE
(
	ProjectID INT 
);

-- Write to temporary table @@ProjectID from xml
INSERT INTO @ProjectID(ProjectID)
    SELECT ProjectID FROM  Acc.fnGetAllProjects (@ProjectIDsSettings);


DECLARE @SqlSmt NVARCHAR(300);
--SET @SqlSmt = 'SELECT DISTINCT VoucherType,RowID FROM Acc.tblTransaction WHERE ledgerID = '+CONVERT(NVARCHAR,@LedgerID)+'AND TransactionID IN(SELECT TransactionID FROM Acc.tblTransactionClass WHERE AccClassID IN ('+@AccClassIDs+ '))';
--Get ledger transactions which falls on the accounting class
--INSERT INTO @VchNRow execute sp_executesql @SqlSmt;


 INSERT INTO @VchNRow SELECT DISTINCT VoucherType,RowID FROM Acc.tblTransaction WHERE ledgerID =@LedgerID AND TransactionID IN(SELECT TransactionID FROM Acc.tblTransactionClass WHERE AccClassID IN (SELECT AccClassID FROM @AccClassID) and ProjectID in (select ProjectID from @ProjectID))


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
    
    if(@FromDate is not null)
		if(@ToDate is not null)
			INSERT INTO @LedgerTransact SELECT a.TransactDate,@AnotherLedgerID,a.Credit_Amount, a.Debit_Amount,a.VoucherType,b.Voucher_No,a.RowID from acc.tblTransaction a,Inv.tblPurchaseInvoiceMaster b WHERE a.VoucherType='PURCH' And a.RowID=@RowID AND a.LedgerID =@LedgerID AND a.RowID = b.PurchaseInvoiceID AND b.PurchaseInvoice_Date BETWEEN @FromDate and @ToDate; 
		else
			INSERT INTO @LedgerTransact SELECT a.TransactDate,@AnotherLedgerID,a.Credit_Amount, a.Debit_Amount,a.VoucherType,b.Voucher_No,a.RowID from acc.tblTransaction a,Inv.tblPurchaseInvoiceMaster b WHERE a.VoucherType='PURCH' And a.RowID=@RowID AND a.LedgerID =@LedgerID AND a.RowID = b.PurchaseInvoiceID AND b.PurchaseInvoice_Date >= @FromDate;
	else
		if(@ToDate is not null)
			INSERT INTO @LedgerTransact SELECT a.TransactDate,@AnotherLedgerID,a.Credit_Amount, a.Debit_Amount,a.VoucherType,b.Voucher_No,a.RowID from acc.tblTransaction a,Inv.tblPurchaseInvoiceMaster b WHERE a.VoucherType='PURCH' And a.RowID=@RowID AND a.LedgerID =@LedgerID AND a.RowID = b.PurchaseInvoiceID AND b.PurchaseInvoice_Date <=@ToDate;
		else
			INSERT INTO @LedgerTransact SELECT a.TransactDate,@AnotherLedgerID,a.Credit_Amount, a.Debit_Amount,a.VoucherType,b.Voucher_No,a.RowID from acc.tblTransaction a,Inv.tblPurchaseInvoiceMaster b WHERE a.VoucherType='PURCH' And a.RowID=@RowID AND a.LedgerID =@LedgerID AND a.RowID = b.PurchaseInvoiceID;

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
    
    
    if(@FromDate is not null)
		if(@ToDate is not null)
			INSERT INTO @LedgerTransact SELECT a.TransactDate,@AnotherLedgerID,a.Credit_Amount, a.Debit_Amount,a.VoucherType,b.Voucher_No,a.RowID from acc.tblTransaction a,Inv.tblPurchaseReturnMaster b WHERE a.VoucherType='PURCH_RTN' And a.RowID=@RowID AND a.LedgerID =@LedgerID AND a.RowID = b.PurchaseReturnID AND b.PurchaseReturn_Date BETWEEN @FromDate and @ToDate; 
		else
			INSERT INTO @LedgerTransact SELECT a.TransactDate,@AnotherLedgerID,a.Credit_Amount, a.Debit_Amount,a.VoucherType,b.Voucher_No,a.RowID from acc.tblTransaction a,Inv.tblPurchaseReturnMaster b WHERE a.VoucherType='PURCH_RTN' And a.RowID=@RowID AND a.LedgerID =@LedgerID AND a.RowID = b.PurchaseReturnID AND b.PurchaseReturn_Date >= @FromDate;
	else
		if(@ToDate is not null)
			INSERT INTO @LedgerTransact SELECT a.TransactDate,@AnotherLedgerID,a.Credit_Amount, a.Debit_Amount,a.VoucherType,b.Voucher_No,a.RowID from acc.tblTransaction a,Inv.tblPurchaseReturnMaster b WHERE a.VoucherType='PURCH_RTN' And a.RowID=@RowID AND a.LedgerID =@LedgerID AND a.RowID = b.PurchaseReturnID AND b.PurchaseReturn_Date <=@ToDate;
		else
			INSERT INTO @LedgerTransact SELECT a.TransactDate,@AnotherLedgerID,a.Credit_Amount, a.Debit_Amount,a.VoucherType,b.Voucher_No,a.RowID from acc.tblTransaction a,Inv.tblPurchaseReturnMaster b WHERE a.VoucherType='PURCH_RTN' And a.RowID=@RowID AND a.LedgerID =@LedgerID AND a.RowID = b.PurchaseReturnID;

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
     if(@FromDate is not null)
		if(@ToDate is not null)
			INSERT INTO @LedgerTransact SELECT a.TransactDate,@AnotherLedgerID,a.Credit_Amount, a.Debit_Amount,a.VoucherType,b.Voucher_No,a.RowID from acc.tblTransaction a,Inv.tblSalesInvoiceMaster b WHERE a.VoucherType='SALES' And a.RowID=@RowID AND a.LedgerID =@LedgerID AND a.RowID = b.SalesInvoiceID AND b.SalesInvoice_Date BETWEEN @FromDate and @ToDate; 
		else
			INSERT INTO @LedgerTransact SELECT a.TransactDate,@AnotherLedgerID,a.Credit_Amount, a.Debit_Amount,a.VoucherType,b.Voucher_No,a.RowID from acc.tblTransaction a,Inv.tblSalesInvoiceMaster b WHERE a.VoucherType='SALES' And a.RowID=@RowID AND a.LedgerID =@LedgerID AND a.RowID = b.SalesInvoiceID AND b.SalesInvoice_Date >= @FromDate;
	else
		if(@ToDate is not null)
			INSERT INTO @LedgerTransact SELECT a.TransactDate,@AnotherLedgerID,a.Credit_Amount, a.Debit_Amount,a.VoucherType,b.Voucher_No,a.RowID from acc.tblTransaction a,Inv.tblSalesInvoiceMaster b WHERE a.VoucherType='SALES' And a.RowID=@RowID AND a.LedgerID =@LedgerID AND a.RowID = b.SalesInvoiceID AND b.SalesInvoice_Date <=@ToDate;
		else
			INSERT INTO @LedgerTransact SELECT a.TransactDate,@AnotherLedgerID,a.Credit_Amount, a.Debit_Amount,a.VoucherType,b.Voucher_No,a.RowID from acc.tblTransaction a,Inv.tblSalesInvoiceMaster b WHERE a.VoucherType='SALES' And a.RowID=@RowID AND a.LedgerID =@LedgerID AND a.RowID = b.SalesInvoiceID;

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
    
     if(@FromDate is not null)
		if(@ToDate is not null)
			INSERT INTO @LedgerTransact SELECT a.TransactDate,@AnotherLedgerID,a.Credit_Amount, a.Debit_Amount,a.VoucherType,b.Voucher_No,a.RowID from acc.tblTransaction a,Inv.tblSalesReturnMaster b WHERE a.VoucherType='SLS_RTN' And a.RowID=@RowID AND a.LedgerID =@LedgerID AND a.RowID = b.SalesReturnID AND b.SalesReturn_Date BETWEEN @FromDate and @ToDate; 
		else
			INSERT INTO @LedgerTransact SELECT a.TransactDate,@AnotherLedgerID,a.Credit_Amount, a.Debit_Amount,a.VoucherType,b.Voucher_No,a.RowID from acc.tblTransaction a,Inv.tblSalesReturnMaster b WHERE a.VoucherType='SLS_RTN' And a.RowID=@RowID AND a.LedgerID =@LedgerID AND a.RowID = b.SalesReturnID AND b.SalesReturn_Date >= @FromDate;
	else
		if(@ToDate is not null)
			INSERT INTO @LedgerTransact SELECT a.TransactDate,@AnotherLedgerID,a.Credit_Amount, a.Debit_Amount,a.VoucherType,b.Voucher_No,a.RowID from acc.tblTransaction a,Inv.tblSalesReturnMaster b WHERE a.VoucherType='SLS_RTN' And a.RowID=@RowID AND a.LedgerID =@LedgerID AND a.RowID = b.SalesReturnID AND b.SalesReturn_Date <=@ToDate;
		else
			INSERT INTO @LedgerTransact SELECT a.TransactDate,@AnotherLedgerID,a.Credit_Amount, a.Debit_Amount,a.VoucherType,b.Voucher_No,a.RowID from acc.tblTransaction a,Inv.tblSalesReturnMaster b WHERE a.VoucherType='SLS_RTN' And a.RowID=@RowID AND a.LedgerID =@LedgerID AND a.RowID = b.SalesReturnID;

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
    
    if(@FromDate is not null)
		if(@ToDate is not null)
			INSERT INTO @LedgerTransact SELECT a.TransactDate,@AnotherLedgerID,a.Credit_Amount, a.Debit_Amount,a.VoucherType,b.Voucher_No,a.RowID from acc.tblTransaction a,Acc.tblJournalMaster b WHERE a.VoucherType='JRNL' And a.RowID=@RowID AND a.LedgerID =@LedgerID AND a.RowID = b.JournalID AND b.Journal_Date BETWEEN @FromDate and @ToDate; 
		else
			INSERT INTO @LedgerTransact SELECT a.TransactDate,@AnotherLedgerID,a.Credit_Amount, a.Debit_Amount,a.VoucherType,b.Voucher_No,a.RowID from acc.tblTransaction a,Acc.tblJournalMaster b WHERE a.VoucherType='JRNL' And a.RowID=@RowID AND a.LedgerID =@LedgerID AND a.RowID = b.JournalID AND b.Journal_Date >= @FromDate;
	else
		if(@ToDate is not null)
			INSERT INTO @LedgerTransact SELECT a.TransactDate,@AnotherLedgerID,a.Credit_Amount, a.Debit_Amount,a.VoucherType,b.Voucher_No,a.RowID from acc.tblTransaction a,Acc.tblJournalMaster b WHERE a.VoucherType='JRNL' And a.RowID=@RowID AND a.LedgerID =@LedgerID AND a.RowID = b.JournalID AND b.Journal_Date <=@ToDate;
		else
			INSERT INTO @LedgerTransact SELECT a.TransactDate,@AnotherLedgerID,a.Credit_Amount, a.Debit_Amount,a.VoucherType,b.Voucher_No,a.RowID from acc.tblTransaction a,Acc.tblJournalMaster b WHERE a.VoucherType='JRNL' And a.RowID=@RowID AND a.LedgerID =@LedgerID AND a.RowID = b.JournalID;
	
    
    
    
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
    
     if(@FromDate is not null)
		if(@ToDate is not null)
			INSERT INTO @LedgerTransact SELECT a.TransactDate,@AnotherLedgerID,a.Credit_Amount, a.Debit_Amount,a.VoucherType,b.Voucher_No,a.RowID from acc.tblTransaction a,Acc.tblContraMaster b WHERE a.VoucherType='CNTR' And a.RowID=@RowID AND a.LedgerID =@LedgerID AND a.RowID = b.ContraID AND b.Contra_Date BETWEEN @FromDate and @ToDate; 
		else
			INSERT INTO @LedgerTransact SELECT a.TransactDate,@AnotherLedgerID,a.Credit_Amount, a.Debit_Amount,a.VoucherType,b.Voucher_No,a.RowID from acc.tblTransaction a,Acc.tblContraMaster b WHERE a.VoucherType='CNTR' And a.RowID=@RowID AND a.LedgerID =@LedgerID AND a.RowID = b.ContraID AND b.Contra_Date >= @FromDate;
	else
		if(@ToDate is not null)
			INSERT INTO @LedgerTransact SELECT a.TransactDate,@AnotherLedgerID,a.Credit_Amount, a.Debit_Amount,a.VoucherType,b.Voucher_No,a.RowID from acc.tblTransaction a,Acc.tblContraMaster b WHERE a.VoucherType='CNTR' And a.RowID=@RowID AND a.LedgerID =@LedgerID AND a.RowID = b.ContraID AND b.Contra_Date <=@ToDate;
		else
			INSERT INTO @LedgerTransact SELECT a.TransactDate,@AnotherLedgerID,a.Credit_Amount, a.Debit_Amount,a.VoucherType,b.Voucher_No,a.RowID from acc.tblTransaction a,Acc.tblContraMaster b WHERE a.VoucherType='CNTR' And a.RowID=@RowID AND a.LedgerID =@LedgerID AND a.RowID = b.ContraID;
	
	
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
    if(@FromDate is not null)
		if(@ToDate is not null)
			INSERT INTO @LedgerTransact SELECT a.TransactDate,@AnotherLedgerID,a.Credit_Amount, a.Debit_Amount,a.VoucherType,b.Voucher_No,a.RowID from acc.tblTransaction a,Acc.tblCashReceiptMaster b WHERE a.VoucherType='CASH_RCPT' And a.RowID=@RowID AND a.LedgerID =@LedgerID AND a.RowID = b.CashReceiptID AND b.CashReceipt_Date BETWEEN @FromDate and @ToDate; 
		else
			INSERT INTO @LedgerTransact SELECT a.TransactDate,@AnotherLedgerID,a.Credit_Amount, a.Debit_Amount,a.VoucherType,b.Voucher_No,a.RowID from acc.tblTransaction a,Acc.tblCashReceiptMaster b WHERE a.VoucherType='CASH_RCPT' And a.RowID=@RowID AND a.LedgerID =@LedgerID AND a.RowID = b.CashReceiptID AND b.CashReceipt_Date >= @FromDate;
	else
		if(@ToDate is not null)
			INSERT INTO @LedgerTransact SELECT a.TransactDate,@AnotherLedgerID,a.Credit_Amount, a.Debit_Amount,a.VoucherType,b.Voucher_No,a.RowID from acc.tblTransaction a,Acc.tblCashReceiptMaster b WHERE a.VoucherType='CASH_RCPT' And a.RowID=@RowID AND a.LedgerID =@LedgerID AND a.RowID = b.CashReceiptID AND b.CashReceipt_Date <=@ToDate;
		else
			INSERT INTO @LedgerTransact SELECT a.TransactDate,@AnotherLedgerID,a.Credit_Amount, a.Debit_Amount,a.VoucherType,b.Voucher_No,a.RowID from acc.tblTransaction a,Acc.tblCashReceiptMaster b WHERE a.VoucherType='CASH_RCPT' And a.RowID=@RowID AND a.LedgerID =@LedgerID AND a.RowID = b.CashReceiptID;

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
    
    if(@FromDate is not null)
		if(@ToDate is not null)
			INSERT INTO @LedgerTransact SELECT a.TransactDate,@AnotherLedgerID,a.Credit_Amount, a.Debit_Amount,a.VoucherType,b.Voucher_No,a.RowID from acc.tblTransaction a,Acc.tblCashPaymentMaster b WHERE a.VoucherType='CASH_PMNT' And a.RowID=@RowID AND a.LedgerID =@LedgerID AND a.RowID = b.CashPaymentID AND b.CashPayment_Date BETWEEN @FromDate and @ToDate; 
		else
			INSERT INTO @LedgerTransact SELECT a.TransactDate,@AnotherLedgerID,a.Credit_Amount, a.Debit_Amount,a.VoucherType,b.Voucher_No,a.RowID from acc.tblTransaction a,Acc.tblCashPaymentMaster b WHERE a.VoucherType='CASH_PMNT' And a.RowID=@RowID AND a.LedgerID =@LedgerID AND a.RowID = b.CashPaymentID AND b.CashPayment_Date >= @FromDate;
	else
		if(@ToDate is not null)
			INSERT INTO @LedgerTransact SELECT a.TransactDate,@AnotherLedgerID,a.Credit_Amount, a.Debit_Amount,a.VoucherType,b.Voucher_No,a.RowID from acc.tblTransaction a,Acc.tblCashPaymentMaster b WHERE a.VoucherType='CASH_PMNT' And a.RowID=@RowID AND a.LedgerID =@LedgerID AND a.RowID = b.CashPaymentID AND b.CashPayment_Date <=@ToDate;
		else
			INSERT INTO @LedgerTransact SELECT a.TransactDate,@AnotherLedgerID,a.Credit_Amount, a.Debit_Amount,a.VoucherType,b.Voucher_No,a.RowID from acc.tblTransaction a,Acc.tblCashPaymentMaster b WHERE a.VoucherType='CASH_PMNT' And a.RowID=@RowID AND a.LedgerID =@LedgerID AND a.RowID = b.CashPaymentID;

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
    
    if(@FromDate is not null)
		if(@ToDate is not null)
			INSERT INTO @LedgerTransact SELECT a.TransactDate,@AnotherLedgerID,a.Credit_Amount, a.Debit_Amount,a.VoucherType,b.Voucher_No,a.RowID from acc.tblTransaction a,Acc.tblBankPaymentMaster b WHERE a.VoucherType='BANK_PMNT' And a.RowID=@RowID AND a.LedgerID =@LedgerID AND a.RowID = b.BankPaymentID AND b.BankPayment_Date BETWEEN @FromDate and @ToDate; 
		else
			INSERT INTO @LedgerTransact SELECT a.TransactDate,@AnotherLedgerID,a.Credit_Amount, a.Debit_Amount,a.VoucherType,b.Voucher_No,a.RowID from acc.tblTransaction a,Acc.tblBankPaymentMaster b WHERE a.VoucherType='BANK_PMNT' And a.RowID=@RowID AND a.LedgerID =@LedgerID AND a.RowID = b.BankPaymentID AND b.BankPayment_Date >= @FromDate;
	else
		if(@ToDate is not null)
			INSERT INTO @LedgerTransact SELECT a.TransactDate,@AnotherLedgerID,a.Credit_Amount, a.Debit_Amount,a.VoucherType,b.Voucher_No,a.RowID from acc.tblTransaction a,Acc.tblBankPaymentMaster b WHERE a.VoucherType='BANK_PMNT' And a.RowID=@RowID AND a.LedgerID =@LedgerID AND a.RowID = b.BankPaymentID AND b.BankPayment_Date<=@ToDate;
		else
			INSERT INTO @LedgerTransact SELECT a.TransactDate,@AnotherLedgerID,a.Credit_Amount, a.Debit_Amount,a.VoucherType,b.Voucher_No,a.RowID from acc.tblTransaction a,Acc.tblBankPaymentMaster b WHERE a.VoucherType='BANK_PMNT' And a.RowID=@RowID AND a.LedgerID =@LedgerID AND a.RowID = b.BankPaymentID;

	FETCH NEXT FROM db_cursor INTO @RowID   
END   

CLOSE db_cursor   
DEALLOCATE db_cursor

-------------------------------FOR BANK RECEIPT -----------------------------------------
DECLARE db_cursor CURSOR FOR SELECT RowID FROM @VchNRow WHERE VoucherType='BANK_RCPT'

OPEN db_cursor FETCH NEXT FROM db_cursor INTO @RowID  
	
WHILE @@FETCH_STATUS = 0   
BEGIN

	--Get the ID of Another Ledger ID from the bunch of transaction.
	SELECT Top 1 @AnotherLedgerID=LedgerID from acc.tblTransaction WHERE VoucherType='BANK_RCPT' And RowID=@RowID AND LedgerID <> @LedgerID;
    --Note the swapping of Debit and Credit Amounts
    
     if(@FromDate is not null)
		if(@ToDate is not null)
			INSERT INTO @LedgerTransact SELECT a.TransactDate,@AnotherLedgerID,a.Credit_Amount, a.Debit_Amount,a.VoucherType,b.Voucher_No,a.RowID from acc.tblTransaction a,Acc.tblBankReceiptMaster b WHERE a.VoucherType='BANK_RCPT' And a.RowID=@RowID AND a.LedgerID =@LedgerID AND a.RowID = b.BankReceiptID AND b.BankReceipt_Date BETWEEN @FromDate and @ToDate; 
		else
			INSERT INTO @LedgerTransact SELECT a.TransactDate,@AnotherLedgerID,a.Credit_Amount, a.Debit_Amount,a.VoucherType,b.Voucher_No,a.RowID from acc.tblTransaction a,Acc.tblBankReceiptMaster b WHERE a.VoucherType='BANK_RCPT' And a.RowID=@RowID AND a.LedgerID =@LedgerID AND a.RowID = b.BankReceiptID AND b.BankReceipt_Date >= @FromDate;
	else
		if(@ToDate is not null)
			INSERT INTO @LedgerTransact SELECT a.TransactDate,@AnotherLedgerID,a.Credit_Amount, a.Debit_Amount,a.VoucherType,b.Voucher_No,a.RowID from acc.tblTransaction a,Acc.tblBankReceiptMaster b WHERE a.VoucherType='BANK_RCPT' And a.RowID=@RowID AND a.LedgerID =@LedgerID AND a.RowID = b.BankReceiptID AND b.BankReceipt_Date <=@ToDate;
		else
			INSERT INTO @LedgerTransact SELECT a.TransactDate,@AnotherLedgerID,a.Credit_Amount, a.Debit_Amount,a.VoucherType,b.Voucher_No,a.RowID from acc.tblTransaction a,Acc.tblBankReceiptMaster b WHERE a.VoucherType='BANK_RCPT' And a.RowID=@RowID AND a.LedgerID =@LedgerID AND a.RowID = b.BankReceiptID;

	FETCH NEXT FROM db_cursor INTO @RowID   
END   

CLOSE db_cursor   
DEALLOCATE db_cursor

---------------------------


Select cast(a.Date as date) as LedgerDate,b.EngName Account, a.LedgerID,a.Credit as Debit,a.Debit as Credit,a.VoucherType,a.VoucherNumber,a.RowID,Date.fnEngtoNep(a.Date) as NepLedgerDate from @LedgerTransact a,Acc.tblLedger b WHERE a.LedgerID=b.LedgerID order by a.Date
END
EXEC sp_xml_removedocument @docHandle 




GO


