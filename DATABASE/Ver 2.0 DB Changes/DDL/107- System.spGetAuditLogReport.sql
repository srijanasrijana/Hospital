
/****** Object:  StoredProcedure [System].[spGetAuditLogReport]    Script Date: 08/25/2017 9:44:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [System].[spGetAuditLogReport] 
(@VoucherTypes nvarchar(max),
@userName nvarchar(50)=null,
@fromDate date,
@toDate date
)
AS
BEGIN
 
 Declare @userSetting nvarchar(100)
 Declare @sqlstmt nvarchar(max)
 
if(@userName is null or @userName='' or @userName=' ')
set @userSetting=' and 1=1 '
else
set @userSetting=' and UserName='+@userName+' ';

Create table #tblTemp (vchType nvarchar(50))

INSERT INTO #tblTemp
SELECT  x.Item 
FROM  Acc.fnSplitString(@VoucherTypes, ',') x

set @sqlstmt='SELECT ROW_NUMBER() over(order by  AuditLogID) AS RowNumber ,cast(VoucherDate as date) as EngVoucherDate,Date.fnEngtoNep(VoucherDate) as VoucherDate,ComputerName,UserName,Voucher_Type,[Action],[Description] from System.tblAuditLog where Voucher_Type in(select *from #tblTemp) ' +' and (VoucherDate between '''+  Convert(VARCHAR,@fromDate,20) + ''' and '''+Convert(VARCHAR,@toDate,20)+''''+') ' +@userSetting

--SELECT ROW_NUMBER() over(order by  AuditLogID) AS RowNumber ,cast(VoucherDate as date) as VoucherDate,Date.fnEngtoNep(VoucherDate) as NepVoucherDate,ComputerName,UserName,Voucher_Type,[Action],[Description] from System.tblAuditLog where Voucher_Type in(select *from #tblTemp)
EXECUTE sp_executesql @SqlStmt
drop table  #tblTemp
 END