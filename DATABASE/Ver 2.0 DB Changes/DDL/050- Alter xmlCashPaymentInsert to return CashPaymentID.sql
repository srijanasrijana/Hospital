/****** Object:  StoredProcedure [Acc].[xmlCashPaymentInsert]    Script Date: 10/18/2016 4:39:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [Acc].[xmlCashPaymentInsert]
 @cashpayment  xml  ,   -- It encludes Cash Payment Master Info, Cash Payment Detail Info And Accounting Class Info
  @returnId int = 0 output 
  AS
DECLARE @docHandle int, @CPID int;
EXEC sp_xml_preparedocument @docHandle OUTPUT, @cashpayment;
--Modified By Ramesh prajapati
DECLARE @CashPaymentTable TABLE
(
	CashPaymentID INT,
	SeriesID INT,
	LedgerID INT,
	Voucher_No NVARCHAR(50), 
	CashPayment_Date DATETIME,
	Remarks NVARCHAR(200),
	ProjectID INT,
	Created_By NVARCHAR(50),
	Created_Date DATETIME,
	Modified_By NVARCHAR(50),
	Modified_Date DATETIME
);

BEGIN TRANSACTION

--It Inserts Cash Payment Master Record From XML to Table
INSERT INTO [Acc].[tblCashPaymentMaster]( SeriesID, LedgerID, Voucher_No, CashPayment_Date,Remarks,ProjectID,Created_By,Created_Date,Modified_By,Modified_Date,Field1,Field2,Field3,Field4,Field5 ) 
  SELECT  SeriesID, LedgerID, Voucher_No, CashPayment_Date, Remarks, ProjectID, Created_By, Created_Date, Modified_By, Modified_Date,Field1,Field2,Field3,Field4,Field5 
  FROM Openxml( @docHandle, '/CASHPAYMENT/CASHPAYMENTMASTER', 2) WITH ( 
  SeriesID int, LedgerID int,Voucher_No nvarchar(30), CashPayment_Date datetime, Remarks nvarchar(200), ProjectID int, Created_By nvarchar(20), Created_Date datetime, Modified_By nvarchar(20), Modified_Date datetime,Field1 nvarchar(50),
  Field2 nvarchar(50),Field3 nvarchar(50),Field4 nvarchar(50),Field5 nvarchar(50) )
  
IF @@ERROR<>0 BEGIN ROLLBACK TRANSACTION RETURN -100 END

SET @CPID = SCOPE_IDENTITY()

SET @returnId = @CPID;
--Also Inserts Cash Payment Master Record From XML to Temporary Table @CashPaymentTable
INSERT INTO @CashPaymentTable(CashPaymentID, SeriesID, LedgerID, Voucher_No, CashPayment_Date,Remarks,Created_By,Created_Date,Modified_By,Modified_Date,ProjectID ) 
SELECT @CPID AS CashPaymentID, SeriesID, LedgerID, Voucher_No, CashPayment_Date, Remarks, Created_By, Created_Date, Modified_By, Modified_Date,ProjectID
  FROM Openxml( @docHandle, '/CASHPAYMENT/CASHPAYMENTMASTER', 2) WITH (
  SeriesID int, LedgerID int, Voucher_No nvarchar(30), CashPayment_Date datetime, Remarks nvarchar(200), Created_By nvarchar(20), Created_Date datetime, Modified_By nvarchar(20), Modified_Date datetime,ProjectID int )
  
DECLARE @CPDetails TABLE
(
CashPaymentID INT, 
LedgerID INT, 
Amount float, 
Remarks NVARCHAR(500)
);

-- It inserts Cash Payment Detail Info to temporary table @CPDetails
INSERT INTO @CPDetails( CashPaymentID, LedgerID, Amount, Remarks )
SELECT @CPID AS CashPaymentID, LedgerID, Amount, Remarks
 FROM OpenXml( @docHandle,  '/CASHPAYMENT/CASHPAYMENTDETAIL/DETAIL', 2)   WITH 
  ( CashPaymentID int,LedgerID int, Amount money,  Remarks nvarchar(500) ) 
  
  -- It inserts Cash Payment Detail Info to table at once
INSERT INTO [Acc].[tblCashPaymentDetails] ( CashPaymentID, LedgerID, Amount, Remarks ) SELECT CashPaymentID, LedgerID, Amount, Remarks FROM @CPDetails;

  
  IF @@ERROR<>0 BEGIN ROLLBACK TRANSACTION RETURN -101 END
  
  DECLARE @CPDueDate TABLE
(
RowID int,
DueDate datetime, 
LedgerID INT, 
VoucherType nvarchar(20), 
VoucherNo NVARCHAR(50)

);
insert into @CPDueDate(RowID,DueDate,LedgerID,VoucherType,VoucherNo)
SELECT @CPID AS JournalID, DueDate, LedgerID, VoucherType, VoucherNo
 FROM OpenXml( @docHandle, '/CASHPAYMENT/CASHDEBTORSDUEDATE/DUEDATEDETAIL', 2)   WITH 
  ( JournalID int,DUEDATE datetime,LedgerID int,VoucherType nvarchar(10),VoucherNo nvarchar(20) )  

insert into System.tblDueDate(DueDate,LedgerID,VoucherType,VoucherNo,RowID) 
SELECT DueDate, LedgerID, VoucherType, VoucherNo,RowID FROM @CPDueDate;   
   
IF @@ERROR<>0 BEGIN ROLLBACK TRANSACTION RETURN -101 END  
  
DECLARE @griddetailvalues nvarchar(4000)

DECLARE @GridDetail TABLE
(
Isnew nvarchar(10),
oldgridvalue NVARCHAR(4000),
newgridvalue NVARCHAR(4000)
);
INSERT INTO @GridDetail(Isnew,oldgridvalue,newgridvalue)select isNew,OldGridDetails,NewGridDetails from openxml (@docHandle,'/CASHPAYMENT/LOGGRIDDETAILS',2) WITH
(isnew nvarchar(10),NewGridDetails nvarchar(4000),OldGridDetails nvarchar(4000))
IF @@ERROR<>0 BEGIN ROLLBACK TRANSACTION RETURN -101 END

set @griddetailvalues=
(
SELECT oldgridvalue+newgridvalue As GridContents from @GridDetail
)
 IF @@ERROR<>0 BEGIN ROLLBACK TRANSACTION RETURN -101 END

 DECLARE @COMPUTERDETAILS TABLE
(
CashPaymentID INT, 
CompName nvarchar(100),
MacAddress nvarchar(50),
IpAddress nvarchar(50),
UserName nvarchar(20),
Voucher_Type nvarchar(20),
Description nvarchar(4000),
vocherdate datetime
);

INSERT INTO @COMPUTERDETAILS(CashPaymentID,CompName,MacAddress,IpAddress,UserName,Voucher_Type,Description,vocherdate)
SELECT @CPID AS CashPaymentID,CompDetails,MacAddress,IpAddress,Created_By,'CASH_PMNT',@griddetailvalues,CashPayment_Date
From openxml(@docHandle,'/CASHPAYMENT/COMPUTERDETAILS',2)WITH (CompDetails nvarchar(100),MacAddress nvarchar(50),IpAddress nvarchar(50)),
Openxml( @docHandle, '/CASHPAYMENT/CASHPAYMENTMASTER', 2) WITH (Created_By nvarchar(20),CashPayment_Date datetime),
openxml(@docHandle,'/CASHPAYMENT/CASHPAYMENTDETAIL/DETAIL',2)WITH (NewGridDetails nvarchar(4000),OldGridDetails nvarchar(4000))
  IF @@ERROR<>0 BEGIN ROLLBACK TRANSACTION RETURN -101 END
  
  INSERT INTO System.tblAuditLog(ComputerName,UserName,Voucher_Type,Action,Description,RowID,VoucherDate,MAC_Address,IP_Address)
   select CompName,UserName,Voucher_Type,isNew,Description,CashPaymentID,vocherdate,MacAddress,IpAddress from @COMPUTERDETAILS,
   openxml(@docHandle,'/CASHPAYMENT/LOGGRIDDETAILS',2)WITH (isNew nvarchar(10))
     IF @@ERROR<>0 BEGIN ROLLBACK TRANSACTION RETURN -101 END
    
  --Insert Debit column in Transaction table
  INSERT INTO [Acc].[tblTransaction] (LedgerID,Debit_Amount,Credit_Amount,VoucherType,RowID,TransactDate,ProjectID)
  SELECT cpd.LedgerID LedgerID, cpd.Amount,0,'CASH_PMNT',cp.CashPaymentID,cp.CashPayment_Date, ProjectID FROM @CashPaymentTable cp Left outer join @CPDetails cpd on cp.CashPaymentID = cpd.CashPaymentID;
  
  --Insert Credit column in Transaction Table
  INSERT INTO [Acc].[tblTransaction] (LedgerID,Debit_Amount,Credit_Amount,VoucherType,RowID,TransactDate,ProjectID)
  SELECT cp.LedgerID LedgerID,0, cpd.Amount,'CASH_PMNT',cp.CashPaymentID,cp.CashPayment_Date, ProjectID FROM @CashPaymentTable cp Left outer join @CPDetails cpd on cp.CashPaymentID=cpd.CashPaymentID;
  
  IF @@ERROR<>0 BEGIN ROLLBACK TRANSACTION RETURN -101 END
  
  -- Process for tblTransactionClass
DECLARE @CPTransactionClass TABLE
(
TransactionID INT,
RowID INT, 
VoucherType NVARCHAR(50)
);
 
 --Read All Accounting Class
DECLARE @CPAccClassID TABLE
(
	AccClassID INT 
);

-- Write to temporary table @JAccClassID from xml
INSERT INTO @CPAccClassID(AccClassID)
    SELECT
        IdNode.value('(.)[1]', 'int')
    FROM
        @cashpayment.nodes('/CASHPAYMENT/ACCCLASSIDS/AccID') AS IdTbl(IdNode)

-- Write from Transaction Table Where RowID is JournalID in JournalDetail
 INSERT INTO @CPTransactionClass (TransactionID,RowID,VoucherType)
	 SELECT TransactionID,RowID,VoucherType FROM [Acc].[tblTransaction] WHERE RowID IN (SELECT DISTINCT CashPaymentID FROM @CPDetails )

  --Insert Accounting Class For each transaction
 INSERT INTO [Acc].[tblTransactionClass] (TransactionID, AccClassID,RowID,VoucherType)
 SELECT TransactionID, TAC.AccClassID,RowID, VoucherType FROM @CPTransactionClass , @CPAccClassID TAC 
  
COMMIT TRANSACTION

EXEC sp_xml_removedocument @docHandle SELECT @CPID AS [CashPayment ID]
