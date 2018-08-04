
-- RUN EACH SCRIPT I.E. LINE BY LINE
alter table Inv.tblSalesInvoiceDetails add QtyUnitID int;

update s set QtyUnitID = (select UnitMaintenanceID from Inv.tblProduct p where p.ProductID = s.ProductID) from Inv.tblSalesInvoiceDetails s