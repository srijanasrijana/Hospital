
/****** Object:  StoredProcedure [Acc].[editBudgetName]    Script Date: 9/5/2016 1:48:18 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

Create procedure [Acc].[spEditBudgetName]
(@budgetID int,
@budgetName nvarchar(50),
@startDate date,
@endDate date,
@description nvarchar(100)
)
as
begin
UPDATE Acc.tblBudget SET   budgetName =@budgetName, startDate =@startDate,
 endDate =@endDate, description =@description where budgetID=@budgetID
end
GO


