
/****** Object:  UserDefinedFunction [Acc].[fnGetAllAccClass]    Script Date: 06/29/2015 07:54:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--This functions intakes an AccClassID and gives out all AccClassID under it
Create FUNCTION [Acc].[fnGetAllAccClass](@AccClassID XML) 
RETURNS @AccTable TABLE
(
	AccClassID INT NOT NULL,
	EngName NVARCHAR(50) NULL,
	ParentID INT NULL
)
AS
BEGIN


 --Read All Accounting Class
DECLARE @SourceAccClassID TABLE
(
	AccClassID INT 
);

-- Write to temporary table @JAccClassID from xml
INSERT INTO @SourceAccClassID(AccClassID)
    SELECT
        IdNode.value('(.)[1]', 'int')
    FROM
        @AccClassID.nodes('/AccClassIDSettings/AccClassID') AS IdTbl(IdNode);
        
        
        
WITH AccClassCTE AS (
  SELECT AccClassID, EngName, ParentID 
  FROM Acc.tblAccClass
  WHERE AccClassID IN (SELECT AccClassID FROM @SourceAccClassID)
    
  UNION ALL
  
  SELECT prnt.AccClassID, prnt.EngName, prnt.ParentID 
  FROM AccClassCTE AS accls
    INNER JOIN Acc.tblAccClass AS prnt
      ON accls.AccClassID = prnt.ParentID
)
INSERT INTO @AccTable
SELECT DISTINCT(AccClassID),EngName,ParentID FROM AccClassCTE;
RETURN
END