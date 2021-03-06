
/****** Object:  StoredProcedure [Inv].[spGetVouchersDetailsSalesReturn]    Script Date: 10/12/2015 03:17:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--Acc.spGetLedgerTransact
ALTER PROC [Inv].[spGetVouchersDetailsSalesReturn]


--Get the transaction for the particular ledger

--product name | unit | net sales qty  | Net Amt
--Acc Class filter, Date filter, depot, single product, sales ledger, project
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
Amount DECIMAL(19,5),
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
    SELECT
        IdNode.value('(.)[1]', 'int')
    FROM
        @AccountClassIDsSettings.nodes('/LEDGERTRANSACT/AccClassIDSettings/AccClassID') AS IdTbl(IdNode)


--Read All Accounting Class
DECLARE @ProjectID TABLE
(
	ProjectID INT 
);

-- Write to temporary table @@ProjectID from xml
INSERT INTO @ProjectID(ProjectID)
    SELECT
        IdNode.value('(.)[1]', 'int')
    FROM
        @ProjectIDsSettings.nodes('/LEDGERTRANSACT/ProjectIDSettings/ProjectID') AS IdTbl(IdNode)

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



----FOR JOURNAL----

--DECLARE db_cursor CURSOR FOR SELECT RowID FROM @VchNRow WHERE VoucherType='JRNL'

--OPEN db_cursor FETCH NEXT FROM db_cursor INTO @RowID  

--WHILE @@FETCH_STATUS = 0   
--BEGIN

----Get Date and Voucher no. details from Journal
--SELECT @Date=Journal_Date, @VoucherNo=Voucher_No  FROM Acc.tblJournalMaster WHERE JournalID=@RowID

----Now Log from Details
--INSERT INTO @LedgerTransact SELECT @Date,dtl.LedgerID,dtl.Amount,

-- 'JRNL',@VoucherNo,@RowID FROM Acc.tblJournalDetail dtl WHERE dtl.JournalID=@RowID;

--	FETCH NEXT FROM db_cursor INTO @RowID   
--END   

--CLOSE db_cursor   
--DEALLOCATE db_cursor

-------------------------
------FOR CONTRA----

--DECLARE db_cursor CURSOR FOR SELECT RowID FROM @VchNRow WHERE VoucherType='CNTR'

--OPEN db_cursor FETCH NEXT FROM db_cursor INTO @RowID  

--WHILE @@FETCH_STATUS = 0   
--BEGIN

----Get Date and Voucher no. details from Journal
--SELECT @Date=Contra_Date, @VoucherNo=Voucher_No  FROM Acc.tblContraMaster WHERE ContraID=@RowID

----Now Log from Details
--INSERT INTO @LedgerTransact SELECT @Date,dtl.LedgerID,dtl.Amount,

--       'CNTR',@VoucherNo,@RowID FROM Acc.tblContraDetails dtl WHERE dtl.ContraID =@RowID;

--	FETCH NEXT FROM db_cursor INTO @RowID   
--END   

--CLOSE db_cursor   
--DEALLOCATE db_cursor

---------------------------
------FOR CASH_PAYMENT----

--DECLARE db_cursor CURSOR FOR SELECT RowID FROM @VchNRow WHERE VoucherType='CASH_PMNT'

--OPEN db_cursor FETCH NEXT FROM db_cursor INTO @RowID  

--WHILE @@FETCH_STATUS = 0   
--BEGIN

----Get Date and Voucher no. details from Journal
--SELECT @Date=CashPayment_Date, @VoucherNo=Voucher_No  FROM Acc.tblCashPaymentMaster WHERE CashPaymentID=@RowID

----Now Log from Details

----- Debit

--INSERT INTO @LedgerTransact SELECT @Date,dtl.LedgerID,dtl.Amount,
--'CASH_PMNT',@VoucherNo,@RowID FROM acc.tblCashPaymentDetails dtl WHERE dtl.CashPaymentID=@RowID;



--	FETCH NEXT FROM db_cursor INTO @RowID   
--END   

--CLOSE db_cursor   
--DEALLOCATE db_cursor


---------------------------
------FOR CASH_RECEIPT----

--DECLARE db_cursor CURSOR FOR SELECT RowID FROM @VchNRow WHERE VoucherType='CASH_RCPT'

--OPEN db_cursor FETCH NEXT FROM db_cursor INTO @RowID  

--WHILE @@FETCH_STATUS = 0   
--BEGIN

----Get Date and Voucher no. details from Journal
--SELECT @Date=CashReceipt_Date, @VoucherNo=Voucher_No  FROM Acc.tblCashReceiptMaster WHERE CashReceiptID=@RowID

----Now Log from Details

----- CREDIT

--INSERT INTO @LedgerTransact SELECT @Date,dtl.LedgerID,dtl.Amount,
--'CASH_RCPT',@VoucherNo,@RowID FROM acc.tblCashReceiptDetails dtl WHERE dtl.CashReceiptID=@RowID;



--	FETCH NEXT FROM db_cursor INTO @RowID   
--END   

--CLOSE db_cursor   
--DEALLOCATE db_cursor


-------------------------
--FOR BANK_PAYMENT----

--DECLARE db_cursor CURSOR FOR SELECT RowID FROM @VchNRow WHERE VoucherType='BANK_PMNT'

--OPEN db_cursor FETCH NEXT FROM db_cursor INTO @RowID  

--WHILE @@FETCH_STATUS = 0   
--BEGIN

----Get Date and Voucher no. details from Journal
--SELECT @Date=BankPayment_Date, @VoucherNo=Voucher_No  FROM Acc.tblBankPaymentMaster WHERE BankPaymentID=@RowID

----Now Log from Details

----- Debit

--INSERT INTO @LedgerTransact SELECT @Date,dtl.LedgerID,dtl.Amount,
--'BANK_PMNT',@VoucherNo,@RowID FROM acc.tblBankPaymentDetails dtl WHERE dtl.BankPaymentID=@RowID;




--	FETCH NEXT FROM db_cursor INTO @RowID   
--END   

--CLOSE db_cursor   
--DEALLOCATE db_cursor


---------------------------
------FOR BANK_RECEIPT----

--DECLARE db_cursor CURSOR FOR SELECT RowID FROM @VchNRow WHERE VoucherType='BANK_RCPT'

--OPEN db_cursor FETCH NEXT FROM db_cursor INTO @RowID  

--WHILE @@FETCH_STATUS = 0   
--BEGIN

----Get Date and Voucher no. details from Journal
--SELECT @Date=BankReceipt_Date, @VoucherNo=Voucher_No  FROM Acc.tblBankReceiptMaster WHERE BankReceiptID=@RowID

----Now Log from Details

----- Debit

--INSERT INTO @LedgerTransact SELECT @Date,dtl.LedgerID,dtl.Amount,
--'BANK_RCPT',@VoucherNo,@RowID FROM acc.tblBankReceiptDetails dtl WHERE dtl.BankReceiptID=@RowID;




--	FETCH NEXT FROM db_cursor INTO @RowID   
--END   

--CLOSE db_cursor   
--DEALLOCATE db_cursor


-----------------------------
------FOR PURCHASE-----

--DECLARE db_cursor CURSOR FOR SELECT RowID FROM @VchNRow WHERE VoucherType='PURCH'

--OPEN db_cursor FETCH NEXT FROM db_cursor INTO @RowID  

--WHILE @@FETCH_STATUS = 0   
--BEGIN

----Get Date and Voucher no. details from Journal
--SELECT @Date=PurchaseInvoice_Date, @VoucherNo=Voucher_No  FROM Inv.tblPurchaseInvoiceMaster WHERE PurchaseInvoiceID=@RowID

----Now Log from Details

----Purchase Ledger
--INSERT INTO @LedgerTransact SELECT @Date,mst.PurchaseLedgerID,mst.Net_Amount,
--'PURCH',@VoucherNo,@RowID FROM Inv.tblPurchaseInvoiceMaster mst WHERE mst.PurchaseInvoiceID=@RowID;



--	FETCH NEXT FROM db_cursor INTO @RowID   
--END   

--CLOSE db_cursor   
--DEALLOCATE db_cursor

------FOR PURCHASE RETURN-----

--DECLARE db_cursor CURSOR FOR SELECT RowID FROM @VchNRow WHERE VoucherType='PURCH_RTN'
--OPEN db_cursor FETCH NEXT FROM db_cursor INTO @RowID  

--WHILE @@FETCH_STATUS = 0   
--BEGIN
--	--Get the ID of Another Ledger ID from the bunch of transaction.
	
--	SELECT @Date=PurchaseReturn_Date, @VoucherNo=Voucher_No  FROM Inv.tblPurchaseReturnMaster WHERE PurchaseReturnID=@RowID
--  --Now Log from Details
  
----PurchaseReturn Ledger
--INSERT INTO @LedgerTransact SELECT @Date,mst.PurchaseLedgerID,
--mst.Net_Amount,'PURCH_RTN',@VoucherNo,@RowID FROM Inv.tblPurchaseReturnMaster mst WHERE mst.PurchaseReturnID=@RowID;


--	FETCH NEXT FROM db_cursor INTO @RowID   
--END   

--CLOSE db_cursor   
--DEALLOCATE db_cursor

----------------FOR SALES INVOICE--------------

--DECLARE db_cursor CURSOR FOR SELECT RowID FROM @VchNRow WHERE VoucherType='SALES'
--OPEN db_cursor FETCH NEXT FROM db_cursor INTO @RowID  

--WHILE @@FETCH_STATUS = 0   
--BEGIN
--	--Get the ID of Another Ledger ID from the bunch of transaction.
	
--	SELECT @Date=SalesInvoice_Date, @VoucherNo=Voucher_No  FROM Inv.tblSalesInvoiceMaster WHERE SalesInvoiceID=@RowID;
--  --Now Log from Details
  
----SALES Ledger
--INSERT INTO @LedgerTransact SELECT @Date,mst.SalesLedgerID,
--mst.Net_Amount,'SALES',@VoucherNo,@RowID  FROM Inv.tblSalesInvoiceMaster mst WHERE SalesInvoiceID=@RowID;



   

--	FETCH NEXT FROM db_cursor INTO @RowID   
--END   

--CLOSE db_cursor   
--DEALLOCATE db_cursor

-------------------------FOR SALES RETURN---------------------------

DECLARE db_cursor CURSOR FOR SELECT RowID FROM @VchNRow WHERE VoucherType='SLS_RTN'
OPEN db_cursor FETCH NEXT FROM db_cursor INTO @RowID  

WHILE @@FETCH_STATUS = 0   
BEGIN
	--Get the ID of Another Ledger ID from the bunch of transaction.
	
	SELECT @Date=SalesReturn_Date, @VoucherNo=Voucher_No  FROM Inv.tblSalesReturnMaster WHERE SalesReturnID=@RowID
  --Now Log from Details
  
--sales Return Ledger
INSERT INTO @LedgerTransact SELECT @Date,mst.SalesLedgerID,
mst.Net_Amount,'SLS_RTN',@VoucherNo,@RowID FROM Inv.tblSalesReturnMaster mst WHERE mst.SalesReturnID=@RowID;



   

	FETCH NEXT FROM db_cursor INTO @RowID   
END   

CLOSE db_cursor   
DEALLOCATE db_cursor




Select a.Date as LedgerDate,b.EngName Account, a.LedgerID,a.Amount,a.VoucherType,a.VoucherNumber,a.RowID from @LedgerTransact a,Acc.tblLedger b WHERE a.LedgerID=b.LedgerID order by a.Date
END
EXEC sp_xml_removedocument @docHandle 