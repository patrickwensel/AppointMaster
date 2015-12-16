set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[usp_LeadsMatchNoTreatments]
	 @DB			SMALLINT
	,@CampaignId	INT
	,@MinDate		DATETIME 
	,@MaxDate		DATETIME
AS
BEGIN
	SET NOCOUNT ON;


select l.ID,
	   l.campaignId,
		L.InComingNumber, 
		L.TimeStamp, 
		L.durationMinutes, 
		L.fileURL,
		p.AMID, 
		p.name,
		p.firstname,
		L.PrimaryPhone

	FROM	
			Lead L,Patient P
	WHERE L.DB = @DB
    AND   L.PatientId= p.AMID
    AND   L.original=0
	AND L.campaignID=@CampaignId	
	AND	CAST(FLOOR(CAST(L.TimeStamp AS FLOAT)) AS DATETIME) BETWEEN @MinDate AND @MaxDate
	AND l.patientId not in (select patientId from dentalprocedure where db=@DB)
    AND   p.AMID = L.PatientId
    AND   p.DB = @DB

	GROUP by l.ID,
			l.campaignId,
			l.InComingNumber,
			 l.TimeStamp,
			 l.durationMinutes,
			 l.fileURL,
		p.AMID, 
		p.name,
		p.firstname,
		L.PrimaryPhone
		

	ORDER by L.TimeStamp
	DESC

END


/**************************************************************************************/




