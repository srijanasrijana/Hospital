
/****** Object:  StoredProcedure [Acc].[spGetAllAccClass]    Script Date: 06/29/2015 07:06:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--This functions intakes an AccClassID and gives out all AccClassID under it
CREATE PROCEDURE [Acc].[spGetAllAccClass](@AccClassID XML) 

AS
BEGIN

SELECT * FROM Acc.fnGetAllAccClass(@AccClassID);

END