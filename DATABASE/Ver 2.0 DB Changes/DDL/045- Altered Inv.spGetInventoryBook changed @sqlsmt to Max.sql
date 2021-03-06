
/****** Object:  StoredProcedure [Inv].[spGetInventoryBook]    Script Date: 02-Sep-16 2:50:53 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



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
 @ProjectIDsSettings xml=null --It includes the projectIDs information
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
DECLARE  @DepotSettings NVARCHAR(500)=''
DECLARE  @SqlStmt as NVARCHAR(Max)
DECLARE  @PurchaseLedgerSettings NVARCHAR(100)=''
--Declareing temporary table for holding masterID 
DECLARE @userData TABLE(
id int NOT NULL
);

declare @FinalResult TABLE(
productID int NOT NULL,
InBound INT NOT NULL,
OutBound INT NOT NULL,
Amount DECIMAL NOT NULL,
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

---*******************BLOCK FOR PRODUCT SETTINGS********************
		--In case of productsetting,If productID is greater than zero than it is for single product setting,filter according to specific ProductID
		--If ProductGroupID is greater than zero than it is for Product Group Settings,collect corresponding ProductIDs from specific ProductGroupID and pass it on query
		--If All Product is set on setting than no need to filter by productID

		if(@ProductGroupID>0)--If setting is according to Product Group wise
		BEGIN
			SET @ProductSettings= ' AND ProductID IN (SELECT productID as ProductID from Inv.tblProduct WHERE GroupID=' + CONVERT(nvarchar,@ProductGroupID) + ')' ;
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
		SET @PartySettings= ' AND CashPartyLedgerID IN (SELECT LedgerID from Acc.tblLedger WHERE GroupID=' + CONVERT(nvarchar,@PartyGroupID) + ')' ;

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
   
--END SETTINGS WORK

--First for Purchase Register
IF(@InventoryBookTypeIndex=0 OR @InventoryBookTypeIndex=1)--for all,Purchase register and Stock Ledger
BEGIN
--SET @SqlStmt='SELECT PurchaseInvoiceID from Inv.tblPurchaseInvoiceMaster where 1=1' + @PurchaseLedgerSettings+@DepotSettings +@PartySettings;
SET @SqlStmt='SELECT PurchaseInvoiceID from Inv.tblPurchaseInvoiceMaster where ProjectID IN ('+@TotalProjectID+')' + @PurchaseLedgerSettings+@DepotSettings +@PartySettings;

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
-- and t.projectid in ('+@TotalProjectID+')

SET @SqlStmt='select distinct d.ProductID,d.Quantity InBound,0 OutBound,d.Amount,m.CashPartyLedgerID PartyID,m.Voucher_No Voucher_No,m.PurchaseInvoice_Date VoucherDate,
m.PurchaseInvoiceID RowID,''PURCH'' VoucherType  from Inv.tblPurchaseInvoiceDetails d, Inv.tblPurchaseInvoiceMaster m,Acc.tblTransactionClass tc where
 d.PurchaseInvoiceID=m.PurchaseInvoiceID and tc.rowid=m.purchaseinvoiceid 
and tc.Accclassid in (' + @TotalAccClass +') AND d.PurchaseInvoiceID IN (' + @resultCSV +')' + @ProductSettings ;


--SET @SqlStmt='select d.ProductID,0 InBound,d.Quantity OutBound,d.Amount,m.CashPartyLedgerID PartyID,m.Voucher_No Voucher_No,m.PurchaseInvoice_Date VoucherDate
--,m.PurchaseInvoiceID RowID,''PURCH'' VoucherType  from Inv.tblPurchaseInvoiceDetails d, Inv.tblPurchaseInvoiceMaster m where d.PurchaseInvoiceID=m.PurchaseInvoiceID 
--AND d.PurchaseInvoiceID IN (' + @resultCSV +')'+@ProductSettings ;

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
SET @SqlStmt='select PurchaseReturnID from Inv.tblPurchaseReturnMaster where ProjectID IN ('+@TotalProjectID+')' + @PurchaseLedgerSettings+@DepotSettings+@PartySettings;
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


SET @SqlStmt='select d.ProductID ProductID,d.Quantity OutBound,0 InBound,d.Amount Amount, m.CashPartyLedgerID PartyID,m.Voucher_No Voucher_No, 
m.PurchaseReturn_Date VoucherDate,m.PurchaseReturnID RowID,''PURCH_RTN'' VoucherType from Inv.tblPurchaseReturnDetails d, Inv.tblPurchaseReturnMaster m where
 d.PurchaseReturnID=m.PurchaseReturnID AND d.PurchaseReturnID IN (' + @resultCSV +')' + @ProductSettings
 
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
SET @SqlStmt='SELECT SalesInvoiceID from Inv.tblSalesInvoiceMaster where ProjectID IN ('+@TotalProjectID+')' +@DepotSettings +@PartySettings;

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

SET @SqlStmt='select distinct d.ProductID,0 InBound,d.Quantity OutBound,d.Amount,m.CashPartyLedgerID PartyID,m.Voucher_No Voucher_No,m.SalesInvoice_Date VoucherDate,
m.SalesInvoiceID RowID,''SALES'' VoucherType  from Inv.tblSalesInvoiceDetails d, Inv.tblSalesInvoiceMaster m,Acc.tblTransactionClass tc where
 d.SalesInvoiceID=m.SalesInvoiceID and tc.rowid=m.SalesInvoiceID
and tc.Accclassid in (' + @TotalAccClass +') AND d.SalesInvoiceID IN (' + @resultCSV +')' + @ProductSettings;
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
SET @SqlStmt='select SalesReturnID from Inv.tblSalesReturnMaster where ProjectID IN ('+@TotalProjectID+')' +@DepotSettings+@PartySettings;
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
SET @SqlStmt='select d.ProductID ProductID,d.Quantity OutBound,0 InBound,d.Amount Amount, m.CashPartyLedgerID PartyID,m.Voucher_No Voucher_No, m.SalesReturn_Date VoucherDate,m.SalesReturnID RowID,''SALES_RTN'' VoucherType from Inv.tblSalesReturnDetails d, Inv.tblSalesReturnMaster m where d.SalesReturnID=m.SalesReturnID AND d.SalesReturnID IN (' + @resultCSV +')' + @ProductSettings
 
INSERT INTO @FinalResult execute sp_executesql @SqlStmt

END--- end of Sales Return Register

--show the final result....
     BEGIN		
		 select * from @FinalResult
     END
 END




