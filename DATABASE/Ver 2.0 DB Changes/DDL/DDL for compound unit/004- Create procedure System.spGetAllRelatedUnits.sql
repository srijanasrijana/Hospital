--USE [BENT003]
--GO
/****** Object:  StoredProcedure [System].[spGetAllRelatedUnits]    Script Date: 2017-08-15 10:47:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 create procedure [System].[spGetAllRelatedUnits](
	@UnitID int = 0,
	@ProductID int = null
 )

 as 
 begin
	
	--declare @UnitID int = 32;
	declare @tblUnits table 
	(
		UnitID int,
		UnitName nvarchar(100)
	);

	if(@ProductID is not null)
		select @UnitID = UnitMaintenanceID from Inv.tblProduct where ProductID = @ProductID;

	if(@ProductID =0 and @UnitID = 0)
		set @UnitID = 22;

	if((select count(*) from System.tblCompoundUnit where UnitID = @UnitID or ParentUnitID = @UnitID) <= 0) -- if the given unit is not saved in compound unit table,
																					-- it means that it does not have relation with other units, 
																					--so load only the unit
		begin
		insert into @tblUnits
			select UnitMaintenanceID UnitID, UnitName  from System.tblUnitMaintenance where UnitMaintenanceID = @UnitID; 
		end

else
	begin
		-- inserts its own record into the table before finding ancestors and successor records
		insert into @tblUnits 
			select  UnitID, (select UnitName from System.tblUnitMaintenance um where um.UnitMaintenanceID = cu.UnitID) as UnitName
			 from System.tblCompoundUnit cu where UnitID = @UnitID --UnitID
			union 
			select  ParentUnitID, (select UnitName from System.tblUnitMaintenance um where um.UnitMaintenanceID = cu.ParentUnitID) as UnitName
			 from System.tblCompoundUnit cu where ParentUnitID = @UnitID; --UnitID


		 -- this common table expression gets all the records of successor/child 
		with CteChildUnitID(unitID, UnitName)
		 as(select UnitID, (select UnitName from System.tblUnitMaintenance um where um.UnitMaintenanceID = cu.UnitID) as UnitName
			 from System.tblCompoundUnit cu where ParentUnitID = @UnitID --UnitID
		 union all
		 select cu.UnitID,  (select UnitName from System.tblUnitMaintenance um where um.UnitMaintenanceID = cu.UnitID) as UnitName
		  from System.tblCompoundUnit cu ,CteChildUnitID where cu.ParentUnitID = CteChildUnitID.unitID)
			insert  into @tblUnits  
				select * from CteChildUnitID;

			--	select * from @tblUnits;

		-- this common table expression gets all the records of ancesrtors
		with CteParentID(unitID, UnitName)
		 as(select ParentUnitID, (select UnitName from System.tblUnitMaintenance um where um.UnitMaintenanceID = cu.ParentUnitID) as UnitName
			 from System.tblCompoundUnit cu where UnitID = @UnitID --UnitID
		 union all
		 select cu.ParentUnitID,  (select UnitName from System.tblUnitMaintenance um where um.UnitMaintenanceID = cu.ParentUnitID) as UnitName
		  from System.tblCompoundUnit cu ,CteParentID where cu.UnitID = CteParentID.unitID)
			insert  into @tblUnits  
				select * from CteParentID;
	end

		select distinct * from @tblUnits;
end