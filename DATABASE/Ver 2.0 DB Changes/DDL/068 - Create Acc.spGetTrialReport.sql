

/****** Object:  StoredProcedure [Acc].[spGetAllGroupTrialReport]    Script Date: 1/23/2017 3:11:14 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

create  PROCEDURE [Acc].[spGetTrialReport]
(
@Transaction_Start_Date DATETIME=NULL,
@Transaction_End_Date DATETIME=NULL,
@AccountClassIDsSettings  xml=NULL,    -- It encludes AccountCLassIDs  Info
@ProjectIDsSettings xml=NULL, --Null means all
@Settings xml=NULL --Null means all
)
AS
begin


declare @LedgerID INT
set @LedgerID=0
	declare @LedgerDetails as table(LedgerName nvarchar(50),LedgerCode nvarchar(50),LedgerID int,GroupName nvarchar(50),
GroupID int,DebitTotal decimal,CreditTotal decimal,OpenBalDr decimal,OpenBalCr decimal)

	
	SET NOCOUNT ON	
	DECLARE @docHandle int;
	DECLARE @docHandle1 int;
	EXEC sp_xml_preparedocument @docHandle OUTPUT, @AccountClassIDsSettings;
	EXEC sp_xml_preparedocument @docHandle1 OUTPUT, @ProjectIDsSettings;

	--Get all Child accounting Class Ids
	Declare @AccountingClassID TABLE
	(
		AccClassID INT
	);
	--To store the dynamic SQLquery
	DECLARE @SqlStmt NVARCHAR(max);
	DECLARE @LedgerStmt VARCHAR(max);

	--To store Date settings in dynamic SQL Query
	DECLARE @DateSettings NVARCHAR(max);
	--To temporarily store debit and credit amount of the ledger
	Declare @temp TABLE
		(
			LedgerID INT,
			Debit DECIMAL(19,5),
			Credit DECIMAL(19,5),
			OpenBalDr DECIMAL(19,5),
			OpenBalCr DECIMAL(19,5)
		);
	
	
	--String to get all accounting class
	DECLARE @strSQL NVARCHAR(max);
	DECLARE @resultCSV nvarchar(max);
	
		--Prepare SQL Statements according to settings
	if((NULLIF(@Transaction_Start_Date, '') is null) and (NULLIF(@Transaction_End_Date, '') is null))
		begin
			SET @DateSettings='';
		end
	else if(NULLIF(@Transaction_Start_Date, '') is null)
		begin
			SET @DateSettings=' AND t.TransactDate<=''' +  Convert(VARCHAR,@Transaction_End_Date,20) + '''';
		end
	else if(NULLIF(@Transaction_End_Date, '') is null)
		begin
			SET @DateSettings=' AND t.TransactDate>=''' + Convert(VARCHAR,@Transaction_Start_Date,20) + '''';
		end
	else
		begin
			SET @DateSettings=' AND t.TransactDate BETWEEN ''' +  Convert(VARCHAR,@Transaction_Start_Date,20) + ''' AND ''' +  Convert(VARCHAR,@Transaction_End_Date,20)+'''';
		end 
	

	--Prepare Accouning Class IDs
	DECLARE @AccClassSTMT VARCHAR(max)='';

	DECLARE @RootAccClass INT=1; --Hold Root Accounting Class. By default it is ROOT (ID =1)
	DECLARE @OpeningBal DECIMAL(19,5);
	DECLARE @OpeningBalDrCr VARCHAR(15);

	--Temporarily store opening balance in the form of different columns for debit and credit
	DECLARE @tmpOpenBal TABLE
	(
		LedgerID  INT,
		Debit  DECIMAL(19,5),
		Credit  DECIMAL(19,5)
	);

		

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

		

		--Insert the openening balance information to different debit and credit columns
		
		INSERT INTO @tmpOpenBal
			Select  LedgerID, 
				CASE OpenBalDrCr
					WHEN 'DEBIT' THEN OpenBal ELSE NULL
				END AS 'Debit', 
				CASE OpenBalDrCr
					WHEN 'CREDIT' THEN OpenBal ELSE NULL
				END As 'Credit'
			FROM Acc.tblOpeningBalance WHERE AccClassID=@RootAccClass;


		


		--Now convert the @AccountingClassID resultset to comma separated ids
		SET @resultCSV = '';

		SELECT @resultCSV = @resultCSV + convert(nvarchar,AccClassID) + N',' FROM @AccountingClassID;
		IF(LEN(@resultCSV)>0)
			begin
			SET @resultCSV= left(@resultCSV,len(@resultCSV)-1); --Trim one last trailing comma
			end
		ELSE
			begin 
			SET @resultCSV=''''''; --Because the IN statement accepts '' when blank. e.g. WHERE AccClassID IN ('');
			end


		SET @AccClassSTMT=' and t.TransactionID IN(
				SELECT tc.TransactionID FROM Acc.tblTransactionClass as tc 
					WHERE tc.AccClassID IN('+@resultCSV + ')) ' + @DateSettings;

	END --end of checking whether accClassID param is passed 
	
	ELSE --AccClassID is not passed
	BEGIN

		INSERT INTO @tmpOpenBal
			Select  LedgerID, 
				CASE OpenBalDrCr
					WHEN 'DEBIT' THEN OpenBal ELSE NULL
				END AS 'Debit', 
				CASE OpenBalDrCr
					WHEN 'CREDIT' THEN OpenBal ELSE NULL
				END As 'Credit'
			FROM Acc.tblOpeningBalance WHERE AccClassID=1; --Default get root's ID


	END --End Else


	SELECT * 
    INTO #tmpOpenBal
    FROM @tmpOpenBal



	SET @SqlStmt='SELECT l.LedgerID AS LedgerID, SUM(t.Debit_Amount) debit,SUM(t.credit_amount) credit, sum(o.Debit)/count(*) as OpenBalDr, sum(o.Credit)/count(*) as OpenBalCr from acc.tblLedger as l left outer join acc.tblTransaction as t on l.LedgerID=t.ledgerID '+@AccClassSTMT+' LEFT JOIN  #tmpOpenBal o on l.LedgerID=o.LedgerID   group by l.LedgerID'; 

	
	INSERT INTO @temp EXECUTE sp_executesql @SqlStmt




	--Master Query for final Execution



	--Check if Ledger ID is given, select particular Ledger information, else give all ledger
	if @LedgerID>=0
		SET @LedgerStmt=' AND t.LedgerID = ' + CONVERT(varchar,@LedgerID);

	--Select only given columns
	--Read All Accounting Class
	DECLARE @Columns TABLE
	(
		Col VARCHAR(50)
	);

	--Write to temporary table @Columns from Settings xml
	INSERT INTO @Columns(Col)
    SELECT
        IdNode.value('(.)[1]', 'int')
    FROM
        @Settings.nodes('/Settings/SelectColumns/Column') AS IdTbl(IdNode);
        
	


	--For SQL writing
	SELECT * 
    INTO #temp
    FROM @temp


	
	--Store all column names to prepare select statement
	DECLARE @SelectCol VARCHAR(500)='l.EngName LedgerName, l.LedgerCode LedgerCode, l.LedgerID LedgerID, g.EngName GroupName, g.GroupID GroupID,ISNULL(t.Debit,0) as DebitTotal,ISNULL(t.Credit,0) as CreditTotal, ISNULL(t.OpenBalDr,0),ISNULL(t.OpenBalCr,0),'; 
	
	--Select only given column in setting xml
	if exists(SELECT Col FROM @Columns WHERE UPPER(Col)='LEDGERNAME')
		SET @SelectCol=@SelectCol + 'l.EngName LedgerName' + ',';
	else if exists(SELECT Col FROM @Columns WHERE UPPER(Col)='LEDGERCODE')
		SET @SelectCol=@SelectCol + 'l.LedgerCode LedgerCode' + ',';
	else if exists(SELECT Col FROM @Columns WHERE UPPER(Col)='LEDGERID')
		SET @SelectCol=@SelectCol + 'l.LedgerID LedgerID' + ',';
	else if exists(SELECT Col FROM @Columns WHERE UPPER(Col)='GROUPNAME')
		SET @SelectCol=@SelectCol + 'g.EngName GroupName' + ',';
	else if exists(SELECT Col FROM @Columns WHERE UPPER(Col)='GROUPID')
		SET @SelectCol=@SelectCol + 'g.GroupID GroupID' + ',';
	else if exists(SELECT Col FROM @Columns WHERE UPPER(Col)='DEBITTOTAL')
		SET @SelectCol=@SelectCol + 't.Debit as DebitTotal' + ',';
	else if exists(SELECT Col FROM @Columns WHERE UPPER(Col)='CREDITTOTAL')
		SET @SelectCol=@SelectCol + 't.Credit as CreditTotal' + ',';
	else if exists(SELECT Col FROM @Columns WHERE UPPER(Col)='OPENBALDR')
		SET @SelectCol=@SelectCol + 't.OpenBalDr' + ',';
	else if exists(SELECT Col FROM @Columns WHERE UPPER(Col)='OPENBALCR')
		SET @SelectCol=@SelectCol + 't.OpenBalCr' + ',';


	--Delete last comma
	SET @SelectCol= left(@SelectCol,len(@SelectCol)-1);

	--Check if specific group is present	
	--Check if specific ledger is present
	SET @LedgerStmt='';
	if(@LedgerID>0)
		SET @LedgerStmt=' AND t.LedgerID=' + CONVERT(VARCHAR,@LedgerID);

	SET @SqlStmt='SELECT ' + @SelectCol + 
		' FROM acc.tblLedger l LEFT OUTER JOIN #temp t ON t.LedgerID = l.LedgerID
		 JOIN acc.tblGroup g  ON l.GroupID=g.GroupID' + @LedgerStmt;


INSERT INTO @LedgerDetails
	EXECUTE sp_executesql @SqlStmt





declare @ID int=0
declare @DebitAmount int
declare @CreditAmount int
declare @DrOpenBal int
declare @CrOpenBal int
declare @str nvarchar(max)='';

--get all group from tblGroup
declare @tblGroup as table(groupID int,parentID int) 
insert into @tblGroup
select GroupID,Parent_GrpID from Acc.tblGroup

declare @tempGroupID as table(grpID int)

declare  @finalTable as table(AccountName nvarchar(50),AccountCode nvarchar(50),AccountID int,ParentGroupID int,DebitTotal decimal,CreditTotal decimal,OpenBalDr decimal,OpenBalCr decimal,	AccountType nvarchar(20))


select  top 1 @ID=isnull(GroupID,0) from @tblGroup

while(@ID!=0)
begin
insert into @tempGroupID values(@ID)

;with CteGroupID(groupID)
 as(select GroupID from acc.tblGroup where Parent_GrpID=@ID
 union all
 select acc.tblGroup.GroupID from acc.tblGroup,CteGroupID where Parent_GrpID=CteGroupID.groupID)
 --get and insert all child group id of current group id
 insert into @tempGroupID
 select *from CteGroupID

 select @DebitAmount=isnull(sum(DebitTotal),0) from @LedgerDetails where GroupID in(select *from @tempGroupID)
  select @CreditAmount=isnull(sum(CreditTotal),0) from @LedgerDetails where GroupID in(select *from @tempGroupID)
   select @DrOpenBal=isnull(sum(OpenBalDr),0) from @LedgerDetails where GroupID in(select *from @tempGroupID)
      select @CrOpenBal=isnull(sum(OpenBalCr),0) from @LedgerDetails where GroupID in(select *from @tempGroupID)

insert into @finalTable 
select EngName,LedgerCode,GroupID,isnull(Parent_GrpID,0),@DebitAmount,@CreditAmount,@DrOpenBal,@CrOpenBal,'GROUP' from Acc.tblGroup where GroupID=@ID

 delete from @tblGroup where groupID=@ID
 delete from @tempGroupID
 set @ID=0
 select  top 1 @ID=isnull(GroupID,0) from @tblGroup;
 
 end
 insert into @finalTable
 select LedgerName,LedgerCode,LedgerID,GroupID,DebitTotal,CreditTotal,OpenBalDr,OpenBalCr,'LEDGER' from @LedgerDetails

 select *from @finalTable

end


GO


