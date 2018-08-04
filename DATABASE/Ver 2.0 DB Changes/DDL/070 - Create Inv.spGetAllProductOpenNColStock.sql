
/****** Object:  StoredProcedure [Inv].[spGetAllProductOpenNColStock]    Script Date: 2/6/2017 1:12:38 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


Create PROCEDURE [Inv].[spGetAllProductOpenNColStock]
( 
  @AtTheEndDate DATETIME = NULL,
@AccountClassIDsSettings  xml=null,     -- It encludes AccountCLassIDs  Info
 @ProjectIDsSettings xml=null			--It includes ProjectIDs information
)
as
begin 
declare @rootClassID int=1--for now (take ids from xml later)
declare @projectID int=1--for now 

select p.ProductID,p.EngName,q.OpenPurchaseQty, (q.OpenPurchaseQty+sum(t.Incoming)-sum(t.Outgoing)-sum(t.Damage)) as ClosingStock,q.OpenPurchaseRate from Inv.tblProduct as p left outer join Inv.tblOpeningQuantity as q on p.ProductID= q.ProductID
join Inv.tblInventoryTrans as t on p.ProductID=t.ProductID and q.AccClassID=1 and p.IsInventoryApplicable=1 group by p.ProductID,p.EngName,q.OpenPurchaseQty,q.OpenPurchaseRate

end

GO


