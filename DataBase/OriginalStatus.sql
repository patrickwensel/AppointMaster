drop FUNCTION  [dbo].[OriginalStatus]
go

CREATE FUNCTION OriginalStatus
(@iStatus int )
RETURNS varchar(30)
AS
BEGIN
declare @Return varchar(30)
select @return = case @iStatus
when 0 then 'Original'								
when 1 then 'Invalid'								
when 2 then 'Name and Firstname exist'								
when 3 then 'InComingNumber not unique'								
when 4 then 'Email not unique'								
when 5 then 'Duplicate matching'								
when 6 then 'Patient Id already exists'								
else 'Unknown(>40)'
end
return @return
end


