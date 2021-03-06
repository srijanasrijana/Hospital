
/****** Object:  StoredProcedure [System].[spCreateVouNumConfig]    Script Date: 23-Sep-16 11:02:43 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER procedure [System].[spCreateVouNumConfig]
(
@NumberingType nvarchar(50),
@DuplicateVouNum nvarchar(50),
@BlankVouNum nvarchar(50),
@StartingNo int,
@SpecifyEndNo bit,
@EndNo int,
@WarningVouLeft int,
@WarningMsg nvarchar(50),
@RenumberingFrq nvarchar(50),
@NumericPart bit,
@TotalLengthNumPart int,
@PaddingChar nvarchar(50),
@SeriesID int,
@HideVouNum bit
)

as
	INSERT INTO System.tblVouNumConfig(NumberingType,DuplicateVouNum,BlankVouNum,StartingNo,SpecifyEndNo,EndNo,WarningVouLeft,WarningMsg,RenumberingFrq,NumericPart,TotalLengthNumPart,PaddingChar,SeriesID,HideVouNum)
	values(@NumberingType,@DuplicateVouNum,@BlankVouNum,@StartingNo,@SpecifyEndNo,@EndNo,@WarningVouLeft,@WarningMsg,@RenumberingFrq,@NumericPart,@TotalLengthNumPart,@PaddingChar,@SeriesID,@HideVouNum)

