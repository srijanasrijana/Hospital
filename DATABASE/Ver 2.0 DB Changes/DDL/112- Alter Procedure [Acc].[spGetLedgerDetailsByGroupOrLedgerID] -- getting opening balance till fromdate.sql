
/****** Object:  StoredProcedure [Acc].[spGetLedgerDetailsByGroupOrLedgerID]    Script Date: 09/06/2017 2:19:40 PM ******/
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

ALTER  PROCEDURE [Acc].[spGetLedgerDetailsByGroupOrLedgerID]
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
	SET NOCOUNT ON	
	DECLARE @docHandle int;
	DECLARE @docHandle1 int;
	EXEC sp_xml_preparedocument @docHandle OUTPUT, @AccountClassIDsSettings;
	EXEC sp_xml_preparedocument @docHandle1 OUTPUT, @ProjectIDsSettings;

	DECLARE @tblOpeningBal table(ldgID int,opnBal decimal(19,5))
	DECLARE @tblTransBal table(ldgID int,transBal decimal(19,5))
	Declare @tblTransAmount table(ldgID int,Debit decimal(19,5), Credit decimal(19,5))


		if(@GroupID is NULL)
	set @GroupID=0

		if(@LedgerID is NULL)
	set @GroupID=0

	if(@Transaction_Start_Date is null)
	select top 1 @Transaction_Start_Date=BookBeginFrom from System.tblCompanyInfo

	if(@Transaction_End_Date is null)
	set @Transaction_End_Date=GETDATE();

	--Get all Child accounting Class Ids
	Declare @AccountingClassID TABLE
	(
		AccClassID INT
	);

	Declare @TransactStmt nvarchar(max);
	--To store Date settings in dynamic SQL Query
	DECLARE @DateSettings NVARCHAR(max);
	--To temporarily store debit and credit amount of the ledger
	
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
	DECLARE @RootAccClass INT=1; --Hold Root Accounting Class. By default it is ROOT (ID =1)
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

    End		


	--get LedgerIDs
	Declare @tblLedgerID table(ldgID int)

	if(@LedgerID !=0) --if ledger id is provided insert only that value
	insert into @tblLedgerID values(@LedgerID)
	 
	 else if(@LedgerID=0 and @GroupID!=0)--get all ledger id with in that group (including ledger under nested group)
	 begin

	           insert into @tblLedgerID
                   select LedgerID from Acc.tblLedger where GroupID=@GroupID
	           
                --get all group under that group (including group under nested group)
               ;with CteGroupID(groupID)
               as(select GroupID from acc.tblGroup where Parent_GrpID=@GroupID
               union all
               select acc.tblGroup.GroupID from acc.tblGroup,CteGroupID where Parent_GrpID=CteGroupID.groupID)
	           
               --get all ledger ids
               insert into @tblLedgerID 
               select LedgerID from Acc.tblLedger where GroupID in(select *from CteGroupID)

	 end

	else 
	insert into @tblLedgerID select LedgerID from Acc.tblLedger


	--get all ledgers opening Balance

	insert into @tblOpeningBal
   select l.ldgID, Case  when (o.OpenBalDrCr='DEBIT') then isnull(o.OpenBal,0) else isnull((0-o.OpenBal),0)  end from  @tblLedgerID l left join Acc.tblOpeningBalance o  on l.ldgID= o.LedgerID and o.AccClassID=@RootAccClass


   if(@Transaction_Start_Date is not null)
   begin
        insert into @tblTransBal
        select l.ldgID, (sum(isnull(t.Debit_Amount,0))-sum(isnull(t.Credit_Amount,0))) from @tblLedgerID l left join  Acc.tblTransaction t   on l.ldgID= t.LedgerID where  t.TransactDate < @Transaction_Start_Date group by l.ldgID
       
        --update @tblOpening Balance
       
        update o set o.opnBal=(o.opnBal+t.transBal)  from @tblOpeningBal o inner join @tblTransBal t on o.ldgID=t.ldgID -- all posetive values are debit and negetive are credit
   end


	--get only transaction amounts between the date
	
	--temporary purpose

	insert into @tblTransAmount
	select l.ldgID, ISNULL(a.Debit,0), ISNULL(a.Credit,0) from @tblLedgerID l left outer join
	 (select t.LedgerID as LedgerID, sum(isnull(t.Debit_Amount,0)) as Debit,SUM(isnull(t.Credit_Amount,0)) as Credit from  Acc.tblTransaction t  where  t.TransactDate between @Transaction_Start_Date and @Transaction_End_Date   group by t.LedgerID) as a
	 on l.ldgID=a.LedgerID

	
	select l.EngName as LedgerName, l.LedgerCode, ob.ldgID as LedgerID , g.EngName as GroupName, g.GroupID, tamt.Debit as DebitTotal , tamt.Credit as CreditTotal,
	case when ob.opnBal>0 then opnBal else 0 end as OpenBalDr,
		case when ob.opnBal<0 then (0-opnBal) else 0 end as OpenBalCr,
		ABS((tamt.Debit-tamt.Credit)+ob.opnBal) as FinalBal,

		case when ((tamt.Debit-tamt.Credit)+ob.opnBal)>=0 then 'DEBIT' else 'CREDIT' end as DrCr,
		l.DrCr as LedgerType

	 from  @tblOpeningBal ob inner join @tblTransAmount tamt on ob.ldgID=tamt.ldgID inner join Acc.tblLedger l on ob.ldgID=l.LedgerID inner join Acc.tblGroup g on l.GroupID=g.GroupID  order by l.EngName


















	





