Alter table Inv.tblOpeningQuantity
Add Constraint FK_tblProduct_tblOpeningQuantity Foreign Key (ProductID) references Inv.tblProduct(ProductID);