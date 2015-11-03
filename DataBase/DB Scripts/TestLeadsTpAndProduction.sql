use lpi



declare @DB int
declare @MaxDate datetime
declare @CampaignId	int
declare @MinDate datetime

set @DB=1135
set @MaxDate='1/1/2100'
set @CampaignId	=1
set @MinDate ='1/1/1900'

select  L.InComingNumber, 
		L.TimeStamp, 
		L.durationMinutes, 
		L.fileURL,
		p.AMID, 
		p.name,
		p.firstname, 
		L.PrimaryPhone,
		SUM(DP.Amount) Amount,
		DP.TreatmentPlan
	FROM	
			Lead L,Patient P , DENTALPROCEDURE DP 
	WHERE P.DB = @DB
	  AND   DP.DB = @DB
	  AND   L.DB = @DB
	  AND   L.original=0
	  AND   L.PatientId= p.AMID
	  AND   DP.PatientId= p.AMID
	AND L.campaignID=@CampaignId	
	AND	CAST(FLOOR(CAST(L.TimeStamp AS FLOAT)) AS DATETIME) BETWEEN @MinDate AND @MaxDate
	GROUP by L.InComingNumber,
			 L.TimeStamp,
			 L.durationMinutes,
			 L.fileURL,
			 p.AMID,
			 p.name,
			 p.firstname,
			 L.PrimaryPhone,
			 DP.TreatmentPlan
	ORDER by L.TimeStamp desc,
			p.AMID,
			 L.PrimaryPhone,
			 DP.TreatmentPlan
			 