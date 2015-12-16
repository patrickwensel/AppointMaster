use Lpi


drop PROCEDURE [dbo].[usp_CampaignResult]
go

CREATE PROCEDURE [dbo].[usp_CampaignResult]
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
			  AND	CAST(FLOOR(CAST(L.TimeStamp AS FLOAT)) AS DATETIME) BETWEEN @MinDate AND @MaxDate
			GROUP BY 
					L.DB, L.CampaignId, C.Name,C.budget
	) CL
	ORDER BY 
			[Name]
END
go

usp_CampaignResult 1113,'2011-1-1','2011-12-31'
go
usp_CampaignResult 500,'2011-1-1','2011-12-31'
select sum(amount) from dentalprocedure where db=1113 and patientId in (select patientId from lead where DB=1113 and campaignId=1)
--go


