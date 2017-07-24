                                    Sybase Suggested Setup
                                    ----------------------

General
-------
To setup a database for the Business Service Warehouse or the Event Data Store, the
Sybase database should be configured to handle a substantial amount of data.  The 
CreateFormula.sql script in this directory creates "suggested" default database and user settings 
for small Managed Objects configurations. To build a Sybase database for a large
configuration, consult the Sybase documentation.

The defaults in the script should be reviewed by the enterprise Database Administrator to ensure
they fit into your existing corporate setting. 

The scripts in the sample Sybase directory create a moderate database
consisting of a 200MB Managed Objects log device and a 100M data device. Also, since the
Business Service Warehouse uses a substantial amount of temp space for sorting during
large queries, a new 50MB device for the tempdb database is created.

For sizing information, consult the "Server Configuration Guide" located in the ManagedObjects\html\docs directory.

To attempt to deal with deadlock issues in a Sybase environment, the database
tables created by the data warehouse use the "row-level" locking feature. This 
reduces the number of lock collisions in a fairly busy system. In order to accommodate
the table setting, the number of locks that can be held open by the system
can be re-configured by the CreateFormula.sql script. Note that changing this
parameter requires the Sybase server to be stopped and restarted.


Setup
-----
The main script used to create the sample Business Service Warehouse database devices is 'FormulaDisk.sql.'
This script must be executed by a user with dba privileges since it creates the Managed Objects
disk devices.

Before running FormulaDisk.sql, you must change the location and size of the device data
files to point to a desired directory. Note also, the specified directory must exist prior 
to executing the FormulaDisk.sql script.

After running the FormulaDisk.sql script, you must then edit and run the 'CreateFormula.sql' 
script to create the Managed Objects database, login, and user. 

Configuration
-------
1. Edit FormulaDisk.sql and change the location and size of the device data files
   for both the 'formulaDisk' and 'formulaLog' devices.

      e.g.,
         SELECT @dbName = "formula"    /* change this value if you want to set
                                        * the database name to something else
                                        */

         SELECT @dskDev  = "/opt/sybase/dbs/formulaLog.dat"
         SELECT @dskSize = 1024000                             /* approx 200M */

         SELECT @logDev  = "/opt/sybase/dbs/formulaDsk.dat"
         SELECT @logSize = 51200                               /* approx 100M */


2. Edit CreateFormula.sql and change the login, user, and password settings if the
   defaults are not desirable.

      e.g., 
         SELECT @dbName   = "formula"   /* Must correspond to value of @dbName
                                         * set in FormulaDisk.sql
                                         */

         SELECT @userName = "formula"
         SELECT @password = "sesame"


3. Run the FormulaDisk.sql script to create the devices

     isql -Usa -Psa-password -Sserver-name -i FormulaDisk.sql

     where:
        sa-password   = The Sybase system administrator password
        server-name   = The Sybase server name


4. Now run the 'CreateFormula.sql' script to create the Managed Objects database, login, and
   user.

     isql -Usa -Psa-password -Sserver-name -i CreateFormula.sql

     where:
        sa-password   = The Sybase system administrator password
        server-name   = The Sybase server name


5. After the Sybase database instance is created, launch the Managed Objects console, 
   and browse to the "Database Definition" element in the hierarchy (i.e., Enterprise > Administration > Database Definitions).


        then, right-click on "Database Definitions" and select "Create
        Database Definition."

        When the "Create Database Definition" window appears, specify all
        required parameters, then select "Sybase" from the "Type" drop
        down list.

        In the "Properties" panel, specify the following properties:

        Host Name           - The name of the host where Sybase is installed
        Listener Port       - The port that the Sybase database is listening on.
                              The default value for this property is "4100." If
                              you setup Sybase to listen on a different port, 
                              specify the new port in this property. See the 
                              Sybase documentation for instructions on how to
                              change the default listener port.
        Database            - This is the database name specified in steps 1 & 
                              2 above.  
        User Name           - This is the "User Name" property specified in 
                              step 2 above.
        Password            - This is the "Password" property specified in 
                              step 2 above.
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

7. To initialize the database schema for the new Sybase installation,
   right-click the new Database Definition, then select "Initialize Database
   Schema."  This step is generally not necessary since the Managed Objects server 
   creates all appropriate schemas when required. However, if you create a database
   definition with a single schema "Event Data Store" you must initialize the 
   schema after you create the definition. 

8. See the "Server Configuration Guide" for more information. 
