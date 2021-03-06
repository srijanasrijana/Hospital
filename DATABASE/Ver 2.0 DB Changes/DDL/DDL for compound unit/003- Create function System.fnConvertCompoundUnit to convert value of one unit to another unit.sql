--USE [BENT003]
--GO
/****** Object:  UserDefinedFunction [System].[fnConvertCompoundUnit]    Script Date: 5/30/2017 10:48:28 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

create function [System].[fnConvertCompoundUnit](
	@defaultUnitID int,
	@curUnitID int,
	@actualValue decimal(20,5) 
)
 Returns decimal(20,5) 
begin
--declare @defaultUnitID int = 32; 
--declare @curUnitID int = 22;
--declare @actualVal decimal(20,4) = 1000;
-- these 3 to be made parameters

declare @parentID int;
declare @childID int;
declare @parentVal decimal(20,5);
declare @childVal decimal(20,5);
declare @val decimal(20,5) = 1;
set @parentID = @curUnitID;
declare  @isParent bit = 0;

while exists(select ParentUnitID, RelationValue from System.tblCompoundUnit where UnitID = @parentID)
begin
	select @parentID = ParentUnitID, @parentVal = RelationValue  from System.tblCompoundUnit where UnitID = @parentID;
		set @Val = @Val * @parentVal;
	if(@parentID = @defaultUnitID)
	begin
		set @actualValue = @actualValue / @val;
		set @isParent = 1;
		break;
		-- break the while loop
	end
end

if(@isParent = 0)  -- if the default unit is not found amongst the ancestors or parents then it must be child 
					-- so reverse the values of default unit and current unit and perform similar operation

declare @temp decimal(20,5) = @curUnitID;
set @curUnitID = @defaultUnitID;
set @defaultUnitID = @temp;
set @parentID = @curUnitID; 
begin
	while exists(select ParentUnitID, RelationValue from System.tblCompoundUnit where UnitID = @parentID)
begin
	select @parentID = ParentUnitID, @parentVal = RelationValue  from System.tblCompoundUnit where UnitID = @parentID;
		set @Val = @Val * @parentVal;
	if(@parentID = @defaultUnitID)
	begin
		set @actualValue = @actualValue * @val;
		set @isParent = 1;
		break;
		-- break the while loop
	end
end
end

return @actualValue;
end