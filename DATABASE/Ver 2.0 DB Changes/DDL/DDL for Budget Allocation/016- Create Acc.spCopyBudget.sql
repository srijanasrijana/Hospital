
/****** Object:  StoredProcedure [Acc].[spCopyBudget]    Script Date: 1/6/2017 11:27:34 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



Create PROCEDURE Acc.spCopyBudget
(@BudgetID int,
@NewBudgetID int)
as
begin
declare		@MasterBudgetID int
declare @id int
BEGIN TRANSACTION [T]
BEGIN TRY

--delete data from both master and detail table with @newBudgetID (if there is)
delete from Acc.tblBudgetAllocationDetail where BudgetMasterID in(select BudgetMasterID from Acc.tblBudgetAllocationMaster where BudgetID=@NewBudgetID)
delete from Acc.tblBudgetAllocationMaster where BudgetID=@NewBudgetID
--get master table data of old budgetid
declare @oldMasterTable table(masterID int,budgetID int,accountID int,accountType nvarchar(50),total decimal)
insert into @oldMasterTable
select *from Acc.tblBudgetAllocationMaster where BudgetID=@BudgetID

while((select count(*)from @oldMasterTable)>0)
begin
select top 1 @id=masterID from @oldMasterTable

insert into	Acc.tblBudgetAllocationMaster 
select	@NewBudgetID,accountID, accountType,total 
from	@oldMasterTable where masterID=@id

set @MasterBudgetID=@@IDENTITY

insert	 into Acc.tblBudgetAllocationDetail 
select	@MasterBudgetID,ClassID,Amount from Acc.tblBudgetAllocationDetail where BudgetMasterID=@id

delete from @oldMasterTable where masterID=@id

end

COMMIT TRANSACTION [T]
END TRY
BEGIN CATCH
ROLLBACK TRANSACTION [T]
RAISERROR('Problem while copying',16,1)
END CATCH

end



