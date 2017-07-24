
Database notes for Business Service Warehouse
---------------------------------------------

General
-------
o See the README.txt file in the required database sub-directory for specific
  installation instructions and requirements.

o Suggest creating a 'formula' user with appropriate 'create/drop/alter' object 
  permissions.  This will allow the Managed Objects server to create the required 
  database tables for the Business Service Warehouse.

o A separate default tablespace should be assigned to the 'formula' user instead
  of using the standard 'user' tablespace. This will isolate Managed Objects from any
  existing data sources and provide a container for all performance and SLA data which can
  be managed separately.

o The default tablespace for the 'formula' database user should have enough
  space available to create the necessary tables and maintain at least 1 weeks
  worth of performance/alarm history data (approximately 200 MB).

o For database requirements, see the "Deployment Guide." 
  For more information on creating databases, see the "Server configuration Guide."


DB2
---
o The included drivers do not support DB2. Therefore the
  generic JDBC drivers supplied with the DB2 distribution must be used.
  These drivers can be found in <DB2-root-dir>\java. 
  
  To install the jdbc 2.0 drivers into the Managed Objects installation:

    1) Make sure your DB2 distribution is using the jdbc 2.0 drivers. 
       a) Shutdown your DB2 server (including all services).
       b) Change directory to <DB2-root-dir>\java12.
       c) Run the usejdbc20.bat script.
       d) Restart the DB2 server (including all services).

    2) Configure the DB2 database properties using the Managed Objects Configuration Manager.
       This will require you to enter the path to the DB2 driver archive in
       the properties panel (e.g.<DB2-root-dir>\java\db2java.zip).
       Once the file path is entered, the Configuration Manager will read the archive and
       re-write it to ManagedObjects\classes\ext directory. This step is necessary
       since there are cases where the default IBM archive format is incompatible 
       with the JRE distribution supplied with Managed Objects.
       
    3) Restart the Managed Objects server.
  
o You need to create new buffer pools. Examples are supplied in the
  ManagedObjects\database\samples\db2\CreateFormula.sql script.


