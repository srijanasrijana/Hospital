/****** Object:  UserDefinedFunction [Acc].[fnGetAllProjects]    Script Date: 07/09/2015 09:04:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--This functions intakes an ProjectID and gives out all ProjectID under it
ALTER FUNCTION [Acc].[fnGetAllProjects](@ProjectID XML) --@ProjectID should be in format <ProjectIDCollection><ProjectID>10</ProjectID></ProjectIDCollection>
RETURNS @ProjectTable TABLE
(
	ProjectID INT NOT NULL,
	EngName NVARCHAR(50) NULL,
	ParentProjectID INT NULL
)
AS
BEGIN


 --Read All Accounting Class
DECLARE @SourceProjectID TABLE
(
	ProjectID INT 
);

-- Write to temporary table @SourceProjectID from xml
INSERT INTO @SourceProjectID(ProjectID)
    SELECT
        IdNode.value('(.)[1]', 'int')
    FROM
        @ProjectID.nodes('/ProjectIDSettings/ProjectID') AS IdTbl(IdNode);
        
        
 --Get all ProjectIDs recursively       
WITH ProjectCTE AS (
  SELECT ProjectID, EngName, ParentProjectID 
  FROM Acc.tblProject
  WHERE ProjectID IN (SELECT ProjectID FROM @SourceProjectID)
    
  UNION ALL
  
  SELECT prnt.ProjectID, prnt.EngName, prnt.ParentProjectID 
  FROM ProjectCTE AS projects
    INNER JOIN Acc.tblProject AS prnt
      ON projects.ProjectID = prnt.ParentProjectID
)

--Now insert into the return table for returning the value
INSERT INTO @ProjectTable
SELECT DISTINCT(ProjectID),EngName,ParentProjectID FROM ProjectCTE;
RETURN
END