
/****** Object:  StoredProcedure [Inv].[spGetSalesReport]    Script Date: 27-Apr-16 9:50:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:Anoj Kumar Shrestha
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- Modified By: Bimal Bahadur Khadka
-- =============================================
--Modified by Ramesh Prajapati
--Modified by: Sunil on 15th April 2016, 27th April 2016
ALTER PROCEDURE [Inv].[spGetSalesReport] 
 @SalesLedgerID INT=null,
 @ProductGroupID INT=null,
 @ProductID INT=null,
 @PartyGroupID INT=NULL,
 @PartyID INT=NULL,
 @DepotID INT = null,
 @ProjectID INT = NULL,
 @FromDate DATETIME = NULL,
 @ToDate DATETIME = NULL,
 @SalesReportTypeIndex INT = NULL,
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
        @AccountClassIDsSettings.nodes('/SALESREPORT/AccClassIDSettings/AccClassID') AS IdTbl(IdNode)
DECLARE @ProductSettings NVARCHAR(500)=''
DECLARE @PartySettings NVARCHAR(500)=''
DECLARE  @DepotSettings NVARCHAR(500)=''
DECLARE  @SqlStmt NVARCHAR(MAX) --This must be max so that there will be no problem as the data increases
DECLARE  @SalesLedgerSettings NVARCHAR(100)=''
DECLARE  @SalesDateRangeSettings NVARCHAR(100)=''
DECLARE  @SalesRtnDateRangeSettings NVARCHAR(100)=''
--Declareing temporary table for holding masterID 
DECLARE @userData TABLE(
id int NOT NULL
);
declare @FinalResult TABLE(
productID int NOT NULL,
productName NVARCHAR(150),
Unit NVARCHAR(50),
SalesRate DECIMAL(18,5),
InBound INT NOT NULL,
OutBound INT NOT NULL,
Amount DECIMAL(18,5) NOT NULL,
PartyID INT NOT NULL,
VoucherNo NVARCHAR(50),
VoucherDate DATETIME NOT NULL,
RowID INT NOT NULL,
VoucherType NVARCHAR(100)
--productID int ,
--InBound INT ,
--OutBound INT ,
--Amount DECIMAL ,
--PartyID INT ,
--VoucherNo NVARCHAR(50),
--VoucherDate DATETIME ,
--RowID INT ,
--VoucherType NVARCHAR(100)
);
--Begin Programming

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

Declare @ProjectSettings nvarchar(500)=''
Declare @ProjectIDCollection Table
(
ProjectIDs int
);
INSERT INTO @ProjectIDCollection(ProjectIDs)
    SELECT
        IdNode.value('(.)[1]', 'int')
    FROM
        @ProjectIDsSettings.nodes('/SALESREPORT/ProjectIDSettings/ProjectID') AS IdTbl(IdNode)
        
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
--SETTINGS WORK

---*******************BLOCK FOR SALES LEDGER SETTINGS**************************
		IF(@SalesLedgerID>0)--IF sales ledger is selected on settings
		BEGIN
			SET @SalesLedgerSettings =' AND SalesLedgerID ='+Convert(nvarchar,@SalesLedgerID);
		END

---*******************END OF SALES LEDGER SETTINGS**************************

---*******************BLOCK FOR DATE RANGE SETTINGS FOR SALES INVOICE********************************
         IF(@FromDate IS NOT NULL AND @ToDate IS NOT NULL)
         BEGIN
			SET @SalesDateRangeSettings ='AND SalesInvoice_Date BETWEEN ''' +  Convert(VARCHAR,@FromDate,20) + ''' AND ''' +  Convert(VARCHAR,@ToDate,20) + '''';
         END


---******************END OF DATE RANGE SETTINGS FOR PURCHASE INVOICE***********************************

--********************BLOCK FOR DATE RANGE SETTINGS FOR SALES RETURN*******************************
		  IF(@FromDate IS NOT NULL AND @ToDate IS NOT NULL)
				 BEGIN
					SET @SalesRtnDateRangeSettings ='AND SalesReturn_Date BETWEEN ''' +  Convert(VARCHAR,@FromDate,20) + ''' AND ''' +  Convert(VARCHAR,@ToDate,20) + '''';
				 END

--*********************END OF DATE RANGE SETTING FOR PURCHASE RETURN

 --Block For The Project Setting
   if(@ProjectID>0)
   begin
	set @ProjectSettings='and ProjectID in (' + @TotalProjectID +')';
   end
   --End For The Project Setting

---*******************BLOCK FOR PRODUCT SETTINGS********************
		--In case of productsetting,If productID is greater than zero than it is for single product setting,filter according to specific ProductID
		--If ProductGroupID is greater than zero than it is for Product Group Settings,collect corresponding ProductIDs from specific ProductGroupID and pass it on query
		--If All Product is set on setting than no need to filter by productID

		if(@ProductGroupID>0)--If setting is according to Product Group wise
		BEGIN
			SET @ProductSettings= ' AND d.ProductID IN (SELECT productID from inv.tblProduct WHERE GroupID=' + CONVERT(nvarchar,@ProductGroupID) + ')' ;
		END
		--ProductID will be checked below from @finalResult table if single product checkbox is selected so no need to check.
		--ELSE IF(@ProductID>0)--If setting is according to Single product wise
		--BEGIN
		--	SET @ProductSettings = ' AND ProductID='+CONVERT(nvarchar,@ProductID);
		--END
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
		IF(@DepotID>0)--IF SALES ledger is selected on settings
				BEGIN
					SET @DepotSettings =' AND DepotID ='+Convert(nvarchar,@DepotID);
		END
--****************** END OF DEPOT SETTINGS******************************
   
--END SETTINGS WORK

--First for SALES Invoice
SET @SqlStmt='SELECT SalesInvoiceID from Inv.tblSalesInvoiceMaster where 1=1' + @SalesLedgerSettings+@DepotSettings +@PartySettings+@SalesDateRangeSettings+@ProjectSettings;

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

--SET @SqlStmt='select d.ProductID,0 InBound,d.Quantity OutBound,d.Amount,m.CashPartyLedgerID PartyID,m.Voucher_No Voucher_No,m.SalesInvoice_Date VoucherDate,
--m.SalesInvoiceID RowID,''SALES'' VoucherType  from Inv.tblSalesInvoiceDetails d, Inv.tblSalesInvoiceMaster m where d.SalesInvoiceID=m.SalesInvoiceID AND 
--d.SalesInvoiceID IN (' + @resultCSV +')' + @ProductSettings;
SET @SqlStmt='select distinct d.ProductID,p.EngName,um.UnitName,d.SalesRate,0 InBound,d.Quantity OutBound,d.Amount,m.CashPartyLedgerID PartyID,m.Voucher_No Voucher_No,m.SalesInvoice_Date VoucherDate,
m.SalesInvoiceID RowID,''SALES'' VoucherType  from Inv.tblSalesInvoiceDetails d, Inv.tblSalesInvoiceMaster m,Acc.tblTransactionClass tc

,Inv.tblProduct p, System.tblUnitMaintenance um where d.ProductID = p.ProductID and p.UnitMaintenanceID = um.UnitMaintenanceID

and d.SalesInvoiceID=m.SalesInvoiceID and tc.rowid=m.SalesInvoiceID
and tc.Accclassid in (' + @TotalAccClass +') AND d.SalesInvoiceID IN (' + @resultCSV +')' + @ProductSettings;

INSERT INTO @FinalResult execute sp_executesql @SqlStmt;


--------------------------
-----------------------
--Flush the @userData Table--
DELETE FROM @userData
--For Sales Return
--SET @SqlStmt='select SalesReturnID from Inv.tblSalesReturnMaster where 1=1' + @SalesLedgerSettings+@DepotSettings+@PartySettings+@SalesRtnDateRangeSettings+@ProductSettings;
SET @SqlStmt='select SalesReturnID from Inv.tblSalesReturnMaster where 1=1' + @SalesLedgerSettings+@DepotSettings+@PartySettings+@SalesRtnDateRangeSettings++@ProjectSettings;
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
SET @SqlStmt='select distinct d.ProductID,p.EngName,um.UnitName,d.SalesRate,d.Quantity InBound,0 OutBound,d.Amount Amount,m.CashPartyLedgerID PartyID,m.Voucher_No Voucher_No,
m.SalesReturn_Date VoucherDate,
m.SalesReturnID RowID,''SLS_RTN'' VoucherType  from Inv.tblSalesReturnDetails d, Inv.tblSalesReturnMaster m,Acc.tblTransactionClass tc

,Inv.tblProduct p, System.tblUnitMaintenance um where d.ProductID = p.ProductID and p.UnitMaintenanceID = um.UnitMaintenanceID

and d.SalesReturnID=m.SalesReturnID and tc.rowid=m.SalesReturnID
and tc.Accclassid in (' + @TotalAccClass +') AND d.SalesReturnID IN (' + @resultCSV +')' + @ProductSettings;

--SET @SqlStmt='select d.ProductID ProductID,d.Quantity InBound,0 OutBound,d.Amount Amount, m.CashPartyLedgerID PartyID,m.Voucher_No Voucher_No, 
--m.SalesReturn_Date VoucherDate,m.SalesReturnID RowID,''SLS_RTN'' VoucherType from Inv.tblSalesReturnDetails d, Inv.tblSalesReturnMaster m where 
--d.SalesReturnID=m.SalesReturnID AND d.SalesReturnID IN (' + @resultCSV +')' + @ProductSettings
 
INSERT INTO @FinalResult execute sp_executesql @SqlStmt

--select * from @FinalResult
--Make the Inbound Amount to negative
UPDATE @FinalResult SET Amount=Amount*-1 WHERE InBound>0;
declare @FinalResult2 TABLE(
InBound INT NOT NULL,
OutBound INT NOT NULL,
Amount DECIMAL NOT NULL,
VoucherNo NVARCHAR(50)

);

--PartyWise
if(@SalesReportTypeIndex=0)--for partywise
begin
	select PartyID, SUM(InBound) InBound,SUM(OutBound) OutBound,SUM(Amount) Amount from @FinalResult GROUP BY PartyID;
end
ELSE IF(@SalesReportTypeIndex=1)--ProductWise
begin
	SELECT productID,MAX(productName) as productName,MAX(Unit) as Unit,MAX(SalesRate) as SalesRate,SUM(InBound) InBound, SUM(OutBound) OutBound, SUM(Amount) Amount FROM @FinalResult GROUP BY productID
end
ELSE IF(@SalesReportTypeIndex=2)--for productwiseTransact
BEGIN
	select * from @FinalResult where productID =@ProductID
END
ELSE IF(@SalesReportTypeIndex=3)--for Partywise Transact
BEGIN
	INSERT INTO @FinalResult2 SELECT SUM(f1.InBound) InBound,SUM(f1.OutBound) OutBound,SUM(f1.Amount) Amount ,f1.VoucherNo VoucherNo FROM @FinalResult f1 WHERE f1.PartyID =@PartyID Group by f1.VoucherNo
	--SElect * from @FinalResult2
	declare @AnotherFinalResult TABLE(

VoucherNo NVARCHAR(50),
VoucherDate DATETIME NOT NULL,
RowID INT NOT NULL,
VoucherType nvarchar(100)


);

	Insert Into @AnotherFinalResult Select distinct(VoucherNo),VoucherDate,RowID,VoucherType from @FinalResult
	--F1 date f2 details
	SELECT f2.InBound InBound,f2.OutBound OutBound,f2.Amount Amount,f2.VoucherNo VoucherNo,f1.VoucherDate VoucherDate,f1.RowID RowID,f1.VoucherType VoucherType from  @FinalResult2 f2 left outer  Join @AnotherFinalResult f1 ON f1.VoucherNo=f2.VoucherNo ;
END
END
