USE [lpi]
GO
/****** Object:  UserDefinedFunction [dbo].[OriginalStatus]    Script Date: 04/17/2014 23:33:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER FUNCTION [dbo].[OriginalStatus]
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
when 7 then 'Patient is not new'								
else 'Unknown(>40)'
end
return @return
end


