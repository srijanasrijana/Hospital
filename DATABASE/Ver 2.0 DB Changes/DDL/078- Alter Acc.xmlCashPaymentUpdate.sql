
/****** Object:  StoredProcedure [Acc].[xmlCashPaymentUpdate]    Script Date: 3/22/2017 2:41:55 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

--CREATED BY:BIMAL BAHADUR KHADKA
ALTER PROCEDURE [Acc].[xmlCashPaymentUpdate]
 @cashpayment  xml     -- It encludes Cash Payment Master Info, Cash Payment Detail Info And Accounting Class Info
  AS
DECLARE @docHandle int;
EXEC sp_xml_preparedocument @docHandle OUTPUT, @cashpayment;

    DECLARE 
	@CashPaymentID INT,
	@SeriesID INT,
	@LedgerID INT,
	@Voucher_No NVARCHAR(50), 
	@CashPayment_Date DATETIME,
	@Remarks NVARCHAR(200),
	@ProjectID INT,
	@Created_By NVARCHAR(50),
	@Created_Date DATETIME,
	@Modified_By NVARCHAR(50),
	@Modified_Date DATETIME,
	@Field1 NVARCHAR(50),
    @Field2 NVARCHAR(50),
	@Field3 NVARCHAR(50),
	@Field4 NVARCHAR(50),
	@Field5 NVARCHAR(50)


SELECT

    @CashPaymentID=CashPaymentID,
	@SeriesID =SeriesID,
	@LedgerID=LedgerID,
	@Voucher_No=Voucher_No, 
	@CashPayment_Date=CashPayment_Date,
	@Remarks=Remarks,
	@ProjectID=ProjectID,
	@Created_By=Created_By,
	@Created_Date=Created_Date,
	@Modified_By=Modified_By,
	@Modified_Date=Modified_Date,
	@Field1=Field1,
    @Field2=Field2,
	@Field3 =Field3,
	@Field4 =Field4,
	@Field5=Field5

  FROM Openxml( @docHandle, '/CASHPAYMENT/CASHPAYMENTMASTER', 2) WITH (CashPaymentID INT,
  SeriesID int, LedgerID int,Voucher_No nvarchar(30), CashPayment_Date datetime, Remarks nvarchar(200), ProjectID int, Created_By nvarchar(20), Created_Date datetime, Modified_By nvarchar(20), Modified_Date datetime,Field1 nvarchar(50),
  Field2 nvarchar(50),Field3 nvarchar(50),Field4 nvarchar(50),Field5 nvarchar(50) )

BEGIN TRANSACTION

UPDATE Acc.tblCashPaymentMaster SET 
	SeriesID=@SeriesID,
	LedgerID=@LedgerID,
	Voucher_No=@Voucher_No, 
	CashPayment_Date=@CashPayment_Date,
	Remarks=@Remarks,
	ProjectID=@ProjectID,
	Created_By=@Created_By,
	Created_Date=@Created_Date,
	Modified_By=@Modified_By,
	Modified_Date=@Modified_Date,
	Field1=@Field1,Field2=@Field2,Field3=@Field3,Field4=@Field4,Field5=@Field5 
	where CashPaymentID=@CashPaymentID
  
IF @@ERROR<>0 BEGIN ROLLBACK TRANSACTION RETURN -100 END

--SET @CPID = SCOPE_IDENTITY()

DELETE FROM Acc.tblCashPaymentDetails WHERE CashPaymentID=@CashPaymentID

DECLARE @CPDetails TABLE
(
CashPaymentID INT, 
LedgerID INT, 
Amount float, 
Remarks NVARCHAR(500)
);

-- It inserts Cash Payment Detail Info to temporary table @CPDetails
INSERT INTO @CPDetails( CashPaymentID, LedgerID, Amount, Remarks )
SELECT  CashPaymentID, LedgerID, Amount, Remarks
 FROM OpenXml( @docHandle,  '/CASHPAYMENT/CASHPAYMENTDETAIL/DETAIL', 2)   WITH 
  ( CashPaymentID int,LedgerID int, Amount money,  Remarks nvarchar(500) ) 
  
  -- It inserts Cash Payment Detail Info to table at once
INSERT INTO [Acc].[tblCashPaymentDetails] ( CashPaymentID, LedgerID, Amount, Remarks ) SELECT CashPaymentID, LedgerID, Amount, Remarks FROM @CPDetails;

  
  IF @@ERROR<>0 BEGIN ROLLBACK TRANSACTION RETURN -101 END
  
  DECLARE @CPDueDate TABLE
(
DueDate datetime, 
LedgerID INT, 
VoucherType nvarchar(20), 
VoucherNo NVARCHAR(50),
RowID int
);
insert into @CPDueDate(DueDate,LedgerID,VoucherType,VoucherNo,RowID)
SELECT  DueDate, LedgerID, VoucherType, VoucherNo,@CashPaymentID
 FROM OpenXml( @docHandle, '/CASHPAYMENT/CASHDEBTORSDUEDATE/DUEDATEDETAIL', 2)   WITH 
  (DUEDATE datetime,LedgerID int,VoucherType nvarchar(10),VoucherNo nvarchar(20) )  

Delete from System.tblDueDate where RowID =@CashPaymentID and VoucherType='CASH_PMNT'

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
SELECT CashPaymentID=@CashPaymentID ,CompDetails,MacAddress,IpAddress,Created_By,'CASH_PMNT',@griddetailvalues,CashPayment_Date
From openxml(@docHandle,'/CASHPAYMENT/COMPUTERDETAILS',2)WITH (CompDetails nvarchar(100),MacAddress nvarchar(50),IpAddress nvarchar(50)),
Openxml( @docHandle, '/CASHPAYMENT/CASHPAYMENTMASTER', 2) WITH (Created_By nvarchar(20),CashPayment_Date datetime),
openxml(@docHandle,'/CASHPAYMENT/CASHPAYMENTDETAIL/DETAIL',2)WITH (NewGridDetails nvarchar(4000),OldGridDetails nvarchar(4000))
  IF @@ERROR<>0 BEGIN ROLLBACK TRANSACTION RETURN -101 END
  
  INSERT INTO System.tblAuditLog(ComputerName,UserName,Voucher_Type,Action,Description,RowID,VoucherDate,MAC_Address,IP_Address)
   select CompName,UserName,Voucher_Type,isNew,Description,CashPaymentID,vocherdate,MacAddress,IpAddress from @COMPUTERDETAILS,
   openxml(@docHandle,'/CASHPAYMENT/LOGGRIDDETAILS',2)WITH (isNew nvarchar(10))
     IF @@ERROR<>0 BEGIN ROLLBACK TRANSACTION RETURN -101 END
    
 --DELETE FROM ACC.tblTransaction WHERE VoucherType='CASH_PMNT' AND RowID=@CashPaymentID
    
    
  ----Insert Debit column in Transaction table
  --INSERT INTO [Acc].[tblTransaction] (LedgerID,Debit_Amount,Credit_Amount,VoucherType,RowID,TransactDate,ProjectID)
  --SELECT cpd.LedgerID LedgerID, cpd.Amount,0,'CASH_PMNT',cp.CashPaymentID,cp.CashPayment_Date, ProjectID FROM ACC.tblCashPaymentMaster cp Left outer join @CPDetails cpd on cp.CashPaymentID = cpd.CashPaymentID WHERE UPPER(cpd.Amount)='DEBIT';
  
  ----Insert Credit column in Transaction Table
  --INSERT INTO [Acc].[tblTransaction] (LedgerID,Debit_Amount,Credit_Amount,VoucherType,RowID,TransactDate,ProjectID)
  --SELECT cpd.LedgerID LedgerID,0, cpd.Amount,'CASH_PMNT',cp.CashPaymentID,cp.CashPayment_Date, ProjectID FROM ACC.tblCashPaymentMaster cp Left outer join @CPDetails cpd on cp.CashPaymentID = cpd.CashPaymentID WHERE UPPER(cpd.Amount)='CREDIT';
  
  
    
  --Insert Debit column in Transaction table
  INSERT INTO [Acc].[tblTransaction] (LedgerID,Debit_Amount,Credit_Amount,VoucherType,RowID,TransactDate,ProjectID)
  SELECT cpd.LedgerID LedgerID, cpd.Amount,0,'CASH_PMNT',cp.CashPaymentID,cp.CashPayment_Date, ProjectID FROM ACC.tblCashPaymentMaster cp Left outer join @CPDetails cpd on cp.CashPaymentID = cpd.CashPaymentID WHERE UPPER(cpd.Amount)='DEBIT';
  
  
  --Insert Credit column in Transaction Table
  INSERT INTO [Acc].[tblTransaction] (LedgerID,Debit_Amount,Credit_Amount,VoucherType,RowID,TransactDate,ProjectID)
  SELECT cp.LedgerID LedgerID,0, cpd.Amount,'CASH_PMNT',cp.CashPaymentID,cp.CashPayment_Date, ProjectID FROM ACC.tblCashPaymentMaster cp Left outer join @CPDetails cpd on cp.CashPaymentID=cpd.CashPaymentID WHERE UPPER(cpd.Amount)='CREDIT';
  
  
  
  
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

--EXEC sp_xml_removedocument @docHandle SELECT @CPID AS [CashPayment ID]

GO


