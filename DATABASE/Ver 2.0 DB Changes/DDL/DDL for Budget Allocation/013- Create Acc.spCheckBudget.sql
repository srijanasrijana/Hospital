USE [BENT003]
GO

/****** Object:  StoredProcedure [Acc].[spCheckBudget1]    Script Date: 12/29/2016 9:36:16 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






ALTER PROCEDURE [Acc].[spCheckBudget]
(@CurrentDate datetime,
@LedgerID int,
@Amount decimal,
@AccountClassIDsSettings  xml=NULL
)
as
begin
 declare @chk int
 declare @BudgetID int
 --get active budget budget id
 Select @BudgetID=budgetID from Acc.tblBudget where startDate<=@CurrentDate and endDate>= @CurrentDate  


 Declare @AccountingClassID TABLE
	(
		AccClassID INT
	);

	
	if(@AccountClassIDsSettings is not null) --If AccountingCLassId is not null
	begin

		-- Write to temporary table @AccClassID from xml
		--INSERT INTO @AccountingClassID(AccClassID)
		--SELECT AccClassID FROM  Acc.fnGetAllAccClass (@AccountClassIDsSettings);

		-- Write to temporary table @JAccClassID from xml
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

 declare @ReturnTable table (
	chk INT NOT NULL,
	AccountName NVARCHAR(50) NULL,
	BudgetAmount Decimal NULL,
	TransactionAmount Decimal NULL
)


 declare @budgetAmount decimal
 declare @ActualTransactAmount decimal
 declare @groupID int=null
 declare @LedgerName nvarchar(50)
 --find wheather the ledger is present in current budget or not
 select @chk=count(*) from Acc.tblBudgetAllocationMaster where BudgetID=@BudgetID and AccountID=@LedgerID and AccountType='Ledger'
 if(@chk>0)--if present then find the budget amount and the actual transaction amount
 begin
        select @budgetAmount= isnull(sum(Amount),0) from Acc.tblBudgetAllocationDetail as BAD inner join
         Acc.tblBudgetAllocationMaster as BAM on BAD.BudgetMasterID=BAM.BudgetMasterID
         where BAM.BudgetID=@BudgetID and BAM.AccountID=@LedgerID and BAM.AccountType='Ledger' and BAD.ClassID in (select *from @AccountingClassID)

		 --find actual transact amount
        select @ActualTransactAmount=(isnull(sum(Debit_Amount),0)-isnull(sum(Credit_Amount),0)) from Acc.tblTransaction as T inner join Acc.tblTransactionClass as TC
       on T.TransactionID=TC.TransactionID where T.LedgerID=@LedgerID and TC.AccClassID in (select *from @AccountingClassID)

      if(@ActualTransactAmount+@Amount>@budgetAmount)
	  begin--if transact amount is greater than budget amount then set 0 for chk field 
       
	      select @LedgerName=EngName from Acc.tblLedger where LedgerID=@LedgerID
	      delete from @ReturnTable
	      insert into @ReturnTable values(0,@LedgerName,@BudgetAmount,@ActualTransactAmount+@Amount)
	   end

      else
       begin-- again check wheather the budget of parent group is violeted or not
	       select @groupID=GroupID from Acc.tblLedger where LedgerID=@LedgerID     
		   delete from @ReturnTable--first clear the table
           insert into @ReturnTable
           select *from Acc.fnCheckBudget(@groupID,@BudgetID,@Amount,@AccountClassIDsSettings)--recursive function call	
       end
 end
 else
 begin--if it is not present in current budget then look wheather its parent group are present or not
 	   select @groupID=GroupID from Acc.tblLedger where LedgerID=@LedgerID
	  
	   delete from @ReturnTable
    insert into @ReturnTable
    select *from Acc.fnCheckBudget(@groupID,@BudgetID,@Amount,@AccountClassIDsSettings)--recursive function call
 end

 select *from @ReturnTable
end





GO


