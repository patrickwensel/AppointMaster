USE [LPI]

--**  1 Create a TempLead Table with an Autoincrement primary Key ID
drop table TEMPLEAD
go

GO
/****** Object:  Table [dbo].[LEAD]    Script Date: 05/17/2014 19:15:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TEMPLEAD](
	ID int IDENTITY PRIMARY KEY,
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

GO
SET ANSI_PADDING OFF


--**  2 Transfer all existing lead into TEMPLEAD

insert into templead  select * from lead 


--** 3 check table templead has it all with new IDs

select * from templead

--**  4 Drop table Lead as it exist


--drop table lead


--**  5 Create new Table Lead with ID primary key but NOT auto-increment


/****** Object:  Table [dbo].[LEAD]    Script Date: 05/17/2014 19:15:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[LEAD](
	ID int not null,
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
alter table LEAD  add constraint PK_LEAD  primary key (ID)

GO
SET ANSI_PADDING OFF


--** 6 Re-transfer all leads from tempLead to lead 

insert into lead select * from templead 


--** 7 Create a table UNIQUE_ID to keep track of latest id

create table UNIQUE_ID (
	TABLE_NAME	VARCHAR(50) 	not null,
	LAST_ID		INTEGER not null
	)
alter table UNIQUE_ID  add constraint PK_ROOT primary key (TABLE_NAME)


--** 8 Update max id for current leads

declare @maxid int
select @maxid=max(id) from lead
insert into unique_id  (table_name,last_id) values ('LEAD',@maxid) 

--* check maxid ok

select * from unique_id

--** 9 Create new function to easily generate newLeadId


set QUOTED_IDENTIFIER ON
GO

create FUNCTION [dbo].[NewLeadId]()
RETURNS int
AS
BEGIN
	declare @Return int
	select @return = last_id from UNIQUE_ID where TABLE_NAME='LEAD'
	if @return IS NULL 
		set @return=1
	else
		set @return=@return+1
	
	return @return
end

-- * check function

select dbo.NewLeadId()

--** 10 Check everything work fine in lead table

select * from lead order by id desc

select dbo.NewLeadId()


--** 9 Create LeadBean as an exact duplicate of Lead

--drop table LeadBean
--go

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[LeadBean](
	ID int not null,
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
alter table LeadBean  add constraint PK_LeadBean  primary key (ID)

GO
SET ANSI_PADDING OFF





select * from lead order by id desc