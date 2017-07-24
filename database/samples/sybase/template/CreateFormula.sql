-- **************************************************************************
-- Creates the Managed Objects database, login, and user
-- **************************************************************************
PRINT ""
PRINT "Managed Objects sample database."
PRINT ""
GO

USE master
SET NOCOUNT on

DECLARE @dbName   VARCHAR(64)
DECLARE @userName VARCHAR(64)
DECLARE @password VARCHAR(64)
DECLARE @msg      VARCHAR(254)
DECLARE @truncLog VARCHAR(254)
DECLARE @dskSize  INT
DECLARE @logSize  INT
DECLARE @numLocks INT

-- **************************************************************************
-- Change these values if necessary
-- **************************************************************************
SELECT @dbName   = "formula"
SELECT @userName = "formula"
SELECT @password = "sesame"
SELECT @truncLog = "true"
SELECT @numLocks = 20000

-- **************************************************************************
-- Changes not necessary beyond this point
-- **************************************************************************
IF EXISTS (select * from master.dbo.sysdatabases where name = @dbName)
BEGIN
   DROP DATABASE @dbName
END

SELECT @msg = 'Creating the ' + @dbName + ' database'
PRINT @msg

CREATE DATABASE @dbName
ON     formulaDisk = 200
LOG ON formulaLog  = 100
WITH override

EXEC sp_dboption @dbName, "trunc log on chkpt", @truncLog

-- **************************************************************************
-- Create user, change db owner, and reset default database for Managed Objects user
-- **************************************************************************
USE @dbName
CHECKPOINT

CHECKPOINT
IF EXISTS (SELECT * FROM master.dbo.sysusers WHERE name = @dbName)
BEGIN
    USE @dbName
    EXEC sp_dropuser @userName
END

IF EXISTS (SELECT * FROM master.dbo.syslogins WHERE name = @dbName)
BEGIN
    USE master
    EXEC sp_droplogin @userName
END

USE master
EXEC sp_addlogin @userName, @password 

USE @dbName
EXEC sp_changedbowner @dbName, true

EXEC sp_defaultdb @userName, @dbName

-- **************************************************************************
-- Reconfiure number of locks parameter
-- **************************************************************************
USE master
EXEC sp_configure "number of locks", @numLocks

exit_script:

GO

