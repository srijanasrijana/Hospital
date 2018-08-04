

/****** Object:  StoredProcedure [Acc].[spInsertMasterBudgetAllocation]    Script Date: 10/26/2016 11:44:49 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [Acc].[spInsertMasterBudgetAllocation]
(@BudgetID int,
@AccountID int,
@AccountType nvarchar(50),
@TotalAllocationForAccount decimal,
@Return nvarchar(20) output)
as
begin

begin try

insert into Acc.tblBudgetAllocationMaster(BudgetID,AccountID,AccountType,TotalAllocationForAccount)
 values(@BudgetID,@AccountID,@AccountType, @TotalAllocationForAccount)
set @return=SCOPE_IDENTITY();
end try
begin catch
raiserror('An error occured during addition of master budget allocation',15,1);
		SET @Return='FAILURE';
		RETURN -100
end catch

end



GO


