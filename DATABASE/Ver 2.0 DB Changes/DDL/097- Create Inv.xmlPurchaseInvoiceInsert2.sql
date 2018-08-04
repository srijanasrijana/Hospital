

/****** Object:  StoredProcedure [Inv].[xmlPurchaseInvoiceInsert2]    Script Date: 07/13/2017 11:24:27 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





CREATE PROC [Inv].[xmlPurchaseInvoiceInsert2](@specific xml,@common xml,@lastReturn nvarchar(20) output)
AS  
BEGIN

    DECLARE @docHandle int;
    DECLARE @PIID INT=0;
	DECLARE @tblProduct TABLE(ProductName NVARCHAR(50));
	DECLARE @Pid INT=0;
	DECLARE @Pname NVARCHAR(50);
    EXEC sp_xml_preparedocument @docHandle output, @specific 

         BEGIN TRANSACTION tranPurchase
         BEGIN TRY

		   --READ ONLY PRODUCT ID FROM XML
		     INSERT INTO @tblProduct (ProductName)
			 SELECT ProductName FROM OPENXML(@docHandle,'/PURCHASEINVOICE/PURCHASEINVOICEDETAIL/DETAIL', 2)
			 WITH(ProductName NVARCHAR(50))

			 --VALIDATE ALL PRODUCTS 
			 --DECLARE P_CURSOR CURSOR FOR SELECT ProductName FROM @tblProduct
			 --OPEN P_CURSOR  FETCH NEXT FROM P_CURSOR INTO @Pname  

			 --WHILE @@FETCH_STATUS = 0   
    --         BEGIN
			 --     SELECT @Pid=ISNULL(ProductID,0) FROM INV.tblProduct WHERE EngName=@Pname
			 --     IF(@Pid=0)
			 --     BEGIN
			 --         DECLARE @STR NVARCHAR(50)
			 --         SET @STR='Product '+ @Pname+' not found'
			 --         Raiserror(@STR,15,1);
			 --         SET @lastReturn='FAILURE'
			 --         RETURN -1;
			 --     END
			 --END   
    --         CLOSE P_CURSOR   
    --         DEALLOCATE P_CURSOR


           --INSERT DATA IN PURCHASE INVOICE MASTER BASE TABLE
             INSERT INTO Inv.tblPurchaseInvoiceMaster(SeriesID,PurchaseLedgerID,
             CashPartyLedgerID,ProjectID,DepotID,OrderNo,Voucher_No,
             PurchaseInvoice_Date,Remarks,Created_By,Created_Date,Net_Amount,Tax1,Tax2,Tax3,VAT,Total_Amount,Gross_Amount,
             SpecialDiscount,TotalQty,CustomDuty,Freight,PartyBillNumber,Field1,Field2,Field3,Field4,Field5)
             
             SELECT SeriesID,PurchaseLedgerID,
             CashPartyLedgerID,ProjectID,DepotID,OrderNo,Voucher_No,
             PurchaseInvoice_Date,Remarks,Created_By,Created_Date,Net_Amount,Tax1,Tax2,Tax3,VAT,Total_Amount,Gross_Amount,
             SpecialDiscount,TotalQty,CustomDuty,Freight,PartyBillNumber,Field1,Field2,Field3,Field4,Field5
             
              FROM OPENXML(@docHandle,'/PURCHASEINVOICE/PURCHASEINVOICEMASTER', 2)
              WITH(
              SeriesID INT,PurchaseLedgerID INT,CashPartyLedgerID INT,ProjectID INT,DepotID INT,OrderNo VARCHAR(20), Voucher_No INT, PurchaseInvoice_Date DATETIME,
              Remarks NVARCHAR(200),Created_By NVARCHAR(20),Created_Date DATETIME,Net_Amount MONEY,
              Tax1 MONEY ,Tax2 MONEY,Tax3 MONEY,VAT MONEY,Total_Amount MONEY ,Gross_Amount MONEY,SpecialDiscount MONEY,TotalQty float, 
              CustomDuty FLOAT,Freight FLOAT,PartyBillNumber VARCHAR(20),Field1 NVARCHAR(50) ,Field2 NVARCHAR(50),Field3 NVARCHAR(50),Field4 NVARCHAR(50),Field5 NVARCHAR(50)) 
 

       --GET  PURCHASE INVOICE MASTER ID
       SET @PIID=SCOPE_IDENTITY(); 

	   --TEMPORARY TABLE FOR STORING PURCHASE INVOICE DETAIL DATA
              DECLARE @PIDetails TABLE
              (
              PurchaseInvoiceID INT,
              Code nvarchar(50), 
              ProductID INT,       
              Quantity float,
              PurchaseRate money,
              Amount money,
              DiscPercentage money,
              Discount money,
              Net_Amount money,
              VAT decimal(19,5),
              CustomDutyPercentage decimal(19,5),
              CustomDuty decimal(19,5),
              Freight decimal(19,5)
              ); 

       --WRITE PURCHASE INVOICE DETAILS INTO TEMPORARY TABLE FROM XML

             INSERT INTO @PIDetails (PurchaseInvoiceID,Code,ProductID,Quantity,PurchaseRate,Amount,DiscPercentage,Discount,Net_Amount,VAT,CustomDutyPercentage,CustomDuty,Freight)
             
			 SELECT @PIID as PurchaseInvoiceID ,Code,ProductID,Quantity,PurchaseRate,Amount,DiscPercentage,Discount,Net_Amount,VAT,CustomDutyPercentage,CustomDuty,Freight FROM
             OPENXML(@docHandle,'/PURCHASEINVOICE/PURCHASEINVOICEDETAIL/DETAIL', 2)
             
			 WITH(Code nvarchar(50),ProductID INT, Quantity float,PurchaseRate money,Amount money,DiscPercentage money,Discount money,Net_Amount money,
             VAT decimal(19,5),CustomDutyPercentage decimal(19,5),CustomDuty decimal(19,5),Freight decimal(19,5)
              )

        --INSERT PURCHASE INVOICE DETAIL INTO BASE TABLE
             INSERT INTO INV.tblPurchaseInvoiceDetails (PurchaseInvoiceID,Code,ProductID,Quantity,PurchaseRate,Amount,DiscPercentage,Discount,Net_Amount,VAT,CustomDutyPercentage,CustomDuty,Freight)
             SELECT PurchaseInvoiceID,Code,ProductID,Quantity,PurchaseRate,Amount,DiscPercentage,Discount,Net_Amount,VAT,CustomDutyPercentage,CustomDuty,Freight FROM @PIDetails


        --INSERT DATA INTO TBLINVENTORYTRANSACTON
             INSERT INTO INV.tblInventoryTrans
             SELECT pid.ProductID,pim.DepotID,pid.Quantity,0,'PURCH',@PIID,pim.PurchaseInvoice_Date,0 FROM @PIDetails as pid inner join inv.tblPurchaseInvoiceMaster as pim on pid.PurchaseInvoiceID=pim.PurchaseInvoiceID
			 
	    --EXECUTE [xmlCommonInsert] 
             DECLARE @tempReturn NVARCHAR(50)
             Exec [Acc].[xmlCommonInsert] @common,@PIID,'PURCH',@tempReturn OUTPUT

        IF(@tempReturn='FAILURE')
        BEGIN
             SET @lastReturn='FAILURE'
        END
		Else 
		Begin
       
            SET @lastReturn='SUCCESS'
            COMMIT TRANSACTION tranPurchase 
				END
       

  END TRY


  BEGIN CATCH
       SET @lastReturn='FAILURE'
       ROLLBACK TRANSACTION tranPurchase

	DECLARE @ErrorMessage NVARCHAR(4000);
    DECLARE @ErrorSeverity INT;
    DECLARE @ErrorState INT;
    SELECT @ErrorMessage = ERROR_MESSAGE(),
        @ErrorSeverity = ERROR_SEVERITY(),
        @ErrorState = ERROR_STATE();
    RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState);
       END CATCH
  END






GO


