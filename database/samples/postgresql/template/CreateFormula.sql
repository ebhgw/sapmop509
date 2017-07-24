-- ****************************************************************************
-- This script will create a sample Managed Objects database in an existing Microsoft
-- PostgreSQL distribution. 
-- ****************************************************************************
--
--  ** IMPORTANT **
--  Replace location in create tablespace command
--  ** IMPORTANT **
--
--  Drop current database
DROP DATABASE IF EXISTS formula
;
--  Drop current tablespace
DROP TABLESPACE IF EXISTS BSAWarehouse
;
--  Drop current user
DROP USER IF EXISTS formula
;
--  Create the user
CREATE USER formula PASSWORD 'formula'
;
--  Create the tablespace
CREATE TABLESPACE BSAWarehouse OWNER formula LOCATION '/var/lib/pgsql/data/BSAWarehouse'
;
--  Create the database
CREATE DATABASE formula WITH OWNER formula TABLESPACE BSAWarehouse
;
