--USE [BENT003]
--GO

/****** Object:  Table [System].[tblCompoundUnit]    Script Date: 5/29/2017 3:23:55 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [System].[tblCompoundUnit](
	[CompoundUnitID] [int] IDENTITY(1,1) NOT NULL,
	[UnitID] [int] NULL,
	[ParentUnitID] [int] NULL,
	[RelationValue] [decimal](18, 5) NULL,
	[Remarks] [nvarchar](500) NULL,
	[CreatedBy] [int] NULL,
	[ModifiedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[ModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_tblCompoundUnit] PRIMARY KEY CLUSTERED 
(
	[CompoundUnitID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [System].[tblCompoundUnit]  WITH CHECK ADD  CONSTRAINT [FK_tblCompoundUnit_tblCompoundUnit] FOREIGN KEY([UnitID])
REFERENCES [System].[tblUnitMaintenance] ([UnitMaintenanceID])
GO

ALTER TABLE [System].[tblCompoundUnit] CHECK CONSTRAINT [FK_tblCompoundUnit_tblCompoundUnit]
GO

ALTER TABLE [System].[tblCompoundUnit]  WITH CHECK ADD  CONSTRAINT [FK_tblCompoundUnit_tblUnitMaintenance] FOREIGN KEY([ParentUnitID])
REFERENCES [System].[tblUnitMaintenance] ([UnitMaintenanceID])
GO

ALTER TABLE [System].[tblCompoundUnit] CHECK CONSTRAINT [FK_tblCompoundUnit_tblUnitMaintenance]
GO


