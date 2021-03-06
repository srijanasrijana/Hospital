
/****** Object:  StoredProcedure [Acc].[xmlBankPaymentUpdate]    Script Date: 3/22/2017 2:33:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--CREATED BY:BIMAL KHADKA

--IN THIS PROC IT UPDATES MASTER TABLE AND DELETE THAT BankPaymentID VALUES FROM DETAILS TABLE AND AGAIN INSERT THE EDITED VALUES IN THE DETAILS TABLE WITH SAME BankPaymentID.
ALTER PROCEDURE [Acc].[xmlBankPaymentUpdate]
 @bankpayment  xml     -- It encludes Bank Payment Master Info, Bank Payment Detail Info And Accounting Class Info
  AS
DECLARE @docHandle int;
EXEC sp_xml_preparedocument @docHandle OUTPUT, @bankpayment;

--FOR TO UPADATE MASTER TABLE
DECLARE 
	@BankPaymentID INT,
	@SeriesID INT,
	@LedgerID INT,
	@Voucher_No NVARCHAR(50), 
	@BankPayment_Date DATETIME,
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
     @BankPaymentID=BankPaymentID,
     @SeriesID= SeriesID,
	 @LedgerID=LedgerID,
	 @Voucher_No=Voucher_No,
	 @BankPayment_Date=BankPayment_Date,
	 @Remarks=Remarks, 
	 @ProjectID=ProjectID, 
	 @Created_By=Created_By, 
	 @Created_Date=Created_Date, 
	 @Modified_By=Modified_By, 
	 @Modified_Date=Modified_Date,
	 @Field1=Field1,
	 @Field2=Field2,
	 @Field3=Field3,
	 @Field4=Field4,
	 @Field5=Field5 
   
  -- SELECTION FROM XML
  FROM Openxml( @docHandle, '/BANKPAYMENT/BANKPAYMENTMASTER', 2) WITH ( BankPaymentID INT,
  SeriesID int, LedgerID int,Voucher_No nvarchar(30),   BankPayment_Date datetime, Remarks nvarchar(200), ProjectID int, Created_By nvarchar(20), Created_Date datetime, Modified_By nvarchar(20), Modified_Date datetime,Field1 nvarchar(50),
  Field2 nvarchar(50),Field3 nvarchar(50),Field4 nvarchar(50),Field5 nvarchar(50) )




BEGIN TRANSACTION

--It Updates BankPayment Master Record From XML to Table
  update  [Acc].[tblBankPaymentMaster] SET 
		SeriesID=@SeriesID,
	    LedgerID=@LedgerID,
		Voucher_No=@Voucher_No, 
        BankPayment_Date=@BankPayment_Date,
        Remarks=@Remarks,
		ProjectID=@ProjectID,
		Created_By=@Created_By,
		Created_Date=@Created_Date,
		Modified_By=@Modified_By,
		Modified_Date=@Modified_Date,
		Field1=@Field1,Field2=@Field2,Field3=@Field3,Field4=@Field4,Field5=@Field5 
		where BankPaymentID=@BankPaymentID
  
  
IF @@ERROR<>0 BEGIN ROLLBACK TRANSACTION RETURN -100 END

--SET @BPID = SCOPE_IDENTITY()
--IT DELETE BankPaymentID AND AGAIN WRITE @BankPaymentID WITH VALUES 
-- TO PREVENT CREATING DUPLICATE ROWS 

DELETE  from acc.tblBankPaymentDetails where BankPaymentID=@BankPaymentID
  
DECLARE @BPDetails TABLE
(
BankPaymentID INT, 
LedgerID INT, 
Amount float, 
Remarks NVARCHAR(500),
ChequeNumber NVARCHAR(50),
ChequeDate DATETIME
);

---- It inserts Bank Payment Detail Info to temporary table @BPDetails
INSERT INTO @BPDetails( BankPaymentID, LedgerID, Amount, Remarks, ChequeNumber, ChequeDate )
SELECT  BankPaymentID, LedgerID, Amount, Remarks, ChequeNumber, ChequeDate
 FROM OpenXml( @docHandle, '/BANKPAYMENT/BANKPAYMENTDETAIL/DETAIL', 2)   WITH 
  ( BankPaymentID int,LedgerID int, Amount money, Remarks nvarchar(500), ChequeNumber nvarchar(50), ChequeDate datetime ) 
  
    -- It inserts Bank Payment Detail Info to table at once
INSERT INTO [Acc].[tblBankPaymentDetails] ( BankPaymentID, LedgerID, Amount, Remarks, ChequeNumber, ChequeDate ) SELECT BankPaymentID, LedgerID, Amount, Remarks, ChequeNumber, ChequeDate FROM @BPDetails;

  
  IF @@ERROR<>0 BEGIN ROLLBACK TRANSACTION RETURN -101 END
  
  DECLARE @BPDueDate TABLE
(
DueDate datetime, 
LedgerID INT, 
VoucherType nvarchar(20), 
VoucherNo NVARCHAR(50),
RowID int
);
insert into @BPDueDate(DueDate,LedgerID,VoucherType,VoucherNo,RowID)
SELECT  DueDate, LedgerID, VoucherType, VoucherNo,@BankPaymentID
 FROM OpenXml( @docHandle, '/BANKPAYMENT/BANKDEBTORSDUEDATE/DUEDATEDETAIL', 2)   WITH 
  ( DUEDATE datetime,LedgerID int,VoucherType nvarchar(10),VoucherNo nvarchar(20) )  

Delete from System.tblDueDate where RowID =@BankPaymentID and VoucherType='BANK_PMNT'

insert into System.tblDueDate(DueDate,LedgerID,VoucherType,VoucherNo,RowID) 
SELECT DueDate, LedgerID, VoucherType, VoucherNo,RowID FROM @BPDueDate;   
   
IF @@ERROR<>0 BEGIN ROLLBACK TRANSACTION RETURN -101 END   
   
 declare @griddetailvalues nvarchar(4000)

DECLARE @GridDetail TABLE
(
Isnew nvarchar(10),
oldgridvalue NVARCHAR(4000),
newgridvalue NVARCHAR(4000)
);
insert into @GridDetail(Isnew,oldgridvalue,newgridvalue)select isNew,OldGridDetails,NewGridDetails from openxml (@docHandle,'/BANKPAYMENT/LOGGRIDDETAILS',2) WITH
(isnew nvarchar(10),NewGridDetails nvarchar(4000),OldGridDetails nvarchar(4000))
IF @@ERROR<>0 BEGIN ROLLBACK TRANSACTION RETURN -101 END

set @griddetailvalues=
(
SELECT oldgridvalue+newgridvalue As GridContents from @GridDetail
)
 IF @@ERROR<>0 BEGIN ROLLBACK TRANSACTION RETURN -101 END

 DECLARE @COMPUTERDETAILS TABLE
(
BankPaymentID INT, 
CompName nvarchar(100),
MacAddress nvarchar(50),
IpAddress nvarchar(50),
UserName nvarchar(20),
Voucher_Type nvarchar(20),
Description nvarchar(4000),
vocherdate datetime
);

INSERT INTO @COMPUTERDETAILS(BankPaymentID,CompName,MacAddress,IpAddress,UserName,Voucher_Type,Description,vocherdate)
SELECT  BankPaymentID=@BankPaymentID,CompDetails,MacAddress,IpAddress,Created_By,'BANK_PMNT',@griddetailvalues,BankPayment_Date
From openxml(@docHandle,'/BANKPAYMENT/COMPUTERDETAILS',2)WITH (CompDetails nvarchar(100),MacAddress nvarchar(50),IpAddress nvarchar(50)),
Openxml( @docHandle, '/BANKPAYMENT/BANKPAYMENTMASTER', 2) WITH (Created_By nvarchar(20),BankPayment_Date datetime),
openxml(@docHandle,'/BANKPAYMENT/BANKPAYMENTDETAIL/DETAIL',2)WITH (NewGridDetails nvarchar(4000),OldGridDetails nvarchar(4000))
  IF @@ERROR<>0 BEGIN ROLLBACK TRANSACTION RETURN -101 END
  
  INSERT INTO System.tblAuditLog(ComputerName,UserName,Voucher_Type,Action,Description,RowID,VoucherDate,MAC_Address,IP_Address)
   select CompName,UserName,Voucher_Type,isNew,Description,BankPaymentID,vocherdate,MacAddress,IpAddress from @COMPUTERDETAILS,
   openxml(@docHandle,'/BANKPAYMENT/LOGGRIDDETAILS',2)WITH (isNew nvarchar(10))
     IF @@ERROR<>0 BEGIN ROLLBACK TRANSACTION RETURN -101 END
     
 --DELETE FROM ACC.tblTransaction WHERE VoucherType='BANK_PMNT' AND RowID=@BankPaymentID
    
  --Insert Debit column in Transaction table
  INSERT INTO [Acc].[tblTransaction] (LedgerID,Debit_Amount,Credit_Amount,VoucherType,RowID,TransactDate,ProjectID)
  SELECT bpd.LedgerID LedgerID, bpd.Amount,0,'BANK_PMNT',bp.BankPaymentID,bp.BankPayment_Date, ProjectID FROM acc.tblBankPaymentMaster bp Left outer join @BPDetails bpd on bp.BankPaymentID=bpd.BankPaymentID 
  WHERE UPPER(bpd.Amount)='DEBIT' ; 
  
  --Insert Credit column in Transaction Table
  INSERT INTO [Acc].[tblTransaction] (LedgerID,Debit_Amount,Credit_Amount,VoucherType,RowID,TransactDate,ProjectID)
  SELECT bp.LedgerID LedgerID,0,bpd.Amount,'BANK_PMNT',bp.BankPaymentID,bp.BankPayment_Date, ProjectID FROM acc.tblBankPaymentMaster bp Left outer join @BPDetails bpd on bp.BankPaymentID=bpd.BankPaymentID
  WHERE UPPER(bpd.Amount)='CREDIT';
  
  IF @@ERROR<>0 BEGIN ROLLBACK TRANSACTION RETURN -101 END
  
  -- Process for tblTransactionClass
DECLARE @BPTransactionClass TABLE
(
TransactionID INT,
RowID INT, 
VoucherType NVARCHAR(50)
);
 
 --Read All Accounting Class
DECLARE @BPAccClassID TABLE
(
	AccClassID INT 
);

-- Write to temporary table @JAccClassID from xml
INSERT INTO @BPAccClassID(AccClassID)
    SELECT
        IdNode.value('(.)[1]', 'int')
    FROM
        @bankpayment.nodes('/BANKPAYMENT/ACCCLASSIDS/AccID') AS IdTbl(IdNode)

-- Write from Transaction Table Where RowID is JournalID in JournalDetail
 INSERT INTO @BPTransactionClass (TransactionID,RowID,VoucherType)
	 SELECT TransactionID,RowID,VoucherType FROM [Acc].[tblTransaction] WHERE RowID IN (SELECT DISTINCT BankPaymentID FROM @BPDetails )

  --Insert Accounting Class For each transaction
 INSERT INTO [Acc].[tblTransactionClass] (TransactionID, AccClassID,RowID,VoucherType)
 SELECT TransactionID, TAC.AccClassID,RowID, VoucherType FROM @BPTransactionClass , @BPAccClassID TAC 
  
COMMIT TRANSACTION

--EXEC sp_xml_removedocument @docHandle SELECT @BPID AS [BankPayment ID]
