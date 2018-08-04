--USE [BENT003]
--GO

/****** Object:  Table [Acc].[tblClosedBankReconciliation]    Script Date: 2/17/2017 2:50:43 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [Acc].[tblClosedBankReconciliation](
	[ClosedBankReconciliationID] [int] IDENTITY(1,1) NOT NULL,
	[BankID] [int] NULL,
	[Date] [datetime] NULL,
	[Description] [nvarchar](500) NULL
) ON [PRIMARY]

GO

ALTER TABLE [Acc].[tblClosedBankReconciliation]  WITH CHECK ADD  CONSTRAINT [FK_tblClosedBankReconciliation_tblLedger] FOREIGN KEY([BankID])
REFERENCES [Acc].[tblLedger] ([LedgerID])
GO

ALTER TABLE [Acc].[tblClosedBankReconciliation] CHECK CONSTRAINT [FK_tblClosedBankReconciliation_tblLedger]
GO


