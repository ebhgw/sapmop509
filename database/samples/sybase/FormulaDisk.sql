-- **************************************************************************
-- Creates the formula disk devices
-- **************************************************************************
DECLARE @dbName        VARCHAR(64)
DECLARE @dskDir        VARCHAR(254)
DECLARE @dskDev        VARCHAR(254)
DECLARE @dskSize       INT
DECLARE @logDev        VARCHAR(254)
DECLARE @logSize       INT
DECLARE @tmpDevName    VARCHAR(254)
DECLARE @tmpDev        VARCHAR(254)
DECLARE @tmpSize       INT
DECLARE @devno         INT
DECLARE @dropped       INT
DECLARE @curTempSize   INT
DECLARE @minTempSize   INT
DECLARE @cnt           INT

-- **************************************************************************
-- Change the settings below
-- **************************************************************************
SELECT @dskDir = "E:\application\NOC00\NovellOperationsCenter\NOC/database/samples/sybase/"
SELECT @dbName = "formula"

SELECT @dskDev  = @dskDir + "formulaLog.dat"
SELECT @dskSize = 102400                              /* approx 200M */

SELECT @logDev  = @dskDir + "formulaDsk.dat"
SELECT @logSize = 51200                               /* approx 100M */

SELECT @tmpDev  = @dskDir + "formulaTmp.dat"
SELECT @tmpSize = 51200                               /* approx 100M */

SELECT @minTempSize = 25600                           /* approx 50M */

-- **************************************************************************
-- Get current tempdb size
-- **************************************************************************
SELECT @curTempSize = SUM(size)*(@@pagesize/1024)
FROM   master.dbo.sysusages
WHERE  dbid = db_id('tempdb')

-- **************************************************************************
-- Changes not necessary beyond this point
-- **************************************************************************
IF EXISTS (select * from master.dbo.sysdatabases where name = @dbName)
BEGIN
   DROP DATABASE @dbName
END

SELECT @dropped = 0
IF EXISTS (select * from master.dbo.sysdevices where name = "formulaDisk")
BEGIN
   SELECT @dropped = 1
   EXEC sp_dropdevice formulaDisk
END

IF EXISTS (SELECT * FROM master.dbo.sysdevices WHERE name = "formulaLog")
BEGIN
   SELECT @dropped = 1
   EXEC sp_dropdevice formulaLog 
end


IF ( @dropped = 1 )
BEGIN
   PRINT "**************************************************************"
   PRINT "* The formulaDisk and/or formulaLog devices were dropped.    *"
   PRINT "* You need to remove the phyical files with the appropriate  *"
   PRINT "* operating system command (ie. rm/del), then rerun this     *"
   PRINT "* script again                                               *"
   PRINT "**************************************************************"
   GOTO exit_script
END

USE master

SELECT @devno = max(low/16777216)+1 from sysdevices

PRINT "Creating formulaDisk"
DISK INIT
    NAME     = "formulaDisk"
  , PHYSNAME = @dskDev
  , VDEVNO   = @devno
  , SIZE     = @dskSize

PRINT "done"

SELECT @devno = max(low/16777216)+1 from sysdevices

PRINT "Creating formulaLog"
DISK INIT
    NAME     = "formulaLog"
  , PHYSNAME = @logDev
  , VDEVNO   = @devno
  , SIZE     = @logSize


-- **************************************************************************
-- Create new device for tempdb if not enough space
-- **************************************************************************
IF (@curTempSize < @minTempSize)
BEGIN

   SELECT @tmpDevName = "formulaTempDB"
   SELECT @cnt = 1

   WHILE EXISTS (SELECT * FROM master.dbo.sysdevices WHERE name = @tmpDevName)
   BEGIN
      SELECT @tmpDevName = "formulaTempDB" + convert(varchar,@cnt)
      IF (@cnt > 10)
      BEGIN
         PRINT "Can't create new device for tempdb database"
         GOTO exit_script
      END
   END
   PRINT 'Creating %1!', @tmpDevName

   SELECT @devno = max(low/16777216)+1 from sysdevices

   DISK INIT
        NAME     = @tmpDevName
      , PHYSNAME = @tmpDev
      , VDEVNO   = @devno
      , SIZE     = @tmpSize
      
   PRINT "done"
   -- 
   -- Reconfigure tempdb for formula database
   --
   ALTER  DATABASE tempdb 
   ON     formulaTempDB=40
   LOG ON formulaLog=10
   WITH OVERRIDE
END

exit_script:

GO

