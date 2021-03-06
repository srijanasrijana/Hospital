
/****** Object:  StoredProcedure [Acc].[spRemoveBankReconciliationEntry]    Script Date: 2/15/2017 2:53:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



create PROCEDURE [Acc].[spRemoveBankReconciliationEntry]
 @BankReconciliationID  INT
AS
	BEGIN
		SET NOCOUNT ON
--if you delete journal master entry it automatically delete from its detail table 
--for this you must have insert update criteria on CASCADE
BEGIN TRANSACTION[T]
	BEGIN TRY
			DELETE FROM Acc.tblTransactionClass WHERE RowID = @BankReconciliationID and VoucherType='BRECON';
			DELETE FROM Acc.tblTransaction WHERE RowID = @BankReconciliationID and VoucherType='BRECON';

			DELETE FROM Acc.tblBankReconciliationDetails WHERE BankReconciliationID = @BankReconciliationID;
			DELETE FROM Acc.tblBankReconciliationMaster WHERE BankReconciliationID =  @BankReconciliationID;		
			
			DELETE FROM System.tblDueDate WHERE RowID = @BankReconciliationID and VoucherType='BRECON';
	END TRY

	BEGIN CATCH
		ROLLBACK TRANSACTION[T];
		CLOSE cur;
		DEALLOCATE cur;
		RAISERROR('Problem while deleting bank reconciliation',16,1)
	END CATCH
	COMMIT TRANSACTION[T]	
	END