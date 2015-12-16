use lpi
drop PROCEDURE [dbo].[usp_RemoveDuplicateLeadsForDb]

use lpi
CREATE PROCEDURE usp_RemoveDuplicateLeadsForDb
	 @DB			SMALLINT

AS
BEGIN
	SET NOCOUNT ON;

	IF object_id('holdkey') is not null
		drop table holdkey
	IF object_id('holddups') is not null
		drop table holddups


	--1. Identify duplicates
	--select DB,campaignId,inComingNumber,timestamp,count(*) from lead 
	--group by DB,campaignId,inComingNumber,timestamp having count(*)>1

	--2.Select the duplicate key values into a holding table. 
	SELECT DB,campaignId,inComingNumber,timestamp,col5=count(*)
	INTO holdkey
	FROM lead 
	where DB=@DB
	group by DB,campaignId,inComingNumber,timestamp having count(*)>1

	-- 3.Select the duplicate rows into a holding table, eliminating duplicates in the process. For example:
	SELECT DISTINCT lead.*
	INTO holddups
	FROM lead, holdkey
	where lead.DB=@DB
	and	 holdkey.DB=@DB
	and lead.DB=holdkey.db
	and lead.campaignId = holdkey.campaignId
	AND lead.inComingNumber = holdkey.inComingNumber
	AND lead.timestamp  = holdkey.timestamp 
									
	--4.Delete the duplicate rows from the original table
	DELETE lead
	FROM lead, holdkey
	where lead.DB=@DB
	and	 holdkey.DB=@DB
	and lead.DB=holdkey.db
	and lead.campaignId = holdkey.campaignId
	AND lead.inComingNumber = holdkey.inComingNumber
	AND lead.timestamp  = holdkey.timestamp 
					
	--5.Put the unique rows back in the original table. For example:

	INSERT lead SELECT * FROM holddups

	IF object_id('holdkey') is not null
		drop table holdkey
	IF object_id('holddups') is not null
		drop table holddups


END

go
			 
use lpi
	usp_RemoveDuplicateLeadsForDb 1135