set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
GO


ALTER PROCEDURE [dbo].[usp_LeadsNoMatch]
	 @DB			SMALLINT
	,@CampaignId	INT
	,@MinDate		DATETIME 
	,@MaxDate		DATETIME
AS
BEGIN
	SET NOCOUNT ON;


select ID,
	   campaignId,
		InComingNumber, 
		TimeStamp, 
		durationMinutes, 
		fileURL,
		PrimaryPhone
	FROM	
			Lead 
	WHERE DB = @DB
	AND original = 0
	AND campaignID=@CampaignId	
	AND	CAST(FLOOR(CAST(TimeStamp AS FLOAT)) AS DATETIME) BETWEEN @MinDate AND @MaxDate
	AND patientId=''
	GROUP by ID,
			campaignId,
			InComingNumber,
			 TimeStamp,
			 durationMinutes,
			 fileURL,
			 PrimaryPhone
	ORDER by TimeStamp desc

END


/**************************************************************************************/




