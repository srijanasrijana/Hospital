--convert vat to vatreciveable in sabal db then delete vat ledger from acc.tblledger
update acc.tblJournalDetail set LedgerID=4698 where LedgerID=7696
update acc.tblTransaction set LedgerID=4698 where LedgerID=7696