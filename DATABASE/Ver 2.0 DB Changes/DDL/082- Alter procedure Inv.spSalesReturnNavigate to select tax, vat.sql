--USE [BENT003]
--GO
/****** Object:  StoredProcedure [Inv].[spSalesReturnNavigate]    Script Date: 4/4/2017 10:03:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Anoj Kumar Shrestha
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- Modified By:		Bimal Bahadur Khadka
-- =============================================
ALTER PROCEDURE [Inv].[spSalesReturnNavigate] 
	@CurrentID INT,
	@NavigateTo NVARCHAR(20)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	IF(@NavigateTo='PREV')
	BEGIN
		-- Get 1 step previous voucher no.
		SELECT @CurrentID=MAX(SalesReturnID) FROM Inv.tblSalesReturnMaster WHERE SalesReturnID<@CurrentID
	END 
	IF(@NavigateTo='NEXT')
	BEGIN
		-- Get 1 step previous voucher no.
		SELECT @CurrentID=MIN(SalesReturnID) FROM Inv.tblSalesReturnMaster WHERE SalesReturnID>@CurrentID
	END 
	IF(@NavigateTo='FIRST')
	BEGIN
		-- Get 1 step previous voucher no.
		SELECT @CurrentID=MIN(SalesReturnID) FROM Inv.tblSalesReturnMaster --WHERE SalesReturnID<@CurrentID
	END 
	IF(@NavigateTo='LAST')
	BEGIN
		-- Get 1 step previous voucher no.
		SELECT @CurrentID=MAX(SalesReturnID) FROM Inv.tblSalesReturnMaster --WHERE SalesReturnID>@CurrentID
	END 


	SELECT SalesReturnID, SeriesID,SalesLedgerID,CashPartyLedgerID,DepotID,Order_No, Voucher_No, SalesReturn_Date, Remarks,Net_Amount,Total_Amt,Total_Qty,SpecialDiscount,Gross_Amount,Field1,Field2,Field3,Field4,Field5,VAT, Tax1, Tax2, Tax3 FROM Inv.tblSalesReturnMaster WHERE SalesReturnID=@CurrentID
	

END
