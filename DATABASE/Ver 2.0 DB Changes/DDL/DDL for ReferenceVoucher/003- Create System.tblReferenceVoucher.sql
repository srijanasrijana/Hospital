
GO

/****** Object:  Table [System].[tblReferenceVoucher]    Script Date: 01/20/2017 2:36:36 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [System].[tblReferenceVoucher](
	[RVID] [int] IDENTITY(1,1) NOT NULL,
	[RefID] [int] NULL,
	[VoucherID] [int] NULL,
	[VoucherType] [nvarchar](50) NULL,
	[IsAgainst] [bit] NULL,
 CONSTRAINT [PK_tblReferenceVoucher] PRIMARY KEY CLUSTERED 
(
	[RVID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [System].[tblReferenceVoucher]  WITH CHECK ADD  CONSTRAINT [FK_tblReferenceVoucher_tblReference] FOREIGN KEY([RefID])
REFERENCES [System].[tblReference] ([RefID])
GO

ALTER TABLE [System].[tblReferenceVoucher] CHECK CONSTRAINT [FK_tblReferenceVoucher_tblReference]
GO


