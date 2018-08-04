--USE [BENT003]
--GO

/****** Object:  Table [Acc].[tblBankReconSaveStateDetails]    Script Date: 2/22/2017 5:44:55 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [Acc].[tblBankReconSaveStateDetails](
	[SaveStateDetailsID] [int] IDENTITY(1,1) NOT NULL,
	[SaveStateID] [int] NULL,
	[VoucherType] [nvarchar](50) NULL,
	[RowID] [int] NULL,
	[Matched] [bit] NULL,
 CONSTRAINT [PK_tblBankReconSaveStateDetails] PRIMARY KEY CLUSTERED 
(
	[SaveStateDetailsID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [Acc].[tblBankReconSaveStateDetails]  WITH CHECK ADD  CONSTRAINT [FK_tblBankReconSaveStateDetails_tblBankReconSaveStateMaster] FOREIGN KEY([SaveStateID])
REFERENCES [Acc].[tblBankReconSaveStateMaster] ([SaveStateID])
GO

ALTER TABLE [Acc].[tblBankReconSaveStateDetails] CHECK CONSTRAINT [FK_tblBankReconSaveStateDetails_tblBankReconSaveStateMaster]
GO


