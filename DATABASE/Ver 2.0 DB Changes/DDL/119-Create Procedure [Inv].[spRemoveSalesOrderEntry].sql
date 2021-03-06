--USE [SABL001]
--GO
/****** Object:  StoredProcedure [Inv].[spRemoveSalesOrderEntry]    Script Date: 3/8/2018 10:11:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create PROCEDURE [Inv].[spRemoveSalesOrderEntry]
 @SalesOrderID  INT
AS
	BEGIN
		SET NOCOUNT ON
	
	--for this you must have insert update criteria on CASCADE
			DELETE FROM Acc.tblTransactionClass WHERE RowID = @SalesOrderID and VoucherType='SALES_ORDER';
			DELETE FROM Acc.tblTransaction WHERE RowID = @SalesOrderID and VoucherType='SALES_ORDER';
			DELETE FROM INV.tblSalesOrderDetails WHERE SalesOrderID = @SalesOrderID;
			DELETE FROM Inv.tblSalesOrderMaster  WHERE SalesOrderID  =  @SalesOrderID;	
			DELETE FROM Inv.tblInventoryTrans WHERE RowID = @SalesOrderID and VoucherType='SALES_ORDER';
			DELETE FROM System.tblDueDate WHERE RowID = @SalesOrderID and VoucherType='SALES_ORDER';		
	END