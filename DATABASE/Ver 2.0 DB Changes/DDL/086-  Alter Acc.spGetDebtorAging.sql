

/****** Object:  StoredProcedure [Acc].[spGetDebtorAging]    Script Date: 4/12/2017 2:33:03 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

















/*Desc: Returns all the Ledger Details with the columns LedgerID, LedgerName, Debit, Credit, OpBalDr, OpBalCr, Remarks
Author: Shamit Shrestha
Ver: 2.0
Modified Date: 22nd December, 2016
Parameters: 
Transaction_Start_Date = '2015-11-10'
Transaction_End_Date = '2015-11-31'
LedgerID = 102 or NULL
AccountClassIDsSettings = <AccClassIDSettings><AccClassID>1</AccClassID><AccClassID>15</AccClassID></AccClassIDSettings>
ProjectIDsSettings = <ProjectIDSettings><ProjectID>1</ProjectID><ProjectID>2</ProjectID></ProjectIDSettings>
Settings = <Settings><SelectColumns><Column>LedgerID</Column><Column>Debit</Column><Column>Credit</Column></SelectColumns></Settings>
GroupID=14 or NULL
*/

ALTER  PROCEDURE [Acc].[spGetDebtorAging]
(
@Transaction_Start_Date DATETIME=NULL,
@Transaction_End_Date DATETIME=NULL,
@LedgerID INT=null, --For specific ledger
@AccountClassIDsSettings  xml=NULL,    -- It encludes AccountCLassIDs  Info
@ProjectIDsSettings xml=NULL, --Null means all
@GroupID INT=NULL,

@Settings xml=NULL --Null means all
)
AS
	--SET NOCOUNT ON	
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
	if(@GroupID is NULL)
	begin
	set @GroupID=0
	end
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
	DECLARE @SelectCol VARCHAR(500)='l.EngName LedgerName, l.LedgerCode LedgerCode, l.LedgerID LedgerID, g.EngName GroupName, g.GroupID GroupID,ISNULL(t.Debit,0) as DebitTotal,ISNULL(t.Credit,0) as CreditTotal, ISNULL(t.OpenBalDr,0) as OpenBalDr,ISNULL(t.OpenBalCr,0) as OpenBalCr,'; 
	
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
	if(@GroupID<=0 or @GroupID=null)
	begin
	--Check if specific ledger is present
	SET @LedgerStmt='';
	if(@LedgerID>0)
		SET @LedgerStmt=' AND t.LedgerID=' + CONVERT(VARCHAR,@LedgerID);

	SET @SqlStmt='SELECT ' + @SelectCol + 
		' FROM acc.tblLedger l LEFT OUTER JOIN #temp t ON t.LedgerID = l.LedgerID
		 JOIN acc.tblGroup g  ON l.GroupID=g.GroupID' + @LedgerStmt;
end
else
begin
	SET @LedgerStmt='';

create table #tempLdgID(ledgerID int)
insert into #tempLdgID
select LedgerID from Acc.tblLedger where GroupID=@GroupID
 ;with CteGroupID(groupID)
 as(select GroupID from acc.tblGroup where Parent_GrpID=@GroupID
 union all
 select acc.tblGroup.GroupID from acc.tblGroup,CteGroupID where Parent_GrpID=CteGroupID.groupID)
 insert into #tempLdgID 
 select LedgerID from Acc.tblLedger where GroupID in(select *from CteGroupID)
 
 		SET @LedgerStmt=' and l.LedgerID in(select *from #tempLdgID )' ;

		SET @SqlStmt='SELECT ' + @SelectCol + 
		' FROM acc.tblLedger l LEFT OUTER JOIN #temp t ON t.LedgerID = l.LedgerID
		 JOIN acc.tblGroup g  ON l.GroupID=g.GroupID' + @LedgerStmt;
end
Declare @tblDue table(duID int,amount decimal(19,5),dueDate datetime);
Declare @Amount decimal(19,5)=0;
Declare @TempAmount decimal (19,5)=0;
Declare @dueID int;
Declare @MLedgerID int=0;
Declare @VoucherType nvarchar(20);
Declare @tblldgFinal table(LedgerID int,LedgerName nvarchar(50),TotalAmount Decimal(19,5));
Declare @tblFinal table(id int identity(1,1),LedgerID int,DueDate datetime,OverDueDay int,Amount decimal(19,5),VoucherType nvarchar(20),RowID int)
Declare @tblDetails table(LedgerName nvarchar(50),LedgerCode nvarchar(20),LedgerID int,GroupName nvarchar(50),GroupID int,DebitTotal decimal(19,5),CreditTotal decimal(19,5),OpenBalDr decimal(19,5),OpenBalCr decimal(19,5))
Insert into @tblDetails
	EXECUTE sp_executesql @SqlStmt

	 if(NULLIF(@Transaction_End_Date, '') is null)
		begin
			SET @Transaction_End_Date=GETDATE();
		end

	Declare db_Maincursor CURSOR FOR SELECT distinct LedgerID from @tblDetails
	Open db_Maincursor fetch next from db_Maincursor into @MLedgerID
	 while @@FETCH_STATUS=0
	          BEGIN

	select @Amount=isnull((sum(DebitTotal+OpenBalDr)-sum(CreditTotal+OpenBalCr)),0) from @tblDetails where LedgerID=@MLedgerID
	--select *from @tblDetails
	
	     IF (@Amount>0)
	     BEGIN
		 delete from @tblDue;
		 insert into @tblDue
		 SELECT distinct  DueDateID,t.Debit_Amount,d.DueDate from System.tblDueDate d inner join acc.tblTransaction t on d.LedgerID=t.LedgerID and d.RowID=t.RowID and d.VoucherType=t.VoucherType inner join  acc.tblTransactionClass tc on t.TransactionID=tc.TransactionID  where d.LedgerID=@MLedgerID and t.TransactDate<=@Transaction_End_Date and tc.AccClassID=@RootAccClass 
		 insert into @tblDue
		 SELECT d.DueDateID,op.OpenBal,d.DueDate from System.tblDueDate d inner join Acc.tblOpeningBalance op on  d.LedgerID=op.LedgerID where d.VoucherType='OPBAL' AND op.LedgerID=@MLedgerID and op.OpenBalDrCr='DEBIT' and op.AccClassID=@RootAccClass

	          Declare db_cursor CURSOR FOR SELECT duID from @tblDue  order by dueDate DESC
	          Open db_cursor fetch next from db_cursor into @dueID
	          
	          while @@FETCH_STATUS=0
	          BEGIN

			  --select @VoucherType=VoucherType from System.tblDueDate where DueDateID=@dueID

			  --if(@VoucherType='OPBAL')--for opening balance
			  --BEGIN
			  --  SELECT @TempAmount=amount from @tblDue where duID=@dueID 

				 --if(@Amount<=@TempAmount)
	    --        BEGIN
     --              insert into @tblFinal 
		   --        select d.LedgerID,l.EngName,d.DueDate, DATEDIFF(DAY,cast(op.OpenBalDate as date),GETDATE()),@Amount,d.VoucherType,isnull(d.RowID,0) from System.tblDueDate d inner join Acc.tblOpeningBalance op on d.LedgerID=op.LedgerID inner join Acc.tblLedger l on d.LedgerID=l.LedgerID  where  d.LedgerID=@dueID 
	    --           set @Amount=0;
		   --        set @TempAmount=0
		   --        Break;
		   --     END
		        
		   --     ELSE
		   --     BEGIN
		   --        insert into @tblFinal 
		   --        select d.LedgerID,l.EngName,d.DueDate, DATEDIFF(DAY,cast(op.OpenBalDate as date),GETDATE()),@TempAmount,d.VoucherType,isnull(d.RowID,0) from System.tblDueDate d inner join Acc.tblOpeningBalance op on d.LedgerID=op.LedgerID inner join Acc.tblLedger l on d.LedgerID=l.LedgerID  where  d.LedgerID=@dueID  
		   --        set @Amount=@Amount-@TempAmount
		   --        set @TempAmount=0
		   --        FETCH NEXT FROM db_cursor into @dueID
		   --     END 

			  --END

			  --ELSE
			  --BEGIN
	              -- select @TempAmount=t.Debit_Amount from Acc.tblTransaction t inner join System.tblDueDate d on t.RowID=d.RowID and t.VoucherType=d.VoucherType and t.LedgerID=d.LedgerID
	              --inner join Acc.tblTransactionClass c on  t.TransactionID=c.TransactionID where d.DueDateID=@dueID and c.AccClassID=@RootAccClass and t.TransactDate<=@Transaction_End_Date
	           SELECT @TempAmount=amount from @tblDue where duID=@dueID 

			    if(@Amount<=@TempAmount)
	            BEGIN
                   insert into @tblFinal 
		           select d.LedgerID,d.DueDate, DATEDIFF(DAY,cast(d.DueDate as date),GETDATE()),@Amount,d.VoucherType,isnull(d.RowID,0) from System.tblDueDate d  where d.DueDateID=@dueID  --inner join Acc.tblTransaction t on  d.RowID=t.RowID and  d.RowID=t.RowID and d.VoucherType=t.VoucherType and d.LedgerID=t.LedgerID  inner join Acc.tblLedger l on d.LedgerID=l.LedgerID
	               set @Amount=0;
		           set @TempAmount=0
		           Break;
		        END
		        
		        ELSE
		        BEGIN
		           insert into @tblFinal 
		           select d.LedgerID,d.DueDate, DATEDIFF(DAY,cast(d.DueDate as date),GETDATE()),@TempAmount,d.VoucherType,isnull(d.RowID,0) from System.tblDueDate d  where d.DueDateID=@dueID --inner join Acc.tblTransaction t on  d.RowID=t.RowID and  d.RowID=t.RowID and d.VoucherType=t.VoucherType and d.LedgerID=t.LedgerID  inner join Acc.tblLedger l on d.LedgerID=l.LedgerID
		           set @Amount=@Amount-@TempAmount
		           set @TempAmount=0
		           FETCH NEXT FROM db_cursor into @dueID
		        END    
				
	      -- END
 
	       END
		   CLOSE db_cursor
		   DEALLOCATE db_cursor

		   if(@Amount>0)
		   BEGIN
		  update @tblFinal set Amount=Amount+@Amount where id in (select top 1 id from @tblFinal where LedgerID=@MLedgerID order by DueDate ASC)
		   END
		 set @Amount=0;

	END

	FETCH NEXT FROM db_Maincursor into @MLedgerID
	END
	CLOSE db_Maincursor
	 DEALLOCATE db_Maincursor
	 
	 insert into @tblldgFinal
	 select ds.LedgerID,l.EngName,isnull((sum(DebitTotal+OpenBalDr)-sum(CreditTotal+OpenBalCr)),0) from @tblDetails ds inner join Acc.tblLedger l on ds.LedgerID=l.LedgerID   group by ds.LedgerID,l.EngName having (isnull((sum(DebitTotal+OpenBalDr)-sum(CreditTotal+OpenBalCr)),0))>0

			 			   select  f.LedgerID,l.LedgerName,l.TotalAmount,f.DueDate,f.OverDueDay,f.Amount,f.VoucherType,f.RowID,t.TransactDate,Date.fnEngtoNep(t.TransactDate) as NepTransactDate,Date.fnEngtoNep(f.DueDate) as NepDueDate from @tblFinal as f inner join @tblldgFinal l on f.LedgerID=l.LedgerID left  join acc.tblTransaction t on f.RowID=t.RowID and f.LedgerID=t.LedgerID and f.VoucherType=t.VoucherType 
              select *from @tblldgFinal

			   
GO


