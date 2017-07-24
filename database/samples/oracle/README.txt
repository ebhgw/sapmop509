                                    Oracle Suggested Setup
                                    ----------------------

General
-------
To setup a database for the Business Service Warehouse or the Event Data Store, the
Oracle database should be configured to handle a substantial amount of data.  The 
CreateFormula.sql script in this directory creates "suggested" default user and tablespace 
settings for small Managed Objects configurations. To build a Oracle database for a large
configuration, consult the Oracle documentation.

The defaults in the script should be reviewed by the enterprise Database Administrator to ensure
they fit into your existing corporate setting. 

For sizing information, consult the "Server Configuration Guide" located in the ManagedObjects\html\docs directory.

Database Requirements
---------------------
The following properties must be set in the init<SID>.ora or init.ora in order
for the Managed Objects database connection pool to operate correctly.

      processes = 200  
      open_cursors = 500 

If Managed Objects is configured to use more than 10 database connections, the 'processes'
and 'open_cursors' properties should be adjusted as follows:

     processes:    number-of-connections * 20
     open_cursors: number-of-connections * 50


Setup
-----
The main script used to create the sample Business Service Warehouse ris 
'CreateFormula.sql.' This script must be executed by the Oracle system user since it creates
a default role, user, and a set of sample tablespace data files for the Business Service Warehouse. 
This script assumes you already have a database instance setup for the Business Service Warehouse. To
setup a special instance, use Oracle's "Database Configuration
Assistant."

By default, the CreateFormula.sql script creates the Managed Objects tablespace data files 
under $ORACLE_HOME/oradata. You can change the default location by editing CreateFormula.sql 
and changing the definition of the dataDir<n> variables. Note also, the specified directories 
must exist and be owned by the 'Oracle' user prior to executing the CreateFormula.sql script.

In addition, you may want to configure the initial and maximum tablespace size variables
defined in CreateFormula.sql. These values depend on available disk space and any 
Operating system imposed file size limits.

Configuration
-------
1. Edit CreateFormula.sql to change the 'userID,' 'passwd,' and 'roleName' variables
   if the defaults are not acceptable. The current defaults are:

     DEFINE userID   = formula
     DEFINE passwd   = sesame
     DEFINE roleName = formula_role


2. Edit CreateFormula.sql and modify the dataDir<n> variables. If the tablespaces are to
   be spread across multiple filesystems, set each variable individually. Otherwise, 
   leave the dataDir[2-4] variables as they are.

     DEFINE dataDir1 = /fs01/oradata/FORMULA
     DEFINE dataDir2 = /fs02/oradata/FORMULA
     DEFINE dataDir3 = &dataDir1
     DEFINE dataDir4 = &dataDir2


3. Configure the initial and maximum tablespace sizes. The defaults specify an initial
   tablespace size of 200M. Since the 'AUTOEXTEND' feature is enabled on all Managed Objects
   tablespaces, the maximum size variable should also be set. The default setting is currently
   set to 2GB (i.e., OS imposed limit on Sun Solaris 2.7). Change these values if required.

      DEFINE initialSize = 200M
      DEFINE maximumSize = 2000M


4. Run the CreateFormula.sql script. This script requires oracle system privileges.

      sqlplus system
      Password: your-system-password
      
      SQL> @CreateFormula.sql
      SQL> exit;


5. After the SQL Server database instance is created, launch the Managed Objects console, 
   and browse to the "Database Definition" element in the hierarchy (i.e., Enterprise > Administration > Database Definitions).

        then, right-click on "Database Definitions" and select "Create
        Database Definition."

        When the "Create Database Definition" window appears, specify all
        required parameters, then select "Oracle" from the "Type" drop
        down list.

        In the "Properties" panel, specify the following properties:

        Host Name           - The name of the host where Oracle is installed.
        Listener Port       - The port that the Oracle database is listening on.
                              The default value for this property is "1521." If
                              you setup Oracle to listen on a different port, 
                              specify the new port in this property. See the 
                              Oracle documentation for instructions on how to
                              change the default listener port.
        Server ID (SID)     - The name of the database.
        User Name           - This is the "User Name" property specified in 
                              step 1 above.
        Password            - This is the "Password" property specified in 
                              step 1 above.
        Initial Connections - Number of connections to start when the database
                              connection pool is started.
        Maximum Connections - Maximum number of connections that can be
                              allocated in the connection pool.

        After entering the necessary properties, click the "Test" button to test the 
        database connection. Upon success, click the "Create" button to complete the creation 
        of the Database definition.

6. Enable the new Database definition by either selecting the "Enable"
   check-box, or right-click the new database definition and select "Enable
   Database Definition."

7. To initialize the database schema for the new Oracle installation,
   right-click the new Database Definition, then select "Initialize Database
   Schema."  This step is generally not necessary since the Managed Objects server 
   creates all appropriate schemas when required. However, if you create a database
   definition with a single schema "Event Data Store" you must initialize the 
   schema after you create the definition.

8. See the "Server Configuration Guide" for more information. 
