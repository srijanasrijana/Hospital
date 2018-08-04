

/****** Object:  Table [Acc].[tblBudget]    Script Date: 10/26/2016 11:38:29 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [Acc].[tblBudget](
	[budgetID] [int] IDENTITY(1,1) NOT NULL,
	[budgetName] [nvarchar](50) NOT NULL,
	[startDate] [date] NOT NULL,
	[endDate] [date] NOT NULL,
	[description] [nvarchar](100) NULL,
	[isActive] [bit] NULL,
	[isBuiltIn] [bit] NULL,
 CONSTRAINT [PK_tblBudget] PRIMARY KEY CLUSTERED 
(
	[budgetID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


