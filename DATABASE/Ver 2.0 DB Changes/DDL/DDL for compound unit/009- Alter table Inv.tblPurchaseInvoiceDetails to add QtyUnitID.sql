
-- RUN EACH SCRIPT I.E. LINE BY LINE
alter table Inv.tblPurchaseInvoiceDetails add QtyUnitID int;

update s set QtyUnitID = (select UnitMaintenanceID from Inv.tblProduct p where p.ProductID = s.ProductID) from Inv.tblPurchaseInvoiceDetails s