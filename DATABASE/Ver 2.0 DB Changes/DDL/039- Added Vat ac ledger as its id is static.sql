--Add vat ac ledger if the ledger is not present
GO
SET IDENTITY_INSERT [Acc].[tblLedger] ON 
GO
INSERT [Acc].[tblLedger] ([LedgerID], [LedgerCode], [LedgerNumber], [EngName], [NepName], [PreYrBal], [PreYrBalDrCr], [OpCCYID], [OpCCR], [OpCCRDate], [DrCr], [GroupID], [Remarks], [PersonName], [Address1], [Address2], [City], [Phone], [Email], [Company], [Website], [VatPanNo], [CreditLimit], [BuiltIn], [IsActive], [Created_By], [Created_Date], [Modified_By], [Modified_Date], [Calculated], [Calculate_Rate], [LF]) VALUES (412, N'L-000024', 0, N'Vat A/c', N'Vat A/c', 0.0000, N'DEBIT', 1, 0.0000, NULL, N'CR', 140, N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', 0.0000, 1, 1, N'1', CAST(N'2016-06-01 11:10:26.240' AS DateTime), NULL, NULL, 0, 0, 0)
GO
SET IDENTITY_INSERT [Acc].[tblLedger] OFF