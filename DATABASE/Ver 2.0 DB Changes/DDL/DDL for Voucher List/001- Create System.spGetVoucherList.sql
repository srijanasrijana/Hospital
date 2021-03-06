--USE [BENT003]
--GO
/****** Object:  StoredProcedure [System].[spGetVoucherList]    Script Date: 2/6/2017 9:25:44 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
create PROCEDURE [System].[spGetVoucherList]
	-- Add the parameters for the stored procedure here
	@MasterID nvarchar(10),
	@VoucherType nvarchar(100),
	@NavigateTo nvarchar(30),
	@MasterTableName nvarchar(100),
	@DetailsTableName nvarchar(100),
	@FieldName nvarchar(100),
	@FilterStr nvarchar(max),
	@VouchCode int,
	@FromDate nvarchar(100),
	@ToDate nvarchar(100),
	@DateField nvarchar(100),
	@RecordCount    INT    OUTPUT
	, @MinDate date OUTPUT

	
AS
BEGIN

		Declare @sql nvarchar(Max);
		Declare @Operator nvarchar(20);
		Declare @OrderBy nvarchar(100);
		declare @cnt nvarchar(20)  ;
		declare @SearchStr nvarchar(max);
		set @RecordCount = 0;
		declare @SqlDate nvarchar(max);
		declare @Date date;
	    declare @DateSearchStr nvarchar(max);
		declare @DefaultDate nvarchar(100);
		--declare @DefaultDate nvarchar(max)
	

	-- case for search String for each voucher type
    set @DateSearchStr = 'and (tblM.'+@DateField+' >= '''+@FromDate+''' and tblM.'+@DateField+' <='''+@ToDate+''') ' ;		 


		if(@VouchCode =0)  -- when the search string is empty
		begin 

            set @SearchStr = @DateSearchStr +' ' ;		 
		end

		else if(@VouchCode =1) -- FOR JOURNAL AND CONTRA
		begin 

            set @SearchStr = @DateSearchStr + 'and (' + --convert(nvarchar(20), Created_Date,126) like ''%' + @FilterStr + '%''  or tblM.Voucher_No like ''%' + @FilterStr +'%''  or 
			'tblM.Voucher_No like ''%' + @FilterStr +
			'%''  or tblM.Remarks like ''%' + @FilterStr +
            '%''  or (Select sum(tblD.Amount) from ' + @DetailsTableName + ' as tblD where tblM.' + @FieldName + '= tblD.' + @FieldName + ' and tblD.DrCr = ''Credit'') like ''%' + @FilterStr +
            '%'' ) ' ;		 
		end

		else if(@VouchCode =2) -- BANK RECEIPT, BANK PAYMENT, CASH RECEIPT, CASH PAYMENT
		begin 
		set @SearchStr = @DateSearchStr + 'and ('+ --convert(nvarchar(20), Created_Date,126) like ''%' + @FilterStr + '%''  or 
		'tblM.Voucher_No like ''%' + @FilterStr +
			 '%''  or tblLedger.EngName like ''%' + @FilterStr +
            '%''  or tblM.Remarks like ''%' + @FilterStr +
            '%''  or (Select sum(tblD.Amount) from ' + @DetailsTableName + ' as tblD where tblM.' + @FieldName + '= tblD.' + @FieldName + ' ) like ''%' + @FilterStr +
            '%'' ) ' ;	
		end

		else if(@VouchCode =3) -- PURCHASE ORDER, SALES ORDER
		begin 
		set @SearchStr = @DateSearchStr + 'and ('+ --convert(nvarchar(20), Created_Date,126) like ''%' + @FilterStr + '%''  or
		' tblM.OrderNo  like ''%' + @FilterStr +

            '%''  or tblM.Remarks like ''%' + @FilterStr +
			 '%''  or tblLedger.EngName like ''%' + @FilterStr +
            '%''  or (Select sum(tblD.Amount) from ' + @DetailsTableName + ' as tblD where tblM.' + @FieldName + '= tblD.' + @FieldName + ') like ''%' + @FilterStr +
            '%'' ) ' ;	
		end

		else if(@VouchCode = 4) -- SALES INVOICE, PURCHASE INVOICE, SALES RETURN, PURCHASE RETURN
		begin 
		set @SearchStr = @DateSearchStr + 'and ('+ --convert(nvarchar(20), Created_Date,126) like ''%' + @FilterStr +'%''  or 
		'tblM.Voucher_No like ''%' + @FilterStr +
		 '%''  or tblLedger.EngName like ''%' + @FilterStr +
            '%''  or tblM.Remarks like ''%' + @FilterStr +
            '%''  or tblM.Gross_Amount like ''%' + @FilterStr +
            '%'' ) ' ;	
		end

	--else
	--begin
	--	set @SearchStr = ' ';
	--end
	--  for minimum Date from Master Table
		set @SqlDate = 'select @Date = COALESCE(min('+@DateField+'), getDate()) from '+@MasterTableName+' ';
		exec sp_executesql
		 @query = @SqlDate,
		 @Date = N'@Date date OUTPUT',
		 @Date = @Date output;

		 set @MinDate = @Date;


		  -- for default date format
		set @SqlDate = 'select @DefaultDate = Value from System.tblSettings where Code = ''DEFAULT_DATE'' ';
		exec sp_executesql
		 @query = @SqlDate,
		 @DefaultDate = N'@DefaultDate nvarchar(20) OUTPUT ',
		 @DefaultDate = @DefaultDate output;

		 if(@DefaultDate = 'Nepali')
			  set @DefaultDate = ' Date.fnEngtoNep(tblM.'+@DateField+') as Created_Date ';
		else
			  set @DefaultDate = 'convert(varchar, tblM.'+@DateField+', 111) as Created_Date ';
		-- set @MinDate = @Date;
	-- case for navigation

	IF(@NavigateTo='PREV')
	BEGIN
		 --Get 1 step previous records	
		set @operator = '<';
		set @OrderBy =' Order By ID desc ';
	   set @cnt = '(30)';	
	END 

	else IF(@NavigateTo='NEXT')
	BEGIN
		-- Get 1 step next records
	   set @operator = '>';
	   set @OrderBy =' Order By ID ';
	   set @cnt = '(30)';	
	END 

	else IF(@NavigateTo='FIRST')
	BEGIN
		-- Get the first records
		set @operator = '>';	
		set @OrderBy =' Order By ID ';
	   set @cnt = '(30)';	
	END 

	else IF(@NavigateTo='LAST')
	BEGIN
		-- Get the last records
		declare @temp_cnt int ;
		set @operator = '>';
		set @cnt = '(30)';	

		Declare @sql1 nvarchar(max);
	-- for last donot load top 2 

	if(@VoucherType='SALES_INVOICE' or @VoucherType = 'PURCHASE_INVOICE' or @VoucherType = 'PURCHASE_RETURN' or @VoucherType = 'SALES_RETURN')
	begin
		set @sql1 = 'select @temp_cnt = coalesce(count(*),0) from '+@MasterTableName+' as tblM '+
					'cross join Acc.tblLedger where tblM.CashPartyLedgerID = tblLedger.LedgerID '+  @SearchStr;
    end

		if(@VoucherType = 'JOURNAL' or @VoucherType = 'CONTRA' or @VoucherType = 'SALES_ORDER' or @VoucherType = 'PURCHASE_ORDER' or @VoucherType = 'BANK_RECEIPT' or @VoucherType = 'BANK_PAYMENT' or @VoucherType = 'CASH_PAYMENT' or @VoucherType = 'CASH_RECEIPT' )
		begin 

		set @sql1 ='SELECT @temp_cnt =  coalesce(count(*),0)  FROM '+@MasterTableName+' as tblM  where tblM.'+ @FieldName + @operator + @MasterID + @SearchStr;

		end

		exec sp_executesql
		 @query = @sql1,
		 @Params = N'@temp_cnt int OUTPUT',
		 @temp_cnt = @temp_cnt output;

		 if(@temp_cnt is not null)
		 begin
			 set @RecordCount = @temp_cnt;

			 if (@temp_cnt%30) <>0         -- if total records is 55 then load 55%30  i.e 25 record when navigation is 'last'
			 begin
				 set @cnt = @temp_cnt;
				 set @cnt ='('+ @cnt + '%30)';
			 end
		 end
		 --else
			--set @cnt = '(30)';
		 print @cnt;

		 set @OrderBy =' Order By ID desc';
	
	
	END 
	else IF(@NavigateTo='ALL')
	BEGIN
		-- Get all of the records for crystal report
		set @operator = '>';	
		set @OrderBy ='Order By ID';
	   set @cnt = '100 percent ';	
	END 
	SET NOCOUNT ON;

	IF(@VoucherType='SALES_INVOICE' or @VoucherType='SALES_RETURN' or  @VoucherType='PURCHASE_INVOICE'  or @VoucherType='PURCHASE_RETURN') -- gross amount is available in Master table

	BEGIN
	set @sql= 'select distinct  top '+@cnt+' tblM.'+@FieldName+

	' as ID,tblM.Voucher_No,'+@DefaultDate+', tblM.Remarks,tblLedger.EngName, COALESCE(tblM.Gross_Amount, 0) as Gross_Amount from '+

		''+@MasterTableName+' as tblM inner join Acc.tblLedger on tblM.CashPartyLedgerID = tblLedger.LedgerID where '

		+ @FieldName + @operator + @MasterID + @SearchStr + @OrderBy;	

	END

	ELSE IF(@VoucherType='SALES_ORDER' or @VoucherType='PURCHASE_ORDER' )  -- OrderNo is selected instead of Voucher_No

	BEGIN
		set @sql= 'SELECT distinct  top '+@cnt+'  tblM.'+@FieldName+' as ID, '+@DefaultDate+' ,'+
		'tblM.OrderNo as Voucher_No,tblM.Remarks, Acc.tblLedger.EngName,'+
		'COALESCE((Select sum(tblD.Amount) from '+@DetailsTableName+' as tblD where tblM.'+@FieldName+' = tblD.'+@FieldName+'),0) as Gross_Amount '+
		'FROM  '+@MasterTableName+' as tblM CROSS JOIN '+
                       --  '+@DetailsTableName+' as tblD ON tblM.'+@FieldName+' = tblD.'+@FieldName+' CROSS JOIN
        'Acc.tblLedger where Acc.tblLedger.LedgerID = tblM.CashPartyID and tblM.'

		+ @FieldName + @operator + @MasterID + @SearchStr + @OrderBy;	
	END

	else IF( @VoucherType = 'BANK_RECEIPT' or @VoucherType = 'BANK_PAYMENT' or @VoucherType = 'CASH_PAYMENT' or @VoucherType = 'CASH_RECEIPT')

	BEGIN
		set @sql= 'SELECT distinct  top '+@cnt+'  tblM.'+@FieldName+' as ID, '+@DefaultDate+' ,'+
		'tblM.Voucher_No,tblM.Remarks, Acc.tblLedger.EngName,'+
		'COALESCE((Select sum(tblD.Amount) from '+@DetailsTableName+' as tblD where tblM.'+@FieldName+' = tblD.'+@FieldName+'), 0)  as Gross_Amount '+
		'FROM '+@MasterTableName+' as tblM  inner JOIN '+
        'Acc.tblLedger on Acc.tblLedger.LedgerID = tblM.LedgerID and tblM.'

		+ @FieldName + @operator + @MasterID + @SearchStr + @OrderBy;	
	END


	else IF(@VoucherType='JOURNAL'or @VoucherType='CONTRA') -- gross amount is calculated from details table by sum of credit or debit value and ledger name is not required

	BEGIN
		set @sql= ' SELECT distinct  top '+@cnt+' tblM.'+@FieldName+' as ID, '+@DefaultDate+' ,'+
		'tblM.Voucher_No, tblM.Remarks, NULL as EngName,'+
		'COALESCE((Select sum(tblD.Amount) from '+@DetailsTableName+' as tblD where tblM.'+@FieldName+' = tblD.'+@FieldName+' AND tblD.Drcr = ''Credit''), 0)  as Gross_Amount '+
		'FROM '+@MasterTableName+' as tblM  where tblM.'
		 + @FieldName + @operator + @MasterID + @SearchStr +  @OrderBy;

	END

 
   --print @sql;
   --print @RecordCount;
	exec sp_executesql @sql;
END

