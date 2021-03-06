--USE [BENT003]
--GO
/****** Object:  StoredProcedure [Inv].[spPurchaseInvoiceDetailCreate]    Script Date: 2017-09-07 11:00:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER  PROCEDURE [Inv].[spPurchaseInvoiceDetailCreate](
@PurchaseInvoiceID NVARCHAR(50),
@Code NVARCHAR(50),
@ProductName NVARCHAR(30)=null,
@ProductID INT=0,
@Language NVARCHAR(30),
@Quantity float=NULL,
@PurchaseRate MONEY,
@Amount MONEY,
@DiscPercentage MONEY,
@Discount MONEY,
@Net_Amount MONEY,
@VAT decimal(20,5),
@CustomDutyPercentage decimal(20,5), 
@CustomDuty decimal(20,5), 
@Freight decimal(20,5),
@QtyunitID int= null,
--@Tax1 money=null,
--@Tax2 money=null,
--@Tax3 money=null,
--@VAT money=null,
--@Total_Amount money=null,
@Return NVARCHAR(20) OUTPUT
)
AS
--Modified By Ramesh Prajapati
SET NOCOUNT ON
IF(@ProductID=0) --IF PRODUCT NAME IS GIVEN
Begin
	

	IF @Language='ENGLISH'
		SELECT @ProductID=ProductID FROM Inv.tblProduct WHERE EngName=@ProductName
	ELSE IF @Language='NEPALI'
		SELECT @ProductID=ProductID FROM Inv.tblProduct WHERE NepName=@ProductName
END
	if @ProductID>0
	begin
--DO THE REAL INSERT
	INSERT INTO Inv.tblPurchaseInvoiceDetails (PurchaseInvoiceID,Code,ProductID,Quantity,PurchaseRate,Amount,DiscPercentage,Discount,Net_Amount, VAT, CustomDutyPercentage, CustomDuty, Freight,QtyunitID)VALUES(
											@PurchaseInvoiceID,@Code,@ProductID,@Quantity,@PurchaseRate,@Amount,@DiscPercentage,@Discount,@Net_Amount, @VAT, @CustomDutyPercentage, @CustomDuty, @Freight,@QtyunitID
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


