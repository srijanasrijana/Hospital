
/****** Object:  StoredProcedure [Inv].[spProductCreate]    Script Date: 2017-08-17 11:13:29 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [Inv].[spProductCreate]
(
@Name NVARCHAR(50),
@GroupID INT,
@ProductCode NVARCHAR(20),
@ProductColor NVARCHAR(20),
@DepotID INT=NULL,
@Remarks NVARCHAR(200),
@UnitMaintenanceID int=null,
@SalesRate MONEY=NULL,
@Quantity FLOAT=NULL,
@ProductTextColor bigint=null,
@PurchaseRate MONEY=NULL,
@PurchaseDiscount MONEY=NULL,
@TotalValue MONEY=NULL,
@Image image = NULL,
@Created_By NVARCHAR(20),
@IsVatApplicable int,
@IsInventoryApplicable int,
@IsDecimalApplicable int,
@ReturnID INT OUTPUT,
@Return NVARCHAR(20) OUTPUT
)
AS
SET NOCOUNT ON

	
--VALIDATE INPUTS
	IF @GroupID IS NULL
	BEGIN
		RAISERROR('Invalid Group Name',15,1)
		RETURN -100
	END
	
	--Finally insert into tblledger
	if(@Image IS NULL)
	
	INSERT INTO Inv.tblProduct(EngName,NepName,GroupID,ProductCode,ProductColor,DepotID,Remarks,UnitMaintenanceID,SalesRate,Quantity,PurchaseRate,PurchaseDiscount,TotalValue,Image,BuiltIn,IsActive,Created_By,Created_Date,Modified_By,Modified_Date,BackColor,isVatApplicable,IsInventoryApplicable,IsDecimalApplicable) VALUES(
										@Name,
										@Name,										
										@GroupID,
                                        @ProductCode,
                                        @ProductColor,
                                        @DepotID,
										@Remarks,
                                        @UnitMaintenanceID,
                                        @SalesRate,
                                        @Quantity,
                                        @PurchaseRate,
                                        @PurchaseDiscount,
                                        @TotalValue,
										@Image,
										0,
										1,
										@Created_By,
										GetDate(),
										null,
										null,
										@ProductTextColor,@IsVatApplicable,@IsInventoryApplicable,@IsDecimalApplicable
											)
 else 
 	INSERT INTO Inv.tblProduct(EngName,NepName,GroupID,ProductCode,ProductColor,DepotID,Remarks,UnitMaintenanceID,SalesRate,Quantity,PurchaseRate,PurchaseDiscount,TotalValue,Image,BuiltIn,IsActive,Created_By,Created_Date,Modified_By,Modified_Date,BackColor,isVatApplicable,IsInventoryApplicable) VALUES(
										@Name,
										@Name,										
										@GroupID,
                                        @ProductCode,
                                        @ProductColor,
                                        @DepotID,
										@Remarks,
                                        @UnitMaintenanceID,
                                        @SalesRate,
                                        @Quantity,
                                        @PurchaseRate,
                                        @PurchaseDiscount,
                                        @TotalValue,
										@Image,
										0,
										1,
										@Created_By,
										GetDate(),
										null,
										null,
										@ProductTextColor,@IsVatApplicable,@IsInventoryApplicable
											)											
											
	SET @ReturnID=Scope_Identity();


	if(@@Error<>0)
	begin
		Set @Return='FAILURE';
	end
	else
	begin
		SET @Return='SUCCESS';
	end

