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

// @debug off

// Set our log category.
formula.setLogCategory( 'Impact.Report' )

// Create the impact report object.
impactReport = new formula.commands.ImpactReport();

// The model dname we will report on, is the element we're passed in the right-click.
impactReport.setModelDName( element.dname );

// Report on alarms from Elements tree.
impactReport.setAlarmSourceDName( 'root=Elements' );

// Setup times to report on.  We are going to report on one day.
var now = java.lang.System.currentTimeMillis()
var start = now - ( 3600 * 1000 * 24 * 1 )
impactReport.setStart( start );
impactReport.setEnd( now );

// The alarm rewind time is the amount of seconds prior to "Start" that we will 
// simulate alarm impact.
// impactReport.setAlarmRewindTime( 14 * 24 * 3600 * 1000 ); // Two weeks
// impactReport.setAlarmRewindTime( 7 * 24 * 3600 * 1000 );  // One week
impactReport.setAlarmRewindTime( 0 );

// Should the impact report using configuration history to initialize the model?
impactReport.setUseConfigHistory( true );

// This filter shows how a regular expression can filter the elements that
// are reported on in the impact listener callback.  More elaborate filtering
// can be done on the impact callback if required.
// 
impactReport.setImpactElementClassFilter( '^mgmt_service$' ); // Another example:  '^(mgmt_service|gen_folder_shared)$' 

// Setup impact callback.
//
// This callback is called once for each alarm that impacts the model, and
// once for each impacted element, subject to filtering by class above.
// 
impactReport.setImpactListener( new formula.commands.ImpactReport.ImpactListener() {
   processImpactingAlarm: function( alarm, reason, affectedElement )
   {
      var alarmTime = alarm.last_update * 1000
      formula.log.info( "Recalculation of state due to alarm " + reason + ": " + alarm.persistentId + ":" + new java.util.Date( alarmTime ) +  
               "\n  > primary affected element: " + affectedElement.dname )
   },
   processImpactedElement: function( element, prior, rootCause )
   {
      var dbg = formula.log.isDebugEnabled() ? " with " + element.getConditionAlgorithm() + " and " + element.getConditionParameters() : "";
      formula.log.info( "  > recalculated element " + element.dname + " from " + prior + " to " + element.getCondition() + dbg + " reason " + rootCause.getReason() );
   }
} )

// Setup closed alarm interceptor
//
// This interceptor allows for the impact report to react to alarm changes
// to simulate a "delete" or close of alarm's impact.
// 
impactReport.setClosedAlarmInterceptor( new formula.commands.ImpactReport.ClosedAlarmInterceptor() {
   isClosed: function( alarm )
   {
      return alarm.Status == 'CLOSED'
   }
} )

// Open and run report.
impactReport.open();
impactReport.runReport();

// Cleanup.
impactReport.close();
delete impactReport 
