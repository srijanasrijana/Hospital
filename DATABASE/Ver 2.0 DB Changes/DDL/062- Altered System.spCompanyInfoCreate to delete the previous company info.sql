
/****** Object:  StoredProcedure [System].[spCompanyInfoCreate]    Script Date: 2017-01-12 3:14:46 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:<Anoj Kumar Shrestha>
-- Create English date: <2012/08/30>
-- Create Nepali  date: <2069/05/15>
-- Description:	<This Class is for Creating Project>
-- =============================================

ALTER PROCEDURE [System].[spCompanyInfoCreate](
@CompanyInfoID INT = NULL,
@CompanyName NVARCHAR(50),
@CompanyCode NVARCHAR(50),
@Address1 NVARCHAR(80)= NULL,
@Address2 NVARCHAR(80)= NULL,
@City NVARCHAR(50)=NULL,
@District NVARCHAR(50) = NULL,
@Zone NVARCHAR(50)= NULL,
@Telephone NVARCHAR(80)= NULL,
@Email NVARCHAR(80)=NULL,
@Website NVARCHAR(80)=NULL,
@POBox NVARCHAR(50) = NULL,
@PAN NVARCHAR(50) = NULL,
@Logo Image = NULL,
@FYFrom DATETIME = NULL,
@BookBeginFrom DATETIME = NULL,
@DBName NVARCHAR(50) = NULL,
@Modified_By NVARCHAR(20) = NULL,
@FiscalYear nvarchar(20)=null,
@Return NVARCHAR(20) OUTPUT
)
AS

--------------------
SET NOCOUNT ON
	IF(@CompanyInfoID !=NULL)
	BEGIN		
			raiserror('Company already exist.',15,1);
			SET @Return='FAILURE';
			RETURN -100
		--SET @SeriesID=Acc.fnGetSeriesIDFromName(@SeriesName,@Language);
	END
	---------------------------
	--IF @CompanyInfoID IS NULL
	--BEGIN
	--	RAISERROR('Company not selected.',15,1)
	--	RETURN -100
	--END

--DO THE REAL INSERT
	INSERT INTO System.tblCompanyInfo VALUES(
										@CompanyName,@CompanyCode,@Address1,@Address2,@City,@District,@Zone,@Telephone,@Email,@Website,
										@POBox,@PAN,@Logo,@FYFrom,@BookBeginFrom,@DBName,
										@Modified_By,GetDate(),@FiscalYear
										)
	Delete from System.tblCompanyInfo where CompanyInfoID <> scope_identity();									
	if @@Error<>0
	begin
		raiserror('An error occured during company creation. Please check the fields and try again!',15,1);
		SET @Return='FAILURE';
		RETURN -100
	end
		SET @Return = 'SUCCESS';
