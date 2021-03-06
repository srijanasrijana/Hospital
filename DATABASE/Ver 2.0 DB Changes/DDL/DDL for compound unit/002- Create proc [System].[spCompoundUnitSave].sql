
/****** Object:  StoredProcedure [System].[spCompoundUnitSave]    Script Date: 2017-08-11 12:25:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [System].[spCompoundUnitSave]
(
	@CompoundUnitID int = null,
	@UnitID int,
	@ParentUnitID int,
	@RelationValue decimal(18,5),
	@Remarks nvarchar(500),
	@CreatedBy int = null,
	@CreatedDate datetime = null,
	@MidifiedBy int = null,
	@MdifiedDate datetime = null,
	@IsNew bit = 1
)
as
begin
--select * from System.tblCompoundUnit
	if(@IsNew = 1) -- when new compound unit relation is created
	begin
			insert into System.tblCompoundUnit(UnitID,ParentUnitID, RelationValue,Remarks, CreatedBy, CreatedDate)
										values(@UnitID, @ParentUnitID, @RelationValue, @Remarks, @CreatedBy, @CreatedDate)
	end

	else -- when existing compound unit relation is modified
	begin
			update System.tblCompoundUnit set 
			UnitID= @UnitID,
			 @ParentUnitID = @ParentUnitID, 
			 RelationValue = @RelationValue, 
			 ModifiedBy = @MidifiedBy, 
			 ModifiedDate = @MdifiedDate 
			 where 
			 CompoundUnitID = @CompoundUnitID;
	end
end