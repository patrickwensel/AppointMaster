use lpi
drop PROCEDURE [dbo].[usp_OneCampaignAllResult]
go
CREATE PROCEDURE [dbo].[usp_OneCampaignAllResult]
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
			AND l.original=0
			and campaignId=@CampID
			and d.db=@DB 
			and d.patientId=l.patientId 
			AND	CAST(FLOOR(CAST(L.TimeStamp AS FLOAT)) AS DATETIME) BETWEEN @MinDate AND @MaxDate

go

usp_OneCampaignResult 1148,2,'2010-1-1','2013-1-1',0
go
usp_OneCampaignAllResult 1148,2,'2010-1-1','2013-1-1'
go

