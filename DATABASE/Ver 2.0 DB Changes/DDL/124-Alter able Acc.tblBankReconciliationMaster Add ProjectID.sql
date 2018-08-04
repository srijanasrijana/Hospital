ALTER TABLE  Acc.tblBankReconciliationMaster
ADD ProjectID int ;



ALTER table Acc.tblBankReconciliationMaster add constraint FK_tblBankReconciliationMaster_tblProject
Foreign key(ProjectID) references Acc.tblProject(ProjectID)