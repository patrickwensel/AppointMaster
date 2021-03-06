select * from patient
select * from dentalprocedure where DB=500 and patientid=1224 and dateTime>='3/1/2011'  and  dateTime<'3/31/2011'
select * from appointment where amid='4448'
select * from lead
delete from lead
BULK INSERT LEAD FROM 'C:\CSHARP\LPI\DataBase\DataSet\leads.csv' WITH (FIELDTERMINATOR = ',')
ALTER TABLE lead add  fileURL VARCHAR(255)	not	null default '500/sample.wav';


select name from campaign where db=500 and ID=3
use lpi
usp_CampaignResult 500,'2011-1-1','2011-3-31'
usp_TopProcedures 500,'2011-3-1','2011-3-31'

usp_Leads 500,3,'2011-3-1','2011-3-31'
usp_Appointments 500,3,'2011-3-1','2011-3-31'
usp_Leads 500,3,'3/1/2011','3/1/2011'

USE LPI
GO
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_Appointments]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[usp_Appointments]

GO
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_CampaignName]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[usp_CampaignName]

GO
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_CampaignResult]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[usp_CampaignResult]

GO
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_Leads]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[usp_Leads]

GO
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_TopProcedures]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[usp_TopProcedures]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_Appointments]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'

--Execute [dbo].[usp_Appointments] @DB=500, @PatientId=95, @MinDate=''Jan 01, 2011'', @MaxDate =''Jan 28, 2011''
--Execute [dbo].[usp_Appointments] @DB=500, @PatientId=95, @MinDate=''Feb 01, 2011'', @MaxDate =''Feb 28, 2011''
--Execute [dbo].[usp_Appointments] @DB=500, @PatientId=95, @MinDate=''Mar 01, 2011'', @MaxDate =''Mar 28, 2011''

CREATE PROCEDURE [dbo].[usp_Appointments]
	 @DB		SMALLINT
	,@PatientID INT
	,@MinDate	DATETIME
	,@MaxDate	DATETIME
AS
BEGIN
	SET NOCOUNT ON;

	SELECT	  P.Name
			, DP.Code
			, DP.Amount
			, DP.DateTime
	FROM	
			Patient P WITH(NOLOCK)
	INNER JOIN
			DentalProcedure DP WITH(NOLOCK) ON P.AMID = DP.PatientId
										   AND CAST(FLOOR(CAST(DP.DateTime AS FLOAT)) AS DATETIME) BETWEEN @MinDate AND @MaxDate
	WHERE  DP.DB = @DB
	  AND  DP.PatientId = @PatientId

	ORDER By
			DP.Code

END



' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_TopProcedures]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
--Execute [dbo].[usp_TopProcedures] @DB=500, @CampaignId=1, @MinDate=''Jan 01, 2011'', @MaxDate =''Jan 28, 2011''
--Execute [dbo].[usp_TopProcedures] @DB=500, @CampaignId=1, @MinDate=''Feb 01, 2011'', @MaxDate =''Feb 28, 2011''
--Execute [dbo].[usp_TopProcedures] @DB=500, @CampaignId=1, @MinDate=''Mar 01, 2011'', @MaxDate =''Mar 28, 2011''

CREATE PROCEDURE [dbo].[usp_TopProcedures]
	  @DB	   SMALLINT
	 ,@MinDate DATETIME 
	 ,@MaxDate DATETIME
AS
BEGIN
	SET NOCOUNT ON;

	SELECT	  TOP 5
			  DP.Code
			, P.Name
			, CONVERT(VARCHAR(10), DP.DateTime, 101) [DateTime]
			, DP.Amount
	FROM
			DENTALPROCEDURE DP WITH(NOLOCK) 
	INNER	JOIN 
			PATIENT P ON DP.PatientId = P.AMID
					 AND CAST(FLOOR(CAST(DP.DateTime AS FLOAT)) AS DATETIME) BETWEEN @MinDate AND @MaxDate
					 AND DP.DB = @DB
	ORDER   BY 
			DP.Amount DESC
END

' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_Leads]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'

--Execute [dbo].[usp_Leads] @DB=500, @CampaignId=1, @MinDate=''Jan 01, 2011'', @MaxDate =''Jan 28, 2011''
--Execute [dbo].[usp_Leads] @DB=500, @CampaignId=1, @MinDate=''Feb 01, 2011'', @MaxDate =''Feb 28, 2011''
--Execute [dbo].[usp_Leads] @DB=500, @CampaignId=1, @MinDate=''Mar 01, 2010'', @MaxDate =''Mar 28, 2010''

CREATE PROCEDURE [dbo].[usp_Leads]
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

	GROUP BY
			  L.DB
			, L.CampaignId
			, L.InComingNumber
			, L.TimeStamp
			, L.durationMinutes
			, L.fileURL 
			, P.AMID
			, P.Name
			, A.DateTime

	ORDER BY 
			L.TimeStamp
END




' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_CampaignResult]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
--Execute [dbo].[usp_CampaignResult] @DB=500, @MinDate=''Jan 01, 2011'', @MaxDate =''Jan 28, 2011''
--Execute [dbo].[usp_CampaignResult] @DB=500, @MinDate=''Feb 01, 2011'', @MaxDate =''Feb 28, 2011''
--Execute [dbo].[usp_CampaignResult] @DB=500, @MinDate=''Mar 01, 2011'', @MaxDate =''Mar 28, 2011''

CREATE PROCEDURE [dbo].[usp_CampaignResult]
	 @DB	  SMALLINT
	,@MinDate DATETIME 
	,@MaxDate DATETIME
AS
BEGIN
	SET NOCOUNT ON;

	SELECT	CampaignId
		  , [Name]
	      , Leads
		  , Patients
		  , (CASE WHEN TotalLeads > 0 THEN (100.0 * Leads)/TotalLeads ELSE 0.0 END) CampLeadPercent 
		  , TotalAmount
	FROM 
	(
			SELECT	  L.DB
					, L.CampaignId
					, C.Name
					, COUNT(L.CampaignId) Leads
					, COUNT(A.PatientId) Patients
					, (	 SELECT COUNT(*)
						 FROM   Lead subL WITH(NOLOCK)
						 WHERE  subL.DB = L.DB
						   AND	CAST(FLOOR(CAST(subL.TimeStamp AS FLOAT)) AS DATETIME) BETWEEN @MinDate AND @MaxDate
					  ) TotalLeads
					, SUM(ISNULL(DP.AMOUNT,0)) TotalAmount
			FROM	Lead L WITH(NOLOCK)
			INNER JOIN 
					Campaign C WITH(NOLOCK) ON L.DB = C.DB AND L.CampaignId = C.ID
			LEFT  OUTER JOIN 
					APPOINTMENT A WITH(NOLOCK) ON L.DB = A.DB
 											  AND L.PatientId = A.PatientId
											  AND CAST(FLOOR(CAST(A.DateTime AS FLOAT)) AS DATETIME) BETWEEN @MinDate AND @MaxDate
			LEFT  OUTER JOIN
					DENTALPROCEDURE DP WITH(NOLOCK) ON L.DB = DP.DB
 												   AND L.PatientId = DP.PatientId
												   AND CAST(FLOOR(CAST(DP.DateTime AS FLOAT)) AS DATETIME) BETWEEN @MinDate AND @MaxDate
			WHERE	L.DB = @DB
			  AND	CAST(FLOOR(CAST(L.TimeStamp AS FLOAT)) AS DATETIME) BETWEEN @MinDate AND @MaxDate
			GROUP BY 
					L.DB, L.CampaignId, C.Name
	) CL
	ORDER BY 
			[Name]
END

' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_CampaignName]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'

--Execute [dbo].[usp_CampaignName] @DB=500, @CampaignId=1
--Execute [dbo].[usp_CampaignName] @DB=500, @CampaignId=2
--Execute [dbo].[usp_CampaignName] @DB=500, @CampaignId=3

CREATE PROCEDURE [dbo].[usp_CampaignName]
	 @DB			SMALLINT
	,@CampaignId	INT
AS
BEGIN
	SET NOCOUNT ON;
	-----Campaign Name

	SELECT	TOP 1 [name] AS CampaignName
	FROM
			CAMPAIGN C WITH(NOLOCK)
	WHERE	C.DB = @DB
	  AND   C.ID = @CampaignId
END




' 
END

