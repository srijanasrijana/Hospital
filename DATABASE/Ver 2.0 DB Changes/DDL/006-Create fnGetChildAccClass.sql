
/****** Object:  UserDefinedFunction [Acc].[fnGetChildAccClass]    Script Date: 06/29/2015 07:23:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--This functions intakes an AccClassID and gives out all AccClassID under it
CREATE FUNCTION [Acc].[fnGetChildAccClass](@id INT) 
RETURNS @AccTable TABLE
(
	AccClassID INT NOT NULL,
	EngName NVARCHAR(50) NULL,
	ParentID INT NULL
)
AS
BEGIN
WITH AccClassCTE AS (
  SELECT AccClassID, EngName, ParentID 
  FROM Acc.tblAccClass
  WHERE AccClassID = @id
    
  UNION ALL
  
  SELECT prnt.AccClassID, prnt.EngName, prnt.ParentID 
  FROM AccClassCTE AS accls
    INNER JOIN Acc.tblAccClass AS prnt
      ON accls.AccClassID = prnt.ParentID
)
INSERT INTO @AccTable
SELECT AccClassID,EngName,ParentID FROM AccClassCTE;
RETURN
END