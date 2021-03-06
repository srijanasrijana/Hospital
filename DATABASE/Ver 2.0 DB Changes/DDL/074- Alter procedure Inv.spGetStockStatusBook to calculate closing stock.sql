--USE [BENT003]
--GO
/****** Object:  StoredProcedure [Inv].[spGetStockStatusBook]    Script Date: 3/9/2017 10:51:46 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [Inv].[spGetStockStatusBook]  
 @ProductGroupID INT=NULL,
 @ProductID INT=NULL,
 @Depot nvarchar(100) ,
 @AtTheEndDate DATETIME = NULL,
 @ShowZeroQuantity bit=0,
 @StockStatusTypeIndex INT =0,     --0 for Opening Stock,1 for Closing Stock,2 for AtLeastOneResult, 3 to display in list ofproduct with closing stock
 @AccountClassIDsSettings  xml,     -- It encludes AccountCLassIDs  Info
 @ProjectIDsSettings xml=null			--It includes ProjectIDs information
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
        @AccountClassIDsSettings.nodes('/STOCKSTATUS/AccClassIDSettings/AccClassID') AS IdTbl(IdNode)
        
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
	
	--Read ProjectIDs from the XML
	
DECLARE @ProjectID TABLE
(
	ProjectID INT 
);

-- Write to temporary table @JAccClassID from xml
INSERT INTO @ProjectID(ProjectID)
    SELECT
        IdNode.value('(.)[1]', 'int')
    FROM
        @ProjectIDsSettings.nodes('/STOCKSTATUS/ProjectIDSettings/ProjectID') AS IdTbl(IdNode)
        
Declare @TotalProject nvarchar(max)
set @TotalProject='';

 SELECT @TotalProject = @TotalProject + convert(nvarchar,ProjectID) + N',' FROM @ProjectID;
	if(LEN(@TotalProject)>0)
	begin 
	SET @TotalProject= left(@TotalProject,len(@TotalProject)-1); --Trim one last trailing comma
	end
	else
	begin
	SET @TotalProject='''''';
	end

Declare @TotalVoucherType nvarchar(max)
set @TotalVoucherType='''PURCH'''+','+'''SALES'''+','+'''SLS_RTN'''+','+'''PURCH_RTN''';

declare @parentaccclass nvarchar(5)
set @parentaccclass= (select top 1 AccClassID from @AccClassID)

--set @parentaccclass=[Acc].[FindRoot](@parentaccclass)


DECLARE @ProductSettings NVARCHAR(500)='' 
DECLARE  @DepotSettings NVARCHAR(500)=''
DECLARE  @AtTheEndDateSettings NVARCHAR(500)=''
DECLARE  @SqlStmt as NVARCHAR(1000)
DECLARE  @SqlStmt1 as NVARCHAR(1000)
DECLARE @ShowZeroQuantitySettings AS NVARCHAR(500)=''
Declare @AccClassSetting as nvarchar(5);
DECLARE  @PurchDateRangeSettings NVARCHAR(100)=''

DECLARE @userData TABLE(
id int NOT NULL
);

declare @FinalResult TABLE(
CreatedDate datetime not null,
productID int NOT NULL,
ProductName nvarchar(100) NOT NULL,
Quantity int  NULL,
Unit nvarchar(100)  NULL,
Depot nvarchar(100)  NULL,
OpenPurchaseRate float null,
OpenSalesRate float null
);


declare @Result TABLE(
productID int NOT NULL,
Quantity int  NULL

);
--Begin Programming

--SETTINGS WORK
---*******************BLOCK FOR PRODUCT SETTINGS********************
		--In case of productsetting,If productID is greater than zero than it is for single product setting,filter according to specific ProductID
		--If ProductGroupID is greater than zero than it is for Product Group Settings,collect corresponding ProductIDs from specific ProductGroupID and pass it on query
		--If All Product is set on setting than no need to filter by productID

		if(@ProductGroupID>0)--If setting is according to Product Group wise
		BEGIN
			SET @ProductSettings= ' AND t.ProductID IN (SELECT productID from inv.tblProduct WHERE GroupID =' + CONVERT(nvarchar,@ProductGroupID) + ')' ;
		END
		ELSE IF(@ProductID>0)--If setting is according to Single product wise
		BEGIN
			--SET @ProductSettings = '  AND t.ProductID = '+CONVERT(nvarchar,@ProductID)+'''';
			SET @ProductSettings = '  AND t.ProductID = '+CONVERT(nvarchar,@ProductID);
		END
---*******************END OF PRODUCT SETTINGS**************************

--For Accounting Class Wise Product
SET @AccClassSetting =' AND t.AccClassID = '+ CONVERT(nvarchar,@parentaccclass) ;



--*******************BLOCK FOR DEPOT SETTINS****************************
		IF(@Depot != '')--IF purchase ledger is selected on settings
				BEGIN
					SET @DepotSettings =' AND t.depotname = ''' + CONVERT(nvarchar,@Depot) + '''' ;
				END
				
--****************** END OF DEPOT SETTINGS******************************

--*******************BLOCK FOR AtThe end date SETTINS****************************
		
					SET @AtTheEndDateSettings =   Convert(date,@AtTheEndDate)  ;
					--SET @AtTheEndDateSettings =   @AtTheEndDate  ;
		
--****************** END OF DEPOT SETTINGS******************************


--*****************Block for show zero quantity settings ***********************
IF(@ShowZeroQuantity !=1 )
BEGIN
	SET @ShowZeroQuantitySettings=' AND t.QUANTITY > 0';
END


	
	
--**************** End of Show zeor quantity settings  ***************************

--For Opening Stock Status
IF(@StockStatusTypeIndex =0 )
BEGIN

set @SqlStmt='with temp as (select distinct p.Created_Date,p.ProductID,p.EngName ,p.unitmaintenanceid,oq.OpenPurchaseQty Quantity,d.DepotName,oq.OpenPurchaseRate,oq.OpenSalesRate from
  Inv.tblOpeningQuantity oq,Inv.tblProduct p left join inv.tblDepot d on p.DepotID=d.DepotID where p.ProductID=oq.ProductID and p.IsInventoryApplicable=1 and oq.AccClassID='+@parentaccclass+'
)
select t.created_date ,t.productid,t.engname,t.quantity,q.unitname,t.depotname,t.OpenPurchaseRate,t.OpenSalesRate from temp t left join system.tblUnitMaintenance q
on t.unitmaintenanceid=q.UnitMaintenanceID WHERE t.Created_Date <= '''  +@AtTheEndDateSettings  +''''    +@ShowZeroQuantitySettings 
+@DepotSettings 
+@ProductSettings 
--+@AccClassSetting


--SET @SqlStmt='with temp as (select x.productid,x.engname,x.quantity,x.created_date, x.unitmaintenanceid,y.depotname
--from inv.tblProduct x left join inv.tblDepot y
--on x.DepotID=y.DepotID
--) 
--select t.created_date ,t.productid,t.engname,t.quantity,q.unitname,t.depotname from temp t left join system.tblUnitMaintenance q
--on t.unitmaintenanceid=q.UnitMaintenanceID WHERE t.Created_Date <= '''  +@AtTheEndDateSettings  +''''    +@ShowZeroQuantitySettings 
--+@DepotSettings 
--+@ProductSettings  


INSERT INTO @FinalResult execute sp_executesql @SqlStmt
--Flush the @userData Table--

 BEGIN		
		 select * from @FinalResult
     END

END
--- end of Opening Stock Status
---------------------------------------------------------------------------------------------------------------

--For Closing Stock Status
IF(@StockStatusTypeIndex = 1 )
BEGIN
--First for Purchase Invoice

--SET @SqlStmt='select d.ProductID,0 InBound,d.Quantity OutBound,d.Amount,m.CashPartyLedgerID PartyID,m.Voucher_No Voucher_No,m.PurchaseInvoice_Date VoucherDate,
--m.PurchaseInvoiceID RowID,''PURCH'' VoucherType  from Inv.tblPurchaseInvoiceDetails d, Inv.tblPurchaseInvoiceMaster m where d.PurchaseInvoiceID=m.PurchaseInvoiceID
--AND d.PurchaseInvoiceID IN (' + @resultCSV +')' + @ProductSettings;
--where TransactDate <= '  + @AtTheEndDateSettings +'

--SET @SqlStmt='select ProductID,SUM(quantity) Quantity from(select distinct a.productid,a.quantity from (select  productid,rowid,SUM(incoming)-SUM(outgoing)-SUM(Damage) as quantity
 --from  inv.tblInventoryTrans where TransactDate <= '''  + @AtTheEndDateSettings +''' group by productid,rowid)a,
--Acc.tblTransactionClass tc where a.RowID=tc.RowID and tc.AccClassID IN ('+@TotalAccClass+'))b group by ProductID'

--SET @SqlStmt='select productid,SUM(incoming)-SUM(outgoing)-SUM(Damage) as quantity from inv.tblInventoryTrans,Acc.tblTransactionClass tc
 --where TransactDate <= '''  +@AtTheEndDateSettings   + ''' group by productid ' 
 
--Recently in Use
--SET @SqlStmt='select ProductID,SUM(quantity) Quantity from(select distinct a.productid,a.quantity from (select  productid,rowid,SUM(incoming)-SUM(outgoing)-SUM(Damage) as quantity
-- from  inv.tblInventoryTrans where TransactDate <= '''  + @AtTheEndDateSettings +''' group by productid,rowid)a,
--Acc.tblTransactionClass tc,Acc.tblTransaction T where a.RowID=tc.RowID and T.TransactionID=tc.TransactionID and t.projectID in ('+@TotalProject+') and tc.AccClassID IN ('+@TotalAccClass+'))b group by ProductID'
SET @SqlStmt='select a.productid,a.quantity from 
(select  productid,SUM(incoming)-SUM(outgoing)-SUM(Damage) as quantity
 from  inv.tblInventoryTrans it where it.TransactDate <= '''  + @AtTheEndDateSettings +''' and it.ProductID not in(select ProductID from Inv.tblProduct 
 where IsInventoryApplicable=0) and it.rowid in 
 (
 select DISTINCT tc.rowid from acc.tbltransactionclass tc,acc.tbltransaction t where
 tc.VoucherType in ('+@TotalVoucherType+') AND
 tc.AccClassID in ('+@TotalAccClass+') and tc.transactionid=t.transactionid and t.projectid in ('+@TotalProject+')
 )
  group by productid)a'


 
INSERT INTO @Result execute sp_executesql @SqlStmt
--Flush the @userData Table--
 BEGIN		
		 select * from @Result
     END

END
--- end of Closing Stock Status
----------------------------------------------------------------------------------------------------------------------------------------------------------------------
--show the final result....
if(@StockStatusTypeIndex =3)
begin 
	set @SqlStmt='with temp as (select distinct p.Created_Date,p.ProductID,p.EngName ,p.unitmaintenanceid,oq.OpenPurchaseQty Quantity,d.DepotName,oq.OpenPurchaseRate,oq.OpenSalesRate from
  Inv.tblOpeningQuantity oq,Inv.tblProduct p left join inv.tblDepot d on p.DepotID=d.DepotID where p.ProductID=oq.ProductID and p.IsInventoryApplicable=1 and oq.AccClassID='+@parentaccclass+'
)
select t.created_date ,t.productid,t.engname,t.quantity,q.unitname,t.depotname,t.OpenPurchaseRate,t.OpenSalesRate from temp t left join system.tblUnitMaintenance q
on t.unitmaintenanceid=q.UnitMaintenanceID WHERE t.Created_Date <= '''  +@AtTheEndDateSettings  +''''    +@ShowZeroQuantitySettings 
+@DepotSettings 
+@ProductSettings 
--+@AccClassSetting

INSERT INTO @FinalResult execute sp_executesql @SqlStmt

SET @SqlStmt='select a.productid,a.quantity from 
(select  productid,SUM(incoming)-SUM(outgoing)-SUM(Damage) as quantity
 from  inv.tblInventoryTrans it where it.TransactDate <= '''  + @AtTheEndDateSettings +''' and it.ProductID not in(select ProductID from Inv.tblProduct 
 where IsInventoryApplicable=0) and it.rowid in 
 (
 select DISTINCT tc.rowid from acc.tbltransactionclass tc,acc.tbltransaction t where
 tc.VoucherType in ('+@TotalVoucherType+') AND
 tc.AccClassID in ('+@TotalAccClass+') and tc.transactionid=t.transactionid and t.projectid in ('+@TotalProject+')
 )
  group by productid)a'


 
INSERT INTO @Result execute sp_executesql @SqlStmt
--Flush the @userData Table--
	 BEGIN		
		select fr.productID,fr.ProductName, (select ProductCode from Inv.tblProduct pd where pd.ProductID = fr.ProductID) as code,
		(select pg.EngName from Inv.tblProduct p inner join Inv.tblProductGroup pg on p.GroupID = pg.GroupID where p.ProductID = fr.productID) as GroupName,
		fr.OpenPurchaseRate, fr.OpenSalesRate, isnull(fr.Quantity,0) as OpeningStock, isnull(r.Quantity,0) TransactionStock, isnull(isnull(fr.Quantity,0) + isnull(r.Quantity,0),0) as ClosingStock from @FinalResult fr 
		left outer join  @Result r  on fr.productID = r.productID;
	 END


end
--For Atleast one record Status
IF(@StockStatusTypeIndex =2 )
BEGIN

SET @SqlStmt='with temp as (select x.productid,x.engname,x.quantity,x.created_date, x.unitmaintenanceid,y.depotname
from inv.tblProduct x left join inv.tblDepot y
on x.DepotID=y.DepotID
) 
select t.created_date ,t.productid,t.engname,t.quantity,q.unitname,t.depotname from temp t left join system.tblUnitMaintenance q
on t.unitmaintenanceid=q.UnitMaintenanceID WHERE t.Created_Date <= '''  +@AtTheEndDateSettings  +''''   
+ @DepotSettings 
+ @ProductSettings  
 
INSERT INTO @FinalResult execute sp_executesql @SqlStmt
--Flush the @userData Table--

 BEGIN		
		 select * from @FinalResult
     END

END
--- end of Opening Stock Status
    
end