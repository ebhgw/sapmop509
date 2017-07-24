/*
-- ****************************************************************************
-- This script will create a sample Managed Objects database in an existing Microsoft
-- SQL Server distribution. 
-- ****************************************************************************
*/
IF EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE name = N'formula')
	DROP DATABASE [formula]
GO

DECLARE @command      varchar(2048)
DECLARE @dataDir      varchar(1024)
DECLARE @dataFile     varchar(1024)
DECLARE @logFile      varchar(1024)
DECLARE @initialSize  varchar(128)
DECLARE @maximumSize  varchar(128)

/*
-- ****************************************************************************
-- The following variables should be modified by the Database Administrator.
-- ****************************************************************************
*/
SET @dataDir     = N'~InstallDir~\database\samples\mssql'
SET @initialSize = N'20'
SET @maximumSize = N'2000'

/*
-- ****************************************************************************
-- Not necessary to modify anything beyond this point
-- ****************************************************************************
*/
SET @dataFile    = @dataDir + N'\formulaData.MDF'
SET @logFile     = @dataDir + N'\formulaLog.LDF'

EXECUTE (
     N'CREATE DATABASE [formula]  ON ('
    + ' NAME       = ''formula_Data'''   + ','
    + ' FILENAME   = ''' + @dataFile     + ''','
    + ' SIZE       = '   + @initialSize  + ',' 
    + ' MAXSIZE    = '   + @maximumSize  + ','
    + ' FILEGROWTH = 5'
    + ') '
    + 'LOG ON ('
    + ' NAME       = ''formula_Log'','
    + ' FILENAME   = ''' + @logFile + ''','
    + ' SIZE       = 5,'
    + ' FILEGROWTH = 10%'
    + ' )'
)
GO

use [formula]
GO

/*
-- ****************************************************************************
-- The following variables should be modified by the Database Administrator.
-- ****************************************************************************
*/
DECLARE @username     varchar(128)
DECLARE @password     varchar(128)

SET @username = N'formula'
SET @password = N'sesame'

/*
-- ****************************************************************************
-- Create the user and grant permissions
-- ****************************************************************************
*/
IF NOT EXISTS (SELECT * FROM master.dbo.syslogins WHERE loginname = @username)
BEGIN
   DECLARE @logindb   nvarchar(132)
   DECLARE @loginlang nvarchar(132) 

   SELECT @logindb = @username, @loginlang = N'us_english'

   IF @logindb IS NULL OR NOT EXISTS (SELECT * from master.dbo.sysdatabases where name = @logindb)
      SELECT @logindb = N'master'

   IF  @loginlang IS NULL 
   OR (NOT EXISTS (SELECT * from master.dbo.syslanguages where name = @loginlang) AND @loginlang <> N'us_english')
      SELECT @loginlang = @@language

   EXEC sp_addlogin @username, @password, @logindb, @loginlang
END

IF NOT EXISTS (SELECT * from master.dbo.syslogins where loginname = @username)
BEGIN
   EXEC sp_grantlogin      @username
   EXEC sp_defaultdb       @username, N'formula'
   EXEC sp_defaultlanguage @username, N'us_english'
END

EXEC sp_addsrvrolemember @username, sysadmin
EXEC sp_addsrvrolemember @username, securityadmin
EXEC sp_addsrvrolemember @username, serveradmin
EXEC sp_addsrvrolemember @username, setupadmin
EXEC sp_addsrvrolemember @username, processadmin
EXEC sp_addsrvrolemember @username, diskadmin
EXEC sp_addsrvrolemember @username, dbcreator

IF NOT EXISTS (SELECT * FROM dbo.sysusers WHERE name = @username AND uid < 16382)
BEGIN
   EXEC sp_grantdbaccess @username, N'formula'
END

PRINT N'CreateFormula complete.'
GO

use [master]
GO
