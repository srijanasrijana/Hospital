/*
   Tuesday, December 8, 20153:17:41 PM
   User: sa
   Server: .
   Database: BENT003
   Application: 
*/

/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE Acc.tblProject ADD CONSTRAINT
	FK_tblProject_tblProject FOREIGN KEY
	(
	ParentProjectID
	) REFERENCES Acc.tblProject
	(
	ProjectID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE Acc.tblProject SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
select Has_Perms_By_Name(N'Acc.tblProject', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'Acc.tblProject', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'Acc.tblProject', 'Object', 'CONTROL') as Contr_Per 