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
// Class aliases
///////////////////////////////////////////////////////////////////////////////////
Class             = Packages.java.lang.Class;
JDBCSession       = Packages.com.mosol.util.db.JDBCSession;
JDBCConstants     = Packages.com.mosol.util.db.JDBCConstants;
JDBCProperties    = Packages.com.mosol.util.db.JDBCProperties;
JDBCSchemaParser  = Packages.com.mosol.util.db.JDBCSchemaParser;
JDBCConnectionManager = Packages.com.mosol.util.db.JDBCConnectionManager;
DriverManager     = Packages.java.sql.DriverManager;
PreparedStatement = Packages.java.sql.PreparedStatement;
ResultSet         = Packages.java.sql.ResultSet;

///////////////////////////////////////////////////////////////////////////////////
// Main
///////////////////////////////////////////////////////////////////////////////////
try {
   var formulaHome = java.lang.System.getProperty( "formula.home" )
   if ( formulaHome == null ) {
      throw new Exception('Required property formula.home is not set')
   }
   JDBCConnectionManager.init( formulaHome, 10, false )

   var session = new JDBCSession(JDBCConstants.WAREHOUSE);
   if (session == null) {
      throw new Exception('Cannot get database session for ' + JDBCConstants.WAREHOUSE)
   }

   var conn = session.connect(0);
   if (conn != null)
   {
      var tables = 
         [ 'BSATimeBand'
         , 'BSASeries'
         , 'BSAHosts'
         , 'BSAFactSeriesData'
         , 'BSAFactSeriesSummary'
         , 'BSAAdapters'
         , 'BSAElements'
         , 'BSAAlarmData'
         , 'BSAAlarmSeries'
         , 'BSAAlarmElements'
         , 'BSAAlarmValueTypes'
         , 'BSAAlarmMetaData'
         , 'BSAAlarmValues'
         , 'BSAAlarmQueue'
         , 'BSAAlarmProperties'
         , 'BSAFactSeriesRootCauses'
         , 'BSARootCauseTrees'
         , 'BSARootCauseReasons'
         , 'BSARootCauseChains'
         , 'BSARootCauseHierarchy'
         , 'BSADataTypes'
         , 'BSAConfigHistory'
      ];


      try {
         var stmt = conn.createStatement();
         formula.log.info('+++ Counting Rows in Warehouse Database Tables +++');

         for (var i = 0; i < tables.length; i++)
         {
            var rset = stmt.executeQuery( 'SELECT count(*) FROM ' + tables[i]);
            if (rset.next())
               formula.log.info( 'Count for Table ' + tables[i] + ' = ' + rset.getInt(1));
            
         }
         formula.log.info('+++ End of row count +++');
      }
      catch (SQLException) {
         formula.log.error( 'SQL Exception: ' + SQLException)
      }
      finally {
         session.disconnect(conn);
      }
   }
}
catch (Exception) {
   formula.log.error( 'exception: ' + Exception);
}

// @internal getRowCounts.fs -b3jh1df
