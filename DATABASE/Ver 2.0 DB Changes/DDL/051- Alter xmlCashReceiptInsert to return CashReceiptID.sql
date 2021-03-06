--USE [BENT003]
--GO
/****** Object:  StoredProcedure [Acc].[xmlCashReceiptInsert]    Script Date: 10/19/2016 2:09:25 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [Acc].[xmlCashReceiptInsert]
 @cashreceipt  xml ,    -- It encludes cash receipt Master Info, cash receipt Detail Info And Accounting Class Info
  @returnId int = 0 output 
  AS
DECLARE @docHandle int, @CRID int;
EXEC sp_xml_preparedocument @docHandle OUTPUT, @cashreceipt;

DECLARE @CashReceiptTable TABLE
(
	CashReceiptID INT,
	SeriesID INT,
	LedgerID INT,
	Voucher_No NVARCHAR(50), 
	CashReceipt_Date DATETIME,
	Remarks NVARCHAR(200),
	ProjectID INT,
	Created_By NVARCHAR(50),
	Created_Date DATETIME,
	Modified_By NVARCHAR(50),
	Modified_Date DATETIME
);

BEGIN TRANSACTION

--It Inserts Cash Receipt Master Record From XML to Table
INSERT INTO [Acc].[tblCashReceiptMaster]( SeriesID, LedgerID, Voucher_No, CashReceipt_Date,Remarks,ProjectID,Created_By,Created_Date,Modified_By,Modified_Date,Field1,Field2,Field3,Field4,Field5 ) 
  SELECT  SeriesID, LedgerID, Voucher_No, CashReceipt_Date, Remarks, ProjectID, Created_By, Created_Date, Modified_By, Modified_Date ,Field1,Field2,Field3,Field4,Field5
  FROM Openxml( @docHandle, '/CASHRECEIPT/CASHRECEIPTMASTER', 2) WITH ( 
  SeriesID int, LedgerID int,Voucher_No nvarchar(30),   CashReceipt_Date datetime, Remarks nvarchar(200), ProjectID int, Created_By nvarchar(20), Created_Date datetime, Modified_By nvarchar(20), Modified_Date datetime,Field1 nvarchar(50),
  Field2 nvarchar(50),Field3 nvarchar(50),Field4 nvarchar(50),Field5 nvarchar(50) )
  
IF @@ERROR<>0 BEGIN ROLLBACK TRANSACTION RETURN -100 END

SET @CRID = SCOPE_IDENTITY()
SET @returnId = @CRID;
--Also Inserts Journal Master Record From XML to Temporary Table @JournalTable
INSERT INTO @CashReceiptTable(CashReceiptID, SeriesID, LedgerID, Voucher_No, CashReceipt_Date,Remarks,Created_By,Created_Date,Modified_By,Modified_Date,ProjectID ) 
SELECT @CRID AS CashReceiptID, SeriesID, LedgerID, Voucher_No, CashReceipt_Date, Remarks, Created_By, Created_Date, Modified_By, Modified_Date,ProjectID 
  FROM Openxml( @docHandle,'/CASHRECEIPT/CASHRECEIPTMASTER', 2) WITH (
  SeriesID int, LedgerID int, Voucher_No nvarchar(30), CashReceipt_Date datetime, Remarks nvarchar(200), Created_By nvarchar(20), Created_Date datetime, Modified_By nvarchar(20), Modified_Date datetime,ProjectID int )
  
DECLARE @CRDetails TABLE
(
CashReceiptID INT, 
LedgerID INT, 
Amount float, 
Remarks NVARCHAR(500),
VoucherType nvarchar(10),
VoucherNumber nvarchar(10)
);

-- It inserts Journal Detail Info to temporary table @JDetails
INSERT INTO @CRDetails( CashReceiptID, LedgerID, Amount, Remarks,VoucherType,VoucherNumber )
SELECT @CRID AS CashReceiptID, LedgerID, Amount,  Remarks,VoucherType,VoucherNumber
 FROM OpenXml( @docHandle, '/CASHRECEIPT/CASHRECEIPTDETAIL/DETAIL', 2)   WITH 
  ( CashReceiptID int,LedgerID int, Amount money,Remarks nvarchar(500),VoucherType nvarchar(10),VoucherNumber nvarchar(10) ) 
  
  -- It inserts Journal Detail Info to table at once
INSERT INTO [Acc].[tblCashReceiptDetails] ( CashReceiptID, LedgerID, Amount, Remarks,VoucherType,VoucherNumber ) SELECT CashReceiptID, LedgerID, Amount, Remarks,VoucherType,VoucherNumber FROM @CRDetails;

  
  IF @@ERROR<>0 BEGIN ROLLBACK TRANSACTION RETURN -101 END
  
  DECLARE @griddetailvalues nvarchar(4000)

DECLARE @GridDetail TABLE
(
Isnew nvarchar(10),
oldgridvalue NVARCHAR(4000),
newgridvalue NVARCHAR(4000)
);
INSERT INTO @GridDetail(Isnew,oldgridvalue,newgridvalue)select isNew,OldGridDetails,NewGridDetails from openxml (@docHandle,'/CASHRECEIPT/LOGGRIDDETAILS',2) WITH
(isnew nvarchar(10),NewGridDetails nvarchar(4000),OldGridDetails nvarchar(4000))
IF @@ERROR<>0 BEGIN ROLLBACK TRANSACTION RETURN -101 END

set @griddetailvalues=
(
SELECT oldgridvalue+newgridvalue As GridContents from @GridDetail
)
 IF @@ERROR<>0 BEGIN ROLLBACK TRANSACTION RETURN -101 END

 DECLARE @COMPUTERDETAILS TABLE
(
CashReceiptID INT, 
CompName nvarchar(100),
MacAddress nvarchar(50),
IpAddress nvarchar(50),
UserName nvarchar(20),
Voucher_Type nvarchar(20),
Description nvarchar(4000),
vocherdate datetime
);

INSERT INTO @COMPUTERDETAILS(CashReceiptID,CompName,MacAddress,IpAddress, UserName,Voucher_Type,Description,vocherdate)
SELECT @CRID AS CashReceiptID,CompDetails,MacAddress,IpAddress, Created_By,'CASH_RCPT',@griddetailvalues,CashReceipt_Date
From openxml(@docHandle,'/CASHRECEIPT/COMPUTERDETAILS',2)WITH (CompDetails nvarchar(100),MacAddress nvarchar(50),IpAddress nvarchar(50)),
Openxml( @docHandle, '/CASHRECEIPT/CASHRECEIPTMASTER', 2) WITH (Created_By nvarchar(20),CashReceipt_Date datetime),
openxml(@docHandle,'/CASHRECEIPT/CASHRECEIPTDETAIL/DETAIL',2)WITH (NewGridDetails nvarchar(4000),OldGridDetails nvarchar(4000))
  IF @@ERROR<>0 BEGIN ROLLBACK TRANSACTION RETURN -101 END
  
  INSERT INTO System.tblAuditLog(ComputerName,UserName,Voucher_Type,Action,Description,RowID,VoucherDate,MAC_Address,IP_Address)
   select CompName,UserName,Voucher_Type,isNew,Description,CashReceiptID,vocherdate,MacAddress,IpAddress from @COMPUTERDETAILS,
   openxml(@docHandle,'/CASHRECEIPT/LOGGRIDDETAILS',2)WITH (isNew nvarchar(10))
     IF @@ERROR<>0 BEGIN ROLLBACK TRANSACTION RETURN -101 END

    
  --Insert Credit column in Transaction table
  INSERT INTO [Acc].[tblTransaction] (LedgerID,Debit_Amount,Credit_Amount,VoucherType,RowID,TransactDate,ProjectID)
  SELECT crd.LedgerID LedgerID,0, crd.Amount,'CASH_RCPT',cr.CashReceiptID,cr.CashReceipt_Date, ProjectID FROM @CashReceiptTable cr Left outer join @CRDetails crd on cr.CashReceiptID=crd.CashReceiptID;
  
  --Insert Debit column in Transaction Table
  INSERT INTO [Acc].[tblTransaction] (LedgerID,Debit_Amount,Credit_Amount,VoucherType,RowID,TransactDate,ProjectID)
  SELECT cr.LedgerID LedgerID, crd.Amount,0,'CASH_RCPT',cr.CashReceiptID,cr.CashReceipt_Date, ProjectID FROM @CashReceiptTable cr Left outer join @CRDetails crd on cr.CashReceiptID=crd.CashReceiptID;
  
  IF @@ERROR<>0 BEGIN ROLLBACK TRANSACTION RETURN -101 END
  
  -- Process for tblTransactionClass
DECLARE @CRTransactionClass TABLE
(
TransactionID INT,
RowID INT, 
VoucherType NVARCHAR(50)
);
 
 --Read All Accounting Class
DECLARE @CRAccClassID TABLE
(
	AccClassID INT 
);

-- Write to temporary table @JAccClassID from xml
INSERT INTO @CRAccClassID(AccClassID)
    SELECT
        IdNode.value('(.)[1]', 'int')
    FROM
        @cashreceipt.nodes('/CASHRECEIPT/ACCCLASSIDS/AccID') AS IdTbl(IdNode)

-- Write from Transaction Table Where RowID is JournalID in JournalDetail
 INSERT INTO @CRTransactionClass (TransactionID,RowID,VoucherType)
	 SELECT TransactionID,RowID,VoucherType FROM [Acc].[tblTransaction] WHERE RowID IN (SELECT DISTINCT CashReceiptID FROM @CRDetails )

  --Insert Accounting Class For each transaction
 INSERT INTO [Acc].[tblTransactionClass] (TransactionID, AccClassID,RowID,VoucherType)
 SELECT TransactionID, TAC.AccClassID,RowID, VoucherType FROM @CRTransactionClass , @CRAccClassID TAC 
  
COMMIT TRANSACTION

EXEC sp_xml_removedocument @docHandle SELECT @CRID AS [CashReceipt ID]