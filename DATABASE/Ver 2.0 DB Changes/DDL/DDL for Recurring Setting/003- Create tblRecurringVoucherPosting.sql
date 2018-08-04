/****** Object:  Table [System].[tblRecurringVoucherPosting]    Script Date: 8/26/2016 4:15:21 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [System].[tblRecurringVoucherPosting](
	[RVPID] [int] IDENTITY(1,1) NOT NULL,
	[VoucherType] [nvarchar](50) NOT NULL,
	[Date] [date] NOT NULL,
	[VoucherID] [nvarchar](50) NOT NULL,
	[isPosted] [bit] NOT NULL,
 CONSTRAINT [PK_tblRecurringVoucherPosting] PRIMARY KEY CLUSTERED 
(
	[RVPID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


