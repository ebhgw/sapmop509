attach to db2 user db2admin;

-----------------------------------------
-- DDL Statements for Managed Objects database --
-----------------------------------------
echo +++;
echo +++ Dropping FORMULA database;
echo +++;
echo +++ Note: Ignore any "database not found" errors.;
echo +++;
DROP DATABASE FORMULA;

echo +++;
echo +++ Creating FORMULA database;
echo +++;
CREATE DATABASE FORMULA 
  ALIAS formula 
  USING CODESET IBM-1252 
  TERRITORY US 
  COLLATE USING 
    SYSTEM USER TABLESPACE MANAGED BY SYSTEM 
      USING ('~InstallDir~/database/samples/db2/user') 
      EXTENTSIZE 32 
    CATALOG TABLESPACE MANAGED BY SYSTEM 
      USING ('~InstallDir~/database/samples/db2/system') 
      EXTENTSIZE 8 
    TEMPORARY TABLESPACE MANAGED BY SYSTEM 
      USING ('~InstallDir~/database/samples/db2/temp') 
      EXTENTSIZE 32 
;

echo +++;
echo +++ Connecting to new FORMULA database;
echo +++;
CONNECT TO FORMULA user db2admin;

------------------------------------
-- DDL Statements for BUFFERPOOLS --
------------------------------------
 
echo +++;
echo +++ Creating FORMULAPOOL BufferPool;
echo +++;
CREATE BUFFERPOOL "FORMULAPOOL"  
   SIZE 250 
   PAGESIZE 32768 
   NOT EXTENDED STORAGE;

CONNECT RESET;

------------------------------------
-- DDL Statements for TABLESPACES --
------------------------------------
echo +++;
echo +++ Re-connecting to FORMULA database to get new buffer pool settings;
echo +++;
CONNECT TO FORMULA user db2admin;

echo +++;
echo +++ Creating BSAWAREHOUSE Tablespace;
echo +++;
CREATE TABLESPACE BSAWAREHOUSE 
  IN NODEGROUP IBMDEFAULTGROUP 
  PAGESIZE 32768 MANAGED BY DATABASE 
  USING (FILE '~InstallDir~/database/samples/db2/BSAwarehouse' 6400)
  EXTENTSIZE 32
  PREFETCHSIZE 16
  BUFFERPOOL FORMULAPOOL;

echo +++;
echo +++ Creating BSAWAREHOUSE_IDX Tablespace;
echo +++;
CREATE TABLESPACE BSAWAREHOUSE_IDX
  IN NODEGROUP IBMDEFAULTGROUP 
  PAGESIZE 32768 MANAGED BY DATABASE 
  USING (FILE '~InstallDir~/database/samples/db2/BSAwarehouse_idx' 6400)
  EXTENTSIZE 32
  PREFETCHSIZE 16
  BUFFERPOOL FORMULAPOOL;


COMMIT WORK;

CONNECT RESET;

TERMINATE;
