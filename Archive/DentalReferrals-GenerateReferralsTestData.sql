DECLARE 
@cnt INT = 0,
@NumberOfReferralsToAdd int,
@PersonToAddReferralTo int,
@PersonToRefer int,
@PersonTreatmentAmount int,
@PersonToReferTreatmentAmount int,
@NextTreatmentID int


--------------------------------------------------------
-----   Add referrals to Persons
--------------------------------------------------------

--Set number of referrals
set @NumberOfReferralsToAdd = 5

--------------------------------------------------------
-----   Do Not Change below line
--------------------------------------------------------



WHILE @cnt < @NumberOfReferralsToAdd
BEGIN




set @PersonToAddReferralTo = 

(

select top 1 id from person where ID not in (
select p2.id
from person p1
inner join person p2 on p1.ID = p2.referedbyid
union
select p1.id
from person p1
inner join person p2 on p1.ID = p2.referedbyid
)
)

set @PersonToRefer = 

(
select top 1  id from person where ID not in (
select p1.id
from person p1
inner join person p2 on p1.referedbyid = p2.id
union
select p2.id
from person p1
inner join person p2 on p1.referedbyid = p2.id

) and id != @PersonToAddReferralTo

)


update person
set referedById = @PersonToRefer
where id = @PersonToAddReferralTo

set @PersonTreatmentAmount = (SELECT CAST(RAND() * 800 AS INT) AS [RandomNumber])

set @PersonToReferTreatmentAmount = (SELECT CAST(RAND() * 800 AS INT) AS [RandomNumber])


set @NextTreatmentID = (select (select max(CAST(ID AS int)) from treatment) + 1)

INSERT INTO [dbo].[TREATMENT]
           ([DataBaseNumber]
           ,[ID]
           ,[CreationDate]
           ,[UpdateDate]
           ,[Version]
           ,[status]
           ,[date]
           ,[amount]
           ,[tooth]
           ,[quadrant]
           ,[surface]
           ,[nbUnits]
           ,[stdTrtId]
           ,[patientId]
           ,[responsibleId]
           ,[officeVisitId]
           ,[providerId])
     VALUES
           (11170
           ,@NextTreatmentID
           ,GETDATE()
           ,GETDATE()
           ,1
           ,0
           ,GETDATE()
           ,@PersonTreatmentAmount
           ,0
           ,0
           ,0
           ,0
           ,11111
           ,@PersonToAddReferralTo
           ,''
           ,''
           ,'')

set @NextTreatmentID = (select (select max(CAST(ID AS int)) from treatment) + 1)

INSERT INTO [dbo].[TREATMENT]
           ([DataBaseNumber]
           ,[ID]
           ,[CreationDate]
           ,[UpdateDate]
           ,[Version]
           ,[status]
           ,[date]
           ,[amount]
           ,[tooth]
           ,[quadrant]
           ,[surface]
           ,[nbUnits]
           ,[stdTrtId]
           ,[patientId]
           ,[responsibleId]
           ,[officeVisitId]
           ,[providerId])
     VALUES
           (11170
           ,@NextTreatmentID
           ,GETDATE()
           ,GETDATE()
           ,1
           ,0
           ,GETDATE()
           ,@PersonToReferTreatmentAmount
           ,0
           ,0
           ,0
           ,0
           ,11111
           ,@PersonToRefer
           ,''
           ,''
           ,'')







   SET @cnt = @cnt + 1;
END;