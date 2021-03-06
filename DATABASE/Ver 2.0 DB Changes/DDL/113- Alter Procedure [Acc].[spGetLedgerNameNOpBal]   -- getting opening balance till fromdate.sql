
/****** Object:  StoredProcedure [Acc].[spGetLedgerNameNOpBal]    Script Date: 09/06/2017 2:20:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROC [Acc].[spGetLedgerNameNOpBal]  
@LedgerID INT=NULL,
@GroupID INT=NULL,
@fromDate datetime =null,
@AccountClassIDsSettings  xml    -- It encludes AccountCLassIDs  Info
as
begin
Declare @AccountingClassID TABLE
	(
		AccClassID INT
	);
	DECLARE @RootAccClass INT=1; --Hold Root Accounting Class. By default it is ROOT (ID =1)
		DECLARE @SqlStmt NVARCHAR(500);

--new
Declare @tblLedgerID table(ldgID int)
Declare @tblopningBal table(ledgerID int,OpeningBal decimal(19,5))
Declare @tblTransaction table(ledgerID int,TransAmount decimal(19,5))

if(@fromDate is null)
 select top 1 @fromDate =  BookBeginFrom from System.tblCompanyInfo

	DECLARE @tmpOpenBal TABLE --table that holds final result
	(
		LedgerID  INT,
		LedgerName nvarchar(50),
		OpenBal  DECIMAL(19,5),
		OpenBalDrCr nvarchar(10),
		OpenBalDate nvarchar(50),
		LedgerCode nvarchar(50)
	);

	if(@GroupID is null)
	begin
	set @GroupID=0
	end
	
	if(@LedgerID is null)
	begin
	set @LedgerID=0
	end
	

		if(@AccountClassIDsSettings is not null) --If AccountingCLassId is not null
	begin

		-- Write to temporary table @AccClassID from xml
		INSERT INTO @AccountingClassID(AccClassID)
		SELECT AccClassID FROM  Acc.fnGetAllAccClass (@AccountClassIDsSettings);
		
		
		DECLARE @tmpAccClassID INT;--for storing AccClassID temporarily
		SELECT TOP 1 @tmpAccClassID=AccClassID FROM @AccountingClassID;

		
		--Find root of accounting class to get Opening Balance		
		SELECT @RootAccClass=Acc.fnFindRootAccClassID(@tmpAccClassID);
		
		--Check if accounting class is valid
		IF @RootAccClass=-1  --Error occurred during Root finding
		BEGIN
			Raiserror('Invalid Accounting Class ID provided for root finding',15,1);
			RETURN -1;
		END				
END

ELSE--if no accounting class is use default root class id 1
BEGIN
set @RootAccClass=1
END

   IF(@GroupID<=0)
   BEGIN
	   IF (@LedgerID=0)
		BEGIN
		--when id is 0 select all ledger from ledger table	
		--set @SqlStmt=@SqlStmt
               insert into @tblLedgerID 
               select LedgerID from Acc.tblLedger;
			END
			
			ELSE IF(@LedgerID>0)
			BEGIN
			--select particular ledger from ledger table
			--set @SqlStmt=@SqlStmt+ ' where Acc.tblLedger.LedgerID='+CONVERT(varchar, @LedgerID)
			insert into @tblLedgerID values(@LedgerID);
		END
			ELSE
			BEGIN
			Raiserror('Invalid Ledger ID provided ',15,1);
			RETURN -1;
		    END

--	 INSERT INTO @tmpOpenBal EXECUTE sp_executesql @SqlStmt
--select *from @tmpOpenBal 

END

ELSE--get all ledger id of given group
BEGIN
--declare @tblLedgerID TABLE(ldgID int)
        insert into @tblLedgerID 
        select LedgerID from Acc.tblLedger where GroupID=@GroupID
        
        ;with CteGroup(grpID) 
        AS(select GroupID from Acc.tblGroup where Parent_GrpID=@GroupID
        UNION ALL
        select GroupID from Acc.tblGroup,CteGroup where Parent_GrpID=CteGroup.grpID)
        
        insert into @tblLedgerID 
        select LedgerID from Acc.tblLedger where GroupID in (select *from CteGroup)
        
END

insert into @tblopningBal 
select l.ldgID, Case  when (o.OpenBalDrCr='DEBIT') then isnull(o.OpenBal,0) else isnull((0-o.OpenBal),0)  end from  @tblLedgerID l left join Acc.tblOpeningBalance o  on l.ldgID= o.LedgerID

insert into @tblTransaction
select l.ldgID, (sum(isnull(t.Debit_Amount,0))-sum(isnull(t.Credit_Amount,0))) from @tblLedgerID l left join  Acc.tblTransaction t  on l.ldgID= t.LedgerID where t.TransactDate is null or t.TransactDate < @fromDate  group by l.ldgID

update o set o.OpeningBal=(o.OpeningBal+t.TransAmount)  from @tblopningBal o inner join @tblTransaction t on o.ledgerID=t.ledgerID

select l.LedgerID,l.EngName as LedgerName,ABS(o.OpeningBal) as OpenBal,case
when o.OpeningBal>=0 then 'DEBIT'
else 'CREDIT'
 end  as OpenBalDrCr,Date.fnEngtoNep(@fromDate) as OpenBalDate,l.LedgerCode
 from Acc.tblLedger l inner join @tblopningBal o on l.LedgerID=o.ledgerID
end











