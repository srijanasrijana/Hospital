
/****** Object:  StoredProcedure [Acc].[spGetLedgerNameNOpBal]    Script Date: 12/22/2016 2:56:57 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

Create PROC [Acc].[spGetLedgerNameNOpBal]
@LedgerID INT=NULL,
@GroupID INT=NULL,
@AccountClassIDsSettings  xml    -- It encludes AccountCLassIDs  Info
as
begin
Declare @AccountingClassID TABLE
	(
		AccClassID INT
	);
	DECLARE @RootAccClass INT=1; --Hold Root Accounting Class. By default it is ROOT (ID =1)
		DECLARE @SqlStmt NVARCHAR(500);

	DECLARE @tmpOpenBal TABLE --table that holds final result
	(
		LedgerID  INT,
		LedgerName nvarchar(50),
		OpenBal  DECIMAL(19,5),
		OpenBalDrCr nvarchar(10),
		OpenBalDate nvarchar(50)
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

 set @SqlStmt='Select Acc.tblLedger.LedgerID, EngName as LedgerName,ISNULL(OpenBal,0),OpenBalDrCr,
			case OpenBalDate 
			when NULL then NULL else Date.fnEngtoNep(OpenBalDate )
			END
			from Acc.tblLedger left outer join  Acc.tblOpeningBalance on 
						Acc.tblLedger.LedgerID=Acc.tblOpeningBalance.LedgerID and  AccClassID= '+CONVERT(varchar, @RootAccClass)


IF(@GroupID<=0)
BEGIN
	   IF (@LedgerID=0)
		BEGIN
		--when id is 0 select all ledger from ledger table	
		set @SqlStmt=@SqlStmt
			END
			
			ELSE IF(@LedgerID>0)
			BEGIN
			--select particular ledger from ledger table
			set @SqlStmt=@SqlStmt+ ' where Acc.tblLedger.LedgerID='+CONVERT(varchar, @LedgerID)
		END
			ELSE
			BEGIN
			Raiserror('Invalid Ledger ID provided ',15,1);
			RETURN -1;
		    END

	 INSERT INTO @tmpOpenBal EXECUTE sp_executesql @SqlStmt
select *from @tmpOpenBal 

END


ELSE
BEGIN
declare @tblLedgerID TABLE(ldgID int)
insert into @tblLedgerID 
select LedgerID from Acc.tblLedger where GroupID=@GroupID

;with CteGroup(grpID) 
AS(select GroupID from Acc.tblGroup where Parent_GrpID=@GroupID
UNION ALL
select GroupID from Acc.tblGroup,CteGroup where Parent_GrpID=CteGroup.grpID)

insert into @tblLedgerID 
select LedgerID from Acc.tblLedger where GroupID in (select *from CteGroup)

--set @SqlStmt=@SqlStmt+' where Acc.tblLedger.LedgerID in(select *from' +CONVERT(varchar,@tblLedgerID)+ ')'
 Select Acc.tblLedger.LedgerID, EngName as LedgerName,ISNULL(OpenBal,0) as OpenBal,OpenBalDrCr,
			case OpenBalDate 
		when NULL then NULL else Date.fnEngtoNep(OpenBalDate )			END as OpenBalDate			from Acc.tblLedger left outer join  Acc.tblOpeningBalance on 
						Acc.tblLedger.LedgerID=Acc.tblOpeningBalance.LedgerID and  AccClassID=  @RootAccClass where 
						 Acc.tblLedger.LedgerID in(select *from @tblLedgerID)

END

end










GO


