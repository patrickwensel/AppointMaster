use Lpi


drop PROCEDURE [dbo].[usp_CampaignResultTP]
go

CREATE PROCEDURE [dbo].[usp_CampaignResultTP]
	 @DB	  SMALLINT
	,@MinDate DATETIME 
	,@MaxDate DATETIME
	,@WithTreatmentPlan SMALLINT 
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
					, (select sum(amount) from dentalprocedure where db=@DB and patientId in (select patientId from lead where DB=@DB and campaignId=L.CampaignId)AND Treatmentplan = @WithTreatmentPlan   ) TotalAmount
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
												   AND DP.Treatmentplan = @WithTreatmentPlan
			WHERE	L.DB = @DB
AND DP.Treatmentplan = @WithTreatmentPlan			  
AND	CAST(FLOOR(CAST(L.TimeStamp AS FLOAT)) AS DATETIME) BETWEEN @MinDate AND @MaxDate
			  
			GROUP BY 
					L.DB, L.CampaignId, C.Name,C.budget
	) CL
	ORDER BY 
			[Name]
END
go

usp_CampaignResultTP 1148,'2011-1-1','2012-12-31',1
go

usp_CampaignResultTP 1148 ,'2011-1-1','2012-12-31',0
go

usp_CampaignResult 1148,'2011-1-1','2012-12-31'
go
