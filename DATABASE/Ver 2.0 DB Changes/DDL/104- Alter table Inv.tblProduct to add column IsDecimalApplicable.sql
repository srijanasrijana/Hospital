alter table Inv.tblProduct add IsDecimalApplicable int;

update Inv.tblProduct set IsDecimalApplicable = 0;