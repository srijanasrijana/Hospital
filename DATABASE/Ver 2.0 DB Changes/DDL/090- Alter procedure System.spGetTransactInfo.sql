--USE [BENT003]
--GO
/****** Object:  StoredProcedure [System].[spGetTransactInfo]    Script Date: 4/19/2017 11:08:39 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
alter proc [System].[spGetTransactInfo]
as
begin 
declare @tblTransactInfo table( 
TransactionType nvarchar(50),
--IntervalType nvarchar(50),
Value nvarchar(50)
);

declare @DateToday date =  GetDate(); -- '2017-04-10';
declare @NepDate nvarchar(50) = '';
declare @Month nvarchar(50) = '';
declare @MonthName nvarchar(50) = '';

declare @Year nvarchar(50) = '';
declare @MonthStartDate nvarchar(50) = '';
declare @DaysInMonth nvarchar(50) = '';
declare @MonthEndDate nvarchar(50) = '';
declare @YearStartDate nvarchar(50) = '';
declare @YearEndDate nvarchar(50) = '';

declare @sqlstmt nvarchar(max) = '';
select @NepDate = Date.fnEngToNep(@DateToday);
	
	set @Year = SUBSTRING(@NepDate, 0, 5); -- extract year part from date
	set @Month = SUBSTRING(@NepDate, 6, 2); -- extract month part from date

select @YearStartDate = EngOnYearStart from Date.NepDate where Years = @Year;  -- get the first day of current year

select @YearEndDate = EngOnYearStart from Date.NepDate where Years = @Year + 1; -- get the first day of next year so that the last day of current year is the day before
--set @YearEndDate = convert(nvarchar(max), @YearEndDate, 111) -1;
select @YearEndDate = DATEADD(day,-1,@YearEndDate);
--select @DaysInMonth = (select monthName from System.tblMonth where monthId = @Month) from Date.NepDate;
--select @MonthName = (select monthName from System.tblMonth where monthId = @Month) from Date.NepDate;

	if(@Month = 1) 
	begin
		set @MonthName = 'Baisakh';
	end

	else if(@Month = 2) 
	begin
		set @MonthName = 'Jestha';
	end

	else if(@Month = 3) 
	begin
		set @MonthName = 'Asadh';
	end

	else if(@Month = 4) 
	begin
		set @MonthName = 'Shrawan';
	end

	else if(@Month = 5) 
	begin
		set @MonthName = 'Bhadra';
	end

	else if(@Month = 6) 
	begin
		set @MonthName = 'Aswin';
	end

	else if(@Month = 7) 
	begin
		set @MonthName = 'Kartik';
	end

	else if(@Month = 8) 
	begin
		set @MonthName = 'Mangsir';
	end

	else if(@Month = 9) 
	begin
		set @MonthName = 'Poush';
	end
	else if(@Month = 10) 
		begin
			set @MonthName = 'Magh';
		end

	else if(@Month = 11) 
	begin
		set @MonthName = 'Falgun';
	end

	else if(@Month = 12) 
		begin
			set @MonthName = 'Chaitra';
		end

set @sqlstmt = ' select @DaysInMonth =  '+@MonthName+' from Date.NepDate where Years = '+@Year+' ';
		exec sp_executesql
		 @query = @sqlstmt,
		 @DaysInMonth = N'@DaysInMonth int OUTPUT',
		 @DaysInMonth = @DaysInMonth output;
		 --set @DaysInMonth = @DaysInMonth;

set @MonthStartDate = @Year + '/'+@Month + '/01' ;
set @MonthEndDate = @Year + '/'+@Month + '/' + @DaysInMonth;

select @MonthStartDate = Date.fnNepToEng(@MonthStartDate);
select @MonthEndDate = Date.fnNepToEng(@MonthEndDate);

insert into @tblTransactInfo
	select 'All_Today' TransactionType, coalesce(count(distinct RowID), 0) Value from Acc.tblTransaction where   convert(date,TransactDate, 111) = convert(date,@DateToday, 111);

insert into @tblTransactInfo
	select 'All_Month' TransactionType, coalesce(count(distinct RowID),0) Value from Acc.tblTransaction where  convert(date,TransactDate, 111) >=@MonthStartDate and convert(date,TransactDate, 111) <= @MonthEndDate;

insert into @tblTransactInfo
	select 'All_Year' TransactionType, coalesce(count(distinct RowID), 0) Value from Acc.tblTransaction where   convert(date,TransactDate, 111) >=@YearStartDate and convert(date,TransactDate, 111) <= @YearEndDate;

insert into @tblTransactInfo
	select 'Sales_Today' TransactionType, coalesce(count(distinct RowID),0) Value from Acc.tblTransaction where  VoucherType = 'SALES'  and   convert(date,TransactDate, 111) = convert(date,@DateToday, 111);

insert into @tblTransactInfo
	select 'Sales_Month' TransactionType, coalesce(count(distinct RowID),0) Value from Acc.tblTransaction where VoucherType = 'SALES'  and  convert(date,TransactDate, 111) >= @MonthStartDate and convert(date,TransactDate, 111) <= @MonthEndDate;

insert into @tblTransactInfo
	select 'Sales_Year' TransactionType, coalesce(count(distinct RowID),0) Value from Acc.tblTransaction where  VoucherType = 'SALES'  and  convert(date,TransactDate, 111) >= @YearStartDate and convert(date,TransactDate, 111) <= @YearEndDate;

insert into @tblTransactInfo
	select 'Sales_Amount_Today' TransactionType, coalesce(sum(distinct Debit_Amount),0) Value from Acc.tblTransaction where  VoucherType = 'SALES'  and  convert(date,TransactDate, 111) = convert(date,@DateToday, 111);

insert into @tblTransactInfo
	select 'Sales_Amount_Month' TransactionType, coalesce(sum(distinct Debit_Amount),0) Value from Acc.tblTransaction where  VoucherType = 'SALES'  and  convert(date,TransactDate, 111) >= @MonthStartDate and convert(date,TransactDate, 111) <= @MonthEndDate;

insert into @tblTransactInfo
	select 'Sales_Amount_Year' TransactionType, coalesce(sum(distinct Debit_Amount),0) Value from Acc.tblTransaction where  VoucherType = 'SALES'  and  convert(date,TransactDate, 111) >= @YearStartDate and convert(date,TransactDate, 111) <= @YearEndDate;

insert into @tblTransactInfo
	select 'Recurring_Reminder' TransactionType, coalesce(count(distinct RVPID), 0) Value from System.tblRecurringVoucherPosting where isPosted = 'false' and convert(date,Date, 111) = convert(date, @DateToday, 111);
	--select 'All' TransactionType,'Today' IntervalType, count(distinct RowID) Value from Acc.tblTransaction where   convert(date,TransactDate, 111) >='2017-04-17'

select * from @tblTransactInfo ;
end