SET IDENTITY_INSERT [Acc].[tblGroup] ON 

INSERT [Acc].[tblGroup] ([GroupID], [LedgerCode], [GroupNumber], [Parent_GrpID], [Level], [EngName], [NepName], [DrCr], [BuiltIn], [Remarks], [AccountType], [Created_By], [Created_Date], [Modified_By], [Modified_Date]) VALUES (2008, N'G-000050', 0, 6, 2, N'Prepaid Expenditure', N'Prepaid Expenditure', N'DR', 1, N'', NULL, NULL, CAST(N'2017-05-03 11:38:37.617' AS DateTime), NULL, NULL)
SET IDENTITY_INSERT [Acc].[tblGroup] OFF

SET IDENTITY_INSERT [Acc].[tblLedger] ON 

INSERT [Acc].[tblLedger] ([LedgerID], [LedgerCode], [LedgerNumber], [EngName], [NepName], [PreYrBal], [PreYrBalDrCr], [OpCCYID], [OpCCR], [OpCCRDate], [DrCr], [GroupID], [Remarks], [PersonName], [Address1], [Address2], [City], [Phone], [Email], [Company], [Website], [VatPanNo], [CreditLimit], [BuiltIn], [IsActive], [Created_By], [Created_Date], [Modified_By], [Modified_Date], [Calculated], [Calculate_Rate], [LF], [IsBillReference]) VALUES (4698, N'L-000058', 4698, N'Vat Receiable', N'Vat Receiable', 0.0000, N'DEBIT', 1, 0.0000, NULL, N'DR', 2008, N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', 0.0000, 1, 1, N'1', CAST(N'2017-02-27 15:16:29.713' AS DateTime), N'1', CAST(N'2017-05-03 12:04:07.350' AS DateTime), 0, 0, 0, 0)


SET IDENTITY_INSERT [Acc].[tblLedger] OFF

update [Acc].[tblLedger] set EngName='Vat Payable' where LedgerID=412