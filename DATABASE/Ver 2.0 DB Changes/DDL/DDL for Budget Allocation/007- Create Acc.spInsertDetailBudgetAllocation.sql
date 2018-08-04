

/****** Object:  StoredProcedure [Acc].[spInsertDetailBudgetAllocation]    Script Date: 10/26/2016 11:44:15 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE procedure [Acc].[spInsertDetailBudgetAllocation]
(@MasterbudgetID int,
@ClassID int,
@Amount decimal,
@Return nvarchar(20) output)
as
begin
begin try
insert into Acc.tblBudgetAllocationDetail 
values(@MasterbudgetID,@ClassID,@Amount)
set @return='SUCCESS'
end try
begin catch
raiserror('An error occured during addition of detail budget allocation',15,1);
		SET @Return='FAILURE';
		RETURN -100
end catch

end



GO


