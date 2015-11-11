use Lpi


drop PROCEDURE [dbo].[usp_OneCampaignResult]
go
CREATE PROCEDURE [dbo].[usp_OneCampaignResult]
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
			AND l.original=0 
			and campaignId=@CampID
			and d.db=@DB 
			and d.patientId=l.patientId 
			and d.treatmentplan=@WithTreatmentPlan 
			AND	CAST(FLOOR(CAST(L.TimeStamp AS FLOAT)) AS DATETIME) BETWEEN @MinDate AND @MaxDate




