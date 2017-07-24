                                    DB2 Suggested Setup
                                    -------------------

General
-------
To setup the Business Servicer Warehouse or the Formula Event Data Store, the
DB2 database should be configured to handle a substantial amount of data.  The 
scripts in this directory are "suggested" default user and tablespace settings for small Managed Objects 
configurations. To build a DB2 database for a large configuration, consult the DB2 documentation.

The defaults in the script should be reviewed by the enterprise Database Administrator to ensure
they fit into your existing corporate setting. 

For sizing information, consult the "Server Configuration Guide" located in the ManagedObjects\html\docs directory.

Setup
-----
The main script used to create the sample Business Service Warehouse is 'CreateFormula.sql.'
This script must be executed by a user with admin privileges since it creates a 
new database, a bufferpool compatible with the Managed Objects schema, and a sample tablespace 
for the Business Service Warehouse. 

The default directory for the database and tablespaces is ManagedObjects/databases/samples/db2. 
This location should be changed to a more suitable directory.

Configuration
-------
1. Edit CreateFormula.sql and change the directories and/or size for each data file. Search 
   for the "FILE" string to review each occurance.


2. Run the CreateFormula.sql script. This script requires admin privileges.

      db2cmd -c db2 -t -z create.log -f CreateFormula.sql


3. After the DB2 database instance is created, launch the Managed Objects console, 
   and browse to the "Database Definition" element in the hierarchy (i.e., Enterprise > Administration > Database Definitions).

        then, right-click on "Database Definitions" and select "Create
        Database Definition."

        When the "Create Database Definition" window appears, specify all
        required parameters, then select "DB2" from the "Type" drop
        down list.

        In the "Properties" panel, specify the following properties:

        Host Name           - The name of the host where DB2 is installed
        Listener Port       - The port that the DB2 database is listening on.
                              The default value for this property is "50000." If
                              you setup DB2 to listen on a different port, 
                              specify the new port in this property. See the 
                              DB2 documentation for instructions on how to
                              change the default listener port.
        Database            - This is the database name specified in step 1 above.                    
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

4. Enable the new Database definition by either selecting the "Enable"
   check-box, or right-click the new database definition and select "Enable
   Database Definition."

5. To initialize the database schema for the new DB2 installation,
   right-click the new Database Definition, then select "Initialize Database
   Schema."  This step is generally not necessary since the Managed Objects server 
   creates all appropriate schemas when required. However, if you create a database
   definition with a single schema "Formula Event Data Store" you must initialize the 
   schema after you create the definition.

6. See the "Server Configuration Guide" for more information. 
