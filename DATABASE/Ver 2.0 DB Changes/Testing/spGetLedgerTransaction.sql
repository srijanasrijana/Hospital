
declare @datest date= '2013-01-01';
declare @dattee date= '2016-11-01';

exec Acc.spGetLedgerTransaction 191,@datest,@dattee,'<AccClassIDSettings><AccClassID>1</AccClassID></AccClassIDSettings>',
'<ProjectIDSettings><ProjectID>1</ProjectID></ProjectIDSettings>';


