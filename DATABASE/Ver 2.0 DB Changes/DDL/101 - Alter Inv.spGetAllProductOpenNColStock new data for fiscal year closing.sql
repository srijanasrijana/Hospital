

/****** Object:  StoredProcedure [Inv].[spGetAllProductOpenNColStock]    Script Date: 07/17/2017 12:30:56 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





  
  


ALTER PROCEDURE [Inv].[spGetAllProductOpenNColStock]
( 
  @AtTheEndDate DATETIME = NULL,
@AccountClassIDsSettings  xml=null,     -- It encludes AccountCLassIDs  Info
 @ProjectIDsSettings xml=null,
 @OpeningStock decimal(19,5) output,
 @ClosingStock decimal(19,5) output
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
Declare @tblResult table(PID int,PName nvarchar(50),OpeningStock decimal(19,5),ClosingStock decimal(19,5),closingUnit decimal(19,5),averagePurchaseRate decimal(19,5),salesRate decimal(19,5))
 
if(@AtTheEndDate=null or @AtTheEndDate='')
set @AtTheEndDate=GETDATE()

select @OpeningStock= isnull( sum(OpenPurchaseQty*OpenPurchaseRate),0) from inv.tblOpeningQuantity

declare @productID int=0;

DECLARE db_cursor CURSOR  for select ProductID from inv.tblProduct

OPEN db_cursor FETCH NEXT FROM db_cursor into @productID    
WHILE @@FETCH_STATUS = 0   
BEGIN
select @ProductOpeningStock=isnull((OpenPurchaseQty*OpenPurchaseRate),0) from inv.tblOpeningQuantity where ProductID=@productID
select @Unit=isnull(OpenPurchaseQty,0) from  inv.tblOpeningQuantity where ProductID=@productID
set @Amount=@ProductOpeningStock;

--add all opening  and purchase unit of a product
select @Unit=(@Unit+isnull(sum(Quantity),0)),@Amount=@Amount+isnull(sum(Amount),0) from inv.tblPurchaseInvoiceDetails where ProductID=@productID

--substract unit and amount of purchase amount
select @Unit=(@unit-isnull(sum(Quantity),0)),@Amount=@Amount-isnull(sum(Amount),0) from inv.tblPurchaseReturnDetails where ProductID=@productID

if(@Unit!=0)
begin
--calculate rate for each product
set @Rate=@Amount/@Unit

select @SalesUnit=(isnull(sum(Quantity),0)) from inv.tblSalesInvoiceDetails where ProductID=@productID
select @SalesUnit=(@SalesUnit-isnull(sum(Quantity),0)) from inv.tblSalesReturnDetails where ProductID=@productID

--total closing stock
set @Unit= @Unit-@SalesUnit;

select @Unit=(@Unit- isnull(sum(Damage),0)) from inv.tblInventoryTrans where ProductID=@productID

set @ProductClosingStock=@Unit*@Rate
end
insert into @tblResult
select @productID ,p.EngName ,@ProductOpeningStock,@ProductClosingStock,@Unit,@Rate,op.OpenSalesRate from inv.tblProduct p inner join inv.tblOpeningQuantity op on p.ProductID=op.ProductID   where p.ProductID=@productID

      set  @ProductOpeningStock =0;
set @ProductClosingStock =0;
set @Unit =0;
set @SalesUnit =0;
set @Amount =0;
set @Rate =0;
set @productID=0;

	FETCH NEXT FROM db_cursor INTO @productID   

END

CLOSE db_cursor   
DEALLOCATE db_cursor
select @ClosingStock=isnull(sum(ClosingStock),0) from @tblResult

--select  @ClosingStock= sum(col.closingstock) from (select ((max(o.OpenPurchaseQty)+sum(t.Incoming)-sum(t.Outgoing)-sum(t.Damage)) * max(o.OpenPurchaseRate)) as closingstock from inv.tblOpeningQuantity o inner join inv.tblInventoryTrans t on o.ProductID=t.ProductID where t.TransactDate<=convert(date,@AtTheEndDate) group by o.ProductID,o.AccClassID having o.AccClassID=@rootClassID)  as col
--select p.ProductID,p.EngName,q.OpenPurchaseQty, (q.OpenPurchaseQty+sum(t.Incoming)-sum(t.Outgoing)-sum(t.Damage)) as ClosingStock,q.OpenPurchaseRate from Inv.tblProduct as p left outer join Inv.tblOpeningQuantity as q on p.ProductID= q.ProductID
--join Inv.tblInventoryTrans as t on p.ProductID=t.ProductID and q.AccClassID=1 and p.IsInventoryApplicable=1 group by p.ProductID,p.EngName,q.OpenPurchaseQty,q.OpenPurchaseRate
select PID as productID,PName as ProductName,OpeningStock,ClosingStock as totalClosingStock,closingUnit as ClosingStock,averagePurchaseRate as AveragePurchaseRate  ,salesRate as SalesRate   from @tblResult


end








GO


