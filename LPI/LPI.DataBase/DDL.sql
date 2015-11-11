use LPI
/***********************[ table PARTNER ]***********************/
DROP TABLE PARTNER 
create table PARTNER (
	ID					INT			not null,
	
	Name				VARCHAR(50) not null,
	Address				VARCHAR(50) not null,
	Address2			VARCHAR(50) not null,
	City				VARCHAR(50) not null,
	State				VARCHAR(2) not null,
	Zip					VARCHAR(20) not null,
	Phone				VARCHAR(15) not null,
	Email				VARCHAR(50) not null,
	MainContact			VARCHAR(50) not null,
)

alter table PARTNER add constraint PK_PARTNER primary key (ID)
create index PARTNER_name on PARTNER (Name)

/***********************[ table APPLIUSER ]***********************/
DROP TABLE APPLIUSER 
create table APPLIUSER (
	DB					INT			not null,
	ID					INT			not null,
	PartnerID			INT			not null,
	
	name				VARCHAR(50) not null,
	username			VARCHAR(50) not null,
	password			VARCHAR(50) not null,
	profile				SMALLINT    not null,
	email				VARCHAR(150) not null,

)

alter table APPLIUSER add constraint PK_APPLIUSER primary key (DB,PartnerID,ID)
create index APPLIUSER_username on APPLIUSER (username)

insert into appliuser (db,id,partnerId,name,username,password,profile,email) values (0,1,0,'Aaron','adrew','lpi2013',0,'aaron.drew@localpatient.com')
insert into appliuser (db,id,partnerId,name,username,password,profile,email) values (0,2,0,'Ralph','rspagnolo','lpi2013',0,'rspagnolo@localpatient.com')
insert into appliuser (db,id,partnerId,name,username,password,profile,email) values (0,3,0,'Christophe','cgissinger','am2013',0,'cgissinger@appointmaster.com')



/***********************[ table ACCOUNT ]***********************/
DROP TABLE ACCOUNT 
create table ACCOUNT (
	DB					INT			not null,
	created				DATETIME	not null,
	
	name				VARCHAR(50) not null,
	mainDoctorName		VARCHAR(50) not null,
	notificationEmail	VARCHAR(60) not null,
	address				VARCHAR(100)not null,
	addressLine2		VARCHAR(50)	not null,
	city				VARCHAR(50)	not null,
	state				VARCHAR(2)	not null,
	zipCode				VARCHAR(11)	not null,
	mainPhone			VARCHAR(15)	not null,
	fax					VARCHAR(15)	not null,
	software			SMALLINT	not	null,
	status				SMALLINT	not null,
	mainContact			VARCHAR(50)	not null,
	webSite				VARCHAR(255)not null,
	userId				VARCHAR(50)	not null,
	password			VARCHAR(50)	not null
	DataLinkUserId		VARCHAR(50)	not null,
	DataLinkPassword	VARCHAR(50)	not null,
	AMServer			VARCHAR(255)	not null
)

alter table ACCOUNT add constraint PK_ACCOUNT primary key (DB)
create index account_name on ACCOUNT (name)

/***********************[ table CAMPAIGN ]***********************/
DROP TABLE CAMPAIGN 
create table CAMPAIGN (
	DB					SMALLINT	not null,
	ID					INT			not	null,
	name				VARCHAR(255) not null,
	phoneNumber			VARCHAR(15)	not null,
	budget				DECIMAL(10,2) not null,
)

alter table CAMPAIGN add constraint PK_CAMPAIGN primary key (DB,ID)
create index CAMPAIGN_NAME on CAMPAIGN (DB,name)





/***********************[ table LEAD ]***********************/
DROP TABLE LEAD 
create table LEAD (
	DB					SMALLINT	not null,
	campaignID			INT			not	null,
	inComingNumber		VARCHAR(15)	not null,
	timeStamp			DateTime	not null,
	durationMinutes		smallint	not null,
	newPatient			smallint	not null,
	birthday            DATETIME	not null,
	
	patientId			VARCHAR(12)	,
	fileURL				VARCHAR(255)	not	null 
)

create index LEAD_CAMPID on LEAD (DB,campaignID)



/***********************[ table DELETEDLEAD  ]***********************/
DROP TABLE DELETEDLEAD 
create table DELETEDLEAD (
	DB					SMALLINT	not null,
	campaignID			INT			not	null,
	PrimaryPhone		VARCHAR(15)	not null,
	timeStamp			DateTime	not null
)

create index DELETEDLEAD_CAMPID on DELETEDLEAD (DB,campaignID)



/***********************[ table PATIENT ]***********************/
DROP TABLE PATIENT
create table PATIENT (
	DB					INT			not null,
	AMID				VARCHAR(10) not null,
	name				VARCHAR(50) not null,
	firstName			VARCHAR(50) not null,
	SSN					VARCHAR(10) not null,
	male				SMALLINT	not	null,
	birthDate			DATETIME	not null,
	address				VARCHAR(100)not null,
	addressLine2		VARCHAR(50)	not null,
	city				VARCHAR(50)	not null,
	state				VARCHAR(2)	not null,
	zipCode				VARCHAR(11)	not null,
	homePhone			VARCHAR(15)	not null,
	cellPhone			VARCHAR(15)	not null,
	workPhone			VARCHAR(15)	not null,
	email				VARCHAR(60) not null,
)

alter table PATIENT add constraint PK_PATIENT primary key (DB, AMID)
create index pers_db_and_name on PATIENT (DB, name)


/***********************[ table APPOINTMENT ]***********************/
DROP TABLE APPOINTMENT
create table APPOINTMENT (
	DB					INT			not null,
	AMID				VARCHAR(14) not null,
	dateTime			DATETIME	not null,
	patientId			VARCHAR(10) not null,
)

alter table APPOINTMENT add constraint PK_APPOINTMENT primary key (DB, AMID)
create index Appt_Patient on APPOINTMENT (DB, patientId)
create index Appt_dateTime on APPOINTMENT (DB, dateTime)


/***********************[ table DENTALPROCEDURE ]***********************/
DROP TABLE DENTALPROCEDURE
create table DENTALPROCEDURE (
	DB					INT			not null,
	patientId			VARCHAR(10) not null,
	CODE				VARCHAR(14) not null,
	apptId				VARCHAR(14) not null,
	dateTime			DATETIME	not null,
	amount				DECIMAL(10,2) not null,
)
create index DP_Patient on DENTALPROCEDURE (DB, patientId,Code)
create index DP_appt on DENTALPROCEDURE (DB, apptId, Code)



/***********************[ table SYSOPTION ]***********************/
DROP TABLE SYSOPTION
create table SYSOPTION (
	DB					INT			not null,
	name				VARCHAR(50) not null,
	value				VARCHAR(255) not null,
)

alter table SYSOPTION add constraint PK_SYSOPTION primary key (DB, name)
create index pers_db_and_name on SYSOPTION (DB, name)



/**********************************************/
/**********************************************/
/**********************************************/

BULK INSERT ACCOUNT FROM 'C:\CSHARP\LPI\DataBase\DataSet\account.txt' WITH (FIELDTERMINATOR = '|')
BULK INSERT CAMPAIGN FROM 'C:\CSHARP\LPI\DataBase\DataSet\campaigns.txt' WITH (FIELDTERMINATOR = '|')
BULK INSERT LEAD FROM 'C:\CSHARP\LPI\DataBase\DataSet\leads.csv' WITH (FIELDTERMINATOR = ',')
BULK INSERT PATIENT FROM 'C:\CSHARP\LPI\DataBase\DataSet\patients.csv' WITH (FIELDTERMINATOR = ',')
BULK INSERT APPOINTMENT FROM 'C:\CSHARP\LPI\DataBase\DataSet\APPOINTMENTs.csv' WITH (FIELDTERMINATOR = ',')
BULK INSERT DENTALPROCEDURE  FROM 'C:\CSHARP\LPI\DataBase\DataSet\PROCEDUREs.csv' WITH (FIELDTERMINATOR = ',')

select * from account
select * from campaign
select * from lead
select * from patient
select * from APPOINTMENT 
select * from DENTALPROCEDURE 



/**********************************************/
/**********************************************/


use lpi

ALTER TABLE account ALTER COLUMN userID varchar(50) not null
ALTER TABLE account ALTER COLUMN password varchar(50) not null
ALTER TABLE account ALTER COLUMN state varchar(20) not null


ALTER TABLE account add  	DataLinkUserId				VARCHAR(50)	not null default ''
ALTER TABLE account add  	DataLinkPassword			VARCHAR(50)	not null default ''
ALTER TABLE account add  	AMServer					VARCHAR(255)	not null default ''
