alter table Acc.tblBankReconciliationMaster 
		add  ProjectID int foreign key references Acc.tblProject(ProjectID)