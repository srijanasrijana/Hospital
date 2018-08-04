

/****** Object:  StoredProcedure [Acc].[xmlCommonInsert]    Script Date: 07/13/2017 11:25:58 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





  CREATE PROC [Acc].[xmlCommonInsert](@common XML,@rowID INT,@voucherType NVARCHAR(20),@Return NVARCHAR(20) OUTPUT)
  AS  
  BEGIN
    --variable declaration
      SET @Return ='FAILURE';
      DECLARE @transID INT=0;
      DECLARE @docHandle INT;

 --prepare xml document
 EXEC sp_xml_preparedocument @docHandle OUTPUT, @common;
 
 --temporary table for storing acc.transact data
     DECLARE @tempTransact TABLE (
           id INT IDENTITY(1,1),
           LedgerID INT,LedgerName NVARCHAR(50),DebitAmount DECIMAL(19,5),
           CreditAmount DECIMAL(19,5),
           VoucherType NVARCHAR(50),
           TransactDate DATE,
           ProjectID INT)

  BEGIN TRANSACTION tranCommon
  BEGIN TRY

    --read xml data for acc.transaction
    INSERT INTO @tempTransact (LedgerID,LedgerName,DebitAmount,CreditAmount,TransactDate,ProjectID)
    SELECT LedgerID,LedgerName,DebitAmount,CreditAmount,TransactDate,ProjectID FROM OPENXML(@docHandle,'/COMMON/LEDGERDETAIL/DETAIL',2)
    WITH(LedgerID INT,LedgerName NVARCHAR(50),DebitAmount DECIMAL(19,5),CreditAmount DECIMAL(19,5),TransactDate DATE,ProjectID INT)

   DECLARE @CAccClassID TABLE
   (
	 AccClassID INT 
   );
   DECLARE @tempLedgerID INT=0
   DECLARE @ID INT=0
   DECLARE @TransClassID INT=0

 -- Write Accounting class ids into temporary table from xml
   INSERT INTO @CAccClassID(AccClassID)
    SELECT
        IdNode.value('(.)[1]', 'int')
    FROM
        @common.nodes('/COMMON/AccClassIDSettings/AccClassID') AS IdTbl(IdNode)


   --cursor for looping through all transaction ledger data
    DECLARE db_cursor CURSOR FOR select id FROM @tempTransact 
    OPEN db_cursor FETCH NEXT FROM db_cursor INTO @ID
   
    WHILE @@FETCH_STATUS=0
    BEGIN

      SELECT @tempLedgerID=LedgerID FROM @tempTransact WHERE id=@ID
      IF(@tempLedgerID=0)--if ledgerid is unknown
      SET @tempLedgerID=(SELECT LedgerID FROM Acc.tblLedger WHERE EngName=(SELECT LedgerName FROM @tempTransact WHERE id=@ID))
      
      --insert transaction ledger data into base table
       INSERT INTO ACC.tblTransaction(LedgerID,Debit_Amount,Credit_Amount,VoucherType,RowID,TransactDate,ProjectID) 
       SELECT @tempLedgerID,DebitAmount,CreditAmount,@voucherType,@rowID,TransactDate,ProjectID FROM @tempTransact WHERE id=@ID
      
       SET @transID= SCOPE_IDENTITY();
      
       --cursor for looping through all transaction class ids
       DECLARE c_cursor CURSOR FOR SELECT AccClassID FROM @CAccClassID
       OPEN c_cursor FETCH NEXT FROM c_cursor INTO @TransClassID
      
       WHILE @@FETCH_STATUS=0
       BEGIN
           --store transaction class IDS into base table
           INSERT INTO ACC.tblTransactionClass(TransactionID,AccClassID,RowID,VoucherType) VALUES(@transID,@TransClassID,@rowID,@voucherType)
           FETCH NEXT FROM c_cursor into @TransClassID
       END
      
       CLOSE c_cursor
       DEALLOCATE c_cursor
       FETCH NEXT FROM db_cursor INTO @ID

    END
    
    CLOSE db_cursor
    DEALLOCATE db_cursor

    --store auditlog
       INSERT INTO SYSTEM.tblAuditLog(ComputerName,UserName,Voucher_Type,[Action],[Description],Rowid,MAC_Address,IP_Address,VoucherDate)
       SELECT ComputerName,UserName,@voucherType,[Action],[Description],@rowID,MAC_Address,IP_Address,VoucherDate from OPENXML(@docHandle,'/COMMON/AUDITDETAIL',2)
       WITH(ComputerName NVARCHAR(50),UserName NVARCHAR(50),[action] NVARCHAR(50),[Description] NVARCHAR(2000),MAC_Address NVARCHAR(50),IP_Address NVARCHAR(50),VoucherDate DATETIME)
      
       INSERT INTO System.tblRecurringVoucher(VoucherID,VoucherType,[Description],RecurringType,Unit1,Unit2,[Date])
       SELECT @rowID,@voucherType,[Description],RecurringType,Unit1,Unit2,[Date] 
       FROM OPENXML(@docHandle,'/COMMON/RECURRINGVOUCHER/DETAIL',2)
       WITH([Description] nvarchar(500),RecurringType nvarchar(20),Unit1 nvarchar(50),Unit2 nvarchar(50),[Date] date)
       
       SET @Return='SUCCESS'
      COMMIT TRANSACTION tranCommon 
  END TRY

  BEGIN CATCH
      SET @Return='FAILURE'
      ROLLBACK TRANSACTION tranCommon

	  DECLARE @ErrorMessage NVARCHAR(4000);
    DECLARE @ErrorSeverity INT;
    DECLARE @ErrorState INT;
    SELECT @ErrorMessage = ERROR_MESSAGE(),
        @ErrorSeverity = ERROR_SEVERITY(),
        @ErrorState = ERROR_STATE();
    RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState);
  END CATCH

END







GO


