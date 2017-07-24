                                    Microsoft SQL Server Suggested Setup
                                    ------------------------------------

General
-------
To set up a database for use with the Business Service Warehouse or the Event Data Store, 
the Microsoft SQL Server database should be configured to handle a substantial amount 
of data. The CreateFormula.sql script in the \template directory creates a suggested default user 
and database configuration for small Managed Objects configurations. To build a Microsoft 
SQL Server database for a large configuration, consult the Microsoft SQL Server documentation.

During SQL Server installation, select "Mixed Mode" when asked to select the security mode
(authentication). Mixed Mode enables users to connect using Windows Authentication or SQL Server 
Authentication. Users who connect through a Microsoft Windows user account can use trusted 
connections (connections validated by Windows) in either Windows Authentication Mode 
or Mixed Mode. SQL Server Authentication is provided for backward compatibility. 

The defaults in the CreateFormula.sql script should be reviewed by the enterprise Database 
Administrator to ensure they are suitable for your existing corporate setting. 

For sizing information, consult the "Server Configuration Guide" located the ManagedObjects\html\docs directory.

Setup
-----
The main script used to create the sample Business Service Warehouse is 'CreateFormula.sql.'
This script must be executed by a user with dba privileges since it creates a default
database and user.

Before running the the sample script, you should edit the CreateFormula.sql file and change
the definition of the dataDir variable to point to a directory outside the Managed Objects distribution
tree. By default, the Managed Objects database is created under ManagedObjects\database\samples\mssql.
Note also, the specified directory must exist prior to executing the CreateFormula.sql script.

In addition, you may want to configure the initial and maximum database size variables
defined in CreateFormula.sql. These values depend on available disk space and any 
Operating system imposed file size limits.

Configuration
-------
1. Edit CreateFormula.sql and modify the dataDir variable to the desired location. 

     SET @dataDir = 'd:\ManagedObjects\database\mssql'


2. Configure the initial and maximum tablespace sizes. The defaults specify an initial
   database size of 200 MB and a maximum size of 2000.  Change these values if required.

     SET @initialSize = 200
     SET @maximumSize = 2000


3. The default database user and password created by CreateFormula.sql are 'formula' 
   and 'sesame' respectively. If you prefer to use different values, change
   them by resetting the following variables:

     SET @username = N'formula'
     SET @password = N'sesame'

4. Run the CreateFormula.sql script. Note: this script requires dba privileges.

    i. If using isql.exe, use the following command line:

          isql -n -U sa -P your-sa-password -S your-server-name -i CreateFormula.sql
      

   ii. If using Microsoft's "SQL Query Analyzer"

       a) Launch the "SQL Server Enterprise Manager" from the "Microsoft SQL Server"
          program menu.

       b) Click "Tools > SQL Query Analyzer." 

       c) Click "File > Open", then browse to the location of the CreateFormula.sql 
          script and select it.

       d) Click "Tools > Execute."

       e) The new Managed Objects database is created.


5. After the SQL Server database instance is created, launch the Managed Objects console, 
   and browse to the "Database Definition" element in the hierarchy (i.e., Enterprise > Administration > Database Definitions). 

        Right-click "Database Definitions" and select "Create
        Database Definition."

        On the "Create Database Definition" window, specify all
        required parameters, then select "Microsoft SQL Server" from the "Type" drop
        down list.

        In the "Properties" panel, specify the following properties:

        Host Name           - The name of the host where SQL Server is installed.
        Listener Port       - The port on which the SQL Server database is listening.
                              The default value for this property is "1433." If
                              you setup SQL Server to listen on a different port, 
                              specify the new port in this property. See the Microsoft
                              SQL Server documentation for instructions on how to
                              change the default listener port.
        User Name           - This is the "User Name" property specified in 
                              step 3 above.
        Password            - This is the "Password" property specified in 
                              step 3 above.
        Initial Connections - Number of connections to start when the database
                              connection pool is started.
        Maximum Connections - Maximum number of connections that can be
                              allocated in the connection pool.

        After entering the necessary properties, click the "Test" button to test the 
        database connection. Upon success, click the "Create" button to create the 
	Database definition.

6. Enable the new Database definition by either selecting the "Enable"
   checkbox, or right-clicking the new database definition and selecting "Enable
   Database Definition."

7. To initialize the database schema for the new SQL Server installation,
   right-click the new Database Definition, then select "Initialize Database
   Schema."  This step is generally not necessary since the Managed Objects server 
   creates all appropriate schemas when required. However, if you create a database
   definition with a single schema, "Event Data Store," you must initialize the 
   schema after you create the definition.

8. See the "Server Configuration Guide" for more information. 
