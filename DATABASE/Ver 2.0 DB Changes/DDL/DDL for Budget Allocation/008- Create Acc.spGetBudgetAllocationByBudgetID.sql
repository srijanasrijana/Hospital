

/****** Object:  StoredProcedure [Acc].[spGetBudgetAllocationByBudgetID]    Script Date: 10/26/2016 11:43:26 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


create procedure [Acc].[spGetBudgetAllocationByBudgetID] 
(@BudgetID int)
as
begin
declare @MasterID int
--select @masterId=budgetMasterID from Acc.tblBudgetAllocationMaster where budgetID=@budgetID
create table #temp(MasterID int,AccountID int,AccountName nvarchar(50), TypeOfAcc nvarchar(50), TotalAllocationForAccount decimal)

--due to group and ledger name are stored in different table we need to fetch data two times and store them in temp datatable 
--get all data from budget allocation where account type  is ledger
insert into #temp
SELECT    bam.BudgetMasterID, 
						 bam.AccountID,
				       Acc.tblLedger.EngName,
						 bam.AccountType, 
                         bam.TotalAllocationForAccount
						 						 
FROM     Acc.tblBudgetAllocationMaster as bam INNER JOIN
                         
						 Acc.tblLedger on Acc.tblLedger.LedgerID=bam.AccountID 

						 where bam.BudgetID=@BudgetID 
						 and 						
						 bam.AccountType='Ledger'

						 --get all data from budget allocation where account type  is group
insert into #temp
SELECT   bam.BudgetMasterID, 
						 bam.AccountID,						   
						 Acc.tblGroup.EngName,
						   bam.AccountType, 
                         bam.TotalAllocationForAccount

FROM            Acc.tblBudgetAllocationMaster as bam INNER JOIN
						  Acc.tblGroup on Acc.tblGroup.GroupID=bam.AccountID 
						 where bam.BudgetID=@BudgetID 
						 and 						
						  bam.AccountType='Group'

						  select *from #temp
						  select startDate,endDate from Acc.tblBudget where budgetID=@budgetID
						  
						  end


GO


