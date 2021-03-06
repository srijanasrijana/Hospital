USE [BISH001]
GO
/****** Object:  StoredProcedure [Acc].[xmlJournalInsert]    Script Date: 06-Jul-16 12:01:52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [Acc].[xmlJournalInsert]
 @journal  xml,     -- It encludes Journal Master Info, Journal Detail Info And Accounting Class Info
 @returnId int = 0 output 
  AS
DECLARE @docHandle int, @JID int;
EXEC sp_xml_preparedocument @docHandle OUTPUT, @journal;
--Modified By Ramesh Prajapati
DECLARE @JournalTable TABLE
(
	JournalID INT,
	SeriesID INT,
	Voucher_No NVARCHAR(50), 
	Journal_Date DATETIME,
	Remarks NVARCHAR(200),
	ProjectID INT,
	Created_By NVARCHAR(50),
	Created_Date DATETIME,
	Modified_By NVARCHAR(50),
	Modified_Date DATETIME
);

BEGIN TRANSACTION

--It Inserts Journal Master Record From XML to Table
INSERT INTO [Acc].[tblJournalMaster]( SeriesID, Voucher_No, Journal_Date,Remarks,ProjectID,Created_By,Created_Date,Modified_By,Modified_Date,Field1,Field2,Field3,Field4,Field5 ) 
  SELECT  SeriesID, Voucher_No, Journal_Date, Remarks, ProjectID, Created_By, Created_Date, Modified_By, Modified_Date,Field1,Field2,Field3,Field4,Field5 
  FROM Openxml( @docHandle, '/JOURNAL/JOURNALMASTER', 2) WITH ( 
  SeriesID int,Voucher_No nvarchar(30),   Journal_Date datetime, Remarks nvarchar(200), ProjectID int, Created_By nvarchar(20), Created_Date datetime, Modified_By nvarchar(20), Modified_Date datetime, Field1 nvarchar(50),
  Field2 nvarchar(50),Field3 nvarchar(50),Field4 nvarchar(50),Field5 nvarchar(50) )
  
IF @@ERROR<>0 BEGIN ROLLBACK TRANSACTION RETURN -100 END

SET @JID = SCOPE_IDENTITY();
SET @returnId = @JID;

--Also Inserts Journal Master Record From XML to Temporary Table @JournalTable
INSERT INTO @JournalTable(JournalID, SeriesID, Voucher_No, Journal_Date,Remarks,Created_By,Created_Date,Modified_By,Modified_Date,ProjectID ) 
SELECT @JID AS JournalID, SeriesID, Voucher_No, Journal_Date, Remarks, Created_By, Created_Date, Modified_By, Modified_Date,ProjectID
  FROM Openxml( @docHandle, '/JOURNAL/JOURNALMASTER', 2) WITH (
  SeriesID int,Voucher_No nvarchar(30),   Journal_Date datetime, Remarks nvarchar(200), Created_By nvarchar(20), Created_Date datetime, Modified_By nvarchar(20), Modified_Date datetime,ProjectID int )
  
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
SELECT @JID AS JournalID, LedgerID, Amount, DrCr, Remarks
 FROM OpenXml( @docHandle, '/JOURNAL/JOURNALDETAIL/DETAIL', 2)   WITH 
  ( JournalID int,LedgerID int, Amount money, DrCr nvarchar(10), Remarks nvarchar(500) ) 
  
  -- It inserts Journal Detail Info to table at once
INSERT INTO [Acc].[tblJournalDetail] ( JournalID, LedgerID, Amount, DrCr,Remarks ) SELECT JournalID, LedgerID, Amount, DrCr,Remarks FROM @JDetails;

IF @@ERROR<>0 BEGIN ROLLBACK TRANSACTION RETURN -101 END

DECLARE @JDueDate TABLE
(
RowID int,
DueDate datetime, 
LedgerID INT, 
VoucherType nvarchar(20), 
VoucherNo NVARCHAR(50)

);
insert into @JDueDate(RowID,DueDate,LedgerID,VoucherType,VoucherNo)
SELECT @JID AS JournalID, DueDate, LedgerID, VoucherType, VoucherNo
 FROM OpenXml( @docHandle, '/JOURNAL/JOURNALDEBTORSDUEDATE/DUEDATEDETAIL', 2)   WITH 
  ( JournalID int,DUEDATE datetime,LedgerID int,VoucherType nvarchar(10),VoucherNo nvarchar(20) )  

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
SELECT @JID AS JournalID,CompDetails,MacAddress,IpAddress,Created_By,'JRNL',@griddetailvalues,Journal_Date
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
    
  --Insert Debit column in Transaction table
  INSERT INTO [Acc].[tblTransaction] (LedgerID,Debit_Amount,Credit_Amount,VoucherType,RowID,TransactDate,ProjectID)
  SELECT jd.LedgerID LedgerID, jd.Amount,0,'JRNL',jm.JournalID,jm.Journal_Date, ProjectID FROM @JournalTable jm Left outer join @JDetails jd on jm.JournalID=jd.JournalID WHERE UPPER(jd.DrCr)='DEBIT';
  
  --Insert Credit column in Transaction Table
  INSERT INTO [Acc].[tblTransaction] (LedgerID,Debit_Amount,Credit_Amount,VoucherType,RowID,TransactDate,ProjectID)
  SELECT jd.LedgerID LedgerID,0, jd.Amount,'JRNL',jm.JournalID,jm.Journal_Date,ProjectID FROM @JournalTable jm Left outer join @JDetails jd on jm.JournalID=jd.JournalID WHERE UPPER(jd.DrCr)='CREDIT';
  
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
        @Journal.nodes('/JOURNAL/ACCCLASSIDS/AccID') AS IdTbl(IdNode)

-- Write from Transaction Table Where RowID is JournalID in JournalDetail
 INSERT INTO @JTransactionClass (TransactionID,RowID,VoucherType)
	 SELECT TransactionID,RowID,VoucherType FROM [Acc].[tblTransaction] WHERE RowID IN (SELECT DISTINCT JournalID FROM @JDetails )

  --Insert Accounting Class For each transaction
 INSERT INTO [Acc].[tblTransactionClass] (TransactionID, AccClassID,RowID,VoucherType)
 SELECT TransactionID, TAC.AccClassID,RowID, VoucherType FROM @JTransactionClass , @JAccClassID TAC 
  
COMMIT TRANSACTION

EXEC sp_xml_removedocument @docHandle SELECT @JID AS [Journal ID]
