
/****** Object:  StoredProcedure [Acc].[spAddBudgetName]    Script Date: 10/26/2016 11:42:44 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



Create PROCEDURE [Acc].[spAddBudgetName]
(@budgetName nvarchar(50),
@startDate date,
@endDate date,
@description nvarchar(100))
as
begin
INSERT into Acc.tblBudget(budgetName,startDate,endDate,[description],isActive,isBuiltIn)
 values(@budgetName,@startDate,@endDate,@description,1,1)
end


GO


