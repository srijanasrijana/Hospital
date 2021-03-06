/****** Object:  StoredProcedure [Inv].[spGetProductPartyDetails]    Script Date: 10/25/2017 5:38:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROC [Inv].[spGetProductPartyDetails] 

--Get the transaction for the particular Product
--SELECT DATEDIFF(DAY,  DATEADD(day, -1, @CreatedDate), GETDATE())
--product name | unit | net sales qty  | Net Amt
--Acc Class filter, Date filter, depot, single product, sales ledger, project
@PartyID INT=NULL,
@PartyGroupID INT=NULL,
@FromDate DATETIME=NULL,

@ToDate DATETIME = NULL, 
@AccountClassIDsSettings  xml,    -- It encludes AccountCLassIDs  Info
@ProjectIDsSettings xml,
@VoucherType nvarchar(50)
  AS
  Declare @docHandle int;
  Declare @docHandle1 int;

  exec sp_xml_preparedocument @docHandle output,@AccountClassIDsSettings
  exec sp_xml_Preparedocument @docHandle1 output, @ProjectIDsSettings

  Begin

  if (@PartyID is null)
  set @PartyID=0;
  if(@PartyGroupID is null)
  set @PartyGroupID=0;


  Declare @tblFinalResult table(
  PartyID int,
  TransactDate date,
  VoucherType nvarchar(30),
  VoucherNo nvarchar(50),
  ProductName nvarchar(50),
  InBoundQty decimal(19,5),
  OutBoundQty decimal(19,5),
  Amount decimal(19,5),
  RowID int,
  ParentID int,
  RowNumber int
  )

  --set fromdate and todate
  if(@FromDate is null)
  select  @FromDate = BookBeginFrom from System.tblCompanyInfo;
  if(@ToDate is null)
  set @ToDate=GETDATE();

  Declare @tblLedgerIDs table(ldgID int)
  Declare @tblGroupIDs table(gID int)

  if (@PartyID!=0)
  insert into @tblLedgerIDs values(@PartyID)

  else
  begin
  --if no group id is provided then select all partis 29-Debtors, 114-creditors, 102 cash in hand
  if(@PartyGroupID=0)
  begin
  insert into @tblGroupIDs values(102)
  insert into @tblGroupIDs values(29)
  insert into @tblGroupIDs values(114)
  end
  else
  begin
  --else insert the provided group id
    insert into @tblGroupIDs values(@PartyGroupID)
  end

  insert into @tblLedgerIDs select LedgerID from Acc.tblLedger where GroupID in(select *from @tblGroupIDs)

  ;with CTEGroupIDs(GroupID)
  as(
  select GroupID from Acc.tblGroup where Parent_GrpID in(select *from @tblGroupIDs)
  union all
  select g.GroupID  from Acc.tblGroup g, CTEGroupIDs cg where g.Parent_GrpID=cg.GroupID
  )

  insert into @tblLedgerIDs select LedgerID from Acc.tblLedger where GroupID in (select distinct GroupID from CTEGroupIDs)



  end



--get accounting class and project ids
  Declare @AccClassIDs table(accClassID int)
  Declare @ProjectIDs table(projectID int)

  insert into @AccClassIDs select AccClassID from Acc.fnGetAllAccClass (@AccountClassIDsSettings); 
  insert into @ProjectIDs   SELECT ProjectID FROM  Acc.fnGetAllProjects (@ProjectIDsSettings);

--Declare @tblVchRow table(voucherType nvarchar(30),rowID int)
--get all transaction related to the given party id
--insert into @tblVchRow
-- select distinct VoucherType,RowID from Acc.tblTransaction where LedgerID in(select ldgID from @tblLedgerIDs) and VoucherType in('PURCH','SALES','PURCH_RTN','SLS_RTN') and TransactDate between @FromDate and @ToDate

  --temp table
 create table #temp(TransactDate date,VoucherType nvarchar(20),Voucher_No nvarchar(20),ProductID int,InBound decimal(19,5),OutBound decimal(19,5),Net_Amount decimal(19,5),RowID int,CashPartyLedgerID int )

 Declare @RowCount int=0;
 Declare @CashPartName nvarchar(50)
 Declare @lID int=0;
 Declare  db_MainCursor cursor for select ldgID from @tblLedgerIDs 
 open db_MainCursor fetch next from db_MainCursor into @lID


 while @@FETCH_STATUS=0
 begin
 Declare @RID int=0;
 select @CashPartName=EngName from Acc.tblLedger where LedgerID=@lID
 select @RowCount=0;
 select @RowCount=ISNULL(count(*),0) from @tblFinalResult

  --party id distinguises the party name from product name (its for result display in gridview)
 insert into @tblFinalResult(PartyID,ProductName,RowNumber,ParentID)
 values(@lID,@CashPartName,@RowCount+1,0)

 if(@VoucherType='PURCHASE')
 BEGIN
          
           Declare db_Cursor cursor for select distinct RowID from Acc.tblTransaction where LedgerID=@lID and VoucherType='PURCH' and TransactDate between @FromDate and @ToDate
           open db_Cursor fetch next from db_Cursor into @RID
          
           while @@FETCH_STATUS=0
           begin
                 insert into #temp(TransactDate,VoucherType,Voucher_No,ProductID,InBound,OutBound,Net_Amount,RowID,CashPartyLedgerID)    
                 select distinct  t.TransactDate,t.VoucherType,pm.Voucher_No,pd.ProductID,pd.Quantity,0 as outbound,pd.Net_Amount,t.RowID,pm.CashPartyLedgerID  from Acc.tblTransaction t inner join Inv.tblPurchaseInvoiceMaster pm 
                 on t.RowID=pm.PurchaseInvoiceID inner join 
                 Inv.tblPurchaseInvoiceDetails pd on t.RowID=pd.PurchaseInvoiceID
                 where  t.RowID=@RID and t.VoucherType='PURCH'
                 
          	  
          	   	FETCH NEXT FROM db_Cursor INTO @RID   
          
            end
            
          CLOSE db_cursor   
          DEALLOCATE db_cursor
          
          set @RID=0;
          
          Declare db_Cursor cursor for select distinct RowID from Acc.tblTransaction where LedgerID=@lID and VoucherType='PURCH_RTN' and TransactDate between @FromDate and @ToDate
          open db_Cursor fetch next from db_Cursor into @RID
          
           while @@FETCH_STATUS=0
           begin
                 insert into #temp(TransactDate,VoucherType,Voucher_No,ProductID,InBound,OutBound,Net_Amount,RowID,CashPartyLedgerID)    
                 select distinct  t.TransactDate,t.VoucherType,prm.Voucher_No,prd.ProductID,0 as inbound,prd.Quantity,prd.Net_Amount,t.RowID,prm.CashPartyLedgerID  from Acc.tblTransaction t inner join Inv.tblPurchaseInvoiceMaster prm 
                 on t.RowID=prm.PurchaseInvoiceID inner join 
                 Inv.tblPurchaseInvoiceDetails prd on t.RowID=prd.PurchaseInvoiceID
                 where  t.RowID=@RID and t.VoucherType='PURCH_RTN'
                 
          	  
          	   	FETCH NEXT FROM db_Cursor INTO @RID   
          
            end
            
          CLOSE db_cursor   
          DEALLOCATE db_cursor

END

else if(@VoucherType='SALES')
     BEGIN
       set @RID=0;
       
        Declare db_Cursor cursor for select distinct RowID from Acc.tblTransaction where LedgerID=@lID and VoucherType='SALES' and TransactDate between @FromDate and @ToDate
        open db_Cursor fetch next from db_Cursor into @RID
       
        while @@FETCH_STATUS=0
        begin
              insert into #temp(TransactDate,VoucherType,Voucher_No,ProductID,InBound,OutBound,Net_Amount,RowID,CashPartyLedgerID)    
              select distinct  t.TransactDate,t.VoucherType,sm.Voucher_No,sd.ProductID,0 as inbound,sd.Quantity,sd.Net_Amount,t.RowID,sm.CashPartyLedgerID  from Acc.tblTransaction t inner join Inv.tblSalesInvoiceMaster sm 
              on t.RowID=sm.SalesInvoiceID inner join 
              Inv.tblSalesInvoiceDetails sd on t.RowID=sd.SalesInvoiceID
              where  t.RowID=@RID and t.VoucherType='SALES'
              
       	  
       	   	FETCH NEXT FROM db_Cursor INTO @RID   
       
         end
         
         CLOSE db_cursor   
       DEALLOCATE db_cursor
       
       set @RID=0;
       
       Declare db_Cursor cursor for select distinct RowID from Acc.tblTransaction where LedgerID=@lID and VoucherType='SLS_RTN' and TransactDate between @FromDate and @ToDate
        open db_Cursor fetch next from db_Cursor into @RID
       
        while @@FETCH_STATUS=0
        begin
              insert into #temp(TransactDate,VoucherType,Voucher_No,ProductID,InBound,OutBound,Net_Amount,RowID,CashPartyLedgerID)    
              select distinct  t.TransactDate,t.VoucherType,srm.Voucher_No,srd.ProductID,srd.Quantity,0 as outbound,srd.Net_Amount,t.RowID,srm.CashPartyLedgerID  from Acc.tblTransaction t inner join Inv.tblSalesReturnMaster srm 
              on t.RowID=srm.SalesReturnID inner join 
              Inv.tblSalesReturnDetails srd on t.RowID=srd.SalesReturnID
              where  t.RowID=@RID and t.VoucherType='SLS_RTN'
              
       	  
       	   	FETCH NEXT FROM db_Cursor INTO @RID   
       
         end
         
         CLOSE db_cursor   
       DEALLOCATE db_cursor
END



 select @RowCount=ISNULL(count(*),0) from @tblFinalResult
       insert into @tblFinalResult
       select 0,TransactDate,VoucherType,Voucher_No,p.EngName,InBound,OutBound,Net_Amount,RowID,CashPartyLedgerID,@RowCount+ROW_NUMBER() over(order by TransactDate) from #temp tp inner join Inv.tblProduct p on tp.ProductID=p.ProductID

        delete from #temp
	   	FETCH NEXT FROM db_MainCursor INTO @lID   

 end
   CLOSE db_MainCursor   
   DEALLOCATE db_MainCursor


   select *, Date.fnEngtoNep(TransactDate) as TransNepDate from @tblFinalResult order by RowNumber
 

  End