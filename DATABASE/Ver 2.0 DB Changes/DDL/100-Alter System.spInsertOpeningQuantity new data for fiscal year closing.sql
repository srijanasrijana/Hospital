
/****** Object:  StoredProcedure [System].[spInsertOpeningQuantity]    Script Date: 07/17/2017 12:02:09 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER Procedure [System].[spInsertOpeningQuantity]
(
@ProductID int, 
@AccClassID int,             
@OpenPurchaseQty    decimal(18,5),           
@OpenPurchaseRate   decimal(18,5),
@OpenSalesRate decimal(18,5)          
)
As
Begin
insert into Inv.tblOpeningQuantity(ProductID,AccClassID,OpenPurchaseQty,OpenPurchaseRate,OpenSalesRate,OpenQuantityDate)values
(@ProductID,@AccClassID,convert(float,@OpenPurchaseQty),convert(float,@OpenPurchaseRate),convert(float,@OpenSalesRate),GETDATE())
	
End;