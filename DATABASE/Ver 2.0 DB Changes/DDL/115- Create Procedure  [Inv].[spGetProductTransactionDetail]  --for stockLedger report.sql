
/****** Object:  StoredProcedure [Inv].[spGetProductTransactionDetail]    Script Date: 09/12/2017 11:01:22 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [Inv].[spGetProductTransactionDetail] 

--Get the transaction for the particular Product
--SELECT DATEDIFF(DAY,  DATEADD(day, -1, @CreatedDate), GETDATE())
--product name | unit | net sales qty  | Net Amt
--Acc Class filter, Date filter, depot, single product, sales ledger, project
@ProductID INT=NULL,
@FromDate DATETIME=NULL,

@ToDate DATETIME = NULL, 
@AccountClassIDsSettings  xml,    -- It encludes AccountCLassIDs  Info
@ProjectIDsSettings xml,
@OpeningStockQty decimal(19,5) output
  AS
  Declare @docHandle int;
  Declare @docHandle1 int;

  exec sp_xml_preparedocument @docHandle output,@AccountClassIDsSettings
  exec sp_xml_Preparedocument @docHandle1 output, @ProjectIDsSettings

  Begin

  if (@ProductID is null)
  set @ProductID=0;


  Declare @tblFinalResult table(
  TransactDate date,
  VoucherType nvarchar(30),
  VoucherNo nvarchar(50),
  Party nvarchar(50),
  InBoundQty decimal(19,5),
  OutBoundQty decimal(19,5),
  Amount decimal(19,5),
  RowID int)

  --set fromdate and todate
  if(@FromDate is null)
  select  @FromDate = BookBeginFrom from System.tblCompanyInfo;
  if(@ToDate is null)
  set @ToDate=GETDATE();
  set @OpeningStockQty=0;
--for output opening stock
select @OpeningStockQty=isnull(OpenPurchaseQty,0) from  inv.tblOpeningQuantity where ProductID=@productID
select @OpeningStockQty=@OpeningStockQty+ isnull((sum(isnull(Incoming,0))-sum(isnull(Outgoing,0))),0) from Inv.tblInventoryTrans where ProductID=@ProductID and TransactDate<@FromDate

--get accounting class and project ids
  Declare @AccClassIDs table(accClassID int)
  Declare @ProjectIDs table(projectID int)

  insert into @AccClassIDs select AccClassID from Acc.fnGetAllAccClass (@AccountClassIDsSettings); 
  insert into @ProjectIDs   SELECT ProjectID FROM  Acc.fnGetAllProjects (@ProjectIDsSettings);

Declare @tblVchRow table(voucherType nvarchar(30),rowID int)
--get all transaction related to the given product id
insert into @tblVchRow
 select distinct VoucherType,RowID from Inv.tblInventoryTrans where ProductID=@ProductID and TransactDate between @FromDate and @ToDate


  Declare @RowID int =0;
  Declare db_cursor cursor for select rowID from @tblVchRow where voucherType='PURCH'
  open db_cursor fetch next from db_cursor into @RowID

  while @@FETCH_STATUS = 0
  begin
  insert into @tblFinalResult
  select t.TransactDate,t.VoucherType,pm.Voucher_No,l.EngName as CashParty, Incoming,Outgoing,pid.Net_Amount, @RowID from Inv.tblInventoryTrans t inner join inv.tblPurchaseInvoiceMaster pm on t.RowID=pm.PurchaseInvoiceID inner join Acc.tblLedger l on pm.CashPartyLedgerID= l.LedgerID inner join Inv.tblPurchaseInvoiceDetails pid on pm.PurchaseInvoiceID=pid.PurchaseInvoiceID and t.ProductID=pid.ProductID where t.VoucherType='PURCH' and t.RowID=@RowID and t.ProductID=@ProductID
  	FETCH NEXT FROM db_cursor INTO @RowID   
  end
  CLOSE db_cursor   
DEALLOCATE db_cursor


  Declare db_cursor cursor for select rowID from @tblVchRow where voucherType='SALES' 
  open db_cursor fetch next from db_cursor into @RowID

  while @@FETCH_STATUS = 0
  begin
  insert into @tblFinalResult
  select t.TransactDate,t.VoucherType,sm.Voucher_No,l.EngName as CashParty, Incoming,Outgoing,sm.Total_Amount,@RowID from Inv.tblInventoryTrans t inner join inv.tblSalesInvoiceMaster sm on t.RowID=sm.SalesInvoiceID inner join Acc.tblLedger l on sm.CashPartyLedgerID= l.LedgerID where t.VoucherType='SALES' and t.RowID=@RowID and t.ProductID=@ProductID
  	FETCH NEXT FROM db_cursor INTO @RowID   
  end
  CLOSE db_cursor   
DEALLOCATE db_cursor

Declare db_cursor cursor for select rowID from @tblVchRow where voucherType='PURCH_RTN'
  open db_cursor fetch next from db_cursor into @RowID

  while @@FETCH_STATUS = 0
  begin
  insert into @tblFinalResult
  select t.TransactDate,t.VoucherType,prm.Voucher_No,l.EngName as CashParty, Incoming,Outgoing,prm.Total_Amt,@RowID from Inv.tblInventoryTrans t inner join inv.tblPurchaseReturnMaster prm on t.RowID=prm.PurchaseReturnID inner join Acc.tblLedger l on prm.CashPartyLedgerID= l.LedgerID where t.VoucherType='PURCH_RTN' and t.RowID=@RowID and t.ProductID=@ProductID
  	FETCH NEXT FROM db_cursor INTO @RowID   
  end
  CLOSE db_cursor   
DEALLOCATE db_cursor

Declare db_cursor cursor for select rowID from @tblVchRow where voucherType='SLS_RTN'
  open db_cursor fetch next from db_cursor into @RowID

  while @@FETCH_STATUS = 0
  begin
  insert into @tblFinalResult
  select t.TransactDate,t.VoucherType,srm.Voucher_No,l.EngName as CashParty, Incoming,Outgoing,srm.Total_Amt,@RowID from Inv.tblInventoryTrans t inner join inv.tblSalesReturnMaster srm on t.RowID=srm.SalesReturnID inner join Acc.tblLedger l on srm.CashPartyLedgerID= l.LedgerID where t.VoucherType='SLS_RTN' and t.RowID=@RowID and t.ProductID=@ProductID
  	FETCH NEXT FROM db_cursor INTO @RowID   
  end
  CLOSE db_cursor   
DEALLOCATE db_cursor


select Date.fnEngtoNep(TransactDate) as NepTransactDate ,* from @tblFinalResult 

  End