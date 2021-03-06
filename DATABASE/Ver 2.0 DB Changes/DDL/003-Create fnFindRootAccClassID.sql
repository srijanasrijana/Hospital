--USE [BENT001]
--GO
/****** Object:  UserDefinedFunction [Acc].[FindRoot]    Script Date: 7/27/2015 9:09:55 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--Intakes any accounting class id and gives its Root Accounting Class ID
CREATE FUNCTION [Acc].[fnFindRootAccClassID](@id int)
RETURNS int
AS  
BEGIN 
  
  DECLARE @AccClassID int=0;
  --SELECT @AccClassID = AccClassID
  --FROM Acc.tblAccClass
  --WHERE ParentID = @id
  SELECT @AccClassID=ParentID FROM Acc.tblAccClass WHERE AccClassID=@id
  --if @AccClassID =1 and @id is not 1 then, throw invalid accounting class exception
  IF(@AccClassID != @id AND @AccClassID=0)
	BEGIN
	Return -1; --minus one is for error
	END
  ELSE --Everything is valid now digg for the root ID
	WHILE @AccClassID is not NULL
		BEGIN
		  SELECT @id = @AccClassID
		  SELECT @AccClassID = ParentID
		  FROM Acc.tblAccClass
		  WHERE AccClassID = @id
		END

  RETURN @id
END