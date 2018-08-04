

/****** Object:  StoredProcedure [Acc].[spCashFlowReport]    Script Date: 3/9/2017 4:07:19 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





CREATE procedure [Acc].[spCashFlowReport]
(@Transaction_Start_Date DATETIME=NULL,
@Transaction_End_Date DATETIME=NULL,
@AccountClassIDsSettings  xml=NULL,    -- It encludes AccountCLassIDs  Info
@ProjectIDsSettings xml=NULL, --Null means all
@Settings xml=NULL) --Null means all)  
as
begin
declare @ID int=0
declare @AmountIn DECIMAL(19,5);
declare @AmountOut DECIMAL(19,5);

declare @tblCashLedgerIDs table(CldgID int);
declare @TemptblGroupIDs table(TGrpID int);
declare @tblcashGroupIDs table(CGrpID int);


insert into @tblcashGroupIDs values(102)--cashinhand group
insert into @tblcashGroupIDs values(7) --banks group

--get all ledgers under cashinhand and banks group 
 ;WITH CteCHgrpID(groupID)
 as(select GroupID from acc.tblGroup where Parent_GrpID in(102,7)
 union all
 select acc.tblGroup.GroupID from acc.tblGroup,CteCHgrpID where Parent_GrpID=CteCHgrpID.groupID)

 INSERT INTO @tblcashGroupIDs SELECT *FROM CteCHgrpID
  INSERT INTO @tblCashLedgerIDs SELECT LedgerID FROM ACC.tblLedger WHERE GroupID IN(SELECT *FROM @tblcashGroupIDs)

declare @ldgID int=0
declare @TemptblTransact table(Account NVARCHAR(50) ,AccountID INT ,AmountInFlow Decimal(19,5),AmountOutFlow Decimal(19,5),GroupID int ,[Type] nvarchar(20) )
declare @tblTransact table(Account NVARCHAR(50) ,AccountID INT ,AmountInFlow Decimal(19,5),AmountOutFlow Decimal(19,5),GroupID int ,[Type] nvarchar(20) )
declare @tblFinal table(Account NVARCHAR(50) ,AccountID INT,AmountInFlow Decimal(19,5),AmountOutFlow Decimal(19,5),GroupID int,[Type] nvarchar(20))

--go through all ledgers under cashin hand and bank group and get all of their transactions
DECLARE db_cursor CURSOR FOR select distinct CldgID from @tblCashLedgerIDs 
OPEN db_cursor FETCH NEXT FROM db_cursor INTO @ldgID  

WHILE @@FETCH_STATUS = 0   
BEGIN
insert into @TemptblTransact
select a.Account,a.LedgerID,isnull(sum(a.Debit),0),isnull(sum(a.Credit),0),a.GroupID,'LEDGER' from Acc.fnGetLedgerTransaction (@ldgID,@Transaction_Start_Date,@Transaction_End_Date,@AccountClassIDsSettings,@ProjectIDsSettings) as a group by a.Account,a.LedgerID,a.GroupID
FETCH NEXT FROM db_cursor INTO @ldgID
END
CLOSE db_cursor   
DEALLOCATE db_cursor


--finnally get all ledgers that make cash and bank flow 
insert into  @tblTransact
select a.Account,a.AccountID,isnull(sum(a.AmountInFlow),0),isnull(sum(a.AmountOutFlow),0),a.GroupID,'LEDGER' from @TemptblTransact as a group by a.Account,a.AccountID,a.GroupID

--delete all cash and banks ledgers 
delete from @tblTransact where GroupID in(select *from @tblcashGroupIDs)

--get all groups except cash and bank groups
DECLARE db_cursor CURSOR FOR select GroupID from Acc.tblGroup where GroupID not in(select *from @tblcashGroupIDs)  
OPEN db_cursor FETCH NEXT FROM db_cursor INTO @ID  

WHILE @@FETCH_STATUS = 0   
BEGIN
--now loop through each group in @tblgroupIDs table and obtain their value
--select top 1 @ID=isnull(GrpID,0) from @tblGroupIDs

insert into @TemptblGroupIDs values(@ID)
 ;with CteGroupID(groupID)
 as(select GroupID from acc.tblGroup where Parent_GrpID=@ID
 union all
 select acc.tblGroup.GroupID from acc.tblGroup,CteGroupID where Parent_GrpID=CteGroupID.groupID)
 --get and insert all child group id of current group id
 insert into @TemptblGroupIDs
 select *from CteGroupID
 select @AmountIn=sum(AmountInFlow),@AmountOut=sum(AmountOutFlow) from @tblTransact where GroupID in(select *from @TemptblGroupIDs)

insert into @tblFinal 
select g.EngName,g.GroupID,@AmountIn,@AmountOut,g.Parent_GrpID,'GROUP'  from acc.tblGroup g where g.GroupID=@ID

--delete from @tblGroupIDs  where GrpID=@ID
delete from @TemptblGroupIDs
--set @ID=0
set @AmountIn=0
set @AmountOut=0
FETCH NEXT FROM db_cursor INTO @ID
--select top 1 @ID=isnull(GrpID,0) from @tblGroupIDs
END

CLOSE db_cursor   
DEALLOCATE db_cursor

insert into @tblFinal
select *from @tblTransact

select Account,AccountID,isnull(AmountInFlow,0) as AmountInFlow, isnull(AmountOutFlow,0)as AmountOutFlow,isnull(GroupID,0) as GroupID,[Type] from @tblFinal where  AmountInFlow!=0 OR AmountOutFlow!=0
end









GO


