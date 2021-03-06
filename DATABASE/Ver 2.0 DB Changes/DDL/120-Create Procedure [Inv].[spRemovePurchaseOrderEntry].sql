--USE [SABL001]
--GO
/****** Object:  StoredProcedure [Inv].[spRemovePurchaseOrderEntry]    Script Date: 3/8/2018 1:38:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create  PROCEDURE [Inv].[spRemovePurchaseOrderEntry]
 @PurchaseOrderID  INT
AS
	BEGIN
		SET NOCOUNT ON
	--if you delete purchase order receipt master entry it automatically delete from its detail table 
	--for this you must have insert update criteria on CASCADE
			DELETE FROM Acc.tblTransactionClass WHERE RowID = @PurchaseOrderID and VoucherType='PURCHASE_ORDER';
			DELETE FROM Acc.tblTransaction WHERE RowID = @PurchaseOrderID and VoucherType='PURCHASE_ORDER';
			DELETE FROM Inv.tblPurchaseOrderAccClass WHERE PurchaseOrderID = @PurchaseOrderID;
			DELETE FROM Inv.tblPurchaseOrderMaster WHERE PurchaseOrderID =  @PurchaseOrderID;	
			DELETE FROM Inv.tblInventoryTrans WHERE RowID = @PurchaseOrderID and VoucherType='PURCHASE_ORDER';	
			DELETE FROM System.tblDueDate WHERE RowID = @PurchaseOrderID and VoucherType='PURCHASE_ORDER';		
			
END