
USE LPI
GO
DROP PROCEDURE [dbo].[usp_TopProcedures]
GO



CREATE PROCEDURE [dbo].[usp_TopProcedures]
	 @DB	  SMALLINT
	,@MinDate DATETIME 
	,@MaxDate DATETIME
AS
BEGIN
	SET NOCOUNT ON;
	-----Campaign Name

	SELECT	  TOP 5
			  DP.Code
			, P.Name
			, p.homephone,p.cellphone,p.workphone
			, DP.Amount
			, C.name
	FROM
			DENTALPROCEDURE DP ,
			Patient p,
			Campaign C,
			Lead l
	where 

	CAST(FLOOR(CAST(DP.DateTime AS FLOAT)) AS DATETIME) BETWEEN @MinDate AND @MaxDate
	AND DP.DB = @DB
	AND l.original=0
	and DP.Patientid=p.amid
	and (l.incomingnumber = p.homephone or  l.incomingnumber = p.cellphone or l.incomingnumber = p.workphone)
	and (l.db=@db)
	and c.Id=l.campaignId
	and (c.db=@db)
ORDER   BY 
			DP.Amount DESC
END


