--USE [BENT003]
--GO
/****** Object:  StoredProcedure [Inv].[spGetSalesInvoiceDetail]    Script Date: 2017-08-16 12:36:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Anoj Kumar Shrestha
-- Create date: <2068/04/32>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [Inv].[spGetSalesInvoiceDetail] 
	@MasterID INT,
	@Language NVARCHAR(20)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	IF @Language='ENGLISH'
		SELECT  j.Code,l.EngName ProductName, l.ProductID ProductID, j.Quantity,j.SalesRate,j.Amount,j.DiscPercentage,j.Discount,j.Net_Amount,j.Electricity,j.Garbage,
		isnull(QtyUnitID,l.UnitMaintenanceID) QtyUnitID,
		l.UnitMaintenanceID UnitID ,--( select UnitName from System.tblUnitMaintenance um where um.UnitMaintenanceID = j.QtyUnitID) UnitName,
		System.fnConvertCompoundUnit(isnull(QtyUnitID,l.UnitMaintenanceID),l.UnitMaintenanceID,  j.Quantity) CurUnitQty
		FROM Inv.tblSalesInvoiceDetails j LEFT OUTER JOIN  Inv.tblProduct l ON j.ProductID=l.ProductID WHERE j.SalesInvoiceID=@MasterID

	ELSE IF @Language='NEPALI'
		SELECT j.Code, l.EngName ProductName, l.ProductID ProductID, j.Quantity,j.SalesRate, j.Amount, j.DiscPercentage,j.Discount,j.Net_Amount,j.Electricity,j.Garbage,
		isnull(QtyUnitID,l.UnitMaintenanceID) QtyUnitID,
		l.UnitMaintenanceID UnitID ,--( select UnitName from System.tblUnitMaintenance um where um.UnitMaintenanceID = l.UnitMaintenanceID) UnitName,
		System.fnConvertCompoundUnit(isnull(QtyUnitID,l.UnitMaintenanceID),l.UnitMaintenanceID,  j.Quantity) CurUnitQty
		FROM Inv.tblSalesInvoiceDetails j LEFT OUTER JOIN Inv.tblProduct l ON j.ProductID=l.ProductID WHERE j.SalesInvoiceID=@MasterID

END
