
/****** Object:  StoredProcedure [Inv].[spGetAllProductOpenNColStock]    Script Date: 09/07/2017 9:41:10 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



alter PROCEDURE [Inv].[spGetAllProductOpenNColStock] 
( 
@FromDate datetime =null,
  @AtTheEndDate DATETIME = NULL,
@AccountClassIDsSettings  xml=null,     -- It encludes AccountCLassIDs  Info
 @ProjectIDsSettings xml=null,
 @InputProductID int=0,
 @ProductGrpID int=0,
 @OpeningStock decimal(19,5) output,--( opening balance (not quantity))
 @ClosingStock decimal(19,5) output --(closing balance (not quantity))

)
as     
begin 
declare @rootClassID int=1--for now (take ids from xml later)
declare @projectID int=1--for now 
Declare @ProductOpeningStock decimal(19,5)=0;
Declare @ProductClosingStock decimal(19,5)=0;
Declare @Unit decimal(19,5)=0;
Declare @SalesUnit decimal(19,5)=0;
Declare @Amount decimal(19,5)=0;
Declare @Rate decimal(19,5)=0;

--variable for calculating opening quantity if fromdate is provided
Declare @TotalUnitTillFromDate decimal(19,5)=0;
Declare @OpeningAmountForFromDate decimal(19,5)=0;
Declare @openingRateForFromDate decimal(19,5)=0;
Declare @salesUnitTillFromDate decimal(19,5)=0;

Declare @tblProductIDs table(pid int);
Declare @tblResult table(PID int,PName nvarchar(50),OpeningStock decimal(19,5),ClosingStock decimal(19,5),closingUnit decimal(19,5),OpeningUnit decimal(19,5),averagePurchaseRate decimal(19,5),salesRate decimal(19,5))
 
if(@AtTheEndDate is null or @AtTheEndDate='')
set @AtTheEndDate=GETDATE()

if(@FromDate is null or @FromDate='')
select @FromDate=BookBeginFrom from System.tblCompanyInfo --set from date to the very first date from where the accounting book begins

declare @productID int=0;

 if(@InputProductID<>0)
 begin
 if exists(select *from Inv.tblProduct where ProductID=@InputProductID and IsInventoryApplicable=1)
 insert into @tblProductIDs values(@InputProductID)
 --select @OpeningStock= isnull((OpenPurchaseQty*OpenPurchaseRate),0) from inv.tblOpeningQuantity where ProductID=@InputProductID;
--DECLARE db_cursor CURSOR  for select ProductID from inv.tblProduct where ProductID=@InputProductID and IsInventoryApplicable=1
end

else if(@ProductGrpID<>0)
begin
insert into @tblProductIDs select ProductID from Inv.tblProduct where GroupID=@ProductGrpID and IsInventoryApplicable=1

 ;with CteGroupID(pgroupID)
               as(select GroupID from Inv.tblProductGroup where Parent_GrpID=@ProductGrpID
               union all
               select Inv.tblProductGroup.GroupID from Inv.tblProductGroup,CteGroupID where Parent_GrpID=CteGroupID.pgroupID)

			   insert into @tblProductIDs select distinct ProductID from Inv.tblProduct where GroupID in(select *from CteGroupID) and IsInventoryApplicable=1
--select @OpeningStock= isnull( sum(OpenPurchaseQty*OpenPurchaseRate),0) from inv.tblOpeningQuantity where ProductID in(select ProductID from Inv.tblProduct where GroupID=@ProductGrpID)
--DECLARE db_cursor CURSOR  for select ProductID from inv.tblProduct where GroupID=@ProductGrpID and IsInventoryApplicable=1
end

else--(@InputProductID=0 and @ProductGrpID=0 )
begin
insert into @tblProductIDs select ProductID from inv.tblProduct where IsInventoryApplicable = 1 
--select @OpeningStock= isnull( sum(OpenPurchaseQty*OpenPurchaseRate),0) from inv.tblOpeningQuantity
--DECLARE db_cursor CURSOR  for select ProductID from inv.tblProduct where IsInventoryApplicable = 1
end


DECLARE db_cursor CURSOR  for select pid from @tblProductIDs


OPEN db_cursor FETCH NEXT FROM db_cursor into @productID    
WHILE @@FETCH_STATUS = 0   
BEGIN
select @ProductOpeningStock=isnull((OpenPurchaseQty*OpenPurchaseRate),0) from inv.tblOpeningQuantity where ProductID=@productID
select @Unit=isnull(OpenPurchaseQty,0) from  inv.tblOpeningQuantity where ProductID=@productID
set @Amount=@ProductOpeningStock;

set @TotalUnitTillFromDate=@Unit;
set @OpeningAmountForFromDate=@ProductOpeningStock;

--add all opening  and purchase unit of a product
--for calculating opening till opening date
select @TotalUnitTillFromDate=(@TotalUnitTillFromDate+isnull(sum(pd.Quantity),0)),@OpeningAmountForFromDate=@OpeningAmountForFromDate+isnull(sum(pd.Amount),0) from inv.tblPurchaseInvoiceDetails pd inner join Inv.tblPurchaseInvoiceMaster pm on pd.PurchaseInvoiceID=pm.PurchaseInvoiceID  where ProductID=@productID and pm.PurchaseInvoice_Date < @FromDate 
--for calculating closing till attheend date
select @Unit=(@Unit+isnull(sum(pd.Quantity),0)),@Amount=@Amount+isnull(sum(pd.Amount),0) from inv.tblPurchaseInvoiceDetails pd inner join Inv.tblPurchaseInvoiceMaster pm on pd.PurchaseInvoiceID=pm.PurchaseInvoiceID  where ProductID=@productID and pm.PurchaseInvoice_Date <= @AtTheEndDate

--substract unit and amount of purchase amount
--for calculating opening till from date
select @TotalUnitTillFromDate=(@TotalUnitTillFromDate-isnull(sum(Quantity),0)),@OpeningAmountForFromDate=@OpeningAmountForFromDate-isnull(sum(Amount),0) from inv.tblPurchaseReturnDetails prd inner join Inv.tblPurchaseReturnMaster prm on prd.PurchaseReturnID=prm.PurchaseReturnID where ProductID=@productID and prm.PurchaseReturn_Date < @FromDate 
--for calculating closing
select @Unit=(@unit-isnull(sum(Quantity),0)),@Amount=@Amount-isnull(sum(Amount),0) from inv.tblPurchaseReturnDetails prd inner join Inv.tblPurchaseReturnMaster prm on prd.PurchaseReturnID=prm.PurchaseReturnID where ProductID=@productID and prm.PurchaseReturn_Date <= @AtTheEndDate

if(@Unit!=0)
begin
--calculate opening rate 
if(@TotalUnitTillFromDate!=0)
set @openingRateForFromDate=@OpeningAmountForFromDate/@TotalUnitTillFromDate;
else
set @openingRateForFromDate=0;
--calculate closing rate for each product
set @Rate=@Amount/@Unit;


--for opening
select @salesUnitTillFromDate=(isnull(sum(sd.Quantity),0)) from inv.tblSalesInvoiceDetails sd inner join Inv.tblSalesInvoiceMaster sm on sd.SalesInvoiceID=sm.SalesInvoiceID where ProductID=@productID and sm.SalesInvoice_Date < @FromDate 
select @salesUnitTillFromDate=(@salesUnitTillFromDate-isnull(sum(srd.Quantity),0)) from inv.tblSalesReturnDetails srd inner join Inv.tblSalesReturnMaster srm on srd.SalesReturnID=srm.SalesReturnID where ProductID=@productID and srm.SalesReturn_Date < @FromDate 

--for closing
select @SalesUnit=(isnull(sum(sd.Quantity),0)) from inv.tblSalesInvoiceDetails sd inner join Inv.tblSalesInvoiceMaster sm on sd.SalesInvoiceID=sm.SalesInvoiceID where ProductID=@productID and sm.SalesInvoice_Date <= @AtTheEndDate
select @SalesUnit=(@SalesUnit-isnull(sum(srd.Quantity),0)) from inv.tblSalesReturnDetails srd inner join Inv.tblSalesReturnMaster srm on srd.SalesReturnID=srm.SalesReturnID where ProductID=@productID and srm.SalesReturn_Date <= @AtTheEndDate

--total opening stock
set @TotalUnitTillFromDate=@TotalUnitTillFromDate-@salesUnitTillFromDate;
--total closing stock
set @Unit= @Unit-@SalesUnit;

--for opening
select @TotalUnitTillFromDate=(@TotalUnitTillFromDate- isnull(sum(t.Damage),0)) from inv.tblInventoryTrans t where ProductID=@productID and t.TransactDate<@FromDate

--for closing
select @Unit=(@Unit- isnull(sum(t.Damage),0)) from inv.tblInventoryTrans t where ProductID=@productID and t.TransactDate<=@AtTheEndDate

set @ProductClosingStock=@Unit*@Rate
set @ProductOpeningStock=0;
set @ProductOpeningStock=@TotalUnitTillFromDate * @openingRateForFromDate;

end
insert into @tblResult
select @productID ,p.EngName ,@ProductOpeningStock,@ProductClosingStock,@Unit,@TotalUnitTillFromDate,@Rate,ISNULL(op.OpenSalesRate,0) from inv.tblProduct p left join inv.tblOpeningQuantity op on p.ProductID=op.ProductID   where p.ProductID=@productID 

      set  @ProductOpeningStock =0;
set @ProductClosingStock =0;
set @Unit =0;
set @SalesUnit =0;
set @Amount =0;
set @Rate =0;
set @productID=0;
set @openingRateForFromDate=0;
set @OpeningAmountForFromDate=0;
set @TotalUnitTillFromDate=0;
set  @salesUnitTillFromDate=0;

	FETCH NEXT FROM db_cursor INTO @productID   

END

CLOSE db_cursor   
DEALLOCATE db_cursor
select @ClosingStock=isnull(sum(ClosingStock),0) from @tblResult
select @OpeningStock=ISNULL(sum(OpeningStock),0) from @tblResult

--select  @ClosingStock= sum(col.closingstock) from (select ((max(o.OpenPurchaseQty)+sum(t.Incoming)-sum(t.Outgoing)-sum(t.Damage)) * max(o.OpenPurchaseRate)) as closingstock from inv.tblOpeningQuantity o inner join inv.tblInventoryTrans t on o.ProductID=t.ProductID where t.TransactDate<=convert(date,@AtTheEndDate) group by o.ProductID,o.AccClassID having o.AccClassID=@rootClassID)  as col
--select p.ProductID,p.EngName,q.OpenPurchaseQty, (q.OpenPurchaseQty+sum(t.Incoming)-sum(t.Outgoing)-sum(t.Damage)) as ClosingStock,q.OpenPurchaseRate from Inv.tblProduct as p left outer join Inv.tblOpeningQuantity as q on p.ProductID= q.ProductID
--join Inv.tblInventoryTrans as t on p.ProductID=t.ProductID and q.AccClassID=1 and p.IsInventoryApplicable=1 group by p.ProductID,p.EngName,q.OpenPurchaseQty,q.OpenPurchaseRate
select ROW_NUMBER() over(order by r.PID) as SN, r.PID as ProductID,r.PName as ProductName,r.OpeningStock as OpeningBalance,r.ClosingStock as ClosingBalance,r.closingUnit as ClosingQty,r.OpeningUnit as OpeningQty,r.averagePurchaseRate as AveragePurchaseRate  ,r.salesRate as SalesRate,isnull(p.InBound,0) as InBound,isnull(p.OutBound,0) as OutBound  from @tblResult r
left join
(select ProductID,sum(Incoming) as InBound,(sum(Outgoing)) as OutBound from Inv.tblInventoryTrans where  TransactDate between  @fromdate and @AtTheEndDate  group By ProductID ) p
on r.PID=p.ProductID

end








