
/****** Object:  UserDefinedFunction [Acc].[fnGetLedgerTransaction]    Script Date: 3/9/2017 4:28:58 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE FUNCTION [Acc].[fnGetLedgerTransaction](@LedgerID INT=NULL,@FromDate DATETIME=NULL,
@ToDate DATETIME = NULL,@AccountClassIDsSettings  xml,@ProjectIDsSettings xml) 
RETURNS @tblTransact TABLE
(
	LedgerDate Date NULL,
	Account NVARCHAR(50) NULL,
	LedgerID INT NULL,
	Debit Decimal(19,5) NULL,
	Credit Decimal(19,5) NULL,
	VoucherType NVARCHAR(50) NULL,
	VoucherNumber NVARCHAR(50) NULL,
	RowID INT NULL,
	NepLedgerDate nvarchar(20) Null,
	GroupID int

)
AS
BEGIN

DECLARE @docHandle int;
DECLARE @docHandle1 int;
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
Debit Decimal(19,5),
Credit Decimal(19,5),
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

       if(@FromDate is not null)
		  if(@ToDate is not null)
            INSERT INTO @VchNRow SELECT DISTINCT VoucherType,RowID FROM Acc.tblTransaction WHERE ledgerID =@LedgerID AND TransactionID IN(SELECT TransactionID FROM Acc.tblTransactionClass WHERE AccClassID IN (SELECT AccClassID FROM @AccClassID) and ProjectID in (select ProjectID from @ProjectID)) and acc.tblTransaction.TransactDate between @FromDate and @ToDate
		  else
		    INSERT INTO @VchNRow SELECT DISTINCT VoucherType,RowID FROM Acc.tblTransaction WHERE ledgerID =@LedgerID AND TransactionID IN(SELECT TransactionID FROM Acc.tblTransactionClass WHERE AccClassID IN (SELECT AccClassID FROM @AccClassID) and ProjectID in (select ProjectID from @ProjectID)) and acc.tblTransaction.TransactDate >= @FromDate;
      else
	  	if(@ToDate is not null)
		  INSERT INTO @VchNRow SELECT DISTINCT VoucherType,RowID FROM Acc.tblTransaction WHERE ledgerID =@LedgerID AND TransactionID IN(SELECT TransactionID FROM Acc.tblTransactionClass WHERE AccClassID IN (SELECT AccClassID FROM @AccClassID) and ProjectID in (select ProjectID from @ProjectID)) and acc.tblTransaction.TransactDate<= @ToDate
	    else
		  INSERT INTO @VchNRow SELECT DISTINCT VoucherType,RowID FROM Acc.tblTransaction WHERE ledgerID =@LedgerID AND TransactionID IN(SELECT TransactionID FROM Acc.tblTransactionClass WHERE AccClassID IN (SELECT AccClassID FROM @AccClassID) and ProjectID in (select ProjectID from @ProjectID))




DECLARE @RowID INT;
DECLARE @AnotherLedgerID INT;
Declare @LedgerAmount decimal(19,5)=0;
Declare @ChkAmount decimal(19,5)=0;
DECLARE @TempAmount decimal(19,5)=0;
 Declare @type nvarchar(20)='';
 Declare @tempLedgerID int;


----FOR PURCHASE-----

DECLARE db_cursor CURSOR FOR SELECT RowID FROM @VchNRow WHERE VoucherType='PURCH'

OPEN db_cursor FETCH NEXT FROM db_cursor INTO @RowID  
set @ChkAmount=0;
WHILE @@FETCH_STATUS = 0   
BEGIN

   select @type=case when Debit_Amount>0 and Credit_Amount<=0 then 'DEBIT' 
                     when Credit_Amount>0 and Debit_Amount<=0 then 'CREDIT' 
					 else '' end,
					 @LedgerAmount=case when Debit_Amount>Credit_Amount then Debit_Amount else Credit_Amount end
					  from acc.tblTransaction a WHERE VoucherType='PURCH' And RowID=@RowID AND LedgerID = @LedgerID;
	if(@type='DEBIT' OR @type='CREDIT' )
	begin
	if @type='DEBIT'
	begin
	DECLARE temp_cursor CURSOR FOR SELECT LedgerID from acc.tblTransaction WHERE VoucherType='PURCH' And RowID=@RowID AND Credit_Amount>0;
	end
	else if @type ='CREDIT'                                                                       
	BEGIN
		DECLARE temp_cursor CURSOR FOR SELECT LedgerID from acc.tblTransaction WHERE VoucherType='PURCH' And RowID=@RowID AND Debit_Amount>0;
	END

	OPEN temp_cursor FETCH NEXT FROM temp_cursor INTO @tempLedgerID
	WHILE @@FETCH_STATUS = 0   
    BEGIN
	SELECT @TempAmount= ISNULL( CASE WHEN Debit_Amount>0 THEN Debit_Amount ELSE Credit_Amount END,0) FROM Acc.tblTransaction  WHERE VoucherType='PURCH' And RowID=@RowID AND LedgerID = @tempLedgerID;
	IF(@ChkAmount+@TempAmount<=@LedgerAmount)
	BEGIN
			INSERT INTO @LedgerTransact SELECT a.TransactDate,@tempLedgerID,a.Debit_Amount, a.Credit_Amount,a.VoucherType,b.Voucher_No,a.RowID from acc.tblTransaction a,Inv.tblPurchaseInvoiceMaster b WHERE a.VoucherType='PURCH' And a.RowID=@RowID AND a.LedgerID =@tempLedgerID AND a.RowID = b.PurchaseInvoiceID;
			set @ChkAmount=@ChkAmount+@TempAmount;
			set @TempAmount=0;
			if(@ChkAmount=@LedgerAmount)
	        BREAK;
	END

	ELSE IF(@ChkAmount+@TempAmount>@LedgerAmount)
	BEGIN
	INSERT INTO @LedgerTransact SELECT a.TransactDate,@tempLedgerID,case @type when 'CREDIT' then (@LedgerAmount-@ChkAmount) else 0 end, case @type when 'DEBIT' then (@LedgerAmount-@ChkAmount) else 0 end,a.VoucherType,b.Voucher_No,a.RowID from acc.tblTransaction a,Inv.tblPurchaseInvoiceMaster b WHERE a.VoucherType='PURCH' And a.RowID=@RowID AND a.LedgerID =@tempLedgerID AND a.RowID = b.PurchaseInvoiceID;
			set @ChkAmount=@ChkAmount+@TempAmount;
			set @TempAmount=0;
	        BREAK;
	END
	FETCH NEXT FROM temp_cursor INTO @tempLedgerID
	END
	CLOSE temp_cursor   
    DEALLOCATE temp_cursor
	END
	FETCH NEXT FROM db_cursor INTO @RowID   
END   

CLOSE db_cursor   
DEALLOCATE db_cursor


----FOR PURCHASE RETURN-----


DECLARE db_cursor CURSOR FOR SELECT RowID FROM @VchNRow WHERE VoucherType='PURCH_RTN'
OPEN db_cursor FETCH NEXT FROM db_cursor INTO @RowID  

WHILE @@FETCH_STATUS = 0   
BEGIN
set @TempAmount=0;
set @ChkAmount=0;
set @LedgerAmount=0;
set @type='';
   select @type=case when Debit_Amount>0 and Credit_Amount<=0 then 'DEBIT' 
                     when Credit_Amount>0 and Debit_Amount<=0 then 'CREDIT' 
					 else '' end,
					 @LedgerAmount=case when Debit_Amount>Credit_Amount then Debit_Amount else Credit_Amount end
					  from acc.tblTransaction a WHERE VoucherType='PURCH_RTN' And RowID=@RowID AND LedgerID = @LedgerID;
if(@type='DEBIT' OR @type='CREDIT' )
	begin
	if @type='DEBIT'
	begin
	DECLARE temp_cursor CURSOR FOR SELECT LedgerID from acc.tblTransaction WHERE VoucherType='PURCH_RTN' And RowID=@RowID AND Credit_Amount>0;
	end
	else if @type ='CREDIT'                                                                       
	BEGIN
		DECLARE temp_cursor CURSOR FOR SELECT LedgerID from acc.tblTransaction WHERE VoucherType='PURCH_RTN' And RowID=@RowID AND Debit_Amount>0;
	END

	OPEN temp_cursor FETCH NEXT FROM temp_cursor INTO @tempLedgerID
	WHILE @@FETCH_STATUS = 0   
    BEGIN
	SELECT @TempAmount= ISNULL( CASE WHEN Debit_Amount>0 THEN Debit_Amount ELSE Credit_Amount END,0) FROM Acc.tblTransaction  WHERE VoucherType='PURCH_RTN' And RowID=@RowID AND LedgerID = @tempLedgerID;
	IF(@ChkAmount+@TempAmount<=@LedgerAmount)
	BEGIN
			INSERT INTO @LedgerTransact SELECT a.TransactDate,@tempLedgerID,a.Debit_Amount, a.Credit_Amount,a.VoucherType,b.Voucher_No,a.RowID from acc.tblTransaction a,Inv.tblPurchaseReturnMaster b WHERE a.VoucherType='PURCH_RTN' And a.RowID=@RowID AND a.LedgerID =@tempLedgerID AND a.RowID = b.PurchaseReturnID;
			set @ChkAmount=@ChkAmount+@TempAmount;
			set @TempAmount=0;
			if(@ChkAmount=@LedgerAmount)
	        BREAK;
	END

	ELSE IF(@ChkAmount+@TempAmount>@LedgerAmount)
	BEGIN
	INSERT INTO @LedgerTransact SELECT a.TransactDate,@tempLedgerID,case @type when 'CREDIT' then (@LedgerAmount-@ChkAmount) else 0 end, case @type when 'DEBIT' then (@LedgerAmount-@ChkAmount) else 0 end,a.VoucherType,b.Voucher_No,a.RowID from acc.tblTransaction a,Inv.tblPurchaseReturnMaster b WHERE a.VoucherType='PURCH' And a.RowID=@RowID AND a.LedgerID =@tempLedgerID AND a.RowID = b.PurchaseReturnID;
			set @ChkAmount=@ChkAmount+@TempAmount;
			set @TempAmount=0;
	        BREAK;
	END
	FETCH NEXT FROM temp_cursor INTO @tempLedgerID
	END
	CLOSE temp_cursor   
    DEALLOCATE temp_cursor
	END
	FETCH NEXT FROM db_cursor INTO @RowID  
END   

CLOSE db_cursor   
DEALLOCATE db_cursor

--------------FOR SALES INVOICE--------------

DECLARE db_cursor CURSOR FOR SELECT RowID FROM @VchNRow WHERE VoucherType='SALES'

OPEN db_cursor FETCH NEXT FROM db_cursor INTO @RowID  


WHILE @@FETCH_STATUS = 0   
BEGIN
set @TempAmount=0;
set @ChkAmount=0;
set @LedgerAmount=0;
set @type='';
 select @type=case when Debit_Amount>0 and Credit_Amount<=0 then 'DEBIT' 
                     when Credit_Amount>0 and Debit_Amount<=0 then 'CREDIT' 
					 else '' end,
					 @LedgerAmount=case when Debit_Amount>Credit_Amount then Debit_Amount else Credit_Amount end
					  from acc.tblTransaction a WHERE VoucherType='SALES' And RowID=@RowID AND LedgerID = @LedgerID;
if(@type='DEBIT' OR @type='CREDIT' )
	begin
	if @type='DEBIT'
	begin
	DECLARE temp_cursor CURSOR FOR SELECT LedgerID from acc.tblTransaction WHERE VoucherType='SALES' And RowID=@RowID AND Credit_Amount>0;
	end
	else if @type ='CREDIT'                                                                       
	BEGIN
		DECLARE temp_cursor CURSOR FOR SELECT LedgerID from acc.tblTransaction WHERE VoucherType='SALES' And RowID=@RowID AND Debit_Amount>0;
	END

	OPEN temp_cursor FETCH NEXT FROM temp_cursor INTO @tempLedgerID
	WHILE @@FETCH_STATUS = 0   
    BEGIN
	SELECT @TempAmount= ISNULL( CASE WHEN Debit_Amount>0 THEN Debit_Amount ELSE Credit_Amount END,0) FROM Acc.tblTransaction  WHERE VoucherType='SALES' And RowID=@RowID AND LedgerID = @tempLedgerID;
	IF(@ChkAmount+@TempAmount<=@LedgerAmount)
	BEGIN
			INSERT INTO @LedgerTransact SELECT a.TransactDate,@tempLedgerID,a.Debit_Amount, a.Credit_Amount,a.VoucherType,b.Voucher_No,a.RowID from acc.tblTransaction a,Inv.tblSalesInvoiceMaster b WHERE a.VoucherType='SALES' And a.RowID=@RowID AND a.LedgerID =@tempLedgerID AND a.RowID = b.SalesInvoiceID;
			set @ChkAmount=@ChkAmount+@TempAmount;
			set @TempAmount=0;
			if(@ChkAmount=@LedgerAmount)
	        BREAK;
	END

	ELSE IF(@ChkAmount+@TempAmount>@LedgerAmount)
	BEGIN
	INSERT INTO @LedgerTransact SELECT a.TransactDate,@tempLedgerID,case @type when 'CREDIT' then (@LedgerAmount-@ChkAmount) else 0 end, case @type when 'DEBIT' then (@LedgerAmount-@ChkAmount) else 0 end,a.VoucherType,b.Voucher_No,a.RowID from acc.tblTransaction a,Inv.tblSalesInvoiceMaster b WHERE a.VoucherType='SALES' And a.RowID=@RowID AND a.LedgerID =@tempLedgerID AND a.RowID = b.SalesInvoiceID;
			set @ChkAmount=@ChkAmount+@TempAmount;
			set @TempAmount=0;
	        BREAK;
	END
	FETCH NEXT FROM temp_cursor INTO @tempLedgerID
	END
	CLOSE temp_cursor   
    DEALLOCATE temp_cursor
	END
	FETCH NEXT FROM db_cursor INTO @RowID  
	
END   

CLOSE db_cursor   
DEALLOCATE db_cursor

-----------------------FOR SALES RETURN
DECLARE db_cursor CURSOR FOR SELECT RowID FROM @VchNRow WHERE VoucherType='SLS_RTN'

OPEN db_cursor FETCH NEXT FROM db_cursor INTO @RowID  

WHILE @@FETCH_STATUS = 0   
BEGIN

	set @TempAmount=0;
set @ChkAmount=0;
set @LedgerAmount=0;
set @type='';
 select @type=case when Debit_Amount>0 and Credit_Amount<=0 then 'DEBIT' 
                     when Credit_Amount>0 and Debit_Amount<=0 then 'CREDIT' 
					 else '' end,
					 @LedgerAmount=case when Debit_Amount>Credit_Amount then Debit_Amount else Credit_Amount end
					  from acc.tblTransaction a WHERE VoucherType='SLS_RTN' And RowID=@RowID AND LedgerID = @LedgerID;
if(@type='DEBIT' OR @type='CREDIT' )
	begin
	if @type='DEBIT'
	begin
	DECLARE temp_cursor CURSOR FOR SELECT LedgerID from acc.tblTransaction WHERE VoucherType='SLS_RTN' And RowID=@RowID AND Credit_Amount>0;
	end
	else if @type ='CREDIT'                                                                       
	BEGIN
		DECLARE temp_cursor CURSOR FOR SELECT LedgerID from acc.tblTransaction WHERE VoucherType='SLS_RTN' And RowID=@RowID AND Debit_Amount>0;
	END

	OPEN temp_cursor FETCH NEXT FROM temp_cursor INTO @tempLedgerID
	WHILE @@FETCH_STATUS = 0   
    BEGIN
	SELECT @TempAmount= ISNULL( CASE WHEN Debit_Amount>0 THEN Debit_Amount ELSE Credit_Amount END,0) FROM Acc.tblTransaction  WHERE VoucherType='SLS_RTN' And RowID=@RowID AND LedgerID = @tempLedgerID;
	IF(@ChkAmount+@TempAmount<=@LedgerAmount)
	BEGIN
			INSERT INTO @LedgerTransact SELECT a.TransactDate,@tempLedgerID,a.Debit_Amount, a.Credit_Amount,a.VoucherType,b.Voucher_No,a.RowID from acc.tblTransaction a,Inv.tblSalesReturnMaster b WHERE a.VoucherType='SLS_RTN' And a.RowID=@RowID AND a.LedgerID =@tempLedgerID AND a.RowID = b.SalesReturnID;
			set @ChkAmount=@ChkAmount+@TempAmount;
			set @TempAmount=0;
			if(@ChkAmount=@LedgerAmount)
	        BREAK;
	END

	ELSE IF(@ChkAmount+@TempAmount>@LedgerAmount)
	BEGIN
	INSERT INTO @LedgerTransact SELECT a.TransactDate,@tempLedgerID,case @type when 'CREDIT' then (@LedgerAmount-@ChkAmount) else 0 end, case @type when 'DEBIT' then (@LedgerAmount-@ChkAmount) else 0 end,a.VoucherType,b.Voucher_No,a.RowID from acc.tblTransaction a,Inv.tblSalesReturnMaster b WHERE a.VoucherType='SLS_RTN' And a.RowID=@RowID AND a.LedgerID =@tempLedgerID AND a.RowID = b.SalesReturnID;
			set @ChkAmount=@ChkAmount+@TempAmount;
			set @TempAmount=0;
	        BREAK;
	END
	FETCH NEXT FROM temp_cursor INTO @tempLedgerID
	END
	CLOSE temp_cursor   
    DEALLOCATE temp_cursor
	END
	FETCH NEXT FROM db_cursor INTO @RowID  
END   

CLOSE db_cursor   
DEALLOCATE db_cursor

----FOR Journal----

DECLARE db_cursor CURSOR FOR SELECT RowID FROM @VchNRow WHERE VoucherType='JRNL'

OPEN db_cursor FETCH NEXT FROM db_cursor INTO @RowID  

WHILE @@FETCH_STATUS = 0   
BEGIN

	set @TempAmount=0;
set @ChkAmount=0;
set @LedgerAmount=0;
set @type='';
 select @type=case when Debit_Amount>0 and Credit_Amount<=0 then 'DEBIT' 
                     when Credit_Amount>0 and Debit_Amount<=0 then 'CREDIT' 
					 else '' end,
					 @LedgerAmount=case when Debit_Amount>Credit_Amount then Debit_Amount else Credit_Amount end
					  from acc.tblTransaction a WHERE VoucherType='JRNL' And RowID=@RowID AND LedgerID = @LedgerID;
if(@type='DEBIT' OR @type='CREDIT' )
	begin
	if @type='DEBIT'
	begin
	DECLARE temp_cursor CURSOR FOR SELECT LedgerID from acc.tblTransaction WHERE VoucherType='JRNL' And RowID=@RowID AND Credit_Amount>0;
	end
	else if @type ='CREDIT'                                                                       
	BEGIN
		DECLARE temp_cursor CURSOR FOR SELECT LedgerID from acc.tblTransaction WHERE VoucherType='JRNL' And RowID=@RowID AND Debit_Amount>0;
	END

	OPEN temp_cursor FETCH NEXT FROM temp_cursor INTO @tempLedgerID
	WHILE @@FETCH_STATUS = 0   
    BEGIN
	SELECT @TempAmount= ISNULL( CASE WHEN Debit_Amount>0 THEN Debit_Amount ELSE Credit_Amount END,0) FROM Acc.tblTransaction  WHERE VoucherType='JRNL' And RowID=@RowID AND LedgerID = @tempLedgerID;
	IF(@ChkAmount+@TempAmount<=@LedgerAmount)
	BEGIN
			INSERT INTO @LedgerTransact SELECT a.TransactDate,@tempLedgerID,a.Debit_Amount, a.Credit_Amount,a.VoucherType,b.Voucher_No,a.RowID from acc.tblTransaction a,acc.tblJournalMaster b WHERE a.VoucherType='JRNL' And a.RowID=@RowID AND a.LedgerID =@tempLedgerID AND a.RowID = b.JournalID;
			set @ChkAmount=@ChkAmount+@TempAmount;
			set @TempAmount=0;
			if(@ChkAmount=@LedgerAmount)
	        BREAK;
	END

	ELSE IF(@ChkAmount+@TempAmount>@LedgerAmount)
	BEGIN
	INSERT INTO @LedgerTransact SELECT a.TransactDate,@tempLedgerID,case @type when 'CREDIT' then (@LedgerAmount-@ChkAmount) else 0 end, case @type when 'DEBIT' then (@LedgerAmount-@ChkAmount) else 0 end,a.VoucherType,b.Voucher_No,a.RowID from acc.tblTransaction a,acc.tblJournalMaster b WHERE a.VoucherType='JRNL' And a.RowID=@RowID AND a.LedgerID =@tempLedgerID AND a.RowID = b.JournalID;
			set @ChkAmount=@ChkAmount+@TempAmount;
			set @TempAmount=0;
	        BREAK;
	END
	FETCH NEXT FROM temp_cursor INTO @tempLedgerID
	END
	CLOSE temp_cursor   
    DEALLOCATE temp_cursor
	END
	FETCH NEXT FROM db_cursor INTO @RowID  
END   

CLOSE db_cursor   
DEALLOCATE db_cursor




----------------FOR CONTRA---------------

DECLARE db_cursor CURSOR FOR SELECT RowID FROM @VchNRow WHERE VoucherType='CNTR'

OPEN db_cursor FETCH NEXT FROM db_cursor INTO @RowID  

WHILE @@FETCH_STATUS = 0   
BEGIN

		set @TempAmount=0;
set @ChkAmount=0;
set @LedgerAmount=0;
set @type='';
 select @type=case when Debit_Amount>0 and Credit_Amount<=0 then 'DEBIT' 
                     when Credit_Amount>0 and Debit_Amount<=0 then 'CREDIT' 
					 else '' end,
					 @LedgerAmount=case when Debit_Amount>Credit_Amount then Debit_Amount else Credit_Amount end
					  from acc.tblTransaction a WHERE VoucherType='CNTR' And RowID=@RowID AND LedgerID = @LedgerID;
if(@type='DEBIT' OR @type='CREDIT' )
	begin
	if @type='DEBIT'
	begin
	DECLARE temp_cursor CURSOR FOR SELECT LedgerID from acc.tblTransaction WHERE VoucherType='CNTR' And RowID=@RowID AND Credit_Amount>0;
	end
	else if @type ='CREDIT'                                                                       
	BEGIN
		DECLARE temp_cursor CURSOR FOR SELECT LedgerID from acc.tblTransaction WHERE VoucherType='CNTR' And RowID=@RowID AND Debit_Amount>0;
	END

	OPEN temp_cursor FETCH NEXT FROM temp_cursor INTO @tempLedgerID
	WHILE @@FETCH_STATUS = 0   
    BEGIN
	SELECT @TempAmount= ISNULL( CASE WHEN Debit_Amount>0 THEN Debit_Amount ELSE Credit_Amount END,0) FROM Acc.tblTransaction  WHERE VoucherType='CNTR' And RowID=@RowID AND LedgerID = @tempLedgerID;
	IF(@ChkAmount+@TempAmount<=@LedgerAmount)
	BEGIN
			INSERT INTO @LedgerTransact SELECT a.TransactDate,@tempLedgerID,a.Debit_Amount, a.Credit_Amount,a.VoucherType,b.Voucher_No,a.RowID from acc.tblTransaction a,acc.tblContraMaster b WHERE a.VoucherType='CNTR' And a.RowID=@RowID AND a.LedgerID =@tempLedgerID AND a.RowID = b.ContraID;
			set @ChkAmount=@ChkAmount+@TempAmount;
			set @TempAmount=0;
			if(@ChkAmount=@LedgerAmount)
	        BREAK;
	END

	ELSE IF(@ChkAmount+@TempAmount>@LedgerAmount)
	BEGIN
	INSERT INTO @LedgerTransact SELECT a.TransactDate,@tempLedgerID,case @type when 'CREDIT' then (@LedgerAmount-@ChkAmount) else 0 end, case @type when 'DEBIT' then (@LedgerAmount-@ChkAmount) else 0 end,a.VoucherType,b.Voucher_No,a.RowID from acc.tblTransaction a,acc.tblContraMaster b WHERE a.VoucherType='CNTR' And a.RowID=@RowID AND a.LedgerID =@tempLedgerID AND a.RowID = b.ContraID;
			set @ChkAmount=@ChkAmount+@TempAmount;
			set @TempAmount=0;
	        BREAK;
	END
	FETCH NEXT FROM temp_cursor INTO @tempLedgerID
	END
	CLOSE temp_cursor   
    DEALLOCATE temp_cursor
	END
	FETCH NEXT FROM db_cursor INTO @RowID  
END   

CLOSE db_cursor   
DEALLOCATE db_cursor

----FOR Cash Receipt----

DECLARE db_cursor CURSOR FOR SELECT RowID FROM @VchNRow WHERE VoucherType='CASH_RCPT'

OPEN db_cursor FETCH NEXT FROM db_cursor INTO @RowID  	
	
WHILE @@FETCH_STATUS = 0   
BEGIN

			set @TempAmount=0;
set @ChkAmount=0;
set @LedgerAmount=0;
set @type='';
 select @type=case when Debit_Amount>0 and Credit_Amount<=0 then 'DEBIT' 
                     when Credit_Amount>0 and Debit_Amount<=0 then 'CREDIT' 
					 else '' end,
					 @LedgerAmount=case when Debit_Amount>Credit_Amount then Debit_Amount else Credit_Amount end
					  from acc.tblTransaction a WHERE VoucherType='CASH_RCPT' And RowID=@RowID AND LedgerID = @LedgerID;
if(@type='DEBIT' OR @type='CREDIT' )
	begin
	if @type='DEBIT'
	begin
	DECLARE temp_cursor CURSOR FOR SELECT LedgerID from acc.tblTransaction WHERE VoucherType='CASH_RCPT' And RowID=@RowID AND Credit_Amount>0;
	end
	else if @type ='CREDIT'                                                                       
	BEGIN
		DECLARE temp_cursor CURSOR FOR SELECT LedgerID from acc.tblTransaction WHERE VoucherType='CASH_RCPT' And RowID=@RowID AND Debit_Amount>0;
	END

	OPEN temp_cursor FETCH NEXT FROM temp_cursor INTO @tempLedgerID
	WHILE @@FETCH_STATUS = 0   
    BEGIN
	SELECT @TempAmount= ISNULL( CASE WHEN Debit_Amount>0 THEN Debit_Amount ELSE Credit_Amount END,0) FROM Acc.tblTransaction  WHERE VoucherType='CASH_RCPT' And RowID=@RowID AND LedgerID = @tempLedgerID;
	IF(@ChkAmount+@TempAmount<=@LedgerAmount)
	BEGIN
			INSERT INTO @LedgerTransact SELECT a.TransactDate,@tempLedgerID,a.Debit_Amount, a.Credit_Amount,a.VoucherType,b.Voucher_No,a.RowID from acc.tblTransaction a,acc.tblCashReceiptMaster b WHERE a.VoucherType='CASH_RCPT' And a.RowID=@RowID AND a.LedgerID =@tempLedgerID AND a.RowID = b.CashReceiptID;
			set @ChkAmount=@ChkAmount+@TempAmount;
			set @TempAmount=0;
			if(@ChkAmount=@LedgerAmount)
	        BREAK;
	END

	ELSE IF(@ChkAmount+@TempAmount>@LedgerAmount)
	BEGIN
	INSERT INTO @LedgerTransact SELECT a.TransactDate,@tempLedgerID,case @type when 'CREDIT' then (@LedgerAmount-@ChkAmount) else 0 end, case @type when 'DEBIT' then (@LedgerAmount-@ChkAmount) else 0 end,a.VoucherType,b.Voucher_No,a.RowID from acc.tblTransaction a,acc.tblCashReceiptMaster b WHERE a.VoucherType='CASH_RCPT' And a.RowID=@RowID AND a.LedgerID =@tempLedgerID AND a.RowID = b.CashReceiptID;
			set @ChkAmount=@ChkAmount+@TempAmount;
			set @TempAmount=0;
	        BREAK;
	END
	FETCH NEXT FROM temp_cursor INTO @tempLedgerID
	END
	CLOSE temp_cursor   
    DEALLOCATE temp_cursor
	END
	FETCH NEXT FROM db_cursor INTO @RowID  
END   

CLOSE db_cursor   
DEALLOCATE db_cursor

--------------FOR CASH PAYMENT-------

DECLARE db_cursor CURSOR FOR SELECT RowID FROM @VchNRow WHERE VoucherType='CASH_PMNT'


OPEN db_cursor FETCH NEXT FROM db_cursor INTO @RowID  

	
	
	
WHILE @@FETCH_STATUS = 0   
BEGIN

			set @TempAmount=0;
set @ChkAmount=0;
set @LedgerAmount=0;
set @type='';
 select @type=case when Debit_Amount>0 and Credit_Amount<=0 then 'DEBIT' 
                     when Credit_Amount>0 and Debit_Amount<=0 then 'CREDIT' 
					 else '' end,
					 @LedgerAmount=case when Debit_Amount>Credit_Amount then Debit_Amount else Credit_Amount end
					  from acc.tblTransaction a WHERE VoucherType='CASH_PMNT' And RowID=@RowID AND LedgerID = @LedgerID;
if(@type='DEBIT' OR @type='CREDIT' )
	begin
	if @type='DEBIT'
	begin
	DECLARE temp_cursor CURSOR FOR SELECT LedgerID from acc.tblTransaction WHERE VoucherType='CASH_PMNT' And RowID=@RowID AND Credit_Amount>0;
	end
	else if @type ='CREDIT'                                                                       
	BEGIN
		DECLARE temp_cursor CURSOR FOR SELECT LedgerID from acc.tblTransaction WHERE VoucherType='CASH_PMNT' And RowID=@RowID AND Debit_Amount>0;
	END

	OPEN temp_cursor FETCH NEXT FROM temp_cursor INTO @tempLedgerID
	WHILE @@FETCH_STATUS = 0   
    BEGIN
	SELECT @TempAmount= ISNULL( CASE WHEN Debit_Amount>0 THEN Debit_Amount ELSE Credit_Amount END,0) FROM Acc.tblTransaction  WHERE VoucherType='CASH_PMNT' And RowID=@RowID AND LedgerID = @tempLedgerID;
	IF(@ChkAmount+@TempAmount<=@LedgerAmount)
	BEGIN
			INSERT INTO @LedgerTransact SELECT a.TransactDate,@tempLedgerID,a.Debit_Amount, a.Credit_Amount,a.VoucherType,b.Voucher_No,a.RowID from acc.tblTransaction a,acc.tblCashPaymentMaster b WHERE a.VoucherType='CASH_PMNT' And a.RowID=@RowID AND a.LedgerID =@tempLedgerID AND a.RowID = b.CashPaymentID;
			set @ChkAmount=@ChkAmount+@TempAmount;
			set @TempAmount=0;
			if(@ChkAmount=@LedgerAmount)
	        BREAK;
	END

	ELSE IF(@ChkAmount+@TempAmount>@LedgerAmount)
	BEGIN
	INSERT INTO @LedgerTransact SELECT a.TransactDate,@tempLedgerID,case @type when 'CREDIT' then (@LedgerAmount-@ChkAmount) else 0 end, case @type when 'DEBIT' then (@LedgerAmount-@ChkAmount) else 0 end,a.VoucherType,b.Voucher_No,a.RowID from acc.tblTransaction a,acc.tblCashPaymentMaster b WHERE a.VoucherType='CASH_PMNT' And a.RowID=@RowID AND a.LedgerID =@tempLedgerID AND a.RowID = b.CashPaymentID;
			set @ChkAmount=@ChkAmount+@TempAmount;
			set @TempAmount=0;
	        BREAK;
	END
	FETCH NEXT FROM temp_cursor INTO @tempLedgerID
	END
	CLOSE temp_cursor   
    DEALLOCATE temp_cursor
	END
	FETCH NEXT FROM db_cursor INTO @RowID  
END   

CLOSE db_cursor   
DEALLOCATE db_cursor

-----------------------FOR BANK PAYMENT------------------------

DECLARE db_cursor CURSOR FOR SELECT RowID FROM @VchNRow WHERE VoucherType='BANK_PMNT'

OPEN db_cursor FETCH NEXT FROM db_cursor INTO @RowID  

	
WHILE @@FETCH_STATUS = 0   
BEGIN

	
			set @TempAmount=0;
set @ChkAmount=0;
set @LedgerAmount=0;
set @type='';
 select @type=case when Debit_Amount>0 and Credit_Amount<=0 then 'DEBIT' 
                     when Credit_Amount>0 and Debit_Amount<=0 then 'CREDIT' 
					 else '' end,
					 @LedgerAmount=case when Debit_Amount>Credit_Amount then Debit_Amount else Credit_Amount end
					  from acc.tblTransaction a WHERE VoucherType='BANK_PMNT' And RowID=@RowID AND LedgerID = @LedgerID;
if(@type='DEBIT' OR @type='CREDIT' )
	begin
	if @type='DEBIT'
	begin
	DECLARE temp_cursor CURSOR FOR SELECT LedgerID from acc.tblTransaction WHERE VoucherType='BANK_PMNT' And RowID=@RowID AND Credit_Amount>0;
	end
	else if @type ='CREDIT'                                                                       
	BEGIN
		DECLARE temp_cursor CURSOR FOR SELECT LedgerID from acc.tblTransaction WHERE VoucherType='BANK_PMNT' And RowID=@RowID AND Debit_Amount>0;
	END

	OPEN temp_cursor FETCH NEXT FROM temp_cursor INTO @tempLedgerID
	WHILE @@FETCH_STATUS = 0   
    BEGIN
	SELECT @TempAmount= ISNULL( CASE WHEN Debit_Amount>0 THEN Debit_Amount ELSE Credit_Amount END,0) FROM Acc.tblTransaction  WHERE VoucherType='BANK_PMNT' And RowID=@RowID AND LedgerID = @tempLedgerID;
	IF(@ChkAmount+@TempAmount<=@LedgerAmount)
	BEGIN
			INSERT INTO @LedgerTransact SELECT a.TransactDate,@tempLedgerID,a.Debit_Amount, a.Credit_Amount,a.VoucherType,b.Voucher_No,a.RowID from acc.tblTransaction a,acc.tblBankPaymentMaster b WHERE a.VoucherType='BANK_PMNT' And a.RowID=@RowID AND a.LedgerID =@tempLedgerID AND a.RowID = b.BankPaymentID;
			set @ChkAmount=@ChkAmount+@TempAmount;
			set @TempAmount=0;
			if(@ChkAmount=@LedgerAmount)
	        BREAK;
	END

	ELSE IF(@ChkAmount+@TempAmount>@LedgerAmount)
	BEGIN
	INSERT INTO @LedgerTransact SELECT a.TransactDate,@tempLedgerID,case @type when 'CREDIT' then (@LedgerAmount-@ChkAmount) else 0 end, case @type when 'DEBIT' then (@LedgerAmount-@ChkAmount) else 0 end,a.VoucherType,b.Voucher_No,a.RowID from acc.tblTransaction a,acc.tblBankPaymentMaster b WHERE a.VoucherType='BANK_PMNT' And a.RowID=@RowID AND a.LedgerID =@tempLedgerID AND a.RowID = b.BankPaymentID;
			set @ChkAmount=@ChkAmount+@TempAmount;
			set @TempAmount=0;
	        BREAK;
	END
	FETCH NEXT FROM temp_cursor INTO @tempLedgerID
	END
	CLOSE temp_cursor   
    DEALLOCATE temp_cursor
	END
	FETCH NEXT FROM db_cursor INTO @RowID  
END   

CLOSE db_cursor   
DEALLOCATE db_cursor

-------------------------------FOR BANK RECEIPT -----------------------------------------
DECLARE db_cursor CURSOR FOR SELECT RowID FROM @VchNRow WHERE VoucherType='BANK_RCPT'

OPEN db_cursor FETCH NEXT FROM db_cursor INTO @RowID  
	
WHILE @@FETCH_STATUS = 0   
BEGIN

			set @TempAmount=0;
set @ChkAmount=0;
set @LedgerAmount=0;
set @type='';
 select @type=case when Debit_Amount>0 and Credit_Amount<=0 then 'DEBIT' 
                     when Credit_Amount>0 and Debit_Amount<=0 then 'CREDIT' 
					 else '' end,
					 @LedgerAmount=case when Debit_Amount>Credit_Amount then Debit_Amount else Credit_Amount end
					  from acc.tblTransaction a WHERE VoucherType='BANK_RCPT' And RowID=@RowID AND LedgerID = @LedgerID;
if(@type='DEBIT' OR @type='CREDIT' )
	begin
	if @type='DEBIT'
	begin
	DECLARE temp_cursor CURSOR FOR SELECT LedgerID from acc.tblTransaction WHERE VoucherType='BANK_RCPT' And RowID=@RowID AND Credit_Amount>0;
	end
	else if @type ='CREDIT'                                                                       
	BEGIN
		DECLARE temp_cursor CURSOR FOR SELECT LedgerID from acc.tblTransaction WHERE VoucherType='BANK_RCPT' And RowID=@RowID AND Debit_Amount>0;
	END

	OPEN temp_cursor FETCH NEXT FROM temp_cursor INTO @tempLedgerID
	WHILE @@FETCH_STATUS = 0   
    BEGIN
	SELECT @TempAmount= ISNULL( CASE WHEN Debit_Amount>0 THEN Debit_Amount ELSE Credit_Amount END,0) FROM Acc.tblTransaction  WHERE VoucherType='BANK_RCPT' And RowID=@RowID AND LedgerID = @tempLedgerID;
	IF(@ChkAmount+@TempAmount<=@LedgerAmount)
	BEGIN
			INSERT INTO @LedgerTransact SELECT a.TransactDate,@tempLedgerID,a.Debit_Amount, a.Credit_Amount,a.VoucherType,b.Voucher_No,a.RowID from acc.tblTransaction a,acc.tblBankReceiptMaster b WHERE a.VoucherType='BANK_RCPT' And a.RowID=@RowID AND a.LedgerID =@tempLedgerID AND a.RowID = b.BankReceiptID;
			set @ChkAmount=@ChkAmount+@TempAmount;
			set @TempAmount=0;
			if(@ChkAmount=@LedgerAmount)
	        BREAK;
	END

	ELSE IF(@ChkAmount+@TempAmount>@LedgerAmount)
	BEGIN
	INSERT INTO @LedgerTransact SELECT a.TransactDate,@tempLedgerID,case @type when 'CREDIT' then (@LedgerAmount-@ChkAmount) else 0 end, case @type when 'DEBIT' then (@LedgerAmount-@ChkAmount) else 0 end,a.VoucherType,b.Voucher_No,a.RowID from acc.tblTransaction a,acc.tblBankReceiptMaster b WHERE a.VoucherType='BANK_RCPT' And a.RowID=@RowID AND a.LedgerID =@tempLedgerID AND a.RowID = b.BankReceiptID;
			set @ChkAmount=@ChkAmount+@TempAmount;
			set @TempAmount=0;
	        BREAK;
	END
	FETCH NEXT FROM temp_cursor INTO @tempLedgerID
	END
	CLOSE temp_cursor   
    DEALLOCATE temp_cursor
	END
	FETCH NEXT FROM db_cursor INTO @RowID  
END   

CLOSE db_cursor   
DEALLOCATE db_cursor

---------------------------


insert into @tblTransact
Select cast(a.Date as date) as LedgerDate,b.EngName Account, a.LedgerID,a.Credit as Debit,a.Debit as Credit,a.VoucherType,a.VoucherNumber,a.RowID,Date.fnEngtoNep(a.Date) as NepLedgerDate,b.GroupID  from @LedgerTransact a,Acc.tblLedger b WHERE a.LedgerID=b.LedgerID order by a.Date

END
RETURN;
--EXEC sp_xml_removedocument @docHandle 



end



GO


