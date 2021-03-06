--USE [BENT003]
--GO
/****** Object:  StoredProcedure [Inv].[spGetPurchaseInvoiceDetail]    Script Date: 2017-09-07 10:34:44 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Anoj Kumar Shrestha
-- Create date: <2068/04/32>
-- Description:	<Description,,>
--Modified By :Bimal Khadka
-- =============================================
ALTER PROCEDURE [Inv].[spGetPurchaseInvoiceDetail] 
	@MasterID INT,
	@Language NVARCHAR(20)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	IF @Language='ENGLISH'
		SELECT  j.Code,l.EngName ProductName,l.ProductID ProductID,j.Quantity,j.PurchaseRate,j.Amount,j.DiscPercentage,j.Discount,j.Net_Amount, isnull(j.VAT,0) VAT, isnull(j.CustomDutyPercentage,0) CustomDutyPercentage, isnull(j.CustomDuty, 0) CustomDuty, isnull(j.Freight,0) Freight, 
		isnull(QtyUnitID,l.UnitMaintenanceID) QtyUnitID,
		l.UnitMaintenanceID UnitID ,--( select UnitName from System.tblUnitMaintenance um where um.UnitMaintenanceID = j.QtyUnitID) UnitName,
		System.fnConvertCompoundUnit(isnull(QtyUnitID,l.UnitMaintenanceID),l.UnitMaintenanceID,  j.Quantity) CurUnitQty
		FROM Inv.tblPurchaseInvoiceDetails j LEFT OUTER JOIN Inv.tblProduct l ON j.ProductID=l.ProductID WHERE j.PurchaseInvoiceID=@MasterID

	ELSE IF @Language='NEPALI'
		SELECT j.Code, l.EngName ProductName,l.ProductID ProductID,j.Quantity,j.PurchaseRate, j.Amount, j.DiscPercentage,j.Discount,j.Net_Amount, isnull(j.VAT,0) VAT, isnull(j.CustomDutyPercentage,0) CustomDutyPercentage, isnull(j.CustomDuty, 0) CustomDuty, isnull(j.Freight,0) Freight, 
		isnull(QtyUnitID,l.UnitMaintenanceID) QtyUnitID,
		l.UnitMaintenanceID UnitID ,--( select UnitName from System.tblUnitMaintenance um where um.UnitMaintenanceID = j.QtyUnitID) UnitName,
		System.fnConvertCompoundUnit(isnull(QtyUnitID,l.UnitMaintenanceID),l.UnitMaintenanceID,  j.Quantity) CurUnitQty
		FROM Inv.tblPurchaseInvoiceDetails j LEFT OUTER JOIN Inv.tblProduct l ON j.ProductID=l.ProductID WHERE j.PurchaseInvoiceID=@MasterID

END
