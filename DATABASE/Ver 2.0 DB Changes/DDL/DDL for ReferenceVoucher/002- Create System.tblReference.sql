
GO

/****** Object:  Table [System].[tblReference]    Script Date: 01/20/2017 2:34:59 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [System].[tblReference](
	[RefID] [int] IDENTITY(1,1) NOT NULL,
	[Ref] [nvarchar](500) NULL,
	[LedgerID] [int] NULL,
 CONSTRAINT [PK_tblReference] PRIMARY KEY CLUSTERED 
(
	[RefID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [System].[tblReference]  WITH CHECK ADD  CONSTRAINT [FK_tblReference_tblLedger] FOREIGN KEY([LedgerID])
REFERENCES [Acc].[tblLedger] ([LedgerID])
GO

ALTER TABLE [System].[tblReference] CHECK CONSTRAINT [FK_tblReference_tblLedger]
GO


