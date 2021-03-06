--USE [SABL001]
--GO
/****** Object:  StoredProcedure [Inv].[spGetInventoryBook]    Script Date: 4/23/2018 12:11:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- Batch submitted through debugger: SQLQuery3.sql|7|0|C:\Users\srijana\AppData\Local\Temp\~vs8C01.sql
-- Batch submitted through debugger: SQLQuery2.sql|7|0|C:\Users\srijana\AppData\Local\Temp\~vsD271.sql

ALTER PROCEDURE [Inv].[spGetInventoryBook]
 @PurchaseLedgerID INT=null,
 @ProductGroupID INT=NULL,
 @ProductID INT=NULL,
 @PartyGroupID INT=NULL,
 @PartyID INT=NULL,
 @DepotID INT = null,
 @ProjectID INT = NULL,
 @FromDate DATETIME = NULL,
 @ToDate DATETIME = NULL,
 @InventoryBookTypeIndex INT =0,--0 for All(daybook),1 for Purch,2 for Purch_Rtn,3 for Sales,4 for sales_Rtn,5 for stock Ledger
 --PASS O index for Stock Ledger ...and ProductID should contain certain value
 -----------------------------------------------
 @AccountClassIDsSettings  xml,    -- It encludes AccountCLassIDs  Info
 @ProjectIDsSettings xml=null, --It includes the projectIDs information 
@OpeningQuantity decimal(19,5) output
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
        @AccountClassIDsSettings.nodes('/INVENTORYBOOK/AccClassIDSettings/AccClassID') AS IdTbl(IdNode)

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
	--Read All The Project IDS
	
DECLARE @ProjectIDs TABLE
(
	ProjectID INT 
);

-- Write to temporary table @JAccClassID from xml
INSERT INTO @ProjectIDs(ProjectID)
    SELECT
        IdNode.value('(.)[1]', 'int')
    FROM
        @ProjectIDsSettings.nodes('/INVENTORYBOOK/ProjectIDSettings/ProjectID') AS IdTbl(IdNode)

Declare @TotalProjectID nvarchar(max)
set @TotalProjectID='';

 SELECT @TotalProjectID = @TotalProjectID + convert(nvarchar,ProjectID) + N',' FROM @ProjectIDs;
	if(LEN(@TotalProjectID)>0)
	begin 
	SET @TotalProjectID= left(@TotalProjectID,len(@TotalProjectID)-1); --Trim one last trailing comma
	end
	else
	begin
	SET @TotalProjectID='''''';
	end
	--Declare @ProjectSettingsID nvarchar(500)='';
	--if @TotalProjectID<>''
	--begin 
	
	--SET @ProjectSettingsID =' AND ProjectID  in (@TotalProjectID)';
	--end
	
DECLARE @ProductSettings NVARCHAR(500)=''
DECLARE @PartySettings NVARCHAR(500)=''
DECLARE @DepotSettings NVARCHAR(500)=''
DECLARE @SqlStmt as NVARCHAR(Max)
DECLARE @PurchaseLedgerSettings NVARCHAR(100)=''
--Declareing temporary table for holding masterID 
DECLARE @userData TABLE(
id int NOT NULL
);

declare @FinalResult TABLE(
SN int,
productID int NOT NULL,
ProductName NVARCHAR(100),
InBoundQty INT NOT NULL,
OutBoundQty INT NOT NULL,
Amount DECIMAL NOT NULL,
PartyID INT NOT NULL,
ProductOrParty NVARCHAR(100),
VoucherNo NVARCHAR(50),
Date nvarchar(50),
RowID INT NOT NULL,
VoucherType NVARCHAR(200),
NepDate nvarchar(50)
);
--Begin Programming

--SETTINGS WORK

---*******************BLOCK FOR PURCHASE LEDGER SETTINGS**************************
		IF(@PurchaseLedgerID>0)--IF purchase ledger is selected on settings
		BEGIN
			SET @PurchaseLedgerSettings =' AND PurchaseLedgerID ='+Convert(nvarchar,@PurchaseLedgerID);
		END

---*******************END OF PURCHASE LEDGER SETTINGS**************************

---*******************BLOCK FOR PRODUCT SETTINGS********************
		--In case of productsetting,If productID is greater than zero than it is for single product setting,filter according to specific ProductID
		--If ProductGroupID is greater than zero than it is for Product Group Settings,collect corresponding ProductIDs from specific ProductGroupID and pass it on query
		--If All Product is set on setting than no need to filter by productID

		if(@ProductGroupID>0)--If setting is according to Product Group wise
		BEGIN
			--SET @ProductSettings= ' AND ProductID  IN (SELECT productID as ProductID from Inv.tblProduct WHERE GroupID=' + CONVERT(nvarchar,@ProductGroupID) + ')' ;
			SET @ProductSettings= ' AND ProductID=' + CONVERT(nvarchar,@ProductGroupID) ;
			RAISERROR(@ProductSettings,16,1);
		END
		ELSE IF(@ProductID>0)--If setting is according to Single product wise
		BEGIN
			SET @ProductSettings = ' AND ProductID='+CONVERT(nvarchar,@ProductID);
		END
---*******************END OF PRODUCT SETTINGS**************************

---*******************BLOCK FOR PARTY SETTINGS*******************

		IF(@PartyGroupID>0)--if setting is according to Party Groupwise
		BEGIN
		----SET @PartySettings= ' AND CashPartyLedgerID IN (SELECT LedgerID from Acc.tblLedger WHERE GroupID=' + CONVERT(nvarchar,@PartyGroupID) + ')' ;
			SET @PartySettings= ' AND CashPartyLedgerID=' + CONVERT(nvarchar,@PartyGroupID);

		END
		IF(@PartyID>0)--IF setting is according to specific party wise
		BEGIN
			SET @PartySettings = ' AND CashPartyLedgerID='+CONVERT(nvarchar,@PartyID);
		END

--*******************END OF PARTY SETTINGS***************************

--*******************BLOCK FOR DEPOT SETTINS****************************
		IF(@DepotID>0)--IF purchase ledger is selected on settings
				BEGIN
					SET @DepotSettings =' AND m.DepotID ='+Convert(nvarchar,@DepotID);
		END
--****************** END OF DEPOT SETTINGS******************************
   
--END SETTINGS WORK

--First for Purchase Register
IF(@InventoryBookTypeIndex=0 OR @InventoryBookTypeIndex=1)--for all,Purchase register and Stock Ledger
BEGIN


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
	SET @resultCSV='''''';
end



SET @SqlStmt='select distinct ROW_NUMBER() over(order by p.EngName) as SN,  d.ProductID,p.EngName ProductName,d.Quantity InBoundQty,0 OutBoundQty,d.Amount,
m.CashPartyLedgerID PartyID,l.EngName ProductOrParty,m.Voucher_No VoucherNo,
convert( varchar, m.PurchaseInvoice_Date, 111) Date, 
m.PurchaseInvoiceID RowID,''PURCH'' VoucherType,Date.fnEngToNep(m.PurchaseInvoice_Date) NepDate
  from
   Inv.tblPurchaseInvoiceDetails d inner join Inv.tblPurchaseInvoiceMaster m on d.PurchaseInvoiceID=m.PurchaseInvoiceID
   join Acc.tblLedger l on l.LedgerID = m.CashPartyLedgerID
   join Inv.tblProduct p on p.ProductID=d.ProductID
   join Inv.tblProductGroup pg on pg.GroupID=p.GroupID
   WHERE
  ProjectID in ('+@TotalProjectID+') ' 
	
	+@DepotSettings
	 + @PartySettings +@ProductSettings


INSERT INTO @FinalResult execute sp_executesql @SqlStmt;

--------------------------
--Flush the @userData Table--
DELETE FROM @userData

END
-----------------------End of Purchase Register
---------------------------
--For Purchase Return Register
IF(@InventoryBookTypeIndex =0 OR @InventoryBookTypeIndex=2)--For All,Purchase Register and Stock Ledger
BEGIN
--SET @SqlStmt='select PurchaseReturnID from Inv.tblPurchaseReturnMaster where ProjectID IN ('+@TotalProjectID+')' + @PurchaseLedgerSettings+@DepotSettings+@PartySettings;
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
	SET @resultCSV='''''';
	end

SET @SqlStmt='select   ROW_NUMBER() over(order by p.EngName) as SN,  d.ProductID,p.EngName ProductName,0 InBoundQty,d.Quantity OutBoundQty,d.Amount,
m.CashPartyLedgerID PartyID,l.EngName ProductOrParty,m.Voucher_No VoucherNo,
convert( varchar, m.PurchaseReturn_Date, 111) Date, 
m.PurchaseReturnID RowID,''PURCH_RTN'' VoucherType,Date.fnEngToNep(m.PurchaseReturn_Date) NepDate
  from
    Inv.tblPurchaseReturnDetails d inner join Inv.tblPurchaseReturnMaster m on d.PurchaseReturnID=m.PurchaseReturnID
 join Acc.tblLedger l on l.LedgerID = m.CashPartyLedgerID
 join Inv.tblProduct p on p.ProductID=d.ProductID
 join Inv.tblProductGroup pg on pg.GroupID=p.GroupID
   WHERE
  ProjectID in ('+@TotalProjectID+') ' 
	
	+@DepotSettings 
	+ @PartySettings +@ProductSettings


 
INSERT INTO @FinalResult execute sp_executesql @SqlStmt
--Flush the @userData Table--
DELETE FROM @userData
END
--- end of Purchase Return Register
---------------------------------------------------------------------------------------------------------------
-- for Sales Register
IF(@InventoryBookTypeIndex=0 OR @InventoryBookTypeIndex=3)--for all,sales register and stock ledger
BEGIN 
--SET @SqlStmt='SELECT SalesInvoiceID from Inv.tblSalesInvoiceMaster where 1=1' +@DepotSettings +@PartySettings;
--SET @SqlStmt='SELECT SalesInvoiceID from Inv.tblSalesInvoiceMaster where ProjectID IN ('+@TotalProjectID+')' +@DepotSettings +@PartySettings;

INSERT INTO @userData execute sp_executesql @SqlStmt;

SET @resultCSV = '';

SELECT @resultCSV = @resultCSV + convert(nvarchar,id) + N',' FROM @userData;
if(LEN(@resultCSV)>0)
begin
	SET @resultCSV= left(@resultCSV,len(@resultCSV)-1); --Trim one last trailing comma
end
else
begin 
	SET @resultCSV='''''';
end


SET @SqlStmt='select distinct ROW_NUMBER() over(order by p.EngName) as SN, d.ProductID,
p.EngName ProductName, 0 InBoundQty,d.Quantity OutBoundQty,
d.Amount,m.CashPartyLedgerID PartyID,l.EngName ProductOrParty,m.Voucher_No  VoucherNo,
convert( varchar, m.SalesInvoice_Date, 111) Date, m.SalesInvoiceID RowID, ''SALES'' VoucherType,
  Date.fnEngToNep(m.SalesInvoice_Date) NepDate
from  
Inv.tblSalesInvoiceDetails d inner join Inv.tblSalesInvoiceMaster m on d.SalesInvoiceID=m.SalesInvoiceID
join Acc.tblLedger l on l.LedgerID = m.CashPartyLedgerID
join Inv.tblProduct p on p.ProductID=d.ProductID
join Inv.tblProductGroup pg on pg.GroupID=p.GroupID

  where
	ProjectID in ('+@TotalProjectID+') ' 
	
	+@DepotSettings + @PartySettings +@ProductSettings


--SET @SqlStmt='select d.ProductID,0 InBound,d.Quantity OutBound,d.Amount,m.CashPartyLedgerID PartyID,m.Voucher_No Voucher_No,m.SalesInvoice_Date VoucherDate,m.SalesInvoiceID RowID,''SALES'' VoucherType  from Inv.tblSalesInvoiceDetails d, Inv.tblSalesInvoiceMaster m where d.SalesInvoiceID=m.SalesInvoiceID AND d.SalesInvoiceID IN (' + @resultCSV +')' + @ProductSettings;

INSERT INTO @FinalResult execute sp_executesql @SqlStmt;

--Flush the @userData Table--
DELETE FROM @userData

END

--End of Sales Register
---------------------------
--For Sales Return Register
IF(@InventoryBookTypeIndex=0 OR @InventoryBookTypeIndex=4)--for all,Sales return register and Stock ledger
BEGIN
--SET @SqlStmt='select SalesReturnID from Inv.tblSalesReturnMaster where ProjectID IN ('+@TotalProjectID+')' +@DepotSettings+@PartySettings;
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
	SET @resultCSV='''''';
	end

SET @SqlStmt='select  ROW_NUMBER() over(order by p.EngName) as SN,d.ProductID ProductID,p.EngName ProductName,  d.Quantity InBoundQty,0 OutBoundQty,
d.Amount Amount, m.CashPartyLedgerID PartyID, l.EngName ProductOrParty,
m.Voucher_No VoucherNo,
convert( varchar, m.SalesReturn_Date, 111) Date, 
m.SalesReturnID RowID,''SALES_RTN'' VoucherType ,Date.fnEngToNep(m.SalesReturn_Date) NepDate
 from 
 Inv.tblSalesReturnDetails d inner join Inv.tblSalesReturnMaster m on d.SalesReturnID=m.SalesReturnID
 join Acc.tblLedger l on l.LedgerID = m.CashPartyLedgerID
 join Inv.tblProduct p on p.ProductID=d.ProductID
 join Inv.tblProductGroup pg on pg.GroupID=p.GroupID
 where
 ProjectID in ('+@TotalProjectID+') ' 

+@DepotSettings + @PartySettings +@ProductSettings
 
INSERT INTO @FinalResult execute sp_executesql @SqlStmt

END--- end of Sales Return Register
set NoCount on;
select @OpeningQuantity=sum(OpenPurchaseQty) from Inv.tblOpeningQuantity

--show the final result....
     BEGIN		
		 select * from @FinalResult
     END
 END




