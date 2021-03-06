/* This file is created by Sunil */
/* New access that were not present before 15th March 2016. */
--'SET IDENTITY_INSERT [table] ON/OFF' should also be selected while selecting insert queries 
--So that 'ON' will let auto increment column be filled with required data
--and 'OFF' will again start auto increment feature

--USE [BENT003]
GO
SET IDENTITY_INSERT [System].[tblAccess] ON 

INSERT [System].[tblAccess] ([AccessID], [EngName], [NepName], [Code], [ParentID], [Description]) VALUES (196, N'Tax1', N'Tax1', N'SLAB_TAX1', 13, NULL)
INSERT [System].[tblAccess] ([AccessID], [EngName], [NepName], [Code], [ParentID], [Description]) VALUES (197, N'Tax2', N'Tax2', N'SLAB_TAX2', 13, NULL)
INSERT [System].[tblAccess] ([AccessID], [EngName], [NepName], [Code], [ParentID], [Description]) VALUES (198, N'Tax3', N'Tax3', N'SLAB_TAX3', 13, NULL)
INSERT [System].[tblAccess] ([AccessID], [EngName], [NepName], [Code], [ParentID], [Description]) VALUES (199, N'VAT', N'VAT', N'SLAB_VAT', 13, NULL)
INSERT [System].[tblAccess] ([AccessID], [EngName], [NepName], [Code], [ParentID], [Description]) VALUES (200, N'Custom Duty', N'Custom Duty', N'SLAB_CUSTOM_DUTY', 13, NULL)
INSERT [System].[tblAccess] ([AccessID], [EngName], [NepName], [Code], [ParentID], [Description]) VALUES (201, N'Purchase Order', N'Purchase Order', N'PURCHASE_ORDER', 51, NULL)
INSERT [System].[tblAccess] ([AccessID], [EngName], [NepName], [Code], [ParentID], [Description]) VALUES (204, N'Create and Modify', N'Create and Modify', N'PURCHASE_ORDER_CREATE_MODIFY', 201, NULL)
INSERT [System].[tblAccess] ([AccessID], [EngName], [NepName], [Code], [ParentID], [Description]) VALUES (205, N'Delete', N'Delete', N'PURCHASE_ORDER_DELETE', 201, NULL)
INSERT [System].[tblAccess] ([AccessID], [EngName], [NepName], [Code], [ParentID], [Description]) VALUES (206, N'View', N'View', N'PURCHASE_ORDER_VIEW', 201, NULL)
INSERT [System].[tblAccess] ([AccessID], [EngName], [NepName], [Code], [ParentID], [Description]) VALUES (207, N'Stock Transfer', N'Stock Transfer', N'STOCK_TRANSFER', 51, NULL)
INSERT [System].[tblAccess] ([AccessID], [EngName], [NepName], [Code], [ParentID], [Description]) VALUES (208, N'Create and Modify', N'Create and Modify', N'STOCK_TRANSFER_CREATE_MODIFY', 207, NULL)
INSERT [System].[tblAccess] ([AccessID], [EngName], [NepName], [Code], [ParentID], [Description]) VALUES (209, N'Delete', N'Delete', N'STOCK_TRANSFER_DELETE', 207, NULL)
INSERT [System].[tblAccess] ([AccessID], [EngName], [NepName], [Code], [ParentID], [Description]) VALUES (210, N'View', N'View', N'STOCK_TRANSFER_VIEW', 207, NULL)
INSERT [System].[tblAccess] ([AccessID], [EngName], [NepName], [Code], [ParentID], [Description]) VALUES (211, N'Damage Items', N'Damage Items', N'DAMAGE_ITEMS', 51, NULL)
INSERT [System].[tblAccess] ([AccessID], [EngName], [NepName], [Code], [ParentID], [Description]) VALUES (212, N'Create and Modify', N'Create and Modify', N'DAMAGE_ITEMS_CREATE_MODIFY', 211, NULL)
INSERT [System].[tblAccess] ([AccessID], [EngName], [NepName], [Code], [ParentID], [Description]) VALUES (213, N'Delete', N'Delete', N'DAMAGE_ITEMS_DELETE', 211, NULL)
INSERT [System].[tblAccess] ([AccessID], [EngName], [NepName], [Code], [ParentID], [Description]) VALUES (214, N'View', N'View', N'DAMAGE_ITEMS_VIEW', 211, NULL)
INSERT [System].[tblAccess] ([AccessID], [EngName], [NepName], [Code], [ParentID], [Description]) VALUES (215, N'Bulk Voucher Posting', N'Bulk Voucher Posting', N'BULK_VOUCHER_POSTING', 51, NULL)
INSERT [System].[tblAccess] ([AccessID], [EngName], [NepName], [Code], [ParentID], [Description]) VALUES (217, N'Inventory Book', N'Inventory Book', N'INVENTORY_BOOK', 83, NULL)
INSERT [System].[tblAccess] ([AccessID], [EngName], [NepName], [Code], [ParentID], [Description]) VALUES (218, N'Day Book', N'Day Book', N'INVENTORY_BOOK_DAY_BOOK', 217, NULL)
INSERT [System].[tblAccess] ([AccessID], [EngName], [NepName], [Code], [ParentID], [Description]) VALUES (219, N'Purchase Register', N'Purchase Register', N'INVENTORY_BOOK_PURCHASE_REGISTER', 217, NULL)
INSERT [System].[tblAccess] ([AccessID], [EngName], [NepName], [Code], [ParentID], [Description]) VALUES (220, N'Purchase Return Register', N'Purchase Return Register', N'INVENTORY_BOOK_PURCHSE_RETURN_REGISTER', 217, NULL)
INSERT [System].[tblAccess] ([AccessID], [EngName], [NepName], [Code], [ParentID], [Description]) VALUES (221, N'Sales Register', N'Sales Register', N'INVENTORY_BOOK_SALES_REGISTER', 217, NULL)
INSERT [System].[tblAccess] ([AccessID], [EngName], [NepName], [Code], [ParentID], [Description]) VALUES (222, N'Sales Return Register', N'Sales Return Register', N'INVENTORY_BOOK_SALES_RETURN_REGISTER', 217, NULL)
INSERT [System].[tblAccess] ([AccessID], [EngName], [NepName], [Code], [ParentID], [Description]) VALUES (223, N'Stock Ledger', N'Stock Ledger', N'INVENTORY_BOOK_STOCK_LEDGER', 217, NULL)
INSERT [System].[tblAccess] ([AccessID], [EngName], [NepName], [Code], [ParentID], [Description]) VALUES (224, N'Stock Ageing', N'Stock Ageing', N'INVENTORY_BOOK_STOCK_AGEING', 217, NULL)
INSERT [System].[tblAccess] ([AccessID], [EngName], [NepName], [Code], [ParentID], [Description]) VALUES (225, N'Purchase', N'Purchase', N'REPORTS_PURCHASE', 83, NULL)
INSERT [System].[tblAccess] ([AccessID], [EngName], [NepName], [Code], [ParentID], [Description]) VALUES (226, N'Purchase Report', N'Purchase Report', N'REPORTS_PURCHASE_REPORT', 225, NULL)
INSERT [System].[tblAccess] ([AccessID], [EngName], [NepName], [Code], [ParentID], [Description]) VALUES (227, N'Sales', N'Sales', N'REPORTS_SALES', 83, NULL)
INSERT [System].[tblAccess] ([AccessID], [EngName], [NepName], [Code], [ParentID], [Description]) VALUES (228, N'Sales Report', N'Sales Report', N'REPORTS_SALES_REPORT', 227, NULL)
INSERT [System].[tblAccess] ([AccessID], [EngName], [NepName], [Code], [ParentID], [Description]) VALUES (229, N'Debtors', N'Debtors', N'DEBTORS', 83, NULL)
INSERT [System].[tblAccess] ([AccessID], [EngName], [NepName], [Code], [ParentID], [Description]) VALUES (230, N'Ageing', N'Ageing', N'DEBTORS_AGEING', 229, NULL)
INSERT [System].[tblAccess] ([AccessID], [EngName], [NepName], [Code], [ParentID], [Description]) VALUES (231, N'Due Days', N'Due Days', N'DEBTORS_DUE_DAYS', 229, NULL)
INSERT [System].[tblAccess] ([AccessID], [EngName], [NepName], [Code], [ParentID], [Description]) VALUES (232, N'Pending', N'Pending', N'DEBTORS_PENDING', 229, NULL)
SET IDENTITY_INSERT [System].[tblAccess] OFF

/* Delete Access control for Access Role, Access Control is not available for Access Role Form except Admin */

/* Delete the records in AccessRoleDtl That has the same AccessID as below */
DELETE FROM System.tblAccessRoleDtl WHERE AccessID = 153
DELETE FROM System.tblAccessRoleDtl WHERE AccessID = 154
DELETE FROM System.tblAccessRoleDtl WHERE AccessID = 155
DELETE FROM System.tblAccessRoleDtl WHERE AccessID = 156

--The Above Delete step must be perfomed first as there is Foreign Key Relation
DELETE FROM System.tblAccess WHERE AccessID = 153
DELETE FROM System.tblAccess WHERE AccessID = 154
DELETE FROM System.tblAccess WHERE AccessID = 155
DELETE FROM System.tblAccess WHERE AccessID = 156


/* Delete Access control for User Maintainance, Access Control is not available forUser Maintainance Form except Admin */

/* Delete the records in AccessRoleDtl That has the same AccessID as below */
DELETE FROM System.tblAccessRoleDtl WHERE AccessID = 3
DELETE FROM System.tblAccessRoleDtl WHERE AccessID = 7
DELETE FROM System.tblAccessRoleDtl WHERE AccessID = 8

--The Above Delete step must be perfomed first as there is Foreign Key Relation
DELETE FROM System.tblAccess WHERE AccessID = 3
DELETE FROM System.tblAccess WHERE AccessID = 7
DELETE FROM System.tblAccess WHERE AccessID = 8

/* Access Role is not editable for admin, so all access control should be provided to Admin in database */
SET IDENTITY_INSERT [System].[tblAccessRoleDtl] ON 

INSERT [System].[tblAccessRoleDtl] ([RoleDtlID], [RoleID], [AccessID]) VALUES (12136, 37, 196)
INSERT [System].[tblAccessRoleDtl] ([RoleDtlID], [RoleID], [AccessID]) VALUES (12137, 37, 197)
INSERT [System].[tblAccessRoleDtl] ([RoleDtlID], [RoleID], [AccessID]) VALUES (12139, 37, 198)
INSERT [System].[tblAccessRoleDtl] ([RoleDtlID], [RoleID], [AccessID]) VALUES (12140, 37, 199)
INSERT [System].[tblAccessRoleDtl] ([RoleDtlID], [RoleID], [AccessID]) VALUES (12141, 37, 200)
INSERT [System].[tblAccessRoleDtl] ([RoleDtlID], [RoleID], [AccessID]) VALUES (12142, 37, 201)
INSERT [System].[tblAccessRoleDtl] ([RoleDtlID], [RoleID], [AccessID]) VALUES (12148, 37, 204)
INSERT [System].[tblAccessRoleDtl] ([RoleDtlID], [RoleID], [AccessID]) VALUES (12149, 37, 205)
INSERT [System].[tblAccessRoleDtl] ([RoleDtlID], [RoleID], [AccessID]) VALUES (12150, 37, 206)
INSERT [System].[tblAccessRoleDtl] ([RoleDtlID], [RoleID], [AccessID]) VALUES (12151, 37, 208)
INSERT [System].[tblAccessRoleDtl] ([RoleDtlID], [RoleID], [AccessID]) VALUES (12152, 37, 209)
INSERT [System].[tblAccessRoleDtl] ([RoleDtlID], [RoleID], [AccessID]) VALUES (12153, 37, 210)
INSERT [System].[tblAccessRoleDtl] ([RoleDtlID], [RoleID], [AccessID]) VALUES (12154, 37, 207)
INSERT [System].[tblAccessRoleDtl] ([RoleDtlID], [RoleID], [AccessID]) VALUES (12155, 37, 211)
INSERT [System].[tblAccessRoleDtl] ([RoleDtlID], [RoleID], [AccessID]) VALUES (12156, 37, 212)
INSERT [System].[tblAccessRoleDtl] ([RoleDtlID], [RoleID], [AccessID]) VALUES (12157, 37, 213)
INSERT [System].[tblAccessRoleDtl] ([RoleDtlID], [RoleID], [AccessID]) VALUES (12158, 37, 214)
INSERT [System].[tblAccessRoleDtl] ([RoleDtlID], [RoleID], [AccessID]) VALUES (12159, 37, 215)
INSERT [System].[tblAccessRoleDtl] ([RoleDtlID], [RoleID], [AccessID]) VALUES (12161, 37, 217)
INSERT [System].[tblAccessRoleDtl] ([RoleDtlID], [RoleID], [AccessID]) VALUES (12162, 37, 218)
INSERT [System].[tblAccessRoleDtl] ([RoleDtlID], [RoleID], [AccessID]) VALUES (12163, 37, 219)
INSERT [System].[tblAccessRoleDtl] ([RoleDtlID], [RoleID], [AccessID]) VALUES (12164, 37, 220)
INSERT [System].[tblAccessRoleDtl] ([RoleDtlID], [RoleID], [AccessID]) VALUES (12165, 37, 221)
INSERT [System].[tblAccessRoleDtl] ([RoleDtlID], [RoleID], [AccessID]) VALUES (12166, 37, 222)
INSERT [System].[tblAccessRoleDtl] ([RoleDtlID], [RoleID], [AccessID]) VALUES (12167, 37, 223)
INSERT [System].[tblAccessRoleDtl] ([RoleDtlID], [RoleID], [AccessID]) VALUES (12168, 37, 224)
INSERT [System].[tblAccessRoleDtl] ([RoleDtlID], [RoleID], [AccessID]) VALUES (12169, 37, 225)
INSERT [System].[tblAccessRoleDtl] ([RoleDtlID], [RoleID], [AccessID]) VALUES (12170, 37, 226)
INSERT [System].[tblAccessRoleDtl] ([RoleDtlID], [RoleID], [AccessID]) VALUES (12171, 37, 227)
INSERT [System].[tblAccessRoleDtl] ([RoleDtlID], [RoleID], [AccessID]) VALUES (12172, 37, 228)
INSERT [System].[tblAccessRoleDtl] ([RoleDtlID], [RoleID], [AccessID]) VALUES (12173, 37, 229)
INSERT [System].[tblAccessRoleDtl] ([RoleDtlID], [RoleID], [AccessID]) VALUES (12174, 37, 230)
INSERT [System].[tblAccessRoleDtl] ([RoleDtlID], [RoleID], [AccessID]) VALUES (12175, 37, 231)
INSERT [System].[tblAccessRoleDtl] ([RoleDtlID], [RoleID], [AccessID]) VALUES (12176, 37, 232)
SET IDENTITY_INSERT [System].[tblAccessRoleDtl] OFF
