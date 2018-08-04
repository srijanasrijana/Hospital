USE [BENT003]
GO

/****** Object:  UserDefinedFunction [Acc].[fnCheckBudget1]    Script Date: 12/29/2016 9:46:40 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


Alter FUNCTION [Acc].[fnCheckBudget](@GroupID int,@BudgetID int,@Amount decimal,@AccountClassIDsSettings  xml=NULL) 
RETURNS @CheckTable TABLE
(
	chk INT NOT NULL,
	AccountName NVARCHAR(50) NULL,
	BudgetAmount Decimal NULL,
	TransactionAmount Decimal NULL
)
AS
BEGIN

declare @Chk int
 DECLARE @Return int
  declare @GrpID int
  declare @BudgetAmount decimal
  declare @ActualTransactAmount decimal
  declare @GroupName nvarchar(50)

   Declare @AccountingClassID TABLE
	(
		AccClassID INT
	);
  if(@AccountClassIDsSettings is not null) --If AccountingCLassId is not null
	begin

		-- Write to temporary table @AccClassID from xml
		--INSERT INTO @AccountingClassID(AccClassID)
		--SELECT AccClassID FROM  Acc.fnGetAllAccClass (@AccountClassIDsSettings);
		INSERT INTO  @AccountingClassID(AccClassID)
    SELECT
        IdNode.value('(.)[1]', 'int')
    FROM
        @AccountClassIDsSettings.nodes('AccClassIDSettings/AccClassID') AS IdTbl(IdNode)
   end

   else
   begin
   		INSERT INTO @AccountingClassID(AccClassID)values(1)
   end

  if(@GroupID=0)--if there is no further group then set first chk field which indicates that provided amount is within budget limit
  begin
  delete from @CheckTable--clear table data
  insert into @CheckTable values(1,' ',0,0)
  return 
  end

  select @Chk=count(*) from Acc.tblBudgetAllocationMaster where BudgetID=@BudgetID and AccountID=@GroupID and AccountType='Group'
  if(@chk=0)--if it is not present in current budget then look wheather its parent group is present or not
  begin
  select @GrpID=isnull(Parent_GrpID,0) from Acc.tblGroup where GroupID=@GroupID;
    delete from @CheckTable
    insert into @CheckTable
    select *from Acc.fnCheckBudget(@GrpID,@BudgetID,@Amount,@AccountClassIDsSettings)--recursive function call
  end

  else
  begin
       --get the budget amount from budget table for the group
         select @BudgetAmount= isnull(sum(Amount),0) from Acc.tblBudgetAllocationDetail as BAD inner join
         Acc.tblBudgetAllocationMaster as BAM on BAD.BudgetMasterID=BAM.BudgetMasterID
         where BAM.BudgetID=@BudgetID and BAM.AccountID=@GroupID and BAM.AccountType='Group' and BAD.ClassID in (select *from @AccountingClassID)

         --create a temp table to hold all ledger with in a group
		 declare @tblLedgerID TABLE(ldgID int)
         insert into @tblLedgerID 
          select LedgerID from Acc.tblLedger where GroupID=@GroupID

       --now use common table expression to get all ledger from its child group
		 ;with CTE_Group(gID)
		 as(select GroupID from Acc.tblGroup where Parent_GrpID=@GroupID
		 union all select GroupID from Acc.tblGroup,CTE_Group where Parent_GrpID=CTE_Group.gID )

		 insert into @tblLedgerID 
		 select LedgerID from Acc.tblLedger where GroupID in (select * from CTE_Group) 

		 --now get the actual transact amount for the group
		         select @ActualTransactAmount=(isnull(sum(Debit_Amount),0)-isnull(sum(Credit_Amount),0)) from Acc.tblTransaction as T inner join Acc.tblTransactionClass as TC
       on T.TransactionID=TC.TransactionID where T.LedgerID in (select * from @tblLedgerID) and TC.AccClassID in (select *from @AccountingClassID)

	   if(@ActualTransactAmount+@Amount>@BudgetAmount)
	   begin-- if transact amount is greater than budget amount then set chk field to 0
	   select @GroupName=EngName from Acc.tblGroup where GroupID=@GroupID
	   delete from @CheckTable
	   insert into @CheckTable values(0,@GroupName,@BudgetAmount,@ActualTransactAmount+@Amount)
	   end

	   else
	   begin
	     select @GrpID=isnull(Parent_GrpID,0) from Acc.tblGroup where GroupID=@GroupID
	     --set @Return= Acc.fnCheckBudget(@GrpID,@BudgetID,@Amount)
	    delete from @CheckTable
        insert into @CheckTable
          select *from Acc.fnCheckBudget(@GrpID,@BudgetID,@Amount,@AccountClassIDsSettings)
	   end
  end

RETURN
END

GO


