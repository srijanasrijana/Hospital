
/****** Object:  StoredProcedure [Inv].[spProductModify]    Script Date: 2017-08-17 11:13:32 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [Inv].[spProductModify]
(
@ProductID INT,
@Lang NVARCHAR(50),
@Name NVARCHAR(50),
@GroupID INT,
@ProductCode NVARCHAR(20),
@ProductColor NVARCHAR(20),
@DepotID INT,
@Remarks NVARCHAR(200),
@UnitMaintenanceID INT=NULL,
@SalesRate MONEY,
@Quantity DECIMAL(12,2),
@PurchaseRate MONEY,
@PurchaseDiscount MONEY,
@TotalValue MONEY,
@Image image=NULL,
@Modified_By NVARCHAR(20),
@ProductTextColor bigint=null,
@isVatApplicable int,
@IsInventoryApplicable int,
@IsDecimalApplicable int,
@ReturnID INT OUTPUT,
@Return NVARCHAR(20) OUTPUT
)
AS
SET NOCOUNT ON

	DECLARE  @BuiltIn BIT

	--CHECK IF IT IS BUILT IN 
	SELECT @BuiltIn=BuiltIn FROM Inv.tblProduct WHERE ProductID=@ProductID;
	if(@BuiltIn=1)
	begin
		raiserror('Built in Product cannot be modified',15,1);
		return -100
	end

	--VALIDATE INPUTS
	IF @GroupID IS NULL
	BEGIN
		RAISERROR('Invalid Group Name',15,1)
		RETURN -100
	END
	

	DECLARE @NameField AS NVARCHAR(20)

	if(@Lang='ENGLISH')
		UPDATE Inv.tblProduct SET EngName=@Name,
									
										GroupID=@GroupID,
                                        ProductCode = @ProductCode,
                                        ProductColor =@ProductColor,
                                        DepotID  = @DepotID,
										Remarks=@Remarks,
										
                                        UnitMaintenanceID =@UnitMaintenanceID,
                                        SalesRate = @SalesRate,
                                        Quantity = @Quantity,
                                        PurchaseRate = @PurchaseRate,
                                        PurchaseDiscount = @PurchaseDiscount,
                                        TotalValue = @TotalValue,
                                        Image = @Image,									
										Modified_By=@Modified_By,
										Modified_Date=GetDate(),
										BackColor=@ProductTextColor,
										isVatApplicable=@isVatApplicable,
										IsInventoryApplicable=@IsInventoryApplicable,
										IsDecimalApplicable = @IsDecimalApplicable
										
		WHERE ProductID=@ProductID;

	ELSE IF(@Lang='NEPALI')
		UPDATE Inv.tblProduct SET NepName=@Name,
										
										GroupID=@GroupID,										
                                        ProductCode = @ProductCode,  
                                        ProductColor =@ProductColor,
                                        Remarks=@Remarks,
                                        UnitMaintenanceID =@UnitMaintenanceID,
                                        SalesRate = @SalesRate,
                                        Quantity = @Quantity,
                                        PurchaseRate = @PurchaseRate,
                                        PurchaseDiscount = @PurchaseDiscount,
                                        TotalValue = @TotalValue,
                                        Image = @Image,										
										Modified_By=@Modified_By,
										Modified_Date=GetDate(),
										BackColor=@ProductTextColor,
										isVatApplicable=@isVatApplicable,
										IsInventoryApplicable=@IsInventoryApplicable,
										IsDecimalApplicable = @IsDecimalApplicable
										
		WHERE ProductID=@ProductID;
											
	SET @ReturnID=Scope_Identity();


	if(@@Error<>0)
	begin
		Set @Return='FAILURE';
	end
	else
	begin
		SET @Return='SUCCESS';
	end
