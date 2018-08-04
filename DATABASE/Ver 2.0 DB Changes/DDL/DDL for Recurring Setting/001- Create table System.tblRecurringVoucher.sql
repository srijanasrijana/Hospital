-- for voucher recurring
/****** Object:  Table [System].[tblRecurringVoucher]    Script Date: 8/25/2016 1:17:32 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [System].[tblRecurringVoucher](
	[RVID] [int] IDENTITY(1,1) NOT NULL,
	[VoucherID] [nvarchar](50) NOT NULL,
	[VoucherType] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](500) NULL,
	[RecurringType] [nvarchar](20) NOT NULL,
	[Unit1] [nvarchar](50) NULL,
	[Unit2] [nvarchar](50) NULL,
 CONSTRAINT [PK_tblRecurringVoucher] PRIMARY KEY CLUSTERED 
(
	[RVID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


