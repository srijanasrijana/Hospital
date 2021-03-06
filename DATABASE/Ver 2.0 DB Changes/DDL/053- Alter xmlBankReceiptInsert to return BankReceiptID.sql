--USE [BENT003]
--GO
/****** Object:  StoredProcedure [Acc].[xmlBankReceiptInsert]    Script Date: 10/19/2016 2:17:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [Acc].[xmlBankReceiptInsert]
 @bankreceipt  xml ,    -- It encludes Bank Receipt Master Info, Bank Receipt Detail Info And Accounting Class Info
   @returnId int = 0 output 
  AS
DECLARE @docHandle int, @BRID int;
EXEC sp_xml_preparedocument @docHandle OUTPUT, @bankreceipt;

DECLARE @BankReceiptTable TABLE
(
	BankReceiptID INT,
	SeriesID INT,
	LedgerID INT,
	Voucher_No NVARCHAR(50), 
	BankReceipt_Date DATETIME,
	Remarks NVARCHAR(200),
	ProjectID INT,
	Created_By NVARCHAR(50),
	Created_Date DATETIME,
	Modified_By NVARCHAR(50),
	Modified_Date DATETIME
);

BEGIN TRANSACTION
--Modified By Ramesh Prajapati
--It Inserts Bank Receipt Master Record From XML to Table
INSERT INTO [Acc].[tblBankReceiptMaster]( SeriesID, LedgerID, Voucher_No, BankReceipt_Date,Remarks,ProjectID,Created_By,Created_Date,Modified_By,Modified_Date,Field1,Field2,Field3,Field4,Field5 ) 
  SELECT  SeriesID, LedgerID, Voucher_No, BankReceipt_Date, Remarks, ProjectID, Created_By, Created_Date, Modified_By, Modified_Date ,Field1,Field2,Field3,Field4,Field5
  FROM Openxml( @docHandle, '/BANKRECEIPT/BANKRECEIPTMASTER', 2) WITH ( 
  SeriesID int, LedgerID int, Voucher_No nvarchar(30), BankReceipt_Date datetime, Remarks nvarchar(200), ProjectID int, Created_By nvarchar(20), Created_Date datetime, Modified_By nvarchar(20), Modified_Date datetime,Field1 nvarchar(50),
  Field2 nvarchar(50),Field3 nvarchar(50),Field4 nvarchar(50),Field5 nvarchar(50) )
  
IF @@ERROR<>0 BEGIN ROLLBACK TRANSACTION RETURN -100 END

SET @BRID = SCOPE_IDENTITY()
SET @returnId = @BRID;
--Also Inserts Journal Master Record From XML to Temporary Table @JournalTable
INSERT INTO @BankReceiptTable(BankReceiptID, SeriesID, LedgerID, Voucher_No, BankReceipt_Date,Remarks, ProjectID,Created_By,Created_Date,Modified_By,Modified_Date ) 
SELECT @BRID AS BankReceiptID, SeriesID, LedgerID, Voucher_No, BankReceipt_Date, Remarks, ProjectID, Created_By, Created_Date, Modified_By, Modified_Date 
  FROM Openxml( @docHandle, '/BANKRECEIPT/BANKRECEIPTMASTER', 2) WITH (
  SeriesID int, LedgerID int,Voucher_No nvarchar(30), BankReceipt_Date datetime, Remarks nvarchar(200), ProjectID int, Created_By nvarchar(20), Created_Date datetime, Modified_By nvarchar(20), Modified_Date datetime )
  
DECLARE @BRDetails TABLE
(
BankReceiptID INT, 
LedgerID INT, 
Amount float, 
Remarks NVARCHAR(500),
ChequeNumber NVARCHAR(50),
ChequeBank NVARCHAR(50),
ChequeDate DATETIME,
VoucherType nvarchar(10),
VoucherNumber nvarchar(10)
);

-- It inserts Bank Receipt Detail Info to temporary table @BRDetails
INSERT INTO @BRDetails( BankReceiptID, LedgerID, Amount, Remarks, ChequeNumber, ChequeBank, ChequeDate,VoucherType,VoucherNumber )
SELECT @BRID AS BankReceiptID, LedgerID, Amount, Remarks, ChequeNumber, ChequeBank, ChequeDate,VoucherType,VoucherNumber
 FROM OpenXml( @docHandle, '/BANKRECEIPT/BANKRECEIPTDETAIL/DETAIL', 2)   WITH 
  ( BankReceiptID int,LedgerID int, Amount money, Remarks nvarchar(500), ChequeNumber nvarchar(50), ChequeBank nvarchar(50), ChequeDate datetime,VoucherType nvarchar(10),VoucherNumber nvarchar(10) ) 
  
  -- It inserts Journal Detail Info to table at once
INSERT INTO [Acc].[tblBankReceiptDetails] ( BankReceiptID, LedgerID, Amount, Remarks, ChequeNumber, ChequeBank, ChequeDate,VoucherType,VoucherNumber ) SELECT BankReceiptID, LedgerID, Amount, Remarks, ChequeNumber, ChequeBank, ChequeDate,VoucherType,VoucherNumber FROM @BRDetails;
 
IF @@ERROR<>0 BEGIN ROLLBACK TRANSACTION RETURN -101 END

declare @griddetailvalues nvarchar(4000)

DECLARE @GridDetail TABLE
(
Isnew nvarchar(10),
oldgridvalue NVARCHAR(4000),
newgridvalue NVARCHAR(4000)
);
insert into @GridDetail(Isnew,oldgridvalue,newgridvalue)select isNew,OldGridDetails,NewGridDetails from openxml (@docHandle,'/BANKRECEIPT/LOGGRIDDETAILS',2) WITH
(isnew nvarchar(10),NewGridDetails nvarchar(4000),OldGridDetails nvarchar(4000))
IF @@ERROR<>0 BEGIN ROLLBACK TRANSACTION RETURN -101 END

set @griddetailvalues=
(
SELECT oldgridvalue+newgridvalue As GridContents from @GridDetail
)
 IF @@ERROR<>0 BEGIN ROLLBACK TRANSACTION RETURN -101 END

 DECLARE @COMPUTERDETAILS TABLE
(
BankReceiptID INT, 
CompName nvarchar(100),
MacAddress nvarchar(50),
IpAddress nvarchar(50),
UserName nvarchar(20),
Voucher_Type nvarchar(20),
Description nvarchar(4000),
vocherdate datetime
);

INSERT INTO @COMPUTERDETAILS(BankReceiptID,CompName,MacAddress,IpAddress,UserName,Voucher_Type,Description,vocherdate)
SELECT @BRID AS BankReceiptID,CompDetails,MacAddress,IpAddress,Created_By,'BANK_RCPT',@griddetailvalues,BankReceipt_Date
From openxml(@docHandle,'/BANKRECEIPT/COMPUTERDETAILS',2)WITH (CompDetails nvarchar(100),MacAddress nvarchar(50),IpAddress nvarchar(50)),
Openxml( @docHandle, '/BANKRECEIPT/BANKRECEIPTMASTER', 2) WITH (Created_By nvarchar(20),BankReceipt_Date datetime),
openxml(@docHandle,'/BANKRECEIPT/BANKRECEIPTDETAIL/DETAIL',2)WITH (NewGridDetails nvarchar(4000),OldGridDetails nvarchar(4000))
  IF @@ERROR<>0 BEGIN ROLLBACK TRANSACTION RETURN -101 END
  
  INSERT INTO System.tblAuditLog(ComputerName,UserName,Voucher_Type,Action,Description,RowID,VoucherDate,MAC_Address,IP_Address)
   select CompName,UserName,Voucher_Type,isNew,Description,BankReceiptID,vocherdate,MacAddress,IpAddress from @COMPUTERDETAILS,
   openxml(@docHandle,'/BANKRECEIPT/LOGGRIDDETAILS',2)WITH (isNew nvarchar(10))
     IF @@ERROR<>0 BEGIN ROLLBACK TRANSACTION RETURN -101 END
   
    
  --Insert Debit column in Transaction table
  INSERT INTO [Acc].[tblTransaction] (LedgerID,Debit_Amount,Credit_Amount,VoucherType,RowID,TransactDate,ProjectID)
  SELECT br.LedgerID LedgerID, brd.Amount,0,'BANK_RCPT',br.BankReceiptID,br.BankReceipt_Date, ProjectID FROM @BankReceiptTable br Left outer join @BRDetails brd on br.BankReceiptID=brd.BankReceiptID;
  
  --Insert Credit column in Transaction Table
  INSERT INTO [Acc].[tblTransaction] (LedgerID,Debit_Amount,Credit_Amount,VoucherType,RowID,TransactDate,ProjectID)
  SELECT brd.LedgerID LedgerID,0, brd.Amount,'BANK_RCPT',br.BankReceiptID,br.BankReceipt_Date, ProjectID FROM @BankReceiptTable br Left outer join @BRDetails brd on br.BankReceiptID=br.BankReceiptID;
  
  IF @@ERROR<>0 BEGIN ROLLBACK TRANSACTION RETURN -101 END
  
  -- Process for tblTransactionClass
DECLARE @BRTransactionClass TABLE
(
TransactionID INT,
RowID INT, 
VoucherType NVARCHAR(50)
);
 
 --Read All Accounting Class
DECLARE @BRAccClassID TABLE
(
	AccClassID INT 
);

-- Write to temporary table @BRAccClassID from xml
INSERT INTO @BRAccClassID(AccClassID)
    SELECT
        IdNode.value('(.)[1]', 'int')
    FROM
        @bankreceipt.nodes('/BANKRECEIPT/ACCCLASSIDS/AccID') AS IdTbl(IdNode)

-- Write from Transaction Table Where RowID is JournalID in JournalDetail
 INSERT INTO @BRTransactionClass (TransactionID,RowID,VoucherType)
	 SELECT TransactionID,RowID,VoucherType FROM [Acc].[tblTransaction] WHERE RowID IN (SELECT DISTINCT BankReceiptID FROM @BRDetails )

  --Insert Accounting Class For each transaction
 INSERT INTO [Acc].[tblTransactionClass] (TransactionID, AccClassID,RowID,VoucherType)
 SELECT TransactionID, TAC.AccClassID,RowID, VoucherType FROM @BRTransactionClass , @BRAccClassID TAC 
  
COMMIT TRANSACTION

EXEC sp_xml_removedocument @docHandle SELECT @BRID AS [BankReceipt ID]
