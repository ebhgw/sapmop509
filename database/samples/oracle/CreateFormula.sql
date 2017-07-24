--
--  Suggested user name, password, and role.
--
DEFINE userID   = formula
DEFINE passwd   = sesame
DEFINE roleName = formula_role
--
-- Set the following variables to the path(s) where tablespace data files should be stored.
-- The default is '?/oradata' which will evaluate to '$ORACLE_HOME/oradata'
-- Note directory must exist prior to running this script and it must be owned by the 
-- 'oracle' user.
--
DEFINE dataDir1 = ?/oradata
DEFINE dataDir2 = &dataDir1
DEFINE dataDir3 = &dataDir1
DEFINE dataDir4 = &dataDir1
--
-- Configure the initial and maximum TableSpace size. Depends on available disk space
-- and any Operating system imposed file size limits.
--
DEFINE initialSize = 200M
DEFINE maximumSize = 2000M
--
-- Run the scripts
@TableSpaces &dataDir1 &dataDir2 &dataDir3 &dataDir4 &initialSize &maximumSize
@RoleAndUser &userID &passwd &roleName
