

/****** Object:  StoredProcedure [Acc].[xmlJournalUpdate]    Script Date: 3/22/2017 1:03:22 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

--CREATED BY:BIMAL BAHADUR KHADKA
ALTER PROCEDURE [Acc].[xmlJournalUpdate]
 @journal  xml     -- It encludes Journal Master Info, Journal Detail Info And Accounting Class Info
  AS
DECLARE @docHandle int;
EXEC sp_xml_preparedocument @docHandle OUTPUT, @journal;

--DECLARE @JournalTable TABLE
--( 
     DECLARE  @JournalID INT,
     @SeriesID INT,
	 @Voucher_No NVARCHAR(50), 
	 @Journal_Date DATETIME,
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
--);


--READ XML and Assign the values of Master variables
SELECT @JournalID=JournalID, 
 @SeriesID=SeriesID,
 @Voucher_No=Voucher_No,
 @Journal_Date=Journal_Date,
 @Remarks=Remarks,
 @ProjectID=ProjectID, 
 @Created_By=Created_By, 
 @Created_Date=Created_Date, 
 @Modified_By=Modified_By, 
 @Modified_Date=Modified_Date,
 @Field1=Field1,@Field2=Field2,@Field3=Field3,@Field4=Field4,@Field5=Field5 
  
  
  FROM Openxml( @docHandle, '/JOURNAL/JOURNALMASTER', 2) WITH (JournalID int,
  SeriesID int,Voucher_No nvarchar(30),   Journal_Date datetime, Remarks nvarchar(200), ProjectID int, Created_By nvarchar(20), Created_Date datetime, Modified_By nvarchar(20), Modified_Date datetime, Field1 nvarchar(50),
  Field2 nvarchar(50),Field3 nvarchar(50),Field4 nvarchar(50),Field5 nvarchar(50) )
 
 
-- select @JournalID;
--return;


BEGIN TRANSACTION

--It Update Journal Master Record From XML to Table
update [Acc].[tblJournalMaster] set 
        SeriesID=@SeriesID,
        Voucher_No=@Voucher_No,
        Journal_Date=@Journal_Date,
		Remarks=@Remarks,
		ProjectID=@ProjectID,
		Created_By=@Created_By,
		Created_Date=@Created_Date,
		Modified_By=@Modified_By,
		Modified_Date=@Modified_Date,
		Field1=@Field1,Field2=@Field2,Field3=@Field3,Field4=@Field4,Field5=@Field5 
		WHERE JournalID=@JournalID;
 
  
  
IF @@ERROR<>0 BEGIN ROLLBACK TRANSACTION RETURN -100 END


--SET @JID = SCOPE_IDENTITY()

DELETE  from acc.tblJournalDetail where JournalID=@JournalID

DECLARE @JDetails TABLE
(
JournalID INT, 
LedgerID INT, 
Amount float, 
DrCr NVARCHAR(50),
Remarks NVARCHAR(500)
);

-- It inserts Journal Detail Info to temporary table @JDetails
INSERT INTO @JDetails( JournalID, LedgerID, Amount, DrCr,Remarks )
SELECT  JournalID, LedgerID, Amount, DrCr, Remarks
 FROM OpenXml( @docHandle, '/JOURNAL/JOURNALDETAIL/DETAIL', 2)   WITH 
  ( JournalID int,LedgerID int, Amount money, DrCr nvarchar(10), Remarks nvarchar(500) ) 
  
  -- It inserts Journal Detail Info to table at once
INSERT INTO [Acc].[tblJournalDetail] ( JournalID, LedgerID, Amount, DrCr,Remarks ) 
SELECT JournalID, LedgerID, Amount, DrCr,Remarks FROM @JDetails;

IF @@ERROR<>0 BEGIN ROLLBACK TRANSACTION RETURN -101 END

--DECLARE @JDueDate TABLE
--(
DECLARE @JDueDate TABLE
(
DueDate datetime, 
LedgerID INT, 
VoucherType nvarchar(20), 
VoucherNo NVARCHAR(50),
RowID int

);
insert into @JDueDate(DueDate,LedgerID,VoucherType,VoucherNo,RowID)
SELECT   DueDate, LedgerID, VoucherType, VoucherNo,@JournalID
 FROM OpenXml( @docHandle, '/JOURNAL/JOURNALDEBTORSDUEDATE/DUEDATEDETAIL', 2)   WITH 
  (DUEDATE datetime,LedgerID int,VoucherType nvarchar(10),VoucherNo nvarchar(20) )  

Delete from System.tblDueDate where RowID =@JournalID and VoucherType='JRNL'
insert into System.tblDueDate(DueDate,LedgerID,VoucherType,VoucherNo,RowID) 
SELECT DueDate, LedgerID, VoucherType, VoucherNo,RowID FROM @JDueDate;

IF @@ERROR<>0 BEGIN ROLLBACK TRANSACTION RETURN -101 END

DECLARE @GridDetail TABLE
(
Isnew nvarchar(10),
oldgridvalue NVARCHAR(4000),
newgridvalue NVARCHAR(4000)
);

insert into @GridDetail(Isnew,oldgridvalue,newgridvalue)select isNew,OldGridDetails,NewGridDetails from openxml (@docHandle,'/JOURNAL/LOGGRIDDETAILS',2) WITH
(isnew nvarchar(10),NewGridDetails nvarchar(4000),OldGridDetails nvarchar(4000))
IF @@ERROR<>0 BEGIN ROLLBACK TRANSACTION RETURN -101 END
-- Declare @IsNew nvarchar(10),@OldGrid nvarchar(4000),@NewGrid nvarchar(4000)

declare @testnew nvarchar(10),@griddetailvalues nvarchar(4000)
set @testnew=
(
select ltrim(rtrim(isnew)) isnew from @GridDetail
)
select @testnew
set @griddetailvalues=
(
SELECT oldgridvalue+newgridvalue As GridContents from @GridDetail
)
 IF @@ERROR<>0 BEGIN ROLLBACK TRANSACTION RETURN -101 END
 --select @IsNew= isNew,@NewGrid=NewGridDetails,@NewGrid=OldGridDetails from openxml(@docHandle,'/JOURNAL/LOGGRIDDETAILS',2) WITH
 --(isnew nvarchar(10),NewGridDetails nvarchar(4000),OldGridDetails nvarchar(4000))
  

  
Declare @CInfo nvarchar(4000);
Declare @Computer table
(
Descriptions nvarchar(4000)
);

insert into @Computer SELECT cast (LedgerID as nvarchar(20))+' '+cast (Amount as nvarchar(20))+' '+ Drcr As Details from @JDetails
IF @@ERROR<>0 BEGIN ROLLBACK TRANSACTION RETURN -101 END

SELECT @CInfo = COALESCE(@CInfo + ', ', '') +'('+ 'LedgerID'+' '+'Amount'+' '+'Drcr '+' '+'Seriesid'+' '+'Voucher_No'+' '+'Journal_Date '+')'+' '
 +Descriptions+' '+cast (SeriesID as nvarchar(10))+' '+ Voucher_No+' '+CAST( Journal_Date as nvarchar(20)) FROM @Computer,
Openxml( @docHandle, '/JOURNAL/JOURNALMASTER', 2) WITH ( 
 SeriesID int,Voucher_No nvarchar(30),   Journal_Date datetime )

SELECT @CInfo
--select CompDetails from openxml (@docHandle,'/JOURNAL/COMPUTERDETAILS',2)
  --WITH (CompDetails nvarchar(100))
 --set @CInfo=
 --(
	--SELECT cast (LedgerID as nvarchar(20))+' '+cast (Amount as nvarchar(20))+' '+ Drcr As Details from @JDetails
 --)
 
 
DECLARE @COMPUTERDETAILS TABLE
(
JournalID INT, 
CompName nvarchar(100),
MacAddress nvarchar(20),
IpAddress nvarchar(30),
UserName nvarchar(20),
Voucher_Type nvarchar(20),
Description nvarchar(4000),
vocherdate datetime
);

INSERT INTO @COMPUTERDETAILS(JournalID,CompName,MacAddress,IpAddress,UserName,Voucher_Type,Description,vocherdate)
SELECT  JournalID=@JournalID,CompDetails,MacAddress,IpAddress,Created_By,'JRNL',@griddetailvalues,Journal_Date
From openxml(@docHandle,'/JOURNAL/COMPUTERDETAILS',2)WITH (CompDetails nvarchar(100),MacAddress nvarchar(20),IpAddress nvarchar(30)),
Openxml( @docHandle, '/JOURNAL/JOURNALMASTER', 2) WITH (Created_By nvarchar(20),Journal_Date datetime)
  IF @@ERROR<>0 BEGIN ROLLBACK TRANSACTION RETURN -101 END

  --Insert Details For AuditLog
  --,UserName,Voucher_Type,Action,Description,RowID
 -- if (@testnew='INSERT')
  --begin
  INSERT INTO System.tblAuditLog(ComputerName,UserName,Voucher_Type,Action,Description,RowID,VoucherDate,MAC_Address,IP_Address)
   select CompName,UserName,Voucher_Type,isNew,Description,JournalID,vocherdate,MacAddress,IpAddress from @COMPUTERDETAILS,
   openxml(@docHandle,'/JOURNAL/LOGGRIDDETAILS',2)WITH (isNew nvarchar(10))
  --end
  --else 
 -- begin
 -- INSERT INTO System.tblAuditLog(ComputerName,UserName,Voucher_Type,Action,Description,RowID,VoucherDate) select CompName,UserName,Voucher_Type,'Edit',Description,JournalID,vocherdate from @COMPUTERDETAILS
  --end;
  
  IF @@ERROR<>0 BEGIN ROLLBACK TRANSACTION RETURN -101 END
    
  Delete from acc.tblTransaction where VoucherType='JRNL' AND RowID=@JournalID
    
  --Insert Debit column in Transaction table
  INSERT INTO [Acc].[tblTransaction] (LedgerID,Debit_Amount,Credit_Amount,VoucherType,RowID,TransactDate,ProjectID)
  SELECT jd.LedgerID LedgerID, jd.Amount,0,'JRNL',jm.JournalID,jm.Journal_Date, ProjectID FROM  [Acc].[tblJournalMaster] jm Left outer join @JDetails jd on jm.JournalID=jd.JournalID WHERE UPPER(jd.DrCr)='DEBIT';
  
  --Insert Credit column in Transaction Table
  INSERT INTO [Acc].[tblTransaction] (LedgerID,Debit_Amount,Credit_Amount,VoucherType,RowID,TransactDate,ProjectID)
  SELECT jd.LedgerID LedgerID,0, jd.Amount,'JRNL',jm.JournalID,jm.Journal_Date,ProjectID FROM  [Acc].[tblJournalMaster] jm Left outer join @JDetails jd on jm.JournalID=jd.JournalID WHERE UPPER(jd.DrCr)='CREDIT';
  
  IF @@ERROR<>0 BEGIN ROLLBACK TRANSACTION RETURN -101 END
  
  -- Process for tblTransactionClass
DECLARE @JTransactionClass TABLE
(
TransactionID INT,
RowID INT, 
VoucherType NVARCHAR(50)
);
 
 --Read All Accounting Class
DECLARE @JAccClassID TABLE
(
	AccClassID INT 
);

-- Write to temporary table @JAccClassID from xml
INSERT INTO @JAccClassID(AccClassID)
    SELECT
        IdNode.value('(.)[1]', 'int')
    FROM
        @Journal.nodes('/JOURNAL/AccClassIDSettings/AccClassID') AS IdTbl(IdNode)

-- Write from Transaction Table Where RowID is JournalID in JournalDetail
 INSERT INTO @JTransactionClass (TransactionID,RowID,VoucherType)
	 SELECT TransactionID,RowID,VoucherType FROM [Acc].[tblTransaction] WHERE RowID IN (SELECT DISTINCT JournalID FROM @JDetails )

 Delete from acc.tblTransactionClass where VoucherType='JRNL' AND RowID=@JournalID
  --Insert Accounting Class For each transaction
 INSERT INTO [Acc].[tblTransactionClass] (TransactionID, AccClassID,RowID,VoucherType)
 SELECT TransactionID, TAC.AccClassID,RowID, VoucherType FROM @JTransactionClass , @JAccClassID TAC 
   IF @@ERROR<>0 BEGIN ROLLBACK TRANSACTION RETURN -101 END
COMMIT TRANSACTION
--select @JournalID;
--return;
--EXEC sp_xml_removedocument @docHandle SELECT @JID AS [Journal ID]

GO


