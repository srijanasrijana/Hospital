
/****** Object:  StoredProcedure [Acc].[spRemoveJournalEntry]    Script Date: 11-Jul-16 11:36:51 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



ALTER PROCEDURE [Acc].[spRemoveJournalEntry]
 @journalID  INT
AS
	BEGIN
		SET NOCOUNT ON
--if you delete journal master entry it automatically delete from its detail table 
--for this you must have insert update criteria on CASCADE
			DELETE FROM Acc.tblTransactionClass WHERE RowID = @journalID and VoucherType='JRNL';
			DELETE FROM Acc.tblTransaction WHERE RowID = @journalID and VoucherType='JRNL';
			DELETE FROM Acc.tblJournalDetail WHERE JournalID = @journalID;
			DELETE FROM Acc.tblJournalMaster WHERE JournalID =  @journalID;		
			
			DELETE FROM System.tblDueDate WHERE RowID = @journalID and VoucherType='JRNL';
				
	END