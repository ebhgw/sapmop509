--
-- Set the following variables to the path(s) where tablespace data files should be stored.
-- Note directory must exist prior to running this script and it must be owned by the 
-- 'oracle' user.
--
DEFINE dataDir1 = &1
DEFINE dataDir2 = &2
DEFINE dataDir3 = &3
DEFINE dataDir4 = &4
--
-- Configure the initial and maximum TableSpace size. Depends on available disk space
-- and any Operating system imposed file size limits.
--
DEFINE initialSize = &5
DEFINE maximumSize = &6

WHENEVER SQLERROR EXIT 1;

SET TERMOUT  off
SET FEEDBACK off

WHENEVER SQLERROR CONTINUE;

DROP TABLESPACE BSAwarehouse_idx
    INCLUDING CONTENTS CASCADE CONSTRAINTS;

DROP TABLESPACE BSAwarehouse
    INCLUDING CONTENTS CASCADE CONSTRAINTS;

SET TERMOUT  on
SET FEEDBACK on

WHENEVER SQLERROR EXIT 1;

CREATE 
TABLESPACE BSAwarehouse
DATAFILE   
    '&dataDir3/BSAwarehouse01.dbf'      SIZE &initialSize REUSE,
    '&dataDir4/BSAwarehouse02.dbf'      SIZE &initialSize REUSE
AUTOEXTEND on
MAXSIZE    &maximumSize
EXTENT MANAGEMENT LOCAL UNIFORM SIZE 10M
/

CREATE 
TABLESPACE BSAwarehouse_idx
DATAFILE   
    '&dataDir3/BSAwarehouse_idx01.dbf' SIZE &initialSize REUSE,
    '&dataDir4/BSAwarehouse_idx02.dbf' SIZE &initialSize REUSE
AUTOEXTEND on
MAXSIZE    &maximumSize
EXTENT MANAGEMENT LOCAL UNIFORM SIZE 10M
/
