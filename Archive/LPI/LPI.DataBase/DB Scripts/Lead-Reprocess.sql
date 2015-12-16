use lpi


--*** 0)  Detect wrong duplicates

select * from lead where email like '%@none.com' and original=4
select * from lead where lastname='none' or firstname='none'  and original=2


--*** 1)  Make sure the lead source is deleted

drop table lead_source

--*** 2)  Create table lead_source


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[lead_source](
	[ID] [int] NOT NULL,
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
	[original] [tinyint] NOT NULL DEFAULT ((0)),
 CONSTRAINT [PK_LEAD_SOURCE] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF


--*** 3)  Copy all leads into lead_source

insert into lead_source select * from lead 

--*** 4)  Update all lead to 0

update lead_source set original=0

--*** 5)  Remove all leads

delete from lead

--*** 6)  Call ReprocessLeads



--*** 7)  Check duplicate

select * from lead where email like '%@none.com' and original=4
select * from lead where (lastname='none' or firstname='none')  and original=2

