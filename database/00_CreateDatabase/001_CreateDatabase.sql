:setvar DatabaseName "JamunaBankProcurement"
IF DB_ID(N'$(DatabaseName)') IS NULL
BEGIN
    DECLARE @CreateDatabaseSql NVARCHAR(MAX);
    SET @CreateDatabaseSql = N'CREATE DATABASE ' + QUOTENAME(N'$(DatabaseName)') + N';';
    EXEC sys.sp_executesql @CreateDatabaseSql;
END;
GO
ALTER DATABASE [$(DatabaseName)] SET READ_COMMITTED_SNAPSHOT ON WITH ROLLBACK IMMEDIATE;
GO
USE [$(DatabaseName)];
GO
