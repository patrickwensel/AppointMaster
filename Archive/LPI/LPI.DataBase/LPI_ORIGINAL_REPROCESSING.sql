/*************************************************************************************
				Sequence to update LPI for lead original status
Author: Philip
Date : 1/10/2014
*************************************************************************************/

/*************************************************************************************
1 Alter Table Lead
*************************************************************************************/
use LPI
ALTER TABLE LEAD add  original tinyint not null default 0;

/*************************************************************************************
2 Create the lead_source
*************************************************************************************/
use LPI
drop table lead_source 
go
CREATE TABLE [dbo].[LEAD_SOURCE](
	[DB] [smallint] NOT NULL,
	[campaignID] [int] NOT NULL,
	[inComingNumber] [varchar](15) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[timeStamp] [datetime] NOT NULL,
	[durationMinutes] [smallint] NOT NULL,
	[newPatient] [smallint] NOT NULL,
	[patientId] [varchar](12) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[fileURL] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[birthday] [datetime] NOT NULL DEFAULT ('1900/1/1'),
	[dentalNeed] [smallint] NOT NULL DEFAULT ((0)),
	[dentalCareIsFor] [smallint] NOT NULL DEFAULT ((0)),
	[preferredAppointmentTime] [smallint] NOT NULL DEFAULT ((0)),
	[insurancePlanBudget] [smallint] NOT NULL DEFAULT ((0)),
	[firstName] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL DEFAULT (''),
	[lastName] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL DEFAULT (''),
	[email] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL DEFAULT (''),
	[PrimaryPhone] [varchar](15) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL DEFAULT (''),
	[source] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[medium] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[term] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[content] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[campaign] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[referred_by] [varchar](40) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[original] [tinyint] NOT NULL DEFAULT ((0))
) ON [PRIMARY]
go

/*************************************************************************************
3 Inject all leads into lead_source
*************************************************************************************/

insert into lead_source select * from lead

/*************************************************************************************
4 Remove all test leads from source
*************************************************************************************/

delete from lead_source where db=500

/*************************************************************************************
4 Remove all leads from LEAD main table execpt test leads
*************************************************************************************/

delete from lead where db<>500

/*************************************************************************************
5 Apply ReprocessLead (see batch directory)
*************************************************************************************/


/*************************************************************************************
6 Check generation is pure
*************************************************************************************/

/* First name and last name should be unique among original leads*/
select rtrim(firstname),rtrim(lastname),count(*) from lead where original=0 and db<>500 group by rtrim(firstname),rtrim(lastname) order by count(*) desc

/* email should be unique among original leads*/
select rtrim(email),count(*) from lead where original=0 and db<>500 group by rtrim(email) order by count(*) desc

/* inComingNumber should be unique among original leads*/
select rtrim(inComingNumber),count(*) from lead where original=0 and db<>500 and len(incomingnumber)=10 group by rtrim(inComingNumber) order by count(*) desc


/*************************************************************************************
7 Check lead distribution
*************************************************************************************/

select dbo.OriginalStatus(original) as 'Status of lead' , count(*) as 'Total Number in database' from lead group by original




/*************************************************************************************
8 Apply Stored Procedures Changes
*************************************************************************************/


GO
/****** Object:  StoredProcedure [dbo].[usp_CampaignResult]    Script Date: 02/19/2014 16:47:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[usp_CampaignResult]
	 @DB	  SMALLINT
	,@MinDate DATETIME 
	,@MaxDate DATETIME
AS
BEGIN
	SET NOCOUNT ON;

	SELECT	CampaignId
		  , [Name]
		  , budget	
	      , Leads
		  , Patients
		  , (CASE WHEN TotalLeads > 0 THEN (100.0 * Leads)/TotalLeads ELSE 0.0 END) CampLeadPercent 
		  , TotalAmount
	FROM 
	(
			SELECT	  L.DB
					, L.CampaignId
					, C.Name
					, C.budget
					, COUNT(distinct L.inComingNumber) Leads
					, COUNT(distinct A.PatientId) Patients
					, (	 SELECT COUNT(*)
						 FROM   Lead subL WITH(NOLOCK)
						 WHERE  subL.DB = L.DB
						   AND	CAST(FLOOR(CAST(subL.TimeStamp AS FLOAT)) AS DATETIME) BETWEEN @MinDate AND @MaxDate
					  ) TotalLeads
					, (select sum(amount) from dentalprocedure where db=@DB and patientId in (select patientId from lead where DB=@DB and campaignId=L.CampaignId) )  TotalAmount
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
			  AND	L.original=0
			  AND	CAST(FLOOR(CAST(L.TimeStamp AS FLOAT)) AS DATETIME) BETWEEN @MinDate AND @MaxDate
			GROUP BY 
					L.DB, L.CampaignId, C.Name,C.budget
	) CL
	ORDER BY 
			[Name]
END






/**************************************************************************************/


GO
/****** Object:  StoredProcedure [dbo].[usp_Leads]    Script Date: 02/19/2014 16:49:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
	AND L.original=0	
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
			, P.firstname
			, A.DateTime

	ORDER BY 
			L.TimeStamp
END



/**************************************************************************************/



GO
/****** Object:  StoredProcedure [dbo].[usp_Leads2]    Script Date: 02/19/2014 16:50:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[usp_Leads2]
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
	  AND	L.original=0
	  AND   L.PatientId= p.AMID
	  AND   DP.PatientId= p.AMID
	AND L.campaignID=@CampaignId	
	AND	CAST(FLOOR(CAST(L.TimeStamp AS FLOAT)) AS DATETIME) BETWEEN @MinDate AND @MaxDate
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


/**************************************************************************************/


GO
/****** Object:  StoredProcedure [dbo].[usp_Leads2]    Script Date: 02/19/2014 16:50:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[usp_Leads2]
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
	  AND	L.original=0
	  AND   L.PatientId= p.AMID
	  AND   DP.PatientId= p.AMID
	AND L.campaignID=@CampaignId	
	AND	CAST(FLOOR(CAST(L.TimeStamp AS FLOAT)) AS DATETIME) BETWEEN @MinDate AND @MaxDate
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


/**************************************************************************************/



GO
/****** Object:  StoredProcedure [dbo].[usp_LeadsMatchNoTreatments]    Script Date: 02/19/2014 16:51:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[usp_LeadsMatchNoTreatments]
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

	GROUP by l.DB,
			l.campaignId,
			l.InComingNumber,
			 l.TimeStamp,
			 l.durationMinutes,
			 l.fileURL,
		p.AMID, 
		p.name,
		p.firstname,
		L.PrimaryPhone
		

	ORDER by l.DB,
			 L.TimeStamp
	DESC

END


/**************************************************************************************/



GO
/****** Object:  StoredProcedure [dbo].[usp_LeadsNoMatch]    Script Date: 02/19/2014 16:52:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


ALTER PROCEDURE [dbo].[usp_LeadsNoMatch]
	 @DB			SMALLINT
	,@CampaignId	INT
	,@MinDate		DATETIME 
	,@MaxDate		DATETIME
AS
BEGIN
	SET NOCOUNT ON;


select DB,
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
	GROUP by DB,
			campaignId,
			InComingNumber,
			 TimeStamp,
			 durationMinutes,
			 fileURL,
			 PrimaryPhone
	ORDER by DB,
			 TimeStamp desc

END


/**************************************************************************************/



GO
/****** Object:  StoredProcedure [dbo].[usp_LeadsTpAndProduction]    Script Date: 02/19/2014 16:52:27 ******/
SET ANSI_NULLS ON
GO
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
		DP.TreatmentPlan
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
	AND Amount >0
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

END



/**************************************************************************************/







/**************************************************************************************/



GO
/****** Object:  StoredProcedure [dbo].[usp_OneCampaignAllResult]    Script Date: 02/19/2014 16:53:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[usp_OneCampaignAllResult]
	 @DB	  SMALLINT
	,@CampID  INT
	,@MinDate DATETIME 
	,@MaxDate DATETIME
AS
		select 
				count( distinct( cast(l.incomingnumber as varchar(20)) + cast(l.primaryphone as varchar(20) )))   ,
				count(distinct(l.patientid)),
				sum(amount) 
		from 
			lead l,dentalprocedure d 
		where 
				l.db=@DB 
			and l.original = 0
			and campaignId=@CampID
			and d.db=@DB 
			and d.patientId=l.patientId 
			AND	CAST(FLOOR(CAST(L.TimeStamp AS FLOAT)) AS DATETIME) BETWEEN @MinDate AND @MaxDate



/**************************************************************************************/



GO
/****** Object:  StoredProcedure [dbo].[usp_OneCampaignResult]    Script Date: 02/19/2014 16:53:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[usp_OneCampaignResult]
	 @DB	  SMALLINT
	,@CampID  INT
	,@MinDate DATETIME 
	,@MaxDate DATETIME
	,@WithTreatmentPlan SMALLINT 
AS
		select 
				count( distinct( cast(l.incomingnumber as varchar(20)) + cast(l.primaryphone as varchar(20) )))   ,
				count(distinct(l.patientid)),
				sum(amount) 
		from 
			lead l,dentalprocedure d 
		where 
				l.db=@DB 
		    and l.original = 0
			and campaignId=@CampID
			and d.db=@DB 
			and d.patientId=l.patientId 
			and d.treatmentplan=@WithTreatmentPlan 
			AND	CAST(FLOOR(CAST(L.TimeStamp AS FLOAT)) AS DATETIME) BETWEEN @MinDate AND @MaxDate





/**************************************************************************************/



GO
/****** Object:  StoredProcedure [dbo].[usp_TopProcedures]    Script Date: 02/19/2014 16:54:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



ALTER PROCEDURE [dbo].[usp_TopProcedures]
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
	and DP.Patientid=p.amid
	and (l.incomingnumber = p.homephone or  l.incomingnumber = p.cellphone or l.incomingnumber = p.workphone)
	and (l.db=@db)
	and c.Id=l.campaignId
	and (c.db=@db)
	and l.original=0
ORDER   BY 
			DP.Amount DESC
END


