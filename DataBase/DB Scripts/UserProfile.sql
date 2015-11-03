
CREATE FUNCTION UserProfile
(@iProfile int )
RETURNS varchar(30)
AS
BEGIN
declare @Return varchar(30)
select @return = case @iProfile
when 0 then 'LPI-Administrator'								
when 1 then 'Partner-Administrator'
when 2 then 'LPI-User'				
when 3 then 'Partner-User'					
when 4 then 'User'					
else 'Unknown(>4)'
end
return @return
end

go
