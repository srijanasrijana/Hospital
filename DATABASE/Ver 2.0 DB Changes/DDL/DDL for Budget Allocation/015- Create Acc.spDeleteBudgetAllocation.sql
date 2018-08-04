Create procedure Acc.spDeleteBudgetAllocation 
(@budgetID int)
AS
BEGIN
BEGIN TRANSACTION[T]
BEGIN TRY
--delete data from both master and detail table with @newBudgetID (if there is)
delete from Acc.tblBudgetAllocationDetail where BudgetMasterID in(select BudgetMasterID from Acc.tblBudgetAllocationMaster where BudgetID=@BudgetID)
delete from Acc.tblBudgetAllocationMaster where BudgetID=@BudgetID

COMMIT TRANSACTION[T]
END TRY

BEGIN CATCH

ROLLBACK TRANSACTION[T]
RAISERROR('Problem while Deleting',16,1)

END CATCH
END

