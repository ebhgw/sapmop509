--
--  Suggested user name, password, and role.
--
DEFINE userID   = &1
DEFINE passwd   = &2
DEFINE roleName = &3
--
-- Drop the user and role if it already exists
--
WHENEVER SQLERROR CONTINUE;

SET TERMOUT  off
SET FEEDBACK off

DROP USER &userID CASCADE;
DROP ROLE &roleName;

SET TERMOUT  on
SET FEEDBACK on

WHENEVER SQLERROR EXIT SQL.SQLCODE;
--
--  Create new role
--
CREATE ROLE &roleName NOT IDENTIFIED;
--
--  Grant necessary permissions
--
GRANT
    CONNECT,
    CREATE SESSION,
    CREATE TABLE,
TO  &roleName
/
--
--  Defrag tablespaces
--
ALTER TABLESPACE BSAwarehouse        COALESCE;
ALTER TABLESPACE BSAwarehouse_idx    COALESCE;
--
--  Create new user specs
--
CREATE
USER &userID
     IDENTIFIED BY	   &passwd
     DEFAULT TABLESPACE	   BSAwarehouse
     QUOTA UNLIMITED ON    BSAwarehouse
     QUOTA UNLIMITED ON    BSAwarehouse_idx
     PROFILE DEFAULT
/
--
--  Grant new user role permissions
-- 

GRANT &roleName 
TO    &userID
/
--
--  Assign role to user
--
ALTER 
USER    &userID 
DEFAULT ROLE &roleName
/
