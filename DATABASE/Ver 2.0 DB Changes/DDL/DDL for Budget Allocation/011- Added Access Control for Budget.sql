
SET IDENTITY_INSERT [System].[tblAccess] ON 

GO

INSERT [System].[tblAccess] ([AccessID], [EngName], [NepName], [Code], [ParentID], [Description]) VALUES (233, N'Budget', N'Budget', N'BUDGET', 9, NULL)
GO
INSERT [System].[tblAccess] ([AccessID], [EngName], [NepName], [Code], [ParentID], [Description]) VALUES (234, N'Budget Setup', N'Budget Setup', N'BUDGET_SETUP', 233, NULL)
GO
INSERT [System].[tblAccess] ([AccessID], [EngName], [NepName], [Code], [ParentID], [Description]) VALUES (235, N'View', N'View', N'BUDGET_SETUP_VIEW', 234, NULL)
GO
INSERT [System].[tblAccess] ([AccessID], [EngName], [NepName], [Code], [ParentID], [Description]) VALUES (236, N'Create and Modify', N'Create and Modify', N'BUDGET_SETUP_CM', 234, NULL)
GO
INSERT [System].[tblAccess] ([AccessID], [EngName], [NepName], [Code], [ParentID], [Description]) VALUES (237, N'Delete', N'Delete', N'BUDGET_SETUP_DEL', 234, NULL)
GO
INSERT [System].[tblAccess] ([AccessID], [EngName], [NepName], [Code], [ParentID], [Description]) VALUES (238, N'Budget Allocation', N'Budget Allocation', N'BUDGET_ALLOC', 233, NULL)
GO
INSERT [System].[tblAccess] ([AccessID], [EngName], [NepName], [Code], [ParentID], [Description]) VALUES (239, N'View', N'View', N'BUDGET_ALLOC_VIEW', 238, NULL)
GO
INSERT [System].[tblAccess] ([AccessID], [EngName], [NepName], [Code], [ParentID], [Description]) VALUES (240, N'Create and Modify', N'Create and Modify', N'BUDGET_ALLOC_CM', 238, NULL)
GO
INSERT [System].[tblAccess] ([AccessID], [EngName], [NepName], [Code], [ParentID], [Description]) VALUES (241, N'Delete', N'Delete', N'BUDGET_ALLOC_DEL', 238, NULL)
GO
INSERT [System].[tblAccess] ([AccessID], [EngName], [NepName], [Code], [ParentID], [Description]) VALUES (242, N'Budget Report', N'Budget Report', N'BUDGET_REPORT', 83, NULL)
GO
SET IDENTITY_INSERT [System].[tblAccess] OFF
GO
SET IDENTITY_INSERT [System].[tblAccessRoleDtl] ON 
GO
INSERT [System].[tblAccessRoleDtl] ([RoleDtlID], [RoleID], [AccessID]) VALUES (12177, 37, 233)
GO
INSERT [System].[tblAccessRoleDtl] ([RoleDtlID], [RoleID], [AccessID]) VALUES (12178, 37, 234)
GO
INSERT [System].[tblAccessRoleDtl] ([RoleDtlID], [RoleID], [AccessID]) VALUES (12179, 37, 235)
GO
INSERT [System].[tblAccessRoleDtl] ([RoleDtlID], [RoleID], [AccessID]) VALUES (12180, 37, 236)
GO
INSERT [System].[tblAccessRoleDtl] ([RoleDtlID], [RoleID], [AccessID]) VALUES (12181, 37, 237)
GO
INSERT [System].[tblAccessRoleDtl] ([RoleDtlID], [RoleID], [AccessID]) VALUES (12182, 37, 238)
GO
INSERT [System].[tblAccessRoleDtl] ([RoleDtlID], [RoleID], [AccessID]) VALUES (12183, 37, 239)
GO
INSERT [System].[tblAccessRoleDtl] ([RoleDtlID], [RoleID], [AccessID]) VALUES (12184, 37, 240)
GO
INSERT [System].[tblAccessRoleDtl] ([RoleDtlID], [RoleID], [AccessID]) VALUES (12185, 37, 241)
GO
INSERT [System].[tblAccessRoleDtl] ([RoleDtlID], [RoleID], [AccessID]) VALUES (12186, 37, 242)
GO
SET IDENTITY_INSERT [System].[tblAccessRoleDtl] OFF
GO
