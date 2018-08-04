
declare @datest date= '2013-01-01';
declare @dattee date= '2016-12-01';

 exec [Acc].[spGetLedgerTransactionNew] 28826,@datest,@dattee,'<AccClassIDSettings><AccClassID>1</AccClassID></AccClassIDSettings>',
'<ProjectIDSettings><ProjectID>1</ProjectID></ProjectIDSettings>';


exec [Acc].[spGetLedgerDetailsByGroupOrLedgerID] @datest,@dattee,0,'<AccClassIDSettings><AccClassID>1</AccClassID></AccClassIDSettings>',
'<ProjectIDSettings><ProjectID>1</ProjectID></ProjectIDSettings>',0;

SELECT DISTINCT VoucherType,RowID FROM Acc.tblTransaction WHERE ledgerID =28826

select *from Acc.tblLedger where LedgerID=28826

select *from Acc.tblTransaction where VoucherType='SALES' and RowID=3033
select *from Inv.tblSalesInvoiceMaster where SalesInvoiceID=3033 and SalesLedgerID=2882

select *from Acc.tblLedger where LedgerID=576

 	INSERT INTO @LedgerTransact
	 SELECT a.TransactDate,576,0,
	0,a.VoucherType,b.Voucher_No,a.RowID from acc.tblTransaction a,Acc.tblJournalMaster b
	 WHERE a.VoucherType='SALES' And a.RowID=3033 AND a.LedgerID =28826 AND a.RowID = b.JournalID AND b.Journal_Date BETWEEN '2013-01-01' and '2018-12-01';  
