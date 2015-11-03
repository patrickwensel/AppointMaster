set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[usp_LeadsTpAndProduction]
	 @DB			SMALLINT
	,@CampaignId	INT
	,@MinDate		DATETIME 
	,@MaxDate		DATETIME

AS
BEGIN
	SET NOCOUNT ON;

select  L.InComingNumber, 
		L.TimeStamp, 
		L.durationMinutes, 
		L.fileURL,
		p.AMID, 
		p.name,
		p.firstname, 
		L.PrimaryPhone,
		SUM(DP.Amount) Amount,
		DP.TreatmentPlan,
		L.ID

	FROM	
			Lead L,Patient P , DENTALPROCEDURE DP 
	WHERE P.DB = @DB
	  AND   DP.DB = @DB
	  AND   L.DB = @DB
	  AND   L.original = 0
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
			 DP.TreatmentPlan,
			 L.ID

	ORDER by L.TimeStamp desc,
			p.AMID,
			 L.PrimaryPhone,
			 DP.TreatmentPlan

END



/**************************************************************************************/




