--change create n modify display to only create
update  System.tblAccess set EngName='Create' where EngName='Create And Modify'

--change old code like JOURNAL_CREATE_MODIFY TO JOURNAL_CREATE
update  System.tblAccess set Code=  REPLACE(Code,'_CREATE_MODIFY','_CREATE') 

--display create instead of sales invoice (correction)
update  System.tblAccess set EngName='Create' where AccessID=121



