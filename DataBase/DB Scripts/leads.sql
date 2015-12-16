use lpi
set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
GO


--Execute [dbo].[usp_Leads] @DB=500, @CampaignId=1, @MinDate='Jan 01, 2011', @MaxDate ='Jan 28, 2011'
--Execute [dbo].[usp_Leads] @DB=500, @CampaignId=1, @MinDate='Feb 01, 2011', @MaxDate ='Feb 28, 2011'
--Execute [dbo].[usp_Leads] @DB=500, @CampaignId=1, @MinDate='Mar 01, 2010', @MaxDate ='Mar 28, 2010'

ALTER PROCEDURE [dbo].[usp_Leads]
	 @DB			SMALLINT
	,@CampaignId	INT
	,@MinDate		DATETIME 
	,@MaxDate		DATETIME
AS
BEGIN
	SET NOCOUNT ON;

	-----Leads
	SELECT	  L.DB
			, L.CampaignId
			, L.InComingNumber
			, L.TimeStamp
			, L.durationMinutes
			, L.fileURL 
			, P.AMID PatientId
			, P.Name
			, P.firstname
			, A.DateTime
			, SUM(DP.Amount) Amount
	FROM	
			Lead L WITH(NOLOCK)
	INNER JOIN
			Patient P WITH(NOLOCK) ON L.PatientId = P.AMID
	LEFT  OUTER JOIN
			APPOINTMENT A WITH(NOLOCK) ON L.DB = A.DB
									  AND L.PatientId = A.PatientId
									  AND CAST(FLOOR(CAST(A.DateTime AS FLOAT)) AS DATETIME) BETWEEN @MinDate AND @MaxDate
	LEFT  OUTER JOIN
			DENTALPROCEDURE DP WITH(NOLOCK) ON L.DB = DP.DB
										   AND L.PatientId = DP.PatientId
										   AND CAST(FLOOR(CAST(DP.DateTime AS FLOAT)) AS DATETIME) BETWEEN @MinDate AND @MaxDate

	WHERE	L.DB = @DB
	  AND   L.CampaignId = @CampaignId
	  AND	CAST(FLOOR(CAST(L.TimeStamp AS FLOAT)) AS DATETIME) BETWEEN @MinDate AND @MaxDate
	  AND   L.original = 0

	GROUP BY
			  L.DB
			, L.CampaignId
			, L.InComingNumber
			, L.TimeStamp
			, L.durationMinutes
			, L.fileURL 
			, P.AMID
			, P.Name
			, P.firstname
			, A.DateTime

	ORDER BY 
			L.TimeStamp
END





