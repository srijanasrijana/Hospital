--USE [SABL001]
--GO

/****** Object:  StoredProcedure [Acc].[spGetTransactionDetails]    Script Date: 6/2/2017 11:28:07 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



--Acc.spGetLedgerTransact
ALTER PROC [Acc].[spGetTransactionDetails]


--Get the transaction for the particular ledger

--product name | unit | net sales qty  | Net Amt
--Acc Class filter, Date filter, depot, single product, sales ledger, project
@FromDate DATETIME=NULL,
@ToDate DATETIME = NULL, 
@AccountClassIDsSettings  xml=NULL,    -- It encludes AccountCLassIDs  Info
@ProjectIDsSettings xml=NULL, --Null means all
@Settings xml=NULL --Null means all
  AS
DECLARE @docHandle int;
DECLARE @docHandle1 int;
EXEC sp_xml_preparedocument @docHandle OUTPUT, @AccountClassIDsSettings;
EXEC sp_xml_preparedocument @docHandle1 OUTPUT, @ProjectIDsSettings;

BEGIN

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
Debit DECIMAL(19,5),
Credit DECIMAL(19,5),
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
    SELECT ProjectID FROM  Acc.fnGetAllProjects(@ProjectIDsSettings);




DECLARE @SqlSmt NVARCHAR(300);
--SET @SqlSmt = 'SELECT DISTINCT VoucherType,RowID FROM Acc.tblTransaction WHERE ledgerID = '+CONVERT(NVARCHAR,@LedgerID)+'AND TransactionID IN(SELECT TransactionID FROM Acc.tblTransactionClass WHERE AccClassID IN ('+@AccClassIDs+ '))';
--Get ledger transactions which falls on the accounting class
--INSERT INTO @VchNRow execute sp_executesql @SqlSmt;

--Collect All records from tblTransaction filtering date
if(@FromDate is not null)
	if(@ToDate is not null)

			INSERT INTO @VchNRow SELECT DISTINCT VoucherType,RowID FROM Acc.tblTransaction WHERE  TransactionID IN(SELECT TransactionID FROM Acc.tblTransactionClass WHERE AccClassID IN (SELECT AccClassID FROM @AccClassID) and ProjectID in (select ProjectID from @ProjectID)) AND TransactDate BETWEEN @FromDate and @ToDate; 

	else
			INSERT INTO @VchNRow SELECT DISTINCT VoucherType,RowID FROM Acc.tblTransaction WHERE  TransactionID IN(SELECT TransactionID FROM Acc.tblTransactionClass WHERE AccClassID IN (SELECT AccClassID FROM @AccClassID) and ProjectID in (select ProjectID from @ProjectID)) AND TransactDate >= @FromDate;
else
	if(@ToDate is not null)
			INSERT INTO @VchNRow SELECT DISTINCT VoucherType,RowID FROM Acc.tblTransaction WHERE  TransactionID IN(SELECT TransactionID FROM Acc.tblTransactionClass WHERE AccClassID IN (SELECT AccClassID FROM @AccClassID) and ProjectID in (select ProjectID from @ProjectID)) AND TransactDate <=@ToDate;
	else
			INSERT INTO @VchNRow SELECT DISTINCT VoucherType,RowID FROM Acc.tblTransaction WHERE  TransactionID IN(SELECT TransactionID FROM Acc.tblTransactionClass WHERE AccClassID IN (SELECT AccClassID FROM @AccClassID) and ProjectID in (select ProjectID from @ProjectID));

DECLARE @RowID INT;
DECLARE @AnotherLedgerID INT;
DECLARE @Date DateTime;
DECLARE @VoucherNo NVARCHAR(50)

DECLARE @Columns TABLE
(
	_Column NVARCHAR(20)
);

-- Write to temporary table @SourceProjectID from xml
INSERT INTO @Columns(_Column)
    SELECT
        IdNode.value('(.)[1]', 'nvarchar(20)')
    FROM
        @Settings.nodes('/SETTINGS/COLUMN') AS IdTbl(IdNode);
        
        
DECLARE @SelectColumn NVARCHAR(500) --HOLDS THE SELECTED COLUMNS FOR OUTPUT
SELECT   @SelectColumn = COALESCE(@SelectColumn + ', ', '') + _Column FROM @Columns 
--SELECT @SelectColumn 
--SELECT * FROM @Columns;
--return
  

----FOR JOURNAL----

DECLARE db_cursor CURSOR FOR SELECT RowID FROM @VchNRow WHERE VoucherType='JRNL'

OPEN db_cursor FETCH NEXT FROM db_cursor INTO @RowID  

WHILE @@FETCH_STATUS = 0   
BEGIN

--Get Date and Voucher no. details from Journal
SELECT @Date=Journal_Date, @VoucherNo=Voucher_No  FROM Acc.tblJournalMaster WHERE JournalID=@RowID

--Now Log from Details
INSERT INTO @LedgerTransact SELECT @Date,dtl.LedgerID,
CASE 
         WHEN UPPER(DrCr)='DEBIT' THEN Amount
         ELSE 0
       END AS Debit, 
CASE WHEN UPPER(DrCr)='CREDIT' THEN Amount
         ELSE 0
       END AS Credit,
       
       'JRNL',@VoucherNo,@RowID FROM Acc.tblJournalDetail dtl WHERE dtl.JournalID=@RowID;

	FETCH NEXT FROM db_cursor INTO @RowID   
END   

CLOSE db_cursor   
DEALLOCATE db_cursor

-------------------------
----FOR CONTRA----

DECLARE db_cursor CURSOR FOR SELECT RowID FROM @VchNRow WHERE VoucherType='CNTR'

OPEN db_cursor FETCH NEXT FROM db_cursor INTO @RowID  

WHILE @@FETCH_STATUS = 0   
BEGIN

--Get Date and Voucher no. details from Journal
SELECT @Date=Contra_Date, @VoucherNo=Voucher_No  FROM Acc.tblContraMaster WHERE ContraID=@RowID

--Now Log from Details
INSERT INTO @LedgerTransact SELECT @Date,dtl.LedgerID,
CASE 
         WHEN UPPER(DrCr)='DEBIT' THEN Amount
         ELSE 0
       END AS Debit, 
CASE 
         WHEN UPPER(DrCr)='CREDIT' THEN Amount
         ELSE 0
       END AS Credit,
       'CNTR',@VoucherNo,@RowID FROM Acc.tblContraDetails dtl WHERE dtl.ContraID =@RowID;

	FETCH NEXT FROM db_cursor INTO @RowID   
END   

CLOSE db_cursor   
DEALLOCATE db_cursor

-------------------------
----FOR CASH_PAYMENT----

DECLARE db_cursor CURSOR FOR SELECT RowID FROM @VchNRow WHERE VoucherType='CASH_PMNT'

OPEN db_cursor FETCH NEXT FROM db_cursor INTO @RowID  

WHILE @@FETCH_STATUS = 0   
BEGIN

--Get Date and Voucher no. details from Journal
SELECT @Date=CashPayment_Date, @VoucherNo=Voucher_No  FROM Acc.tblCashPaymentMaster WHERE CashPaymentID=@RowID


--Now Log from Details
DECLARE @SUMDEBIT MONEY;
--- Debit
INSERT INTO @LedgerTransact SELECT @Date,dtl.LedgerID,dtl.Amount,0,
'CASH_PMNT',@VoucherNo,@RowID FROM acc.tblCashPaymentDetails dtl WHERE dtl.CashPaymentID=@RowID;

SELECT  @SUMDEBIT= SUM(dtl.Amount) FROM acc.tblCashPaymentDetails dtl WHERE dtl.CashPaymentID=@RowID;

--Credit

INSERT INTO @LedgerTransact SELECT @Date,mstr.LedgerID,0,
@SUMDEBIT,'CASH_PMNT',@VoucherNo,@RowID FROM acc.tblCashPaymentMaster mstr WHERE mstr.CashPaymentID=@RowID;



	FETCH NEXT FROM db_cursor INTO @RowID   
END   

CLOSE db_cursor   
DEALLOCATE db_cursor


-------------------------
----FOR CASH_RECEIPT----

DECLARE db_cursor CURSOR FOR SELECT RowID FROM @VchNRow WHERE VoucherType='CASH_RCPT'

OPEN db_cursor FETCH NEXT FROM db_cursor INTO @RowID  

WHILE @@FETCH_STATUS = 0   
BEGIN

--Get Date and Voucher no. details from Journal
SELECT @Date=CashReceipt_Date, @VoucherNo=Voucher_No  FROM Acc.tblCashReceiptMaster WHERE CashReceiptID=@RowID

--Now Log from Details

--- CREDIT

INSERT INTO @LedgerTransact SELECT @Date,dtl.LedgerID,0,dtl.Amount,
'CASH_RCPT',@VoucherNo,@RowID FROM acc.tblCashReceiptDetails dtl WHERE dtl.CashReceiptID=@RowID;

--DEBIT
DECLARE @SUMDEBITdr MONEY;
SELECT  @SUMDEBITdr= SUM(dtl.Amount) FROM acc.tblCashReceiptDetails dtl WHERE dtl.CashReceiptID=@RowID;
INSERT INTO @LedgerTransact SELECT @Date,mastr.LedgerID,
 @SUMDEBITdr,0,'CASH_RCPT',@VoucherNo,@RowID FROM acc.tblCashReceiptMaster mastr WHERE mastr.CashReceiptID=@RowID;

	FETCH NEXT FROM db_cursor INTO @RowID   
END   

CLOSE db_cursor   
DEALLOCATE db_cursor


-------------------------
----FOR BANK_PAYMENT----

DECLARE db_cursor CURSOR FOR SELECT RowID FROM @VchNRow WHERE VoucherType='BANK_PMNT'

OPEN db_cursor FETCH NEXT FROM db_cursor INTO @RowID  

WHILE @@FETCH_STATUS = 0   
BEGIN

--Get Date and Voucher no. details from Journal
SELECT @Date=BankPayment_Date, @VoucherNo=Voucher_No  FROM Acc.tblBankPaymentMaster WHERE BankPaymentID=@RowID

--Now Log from Details

--- Debit

INSERT INTO @LedgerTransact SELECT @Date,dtl.LedgerID,dtl.Amount,0,
'BANK_PMNT',@VoucherNo,@RowID FROM acc.tblBankPaymentDetails dtl WHERE dtl.BankPaymentID=@RowID;

--Credit
DECLARE @SUMDEBITbp MONEY;
SELECT  @SUMDEBITbp= SUM(dtl.Amount) FROM acc.tblBankPaymentDetails dtl WHERE dtl.BankPaymentID=@RowID;
INSERT INTO @LedgerTransact SELECT @Date,mstr.LedgerID,0,
@SUMDEBITbp,'BANK_PMNT',@VoucherNo,@RowID FROM acc.tblBankPaymentMaster mstr WHERE mstr.BankPaymentID=@RowID;


	FETCH NEXT FROM db_cursor INTO @RowID   
END   

CLOSE db_cursor   
DEALLOCATE db_cursor


-------------------------
----FOR BANK_RECEIPT----

DECLARE db_cursor CURSOR FOR SELECT RowID FROM @VchNRow WHERE VoucherType='BANK_RCPT'

OPEN db_cursor FETCH NEXT FROM db_cursor INTO @RowID  

WHILE @@FETCH_STATUS = 0   
BEGIN

--Get Date and Voucher no. details from Journal
SELECT @Date=BankReceipt_Date, @VoucherNo=Voucher_No  FROM Acc.tblBankReceiptMaster WHERE BankReceiptID=@RowID

--Now Log from Details

--- CREDIT

INSERT INTO @LedgerTransact SELECT @Date,dtl.LedgerID,0,
dtl.Amount,'BANK_RCPT',@VoucherNo,@RowID FROM acc.tblBankReceiptDetails dtl WHERE dtl.BankReceiptID=@RowID;

--DEBIT
DECLARE @SUMDEBITBR MONEY;
SELECT  @SUMDEBITBR= SUM(dtl.Amount) FROM acc.tblBankReceiptDetails dtl WHERE dtl.BankReceiptID=@RowID;
INSERT INTO @LedgerTransact SELECT @Date,mstr.LedgerID,@SUMDEBITBR,0,
'BANK_RCPT',@VoucherNo,@RowID FROM acc.tblBankReceiptMaster mstr WHERE  mstr.BankReceiptID=@RowID;






	FETCH NEXT FROM db_cursor INTO @RowID   
END   

CLOSE db_cursor   
DEALLOCATE db_cursor


---------------------------
----FOR PURCHASE-----

DECLARE db_cursor CURSOR FOR SELECT RowID FROM @VchNRow WHERE VoucherType='PURCH'

OPEN db_cursor FETCH NEXT FROM db_cursor INTO @RowID  

WHILE @@FETCH_STATUS = 0   
BEGIN

--Get Date and Voucher no. details from Journal
SELECT @Date=PurchaseInvoice_Date, @VoucherNo=Voucher_No  FROM Inv.tblPurchaseInvoiceMaster WHERE PurchaseInvoiceID=@RowID

--Now Log from Details

--Purchase Ledger
INSERT INTO @LedgerTransact SELECT @Date,mst.PurchaseLedgerID,mst.Net_Amount,0,
'PURCH',@VoucherNo,@RowID FROM Inv.tblPurchaseInvoiceMaster mst WHERE mst.PurchaseInvoiceID=@RowID;

--Cash Party 
INSERT INTO @LedgerTransact SELECT @Date,mst.CashPartyLedgerID,0,
mst.Total_Amount,'PURCH',@VoucherNo,@RowID FROM Inv.tblPurchaseInvoiceMaster mst WHERE mst.PurchaseInvoiceID=@RowID;

--VAT
INSERT INTO @LedgerTransact SELECT @Date,4698,mst.VAT,0,
'PURCH',@VoucherNo,@RowID FROM Inv.tblPurchaseInvoiceMaster mst WHERE mst.PurchaseInvoiceID=@RowID;

--Tax1
INSERT INTO @LedgerTransact SELECT @Date,25717,
mst.Tax1,0,'PURCH',@VoucherNo,@RowID FROM Inv.tblPurchaseInvoiceMaster mst WHERE mst.PurchaseInvoiceID=@RowID;

--Tax2
INSERT INTO @LedgerTransact SELECT @Date,25718,
mst.Tax2,0,'PURCH',@VoucherNo,@RowID FROM Inv.tblPurchaseInvoiceMaster mst WHERE mst.PurchaseInvoiceID=@RowID;

--Tax3
INSERT INTO @LedgerTransact SELECT @Date,25719,
mst.Tax3,0,'PURCH',@VoucherNo,@RowID FROM Inv.tblPurchaseInvoiceMaster mst WHERE mst.PurchaseInvoiceID=@RowID;

--Custom Duty
INSERT INTO @LedgerTransact SELECT @Date,504,
mst.CustomDuty,0,'PURCH',@VoucherNo,@RowID FROM Inv.tblPurchaseInvoiceMaster mst WHERE mst.PurchaseInvoiceID=@RowID;

--Freight
INSERT INTO @LedgerTransact SELECT @Date,503,
mst.Freight,0,'PURCH',@VoucherNo,@RowID FROM Inv.tblPurchaseInvoiceMaster mst WHERE mst.PurchaseInvoiceID=@RowID;

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
	
	SELECT @Date=PurchaseReturn_Date, @VoucherNo=Voucher_No  FROM Inv.tblPurchaseReturnMaster WHERE PurchaseReturnID=@RowID
  --Now Log from Details
  
--PurchaseReturn Ledger
INSERT INTO @LedgerTransact SELECT @Date,mst.PurchaseLedgerID,0,
mst.Net_Amount,'PURCH_RTN',@VoucherNo,@RowID FROM Inv.tblPurchaseReturnMaster mst WHERE mst.PurchaseReturnID=@RowID;


--Cash Party 
INSERT INTO @LedgerTransact SELECT @Date,mst.CashPartyLedgerID,
mst.Total_Amt,0,'PURCH_RTN',@VoucherNo,@RowID FROM Inv.tblPurchaseReturnMaster mst WHERE mst.PurchaseReturnID=@RowID;

--VAT
INSERT INTO @LedgerTransact SELECT @Date,4698,0,mst.VAT,
'PURCH_RTN',@VoucherNo,@RowID FROM Inv.tblPurchaseReturnMaster mst WHERE mst.PurchaseReturnID=@RowID;
    
 --Tax1
INSERT INTO @LedgerTransact SELECT @Date,25717,0,mst.Tax1,
'PURCH_RTN',@VoucherNo,@RowID FROM Inv.tblPurchaseReturnMaster mst WHERE mst.PurchaseReturnID=@RowID;

--Tax2
INSERT INTO @LedgerTransact SELECT @Date,25718,0,mst.Tax2,
'PURCH_RTN',@VoucherNo,@RowID FROM Inv.tblPurchaseReturnMaster mst WHERE mst.PurchaseReturnID=@RowID;

--Tax3
INSERT INTO @LedgerTransact SELECT @Date,25719,0,mst.Tax3,
'PURCH_RTN',@VoucherNo,@RowID FROM Inv.tblPurchaseReturnMaster mst WHERE mst.PurchaseReturnID=@RowID;
   

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
	
	SELECT @Date=SalesInvoice_Date, @VoucherNo=Voucher_No  FROM Inv.tblSalesInvoiceMaster WHERE SalesInvoiceID=@RowID;
  --Now Log from Details
  
--SALES Ledger
INSERT INTO @LedgerTransact SELECT @Date,mst.SalesLedgerID,0,
mst.Net_Amount,'SALES',@VoucherNo,@RowID  FROM Inv.tblSalesInvoiceMaster mst WHERE SalesInvoiceID=@RowID;


--Cash Party 
INSERT INTO @LedgerTransact SELECT @Date,mst.CashPartyLedgerID,
mst.Total_Amount,0,'SALES',@VoucherNo,@RowID  FROM Inv.tblSalesInvoiceMaster mst WHERE SalesInvoiceID=@RowID;

--VAT
INSERT INTO @LedgerTransact SELECT @Date,412,0,mst.VAT,
'SALES',@VoucherNo,@RowID  FROM Inv.tblSalesInvoiceMaster  mst WHERE SalesInvoiceID=@RowID;
    
 --Tax1
INSERT INTO @LedgerTransact SELECT @Date,314,0,mst.Tax1,
'SALES',@VoucherNo,@RowID  FROM Inv.tblSalesInvoiceMaster mst WHERE SalesInvoiceID=@RowID;

--Tax2
INSERT INTO @LedgerTransact SELECT @Date,315,0,mst.Tax2,
'SALES',@VoucherNo,@RowID  FROM Inv.tblSalesInvoiceMaster  mst WHERE SalesInvoiceID=@RowID

--Tax3
INSERT INTO @LedgerTransact SELECT @Date,316,0,mst.Tax3,
'SALES',@VoucherNo,@RowID  FROM Inv.tblSalesInvoiceMaster mst  WHERE SalesInvoiceID=@RowID;
   

	FETCH NEXT FROM db_cursor INTO @RowID   
END   

CLOSE db_cursor   
DEALLOCATE db_cursor

-----------------------FOR SALES RETURN---------------------------

DECLARE db_cursor CURSOR FOR SELECT RowID FROM @VchNRow WHERE VoucherType='SLS_RTN'
OPEN db_cursor FETCH NEXT FROM db_cursor INTO @RowID  

WHILE @@FETCH_STATUS = 0   
BEGIN
	--Get the ID of Another Ledger ID from the bunch of transaction.
	
	SELECT @Date=SalesReturn_Date, @VoucherNo=Voucher_No  FROM Inv.tblSalesReturnMaster WHERE SalesReturnID=@RowID
  --Now Log from Details
  
--sales Return Ledger
INSERT INTO @LedgerTransact SELECT @Date,mst.SalesLedgerID,
mst.Net_Amount,0,'SLS_RTN',@VoucherNo,@RowID FROM Inv.tblSalesReturnMaster mst WHERE mst.SalesReturnID=@RowID;


--Cash Party 
INSERT INTO @LedgerTransact SELECT @Date,mst.CashPartyLedgerID,0,
mst.Total_Amt,'SLS_RTN',@VoucherNo,@RowID FROM Inv.tblSalesReturnMaster mst WHERE mst.SalesReturnID=@RowID;

--VAT
INSERT INTO @LedgerTransact SELECT @Date,412,mst.VAT,0,
'SLS_RTN',@VoucherNo,@RowID FROM Inv.tblSalesReturnMaster mst WHERE mst.SalesReturnID=@RowID;

 --Tax1
INSERT INTO @LedgerTransact SELECT @Date,314,mst.Tax1,
0,'SLS_RTN',@VoucherNo,@RowID FROM Inv.tblSalesReturnMaster mst WHERE mst.SalesReturnID=@RowID;

--Tax2
INSERT INTO @LedgerTransact SELECT @Date,315,mst.Tax2,
0,'SLS_RTN',@VoucherNo,@RowID FROM Inv.tblSalesReturnMaster mst WHERE mst.SalesReturnID=@RowID;

--Tax3
INSERT INTO @LedgerTransact SELECT @Date,316,mst.Tax3,
0,'SLS_RTN',@VoucherNo,@RowID FROM Inv.tblSalesReturnMaster mst WHERE mst.SalesReturnID=@RowID;
   

	FETCH NEXT FROM db_cursor INTO @RowID   
END   

CLOSE db_cursor   
DEALLOCATE db_cursor

Select a.Date as LedgerDate, Date.fnEngtoNep(a.Date) as LedgerNepDate,b.EngName Account, a.LedgerID,a.Debit as Debit,a.Credit as Credit,a.VoucherType,a.VoucherNumber,a.RowID from @LedgerTransact a,Acc.tblLedger b WHERE a.LedgerID=b.LedgerID order by a.Date
END
EXEC sp_xml_removedocument @docHandle 

GO


