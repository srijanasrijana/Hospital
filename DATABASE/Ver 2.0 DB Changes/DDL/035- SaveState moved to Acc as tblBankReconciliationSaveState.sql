--Renamed the SaveState table to tblBankReconciliationSaveState
Exec sp_rename 'SaveState', 'tblBankReconciliationSaveState';

--And transfered the table from dbo schema to Acc schema
Alter SCHEMA Acc TRANSFER dbo.tblBankReconciliationSaveState;