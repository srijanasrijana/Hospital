--Delete the row of tblOpeningQuantity that has productID which are not present in tblProduct

Delete from Inv.tblOpeningQuantity where ProductID Not In (Select ProductID from Inv.tblProduct);

Delete from Inv.tblOpeningQuantity where AccClassID Not In (Select AccClassID from Acc.tblAccClass);