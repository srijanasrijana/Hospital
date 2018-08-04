--USE [BENT003]
--GO

/****** Object:  Table [Acc].[tblBankReconSaveStateMaster]    Script Date: 2/22/2017 5:44:51 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [Acc].[tblBankReconSaveStateMaster](
	[SaveStateID] [int] IDENTITY(1,1) NOT NULL,
	[BankID] [int] NULL,
	[PartyID] [int] NULL,
	[PmtRcpTypeID] [int] NULL,
	[FromDate] [date] NULL,
	[ToDate] [date] NULL,
	[Balance] [decimal](20, 5) NULL,
 CONSTRAINT [PK_tblBankReconSaveStateMaster] PRIMARY KEY CLUSTERED 
(
	[SaveStateID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


