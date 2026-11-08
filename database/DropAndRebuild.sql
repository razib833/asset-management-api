:on error exit
:setvar DatabaseName "JamunaBankProcurement"
USE [master];
GO
IF DB_ID(N'$(DatabaseName)') IS NOT NULL
BEGIN
 ALTER DATABASE [$(DatabaseName)] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
 DROP DATABASE [$(DatabaseName)];
END;
GO
:r .\Deploy.sql
