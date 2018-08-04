
/****** Object:  Table [Acc].[tblBudgetAllocationDetail]    Script Date: 10/26/2016 11:39:36 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [Acc].[tblBudgetAllocationDetail](
	[BudgetdetailID] [int] IDENTITY(1,1) NOT NULL,
	[BudgetMasterID] [int] NULL,
	[ClassID] [int] NULL,
	[Amount] [decimal](18, 0) NULL,
 CONSTRAINT [PK_tblBudgetAllocationDetail] PRIMARY KEY CLUSTERED 
(
	[BudgetdetailID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [Acc].[tblBudgetAllocationDetail]  WITH CHECK ADD  CONSTRAINT [FK_tblBudgetAllocationDetail_tblAccClass] FOREIGN KEY([ClassID])
REFERENCES [Acc].[tblAccClass] ([AccClassID])
GO

ALTER TABLE [Acc].[tblBudgetAllocationDetail] CHECK CONSTRAINT [FK_tblBudgetAllocationDetail_tblAccClass]
GO

ALTER TABLE [Acc].[tblBudgetAllocationDetail]  WITH CHECK ADD  CONSTRAINT [FK_tblBudgetAllocationDetail_tblBudgetAllocationMaster] FOREIGN KEY([BudgetMasterID])
REFERENCES [Acc].[tblBudgetAllocationMaster] ([BudgetMasterID])
GO

ALTER TABLE [Acc].[tblBudgetAllocationDetail] CHECK CONSTRAINT [FK_tblBudgetAllocationDetail_tblBudgetAllocationMaster]
GO


