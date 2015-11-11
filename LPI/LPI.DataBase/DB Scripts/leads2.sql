drop procedure [dbo].[usp_Leads2]
go
CREATE PROCEDURE [dbo].[usp_Leads2]
	 @DB			SMALLINT
	,@CampaignId	INT
	,@MinDate		DATETIME 
	,@MaxDate		DATETIME
AS
BEGIN
	SET NOCOUNT ON;



select l.DB,
	   l.campaignId,
		L.InComingNumber, 
		L.TimeStamp, 
		L.durationMinutes, 
		L.fileURL,
		p.AMID, 
		p.name,
		p.firstname,
		L.PrimaryPhone,
		SUM(DP.Amount) Amount
	FROM	
			Lead L,Patient P , DENTALPROCEDURE DP 
	WHERE P.DB = @DB
	  AND   DP.DB = @DB
	  AND   L.DB = @DB
	  AND   L.PatientId= p.AMID
	  AND   DP.PatientId= p.AMID
	AND L.campaignID=@CampaignId	
	AND	CAST(FLOOR(CAST(L.TimeStamp AS FLOAT)) AS DATETIME) BETWEEN @MinDate AND @MaxDate
	AND L.original=0
	--AND CAST(FLOOR(CAST(DP.DateTime AS FLOAT)) AS DATETIME) BETWEEN l.TimeStamp AND @MaxDate
	GROUP by l.DB,
	l.campaignId,
			L.InComingNumber,
			 L.TimeStamp,
			 L.durationMinutes,
			 L.fileURL,
			 p.AMID,
			 p.name,
			 p.firstname,
			 L.PrimaryPhone
	ORDER by l.DB,
			 L.TimeStamp
	DESC

END
go

usp_Leads2 500,1,'2011-1-1','2011-3-31'
usp_Leads2 1113,1,'2011-1-1','2011-12-31'



-- select * from DENTALPROCEDURE where DB=@DB order by dateTime
-- select * from lead where DB=@DB order by timestamp