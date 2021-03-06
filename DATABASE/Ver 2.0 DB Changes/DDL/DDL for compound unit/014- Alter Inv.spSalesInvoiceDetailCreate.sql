--USE [BENT003]
--GO
/****** Object:  StoredProcedure [Inv].[spSalesInvoiceDetailCreate]    Script Date: 2017-08-15 3:04:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [Inv].[spSalesInvoiceDetailCreate](
@SalesInvoiceID NVARCHAR(50),
@Code NVARCHAR(50),
@ProductName NVARCHAR(30)=null,
@ProductID INT=0,
@Language NVARCHAR(30),
@Quantity Float,
@SalesRate MONEY,
@Amount MONEY,
@DiscPercentage MONEY,
@Discount MONEY,
@Net_Amount MONEY,
@QtyUnitID int = null,
@Return NVARCHAR(20) OUTPUT
)
AS

SET NOCOUNT ON

IF(@ProductID=0)
BEGIN --IF PRODUCT NAME IS GIVEN
--Product ID GETTING CODE
--	SET @ServicesID=null
	IF @Language='ENGLISH'
		SELECT @ProductID=ProductID FROM Inv.tblProduct WHERE EngName=@ProductName
	ELSE IF @Language='NEPALI'
		SELECT @ProductID=ProductID FROM Inv.tblProduct WHERE NepName=@ProductName
END	

	if @ProductID>0
	begin
--DO THE REAL INSERT
-- convert the quatity into default unit
-- even if the quantity is in default unit, it does not do any harm
	--select @Quantity = System.fnConvertCompoundUnit((select UnitMaintenanceID from Inv.tblProduct where ProductID = @ProductID),  @QtyUnitID, @Quantity);

	INSERT INTO Inv.tblSalesInvoiceDetails (SalesInvoiceID,Code,ProductID,Quantity,SalesRate,Amount,DiscPercentage,Discount,Net_Amount, QtyUnitID)VALUES(
										@SalesInvoiceID,@Code,@ProductID,@Quantity,@SalesRate,@Amount,@DiscPercentage,@Discount,@Net_Amount, @QtyUnitID
										)
	SET @Return='SUCCESS';
	end 
	else 
	begin
		raiserror('Invalid Ledger Selected!',15,1);
		SET @Return='FAILURE';
		RETURN -100
	end
	




	if @@Error<>0
	begin
		raiserror('An error occured during addition of accounnting group. Please check the fields and try again!',15,1);
		SET @Return='FAILURE';
		RETURN -100
	end


