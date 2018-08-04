
/****** Object:  StoredProcedure [Acc].[spFilterLedgersOrGroupByGroupID]    Script Date: 4/4/2017 2:49:37 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





CREATE PROCEDURE [Acc].[spFilterLedgersOrGroupByGroupID]
(@LedgersID XML,@ChildGroupID int=0,@GroupID int)
AS
BEGIN
SET NOCOUNT ON

Declare @tblGroup table(grpID int)
INSERT INTO @tblGroup VALUES(@GroupID);

;with cteGroupIDs(GroupID)
as(select GroupID from Acc.tblGroup where Parent_GrpID=@GroupID union all 
select Acc.tblGroup.GroupID from Acc.tblGroup,cteGroupIDs where Parent_GrpID=cteGroupIDs.GroupID)
INSERT INTO @tblGroup SELECT * FROM cteGroupIDs


IF(@ChildGroupID=0)
BEGIN
 	DECLARE @docHandle int;
	EXEC sp_xml_preparedocument @docHandle OUTPUT, @LedgersID;

	DECLARE @tblLedgerIDs table(ldgID int)
	DECLARE @tblLedgersUnderGroup table(ldgID int)

	INSERT INTO @tblLedgerIDs(ldgID)
    SELECT
        IdNode.value('(.)[1]', 'int')
    FROM
        @LedgersID.nodes('/LedgerIDs/LedgerID') AS IdTbl(IdNode);

		--now get all ledgers under given group 

insert into @tblLedgersUnderGroup select LedgerID from Acc.tblLedger where GroupID in(select *from @tblGroup)

select lg.ldgID as LedgerID,l.EngName as LedgerName from @tblLedgersUnderGroup lg inner join @tblLedgerIDs li on lg.ldgID=li.ldgID inner join acc.tblLedger l on lg.ldgID=l.LedgerID 
END

ELSE
BEGIN
SELECT t.grpID as GroupID,g.EngName as GroupName  from @tblGroup t INNER JOIN Acc.tblGroup g on t.grpID=g.GroupID where t.grpID=@ChildGroupID
END
END

GO


