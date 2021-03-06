
/****** Object:  StoredProcedure [Acc].[spBudgetReport]    Script Date: 13-Dec-16 4:32:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



ALTER procedure  [Acc].[spBudgetReport]
(@BudgetID int,
@ClassIDs nvarchar(300),
@ProjectID int)
AS
begin
--creating temporary table for storing final data
create table #final(ActualAmount decimal,BudgetAmount decimal,AccountID int,AccountName nvarchar(50),Acctype nvarchar(20))
--temporary table for storing actual expenses on account head
create table #ActualAmount(Amount decimal,AccountID int)


create table #temp(Ledgerid int,ParentID int)
--insert only ledger id and parent as ledger id on temporary table parnet id is only useful when we deal with group in temp table
insert into #temp
select AccountID,AccountID from Acc.tblBudgetAllocationMaster where BudgetID=@BudgetID and AccountType='Ledger'

insert into #ActualAmount
select isnull((isnull(sum(T.Debit_Amount),0)-isnull(sum(T.Credit_Amount),0)),0),#temp.ParentID from Acc.tblTransaction as T inner join Acc.tblTransactionClass as TC on T.TransactionID=TC.TransactionID
inner join #temp on #temp.Ledgerid=T.LedgerID where TC.AccClassID IN (select cast(Item AS INTEGER) FROM Acc.fnSplitString(@ClassIDs,',')) and T.ProjectID=@ProjectID 
group by #temp.ParentID
insert into #final
SELECT ISNULL( #ActualAmount.Amount,0) as ActualAmount,sum(BAD.Amount) as BudgetAmount, BAM.AccountID,ld.EngName,'ledger' as AccType
FROM   #ActualAmount right join Acc.tblBudgetAllocationMaster as BAM on #ActualAmount.AccountID=BAM.AccountID inner join Acc.tblBudgetAllocationDetail as BAD
    ON BAD.BudgetMasterID = BAM.BudgetMasterID inner join Acc.tblLedger as ld on BAM.AccountID=ld.LedgerID    
   where BAD.ClassID in (select cast(Item AS INTEGER) FROM Acc.fnSplitString(@ClassIDs,',')) and BAM.BudgetID=@BudgetID group by BAM.AccountID,ld.EngName,#ActualAmount.Amount

delete from #temp
delete from #ActualAmount

;with practice(GroupID,parentgrpid)
as(select AccountID,AccountID from Acc.tblBudgetAllocationMaster where BudgetID=@BudgetID and AccountType='Group'
union all 
select grp.GroupID,practice.parentgrpid from acc.tblGroup as grp inner join practice on grp.Parent_GrpID= practice.GroupID )

insert into #temp 
select ld.LedgerID,practice.parentgrpid from acc.tblLedger as ld inner join practice on ld.GroupID=practice.GroupID

insert into #ActualAmount
select isnull( (isnull(sum(T.Debit_Amount),0)-isnull(sum(T.Credit_Amount),0)),0),#temp.ParentID from Acc.tblTransaction as T inner join Acc.tblTransactionClass as TC on T.TransactionID=TC.TransactionID
inner join #temp on #temp.Ledgerid=T.LedgerID where TC.AccClassID IN (select cast(Item AS INTEGER) FROM Acc.fnSplitString(@ClassIDs,',')) and T.ProjectID=@ProjectID 
group by #temp.ParentID


insert into #final
SELECT ISNULL(#ActualAmount.Amount,0) as ActualAmount,sum(BAD.Amount) as BudgetAmount, BAM.AccountID,grp.EngName,'Group' as AccType
FROM   #ActualAmount right join Acc.tblBudgetAllocationMaster as BAM on #ActualAmount.AccountID=BAM.AccountID inner join Acc.tblBudgetAllocationDetail as BAD
    ON BAD.BudgetMasterID = BAM.BudgetMasterID inner join Acc.tblGroup as grp on BAM.AccountID=grp.GroupID    
   where BAD.ClassID in (select cast(Item AS INTEGER) FROM Acc.fnSplitString(@ClassIDs,',')) and BAM.BudgetID=@BudgetID group by BAM.AccountID,grp.EngName,#ActualAmount.Amount

select *from  #final

end

