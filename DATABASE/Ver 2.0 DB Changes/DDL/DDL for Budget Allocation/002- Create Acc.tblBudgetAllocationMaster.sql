

/****** Object:  Table [Acc].[tblBudgetAllocationMaster]    Script Date: 10/26/2016 11:40:07 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [Acc].[tblBudgetAllocationMaster](
	[BudgetMasterID] [int] IDENTITY(1,1) NOT NULL,
	[BudgetID] [int] NOT NULL,
	[AccountID] [int] NULL,
	[AccountType] [nvarchar](50) NULL,
	[TotalAllocationForAccount] [decimal](18, 4) NULL,
 CONSTRAINT [PK_tblBudgetAllocationMaster] PRIMARY KEY CLUSTERED 
(
	[BudgetMasterID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [Acc].[tblBudgetAllocationMaster]  WITH CHECK ADD  CONSTRAINT [FK_tblBudgetAllocationMaster_tblBudget] FOREIGN KEY([BudgetID])
REFERENCES [Acc].[tblBudget] ([budgetID])
GO

ALTER TABLE [Acc].[tblBudgetAllocationMaster] CHECK CONSTRAINT [FK_tblBudgetAllocationMaster_tblBudget]
GO


