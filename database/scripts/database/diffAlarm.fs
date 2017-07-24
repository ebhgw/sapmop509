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

formula.logCategory = 'diffAlarm'
var log = formula.log
var textArea = ''

///////////////////////////////////////////////////////////////////////////////////
// Class aliases
///////////////////////////////////////////////////////////////////////////////////
JDBCSession            = Packages.com.mosol.util.db.JDBCSession
JDBCConstants          = Packages.com.mosol.util.db.JDBCConstants
PreparedStatement      = Packages.java.sql.PreparedStatement
ResultSet              = Packages.java.sql.ResultSet
CorbaObjectInputStream = Packages.com.mosol.Formula.Performance.shared.CorbaObjectInputStream
Alarm                  = Packages.com.mosol.ORB.Formula.Alarm
AttrValConverter       = Packages.com.mosol.Formula.Common.AttrValConverter

///////////////////////////////////////////////////////////////////////////////////
// Add strings to buffer
///////////////////////////////////////////////////////////////////////////////////
function printMsg( msg )
{
   textArea.append( msg + '\n' )
   log.info( msg )
}

///////////////////////////////////////////////////////////////////////////////////
// Diff alarms. Only diff alarm fields we care about
///////////////////////////////////////////////////////////////////////////////////
function diffAlarms( oldAlarm, newAlarm )
{
   if ( oldAlarm.condition.value() != newAlarm.condition.value() )
   {
      printMsg( 'old> condition = ' + oldAlarm.condition.value() )
      printMsg( 'new> condition = ' + newAlarm.condition.value() )
   }

   if ( oldAlarm.user_clearable != newAlarm.user_clearable )
   {
      printMsg( 'old> user_clearable = ' + oldAlarm.user_clearable )
      printMsg( 'new> user_clearable = ' + newAlarm.user_clearable )
   }

   if ( oldAlarm.user_assignable != newAlarm.user_assignable )
   {
      printMsg( 'old> user_assignable = ' + oldAlarm.user_assignable )
      printMsg( 'new> user_assignable = ' + newAlarm.user_assignable )
   }

   if ( oldAlarm.native_last_update != newAlarm.native_last_update )
   {
      printMsg( 'old> native_last_update = ' + oldAlarm.native_last_update 
         + ' Time = ' + new java.sql.Timestamp( oldAlarm.native_last_update * 1000 )
      )
      printMsg( 'new> native_last_update = ' + newAlarm.native_last_update
         + ' Time = ' + new java.sql.Timestamp( newAlarm.native_last_update * 1000 )
      )
   }

   if ( ! oldAlarm.persistentId.equals( newAlarm.persistentId ) )
   {
      printMsg( 'old> persistentId = ' + oldAlarm.persistentId )
      printMsg( 'new> persistentId = ' + newAlarm.persistentId )
   }

   for ( var i = 0; i < oldAlarm.additional.length; i++ )
   {
      var oldField = oldAlarm.additional[i]
      var oldValue = AttrValConverter.getValue( oldField.value )

      var newField = null
      var newValue = null

      for ( var j = 0; j < newAlarm.additional.length; j++ ) 
      {
         var nf = newAlarm.additional[j]
         if ( oldField.field.equals( nf.field ) )
         {
            newField = nf
            newValue = AttrValConverter.getValue( newField.value )
            break
         }
      }

      if ( ! oldValue.equals( newValue ) )
      {
         printMsg( 'old> property[ ' + oldField.field + ' ] = ' + oldValue )
         printMsg( 'new> property[ ' + oldField.field + ' ] = ' + newValue )
      }
   }
}

///////////////////////////////////////////////////////////////////////////////////
// Class aliases
///////////////////////////////////////////////////////////////////////////////////
function doDiff( oneAlarm ) 
{
   try {

      if ( ! oneAlarm )
         throw 'Alarm is null'

      var persistentId = oneAlarm['persistentId']
      if ( persistentId == null )
         throw 'Alarm persistentId is null'

      printMsg( '\n' )
      printMsg( '>>>> START persistentID: ' + persistentId )

      var session = new JDBCSession( JDBCConstants.WAREHOUSE )
      if ( session == null ) 
         throw 'Cannot get database session for ' + JDBCConstants.WAREHOUSE

      var conn = session.connect( 0 )
      if ( conn != null )
      {
         var stmt = null
         var rset = null

         try {
            var sql = 
                 ' SELECT AlarmKey'
               + '       , AlarmBlob'
               + ' FROM  BSAAlarmData'
               + ' WHERE PersistentId = ?'
               + ' ORDER BY'
               + '   RecordedDateTime'
            ; // end of sql

            stmt = conn.prepareStatement( sql )
            stmt.setString( 1, persistentId )
            rset = stmt.executeQuery()

            var prevKey   = null
            var prevAlarm = null
            var count = 0

            while ( rset.next() )
            {
               var alarmKey  = rset.getLong( 1 )
               var alarmBlob = rset.getBytes( 2 )

               var currAlarm = CorbaObjectInputStream.melt( alarmBlob )
               if ( prevAlarm != null )
               {
                  var crlf = ''
                  if ( count > 1 )
                     crlf = '\n'

                  printMsg( crlf + 'AlarmKey <old>: ' + prevKey + ' and <new>: ' + alarmKey )
                  diffAlarms( prevAlarm, currAlarm )
               }
               prevKey   = alarmKey
               prevAlarm = currAlarm
               count++
            }
            if ( count < 2 )
               printMsg( 'Diff not possible. ' + count + ' alarms found for persistentId: ' + persistentId )

            printMsg( '<<<< END persistentID: ' + persistentId )
         }
         catch (SQLException) {
            alert( 'Warning: SQL Exception: ' + SQLException )
         }
         finally 
         {
            try { if (rset != null) rset.close(); } catch ( ignore ) {}
            try { if (stmt != null) session.closeStatement(stmt); } catch ( ignore ) {}
            session.disconnect( conn )
         }
      }
   }
   catch ( Exception ) {
      alert( 'Exception: ' + Exception )
   }
   
}

///////////////////////////////////////////////////////////////////////////////////
// Main
///////////////////////////////////////////////////////////////////////////////////
log.info('Starting diff')

if ( this.alarms[0] )
{
   // Window setup
   var frame = new java.awt.Frame( 'Alarm Diff' )
   frame.setSize( 600, 400 )
   formula.util.center( frame )

   frame.addWindowListener(
      new java.awt.event.WindowAdapter() 
      {
         windowClosing: function( evt ) { frame.setVisible( false ) }
      }
   )

   textArea = new java.awt.TextArea( "", 40, 70, java.awt.TextArea.SCROLLBARS_VERTICAL_ONLY )
   textArea.setBackground( java.awt.Color.white )
   textArea.setForeground( java.awt.Color.black )
   textArea.setText( '' )

   frame.add( textArea )
   frame.setVisible( true )

   for ( var i = 0; i < this.alarms.length; i++ )
      doDiff( this.alarms[i] )
}
else
{
   log.error( 'No alarms defined.' )
}



// @internal diffAlarm.fs 2beg3j2
