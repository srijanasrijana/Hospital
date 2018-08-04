

/****** Object:  StoredProcedure [Inv].[spGetPurchaseReport]    Script Date: 4/25/2017 12:07:24 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




--Modified By Ramesh Prajapati , Further Modification by Sunil Shrestha on 06-Apr-16
ALTER PROCEDURE [Inv].[spGetPurchaseReport] 
 @PurchaseLedgerID INT=null,
 @ProductGroupID INT=null,
 @ProductID INT=null,
 @PartyGroupID INT=NULL,
 @PartyID INT=NULL,
 @DepotID INT = null,
 @ProjectID INT = NULL,
 @FromDate DATETIME = NULL,
 @ToDate DATETIME = NULL,
 @PurchaseReportTypeIndex INT = NULL,
 @AccountClassIDsSettings  xml,     -- It encludes AccountCLassIDs  Info
 @ProjectIDsSettings xml           -- It includes ProjectIDs Info
  AS
DECLARE @docHandle int;
DECLARE @docHandle1 int;
EXEC sp_xml_preparedocument @docHandle OUTPUT, @AccountClassIDsSettings;
EXEC sp_xml_preparedocument @docHandle1 OUTPUT, @ProjectIDsSettings;




BEGIN
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
        @AccountClassIDsSettings.nodes('/PURCHASEREPORT/AccClassIDSettings/AccClassID') AS IdTbl(IdNode)
DECLARE @ProductSettings NVARCHAR(500)=''
DECLARE @PartySettings NVARCHAR(500)=''
DECLARE  @DepotSettings NVARCHAR(500)=''
DECLARE  @SqlStmt as NVARCHAR(Max)
DECLARE  @PurchaseLedgerSettings NVARCHAR(100)=''
DECLARE  @PurchDateRangeSettings NVARCHAR(100)=''
DECLARE  @PurchRtnDateRangeSettings NVARCHAR(100)=''
Declare @ProjectSettings nvarchar(500)=''
--Declareing temporary table for holding masterID 
DECLARE @userData TABLE(
id int NOT NULL
);

Declare @TotalAccClass nvarchar(max)
set @TotalAccClass='';

 SELECT @TotalAccClass = @TotalAccClass + convert(nvarchar,AccClassID) + N',' FROM @AccClassID;
	if(LEN(@TotalAccClass)>0)
	begin 
	SET @TotalAccClass= left(@TotalAccClass,len(@TotalAccClass)-1); --Trim one last trailing comma
	end
	else
	begin
	SET @TotalAccClass='''''';
	end
Declare @ProjectIDCollection Table
(
ProjectIDs int
);
INSERT INTO @ProjectIDCollection(ProjectIDs)
    SELECT
        IdNode.value('(.)[1]', 'int')
    FROM
        @ProjectIDsSettings.nodes('/PURCHASEREPORT/ProjectIDSettings/ProjectID') AS IdTbl(IdNode)
        
Declare @TotalProjectID nvarchar(max)
set @TotalProjectID='';

 SELECT @TotalProjectID = @TotalProjectID + convert(nvarchar,ProjectIDs) + N',' FROM @ProjectIDCollection;
	if(LEN(@TotalProjectID)>0)
	begin 
	SET @TotalProjectID= left(@TotalProjectID,len(@TotalProjectID)-1); --Trim one last trailing comma
	end
	else
	begin
	SET @TotalProjectID='''''';
	end


declare @FinalResult TABLE(
productID int NOT NULL,
productName NVARCHAR(150),
Unit NVARCHAR(50),
PurchaseRate DECIMAL(18,5),
InBound INT NOT NULL,
OutBound INT NOT NULL,
Amount DECIMAL(18,5) NOT NULL,
PartyID INT NOT NULL,
VoucherNo NVARCHAR(50),
VoucherDate DATETIME NOT NULL,
RowID INT NOT NULL,
VoucherType NVARCHAR(100)
);
--Begin Programming

--SETTINGS WORK

---*******************BLOCK FOR PURCHASE LEDGER SETTINGS**************************
		IF(@PurchaseLedgerID>0)--IF purchase ledger is selected on settings
		BEGIN
			SET @PurchaseLedgerSettings =' AND PurchaseLedgerID ='+Convert(nvarchar,@PurchaseLedgerID);
		END

---*******************END OF PURCHASE LEDGER SETTINGS**************************

---*******************BLOCK FOR DATE RANGE SETTINGS FOR PURCHASE INVOICE********************************
         IF(@FromDate IS NOT NULL AND @ToDate IS NOT NULL)
         BEGIN
			SET @PurchDateRangeSettings =' AND PurchaseInvoice_Date BETWEEN ''' +  Convert(VARCHAR,@FromDate,20) + ''' AND ''' +  Convert(VARCHAR,@ToDate,20) + '''';
         END


---******************END OF DATE RANGE SETTINGS FOR PURCHASE INVOICE***********************************

--********************BLOCK FOR DATE RANGE SETTINGS FOR PURCHSE RETURN*******************************
		  IF(@FromDate IS NOT NULL AND @ToDate IS NOT NULL)
				 BEGIN
				
					SET @PurchRtnDateRangeSettings =' AND PurchaseReturn_Date BETWEEN ''' +  Convert(VARCHAR,@FromDate,20) + ''' AND ''' +  Convert(VARCHAR,@ToDate,20) + '''';
				 END

--*********************END OF DATE RANGE SETTING FOR PURCHASE RETURN
          

---*******************BLOCK FOR PRODUCT SETTINGS********************
		--In case of productsetting,If productID is greater than zero than it is for single product setting,filter according to specific ProductID
		--If ProductGroupID is greater than zero than it is for Product Group Settings,collect corresponding ProductIDs from specific ProductGroupID and pass it on query
		--If All Product is set on setting than no need to filter by productID

		if(@ProductGroupID>0)--If setting is according to Product Group wise
		BEGIN

		create table #TempGroupID(PDGroupID int)
insert into #TempGroupID values(@ProductGroupID)
 ;with CteGroupID(groupID)
 as(select GroupID from Inv.tblProductGroup where Parent_GrpID=@ProductGroupID
 union all
 select Inv.tblProductGroup.GroupID from Inv.tblProductGroup,CteGroupID where Parent_GrpID=CteGroupID.groupID)
 insert into #TempGroupID 
 select * from CteGroupID

			SET @ProductSettings= ' AND d.ProductID IN (SELECT productID from inv.tblProduct WHERE GroupID in (select *from #TempGroupID))' ;
		END
		--ProductID will be checked below from @finalResult table if single product checkbox is selected so no need to check.
		--ELSE IF(@ProductID>0)--If setting is according to Single product wise
		--BEGIN
		--	SET @ProductSettings = ' AND d.ProductID='+CONVERT(nvarchar,@ProductID);
		--END
---*******************END OF PRODUCT SETTINGS**************************

---*******************BLOCK FOR PARTY SETTINGS*******************

		IF(@PartyGroupID>0)--if setting is according to Party Groupwise
		BEGIN

			create table #TempPTGroupID(PTGroupID int)
insert into #TempPTGroupID values(@PartyGroupID)
 ;with CtePTGroupID(groupID)
 as(select GroupID from acc.tblGroup where Parent_GrpID=@PartyGroupID
 union all
 select acc.tblGroup.GroupID from acc.tblGroup,CtePTGroupID where Parent_GrpID=CtePTGroupID.groupID)
 insert into #TempPTGroupID 
 select * from CtePTGroupID
		SET @PartySettings= ' AND CashPartyLedgerID IN (SELECT LedgerID from Acc.tblLedger WHERE GroupID in (select *from #TempPTGroupID))' ;
		END
		IF(@PartyID>0)--IF setting is according to specific party wise
		BEGIN
			SET @PartySettings = ' AND CashPartyLedgerID='+CONVERT(nvarchar,@PartyID);
		END

--*******************END OF PARTY SETTINGS***************************

--*******************BLOCK FOR DEPOT SETTINS****************************
		IF(@DepotID>0)--IF purchase ledger is selected on settings
				BEGIN
					SET @DepotSettings =' AND DepotID ='+Convert(nvarchar,@DepotID);
		END
--****************** END OF DEPOT SETTINGS******************************
   
   
   --Block For The Project Setting
   if(@ProjectID>0)
   begin
	set @ProjectSettings='and ProjectID in (' + @TotalProjectID +')';
   end
   --End For The Project Setting
--END SETTINGS WORK

--First for Purchase Invoice
SET @SqlStmt='SELECT PurchaseInvoiceID from Inv.tblPurchaseInvoiceMaster where 1=1' + @PurchaseLedgerSettings+@DepotSettings +@PartySettings+@PurchDateRangeSettings+@ProjectSettings;

INSERT INTO @userData execute sp_executesql @SqlStmt;
--Now convert the @userData resultset to comma separated ids
DECLARE @resultCSV nvarchar(max)
SET @resultCSV = '';

SELECT @resultCSV = @resultCSV + convert(nvarchar,id) + N',' FROM @userData;
if(LEN(@resultCSV)>0)
begin
	SET @resultCSV= left(@resultCSV,len(@resultCSV)-1); --Trim one last trailing comma
end
else
begin 
	SET @resultCSV='0';
end

--SET @SqlStmt='select d.ProductID,0 InBound,d.Quantity OutBound,d.Amount,m.CashPartyLedgerID PartyID,m.Voucher_No Voucher_No,m.PurchaseInvoice_Date VoucherDate,
--m.PurchaseInvoiceID RowID,''PURCH'' VoucherType  from Inv.tblPurchaseInvoiceDetails d, Inv.tblPurchaseInvoiceMaster m where d.PurchaseInvoiceID=m.PurchaseInvoiceID
--AND d.PurchaseInvoiceID IN (' + @resultCSV +')' + @ProductSettings;

SET @SqlStmt='select distinct d.ProductID,p.EngName,um.UnitName,d.PurchaseRate,d.Quantity InBound,0 OutBound,d.Amount,m.CashPartyLedgerID PartyID,m.Voucher_No Voucher_No,m.PurchaseInvoice_Date VoucherDate,
m.PurchaseInvoiceID RowID,''PURCH'' VoucherType  from Inv.tblPurchaseInvoiceDetails d, Inv.tblPurchaseInvoiceMaster m,Acc.tblTransactionClass tc

,Inv.tblProduct p, System.tblUnitMaintenance um where d.ProductID = p.ProductID and p.UnitMaintenanceID = um.UnitMaintenanceID

 and d.PurchaseInvoiceID=m.PurchaseInvoiceID and tc.rowid=m.purchaseinvoiceid and tc.Accclassid in (' + @TotalAccClass +') AND d.PurchaseInvoiceID IN (' + @resultCSV +')' + @ProductSettings;
--and tc.Accclassid=AccClassID AND d.PurchaseInvoiceID IN (' + @resultCSV +')' + @ProductSettings;

INSERT INTO @FinalResult execute sp_executesql @SqlStmt;

--------------------------
-----------------------
--Flush the @userData Table--
DELETE FROM @userData
--For Sales Return
SET @SqlStmt='select PurchaseReturnID from Inv.tblPurchaseReturnMaster where 1=1' + @PurchaseLedgerSettings+@DepotSettings+@PartySettings+@PurchRtnDateRangeSettings;
INSERT INTO @userData execute sp_executesql @SqlStmt
--For test
--Now convert the @userData resultset to comma separated ids
SET @resultCSV = '';
SELECT @resultCSV = @resultCSV + convert(nvarchar,id) + N',' FROM @userData;
----
if(LEN(@resultCSV)>0)
begin
	SET @resultCSV= left(@resultCSV,len(@resultCSV)-1); --Trim one last trailing comma
end
else
begin 
	SET @resultCSV='0';
	end
---
SET @SqlStmt='select distinct d.ProductID,p.EngName,um.UnitName,d.PurchaseRate,0 InBound ,d.Quantity OutBound,d.Amount,m.CashPartyLedgerID PartyID,m.Voucher_No Voucher_No,m.PurchaseReturn_Date VoucherDate,
m.PurchaseReturnID RowID,''PURCH_RTN'' VoucherType  from Inv.tblPurchaseReturnDetails d, Inv.tblPurchaseReturnMaster m,Acc.tblTransactionClass tc 

,Inv.tblProduct p, System.tblUnitMaintenance um where d.ProductID = p.ProductID and p.UnitMaintenanceID = um.UnitMaintenanceID

and d.PurchaseReturnID=m.PurchaseReturnID and tc.rowid=m.PurchaseReturnID
and tc.Accclassid in (' + @TotalAccClass +') AND d.PurchaseReturnID IN (' + @resultCSV +')' + @ProductSettings;


--SET @SqlStmt='select d.ProductID ProductID,d.Quantity OutBound,0 InBound,d.Amount Amount, m.CashPartyLedgerID PartyID,m.Voucher_No Voucher_No, m.PurchaseReturn_Date 
--VoucherDate,m.PurchaseReturnID RowID,''PURCH_RTN'' VoucherType from Inv.tblPurchaseReturnDetails d, Inv.tblPurchaseReturnMaster m where 
--d.PurchaseReturnID=m.PurchaseReturnID AND d.PurchaseReturnID IN (' + @resultCSV +')' + @ProductSettings
 
INSERT INTO @FinalResult execute sp_executesql @SqlStmt
--Make the Inbound Amount to negative
UPDATE @FinalResult SET Amount=Amount*-1 WHERE OutBound>0;
declare @FinalResult2 TABLE(
InBound INT NOT NULL,
OutBound INT NOT NULL,
Amount DECIMAL NOT NULL,
VoucherNo NVARCHAR(50)

);
--PartyWise
if(@PurchaseReportTypeIndex=0)--for partywise
begin
	select PartyID,l.EngName as PartyName, SUM(InBound) InBound,SUM(OutBound) OutBound,SUM(Amount) Amount from @FinalResult f inner join acc.tblLedger l on f.PartyID=l.LedgerID  GROUP BY PartyID,l.EngName;
end
ELSE IF(@PurchaseReportTypeIndex=1)--ProductWise
begin
	SELECT productID,MAX(productName) as productName,MAX(Unit) as Unit,MAX(PurchaseRate) as PurchaseRate ,SUM(InBound) InBound, SUM(OutBound) OutBound, SUM(Amount) Amount FROM @FinalResult GROUP BY productID;
end
ELSE IF(@PurchaseReportTypeIndex=2)--for productwiseTransact
BEGIN
	select productID as ProductID,productName as ProductName,InBound,OutBound,Amount,PartyID,l.EngName as PartyName,VoucherNo,VoucherDate,Date.fnEngtoNep(VoucherDate) as NepVoucherDate,RowID,VoucherType from @FinalResult  f inner join acc.tblLedger l on f.PartyID=l.LedgerID where productID =@ProductID;
END
ELSE IF(@PurchaseReportTypeIndex=3)--for Partywise Transact
BEGIN

	select productID as ProductID,productName as ProductName,InBound,OutBound,Amount,PartyID,l.EngName as PartyName,VoucherNo,VoucherDate,Date.fnEngtoNep(VoucherDate) as NepVoucherDate,RowID,VoucherType from @FinalResult  f inner join acc.tblLedger l on f.PartyID=l.LedgerID where f.PartyID =@PartyID;


--	INSERT INTO @FinalResult2 SELECT SUM(f1.InBound) InBound,SUM(f1.OutBound) OutBound,SUM(f1.Amount) Amount ,f1.VoucherNo VoucherNo FROM @FinalResult f1 WHERE f1.PartyID =@PartyID Group by f1.VoucherNo
--	--SElect * from @FinalResult2
--	declare @AnotherFinalResult TABLE(

--VoucherNo NVARCHAR(50),
--VoucherDate DATETIME NOT NULL,
--RowID INT NOT NULL,
--VoucherType nvarchar(100)
--);

--	Insert Into @AnotherFinalResult Select distinct(VoucherNo),VoucherDate,RowID,VoucherType from @FinalResult
--	--F1 date f2 details
--	SELECT f2.InBound InBound,f2.OutBound OutBound,f2.Amount Amount,f2.VoucherNo VoucherNo,f1.VoucherDate VoucherDate,f1.RowID RowID,f1.VoucherType VoucherType from  @FinalResult2 f2 left outer  Join @AnotherFinalResult f1 ON f1.VoucherNo=f2.VoucherNo ;
END
END





GO


