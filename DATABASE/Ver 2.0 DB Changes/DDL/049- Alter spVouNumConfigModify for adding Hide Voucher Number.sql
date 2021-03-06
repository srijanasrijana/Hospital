
/****** Object:  StoredProcedure [System].[spVouNumConfigModify]    Script Date: 23-Sep-16 11:08:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [System].[spVouNumConfigModify](
@SeriesID INT,
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
@HideVouNum bit

)
AS

--DO THE REAL UPDATE
	UPDATE System.tblVouNumConfig SET 
                                   NumberingType= @NumberingType ,
                                   DuplicateVouNum =@DuplicateVouNum,
                                   StartingNo= @StartingNo ,
                                   SpecifyEndNo=@SpecifyEndNo,
                                   EndNo= @EndNo ,
                                   WarningVouLeft =@WarningVouLeft ,
                                   WarningMsg = @WarningMsg ,
                                   RenumberingFrq =@RenumberingFrq ,
                                   NumericPart=  @NumericPart, 
                                   TotalLengthNumPart= @TotalLengthNumPart ,
                                   PaddingChar = @PaddingChar,
								   HideVouNum  = @HideVouNum

	WHERE SeriesID=@SeriesID;

	