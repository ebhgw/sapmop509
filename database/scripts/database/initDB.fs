//
// Copyright (c) 2014 NetIQ Corporation.  All Rights Reserved.
//
// THIS WORK IS SUBJECT TO U.S. AND INTERNATIONAL COPYRIGHT LAWS AND TREATIES.  IT MAY NOT BE USED, COPIED, 
// DISTRIBUTED, DISCLOSED, ADAPTED, PERFORMED, DISPLAYED, COLLECTED, COMPILED, OR LINKED WITHOUT NETIQ'S
// PRIOR WRITTEN CONSENT. USE OR EXPLOITATION OF THIS WORK WITHOUT AUTHORIZATION COULD SUBJECT THE 
// PERPETRATOR TO CRIMINAL AND CIVIL LIABILITY.
//
// NETIQ PROVIDES THE WORK "AS IS," WITHOUT ANY EXPRESS OR IMPLIED WARRANTY, INCLUDING WITHOUT THE
// IMPLIED WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE, AND NON-INFRINGEMENT. NETIQ,
// THE AUTHORS OF THE WORK, AND THE OWNERS OF COPYRIGHT IN THE WORK ARE NOT LIABLE FOR ANY CLAIM,
// DAMAGES, OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT, OR OTHERWISE, ARISING FROM, OUT OF,
// OR IN CONNECTION WITH THE WORK OR THE USE OR OTHER DEALINGS IN THE WORK.
//

///////////////////////////////////////////////////////////////////////////////////
//
// Make a connection to the WAREHOUSE database and run the DBSync Oracle stored
// procedure. Once complete, print the synchronization report.
//
// Note:    Requires the path to the Formula.properties file from the
//          config directory.
//
// @internal
///////////////////////////////////////////////////////////////////////////////////

var formulaHome = java.lang.System.getProperty('formula.home')

///////////////////////////////////////////////////////////////////////////////////
// Class aliases
///////////////////////////////////////////////////////////////////////////////////
com = Packages.com
java = Packages.java

///////////////////////////////////////////////////////////////////////////////////
// Identification
///////////////////////////////////////////////////////////////////////////////////
writeln( "" )
writeln( "Database Initialization Utility - NetIQ Operations Center(r) Copyright 2014 NetIQ Corporation." )
writeln( "" )

///////////////////////////////////////////////////////////////////////////////////
// Main
///////////////////////////////////////////////////////////////////////////////////
try {
   var schema = com.mosol.util.db.JDBCConstants.WAREHOUSE
   var kind = com.mosol.util.db.JDBCConstants.KIND_SCHEMA_NAME

   if (args.length == 1)
   {
      schema = args[0]
      kind = com.mosol.util.db.JDBCConstants.KIND_DATASOURCE_NAME
   }

   if (formulaHome == null) 
      throw new java.lang.Exception("Missing property 'formula.home'")

   // Initialize connection manager
   com.mosol.util.db.JDBCConnectionManager.init(formulaHome, 0, false)

   // Get data source
   var ds = com.mosol.util.db.JDBCConnectionManager.getDataSource( schema, kind )
   if ( ds == null ) {
      throw new java.lang.Exception("Can't find schema for " + schema )
   }
   writeln( "" )
   writeln( "Initializing " + schema + " database schema ..." )
   ds.initSchemas( formulaHome )
}
catch (Exception) {
   writeln( "" )
   writeln( 'ERROR: ', Exception)
   writeln( "" )
}
com.mosol.util.db.JDBCConnectionManager.shutdown()

// @internal initDB.fs -4kcjagm
