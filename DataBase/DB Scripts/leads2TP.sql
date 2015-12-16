drop procedure [dbo].[usp_Leads2TP]
go
CREATE PROCEDURE [dbo].[usp_Leads2TP]
	 @DB			SMALLINT
	,@CampaignId	INT
	,@MinDate		DATETIME 
	,@MaxDate		DATETIME
	,@WithTreatmentPlan SMALLINT 

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
	AND L.original=0
	  AND   DP.DB = @DB
	  AND   L.DB = @DB
	  AND   L.PatientId= p.AMID
	  AND   DP.PatientId= p.AMID
	AND L.campaignID=@CampaignId	
	AND	CAST(FLOOR(CAST(L.TimeStamp AS FLOAT)) AS DATETIME) BETWEEN @MinDate AND @MaxDate
	AND DP.TreatmentPlan=@WithTreatmentPlan
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

usp_Leads2TP 1148,1,'2011-1-1','2012-12-31',0


select * from lead where db=1148 and campaignid=1