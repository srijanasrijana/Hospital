--USE [BENT003]
--GO
/****** Object:  StoredProcedure [System].[spFiscalYearClosing]    Script Date: 5/9/2017 4:51:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [System].[spFiscalYearClosing](
@prevYrDBName nvarchar(100),
@newYrDBName nvarchar(100)

)
as
begin
-- to insert ledger details from previous database to the new one
	declare @sqlStmt nvarchar(max) = 'DELETE FROM ['+@newYrDBName+'].[Acc].[tblLedger]; '+
									 'DELETE FROM ['+@newYrDBName+'].[Acc].[tblGroup]; '+
									 'SET IDENTITY_INSERT ['+@newYrDBName+'].[Acc].[tblGroup] ON INSERT INTO  '+@newYrDBName+'.Acc.tblGroup (AccountType, BuiltIn, Created_By, Created_Date, DrCr, EngName, GroupID, GroupNumber, LedgerCode,Level, Modified_By, Modified_Date, NepName, Parent_GrpID, Remarks) SELECT AccountType, BuiltIn, Created_By, Created_Date, DrCr, EngName, GroupID, GroupNumber, LedgerCode,Level, Modified_By, Modified_Date, NepName, Parent_GrpID, Remarks FROM  '+@prevYrDBName+'.Acc.tblGroup SET IDENTITY_INSERT ['+@newYrDBName+'].[Acc].[tblGroup] OFF '+
									 'SET IDENTITY_INSERT ['+@newYrDBName+'].[Acc].[tblLedger] ON INSERT INTO  '+@newYrDBName+'.Acc.tblLedger(Address1,Address2, BuiltIn, Calculate_Rate, Calculated, City, Company,Created_By, Created_Date, CreditLimit, DrCr, Email, EngName, GroupID, IsActive, IsBillReference, LedgerCode, LedgerID, LedgerNumber, LF, Modified_By, Modified_Date,NepName,OpCCR,OpCCRDate,OpCCYID, PersonName, Phone, PreYrBal, PreYrBalDrCr, Remarks, VatPanNo, Website)SELECT Address1,Address2, BuiltIn, Calculate_Rate, Calculated, City, Company,Created_By, Created_Date, CreditLimit, DrCr, Email, EngName, GroupID, IsActive, IsBillReference, LedgerCode, LedgerID, LedgerNumber, LF, Modified_By, Modified_Date,NepName,OpCCR,OpCCRDate,OpCCYID, PersonName, Phone, PreYrBal, PreYrBalDrCr, Remarks, VatPanNo, Website FROM  '+@prevYrDBName+'.Acc.tblLedger SET IDENTITY_INSERT ['+@newYrDBName+'].[Acc].[tblLedger] OFF';

			exec sp_executesql @sqlStmt;

-- to insert accounting class details from previous database to the new one
	--set @sqlStmt = 'DELETE FROM ['+@newYrDBName+'].[Acc].[tblAccClass]; '+
	--								 'SET IDENTITY_INSERT ['+@newYrDBName+'].[Acc].[tblAccClass] ON INSERT INTO  '+@newYrDBName+'.Acc.tblAccClass (AccClassID, EngName, NepName, ParentID, Remarks, Created_By, Created_Date, Modified_By, Modified_Date) SELECT AccClassID, EngName, NepName, ParentID, Remarks, Created_By, Created_Date, Modified_By, Modified_Date FROM  '+@prevYrDBName+'.Acc.tblAccClass SET IDENTITY_INSERT ['+@newYrDBName+'].[Acc].[tblAccClass] OFF ';

			--exec sp_executesql @sqlStmt;
	
-- to insert ledger details from previous database to the new one
	set @sqlStmt = 'delete from ['+@newYrDBName+'].[Inv].[tblOpeningQuantity];
	delete from ['+@newYrDBName+'].[Inv].[tblProduct]; '+
					'SET IDENTITY_INSERT ['+@newYrDBName+'].[Inv].[tblProduct] ON INSERT INTO  '+@newYrDBName+'.Inv.tblProduct (BackColor, BuiltIn, Created_By, Created_Date, DebtorsID, DepotID, EngName, GroupID, Image, IsActive, IsInventoryApplicable, IsVatApplicable, Modified_By, Modified_Date, NepName, ProductCode, ProductColor, ProductID, PurchaseDiscount, PurchaseRate, Quantity, Remarks, RentDate, SalesRate , TotalValue, UnitMaintenanceID) SELECT BackColor, BuiltIn, Created_By, Created_Date, DebtorsID, DepotID, EngName, GroupID, Image, IsActive, IsInventoryApplicable, IsVatApplicable, Modified_By, Modified_Date, NepName, ProductCode, ProductColor, ProductID, PurchaseDiscount, PurchaseRate, Quantity, Remarks, RentDate, SalesRate , TotalValue, UnitMaintenanceID FROM  '+@prevYrDBName+'.Inv.tblProduct  SET IDENTITY_INSERT ['+@newYrDBName+'].[Inv].[tblProduct] OFF';

		   exec sp_executesql @sqlStmt;

-- add user and accounting class details from previous database to the new one
	set @sqlStmt = 'delete from '+@newYrDBName+'.System.tblUserReportPreferences;
					delete from '+@newYrDBName+'.System.tblUser_Preference;
					delete from '+@newYrDBName+'.System.tblUser;
					DELETE FROM '+@newYrDBName+'.Acc.tblAccClass; 
					delete from '+@newYrDBName+'.System.tblAccessRoleDtl;
					delete from '+@newYrDBName+'.System.tblAccess;
					delete from '+@newYrDBName+'.System.tblAccessRole;

					SET IDENTITY_INSERT ['+@newYrDBName+'].[Acc].[tblAccClass] ON 
					INSERT INTO  '+@newYrDBName+'.Acc.tblAccClass (AccClassID, EngName, NepName, ParentID, Remarks, Created_By, Created_Date, Modified_By, Modified_Date) 
						SELECT AccClassID, EngName, NepName, ParentID, Remarks, Created_By, Created_Date, Modified_By, Modified_Date FROM  '+@prevYrDBName+'.Acc.tblAccClass SET IDENTITY_INSERT ['+@newYrDBName+'].[Acc].[tblAccClass] OFF 

					SET IDENTITY_INSERT ['+@newYrDBName+'].[System].[tblAccessRole] ON 
					INSERT INTO  '+@newYrDBName+'.System.tblAccessRole(BuiltIn, Created_By, Created_Date, Description, EngName, NepName, RoleID)
						SELECT BuiltIn, Created_By, Created_Date, Description, EngName, NepName, RoleID FROM  '+@prevYrDBName+'.System.tblAccessRole 

					SET IDENTITY_INSERT ['+@newYrDBName+'].[System].[tblAccessRole] OFF

					SET IDENTITY_INSERT ['+@newYrDBName+'].[System].[tblAccess] ON 
					INSERT INTO  '+@newYrDBName+'.System.tblAccess(AccessID, Code, Description, EngName, NepName, ParentID)
						SELECT AccessID, Code, Description, EngName, NepName, ParentID FROM  '+@prevYrDBName+'.System.tblAccess 

					SET IDENTITY_INSERT ['+@newYrDBName+'].[System].[tblAccess] OFF

					SET IDENTITY_INSERT ['+@newYrDBName+'].[System].[tblAccessRoleDtl] ON 
					INSERT INTO  '+@newYrDBName+'.System.tblAccessRoleDtl(AccessID, RoleDtlID, RoleID)
						SELECT AccessID, RoleDtlID, RoleID FROM  '+@prevYrDBName+'.System.tblAccessRoleDtl 

					SET IDENTITY_INSERT ['+@newYrDBName+'].[System].[tblAccessRoleDtl] OFF

					SET IDENTITY_INSERT ['+@newYrDBName+'].[System].[tblUser] ON 
					INSERT INTO  '+@newYrDBName+'.System.tblUser(AccClassID, AccessRoleID, Address, Contact, CreatedBy, CreatedDate, Department, Email, ModifiedBy, ModifiedDate, Name, Password, UserID, UserName, UserStatus)
						SELECT AccClassID, AccessRoleID, Address, Contact, CreatedBy, CreatedDate, Department, Email, ModifiedBy, ModifiedDate, Name, Password, UserID, UserName, UserStatus FROM  '+@prevYrDBName+'.System.tblUser 

					SET IDENTITY_INSERT ['+@newYrDBName+'].[System].[tblUser] OFF

					SET IDENTITY_INSERT  ['+@newYrDBName+'].[System].[tblUserReportPreferences] ON 
					INSERT INTO  '+@newYrDBName+'.System.tblUserReportPreferences(ReportPreferenceID, UserID, UserReportPreferenceID, Value)
						SELECT ReportPreferenceID, UserID, UserReportPreferenceID, Value FROM '+@prevYrDBName+'.System.tblUserReportPreferences 

					SET IDENTITY_INSERT ['+@newYrDBName+'].[System].[tblUserReportPreferences] OFF

					SET IDENTITY_INSERT  ['+@newYrDBName+'].[System].[tblUser_Preference] ON 
					INSERT INTO  '+@newYrDBName+'.System.tblUser_Preference(PreferenceID, UserID, UserPreferenceID, Value)
						SELECT PreferenceID, UserID, UserPreferenceID, Value FROM '+@prevYrDBName+'.System.tblUser_Preference 

					SET IDENTITY_INSERT ['+@newYrDBName+'].[System].[tblUser_Preference] OFF
					';

		exec sp_executesql @sqlStmt;

end