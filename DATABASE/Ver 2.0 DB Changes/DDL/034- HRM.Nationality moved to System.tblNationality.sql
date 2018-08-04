--Renamed the Nationality table to tblNationality
--And transfered the table to System schema as this table is used in multiple projects
Exec sp_rename 'HRM.Nationality', 'tblNationality';

Alter SCHEMA System TRANSFER HRM.tblNationality;